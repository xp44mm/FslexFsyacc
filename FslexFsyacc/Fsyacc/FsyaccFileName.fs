[<System.Obsolete("FsyaccFileRules")>]
module FslexFsyacc.Fsyacc.FsyaccFileName

open FSharp.Idioms

let productionToHeadBody (names:Map<string list,string>) =
    names
    |> Map.toList
    |> List.map(fun(ls,s)->ls.Head,(ls.Tail,s))
    |> List.groupBy fst
    |> List.map(fun(lhs,ls)->
        let rhs = ls |> List.map snd
        lhs,Map.ofList rhs
    )
    |> Map.ofList

let nameRules 
    (rules:(string*((string list*string*string)list))list)
    (names:Map<string,Map<string list,string>>) =

    rules
    |> List.map(fun((lhs,rhs) as rg) ->
        if names.ContainsKey lhs then
            let names = names.[lhs]
            let rhs =
                rhs
                |> List.map(fun((body,name,sem)as bg) ->
                    if names.ContainsKey body then
                        body,names.[body],sem
                    else bg
                )
            lhs,rhs
        else rg
    )
