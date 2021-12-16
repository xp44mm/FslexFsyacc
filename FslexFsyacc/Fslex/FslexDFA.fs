module FslexFsyacc.Fslex.FslexDFA
let nextStates = Map [0u,Map ["\n",7u;"%%",4u;"&",11u;"(",11u;")",9u;"*",9u;"+",9u;"/",11u;"=",11u;"?",9u;"EOF",8u;"HEADER",2u;"HOLE",9u;"ID",9u;"QUOTE",9u;"SEMANTIC",11u;"[",11u;"]",9u;"|",11u];1u,Map ["\n",1u];2u,Map ["\n",1u];3u,Map ["\n",3u;"%%",5u;"EOF",8u];4u,Map ["\n",3u;"%%",5u;"EOF",8u];5u,Map ["\n",5u;"%%",5u;"EOF",8u];6u,Map ["\n",6u;"%%",5u;"EOF",8u];7u,Map ["\n",6u;"%%",5u;"EOF",8u];9u,Map ["(",10u;"HOLE",10u;"ID",10u;"QUOTE",10u;"[",10u]]
let lexemesFromFinal = Map [10u,set [9u]]
let universalFinals = set [1u;2u;3u;4u;6u;7u;8u;9u;10u;11u]
let indicesFromFinal = Map [1u,0;2u,5;3u,1;4u,5;6u,3;7u,5;8u,2;9u,5;10u,4;11u,5]
let header = "open FslexFsyacc.Fslex\r\nopen FslexFsyacc.Fslex.FslexTokenUtils"
let semantics = ["lexbuf |> List.take 1";"lexbuf |> List.take 1";"[]";"lexbuf |> List.take 1";"appendAMP lexbuf";"lexbuf"]
open FslexFsyacc.Fslex
open FslexFsyacc.Fslex.FslexTokenUtils
let mappers = [|
    fun (lexbuf:(int*int*_)list) ->
        lexbuf |> List.take 1
    fun (lexbuf:(int*int*_)list) ->
        lexbuf |> List.take 1
    fun (lexbuf:(int*int*_)list) ->
        []
    fun (lexbuf:(int*int*_)list) ->
        lexbuf |> List.take 1
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