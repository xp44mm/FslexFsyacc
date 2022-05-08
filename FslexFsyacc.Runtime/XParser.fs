namespace FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Literals

type Parser<'tok> (
        rules: (string list*(obj[]->obj))[],
        actions: (string*int)[][],
        closures: (int*int*string[])[][],
        getTag:'tok -> string,
        getLexeme:'tok->obj
    ) =

    let tbl =
        ParserTable.create(rules, actions, closures)

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
                | Some token ->
                    this.next(states,token)
                | None ->
                    this.complete(states)

            match action() with
            | Reduce states ->
                loop states maybeToken
            | Shift states ->
                iterator.tryNext()
                |> loop states
            | Accept ->
                states
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

    member this.next(states,token:'tok) =
        match tbl.next(getTag, getLexeme,states,token) with
        | Reduce states -> 
            this.next(states,token)
        | Accept -> failwith $"next never meet Accept."
        | act -> act

    member this.complete(states) =
        match tbl.complete(states) with
        | Shift _ -> failwith "EOF never meet shift."
        | act -> act

