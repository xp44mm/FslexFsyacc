module FslexFsyacc.TypeArguments.TypeArgumentCompiler

open FslexFsyacc
open FslexFsyacc.TypeArguments.TypeArgumentParseTable
open FSharp.Idioms
open FSharp.Idioms.Literal
open System
open System.Text.RegularExpressions

let parser = app.getParser<PositionWith<TypeArgumentToken>> (    
    TypeArgumentTokenUtils.getTag,
    TypeArgumentTokenUtils.getLexeme)

let table = app.getTable parser

let parse (tokens:seq<PositionWith<TypeArgumentToken>>) =
    tokens
    |> parser.parse
    |> TypeArgumentParseTable.unboxRoot

let compile (exit:string->bool) (sourceText:SourceText) =
    let mutable tokens = []
    let mutable states = [0,null]
    let tokenIterator = Iterator(TypeArgumentTokenUtils.tokenize sourceText)

    let exit rest = exit rest || Regex.IsMatch(rest,"^\s*$")

    let rec loop (fromSrc:SourceText) (maybeToken:option<PositionWith<TypeArgumentToken>>) =
        let token = maybeToken.Value
        let nextSrc = fromSrc.jump (token.index + token.length)

        Console.WriteLine($"token:{stringify token}")
        Console.WriteLine($"source text:{stringify nextSrc}")

        tokens <- token::tokens
        states <- parser.shift(states,token)

        //let offset = token.index + token.length
        //let rest = txt.[offset-i..]

        if exit nextSrc.text then
            match parser.tryAccept(states) with
            | Some lxm -> 
                (TypeArgumentParseTable.unboxRoot lxm), nextSrc
            | None ->
                tokenIterator.tryNext()
                |> loop nextSrc
        else
            tokenIterator.tryNext()
            |> loop nextSrc

    let targ, src =
        tokenIterator.tryNext()
        |> loop sourceText

    let length = src.index - sourceText.index
    targ, length
