module FslexFsyacc.Fsyacc.FsyaccCompiler

let compile (inp:string) =
    inp
    |> FsyaccTokenUtils.tokenize
    |> Fsyacc2ParseTable.parse
