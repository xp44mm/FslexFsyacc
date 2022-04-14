[<RequireQualifiedAccess>]
module FslexFsyacc.Yacc.PrecedeFactory

let make 
    //(nonterminals:Set<string>) 
    (nullables:Set<string>)
    (lasts:Map<string,Set<string>>)
    (productions:Set<string list>) =

    productions
    |> Set.map(fun p -> p.Head :: List.rev p.Tail)
    |> FollowFactory.make nullables lasts

