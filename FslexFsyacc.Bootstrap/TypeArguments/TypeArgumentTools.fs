module FslexFsyacc.TypeArguments.TypeArgumentTools

//本模块中的函数用于fsyacc中的代码块使用。

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
    | _ -> 
        ls 
        |> List.map ofTuple 
        |> Fun

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

let uniform (targ:TypeArgument) = 
    let rec app2ctor suffixs ctorArgs =
        match suffixs with
        | [] -> ctorArgs |> Seq.exactlyOne
        | (h:SuffixType) :: t ->
            let ctor = [Ctor(h.toLongIdent, ctorArgs)]
            app2ctor t ctor
    match targ with
    | Anon -> targ
    | Fun _ -> targ
    | Tuple _ -> targ
    | TypeParam _ -> targ
    | Ctor  _ -> targ
    | AnonRecd  _ -> targ
    | Subtype  _ -> targ
    | App (atomtype:TypeArgument, suffixTypes:SuffixType list) ->
        [atomtype]
        |> app2ctor suffixTypes

    | Flexible btp ->
        match btp with
        | FlexibleAnon
        | FlexibleCtor _ 
            -> targ
        | FlexibleApp (atomtype: TypeArgument, suffixTypes: SuffixType list) ->
            match
                [atomtype]
                |> app2ctor suffixTypes
            with
            | Ctor(a,b) -> Flexible(FlexibleCtor(a,b))
            | _ -> failwith "never"

