module FslexFsyacc.Fsyacc.FsyaccCompiler

let compile (inp:string) =
    inp
    |> FsyaccTokenUtils.tokenize
    |> FsyaccParseTable2.parse
