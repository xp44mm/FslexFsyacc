/// 扩展正则表达式转化为正常的正则表达式
module FslexFsyacc.Lex.LexFileNormalization

/// 代入正则定义
let substitute (definitions:Map<string,_>) (expr:RegularExpression<_>) =
    let rec loop = function
        | Character x  -> Character x
        | Uion (x,y)   -> Uion(loop x, loop y)
        | Concat (x,y) -> Concat(loop x, loop y)
        | Natural x    -> Natural(loop x)
        | Positive x   -> Positive(loop x)
        | Maybe x      -> Maybe(loop x)
        | Hole id -> definitions.[id]

    loop expr

let definitionNames (expr:RegularExpression<_>) =
    let rec loop (expr:RegularExpression<_>) = 
        seq {
            match expr with
            | Character _   -> ()
            | Uion (x,y)
            | Concat (x,y)  -> yield! loop x; yield! loop y;
            | Natural x
            | Positive x
            | Maybe x       -> yield! loop x;
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
        | Character x -> 
            x::ls
        | Uion(a,b) -> 
            let aa = loop [] a
            let bb = loop [] b
            ls @ aa @ bb
        | x -> failwithf "%A" x
    loop [] rgx

