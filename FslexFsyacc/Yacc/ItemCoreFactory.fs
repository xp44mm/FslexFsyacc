[<RequireQualifiedAccess>]
module FslexFsyacc.Yacc.ItemCoreFactory

let make (productions:Set<string list>) =
    productions
    |> Seq.collect(fun prod ->
        let body = prod.Tail
        [0..body.Length]
        |> Seq.map(fun i -> {production = prod; dot = i})
    )
    |> Set.ofSeq