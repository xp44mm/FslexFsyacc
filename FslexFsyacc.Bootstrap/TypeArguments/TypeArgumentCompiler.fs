module FslexFsyacc.TypeArguments.TypeArgumentCompiler

open FslexFsyacc
open FslexFsyacc.TypeArguments.TypeArgumentParseTable
open FSharp.Idioms
open FSharp.Idioms.Literal

let parser = app.getParser<Position<FSharpToken>> (    
    TypeArgumentUtils.getTag,
    TypeArgumentUtils.getLexeme)

let table = app.getTable parser

let parse (tokens:seq<Position<FSharpToken>>) =
    tokens
    |> parser.parse
    |> TypeArgumentParseTable.unboxRoot

let compile (exit) (i:int) (txt:string) =
    let mutable tokens = []
    let mutable states = [0,null]
    let tokenIterator = Iterator(TypeArgumentUtils.tokenize i txt)

    let rec loop (maybeToken:option<Position<FSharpToken>>) =
        let token = maybeToken.Value
        tokens <- token::tokens
        states <- parser.shift(states,token)
        let offset = token.nextIndex
        let rest = txt.[offset..]

        if exit rest then
            match parser.tryAccept(states) with
            | Some lxm -> 
                (TypeArgumentParseTable.unboxRoot lxm),offset,rest
            | None ->
                tokenIterator.tryNext()
                |> loop
        else
            tokenIterator.tryNext()
            |> loop
        
    tokenIterator.tryNext()
    |> loop

