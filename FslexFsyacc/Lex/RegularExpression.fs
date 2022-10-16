namespace FslexFsyacc.Lex

///3.3 正则表达式
type RegularExpression<'c> =
    /// a: atomic
    | Atomic of 'c
    /// a|b :either
    | Either of RegularExpression<'c> * RegularExpression<'c>
    /// ab :both
    | Both of RegularExpression<'c> * RegularExpression<'c>
    /// a*
    | Natural of RegularExpression<'c>
    /// a+, plural
    | Plural of RegularExpression<'c>
    /// a? :optional
    | Optional of RegularExpression<'c>
    /// <id> 
    | Hole of id:string

    member this.getCharacters() =
        [
            match this with
            | Atomic c -> yield c
            | Either(x,y) 
            | Both(x,y)
                -> yield! x.getCharacters(); yield! y.getCharacters()
            | Natural x
            | Plural x
            | Optional x
                -> yield! x.getCharacters()
            | _ -> ()
        ]
