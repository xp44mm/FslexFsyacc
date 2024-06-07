namespace FslexFsyacc.TypeArguments

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

