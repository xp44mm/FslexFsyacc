module Program 

open System.IO
open FslexFsyacc.Fsyacc
open System
open FSharp.Literals
open FslexFsyacc.Yacc

let t = set[
    ["expr";"-";"expr"];
    ["expr";"expr";"*";"expr"];
    ["expr";"expr";"+";"expr"];
    ["expr";"expr";"-";"expr"];
    ["expr";"expr";"/";"expr"]]

let x = set[
    ["expr";"-";"expr"];
    ["expr";"expr";"*";"expr"];
    ["expr";"expr";"+";"expr"];
    ["expr";"expr";"-";"expr"];
    ["expr";"expr";"/";"expr"]]

let y = [|
    ["expr";"-";"expr"],"prec terminal conflicted.";
    ["expr";"expr";"*";"expr"],"*";
    ["expr";"expr";"+";"expr"],"+";
    ["expr";"expr";"-";"expr"],"prec terminal conflicted.";
    ["expr";"expr";"/";"expr"],"/"|]


let [<EntryPoint>] main _ = 
    0
