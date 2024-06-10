namespace FslexFsyacc.TypeArguments

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

    member this.toCode() = 
        match this with
        | Anon -> "_"
        | Fun ls ->
            let last = ls.Length - 1
            ls
            |> List.mapi (fun i e -> 
                match e with
                | Fun _ when i < last ->
                    $"({e.toCode()})"
                | _ -> e.toCode()
            )
            |> String.concat "->"
        | Tuple (isStruct, ls: TypeArgument list) -> 
            let m =
                ls
                |> List.map (fun e -> 
                    match e with
                    | Fun _ | Tuple _ ->
                        $"({e.toCode()})"
                    | _ -> e.toCode()
                    )
                |> String.concat "*"
            if isStruct then
                $"struct ({m})"
            else
                m
        | App (atomtype:TypeArgument, suffixTypes:SuffixType list) -> 
            [
                match atomtype with
                | Fun _ | Tuple _ -> 
                    $"({atomtype.toCode()})"
                | _ -> atomtype.toCode()
                yield! suffixTypes |> List.map(fun t -> t.toCode())
            ]
            |> String.concat " " // 如果是[]不用空格？

        | TypeParam (isInline:bool, nm: string) ->
            let p = if isInline then "^" else "'"
            $"{p}{nm}"

        | Ctor (longIdent:string list, ls: TypeArgument list) ->
            let h = String.concat "." longIdent
            if ls.IsEmpty then
                h
            else
                let aa = 
                    ls 
                    |> List.map(fun a -> a.toCode())
                    |> String.concat ","
                $"{h}<{aa}>"

        | AnonRecd (isStruct: bool, fields: (string * TypeArgument) list) -> 
            let cc =
                fields
                |> List.map(fun(n,t) -> $"{n}:{t.toCode()}" )
                |> String.concat ";"
                |> sprintf "{|%s|}"
            [
                if isStruct then yield "struct" else ()
                yield cc
            ]
            |> String.concat " "
        | Flexible (tp: BaseOrInterfaceType) -> 
            tp.toCode()
        | Subtype (tp: Typar, it: BaseOrInterfaceType) ->
            $"{tp.toString()}:>{it.toCode()}"

and BaseOrInterfaceType =
    | FlexibleAnon
    | FlexibleApp of atomtype: TypeArgument * suffixTypes: SuffixType list
    | FlexibleCtor of string list * TypeArgument list

    member this.toCode() = 
        match this with
        | FlexibleAnon -> "#_"

        | FlexibleApp (atomtype: TypeArgument , suffixTypes: SuffixType list) ->
            let app =
                [
                    atomtype.toCode()
                    yield! suffixTypes |> List.map(fun t -> t.toCode())
                ]
                |> String.concat " "
            $"#({app})"

        | FlexibleCtor (longIdent: string list , ls: TypeArgument list) ->
            let ctor =
                let h = String.concat "." longIdent
                if ls.IsEmpty then
                    h
                else
                    let aa = ls |> List.map(fun a -> a.toCode()) |> String.concat ","
                    $"{h}<{aa}>"
            $"#{ctor}"
