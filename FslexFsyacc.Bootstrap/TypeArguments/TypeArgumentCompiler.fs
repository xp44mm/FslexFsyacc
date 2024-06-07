module FslexFsyacc.TypeArguments.TypeArgumentCompiler

open FslexFsyacc
open FslexFsyacc.TypeArguments.TypeArgumentParseTable
open FSharp.Idioms
open FSharp.Idioms.Literal
open System

let parser = app.getParser<Position<TypeArgumentToken>> (    
    TypeArgumentTokenUtils.getTag,
    TypeArgumentTokenUtils.getLexeme)

let table = app.getTable parser

let parse (tokens:seq<Position<TypeArgumentToken>>) =
    tokens
    |> parser.parse
    |> TypeArgumentParseTable.unboxRoot

let compile (exit:string->bool) (i:int) (txt:string) =
    let mutable tokens = []
    let mutable states = [0,null]
    let tokenIterator = Iterator(TypeArgumentTokenUtils.tokenize i txt)

    let rec loop (maybeToken:option<Position<TypeArgumentToken>>) =
        let token = maybeToken.Value
        //Console.WriteLine($"ta:{stringify token}")
        tokens <- token::tokens
        states <- parser.shift(states,token)
        let offset = token.nextIndex
        let rest = txt.[offset-i..]
        //Console.WriteLine($"{stringify rest}")
        //Console.WriteLine($"{exit rest}")

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

