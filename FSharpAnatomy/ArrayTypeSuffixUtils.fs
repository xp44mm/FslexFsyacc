module FSharpAnatomy.ArrayTypeSuffixUtils

open FslexFsyacc.Runtime

let getTag (token:Position<FSharpToken>) =
    match token.value with
    | LBRACK -> "["
    | RBRACK -> "]"
    | COMMA -> ","
    | WHITESPACE _ -> "WHITESPACE"
    | COMMENT _ -> "COMMENT"
    | ARRAY_TYPE_SUFFIX _ -> "ARRAY_TYPE_SUFFIX"
    | x -> "ordinary"
