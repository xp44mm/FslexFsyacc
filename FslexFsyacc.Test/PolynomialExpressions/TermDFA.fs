module PolynomialExpressions.TermDFA
let header = "open PolynomialExpressions.Tokenizer\r\ntype token = int*int*Token"
let nextStates = [|0u,[|"+",1u;"-",1u;"ID",3u;"INT",2u|];1u,[|"ID",3u;"INT",2u|];2u,[|"ID",3u|];3u,[|"**",4u|];4u,[|"INT",5u|]|]
let rules:(uint32[]*uint32[]*string)[] = [|[|2u|],[||],"// multiline test\r\ntoConst lexbuf";[|3u;5u|],[||],"toTerm lexbuf"|]
open PolynomialExpressions.Tokenizer
type token = int*int*Token
let fxRules:(uint32[]*uint32[]*_)[] = [|
    [|2u|],[||],fun (lexbuf:token list) ->
        // multiline test
        toConst lexbuf
    [|3u;5u|],[||],fun (lexbuf:token list) ->
        toTerm lexbuf
|]
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, fxRules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)