namespace FslexFsyacc.Fsyacc
open System
open FslexFsyacc
open System.Text.RegularExpressions
open FSharp.Idioms
open FSharp.Idioms.Line

type RuleBody =
    {
        rhs:string list // right hand side
        dummy:string
        reducer:string
    }

    member this.toCode() =
        let renderBody (body:string list) =
            if body.IsEmpty then
                "(*empty*)"
            else
                body
                |> List.map RawFsyaccFileRender.renderSymbol
                |> String.concat " "

        let renderReducer(reducer:string) =
            if Regex.IsMatch(reducer,@"[\r\n]") then
                [
                    "{"
                    indentCodeBlock 4 reducer
                    "}"
                ]
                |> String.concat "\r\n"

            elif String.IsNullOrWhiteSpace(reducer) then
                "{}"
            else
                "{" + reducer + "}"

        //RawFsyaccFileRender.renderComponent(this.rhs, this.dummy, this.reducer)
        [
            yield "|"
            yield renderBody this.rhs
            if this.dummy = "" then () else
                yield "%prec " + this.dummy
            yield renderReducer this.reducer
        ]
        |> String.concat " "
