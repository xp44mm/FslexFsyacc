module FslexFsyacc.Lex.RegularExpressionUtils

let rec getCharacters(this:RegularExpression<'c>) =
    [
        match this with
        | Atomic c -> yield c
        | Natural x
        | Plural x
        | Optional x
            -> 
            yield! x |> getCharacters 
        | Either(x,y) 
        | Both(x,y)
            -> 
            yield! x |> getCharacters 
            yield! y |> getCharacters
        | _ -> ()
    ]
