module FslexFsyacc.Fslex.FslexDFA2
let nextStates = Map [0u,Map ["\n",5u;"%%",6u;"&",11u;"(",11u;")",9u;"*",9u;"+",9u;"/",11u;"=",11u;"?",9u;"BOF",1u;"EOF",7u;"HEADER",8u;"HOLE",9u;"ID",9u;"QUOTE",9u;"SEMANTIC",11u;"[",11u;"]",9u;"|",11u];1u,Map ["\n",1u;"%%",1u];2u,Map ["\n",2u;"%%",2u;"EOF",7u;"HEADER",8u];3u,Map ["\n",3u;"%%",2u;"EOF",7u;"HEADER",8u];4u,Map ["\n",4u;"%%",2u;"EOF",7u;"HEADER",8u];5u,Map ["\n",4u;"%%",2u;"EOF",7u;"HEADER",8u];6u,Map ["\n",3u;"%%",2u;"EOF",7u;"HEADER",8u];8u,Map ["\n",8u;"%%",8u];9u,Map ["(",10u;"HOLE",10u;"ID",10u;"QUOTE",10u;"[",10u]]
let lexemesFromFinal = Map [10u,set [9u]]
let universalFinals = set [1u;3u;4u;5u;6u;7u;8u;9u;10u;11u]
let indicesFromFinal = Map [1u,0;3u,3;4u,4;5u,6;6u,6;7u,1;8u,2;9u,6;10u,5;11u,6]
let header = "open FslexFsyacc.Fslex.FslexToken"
let semantics = ["[]";"[]";"lexbuf |> List.filter(function HEADER _ -> true | _ -> false)";"[lexbuf.[0]]";"[lexbuf.[0]]";"[lexbuf.[0]; AMP]";"lexbuf"]
open FslexFsyacc.Fslex.FslexToken
let finalMappers = Map [
    1u, fun (lexbuf:_ list) ->
        []
    3u, fun (lexbuf:_ list) ->
        [lexbuf.[0]]
    4u, fun (lexbuf:_ list) ->
        [lexbuf.[0]]
    5u, fun (lexbuf:_ list) ->
        lexbuf
    6u, fun (lexbuf:_ list) ->
        lexbuf
    7u, fun (lexbuf:_ list) ->
        []
    8u, fun (lexbuf:_ list) ->
        lexbuf |> List.filter(function HEADER _ -> true | _ -> false)
    9u, fun (lexbuf:_ list) ->
        lexbuf
    10u, fun (lexbuf:_ list) ->
        [lexbuf.[0]; AMP]
    11u, fun (lexbuf:_ list) ->
        lexbuf
]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, lexemesFromFinal, universalFinals, finalMappers)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)