namespace FslexFsyacc.Yacc

///// BNF不带优先级
//type ParseTable =
//    {
//        encoder:ParseTableEncoder
//        actions:(string*int)list list
//        closures:(int*int*string list)list list
//    }
//    [<System.ObsoleteAttribute("EncodedParseTableCrewUtils.getEncodedParseTableCrew")>]
//    static member create(
//        mainProductions:string list list,
//        productionNames:Map<string list,string>,
//        precedences:Map<string,int>
//        ) =

//        // 动作无歧义的表
//        let tbl =
//            ActionParseTableCrewUtils.getActionParseTableCrew
//                mainProductions
//                productionNames
//                precedences

//        let encoder =
//            {
//                productions =
//                    ParseTableEncoder.getProductions tbl.augmentedProductions
//                kernels = tbl.kernels |> Seq.mapi(fun i k -> k,i) |> Map.ofSeq
//            } : ParseTableEncoder

//        {
//            encoder  = encoder
//            actions  = encoder.getEncodedActions  tbl.actions
//            closures = encoder.getEncodedClosures tbl.resolvedClosures
//        }
