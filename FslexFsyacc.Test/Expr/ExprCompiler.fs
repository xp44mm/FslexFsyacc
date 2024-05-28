module FslexFsyacc.Expr.ExprCompiler

open System.IO
open System.Text

open FSharp.Idioms
open FSharp.Idioms.Literal

open FslexFsyacc.Runtime

open FslexFsyacc.Expr.ExprParseTable

//let stateSymbolPairs = parser.getStateSymbolPairs()

let getParser<'tok> getTag getLexeme =
    let getTag (tok:'tok) =
        let tag = getTag tok
        if tokens.Contains tag then
            tag
        else raise (invalidArg "tok" $"{tag}")
    FslexFsyacc.Runtime.MoreParser<'tok>.from(rules, actions, getTag, getLexeme)

let parser = getParser< Position<ExprToken> >
                ExprToken.getTag
                ExprToken.getLexeme

let parse(tokens:seq<Position<ExprToken>>) =
    tokens
    |> parser.parse
    |> ExprParseTable.unboxRoot

//let compile (txt:string) =
//    txt
//    |> ExprToken.tokenize 0
//    |> parse

let compile (txt:string) =
    let mutable tokens = []
    let mutable states = [0,null]

    txt
    |> ExprToken.tokenize 0
    |> Seq.map(fun tok ->
        tokens <- tok::tokens
        //let tag = ExprToken.getTag tok
        //if ExprParseTable.tokens.Contains tag = false then
        //    failwith $"{tag} not in {ExprParseTable.tokens}"
        tok
    )
    |> Seq.iter(fun tok ->
        // 若干步reduce
        match parser.tryReduce(states,tok) with
        | None -> ()
        | Some x -> states <- x

        //正常流程一定是shift
        states <- parser.shift(states,tok)
    )

    match parser.tryReduce(states) with
    | None -> ()
    | Some x -> states <- x

    match states with
    |[1,lxm; 0,null] ->
        ExprParseTable.unboxRoot lxm
    | _ ->
        failwith $"{stringify states}"
