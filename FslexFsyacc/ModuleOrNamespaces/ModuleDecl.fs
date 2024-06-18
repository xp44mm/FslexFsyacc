namespace FslexFsyacc.ModuleOrNamespaces
open FslexFsyacc.TypeArguments

type ModuleDecl =
    | Open of string list
    | OpenType of TypeArgument
    //| LetLine of string
    //| TypeLine of string
