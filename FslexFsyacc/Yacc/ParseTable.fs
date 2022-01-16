namespace FslexFsyacc.Yacc

open FSharp.Idioms

type ParseTable =
    {
        productions:string list[]
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
            productions = 
                tbl.grammar.productions 
                |> Set.toArray            
            closures = 
                encoder.encodeClosures tbl.closures
            actions = 
                encoder.encodeActions tbl.actions
            encoder = encoder
        }
