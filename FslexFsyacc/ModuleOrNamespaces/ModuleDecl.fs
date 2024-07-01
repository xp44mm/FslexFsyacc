namespace FslexFsyacc.ModuleOrNamespaces
open FslexFsyacc.TypeArguments

//type Typar = Typar of string

type ModuleDecl =
    | Open of string list
    | OpenType of TypeArgument
    | TypeAbb of string list * TypeArgument
    //| LetLine of string
