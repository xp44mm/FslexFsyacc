module FslexFsyacc.VanillaFSharp.TypeArgumentAngleCompiler
open FslexFsyacc.Brackets
open FslexFsyacc
open FSharp.Idioms.Literal
open FSharp.Idioms
open System
open System.Diagnostics

let parser = BoundedParseTable.app.getParser<Position<TypeArgumentAngleToken>>(
                TypeArgumentAngleToken.getTag,
                TypeArgumentAngleToken.getLexeme
                )

let tbl = BoundedParseTable.app.getTable parser

let compile (offset) (input:string) =
    //let mutable tokens = []
    let mutable states = [0,null]

    let acceptStates = ["LEFT"; "bands"; "RIGHT"] |> List.rev

    let ongoing () = 
        let rec loop expect actual =
            match expect, actual with
            | he::te,(state,_)::ta when tbl.kernelSymbols.[state] = he ->
                loop te ta
            | [],[0,null] -> false
            | _ -> true
        loop acceptStates states

    let tokenIterator = Iterator(TypeArgumentAngleToken.tokenize offset input)

    seq {
        while ongoing() do
            // 无限循环，一定有下一个，可直接获取值
            yield tokenIterator.tryNext().Value
    }
    //|> Seq.map(fun tok ->
    //    tokens <- tok::tokens // 记录当前tokens
    //    tok
    //)
    |> Seq.filter(fun tok ->
        match tok.value with
        | WHITESPACE _ | COMMENT _ -> false
        | _ -> true
    )
    |> Seq.iter(fun tok ->
        //Console.WriteLine(stringify states)
        states <- parser.shift(states,tok)
    )
    match parser.tryReduce(states) with
    | None -> ()
    | Some x -> states <- x

    match states with
    | [1,lxm; 0,null] -> BoundedParseTable.unboxRoot lxm
    | _ -> failwith $"{stringify states}"
        
let getRange (offset:int) (input:string) =
    match compile offset input with
    | FslexFsyacc.Brackets.Band.Bounded(i,_,j) ->
        Debug.Assert((i=offset))
        {
            index = i
            length = j-i+1
            value = input.[1..j-i-1].Trim()
        }
    | never -> failwith $"{never}"
