module FslexFsyacc.Fsyacc.FsyaccDFA
let nextStates = Map [0u,Map ["%%",3u;"%left",11u;"%nonassoc",11u;"%prec",11u;"%right",11u;":",11u;";",5u;"BOF",1u;"EOF",6u;"HEADER",7u;"IDENTIFIER",11u;"QUOTE",11u;"SEMANTIC",8u;"|",11u];1u,Map ["%%",1u;";",1u];2u,Map ["%%",2u;";",2u;"EOF",6u];3u,Map ["%%",2u;";",3u;"EOF",6u];4u,Map ["%%",3u;";",4u;"EOF",6u];5u,Map ["%%",3u;";",4u;"EOF",6u];7u,Map ["%%",7u;";",7u];8u,Map ["IDENTIFIER",9u];9u,Map [":",10u]]
let lexemesFromFinal = Map.empty
let universalFinals = set [1u;3u;4u;5u;6u;7u;8u;10u;11u]
let indicesFromFinal = Map [1u,0;3u,3;4u,4;5u,6;6u,1;7u,2;8u,6;10u,5;11u,6]
let header = "open FslexFsyacc.Fsyacc.FsyaccToken"
let semantics = ["[]";"[]";"[lexbuf.[0]]";"[PERCENT]";"[SEMICOLON]";"lexbuf.Head :: SEMICOLON :: lexbuf.Tail";"lexbuf"]
open FslexFsyacc.Fsyacc.FsyaccToken
let mappers = [|
    fun (lexbuf:_ list) ->
        []
    fun (lexbuf:_ list) ->
        []
    fun (lexbuf:_ list) ->
        [lexbuf.[0]]
    fun (lexbuf:_ list) ->
        [PERCENT]
    fun (lexbuf:_ list) ->
        [SEMICOLON]
    fun (lexbuf:_ list) ->
        lexbuf.Head :: SEMICOLON :: lexbuf.Tail
    fun (lexbuf:_ list) ->
        lexbuf
|]
let finalMappers =
    indicesFromFinal
    |> Map.map(fun _ i -> mappers.[i])
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, lexemesFromFinal, universalFinals, finalMappers)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)