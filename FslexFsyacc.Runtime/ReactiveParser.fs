namespace FslexFsyacc.Runtime

open FSharp.Idioms
open FSharp.Literals
open System.Collections.Generic

type ReactiveParser<'tok> (
        rules: (string list*(obj[]->obj))[],
        actions: (string*int)[][],
        closures: (int*int*string[])[][],
        getTag:'tok -> string,
        getLexeme:'tok->obj
    ) =

    let tbl = 
        ParserTable.create(rules, actions, closures)

    member this.next(states,token:'tok) =
        match tbl.next(getTag, getLexeme,states,token) with
        | Dead(sm,ai) -> 
            let closure = 
                tbl.closures.[sm]
                |> RenderUtils.renderClosure
            let tok =
                token
                |> Literal.stringify 
            failwith $"\r\nlookahead:{tok}\r\nclosure {sm}:\r\n{closure}"
        | Reduce states -> 
            this.next(states,token)
        | Accept -> failwith $"next never meet Accept."
        | act -> act

    member this.complete(states) =
        match tbl.complete(states) with
        | Dead(sm,ai) -> 
            let closure = 
                tbl.closures.[sm]
                |> RenderUtils.renderClosure
            failwith $"\r\nlookahead:EOF\r\nclosure {sm}:\r\n{closure}"
        | Shift _ -> failwith "EOF never meet shift."
        | act -> act

        