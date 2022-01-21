module FslexFsyacc.Fslex.FslexDFA2
let header = "open FslexFsyacc.Fslex\r\nopen FslexFsyacc.Fslex.FslexTokenUtils\r\ntype token = int*int*FslexToken"
let nextStates = [|0u,[|"%%",5u;"&",5u;"(",5u;")",3u;"*",3u;"+",3u;"/",5u;"=",5u;"?",3u;"CAP",5u;"HEADER",2u;"HOLE",3u;"ID",3u;"QUOTE",3u;"SEMANTIC",5u;"[",5u;"]",3u;"|",5u|];1u,[|"%%",1u|];2u,[|"%%",1u|];3u,[|"(",4u;"HOLE",4u;"ID",4u;"QUOTE",4u;"[",4u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|1u|],[||],"[lexbuf.Head]";[|4u|],[|3u|],"appendAMP lexbuf";[|2u;3u;5u|],[||],"lexbuf"|]
open FslexFsyacc.Fslex
open FslexFsyacc.Fslex.FslexTokenUtils
type token = int*int*FslexToken
let fRules:(uint32[]*uint32[]*_)[] = [|
    [|1u|],[||],fun (lexbuf:token list) ->
        [lexbuf.Head]
    [|4u|],[|3u|],fun (lexbuf:token list) ->
        appendAMP lexbuf
    [|2u;3u;5u|],[||],fun (lexbuf:token list) ->
        lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer2(nextStates, fRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)