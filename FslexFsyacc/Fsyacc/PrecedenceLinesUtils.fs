module FslexFsyacc.Fsyacc.PrecedenceLinesUtils

let toMap (precedenceLines:list<string* list<string>>) =
        precedenceLines
        |> List.mapi(fun i (assoc,symbols) ->
            let assocoffset =
                match assoc with
                | "left" -> -1
                | "right" -> 1
                | "nonassoc" -> 0
                | _ -> failwith assoc

            let prec = (i+1) * 100 // 索引大，则优先级高

            symbols
            |> List.map(fun symbol -> symbol, prec + assocoffset)
        )
        |> List.concat
        |> Map.ofList

let ofMap (precedences:Map<string,int>) =
    precedences
    |> Map.toList
    |> List.sortBy snd
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
            | _ -> failwith $"precedence nonassoc(0)/right(1)/left(9) != {i}"

        assoc,tokens
    )

