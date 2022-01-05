namespace FslexFsyacc.Yacc
open FSharp.Idioms

type ItemCoreLookaheads =
    {
        itemCore:ItemCore
        lookaheads:string Set
    }

    static member from(closure:Set<ItemCore*Set<string>>) =
        closure
        |> Set.map(fun(itemCore,lookaheads)->
            {
                itemCore = itemCore
                lookaheads = 
                    if itemCore.gone then
                        lookaheads
                    else
                        //GOTO的nextSymbol分为两种情况：
                        //当nextSymbol是terminal时用于侦察冲突
                        //当nextSymbol是nonterminal时仅用于占位
                        Set.singleton itemCore.nextSymbol
            }
        )

