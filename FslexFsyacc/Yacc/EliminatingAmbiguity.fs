module FslexFsyacc.Yacc.EliminatingAmbiguity

//let eliminateActions 
//    (productionOperators:Map<string list,string>) 
//    (kernelProductions:Map<Set<ItemCore>,Set<string list>>)
//    (precedences: Map<PrecedenceKey,int>)
//    =

//    let precOfProd = PrecedenceResolver.resolvePrecOfProd productionOperators precedences
//    let precOfTerm = PrecedenceResolver.resolvePrecOfTerminal kernelProductions precedences

//    fun (source:Set<ItemCore>, symbol:string, actions:Set<Action>) ->
//        let action =
//            match Set.toList actions with
//            | [action] -> action
//            | [Shift target as shift; Reduce production as reduce] ->
//                let precTerminal = precOfTerm target symbol
//                let precProd = precOfProd(production)
//                if precProd > precTerminal then
//                    reduce
//                elif precProd < precTerminal then
//                    shift
//                else (* precProd = precTerminal *)
//                    match precProd % 10 with
//                    | 9 -> // %left
//                        reduce
//                    | 1 -> // %right
//                        shift
//                    | 0 | _ -> // %nonassoc
//                        //不相关
//                        DeadState
//            | ls -> failwithf "%A" ls
//        source,symbol,action
