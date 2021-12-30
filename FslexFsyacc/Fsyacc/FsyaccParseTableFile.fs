namespace FslexFsyacc.Fsyacc

open System

open FSharp.Idioms
open FSharp.Literals

type FsyaccParseTableFile =
    { 
        header: string
        productions: Map<int, string list>
        actions: Map<int, Map<string, int>>
        kernelSymbols: Map<int, string>
        semantics: Map<int, string> 
        declarations:(string*string)list
    }

    /// 输入模块带名字空间的全名
    member this.generate(moduleName:string) =
        let types = Map.ofList this.declarations // symbol -> type of symbol
        let semantics =
            this.semantics
            |> Map.map
                (fun i semantic ->
                    let prod = this.productions.[i]
                    SemanticGenerator.decorateSemantic types prod semantic)
            |> Map.toArray

        let mappers =
            [ 
                for i, fn in semantics do
                    yield $"    {i},{fn}" 
            ]
            |> String.concat Environment.NewLine
            |> sprintf "Map [|\r\n%s|]"
        let startSymbol = this.productions.[0].Tail.Head
        //解析表数据
        let result =
            [ 
                $"module {moduleName}"
                $"let header = {Literal.stringify this.header}"
                $"let productions = {Literal.stringify this.productions}"
                $"let actions = {Literal.stringify this.actions}"
                $"let kernelSymbols = {Literal.stringify this.kernelSymbols}"
                $"let semantics = {Literal.stringify this.semantics}"
                $"let declarations = List.ofArray {Literal.stringify(List.toArray this.declarations)}"
                this.header
                $"let mappers:Map<int,(obj[]->obj)> = {mappers}"
                "open FslexFsyacc.Runtime"
                "let parser = Parser(productions, actions, kernelSymbols, mappers)"
                "let parse (tokens:seq<_>) ="
                "    parser.parse(tokens, getTag, getLexeme)"
                $"    |> unbox<{types.[startSymbol]}>"
            ]
            |> String.concat Environment.NewLine
        result

