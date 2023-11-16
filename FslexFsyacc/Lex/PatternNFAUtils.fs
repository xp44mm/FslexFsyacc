module FslexFsyacc.Lex.PatternNFAUtils

open FSharp.Idioms.Literal

//Ordinary and Exotic
/// s/t: an s but only if it is followed by an t. The t is not part of the matched text.
let lookahead(s:RegularNFA<'a>, trailingContext:RegularNFA<'a>) =
    {
        transition = 
            s.transition + trailingContext.transition
            |> Set.add(s.maxState, None, trailingContext.minState)
        minState = s.minState
        lexemeState = s.maxState
        maxState = trailingContext.maxState
    }
        
let sole(s:RegularNFA<'a>) =
    {
        transition = s.transition
        minState = s.minState
        lexemeState = s.maxState
        maxState = s.maxState
    }

let fromRgx(i:uint32, pattern:RegularExpression<'a> list) =
    match pattern with
    | [rgx] ->
        let n1 = RegularNFAUtils.fromRgx i rgx
        sole(n1)
    | [rgx;trailling] ->
        let n1 = RegularNFAUtils.fromRgx i rgx
        let n2 = RegularNFAUtils.fromRgx (n1.maxState+1u) trailling
        lookahead(n1, n2)
    | _ -> failwith $"fromRgx never:{stringify pattern}"
