module FSharpAnatomy.TypeArgumentCompiler

open FslexFsyacc.Runtime
open FSharp.Literals.Literal

//let analyze (posTokens:seq<Position<FSharpToken>>) = 
//    posTokens
//    |> ArrayTypeSuffixDFA.analyze

let parser = Parser<Position<FSharpToken>> (
    TypeArgumentParseTable.rules,
    TypeArgumentParseTable.actions,
    TypeArgumentParseTable.closures,
    
    TypeArgumentUtils.getTag,
    TypeArgumentUtils.getLexeme)

let parse (tokens:seq<Position<FSharpToken>>) =
    tokens
    |> parser.parse
    |> TypeArgumentParseTable.unboxRoot

let compile (txt:string) =
    let mutable tokens = []
    let mutable states = [0,null]

    txt
    |> TypeArgumentUtils.tokenize 0
    //|> ArrayTypeSuffixDFA.analyze
    |> Seq.map(fun tok ->
        tokens <- tok::tokens
        tok
    )
    |> Seq.iter(fun tok ->
        //match parser.tryReduce(states,tok) with
        //| Some x -> states <- x
        //| None -> ()

        states <- parser.shift(states,tok)
    )
    //match parser.tryReduce(states) with
    //| Some x -> states <- x
    //| None -> ()

    match parser.accept states with
    | [1,lxm; 0,null] ->
        TypeArgumentParseTable.unboxRoot lxm
    | _ ->
        failwith $"{stringify states}"

