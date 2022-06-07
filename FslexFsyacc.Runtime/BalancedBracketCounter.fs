namespace FslexFsyacc

// 大小相等，符号相反的括号是配对的括号
type BalancedBracketCounter<'t when 't : equality>() =
    let mutable tick = 0
    let mutable brackets = []
    
    member this.addLeft(bracket:'t) =
        tick <- tick + 1
        brackets <- (bracket,tick)::brackets

    // 0表示没有未匹配的左括号
    member this.lastUnmatchedLeft() =
        let rec loop (negs:Set<_>) (unmatched) =
            match unmatched with
            | [] -> 0
            | (_,tck)::tail ->
                if tck > 0 then
                    if negs.IsEmpty then
                        tck
                    else
                        let negs = negs.Remove(-tck)
                        loop negs tail
                else
                    let negs = negs.Add tck
                    loop negs tail 
        loop Set.empty brackets

    member this.addRight(bracket:'t) =
        let i = this.lastUnmatchedLeft()
        brackets <- (bracket,-i)::brackets

    member this.getBrackets() = 
        brackets
        |> List.rev

    member this.getOpposite(bracket:'t) =
        let tck =
            brackets
            |> List.find(fun(bra,tick) ->bra=bracket)
            |> snd
        let opp =
            brackets
            |> List.find(fun(bra,tick)->tick= -tck)
            |> fst
        opp