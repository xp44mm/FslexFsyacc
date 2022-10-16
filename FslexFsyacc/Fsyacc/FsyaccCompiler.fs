module FslexFsyacc.Fsyacc.FsyaccCompiler

open FslexFsyacc.Runtime
open FslexFsyacc.Fsyacc
open FslexFsyacc.Fsyacc.FsyaccTokenUtils

let parser = Parser<int*int*FsyaccToken>(
    Fsyacc2ParseTable.rules,
    Fsyacc2ParseTable.actions,
    Fsyacc2ParseTable.closures,getTag,getLexeme)

let parse(tokens:seq<int*int*FsyaccToken>) =
    tokens
    |> parser.parse
    |> Fsyacc2ParseTable.unboxRoot

let compile (inp:string) =
    inp
    |> FsyaccTokenUtils.tokenize
    |> parse
