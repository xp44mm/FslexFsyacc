module PolynomialExpressions.TermDFA
let nextStates = Map [0u,Map ["+",1u;"-",1u;"ID",3u;"INT",2u];1u,Map ["ID",3u;"INT",2u];2u,Map ["ID",3u];3u,Map ["**",4u];4u,Map ["INT",5u]]
let lexemesFromFinal = Map.empty
let universalFinals = set [2u;3u;5u]
let indicesFromFinal = Map [2u,0;3u,1;5u,1]
let header = "open System\r\nopen PolynomialExpressions.Tokenizer"
let semantics = ["// multiline test\r\ntoConst lexbuf";"toTerm lexbuf"]
open System
open PolynomialExpressions.Tokenizer
let mappers = [|
    fun (lexbuf:(int*int*_)list) ->
        // multiline test
        toConst lexbuf
    fun (lexbuf:(int*int*_)list) ->
        toTerm lexbuf
|]
let finalMappers =
    indicesFromFinal
    |> Map.map(fun _ i -> mappers.[i])
open FslexFsyacc.Runtime
let analyzer = Analyzer(nextStates, lexemesFromFinal, universalFinals, finalMappers)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)