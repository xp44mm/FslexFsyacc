namespace FslexFsyacc.Fsyacc

//open System
//open FSharp.Idioms
//open FSharp.Literals

//type FsyaccParseTableModule =
//    {
//        productions:(string list)[]
//        closures:(int*int*string[])[][]
//        actions:(string*int)[][]

//        header: string
//        semantics: string[]
//        declarations:(string*string)[]
//    }

//    /// 输入模块带名字空间的全名
//    member this.generate(moduleName:string) =
//        let types = Map.ofArray this.declarations // symbol -> type of symbol
        
//        ///mainProductions的集合顺序和semantics是一一对应的
//        let getProdByIndex i =
//            this.productions.[i+1]

//        let prodSemanticPairs =
//            this.semantics
//            |> Array.mapi(fun i sem ->
//                let prod = getProdByIndex i
//                prod,sem
//            )

//        let mappers =
//            prodSemanticPairs
//            |> Array.map(fun(prod, semantic) ->
//                SemanticGenerator.decorateSemantic types prod semantic)

//        // 第0项是增广产生式，它是：s' -> s，其中第1项s是开始符号
//        let startSymbol = this.productions.[0].[1]
//        //解析表数据
//        let result =
//            [
//                $"module {moduleName}"
//                $"let productions = {Literal.stringify this.productions}"
//                $"let closures = {Literal.stringify this.closures}"
//                $"let actions = {Literal.stringify this.actions}"
//                $"let header = {Literal.stringify this.header}"
//                $"let semantics = {Literal.stringify this.semantics}"
//                $"let declarations = {Literal.stringify this.declarations}"
//                this.header
//                $"let mappers:(obj[]->obj)[] = [|"
//                for mpr in mappers do
//                    $"{space 4}{mpr}"
//                "|]"
//                "open FslexFsyacc.Runtime"
//                "let parser = Parser(productions, closures, actions, mappers)"
//                "let parse (tokens:seq<_>) ="
//                "    parser.parse(tokens, getTag, getLexeme)"
//                $"    |> unbox<{types.[startSymbol]}>"
//            ] |> String.concat Environment.NewLine
//        result



