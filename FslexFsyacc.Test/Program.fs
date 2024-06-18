module FslexFsyacc.Program

open FSharp.Idioms
open FSharp.Idioms.Literal
open FSharp.xUnit

open FslexFsyacc
open FslexFsyacc.Brackets
open FslexFsyacc.Expr
open FslexFsyacc.Fsyacc
open FslexFsyacc.ItemCores
open FslexFsyacc.Lex
open FslexFsyacc.Precedences
open FslexFsyacc.VanillaFSharp
open FslexFsyacc.Yacc

open System
open System.Reflection
open System.Text.RegularExpressions

open Xunit
open Xunit.Abstractions
open FslexFsyacc.ModuleOrNamespaces

//let x = [
//    {index=9;length=4;value=KW_OPEN};
//    {index=14;length=6;value=IDENT "System"};
//    {index=20;length=1;value=DOT};{index=21;length=2;value=IDENT "IO"};
//    {index=25;length=4;value=KW_OPEN};{index=30;length=4;value=KW_TYPE};
//    {index=34;length=13;value=TYPE_ARGUMENT(Ctor(["System";"Math"],[]))}
//    ]


[<EntryPoint>]
let main _ =
    Console.WriteLine("")

    0
