module FslexFsyacc.Fsyacc.FsyaccDFA
let nextStates = Map [0u,Map ["\n",9u;"%%",6u;"%left",14u;"%nonassoc",14u;"%prec",14u;"%right",14u;":",14u;";",4u;"BOF",1u;"EOF",8u;"HEADER",10u;"IDENTIFIER",14u;"QUOTE",14u;"SEMANTIC",11u;"|",14u];1u,Map ["%%",1u;";",1u];2u,Map ["%%",2u;";",2u;"EOF",8u];3u,Map ["%%",7u;";",3u;"EOF",8u];4u,Map ["%%",7u;";",3u;"EOF",8u];5u,Map ["\n",9u;"%%",5u;";",2u;"EOF",8u;"HEADER",10u];6u,Map ["\n",9u;"%%",5u;";",2u;"EOF",8u;"HEADER",10u];7u,Map ["%%",2u;";",2u;"EOF",8u];9u,Map ["\n",9u;"%%",9u;"HEADER",10u];10u,Map ["\n",10u;"%%",10u];11u,Map ["IDENTIFIER",12u];12u,Map [":",13u]]
let lexemesFromFinal = Map.empty
let universalFinals = set [1u;3u;4u;6u;7u;8u;10u;11u;13u;14u]
let indicesFromFinal = Map [1u,0;3u,4;4u,6;6u,6;7u,3;8u,1;10u,2;11u,6;13u,5;14u,6]
let header = "open FslexFsyacc.Fsyacc.FsyaccToken"
let semantics = ["[]";"[]";"lexbuf |> List.filter(function HEADER _ -> true | _ -> false)";"[PERCENT]";"[SEMICOLON]";"lexbuf.Head :: SEMICOLON :: lexbuf.Tail";"lexbuf"]
open FslexFsyacc.Fsyacc.FsyaccToken
let mappers = [|
    fun (lexbuf:_ list) ->
        []
    fun (lexbuf:_ list) ->
        []
    fun (lexbuf:_ list) ->
        lexbuf |> List.filter(function HEADER _ -> true | _ -> false)
    fun (lexbuf:_ list) ->
        [PERCENT]
    fun (lexbuf:_ list) ->
        [SEMICOLON]
    fun (lexbuf:_ list) ->
        lexbuf.Head :: SEMICOLON :: lexbuf.Tail
    fun (lexbuf:_ list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = LexicalAnalyzer(nextStates, lexemesFromFinal, universalFinals, indicesFromFinal, mappers)
let split (tokens:seq<_>) = 
    analyzer.split(tokens,getTag)