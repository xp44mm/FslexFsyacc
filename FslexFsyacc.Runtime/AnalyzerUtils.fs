module FslexFsyacc.Runtime.AnalyzerUtils

let skipUntilFoundIn<'a> (significants:Set<uint32>) (revStates:uint32 list) (revTokens:'a list) =
    let rec loop (revStates) (revTokens:_ list) =
        match revStates with
        | [] -> failwith "never"
        | [0u] -> failwith "no found."
        | state::statetail ->
            if significants.Contains state then
                revStates,revTokens
            else
                loop statetail revTokens.Tail
    loop (revStates) (revTokens)
