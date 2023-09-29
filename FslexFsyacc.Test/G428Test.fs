namespace FslexFsyacc

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FslexFsyacc.Runtime
open FslexFsyacc.Dir

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text
open System.Text.RegularExpressions

open FSharp.xUnit
open FSharp.Literals
open FSharp.Literals.Literal

type G428Test(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    // ** input **
    let filePath = Path.Combine(__SOURCE_DIRECTORY__, @"428.fsyacc")
    let text = File.ReadAllText(filePath,Encoding.UTF8)

    // 与fsyacc文件完全相对应的结构树
    let rawFsyacc = 
        text
        |> RawFsyaccFileUtils.parse 

    let flatedFsyacc = 
        rawFsyacc 
        |> RawFsyaccFileUtils.toFlated

    let inputProductionList =
        flatedFsyacc.rules
        |> FsyaccFileRules.getMainProductions

    [<Fact>]
    member _.``01 - norm fsyacc file``() =
        let fsyacc = 
            text
            |> FlatFsyaccFileUtils.parse
        // the start symbol of bnf 
        let s0 = 
            fsyacc.rules
            |> FlatFsyaccFileRule.getStartSymbol

        let src = 
            fsyacc.start(s0, Set.empty)
            |> RawFsyaccFileUtils.fromFlat
            |> RawFsyaccFileUtils.render

        output.WriteLine(src)

    [<Fact>]
    member _.``02 - data printer``() =
        output.WriteLine($"let inputProductionList = {stringify inputProductionList}")

        let grammar = Yacc.Grammar.from inputProductionList
        output.WriteLine($"let mainProductions = {stringify grammar.mainProductions}")
        output.WriteLine($"let augmentedProductions = {stringify grammar.productions}")
        output.WriteLine($"let symbols = {stringify grammar.symbols}")
        output.WriteLine($"let nonterminals = {stringify grammar.nonterminals}")
        output.WriteLine($"let terminals = {stringify grammar.terminals}")
        output.WriteLine($"let nullables = {stringify grammar.nullables}")
        output.WriteLine($"let firsts = {stringify grammar.firsts}")
        output.WriteLine($"let lasts = {stringify grammar.lasts}")
        output.WriteLine($"let follows = {stringify grammar.follows}")
        output.WriteLine($"let precedes = {stringify grammar.precedes}")

        let itemCores = ItemCoreFactory.make grammar.productions
        output.WriteLine($"let itemCores = {stringify itemCores}")

        let itemCoreAttributes =
            ItemCoreAttributeFactory.make grammar.nonterminals grammar.nullables grammar.firsts itemCores
        output.WriteLine($"let itemCoreAttributes = {stringify itemCoreAttributes}")

        let lalrCollection = Yacc.LALRCollection.create(inputProductionList)
        let kernels = 
            lalrCollection.kernels
            |> FSharp.Idioms.Map.keys

        output.WriteLine($"let kernels = {stringify kernels}")

        let closures =
            lalrCollection.closures
            |> Map.values
            |> Seq.toList

        output.WriteLine($"let closures = {stringify closures}")

        let gotos =
            lalrCollection.getGOTOs()
            |> Map.values
            |> Seq.toList

        output.WriteLine($"let gotos = {stringify gotos}")

        let ambCollection = AmbiguousCollection.create inputProductionList

        let conflicts =
            ambCollection.conflicts
            |> Map.values
            |> Seq.toList
        output.WriteLine($"let conflicts = {stringify conflicts}")

        let parsingTable = 
            let productionNames = FsyaccFileRules.getProductionNames flatedFsyacc.rules
            
            ParsingTable.create(inputProductionList,productionNames,flatedFsyacc.precedences)

        let actions = 
            parsingTable.actions
            |> Map.values
            |> Seq.toList
        let resolvedClosures = 
            parsingTable.closures
            |> Map.values
            |> Seq.toList
        output.WriteLine($"let actions = {stringify actions}")
        output.WriteLine($"let resolvedClosures = {stringify resolvedClosures}")


