module FSharpAnatomy.PostfixTyparDeclsCompiler

open FslexFsyacc.Runtime
open FSharp.Idioms
open FSharp.Literals.Literal

open System

let analyze (posTokens:seq<Position<FSharpToken>>) = 
    posTokens
    |> ArrayTypeSuffixDFA.analyze

let parser = Parser<Position<FSharpToken>> (
    PostfixTyparDeclsParseTable.rules,
    PostfixTyparDeclsParseTable.actions,
    PostfixTyparDeclsParseTable.closures,
    
    PostfixTyparDeclsUtils.getTag,
    PostfixTyparDeclsUtils.getLexeme)

let parse(tokens:seq<Position<FSharpToken>>) =
    tokens
    |> parser.parse
    |> PostfixTyparDeclsParseTable.unboxRoot

let compile (txt:string) =
    let mutable tokens = []
    let mutable states = [0,null]
    //let mutable result = defaultValue<_>

    txt
    |> PostfixTyparDeclsUtils.tokenize 0
    |> ArrayTypeSuffixDFA.analyze
    |> Seq.map(fun tok ->
        tokens <- tok::tokens
        tok
    )
    |> Seq.iter(fun tok ->
        match parser.tryReduce(states,tok) with
        | Some x -> states <- x
        | None -> ()
            
        states <- parser.shift(states,tok)
    )
    match parser.tryReduce(states) with
    | Some x -> states <- x
    | None -> ()

    match states with
    |[1,lxm; 0,null] ->
        PostfixTyparDeclsParseTable.unboxRoot lxm
    | _ ->
        failwith $"{stringify states}"

