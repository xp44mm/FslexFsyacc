namespace FslexFsyacc.ModuleOrNamespaces
open FslexFsyacc.TypeArguments

type ModuleOrNamespaceToken =
    | OPEN
    | TYPE
    | DOT
    | IDENT of string
    | TARG of TypeArgument

    member token.getTag() =
        match token with
        | OPEN -> "OPEN"
        | TYPE -> "TYPE"
        | DOT -> "."
        | IDENT _ -> "IDENT"
        | TARG _ -> "TARG"

    member token.getLexeme() =
        match token with
        | IDENT x -> box x
        | TARG x -> box x
        | _ -> null

