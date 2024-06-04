[<RequireQualifiedAccess>]
module FslexFsyacc.Runtime.Grammars.PrecedeUtils

let make 
    (nullables:Set<string>)
    (lasts:Map<string,Set<string>>)
    (productions:Set<string list>) =

    productions
    |> Set.map(fun p -> p.Head :: List.rev p.Tail)
    |> FollowUtils.make nullables lasts

