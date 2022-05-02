module Interpolation.PlaceholderUtils
open FslexFsyacc.Runtime

let getTag(token:Position<Token>) = 
    match token.value with
    | NumericLiteral _ -> "NUMBER"
    | Punctuator x   -> x
    | _ -> failwith $"getTag:{token}"

let getLexeme(token:Position<Token>) = 
    match token.value with
    | NumericLiteral n -> box n
    | _   -> null

let tokenFilter(token:Position<Token>) = 
    match token.value with
    | WhiteSpace _             -> false
    | LineTerminatorSequence _ -> false
    | SingleLineComment _      -> false
    | MultiLineComment _       -> false
    | IdentifierName _         -> false
    | Punctuator _             -> true
    | NumericLiteral _         -> true
    | SingleStringLiteral _    -> false
    | DoubleStringLiteral _    -> false