module FslexFsyacc.Fsyacc.Declaration
open FslexFsyacc.TypeArguments

/// 
let types (declarationLines:Map<TypeArgument, Set<string>>) = 
    // todo 检测符号重复
    declarationLines
    |> Seq.collect(fun(KeyValue (tp,symbols)) ->
        symbols
        |> Seq.map(fun sym -> sym,tp))
    |> Map.ofSeq

let lines (declarations:Map<string,TypeArgument>) = 
    declarations
    |> Seq.groupBy(fun (KeyValue(sym,tp)) -> tp)
    |> Seq.map(fun (tp,groups) ->
        let symbols =
            groups 
            |> Seq.map(fun (KeyValue(sym,tp)) -> sym)
            |> Set.ofSeq
        tp,symbols
    )
    |> Map.ofSeq
