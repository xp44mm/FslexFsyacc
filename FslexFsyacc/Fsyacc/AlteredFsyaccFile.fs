namespace FslexFsyacc.Fsyacc

open FSharp.Idioms

type AlteredFsyaccFile = 
    {
        mainProductions:string list list
        productionNames:Map<string list,string>
        precedences:Map<string,int>

        header:string
        semantics: Map<string list,string>
        declarations:(string*string)list
    }

    static member fromRaw(fsyacc:FsyaccFile) =
        let mainRules =
            //简写的产生式规范化为完整的产生式
            fsyacc.rules
            |> List.collect(fun (head,bodies)->
                bodies
                |> List.map(fun (symbols,name,semantic)->
                    head::symbols,name,semantic
                )
            )

        let mainProductions =
            mainRules |> List.map Triple.first

        // production -> name
        let productionNames =
            mainRules
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

        let semantics = 
                mainRules
                |> Seq.map(fun(prod,_,sem)->prod,sem)
                |> Map.ofSeq

        {
            mainProductions = mainProductions
            productionNames = productionNames
            precedences = precedences

            header = fsyacc.header
            semantics = semantics
            declarations = fsyacc.declarations
        }

    member this.toFsyaccParseTable() = 
        let parseTable = 
            FslexFsyacc.Yacc.ParseTable.create(
                this.mainProductions,
                this.productionNames,
                this.precedences)

        { 
            productions = parseTable.productions
            closures = parseTable.closures
            actions = parseTable.actions

            header = this.header
            semantics = 
                this.semantics
                |> Map.values
                |> Array.ofSeq

            declarations = Array.ofList this.declarations
        }:FsyaccParseTableModule
