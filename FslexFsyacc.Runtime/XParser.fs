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
        ParserTable.create(
            rules,
            actions,
            closures
        )

    member this.parse(tokens:seq<'tok>) =
        let iterator = 
            tokens.GetEnumerator()
            |> Iterator

        let rec loop
            (states: (int*obj) list)
            (maybeToken: 'tok option)
            =
            let action() =
                match maybeToken with
                | None -> tbl.complete(states)
                | Some token -> tbl.next(getTag,getLexeme,states,token)

            match action() with
            | Accept -> states
            | Shift states ->
                iterator.tryNext()
                |> loop states 
            | Reduce states ->
                loop states maybeToken
            | Dead(sm,ai) ->
                let closure = 
                    tbl.closures.[sm]
                    |> RenderUtils.renderClosure
                let tok =
                    match maybeToken with
                    | None -> "EOF"
                    | Some tok -> Literal.stringify tok
                failwith $"\r\nlookahead:{tok}\r\nclosure {sm}:\r\n{closure}"

        iterator.tryNext()
        |> loop [0,null]
        |> List.head
        |> snd

