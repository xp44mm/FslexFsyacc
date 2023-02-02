module FSharpAnatomy.PostfixTyparDeclsCompiler

open FslexFsyacc.Runtime
open FSharp.Idioms
open FSharp.Literals.Literal

open System
open System.Reactive
open System.Reactive.Linq

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
    let mutable result = defaultValue<_>

    let sq =
        txt
        |> PostfixTyparDeclsUtils.tokenize 0
        |> ArrayTypeSuffixDFA.analyze
        |> Seq.map(fun tok ->
            tokens <- tok::tokens
            tok
        )
        |> Seq.map(fun tok ->
            match parser.tryReduce(states,tok) with
            | Some x -> states <- x
            | None -> ()
            tok
        )
    use _ =
        sq.Subscribe(Observer.Create(
            (fun lookahead ->
                states <- parser.shift(states,lookahead)
             ),(fun () ->
                match parser.tryReduce(states) with
                | Some x -> states <- x
                | None -> ()

                match states with
                |[1,lxm; 0,null] ->
                    result <- PostfixTyparDeclsParseTable.unboxRoot lxm
                | _ ->
                    failwith $"{stringify states}"
             )
        ))
    result

