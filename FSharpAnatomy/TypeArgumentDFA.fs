module FSharpAnatomy.TypeArgumentDFA
let nextStates = [0u,["#",4u;"(",4u;")",4u;"*",4u;",",4u;"->",4u;".",4u;":",4u;":>",4u;";",4u;"<",4u;">",4u;"ARRAY_TYPE_SUFFIX",4u;"COMMENT",4u;"IDENT",4u;"INLINE_TYPAR",4u;"TYPAR",4u;"WHITESPACE",4u;"[",1u;"]",4u;"_",4u;"struct",4u;"{|",4u;"|}",4u];1u,[",",2u;"]",3u];2u,[",",2u;"]",3u]]
open FslexFsyacc.Runtime
open FSharpAnatomy.FSharpTokenUtils
type token = Position<FSharpToken>
let rules:list<uint32 list*uint32 list*_> = [
    [3u],[],fun(lexbuf:token list)->
        {
            index = lexbuf.Head.index
            length = Position.totalLength(lexbuf)
            value = ARRAY_TYPE_SUFFIX (lexbuf.Length-1)
        }
    [1u;4u],[],fun(lexbuf:token list)->
        lexbuf.Head
]
let analyzer = Analyzer(nextStates, rules)
let analyze (tokens:seq<_>) = 
    analyzer.analyze(tokens,getTag)