module FslexFsyacc.Program
open FslexFsyacc.VanillaFSharp

open System
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions

open FSharp.xUnit
open FSharp.Idioms.Literal
open FSharp.Idioms

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open System.Reflection
open FslexFsyacc
open FslexFsyacc.ItemCores
open FslexFsyacc.Expr
open FslexFsyacc.Precedences
open FslexFsyacc.Lex
open FslexFsyacc.TypeArguments
open FslexFsyacc.Brackets


let ls = [

    typeof<(Associativity*string list)list>,typeof<list<Associativity*list<string>>>
    typeof<(RegularExpression<string>list*string)list>,typeof<list<list<RegularExpression<string>>*string>>
    typeof<(TypeArgument*string list)list>,typeof<list<TypeArgument*list<string>>>
    typeof<(string*RegularExpression<string>)list>,typeof<list<string*RegularExpression<string>>>
    typeof<(string*TypeArgument)list>,typeof<list<string*TypeArgument>>
    typeof<(string*string list)list>,typeof<list<string*list<string>>>
    typeof<Associativity>,typeof<Associativity>
    typeof<Associativity*string list>,typeof<Associativity*list<string>>
    typeof<Band>,typeof<Band>
    typeof<Band list>,typeof<list<Band>>
    typeof<BaseOrInterfaceType>,typeof<BaseOrInterfaceType>
    typeof<RawFsyaccFile>,typeof<RawFsyaccFile>
    typeof<RegularExpression<string>>,typeof<RegularExpression<string>>
    typeof<RegularExpression<string> list>,typeof<list<RegularExpression<string>>>
    typeof<RegularExpression<string>list*string>,typeof<list<RegularExpression<string>>*string>
    typeof<RegularSymbol>,typeof<RegularSymbol>
    typeof<RegularSymbol list>,typeof<list<RegularSymbol>>
    typeof<RegularSymbol*string>,typeof<RegularSymbol*string>
    typeof<RuleBody>,typeof<RuleBody>
    typeof<RuleBody list>,typeof<list<RuleBody>>
    typeof<RuleGroup>,typeof<RuleGroup>
    typeof<RuleGroup list>,typeof<list<RuleGroup>>
    typeof<SuffixType>,typeof<SuffixType>
    typeof<SuffixType list>,typeof<list<SuffixType>>
    typeof<Typar>,typeof<Typar>
    typeof<TypeArgument>,typeof<TypeArgument>
    typeof<TypeArgument list>,typeof<list<TypeArgument>>
    typeof<TypeArgument list list>,typeof<list<list<TypeArgument>>>
    typeof<TypeArgument*string list>,typeof<TypeArgument*list<string>>
    typeof<int>,typeof<int>
    typeof<string>,typeof<string>
    typeof<string list>,typeof<list<string>>
    typeof<string list * TypeArgument list>,typeof<list<string>*list<TypeArgument>>
    typeof<string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list>,typeof<string*list<string*RegularExpression<string>>*list<list<RegularExpression<string>>*string>>
    typeof<string*RegularExpression<string>>,typeof<string*RegularExpression<string>>
    typeof<string*TypeArgument>,typeof<string*TypeArgument>
    typeof<string*string list>,typeof<string*list<string>>
]

[<EntryPoint>]
let main _ =
    let ret =
        ls
        |> List.forall(fun (a,b) -> 
            a = b
        )
    Console.WriteLine(stringify ret)

    0
