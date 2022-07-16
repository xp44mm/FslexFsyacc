module FslexFsyacc.Fsyacc.FsyaccParseTable
let actions = [|[|"HEADER",21;"file",1|];[|"",0|];[|"ID",-1;"QUOTE",-1|];[|"ID",-2;"QUOTE",-2|];[|"ID",-3;"QUOTE",-3|];[|"",-22;"%%",-22;"ID",-22;"|",6|];[|"%prec",-17;"ID",41;"QUOTE",42;"SEMANTIC",-17;"body",7;"nullableSymbols",9;"symbol",43;"symbols",31|];[|"",-4;"%%",-4;"ID",-4;"|",-4|];[|"",-5;"%%",-5;"ID",-5;"|",-5|];[|"%prec",10;"SEMANTIC",13|];[|"ID",11|];[|"SEMANTIC",12|];[|"",-6;"%%",-6;"ID",-6;"|",-6|];[|"",-7;"%%",-7;"ID",-7;"|",-7|];[|":",15|];[|"ID",41;"QUOTE",42;"symbol",16|];[|"",-8;"%%",-8;"ID",-8;"QUOTE",-8|];[|"",-9;"%%",-9;"ID",-9;"QUOTE",-9|];[|"",-29;"%%",45;"ID",41;"QUOTE",42;"declaration",20;"symbol",14;"tailPercent",24|];[|"",-29;"%%",45;"ID",41;"QUOTE",42;"declaration",20;"symbol",14;"tailPercent",27|];[|"",-10;"%%",-10;"ID",-10;"QUOTE",-10|];[|"ID",36;"rule",39;"rules",22|];[|"",-29;"%%",23;"ID",36;"rule",40;"tailPercent",29|];[|"",-30;"%left",2;"%nonassoc",3;"%right",4;"ID",41;"QUOTE",42;"assoc",32;"declaration",17;"declarations",18;"precedence",34;"precedences",25;"symbol",14|];[|"",-11|];[|"",-29;"%%",26;"%left",2;"%nonassoc",3;"%right",4;"assoc",32;"precedence",35;"tailPercent",28|];[|"",-30;"ID",41;"QUOTE",42;"declaration",17;"declarations",19;"symbol",14|];[|"",-12|];[|"",-13|];[|"",-14|];[|"%prec",-16;"ID",-16;"QUOTE",-16;"SEMANTIC",-16|];[|"%prec",-18;"ID",41;"QUOTE",42;"SEMANTIC",-18;"symbol",44|];[|"ID",41;"QUOTE",42;"symbol",43;"symbols",33|];[|"",-19;"%%",-19;"%left",-19;"%nonassoc",-19;"%right",-19;"ID",41;"QUOTE",42;"symbol",44|];[|"",-20;"%%",-20;"%left",-20;"%nonassoc",-20;"%right",-20|];[|"",-21;"%%",-21;"%left",-21;"%nonassoc",-21;"%right",-21|];[|":",37|];[|"%prec",-15;"ID",-15;"QUOTE",-15;"SEMANTIC",-15;"headBar",38;"|",30|];[|"%prec",-17;"ID",41;"QUOTE",42;"SEMANTIC",-17;"bodies",5;"body",8;"nullableSymbols",9;"symbol",43;"symbols",31|];[|"",-23;"%%",-23;"ID",-23|];[|"",-24;"%%",-24;"ID",-24|];[|"",-25;"%%",-25;"%left",-25;"%nonassoc",-25;"%prec",-25;"%right",-25;":",-25;"ID",-25;"QUOTE",-25;"SEMANTIC",-25|];[|"",-26;"%%",-26;"%left",-26;"%nonassoc",-26;"%prec",-26;"%right",-26;":",-26;"ID",-26;"QUOTE",-26;"SEMANTIC",-26|];[|"",-27;"%%",-27;"%left",-27;"%nonassoc",-27;"%prec",-27;"%right",-27;"ID",-27;"QUOTE",-27;"SEMANTIC",-27|];[|"",-28;"%%",-28;"%left",-28;"%nonassoc",-28;"%prec",-28;"%right",-28;"ID",-28;"QUOTE",-28;"SEMANTIC",-28|];[|"",-30|]|]
let closures = [|[|0,0,[||];-11,0,[||];-12,0,[||];-13,0,[||];-14,0,[||]|];[|0,1,[|""|]|];[|-1,1,[|"ID";"QUOTE"|]|];[|-2,1,[|"ID";"QUOTE"|]|];[|-3,1,[|"ID";"QUOTE"|]|];[|-4,1,[||];-22,4,[|"";"%%";"ID"|]|];[|-4,2,[||];-6,0,[||];-7,0,[||];-17,0,[|"%prec";"SEMANTIC"|];-18,0,[||];-25,0,[||];-26,0,[||];-27,0,[||];-28,0,[||]|];[|-4,3,[|"";"%%";"ID";"|"|]|];[|-5,1,[|"";"%%";"ID";"|"|]|];[|-6,1,[||];-7,1,[||]|];[|-6,2,[||]|];[|-6,3,[||]|];[|-6,4,[|"";"%%";"ID";"|"|]|];[|-7,2,[|"";"%%";"ID";"|"|]|];[|-8,1,[||]|];[|-8,2,[||];-25,0,[||];-26,0,[||]|];[|-8,3,[|"";"%%";"ID";"QUOTE"|]|];[|-9,1,[|"";"%%";"ID";"QUOTE"|]|];[|-8,0,[||];-10,1,[||];-11,4,[||];-25,0,[||];-26,0,[||];-29,0,[|""|];-30,0,[||]|];[|-8,0,[||];-10,1,[||];-12,6,[||];-25,0,[||];-26,0,[||];-29,0,[|""|];-30,0,[||]|];[|-10,2,[|"";"%%";"ID";"QUOTE"|]|];[|-11,1,[||];-12,1,[||];-13,1,[||];-14,1,[||];-22,0,[||];-23,0,[||];-24,0,[||]|];[|-11,2,[||];-12,2,[||];-13,2,[||];-14,2,[||];-22,0,[||];-24,1,[||];-29,0,[|""|];-30,0,[||]|];[|-1,0,[||];-2,0,[||];-3,0,[||];-8,0,[||];-9,0,[||];-10,0,[||];-11,3,[||];-12,3,[||];-13,3,[||];-19,0,[||];-20,0,[||];-21,0,[||];-25,0,[||];-26,0,[||];-30,1,[|""|]|];[|-11,5,[|""|]|];[|-1,0,[||];-2,0,[||];-3,0,[||];-12,4,[||];-13,4,[||];-19,0,[||];-21,1,[||];-29,0,[|""|];-30,0,[||]|];[|-8,0,[||];-9,0,[||];-10,0,[||];-12,5,[||];-25,0,[||];-26,0,[||];-30,1,[|""|]|];[|-12,7,[|""|]|];[|-13,5,[|""|]|];[|-14,3,[|""|]|];[|-16,1,[|"%prec";"ID";"QUOTE";"SEMANTIC"|]|];[|-18,1,[|"%prec";"SEMANTIC"|];-25,0,[||];-26,0,[||];-28,1,[||]|];[|-19,1,[||];-25,0,[||];-26,0,[||];-27,0,[||];-28,0,[||]|];[|-19,2,[|"";"%%";"%left";"%nonassoc";"%right"|];-25,0,[||];-26,0,[||];-28,1,[||]|];[|-20,1,[|"";"%%";"%left";"%nonassoc";"%right"|]|];[|-21,2,[|"";"%%";"%left";"%nonassoc";"%right"|]|];[|-22,1,[||]|];[|-15,0,[|"%prec";"ID";"QUOTE";"SEMANTIC"|];-16,0,[||];-22,2,[||]|];[|-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-17,0,[|"%prec";"SEMANTIC"|];-18,0,[||];-22,3,[||];-25,0,[||];-26,0,[||];-27,0,[||];-28,0,[||]|];[|-23,1,[|"";"%%";"ID"|]|];[|-24,2,[|"";"%%";"ID"|]|];[|-25,1,[|"";"%%";"%left";"%nonassoc";"%prec";"%right";":";"ID";"QUOTE";"SEMANTIC"|]|];[|-26,1,[|"";"%%";"%left";"%nonassoc";"%prec";"%right";":";"ID";"QUOTE";"SEMANTIC"|]|];[|-27,1,[|"";"%%";"%left";"%nonassoc";"%prec";"%right";"ID";"QUOTE";"SEMANTIC"|]|];[|-28,2,[|"";"%%";"%left";"%nonassoc";"%prec";"%right";"ID";"QUOTE";"SEMANTIC"|]|];[|-30,1,[|""|]|]|]
open FslexFsyacc.Runtime
open FslexFsyacc.Fsyacc
open FslexFsyacc.Fsyacc.FsyaccTokenUtils
type token = int*int*FsyaccToken
let rules:(string list*(obj[]->obj))[] = [|
    ["file";"HEADER";"rules";"tailPercent"],fun (ss:obj[]) ->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(string*((string list*string*string)list))list> ss.[1]
        let result:string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list =
            s0, List.rev s1,[],[]
        box result
    ["file";"HEADER";"rules";"%%";"precedences";"tailPercent"],fun (ss:obj[]) ->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(string*((string list*string*string)list))list> ss.[1]
        let s3 = unbox<(string*string list)list> ss.[3]
        let result:string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list =
            s0, List.rev s1,List.rev s3,[]
        box result
    ["file";"HEADER";"rules";"%%";"declarations";"tailPercent"],fun (ss:obj[]) ->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(string*((string list*string*string)list))list> ss.[1]
        let s3 = unbox<(string*string)list> ss.[3]
        let result:string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list =
            s0, List.rev s1,[],List.rev s3
        box result
    ["file";"HEADER";"rules";"%%";"precedences";"%%";"declarations";"tailPercent"],fun (ss:obj[]) ->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(string*((string list*string*string)list))list> ss.[1]
        let s3 = unbox<(string*string list)list> ss.[3]
        let s5 = unbox<(string*string)list> ss.[5]
        let result:string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list =
            s0, List.rev s1,List.rev s3,List.rev s5
        box result
    ["tailPercent"],fun (ss:obj[]) ->
        null
    ["tailPercent";"%%"],fun (ss:obj[]) ->
        null
    ["rules";"rule"],fun (ss:obj[]) ->
        let s0 = unbox<string*((string list*string*string)list)> ss.[0]
        let result:(string*((string list*string*string)list))list =
            [s0]
        box result
    ["rules";"rules";"rule"],fun (ss:obj[]) ->
        let s0 = unbox<(string*((string list*string*string)list))list> ss.[0]
        let s1 = unbox<string*((string list*string*string)list)> ss.[1]
        let result:(string*((string list*string*string)list))list =
            s1::s0
        box result
    ["rule";"ID";":";"headBar";"bodies"],fun (ss:obj[]) ->
        let s0 = unbox<string> ss.[0]
        let s3 = unbox<(string list*string*string)list> ss.[3]
        let result:string*((string list*string*string)list) =
            s0,List.rev s3
        box result
    ["headBar"],fun (ss:obj[]) ->
        null
    ["headBar";"|"],fun (ss:obj[]) ->
        null
    ["precedences";"precedence"],fun (ss:obj[]) ->
        let s0 = unbox<string*string list> ss.[0]
        let result:(string*string list)list =
            [s0]
        box result
    ["precedences";"precedences";"precedence"],fun (ss:obj[]) ->
        let s0 = unbox<(string*string list)list> ss.[0]
        let s1 = unbox<string*string list> ss.[1]
        let result:(string*string list)list =
            s1::s0
        box result
    ["precedence";"assoc";"symbols"],fun (ss:obj[]) ->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<string list> ss.[1]
        let result:string*string list =
            s0,List.rev s1
        box result
    ["bodies";"body"],fun (ss:obj[]) ->
        let s0 = unbox<string list*string*string> ss.[0]
        let result:(string list*string*string)list =
            [s0]
        box result
    ["bodies";"bodies";"|";"body"],fun (ss:obj[]) ->
        let s0 = unbox<(string list*string*string)list> ss.[0]
        let s2 = unbox<string list*string*string> ss.[2]
        let result:(string list*string*string)list =
            s2::s0
        box result
    ["body";"nullableSymbols";"SEMANTIC"],fun (ss:obj[]) ->
        let s0 = unbox<string list> ss.[0]
        let s1 = unbox<string> ss.[1]
        let result:string list*string*string =
            List.rev s0,"",s1
        box result
    ["body";"nullableSymbols";"%prec";"ID";"SEMANTIC"],fun (ss:obj[]) ->
        let s0 = unbox<string list> ss.[0]
        let s2 = unbox<string> ss.[2]
        let s3 = unbox<string> ss.[3]
        let result:string list*string*string =
            List.rev s0,s2,s3
        box result
    ["nullableSymbols"],fun (ss:obj[]) ->
        let result:string list =
            []
        box result
    ["nullableSymbols";"symbols"],fun (ss:obj[]) ->
        let s0 = unbox<string list> ss.[0]
        let result:string list =
            s0
        box result
    ["symbols";"symbol"],fun (ss:obj[]) ->
        let s0 = unbox<string> ss.[0]
        let result:string list =
            [s0]
        box result
    ["symbols";"symbols";"symbol"],fun (ss:obj[]) ->
        let s0 = unbox<string list> ss.[0]
        let s1 = unbox<string> ss.[1]
        let result:string list =
            s1::s0
        box result
    ["symbol";"ID"],fun (ss:obj[]) ->
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    ["symbol";"QUOTE"],fun (ss:obj[]) ->
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    ["assoc";"%left"],fun (ss:obj[]) ->
        let result:string =
            "left"
        box result
    ["assoc";"%right"],fun (ss:obj[]) ->
        let result:string =
            "right"
        box result
    ["assoc";"%nonassoc"],fun (ss:obj[]) ->
        let result:string =
            "nonassoc"
        box result
    ["declarations";"declaration"],fun (ss:obj[]) ->
        let s0 = unbox<string*string> ss.[0]
        let result:(string*string)list =
            [s0]
        box result
    ["declarations";"declarations";"declaration"],fun (ss:obj[]) ->
        let s0 = unbox<(string*string)list> ss.[0]
        let s1 = unbox<string*string> ss.[1]
        let result:(string*string)list =
            s1::s0
        box result
    ["declaration";"symbol";":";"symbol"],fun (ss:obj[]) ->
        let s0 = unbox<string> ss.[0]
        let s2 = unbox<string> ss.[2]
        let result:string*string =
            s0,s2.Trim()
        box result
|]
let parser = Parser<token>(rules,actions,closures,getTag,getLexeme)
let parse(tokens:seq<token>) =
    tokens
    |> parser.parse
    |> unbox<string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list>