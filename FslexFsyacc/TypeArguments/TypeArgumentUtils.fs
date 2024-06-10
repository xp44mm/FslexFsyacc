module FslexFsyacc.TypeArguments.TypeArgumentUtils

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
        | [] -> 
            ctorArgs
            |> Seq.exactlyOne
        | (h:SuffixType) :: t ->            
            let ctor = [Ctor(h.toLongIdent, ctorArgs)]
            app2ctor t ctor

    let rec loop (targ:TypeArgument) =
        match targ with
        | Anon -> targ
        | TypeParam _ -> targ
        | Fun targs -> Fun (targs |> List.map loop)
        | Tuple (isStruct,targs) -> Tuple (isStruct, targs |> List.map loop)
        | Ctor (id,gargs) -> Ctor (id,gargs |> List.map loop)
        | AnonRecd (isStruct,fields) -> 
            let fields = 
                fields
                |> List.map(fun (nm,tp) -> nm, loop tp)
                |> Set.ofList |> List.ofSeq
            AnonRecd (isStruct,fields)
        | App (atomtype:TypeArgument, suffixTypes:SuffixType list) ->
            [loop atomtype]
            |> app2ctor suffixTypes
        | Subtype (typar,btp) -> Subtype (typar,flexibleLoop btp)
        | Flexible btp -> Flexible (flexibleLoop btp)

    and flexibleLoop (btp:BaseOrInterfaceType) =
        match btp with
        | FlexibleAnon
        | FlexibleCtor _ 
            -> btp
        | FlexibleApp (atomtype: TypeArgument, suffixTypes: SuffixType list) ->
            match
                [loop atomtype]
                |> app2ctor suffixTypes
            with
            | Ctor(a,targs) -> FlexibleCtor(a,targs |> List.map loop)
            | _ -> failwith "never"

    loop targ
