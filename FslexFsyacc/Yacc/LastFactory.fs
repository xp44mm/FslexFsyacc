[<RequireQualifiedAccess>]
module FslexFsyacc.Yacc.LastFactory

open FSharp.Idioms

/// 单个符号的last终结符集合，包括所有符号的查询表，包括终结符号和非终结符号
let make 
    (nullables:Set<string>) 
    (mainProductions:#seq<string list>) =
    mainProductions
    |> Seq.map(fun p -> p.Head :: List.rev p.Tail)
    |> FirstFactory.make nullables