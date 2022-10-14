namespace FslexFsyacc.Lex

///3.3 正则表达式
type RegularExpression<'c> =
    /// a: atomic
    | Character of 'c
    /// a|b :either
    | Uion of RegularExpression<'c> * RegularExpression<'c>
    /// ab :both
    | Concat of RegularExpression<'c> * RegularExpression<'c>
    /// a*
    | Natural of RegularExpression<'c>
    /// a+, plural
    | Positive of RegularExpression<'c>
    /// a? :optional
    | Maybe of RegularExpression<'c>
    /// <id> 
    | Hole of id:string

    member this.getCharacters() =
        [
            match this with
            | Character c -> yield c
            | Uion(x,y) 
            | Concat(x,y)
                -> yield! x.getCharacters(); yield! y.getCharacters()
            | Natural x
            | Positive x
            | Maybe x
                -> yield! x.getCharacters()
            | _ -> ()
        ]
