module FslexFsyacc.Lex.NFACombine

/// Figure 3.40: NFA for the union of two NFA transition
/// 状态编号由小到大排列，没有重复，没有空位：i,s,t
let union (i:uint32) (s:RegularNFA<'a>) (t:RegularNFA<'a>) =
    let f = t.maxState + 1u
    {
        transition = 
            s.transition + t.transition
            |> Set.add(i, None, s.minState)
            |> Set.add(i, None, t.minState)
            |> Set.add(s.maxState, None, f)
            |> Set.add(t.maxState, None, f)
        minState = i
        maxState = f
    }

/// Figure 3.41: NFA for the concatenation of two NFA transition
/// 状态编号：x的最大状态，就是y的最小状态
let concat (s:RegularNFA<'a>) (t:RegularNFA<'a>) =
    {
        transition = 
            s.transition + t.transition
        minState = s.minState
        maxState = t.maxState
    }

/// 数量词：零个或多个
let natural (i:uint32) (nfa:RegularNFA<'a>) =
    let si = nfa.minState
    let sf = nfa.maxState
    let f = nfa.maxState + 1u
    {
        transition =
            nfa.transition
            |> Set.add(i, None, si)
            |> Set.add(i, None, f)
            |> Set.add(sf, None, si)
            |> Set.add(sf, None, f)
        minState = i
        maxState = f
    }

/// 数量词：一个或多个
let positive (nfa:RegularNFA<'a>) =
    let si = nfa.minState
    let sf = nfa.maxState
    { nfa with
        transition =
            nfa.transition
            |> Set.add(sf, None, si)
    }

/// 数量词：零个或一个
let maybe (i:uint32) (nfa:RegularNFA<'a>) =
    let si = nfa.minState
    let sf = nfa.maxState
    let f = nfa.maxState + 1u
    {
        transition =
            nfa.transition
            |> Set.add(i, None, si)
            |> Set.add(i, None, f)
            |> Set.add(sf, None, f)
        minState = i
        maxState = f
    }

