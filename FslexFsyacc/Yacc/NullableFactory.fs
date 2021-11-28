[<RequireQualifiedAccess>]
module FslexFsyacc.Yacc.NullableFactory

open FSharp.Idioms
open System.Collections.Concurrent

let make (mainProductions: Set<string list>) =
    /// 可以为空的非终结符表
    let rec loop (acc:Set<string>) (productions:Set<string list>) =
        let empties, remains =
            productions
            |> Set.partition(fun p -> p.Length = 1) //空产生式

        // 统计空产生式
        let newAcc =
            empties
            |> Set.map(List.head) //空产生式的左侧
            |> Set.union acc

        // 删除剩余产生式中可为空的符号
        let newProductions =
            remains
            |> Set.filter(List.head >> newAcc.Contains >> not) // 丢弃已知为空的产生式
            |> Set.map(List.filter(newAcc.Contains >> not)) // 删除产生式体中可为空的符号

        if newProductions = productions then
            newAcc
        else
            loop newAcc newProductions
    
    loop Set.empty mainProductions

/// 判断输入的符号串是否可以为空
let nullable (nullables:Set<string>) =
    let tbl = ConcurrentDictionary<string list, bool>(HashIdentity.Structural)
    let rec loop (alpha:string list) =
        tbl.GetOrAdd(
            alpha,
            function
            | [] -> true
            | h::t -> nullables.Contains h && loop t
            )
    loop

/// 每个nullables只生成一个缓存
let leftmostLookupMemoiz = ConcurrentDictionary<Set<string>, ConcurrentDictionary<string list, Set<string>>>(HashIdentity.Structural)
       
/// 一个符号串可能的最左符号
let leftmost (nullables:Set<string>) =
    let lookup = 
        leftmostLookupMemoiz.GetOrAdd(
            nullables, 
            fun _ -> ConcurrentDictionary<string list, Set<string>>(HashIdentity.Structural)
            )

    let rec loop (alpha:string list) =
        lookup.GetOrAdd(
            alpha,
            function
            | [] -> Set.empty
            | x :: tail ->
                let rest =
                    if nullables.Contains x then
                        loop tail
                    else Set.empty
                rest.Add x
        )
    loop

let rightmost (nullables:Set<string>) =
    let leftmost = leftmost nullables
    fun (alpha:string list) ->
        alpha
        |> List.rev
        |> leftmost

