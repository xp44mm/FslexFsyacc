namespace FslexFsyacc.Fsyacc

open FSharp.Idioms
open FslexFsyacc.Yacc
open System

[<Obsolete(nameof FlatFsyaccFile)>]
type NormFsyaccFile = 
    {
        rules:list<string list*string>
        productionNames:Map<string list,string> // to rename persudo token
        precedences:Map<string,int>
        header:string
        declarations:(string*string)list
    }

    static member fromRaw(fsyacc:RawFsyaccFile) =
        let rules =
            fsyacc.rules
            |> FsyaccFileRules.rawToFlatRules

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
            rules = rules |> List.map Triple.ends
            productionNames = productionNames
            precedences = precedences
            header = fsyacc.header
            declarations = fsyacc.declarations
        }

    member this.getMainProductions() =
        this.rules |> List.map fst
            
    member this.toFsyaccParseTableFile() = 
        let parseTable = 
            ParseTable.create(
                this.getMainProductions(),
                this.productionNames,
                this.precedences)
        { 
            rules = this.rules
            actions = parseTable.actions
            closures = parseTable.closures
            header = this.header
            declarations = this.declarations
        }
