namespace FslexFsyacc.Yacc

/// BNF不带优先级
type ParseTable =
    {
        encoder:ParseTableEncoder
        actions:(string*int)list list
        closures:(int*int*string list)list list
    }

    static member create(
        mainProductions:string list list,
        productionNames:Map<string list,string>,
        precedences:Map<string,int>
        ) =

        // 动作无歧义的表
        let tbl =
            ParsingTable.create(
                mainProductions,
                productionNames,
                precedences
            )

        let encoder =
            {
                productions =
                    ParseTableEncoder.getProductions tbl.grammar.productions
                kernels = tbl.kernels
            }:ParseTableEncoder

        {
            encoder  = encoder
            actions  = encoder.encodeActions  tbl.actions
            closures = encoder.encodeClosures tbl.closures
        }
