namespace FslexFsyacc.Fsyacc

type FsyaccFile = 
    {
        header:string
        rules:(string*((string list*string*string)list))list
        precedences:(string*string list)list
        declarations:(string*string)list
    }

    member this.render() =
        FsyaccFileRender.renderFsyacc
            this.header
            this.rules
            this.precedences
            this.declarations

    member this.start(startSymbol:string, terminals:Set<string>) =
        let o = 
            this.rules
            |> List.filter(fun (lhs,_) -> 
                lhs 
                |> terminals.Contains
                |> not)
            |> FsyaccFileStart.extractRules <| startSymbol
        let rules = o.rules
        let symbols = o.symbols

        let precedences =
            this.precedences
            |> List.map(fun(assoc,ls)->
                assoc,ls|>List.filter(symbols.Contains)
            )
            |> List.filter(fun (_,ls)-> not ls.IsEmpty)
        let declarations =
            this.declarations
            |> List.filter(fst>>symbols.Contains)

        {
            this with
                rules = rules
                precedences = precedences
                declarations = declarations
        }

    member this.nameRules(productionNames:Map<string list,string>) =
        productionNames
        |> FsyaccFileName.productionToHeadBody
        |> FsyaccFileName.nameRules this.rules

    member this.refineRules(
        oldProd:string list,
        newProd:string list) =

        this.rules
        |> FsyaccFileRefine.refineRules
            oldProd
            newProd

    static member parse(sourceText:string) =
        let header,rules,precedences,declarations = 
            FsyaccCompiler.compile sourceText
        {
            header = header
            rules = rules
            precedences = precedences
            declarations = declarations

        }

