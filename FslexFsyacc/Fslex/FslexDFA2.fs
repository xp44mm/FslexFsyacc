module FslexFsyacc.Fslex.FslexDFA2
let nextStates = [|0u,[|"%%",5u;"&",5u;"(",5u;")",3u;"*",3u;"+",3u;"/",5u;"=",5u;"?",3u;"CAP",5u;"HEADER",2u;"HOLE",3u;"ID",3u;"QUOTE",3u;"SEMANTIC",5u;"[",5u;"]",3u;"|",5u|];1u,[|"%%",1u|];2u,[|"%%",1u|];3u,[|"(",4u;"HOLE",4u;"ID",4u;"QUOTE",4u;"[",4u|]|]
let finalLexemes:(uint32[]*uint32[])[] = [|[|1u|],[||];[|4u|],[|3u|];[|2u;3u;5u|],[||]|]
let header = "open FslexFsyacc.Fslex\r\nopen FslexFsyacc.Fslex.FslexTokenUtils\r\ntype token = int*int*FslexToken"
let semantics = [|"[lexbuf.Head]";"appendAMP lexbuf";"lexbuf"|]
open FslexFsyacc.Fslex
open FslexFsyacc.Fslex.FslexTokenUtils
type token = int*int*FslexToken
let mappers = [|
    fun (lexbuf:token list) ->
        [lexbuf.Head]
    fun (lexbuf:token list) ->
        appendAMP lexbuf
    fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer2(nextStates, finalLexemes, mappers)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)