module FslexFsyacc.Program 

open System
open System.IO

open FSharp.Literals.Literal
open FslexFsyacc.Yacc

let j = set [{production= ["expr";"expr";"*";"expr"];dot= 1}] //not in 
let mp = Map [
    set [{production= ["";"expr"];dot= 0}],0;
    set [{production= ["";"expr"];dot= 1};{production= ["expr";"expr";"*";"expr"];dot= 1};{production= ["expr";"expr";"+";"expr"];dot= 1};{production= ["expr";"expr";"-";"expr"];dot= 1};{production= ["expr";"expr";"/";"expr"];dot= 1}],1;
    set [{production= ["expr";"(";"expr";")"];dot= 1}],2;
    set [{production= ["expr";"(";"expr";")"];dot= 2};{production= ["expr";"expr";"*";"expr"];dot= 1};{production= ["expr";"expr";"+";"expr"];dot= 1};{production= ["expr";"expr";"-";"expr"];dot= 1};{production= ["expr";"expr";"/";"expr"];dot= 1}],3;
    set [{production= ["expr";"(";"expr";")"];dot= 3}],4;
    set [{production= ["expr";"-";"expr"];dot= 1}],5;
    set [{production= ["expr";"-";"expr"];dot= 2};{production= ["expr";"expr";"*";"expr"];dot= 1};{production= ["expr";"expr";"+";"expr"];dot= 1};{production= ["expr";"expr";"-";"expr"];dot= 1};{production= ["expr";"expr";"/";"expr"];dot= 1}],6;
    set [{production= ["expr";"NUMBER"];dot= 1}],7;
    set [{production= ["expr";"expr";"*";"expr"];dot= 1};{production= ["expr";"expr";"*";"expr"];dot= 3};{production= ["expr";"expr";"+";"expr"];dot= 1};{production= ["expr";"expr";"-";"expr"];dot= 1};{production= ["expr";"expr";"/";"expr"];dot= 1}],8;
    set [{production= ["expr";"expr";"*";"expr"];dot= 1};{production= ["expr";"expr";"+";"expr"];dot= 1};{production= ["expr";"expr";"+";"expr"];dot= 3};{production= ["expr";"expr";"-";"expr"];dot= 1};{production= ["expr";"expr";"/";"expr"];dot= 1}],9;
    set [{production= ["expr";"expr";"*";"expr"];dot= 1};{production= ["expr";"expr";"+";"expr"];dot= 1};{production= ["expr";"expr";"-";"expr"];dot= 1};{production= ["expr";"expr";"-";"expr"];dot= 3};{production= ["expr";"expr";"/";"expr"];dot= 1}],10;
    set [{production= ["expr";"expr";"*";"expr"];dot= 1};{production= ["expr";"expr";"+";"expr"];dot= 1};{production= ["expr";"expr";"-";"expr"];dot= 1};{production= ["expr";"expr";"/";"expr"];dot= 1};{production= ["expr";"expr";"/";"expr"];dot= 3}],11;
    set [{production= ["expr";"expr";"*";"expr"];dot= 2}],12;
    set [{production= ["expr";"expr";"+";"expr"];dot= 2}],13;
    set [{production= ["expr";"expr";"-";"expr"];dot= 2}],14;
    set [{production= ["expr";"expr";"/";"expr"];dot= 2}],15]
let [<EntryPoint>] main _ = 

    Console.WriteLine(stringify "")
    0
