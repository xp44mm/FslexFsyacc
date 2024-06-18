namespace FslexFsyacc.Fsyacc
open System
open FSharp.Idioms
open FSharp.Idioms.Line


type RuleGroup =
    {
        lhs: string // left hand side
        bodies: RuleBody list
    }

    member this.toCode() =
        [
            RawFsyaccFileRender.renderSymbol this.lhs + " :"
            this.bodies
            |> List.map(fun bd -> bd.toCode())
            |> String.concat "\r\n"
            |> indentCodeBlock 4
        ]
        |> String.concat "\r\n"
