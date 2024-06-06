namespace rec FslexFsyacc.TypeArguments

/// 又名：signature syntax
type Typar =
    | AnonTypar
    | NamedTypar of isInline:bool * string // todo '\'' '^'

    member this.toString() = 
        match this with
        | AnonTypar -> "_"
        | NamedTypar (isInline, id) -> 
            let p = if isInline then "^" else "'"
            $"{p}{id}"

type SuffixType =
    | LongIdent of string list
    | ArrayTypeSuffix of int

    member this.toString() = 
        match this with
        | LongIdent ids -> ids |> String.concat "."
        | ArrayTypeSuffix rank -> 
            let cs = String.replicate rank ","
            $"[{cs}]"

type TypeArgument =
    | Anon
    | Fun of TypeArgument list
    | Tuple of isStruct: bool * TypeArgument list
    | App of atomtype:TypeArgument * suffixTypes:SuffixType list
    | TypeParam of isInline:bool * string
    | Ctor of longIdent:string list * TypeArgument list
    | AnonRecd of isStruct: bool * fields: (string * TypeArgument) list
    | Flexible of BaseOrInterfaceType
    | Subtype of Typar * BaseOrInterfaceType

    member this.toString() = 
        match this with
        | Anon -> "_"
        | Fun ls -> sprintf ""
        | Tuple (isStruct, ls: TypeArgument list) -> ""
        | App (atomtype:TypeArgument , suffixTypes:SuffixType list) -> sprintf ""
        | TypeParam ( isInline:bool , nm: string) -> 
            let p = if isInline then "^" else "'"
            $"{p}{nm}"
        | Ctor (longIdent:string list , ls: TypeArgument list) -> ""
        | AnonRecd ( isStruct: bool, fields: (string * TypeArgument) list) -> ""
        | Flexible (tp: BaseOrInterfaceType) -> ""
        | Subtype (tp: Typar , it: BaseOrInterfaceType) -> ""

type BaseOrInterfaceType =
    | FlexibleAnon
    | FlexibleApp of atomtype:TypeArgument * suffixTypes:SuffixType list
    | FlexibleCtor of string list * TypeArgument list

