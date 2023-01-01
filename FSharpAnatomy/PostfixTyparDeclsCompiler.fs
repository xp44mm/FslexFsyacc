module FSharpAnatomy.PostfixTyparDeclsCompiler

open FslexFsyacc.Runtime
open FSharp.Idioms
open FSharp.Literals.Literal
open System

let analyze (posTokens:seq<Position<FSharpToken>>) = 
    posTokens
    |> ArrayTypeSuffixDFA.analyze

let parser = Parser<Position<FSharpToken>> (
    PostfixTyparDeclsParseTable.rules,
    PostfixTyparDeclsParseTable.actions,
    PostfixTyparDeclsParseTable.closures,
    
    PostfixTyparDeclsUtils.getTag,
    PostfixTyparDeclsUtils.getLexeme)

let parse(tokens:seq<Position<FSharpToken>>) =
    tokens
    |> parser.parse
    |> PostfixTyparDeclsParseTable.unboxRoot

let compile(txt:string) =
    let tokenize(context:CompilerContext<FSharpToken>) =
        let i = CompilerContext.nextIndex context
        match
            txt.[i..]
            |> PostfixTyparDeclsUtils.tokenize i
            |> ArrayTypeSuffixDFA.analyze
            |> Seq.head
        with tok ->
            {
                context with
                    tokens = tok::context.tokens
            }
            
    let parse loop (context:CompilerContext<FSharpToken>) =
        match context.tokens.Head with
        | {value=EOF} ->
            let states = 
                match parser.tryReduce(context.states) with
                | Some nextStates -> nextStates
                | None -> context.states
            match states with
            |[(1,lxm);(0,null)] ->  
                lxm
                |> PostfixTyparDeclsParseTable.unboxRoot
            | _ -> failwith "不完整"
        | lookahead ->
            let states = 
                try
                match parser.tryReduce(context.states,lookahead) with
                | Some nextStates -> nextStates
                | None -> context.states
                with _ -> failwith $"{stringify context}"
            match states with
            |[(1,lxm);(0,null)] ->  
                lxm
                |> PostfixTyparDeclsParseTable.unboxRoot
            | _ ->
                loop {
                    context with
                        states = parser.shift(states,lookahead)
                }

    let rec loop (context:CompilerContext<FSharpToken>) =
        context
        |> tokenize
        |> parse loop

    loop {
        tokens = []
        states = [0,null]
    }
