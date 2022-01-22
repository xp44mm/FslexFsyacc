namespace FslexFsyacc.Fsyacc

open FSharp.Idioms
open FslexFsyacc.Yacc
type NormFsyaccFile = 
    {
        rules:(string list*string)[]
        productionNames:Map<string list,string>
        precedences:Map<string,int>

        header:string
        declarations:(string*string)list
    }

    static member fromRaw(fsyacc:FsyaccFile) =
        let rules =
            //简写的产生式规范化为完整的产生式
            fsyacc.rules
            |> Seq.collect(fun (head,bodies)->
                bodies
                |> Seq.map(fun (symbols,name,semantic)->
                    head::symbols,name,semantic
                )
            )

        // production -> name
        let productionNames =
            rules
            |> Seq.filter(fun(_,nm,_) -> nm > "")
            |> Seq.map(fun(prod,nm,_) -> prod,nm)
            |> Map.ofSeq

        let precedences =
            fsyacc.precedences
            |> Seq.mapi(fun i (assoc,symbols)->
                let assocoffset =
                    match assoc with
                    | "left" -> -1
                    | "right" -> 1
                    | "nonassoc" -> 0
                    | _ -> failwith assoc

                let prec = (i+1) * 100 // 索引大，则优先级高
                symbols
                |> Seq.map(fun symbol -> symbol, prec + assocoffset)
            )
            |> Seq.concat
            |> Map.ofSeq

        {
            rules = rules |> Seq.map Triple.ends |> Seq.toArray
            productionNames = productionNames
            precedences = precedences

            header = fsyacc.header
            declarations = fsyacc.declarations
        }

    member this.toFsyaccParseTableFile() = 
        let parseTable = 
            ParseTable.create(
                this.rules |> Array.map fst |> Array.toList,
                this.productionNames,
                this.precedences)
        { 
            rules = this.rules
            actions = parseTable.actions
            closures = parseTable.closures
            header = this.header
            declarations = Array.ofList this.declarations
        }: FsyaccParseTableFile
