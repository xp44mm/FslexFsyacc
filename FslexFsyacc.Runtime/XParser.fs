namespace FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Literals
open FslexFsyacc.Runtime.ParseTableUtils
open FSharp.Idioms
open FSharp.Literals

type XParser<'tok> (
        rules: (string list*(obj[]->obj))[],
        actions: (string*int)[][],
        closures: (int*int*string[])[][],
        getTag:'tok -> string,
        getLexeme:'tok->obj

    ) =

    let tbl = 
        ParserTable<'tok>.create(
            rules,
            actions,
            closures,
            getTag,
            getLexeme
        )

    ///
    member _.parse(tokens:seq<'tok>) =
        let iterator = Iterator(tokens.GetEnumerator())
        let rec loop
            (states: int list)
            (trees: obj list)
            (maybeToken:'tok option)
            =
            match tbl.execute(states,trees,maybeToken) with
            | Accept -> trees
            | Shift(nextStates,nextTrees) ->
                iterator.tryNext()
                |> loop nextStates nextTrees
            | Reduce(nextStates,nextTrees) ->
                loop nextStates nextTrees maybeToken
            | Dead ->
                let sm = states.Head
                let closure = 
                    tbl.closures.[sm]
                    |> RenderUtils.renderClosure
                let tok =
                    match maybeToken with
                    | None -> "EOF"
                    | Some tok -> Literal.stringify tok
                failwith $"\r\nlookahead:{tok}\r\nclosure {sm}:\r\n{closure}"

        iterator.tryNext()
        |> loop [0] []
        |> List.exactlyOne

