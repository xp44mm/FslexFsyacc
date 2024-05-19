module FslexFsyacc.Fsyacc.DeclarationLinesUtils

//let toMap (declarationLines:(string*string list)list) = 
//    declarationLines
//    |> List.collect(fun (tp,symbols)->
//        symbols
//        |> List.map(fun sym -> sym,tp))
//    |> Map.ofList

//let ofMap (declarations:Map<string,string>) = 
//    declarations
//    |> Map.toList
//    |> List.groupBy(fun (sym,tp)->tp)
//    |> List.map(fun (tp,groups) ->
//        let symbols =
//            groups 
//            |> List.map fst
//        tp,symbols
//    )
