[<RequireQualifiedAccess>]
module FslexFsyacc.Yacc.ConflictFactory

open FSharp.Idioms

/// 列出了需修改产生式规则的冲突
let productionConflict(parseTable:Set<_*_*Set<Action>>)=
    parseTable
    |> Set.filter(fun(src,sym,targets) ->
        match Set.toList targets with
        | [_] -> false
        | [Shift _ ; Reduce _ ] -> false
        | _ -> true
    )

/// 运算符重载警告，运算符被用于多个目的
let overloadsWarning (ambiguousTable:AmbiguousTable) =
    ambiguousTable.kernelProductions
    |> Map.toList
    |> List.map snd
    |> List.filter(fun st -> st.Count > 1)

/// 列出了需使用优先级解决的冲突，需要先固定产生式规则
let shiftReduceConflict (ambiguousTable:AmbiguousTable) =
    ambiguousTable.ambiguousTable
    |> Set.toList
    |> List.choose(fun(src,sym,targets) ->
        match Set.toList targets with
        | [_] -> None
        | [Shift j ; Reduce p2 ] ->
            let p1 = Seq.exactlyOne ambiguousTable.kernelProductions.[j]
            Some(set[p1;p2])
        | _ -> failwithf "请先解决产生式冲突：%A" (src,sym,targets)
    )
    |> Set.ofList
 
