namespace FslexFsyacc.Yacc

open FSharp.Idioms
open FSharp.Literals.Literal

type BNF =
    {
        productions:list<string list>
    }

    /// append to end
    member this.addProduction(production:string list) =
        {
            productions =
                this.productions @ [production]
        }
    member this.removeProduction(production:string list) =
        let ({productions=prods}:BNF) = this
        {
            productions =
                prods
                |> List.filter((<>)production)
        }
    member this.findProductionIndex (prod:string list) =
        this.productions
        |> List.findIndex((=)prod)

    /// 保持替换的位置
    member this.replaceRule(oldProd:string list,newProd:string list) =
        let sameLhs () =
            oldProd.Head = newProd.Head

        if not(sameLhs ()) then
            failwith $"replaceRule should is same lhs."

        let i =
            this.productions
            |> List.findIndex((=)oldProd)

        let x,y =
            this.productions
            |> List.splitAt i

        [
            yield! x
            newProd
            yield! y.Tail
        ]

    ///检查是否有重复的产生式
    member this.duplicateProductions() =
        this.productions
        |> List.countBy (fun(rule)->rule)
        |> List.filter(fun(r,c)->c>1)

    member this.isRecurSymbol (symbol:string) =
        let productions =
            this.productions
            |> List.filter(fun p -> p.Head=symbol)

        if productions.IsEmpty then
            false
        else
            productions
            |> List.map List.tail
            |> List.exists(fun body -> body |> List.exists((=)symbol))

    member this.eliminate(removed:string) =
        let removedProds,keepedProds =
            this.productions
            |> List.partition(fun prod ->
                prod.Head = removed
            )
        if removedProds.IsEmpty then
            failwith "removed is terminal or no-exists"

        let bodies = removedProds |> List.map List.tail

        {
            productions =
                keepedProds
                |> List.collect(fun prod ->
                    let body = prod.Tail
                    if body |> List.exists(fun s -> s = removed) then
                        ProductionUtils.eliminateSymbol removed bodies body
                        |> List.map(fun body -> prod.Head::body)
                    else [prod]
                )
                //|> List.distinct
        }
