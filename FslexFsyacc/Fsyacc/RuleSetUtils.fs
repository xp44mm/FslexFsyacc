module FslexFsyacc.Fsyacc.RuleSetUtils

open FslexFsyacc.Runtime
open FslexFsyacc.Yacc

open FSharp.Idioms
open FSharp.Literals.Literal
open System

/////
//let ofRaw (rules:(string*(list<list<string>*string*string>))list) =
//    rules
//    |> List.collect(fun(lhs,bodies)->
//        bodies
//        |> List.map(fun (body,name,semantic)->
//            lhs::body,name,semantic
//        )
//    )
//    |> Set.ofList

///// 对相同lhs的rule合并，仅此
//let toRaw (rules:Set<list<string>*string*string>) =
//    rules
//    |> Seq.groupBy(fun(prod,_,_)->prod.Head) //lhs
//    |> Seq.map(fun(lhs,groups)->
//        let rhs =
//            groups
//            |> Seq.map(fun(prod,name,sem) ->
//                prod.Tail,name,sem
//            )
//            |> Seq.toList
//        lhs,rhs
//    )
//    |> Seq.toList

//let ofMainRules (mainRules:list<Production*string*string>) =
//    let p0,_,_ = mainRules.[0]
//    let startSymbol = p0.[0]

//    mainRules
//    |> List.map(fun (p,d,s) -> p,(d,s))
//    |> Map.ofList
//    |> Map.add ["";startSymbol] ("","s0")

//let getMainRules (augmentRules:Map<Production,string*string>) =
//    let augProds = 
//        augmentRules
//        |> Map.keys

//    let crew = 
//        augProds
//        |> ProductionsCrewUtils.ofAugmentedProductions

//    let rules =
//        crew.inputProductionList
//        |> List.map(fun prod -> 
//            let dummy,semantic = augmentRules.[prod]
//            prod,dummy,semantic)
//    rules

//let filterDummyProductions (augmentRules:Map<Production,string*string>) =
//    augmentRules
//    |> Map.filter(fun prod (dummy,act) -> dummy > "")
//    |> Map.map (fun prod (dummy,act) -> dummy)

//let getSemanticRules (augmentRules:Map<Production,string*string>) = 
//    augmentRules 
//    |> Map.filter(fun prod _ -> prod.[0] > "")
//    |> Map.map (fun _ (_,s) -> s)

//let removeErrorRules (robust:Set<string>) (rules:Map<Production,string*string>) =
//    rules
//    |> Map.filter(fun prod _ -> ProductionUtils.isWithoutError robust prod)

//let eliminateChomsky (augmentRules:Map<Production,string*string>) =
//    let productions =
//        augmentRules
//        |> Map.keys
//        |> Set.toList

//    let newProductions =
//        productions
//        |> ProductionListUtils.eliminateChomsky

//    newProductions
//    |> List.map(fun prod -> prod,augmentRules.[prod])
//    |> Map.ofList




