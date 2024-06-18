module FslexFsyacc.ModuleOrNamespaces.ModuleDeclsCompiler
open FslexFsyacc.ModuleOrNamespaces.ModuleDeclsParseTable

open FslexFsyacc
open FSharp.Idioms
open FSharp.Idioms.Literal
open System
open System.Text.RegularExpressions

let parser = app.getParser<Position<ModuleOrNamespaceToken>> (    
    ModuleOrNamespaceTokenUtils.getTag,
    ModuleOrNamespaceTokenUtils.getLexeme)

let table = app.getTable parser

let parse (tokens:seq<Position<ModuleOrNamespaceToken>>) =
    tokens
    |> parser.parse
    |> unboxRoot

let compile (exit:string->bool) (i:int) (txt:string) =
    let exit rest = 
        exit rest || 
        Regex.IsMatch(rest,@"^\s*$")

    let mutable tokens = []
    let mutable states = [0,null]
    let tokenIterator = 
        ModuleOrNamespaceTokenUtils.tokenize exit i txt
        |> Iterator

    let rec loop (maybeToken:option<Position<ModuleOrNamespaceToken>>) =
        let token = maybeToken.Value
        //Console.WriteLine($"ta:{stringify token}")
        tokens <- token::tokens
        states <- parser.shift(states,token)
        let offset = token.nextIndex
        let rest = txt.[offset-i..]

        if exit rest then
            match parser.tryAccept(states) with
            | Some lxm -> 
                (unboxRoot lxm),offset,rest
            | None ->
                tokenIterator.tryNext()
                |> loop
        else
            tokenIterator.tryNext()
            |> loop
        
    tokenIterator.tryNext()
    |> loop

