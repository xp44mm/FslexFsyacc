namespace FslexFsyacc.Yacc

open FSharp.Idioms

type Associativity = 
    | LeftAssoc 
    | RightAssoc 
    | NonAssoc

    member this.offset =
        match this with
        | LeftAssoc   -> -1
        | RightAssoc  ->  1
        | NonAssoc    ->  0

    /// 将二维的输入整理成单列的优先级
    static member from (declarations: (Associativity*'op list) list) =
        declarations
        |> List.mapi(fun i (assoc,ops) ->
            let prec = (i+1) * 100 // 索引大，则优先级高
            ops
            |> List.map(fun op -> op, prec + assoc.offset)
        )
        |> List.concat
        |> List.toMap
        |> Map.map(fun _ ls -> List.exactlyOne ls)