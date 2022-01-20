module PolynomialExpressions.TermDFA
let nextStates = [|0u,[|"+",1u;"-",1u;"ID",3u;"INT",2u|];1u,[|"ID",3u;"INT",2u|];2u,[|"ID",3u|];3u,[|"**",4u|];4u,[|"INT",5u|]|]
let finalLexemes:(uint32[]*uint32[])[] = [|[|2u|],[||];[|3u;5u|],[||]|]
let header = "open PolynomialExpressions.Tokenizer\r\ntype token = int*int*Token"
let semantics = [|"// multiline test\r\ntoConst lexbuf";"toTerm lexbuf"|]
open PolynomialExpressions.Tokenizer
type token = int*int*Token
let mappers = [|
    fun (lexbuf:token list) ->
        // multiline test
        toConst lexbuf
    fun (lexbuf:token list) ->
        toTerm lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer2(nextStates, finalLexemes, mappers)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)