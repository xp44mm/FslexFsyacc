module FslexFsyacc.Fsyacc.FsyaccCompiler

let compile (inp:string) =
    inp
    |> FsyaccToken.tokenize
    |> FsyaccDFA.split
    |> Seq.concat
    |> FsyaccParseTable.parse
