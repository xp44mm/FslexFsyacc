namespace FslexFsyacc.Fsyacc

type PolynomialSymbol =
    /// a
    | Atomic of string

    /// a?*+ :repetition
    | Repetition of PolynomialSymbol*string

    /// [ a b c ]
    | Oneof of PolynomialSymbol list

    /// (a b c)
    | Chain of PolynomialSymbol list

open FslexFsyacc.Runtime.RenderUtils

module PolynomialSymbol =
    let rec render poly =
        match poly with
        | Atomic x -> renderSymbol x

        | Repetition(p,q) -> $"{render p}{q}"

        | Oneof ls -> 
            let content =
                ls 
                |> List.map render
                |> String.concat " "
            $"[{content}]"
        | Chain ls -> 
            let content =
                ls 
                |> List.map render
                |> String.concat " "
            $"({content})"

    /// 转换表的符号: 设计可以反向解析的格式，符号的内部表达
    let innerSymbol poly =
        match poly with
        | Atomic x -> x
        | _ -> "{" + render poly + "}"
                
