namespace FslexFsyacc.LALRs

open FslexFsyacc.Grammars
open FslexFsyacc.ItemCores
open FSharp.Idioms

type GOTO = 
    {
        sourceKernel: Set<ItemCore>
        symbol: string
        targetKernel: Set<ItemCore>
    }

    static member just(sourceKernel,symbol,targetKernel) = 
        {
            sourceKernel = sourceKernel
            symbol = symbol
            targetKernel = targetKernel
        }

    // 需要记住路径 src -> string -> tgt
    static member from (closure: SpreadClosure) =
        let src = closure.getKernel()
        
        closure.items
        |> Set.filter(fun(la,item) -> not item.dotmax)
        |> Seq.groupBy(fun (la,_) -> la)
        |> Seq.map(fun (la,pairs) ->
            let kernel =
                pairs
                |> Seq.map(fun (la,itemcore) ->
                    { itemcore with dot = itemcore.dot + 1 }
                )
                |> Set.ofSeq
            la, kernel
        )
        |> Seq.map(fun (la,tgt) -> GOTO.just(src,la,tgt) )
