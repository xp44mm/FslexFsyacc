module FslexFsyacc.Fsyacc.FsyaccCompiler

let compile (inp:string) =
    inp
    |> FsyaccTokenUtils.tokenize
    |> Fsyacc1ParseTable.parse
