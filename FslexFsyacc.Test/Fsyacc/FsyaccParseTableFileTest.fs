namespace FslexFsyacc.Fsyacc

open Xunit
open Xunit.Abstractions

open System.IO
open System.Text.RegularExpressions

open FSharp.xUnit
open FSharp.Literals

open FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc

type FsyaccParseTableFileTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    let src = {
        rules= [["file";"HEADER";"rules";"tailPercent"],"s0, List.rev s1,[],[]";["file";"HEADER";"rules";"%%";"precedences";"tailPercent"],"s0, List.rev s1,List.rev s3,[]";["file";"HEADER";"rules";"%%";"declarations";"tailPercent"],"s0, List.rev s1,[],List.rev s3";["file";"HEADER";"rules";"%%";"precedences";"%%";"declarations";"tailPercent"],"s0, List.rev s1,List.rev s3,List.rev s5";["tailPercent"],"";["tailPercent";"%%"],"";["rules";"rule"],"[s0]";["rules";"rules";"rule"],"s1::s0";["rule";"ID";":";"headBar";"bodies"],"s0,List.rev s3";["headBar"],"";["headBar";"|"],"";["precedences";"precedence"],"[s0]";["precedences";"precedences";"precedence"],"s1::s0";["precedence";"assoc";"symbols"],"s0,List.rev s1";["bodies";"body"],"[s0]";["bodies";"bodies";"|";"body"],"s2::s0";["body";"SEMANTIC"],"[],\"\",s0";["body";"symbols";"SEMANTIC"],"List.rev s0,\"\",s1";["body";"symbols";"%prec";"ID";"SEMANTIC"],"List.rev s0,s2,s3";["symbols";"symbol"],"[s0]";["symbols";"symbols";"symbol"],"s1::s0";["symbol";"ID"],"s0";["symbol";"QUOTE"],"s0";["assoc";"%left"],"\"left\"";["assoc";"%right"],"\"right\"";["assoc";"%nonassoc"],"\"nonassoc\"";["declarations";"declaration"],"[s0]";["declarations";"declarations";"declaration"],"s1::s0";["declaration";"symbol";":";"symbol"],"s0,s2.Trim()"];
        actions= [["HEADER",22;"file",1];["",0];["ID",-1;"QUOTE",-1];["ID",-2;"QUOTE",-2];["ID",-3;"QUOTE",-3];["",-21;"%%",-21;"ID",-21;"|",6];["ID",41;"QUOTE",42;"SEMANTIC",9;"body",7;"symbol",43;"symbols",10];["",-4;"%%",-4;"ID",-4;"|",-4];["",-5;"%%",-5;"ID",-5;"|",-5];["",-6;"%%",-6;"ID",-6;"|",-6];["%prec",11;"ID",41;"QUOTE",42;"SEMANTIC",14;"symbol",44];["ID",12];["SEMANTIC",13];["",-7;"%%",-7;"ID",-7;"|",-7];["",-8;"%%",-8;"ID",-8;"|",-8];[":",16];["ID",41;"QUOTE",42;"symbol",17];["",-9;"%%",-9;"ID",-9;"QUOTE",-9];["",-10;"%%",-10;"ID",-10;"QUOTE",-10];["",-28;"%%",45;"ID",41;"QUOTE",42;"declaration",21;"symbol",15;"tailPercent",25];["",-28;"%%",45;"ID",41;"QUOTE",42;"declaration",21;"symbol",15;"tailPercent",28];["",-11;"%%",-11;"ID",-11;"QUOTE",-11];["ID",36;"rule",39;"rules",23];["",-28;"%%",24;"ID",36;"rule",40;"tailPercent",30];["",-29;"%left",2;"%nonassoc",3;"%right",4;"ID",41;"QUOTE",42;"assoc",32;"declaration",18;"declarations",19;"precedence",34;"precedences",26;"symbol",15];["",-12];["",-28;"%%",27;"%left",2;"%nonassoc",3;"%right",4;"assoc",32;"precedence",35;"tailPercent",29];["",-29;"ID",41;"QUOTE",42;"declaration",18;"declarations",20;"symbol",15];["",-13];["",-14];["",-15];["ID",-17;"QUOTE",-17;"SEMANTIC",-17];["ID",41;"QUOTE",42;"symbol",43;"symbols",33];["",-18;"%%",-18;"%left",-18;"%nonassoc",-18;"%right",-18;"ID",41;"QUOTE",42;"symbol",44];["",-19;"%%",-19;"%left",-19;"%nonassoc",-19;"%right",-19];["",-20;"%%",-20;"%left",-20;"%nonassoc",-20;"%right",-20];[":",37];["ID",-16;"QUOTE",-16;"SEMANTIC",-16;"headBar",38;"|",31];["ID",41;"QUOTE",42;"SEMANTIC",9;"bodies",5;"body",8;"symbol",43;"symbols",10];["",-22;"%%",-22;"ID",-22];["",-23;"%%",-23;"ID",-23];["",-24;"%%",-24;"%left",-24;"%nonassoc",-24;"%prec",-24;"%right",-24;":",-24;"ID",-24;"QUOTE",-24;"SEMANTIC",-24];["",-25;"%%",-25;"%left",-25;"%nonassoc",-25;"%prec",-25;"%right",-25;":",-25;"ID",-25;"QUOTE",-25;"SEMANTIC",-25];["",-26;"%%",-26;"%left",-26;"%nonassoc",-26;"%prec",-26;"%right",-26;"ID",-26;"QUOTE",-26;"SEMANTIC",-26];["",-27;"%%",-27;"%left",-27;"%nonassoc",-27;"%prec",-27;"%right",-27;"ID",-27;"QUOTE",-27;"SEMANTIC",-27];["",-29]];
        closures= [[0,0,[];-12,0,[];-13,0,[];-14,0,[];-15,0,[]];[0,1,[""]];[-1,1,["ID";"QUOTE"]];[-2,1,["ID";"QUOTE"]];[-3,1,["ID";"QUOTE"]];[-4,1,[];-21,4,["";"%%";"ID"]];[-4,2,[];-6,0,[];-7,0,[];-8,0,[];-24,0,[];-25,0,[];-26,0,[];-27,0,[]];[-4,3,["";"%%";"ID";"|"]];[-5,1,["";"%%";"ID";"|"]];[-6,1,["";"%%";"ID";"|"]];[-7,1,[];-8,1,[];-24,0,[];-25,0,[];-27,1,[]];[-7,2,[]];[-7,3,[]];[-7,4,["";"%%";"ID";"|"]];[-8,2,["";"%%";"ID";"|"]];[-9,1,[]];[-9,2,[];-24,0,[];-25,0,[]];[-9,3,["";"%%";"ID";"QUOTE"]];[-10,1,["";"%%";"ID";"QUOTE"]];[-9,0,[];-11,1,[];-12,4,[];-24,0,[];-25,0,[];-28,0,[""];-29,0,[]];[-9,0,[];-11,1,[];-13,6,[];-24,0,[];-25,0,[];-28,0,[""];-29,0,[]];[-11,2,["";"%%";"ID";"QUOTE"]];[-12,1,[];-13,1,[];-14,1,[];-15,1,[];-21,0,[];-22,0,[];-23,0,[]];[-12,2,[];-13,2,[];-14,2,[];-15,2,[];-21,0,[];-23,1,[];-28,0,[""];-29,0,[]];[-1,0,[];-2,0,[];-3,0,[];-9,0,[];-10,0,[];-11,0,[];-12,3,[];-13,3,[];-14,3,[];-18,0,[];-19,0,[];-20,0,[];-24,0,[];-25,0,[];-29,1,[""]];[-12,5,[""]];[-1,0,[];-2,0,[];-3,0,[];-13,4,[];-14,4,[];-18,0,[];-20,1,[];-28,0,[""];-29,0,[]];[-9,0,[];-10,0,[];-11,0,[];-13,5,[];-24,0,[];-25,0,[];-29,1,[""]];[-13,7,[""]];[-14,5,[""]];[-15,3,[""]];[-17,1,["ID";"QUOTE";"SEMANTIC"]];[-18,1,[];-24,0,[];-25,0,[];-26,0,[];-27,0,[]];[-18,2,["";"%%";"%left";"%nonassoc";"%right"];-24,0,[];-25,0,[];-27,1,[]];[-19,1,["";"%%";"%left";"%nonassoc";"%right"]];[-20,2,["";"%%";"%left";"%nonassoc";"%right"]];[-21,1,[]];[-16,0,["ID";"QUOTE";"SEMANTIC"];-17,0,[];-21,2,[]];[-4,0,[];-5,0,[];-6,0,[];-7,0,[];-8,0,[];-21,3,[];-24,0,[];-25,0,[];-26,0,[];-27,0,[]];[-22,1,["";"%%";"ID"]];[-23,2,["";"%%";"ID"]];[-24,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";":";"ID";"QUOTE";"SEMANTIC"]];[-25,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";":";"ID";"QUOTE";"SEMANTIC"]];[-26,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"ID";"QUOTE";"SEMANTIC"]];[-27,2,["";"%%";"%left";"%nonassoc";"%prec";"%right";"ID";"QUOTE";"SEMANTIC"]];[-29,1,[""]]];
        header= "open FslexFsyacc.Fsyacc\r\nopen FslexFsyacc.Fsyacc.FsyaccTokenUtils\r\ntype token = int*int*FsyaccToken";
        declarations= ["HEADER","string";"ID","string";"QUOTE","string";"SEMANTIC","string";"assoc","string";"bodies","(string list*string*string)list";"body","string list*string*string";"declaration","string*string";"declarations","(string*string)list";"file","string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list";"precedence","string*string list";"precedences","(string*string list)list";"rule","string*((string list*string*string)list)";"rules","(string*((string list*string*string)list))list";"symbol","string";"symbols","string list"]}


    [<Fact>]
    member _.``1 - generateMappers test``() =
        let mappers = src.generateMappers()
        output.WriteLine(mappers)


