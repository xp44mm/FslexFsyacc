module FslexFsyacc.Fsyacc.FlatFsyaccFileUtils

open System
open System.Text
open System.Text.RegularExpressions
open System.IO

open FSharp.Idioms
open FSharp.Literals.Literal

open FslexFsyacc.Yacc

let parse text =
        text
        |> Fsyacc2Compiler.compile
        |> RawFsyaccFile2Utils.flat

let toGrammar (fsyacc:FlatFsyaccFile) =
    fsyacc.rules
    |> FsyaccFileRules.getMainProductions
    |> Grammar.from

let toAmbiguousCollection (fsyacc:FlatFsyaccFile) =
    fsyacc.rules
    |> FsyaccFileRules.getMainProductions
    |> AmbiguousCollection.create

let toFsyaccParseTableFile (this:FlatFsyaccFile) =
        let mainProductions = FsyaccFileRules.getMainProductions this.rules
        let productionNames = FsyaccFileRules.getProductionNames this.rules
        let parseTable =
            ParseTable.create(
                mainProductions,
                productionNames,
                this.precedences)
        {
            header = this.header
            rules = this.rules |> List.map Triple.ends
            actions = parseTable.actions
            declarations = this.declarations
            closures = parseTable.closures
        }
