namespace FslexFsyacc.Lex

///3.3 正则表达式
type RegularExpression<'c> =
    /// a
    | Character of 'c
    /// a|b      
    | Uion of RegularExpression<'c> * RegularExpression<'c>
    /// ab       
    | Concat of RegularExpression<'c> * RegularExpression<'c>
    /// a*       
    | Natural of RegularExpression<'c>
    /// a+
    | Positive of RegularExpression<'c>
    /// a?       
    | Maybe of RegularExpression<'c>
    /// {id}     
    | Hole of id:string

    member this.getCharacters() =
        [|
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
        |]