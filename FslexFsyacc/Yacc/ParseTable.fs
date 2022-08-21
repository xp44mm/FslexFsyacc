namespace FslexFsyacc.Yacc

type ParseTable =
    {
        actions:(string*int)[][]
        closures:(int*int*string[])[][]
        encoder:ParseTableEncoder
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
            actions  = encoder.encodeActions  tbl.actions
            closures = encoder.encodeClosures tbl.closures
            encoder  = encoder
        }
