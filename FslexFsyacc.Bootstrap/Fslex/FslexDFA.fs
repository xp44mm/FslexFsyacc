module FslexFsyacc.Fslex.FslexDFA
let nextStates = [0u,["%%",5u;"&",5u;"(",5u;")",3u;"*",3u;"+",3u;"/",5u;"=",5u;"?",3u;"CAP",5u;"HEADER",2u;"HOLE",3u;"ID",3u;"LITERAL",3u;"REDUCER",5u;"[",5u;"]",3u;"|",5u];1u,["%%",1u];2u,["%%",1u];3u,["(",4u;"HOLE",4u;"ID",4u;"LITERAL",4u;"[",4u]]
open FslexFsyacc.Runtime
open FslexFsyacc.Fslex
open FslexFsyacc.Fslex.FslexTokenUtils
type token = Position<FslexToken>
let rules:list<uint32 list*uint32 list*(list<token>->_)> = [
    [1u],[],fun (lexbuf:list<_>) ->
        [lexbuf.Head]
    [4u],[3u],fun (lexbuf:list<_>) ->
        appendAMP lexbuf
    [2u;3u;5u],[],fun (lexbuf:list<_>) ->
        lexbuf
]
let analyzer = Analyzer<_,_>(nextStates, rules)
