namespace FslexFsyacc.Fsyacc

open System

open FSharp.Idioms
open FSharp.Literals

type FsyaccParseTableFile2 =
    {
        header: string

        //productions: Map<int, string list>
        productions  : (int*string [])[]
        //actions: Map<int, Map<string, int>>
        actions      : (int*(string*int)[])[]
        //kernelSymbols: Map<int, string>
        kernelSymbols: (int*string)[]
        //mappers      : (int*(obj[]->obj))[]

        semantics: (int*string)[] // Map<int, string>
        declarations:(string*string)[] //(string*string)list
    }

    /// 输入模块带名字空间的全名
    member this.generate(moduleName:string) =
        let types = Map.ofArray this.declarations // symbol -> type of symbol
        let prods = this.productions |> Map.ofArray
        let semantics =
            this.semantics
            |> Array.map
                (fun (i, semantic) ->
                    let prod = prods.[i] |> List.ofArray
                    i,SemanticGenerator.decorateSemantic types prod semantic)

        let mappers =
            [
                for i, fn in semantics do
                    yield $"    {i},{fn}"
            ]
            |> String.concat Environment.NewLine
            |> sprintf "[|\r\n%s|]"
        let startSymbol = prods.[0].[1] // 增广产生式是：s' -> s，其中s是开始符号
        //解析表数据
        let result =
            [
                $"module {moduleName}"
                $"let header = {Literal.stringify this.header}"
                $"let productions = {Literal.stringify this.productions}"
                $"let actions = {Literal.stringify this.actions}"
                $"let kernelSymbols = {Literal.stringify this.kernelSymbols}"
                $"let semantics = {Literal.stringify this.semantics}"
                $"let declarations = {Literal.stringify this.declarations}"
                this.header
                $"let mappers:(int*(obj[]->obj))[] = {mappers}"
                "open FslexFsyacc.Runtime"
                "let parser = Parser2(productions, actions, kernelSymbols, mappers)"
                "let parse (tokens:seq<_>) ="
                "    parser.parse(tokens, getTag, getLexeme)"
                $"    |> unbox<{types.[startSymbol]}>"
            ] |> String.concat Environment.NewLine
        result

