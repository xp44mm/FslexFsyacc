namespace FslexFsyacc.ModuleOrNamespaces
open FslexFsyacc.TypeArguments

type ModuleOrNamespaceToken =
    | COMMA
    | DOT
    | EQUALS
    | LANGLE
    | RANGLE
    | OPEN
    | TYPE
    | IDENT of string
    | TARG of TypeArgument
    | TYPAR of string

    member token.getTag() =
        match token with
        | COMMA -> ","
        | DOT -> "."
        | EQUALS -> "="
        | LANGLE -> "<"
        | RANGLE -> ">"
        | OPEN -> "OPEN"
        | TYPE -> "TYPE"
        | IDENT _ -> "IDENT"
        | TARG _ -> "TARG"
        | TYPAR _ -> "TYPAR"

    member token.getLexeme() =
        match token with
        | IDENT x -> box x
        | TARG x -> box x
        | TYPAR x -> box x
        | _ -> null

