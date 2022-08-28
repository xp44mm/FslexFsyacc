module FslexFsyacc.Fsyacc.FsyaccFilePrecedences

let normToRawPrecedences
    (precedences:Map<string,int>)
    =
    precedences
    |> Map.toList
    |> List.groupBy snd
    |> List.map(fun(i,groups)->
        let tokens =
            groups
            |> List.map fst

        let assoc = 
            match i % 10 with
            | 0 -> "nonassoc"
            | 1 -> "right"
            | 9 -> "left"
            | _ -> failwith $"precedence nonassoc(0)/right(1)/left(9): {i}"

        assoc,tokens
    )

