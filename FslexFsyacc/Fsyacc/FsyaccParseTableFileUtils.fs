module FslexFsyacc.Fsyacc.FsyaccParseTableFileUtils

open System
open FSharp.Idioms
open FslexFsyacc.Runtime
open FslexFsyacc.Yacc

let ofSemanticParseTableCrew (crew:SemanticParseTableCrew) = 
    id<FsyaccParseTableFile> {
        header = crew.header
        rules = crew.rules
        actions = crew.encodedActions
        closures = crew.encodedClosures
        declarations = crew.declarations
    }

///闭包就是状态
let printClosures (this:FsyaccParseTableFile) =
    let startSymbol = (fst this.rules.[0]).[0]

    let rules =
        [
            yield (["";startSymbol], "")
            yield! this.rules
        ]
        |> List.sortBy fst
        |> List.mapi(fun i entry ->(-i, entry))
        |> Map.ofList

    let closures =
        this.closures
        |> List.mapi(fun state closure ->
            let closure =
                closure
                |> List.map(fun(prod,dot,las) ->
                    let prod = fst rules.[prod]
                    prod,dot,las
                )
            state,closure
        )
    //打印
    closures
    |> List.map(fun(state,closure)->
        $"state {state}:\r\n{RenderUtils.renderClosure closure}"
    )
    |> String.concat "\r\n"

/// 生成 ParseTable Module
/// 输入模块带名字空间的全名
/// 删除Parse代码
let generateModule (moduleName:string) (this:FsyaccParseTableFile) =
    let types = this.declarations 
        
    let rules =
        this.rules
        |> List.map(fun(prod, semantic) ->
            let mapper = SemanticGenerator.decorateSemantic types prod semantic
            $"{Literal.stringify prod},{mapper}"
            )
        |> String.concat Environment.NewLine

    // 第0项是增广产生式，它是：s' -> s，其中第1项s是开始符号
    let startSymbol = (fst this.rules.[0]).[0]

    if types.ContainsKey startSymbol then
        ()
    else 
        failwith $"type annot `{startSymbol}` is required."

    //解析表数据
    let result =
        [
            $"module {moduleName}"
            $"let actions = {Literal.stringify this.actions}"
            $"let closures = {Literal.stringify this.closures}"
            this.header
            $"let rules:(string list*(obj list->obj))list = ["
            rules |> Line.indentCodeBlock 4
            "]"
            "let unboxRoot ="
            $"    unbox<{types.[startSymbol]}>"
            "let theoryParser = FslexFsyacc.Runtime.TheoryParser.create(rules, actions, closures)"
            "let stateSymbolPairs = theoryParser.getStateSymbolPairs()"
        ] |> String.concat "\r\n"
    result

/// 单独生成action code的源代码module
let generateMappers (this:FsyaccParseTableFile) =
    let types = this.declarations // symbol -> type of symbol

    this.rules
    |> List.map(fun(prod, semantic) ->
        SemanticGenerator.decorateSemantic types prod semantic
        )
    |> String.concat Environment.NewLine
