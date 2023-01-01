module FSharpAnatomy.FSharpCompiler
open FSharp.Literals.Literal
open FslexFsyacc.Runtime

//let willTypeArgument (states:(int*obj)list) =
//    states
//    |> TypeConstraintsLookbackDFA.analyze
//    |> Seq.tryHead
//    |> Option.defaultValue false

//let parsers = Map [
//    "TypeArgument",TypeArgumentCompiler.parser
//    "TypeConstraints",TypeConstraintsCompiler.parser
//]

//let text = ""

//let tokenizes = Map [
//    "TypeArgument",(fun i ->
//        text.[i..]
//        |> TypeArgumentUtils.tokenize
//        |> TypeArgumentDFA.analyze
//        )
//    "TypeConstraints",(fun i ->
//        text.[i..]
//        |> TypeConstraintsUtils.tokenize
//        )
//]

//let tokenizeTypeConstraints (contexts:(string*FSharpToken CompilerContext)list) =
//    let kind, context = contexts.Head
//    let tokenize = tokenizes.[kind]

//    if willTypeArgument context.states then
//        let yieldContext = {
//            tokens = []
//            states = [0,null]
//        }
//        let contexts = ("TypeArgument",yieldContext)::contexts
//        contexts
//    else
//        let context =
//            match context |> CompilerContext.nextIndex |> tokenize |> Seq.tryHead with
//            | Some tok ->
//                {
//                    context with
//                        tokens = tok :: context.tokens
//                }
//            | None ->context
//        let contexts = ("TypeConstraints",context) :: contexts.Tail
//        contexts

//let shiftTypeConstraints (contexts:(string*FSharpToken CompilerContext)list) =
//    let kind,context = contexts.Head
//    let parser = parsers.[kind]
//    let states = context.states

//    let lookahead = context.tokens.Head
//    {
//        context with
//            states = parser.shift(states,lookahead)
//    }

//let acceptTypeConstraints (contexts:(string*FSharpToken CompilerContext)list) =
//    let kind,context = contexts.Head
//    let parser = parsers.[kind]
//    let states = context.states

//    match
//        parser.tryReduce(states)
//        |> Option.defaultValue states
//    with
//    |[(1,lxm);(0,null)] ->
//        lxm
//        |> TypeConstraintsParseTable.unboxRoot
//    | newStates ->
//        failwith $"states:{stringify newStates}\r\ntok:EOF"

//let tokenizeTypeArgument (contexts:(string*FSharpToken CompilerContext)list) =
//    let kind,context = contexts.Head
//    let tokenize = tokenizes.[kind]

//    let context =
//        match context |> CompilerContext.nextIndex |> tokenize |> Seq.tryHead with
//        | Some tok ->
//            {
//                context with
//                    tokens = tok :: context.tokens
//            }
//        | None -> context
//    let contexts = ("TypeArgument",context) :: contexts.Tail
//    contexts

//let shiftTypeArgument (contexts:(string*FSharpToken CompilerContext)list) =
//    let kind,context = contexts.Head
//    let parser = parsers.[kind]
//    let states = context.states
    
//    let lookahead = context.tokens.Head
//    {
//        context with
//            states = parser.shift(states,lookahead)
//    }
