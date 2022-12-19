namespace FSharpAnatomy

type Typar =
    | AnonTypar
    | NamedTypar of isInline:bool * string

type SuffixType =
    | LongIdent of string list
    | ArrayTypeSuffix of int

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

and BaseOrInterfaceType =
    | FlexibleAnon
    | FlexibleApp of atomtype:TypeArgument * suffixTypes:SuffixType list
    | FlexibleCtor of string list * TypeArgument list

module TypeArgument =
    let ofApp (atomtype:TypeArgument, suffixTypes:_ list) =
        match suffixTypes with
        | [] -> atomtype
        | _ -> App(atomtype,suffixTypes)

    let ofTuple (ls:TypeArgument list) =
        match ls with
        | [] -> failwith ""
        | [x] -> x
        | _ -> Tuple(false,ls)

    let ofFun (ls:TypeArgument list list) =
        match ls with
        | [] -> failwith ""
        | [x] -> ofTuple x
        | _ -> ls |> List.map ofTuple |> Fun

    let ofTypar (typar:Typar) =
        match typar with
        | NamedTypar (isInline, id) -> TypeParam(isInline,id)
        | AnonTypar -> failwith ""

    let toBaseOrInterfaceType (apptype:TypeArgument) = 
        match apptype with
        | Anon -> FlexibleAnon
        | App(ty,suffixes) -> FlexibleApp(ty,suffixes)
        | Ctor(id,args) -> FlexibleCtor(id,args)
        | _ -> failwith ""
