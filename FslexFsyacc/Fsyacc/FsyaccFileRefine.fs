module FslexFsyacc.Fsyacc.FsyaccFileRefine

open FSharp.Idioms

let refineRules 
    (oldProd:string list)
    (newProd:string list)
    (rules:(string*((string list*string*string)list))list)
    =
    if oldProd.Head <> newProd.Head then
        failwith "refineRules:oldProd.Head <> newProd.Head"
    let newRules =
        rules
        |> List.map(fun((lhs,rhs) as rg) ->
            if lhs = oldProd.Head then
                let rhs =
                    rhs
                    |> List.map(fun((body,name,sem) as bg) ->
                        if body = oldProd.Tail then
                            newProd.Tail,name,sem
                        else bg
                    )
                lhs,rhs
            else rg
        )
    if newRules = rules then
        failwith "refineRules:newRules = rules"
    else
        newRules