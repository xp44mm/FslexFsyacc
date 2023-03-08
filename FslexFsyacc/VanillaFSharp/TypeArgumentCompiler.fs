module FslexFsyacc.VanillaFSharp.TypeArgumentCompiler

open FslexFsyacc.Runtime
open FSharp.Literals.Literal

let parser = Parser<Position<FSharpToken>> (
    TypeArgumentParseTable.rules,
    TypeArgumentParseTable.actions,
    TypeArgumentParseTable.closures,
    
    TypeArgumentTokenUtils.getTag,
    TypeArgumentTokenUtils.getLexeme)

let parse (tokens:seq<Position<FSharpToken>>) =
    tokens
    |> parser.parse
    |> TypeArgumentParseTable.unboxRoot

let compile (txt:string) =
    let mutable tokens = []
    let mutable states = [0,null]

    txt
    |> TypeArgumentTokenUtils.tokenize 0
    |> Seq.map(fun tok ->
        tokens <- tok::tokens
        tok
    )
    |> Seq.iter(fun tok ->
        states <- parser.shift(states,tok)
    )

    match parser.accept states with
    | [1,lxm; 0,null] -> TypeArgumentParseTable.unboxRoot lxm
    | _ -> failwith $"{stringify states}"

