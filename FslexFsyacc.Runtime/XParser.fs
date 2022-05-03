namespace FslexFsyacc.Runtime

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

    member this.parse(tokens:seq<'tok>, isFinal) =
        let iterator = 
            tokens.GetEnumerator()
            |> Iterator

        let rec loop
            (states: int list)
            (trees: obj list)
            (maybeToken: 'tok option)
            =

            if isFinal states trees maybeToken then
                states,trees,maybeToken
            else
                match tbl.execute(states,trees,maybeToken) with
                | Accept -> states,trees,maybeToken
                | Shift(states,trees) ->
                    iterator.tryNext()
                    |> loop states trees
                | Reduce(states,trees) ->
                    loop states trees maybeToken
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

    member this.parse(tokens) = 
        let _,trees,_ = 
            this.parse(tokens,fun _ _ _ -> false)
        trees
        |> List.exactlyOne

