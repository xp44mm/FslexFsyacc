namespace FslexFsyacc.Fsyacc
open System
open FslexFsyacc.Precedences
open FslexFsyacc.TypeArguments
open FSharp.Idioms
open FSharp.Idioms.Line

//内嵌在*.yacc源文件的reducer中，表示yacc的返回结果。
type RawFsyaccFile = 
    {
        header: string
        ruleGroups: RuleGroup list
        operatorsLines: list<Associativity*string list>
        declarationsLines: list<TypeArgument*string list>
    }

    member fsyacc.toCode() =
        let h = 
            fsyacc.header
            |> RawFsyaccFileRender.renderHeader 

        let r =
            fsyacc.ruleGroups
            |> List.map(fun ruleGroup ->
                ruleGroup.toCode()
                )
            |> String.concat "\r\n"

        let p() =
            fsyacc.operatorsLines
            |> List.map(fun (assoc,symbols) ->
                //RawFsyaccFileRender.renderPrecedenceLine(assoc.render(),symbols)
                [
                    assoc.render()
                    yield! symbols 
                        |> Seq.map RawFsyaccFileRender.renderSymbol
                ]
                |> String.concat " "
                )
            |> String.concat "\r\n"

        let d() =
            fsyacc.declarationsLines
            |> List.map RawFsyaccFileRender.renderTypeLine
            |> String.concat "\r\n"

        // rule+prec+decl
        let rpd =
            [
                r
                if fsyacc.operatorsLines.IsEmpty then () else p()
                if fsyacc.declarationsLines.IsEmpty then () else d()
            ]
            |> String.concat "\r\n%%\r\n"

        [h;rpd] |> String.concat "\r\n"
