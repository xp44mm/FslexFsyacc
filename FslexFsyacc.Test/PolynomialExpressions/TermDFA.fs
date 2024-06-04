module PolynomialExpressions.TermDFA
let nextStates = [0u,["+",1u;"-",1u;"ID",3u;"INT",2u];1u,["ID",3u;"INT",2u];2u,["ID",3u];3u,["**",4u];4u,["INT",5u]]
open FslexFsyacc
open PolynomialExpressions.Tokenizer
type token = Position<Token>
let rules:list<uint32 list*uint32 list*(list<token>->_)> = [
    [2u],[],fun (lexbuf:list<_>) ->
        // multiline test
        toConst lexbuf
    [3u;5u],[],fun (lexbuf:list<_>) ->
        toTerm lexbuf
]
let analyzer = Analyzer<_,_>(nextStates, rules)
