module FslexFsyacc.Fsyacc.FsyaccDFA
let nextStates = Map [0u,Map ["%%",4u;"%left",6u;"%nonassoc",6u;"%prec",6u;"%right",6u;":",6u;";",2u;"EOF",5u;"HEADER",6u;"IDENTIFIER",6u;"QUOTE",6u;"SEMANTIC",6u;"|",6u];1u,Map ["%%",3u;";",1u;"EOF",5u];2u,Map ["%%",3u;";",1u;"EOF",5u];3u,Map ["%%",3u;";",3u;"EOF",5u];4u,Map ["%%",3u;";",3u;"EOF",5u]]
let lexemesFromFinal = Map.empty
let universalFinals = set [1u;2u;4u;5u;6u]
let indicesFromFinal = Map [1u,0;2u,2;4u,2;5u,1;6u,2]
let header = "open FslexFsyacc.Fsyacc.FsyaccToken"
let semantics = ["lexbuf |> List.take 1";"[]";"lexbuf"]
open FslexFsyacc.Fsyacc.FsyaccToken
let mappers = [|
    fun (lexbuf:(int*_) list) ->
        lexbuf |> List.take 1
    fun (lexbuf:(int*_) list) ->
        []
    fun (lexbuf:(int*_) list) ->
        lexbuf
|]
let finalMappers =
    indicesFromFinal
    |> Map.map(fun _ i -> mappers.[i])
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, lexemesFromFinal, universalFinals, finalMappers)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)