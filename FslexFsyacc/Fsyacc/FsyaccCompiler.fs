module FslexFsyacc.Fsyacc.FsyaccCompiler

let compile (inp:string) =
    inp
    |> FsyaccToken.tokenize
    //|> FsyaccDFA.analyze
    //|> Seq.concat
    |> FsyaccParseTable.parse
