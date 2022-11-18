module FSharp.Compiler.SyntaxTreeX.Parser

open FSharp.Compiler.Text
open FSharp.Compiler.CodeAnalysis
open FSharp.Compiler.Syntax
open FSharp.Literals.Literal
open FSharp.Compiler.SyntaxTreeX.Readers

let getResults (filename, input: string) =
    let checker = FSharpChecker.Create()
    let sourceText = SourceText.ofString input
    let opts = {
        FSharpParsingOptions.Default with
            SourceFiles = [| filename |]
        }
    let parseResults: FSharpParseFileResults = 
        checker.ParseFile(filename, sourceText, opts)
        |> Async.RunSynchronously
    parseResults

let getModuleOrNamespace (parseResults:FSharpParseFileResults) =
    if parseResults.ParseHadErrors then
        failwith $"{parseResults.Diagnostics}" 
    else
        match parseResults.ParseTree with
        | ParsedInput.ImplFile(ParsedImplFileInput(
            fn, _, _, _, _, [ m ], _,_
            )) -> 
            Transformation.getModuleOrNamespace m
        | _ -> 
            failwith $"Unexpected parse tree: {parseResults.ParseTree}"

let getDecls(file, source) =
    let parseResult = getResults(file, source)

    let x = 
        parseResult
        |> getModuleOrNamespace 
        |> ModuleOrNamespace.getDecls
    x

let rec getElements(expr:XExpr) =
    seq {
        match expr with
        | XExpr.Sequential(_,e0,e1) ->
            yield e0
            yield! getElements e1
        | _ -> yield expr
    }