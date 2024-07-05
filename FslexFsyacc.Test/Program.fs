module FslexFsyacc.Program

open FSharp.Idioms
open FSharp.Idioms.Literal

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

open FslexFsyacc.ModuleOrNamespaces
open FslexFsyacc.TypeArguments

;      type 
       PositionWith<
   'value
  > 
 =
  { index : int list 
          list;       length: int
          list
    value 
         : 'value
    }


[<EntryPoint>]
let main _ =
    let exit (rest:string) = Regex.IsMatch(rest, @"^\s*\>")

    let x = "Band list"
    let src = SourceText.just(99, $"{x}>")

    //TypeArgumentTokenUtils.tokenize src
    //|> Seq.iter(fun tok ->
    //    Console.WriteLine(stringify tok)
    
    //)
    let ta,length = TypeArgumentCompiler.compile exit src
    Console.WriteLine(stringify ta)
    Console.WriteLine($"length={length}")


    0
