module FslexFsyacc.Runtime.YACCs.ParseTableAction

open System
open FSharp.Idioms.Literal
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.BNFs

//[<Obsolete("未使用")>]
//let getExactlyOneAction (actions:ParseTableAction list) =
//    match actions with
//    | [] -> failwith $"nonassoc error."
//    | [x] -> x
//    | acts -> failwith $"there is a conflict: {stringify acts}"

///// 删除冲突的项目
//let disambiguate (tryGetPrecedenceCode: string list -> int option) (actions: ParseTableAction Set) =
//    match Seq.toList actions with
//    | [] -> failwith "never"
//    | [x] -> Some x
//    | [Shift k as s; Reduce p as r] ->
//        //shift的优先级
//        let sprec =
//            (Seq.head k).production
//            |> tryGetPrecedenceCode

//        //reduce的优先级
//        let rprec = tryGetPrecedenceCode p

//        match sprec,rprec with
//        | _,None | None,_ -> Some s
//        | Some sprec, Some rprec -> 
//        // 如果没有取得iprec则用shift
//        if rprec > sprec then
//            Some r
//        elif rprec < sprec then
//            Some s
//        else
//            //优先级相等，在同一行，有相同结合性
//            match Associativity.from(rprec) with
//            | NonAssoc -> // %nonassoc
//                None
//            | RightAssoc -> // %right
//                Some s
//            | LeftAssoc -> // %left
//                Some r
//            //| _ -> failwith $"precedence should int [0;1;9] but {rprec}."
//    | ls -> failwith $"{ls}"

/// 删除冲突的项目
let disambiguate2 (tryGetPrecedence: string list -> option<int*Associativity>) (actions: ParseTableAction Set) =
    match Seq.toList actions with
    | [] -> failwith "never"
    | [x] -> Some x
    | [Shift k as s; Reduce p as r] ->
        //shift的优先级
        let sprec =
            (Seq.head k).production
            |> tryGetPrecedence

        //reduce的优先级
        let rprec = tryGetPrecedence p

        match sprec,rprec with
        | _,None | None,_ -> Some s
        | Some (sprec,sassoc), Some (rprec,rassoc) ->
        // 如果没有取得iprec则用shift
        if rprec > sprec then
            Some r
        elif rprec < sprec then
            Some s
        else
            if sassoc <> rassoc then failwith "never"
            //优先级相等，在同一行，有相同结合性
            match sassoc with
            | NonAssoc -> None
            | RightAssoc -> Some s
            | LeftAssoc -> Some r
    | ls -> failwith $"{ls}"

