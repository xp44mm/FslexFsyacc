module FSharpCompilerServiceTest.Tests

open System
open Xunit

//#r "nuget: FSharp.Compiler.Service, 41.0.1"

open FSharp.Compiler.Syntax
open FSharp.Compiler.Text
open FSharp.Compiler.CodeAnalysis

let tryParseExpression (input : string) =
  async {
    let checker = FSharpChecker.Create()

    let filename = "fs.fsx"

    let sourceText = SourceText.ofString input

    let opts =
      {
        FSharpParsingOptions.Default with
          SourceFiles = [| filename |]
      }

    let! parseResults = checker.ParseFile(filename, sourceText, opts)

    if parseResults.ParseHadErrors then
      return Error parseResults.Diagnostics
    else
      match parseResults.ParseTree with
      | ParsedInput.ImplFile (ParsedImplFileInput (fn, true, _, _, _, [ m ], _,_)) when fn = filename ->
        return Ok m
      | _ ->
        return failwithf "Unexpected parse tree: %A" parseResults.ParseTree
  }

module Result =

  let get =
    function
    | Ok x -> x
    | Error e -> failwithf "%A" e

"let x = 1 + 1"
|> tryParseExpression
|> Async.RunSynchronously
|> Result.get
|> printfn "%A"

(*

SynModuleOrNamespace
  ([Snippet], false, AnonModule,
   [Let
      (false,
       [SynBinding
          (None, Normal, false, false, [],
           PreXmlDoc ((1,4), FSharp.Compiler.Xml.XmlDocCollector),
           SynValData
             (None, SynValInfo ([], SynArgInfo ([], false, None)), None),
           Named (x, false, None, snippet.fsx (1,4--1,5)), None,
           App
             (NonAtomic, false,
              App
                (NonAtomic, true, Ident op_Addition,
                 Const (Int32 1, snippet.fsx (1,8--1,9)),
                 snippet.fsx (1,8--1,11)),
              Const (Int32 1, snippet.fsx (1,12--1,13)), snippet.fsx (1,8--1,13)),
           snippet.fsx (1,4--1,5), Yes snippet.fsx (1,0--1,13))],
       snippet.fsx (1,0--1,13));
    Let
      (false,
       [SynBinding
          (None, Normal, false, false, [],
           PreXmlDoc ((2,4), FSharp.Compiler.Xml.XmlDocCollector),
           SynValData
             (None, SynValInfo ([], SynArgInfo ([], false, None)), None),
           Named (y, false, None, snippet.fsx (2,4--2,5)), None,
           Const (Int32 2, snippet.fsx (2,8--2,9)), snippet.fsx (2,4--2,5),
           Yes snippet.fsx (2,0--2,9))], snippet.fsx (2,0--2,9))],
   PreXmlDocEmpty, [], None, snippet.fsx (1,0--2,9))

*)