module FslexFsyacc.Fslex.FslexDFA2
let nextStates = Map [0u,Map ["%%",5u;"&",5u;"(",5u;")",3u;"*",3u;"+",3u;"/",5u;"=",5u;"?",3u;"CAP",5u;"HEADER",2u;"HOLE",3u;"ID",3u;"QUOTE",3u;"SEMANTIC",5u;"[",5u;"]",3u;"|",5u];1u,Map ["%%",1u];2u,Map ["%%",1u];3u,Map ["(",4u;"HOLE",4u;"ID",4u;"QUOTE",4u;"[",4u]]
let lexemesFromFinal = Map [4u,set [3u]]
let universalFinals = set [1u;2u;3u;4u;5u]
let indicesFromFinal = Map [1u,0;2u,2;3u,2;4u,1;5u,2]
let header = "open FslexFsyacc.Fslex\r\nopen FslexFsyacc.Fslex.FslexTokenUtils"
let semantics = ["[lexbuf.Head]";"appendAMP lexbuf";"lexbuf"]
open FslexFsyacc.Fslex
open FslexFsyacc.Fslex.FslexTokenUtils
let mappers = [|
    fun (lexbuf:(int*int*_)list) ->
        [lexbuf.Head]
    fun (lexbuf:(int*int*_)list) ->
        appendAMP lexbuf
    fun (lexbuf:(int*int*_)list) ->
        lexbuf
|]
let finalMappers =
    indicesFromFinal
    |> Map.map(fun _ i -> mappers.[i])
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, lexemesFromFinal, universalFinals, finalMappers)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)