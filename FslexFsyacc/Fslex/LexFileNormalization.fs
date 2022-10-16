/// 扩展正则表达式转化为正常的正则表达式
module FslexFsyacc.Lex.LexFileNormalization

/// 代入正则定义
let substitute 
    (definitions:Map<string,RegularExpression<_>>) 
    (expr:RegularExpression<_>) 
    =

    let rec loop = function
        | Atomic x  -> Atomic x
        | Either (x,y)   -> Either(loop x, loop y)
        | Both (x,y) -> Both(loop x, loop y)
        | Natural x    -> Natural(loop x)
        | Plural x   -> Plural(loop x)
        | Optional x      -> Optional(loop x)
        | Hole id -> definitions.[id]

    loop expr

let definitionNames (expr:RegularExpression<_>) =
    let rec loop (expr:RegularExpression<_>) = 
        seq {
            match expr with
            | Atomic _  -> ()
            | Either (x,y)
            | Both (x,y) -> yield! loop x; yield! loop y;
            | Natural x
            | Plural x
            | Optional x      -> yield! loop x;
            | Hole id -> yield id
        }

    loop expr |> Set.ofSeq

/// 定义的右侧，即定义的正则表达式标准化。
let normDefinitions definitions =
    (Map.empty, definitions)
    ||> List.fold(fun mp (id,expr)->
        let expr = substitute mp expr
        mp |> Map.add id expr
    )

/// lex文件规则的正则表达式，获取其求解状态。
let normRules definitions rules =
    let definitions = normDefinitions definitions
    let norm = substitute definitions

    rules
    |> List.map(fun ls -> ls |> List.map norm)

/// 
let characterclass (rgx:RegularExpression<_>) =
    let rec loop ls = function
        | Atomic x -> 
            x::ls
        | Either(a,b) -> 
            let aa = loop [] a
            let bb = loop [] b
            ls @ aa @ bb
        | x -> failwithf "%A" x
    loop [] rgx

