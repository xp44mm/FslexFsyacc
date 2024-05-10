module FslexFsyacc.Runtime.ParseTables.Action

open FSharp.Idioms.Literal
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.BNFs

let getExactlyOneAction (actions:Action list) =
    match actions with
    | [] -> failwith $"nonassoc error."
    | [x] -> x
    | acts -> failwith $"there is a conflict: {stringify acts}"

/// 删除冲突的项目
let disambiguate (tryGetPrecedenceCode: string list -> int option) (actions: Action Set) =
    match Seq.toList actions with
    | [] -> failwith "never"
    | [x] -> Some x
    | [Shift k as s; Reduce p as r] ->
        //shift的优先级
        let sprec =
            (Seq.head k).production
            |> tryGetPrecedenceCode

        //reduce的优先级
        let rprec = tryGetPrecedenceCode p

        match sprec,rprec with
        | _,None | None,_ -> Some s
        | Some sprec, Some rprec -> 
        // 如果没有取得iprec则用shift
        if rprec > sprec then
            Some r
        elif rprec < sprec then
            Some s
        else
            match rprec % 10 with
            | 0 -> // %nonassoc
                None
            | 1 -> // %right
                Some s
            | 9 -> // %left
                Some r
            | _ -> failwith $"precedence should int [0;1;9] but {rprec}."
    | ls -> failwith $"{ls}"

