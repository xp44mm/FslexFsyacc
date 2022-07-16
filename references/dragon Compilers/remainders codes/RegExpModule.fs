module OptimizationLex.RegExpModule

open Compiler

///叶节点的列表，倒叙
let leaves this =
    let rec loop r acc =
        match r with
        | Leaf c -> c::acc
        | Concat (a,b)
        | Union (a,b) ->
            ([a;b],acc)
            ||> List.foldBack loop
        | Natural c1 
        | Positive c1 
        | Maybe c1 ->
            loop c1 acc
        | extends -> failwith ""
    loop this []

let rec nullable this =
    match this with
    | Natural _ | Maybe _ -> true
    | Leaf _ -> false
    | Union (a,b) -> nullable a || nullable b
    | Concat (a,b) ->  nullable a && nullable b
    | Positive re -> nullable re
    | extends -> failwith ""