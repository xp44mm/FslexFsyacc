namespace FslexFsyacc.Runtime.YACCs
open System

type Associativity =
    | LeftAssoc
    | RightAssoc
    | NonAssoc

    //static member from (iprec:int) =
    //    match iprec % 10 with
    //    | 0 -> NonAssoc
    //    | 1 -> RightAssoc
    //    | 9 -> LeftAssoc
    //    | _ -> raise <| ArgumentOutOfRangeException($"iprec = {iprec} not in [0;1;9]")
                
    //member assoc.value =
    //    match assoc with
    //    | LeftAssoc -> -1
    //    | RightAssoc -> 1
    //    | NonAssoc -> 0
