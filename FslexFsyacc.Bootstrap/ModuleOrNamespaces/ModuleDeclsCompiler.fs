module FslexFsyacc.ModuleOrNamespaces.ModuleDeclsCompiler
open FslexFsyacc.ModuleOrNamespaces.ModuleDeclsParseTable

open FslexFsyacc
open FSharp.Idioms
open FSharp.Idioms.Literal
open System
open System.Text.RegularExpressions

let parser = app.getParser<PositionWith<ModuleOrNamespaceToken>> (    
    ModuleOrNamespaceTokenUtils.getTag,
    ModuleOrNamespaceTokenUtils.getLexeme)

let table = app.getTable parser

let parse (tokens:seq<PositionWith<ModuleOrNamespaceToken>>) =
    tokens
    |> parser.parse
    |> unboxRoot

let compile (exit:string->bool) (sourceText:SourceText) =
    //(i:int) (txt:string)
    //let src = SourceText.just(i,txt)
    let exit rest = 
        exit rest || 
        Regex.IsMatch(rest,@"^\s*$")

    let mutable tokens = []
    let mutable states = [0,null]
    let tokenIterator = 
        ModuleOrNamespaceTokenUtils.tokenize exit sourceText
        |> Iterator

    let rec loop (src:SourceText) (maybeToken:option<PositionWith<ModuleOrNamespaceToken>>) =
        let token = maybeToken.Value
        let src = src.jump(token.index+token.length)
        //let offset = token.nextIndex
        //let rest = txt.[offset-i..]

        //Console.WriteLine($"ta:{stringify token}")
        tokens <- token::tokens
        states <- parser.shift(states,token)

        if exit src.text then
            match parser.tryAccept(states) with
            | Some lxm -> 
                (unboxRoot lxm),src
            | None ->
                tokenIterator.tryNext()
                |> loop src
        else
            tokenIterator.tryNext()
            |> loop src
        
    tokenIterator.tryNext()
    |> loop sourceText

