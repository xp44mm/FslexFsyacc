module FslexFsyacc.Fslex.FslexParseTable
let header = "open FslexFsyacc.Lex\r\nopen FslexFsyacc.Fslex.FslexTokenUtils"
let productions = [|0,[|"";"file"|];-1,[|"character";"ID"|];-2,[|"character";"QUOTE"|];-3,[|"characters";"character"|];-4,[|"characters";"characters";"&";"character"|];-5,[|"definition";"CAP";"=";"expr"|];-6,[|"definitions";"definition"|];-7,[|"definitions";"definitions";"definition"|];-8,[|"expr";"(";"expr";")"|];-9,[|"expr";"HOLE"|];-10,[|"expr";"[";"characters";"]"|];-11,[|"expr";"character"|];-12,[|"expr";"expr";"&";"expr"|];-13,[|"expr";"expr";"*"|];-14,[|"expr";"expr";"+"|];-15,[|"expr";"expr";"?"|];-16,[|"expr";"expr";"|";"expr"|];-17,[|"file";"HEADER";"definitions";"%%";"rules"|];-18,[|"file";"HEADER";"rules"|];-19,[|"rule";"expr";"/";"expr";"SEMANTIC"|];-20,[|"rule";"expr";"SEMANTIC"|];-21,[|"rules";"rule"|];-22,[|"rules";"rules";"rule"|]|]
let actions = [|0,[|"HEADER",30;"file",1|];1,[|"",0|];2,[|"%%",-1;"&",-1;")",-1;"*",-1;"+",-1;"/",-1;"?",-1;"CAP",-1;"SEMANTIC",-1;"]",-1;"|",-1|];3,[|"%%",-2;"&",-2;")",-2;"*",-2;"+",-2;"/",-2;"?",-2;"CAP",-2;"SEMANTIC",-2;"]",-2;"|",-2|];4,[|"&",-3;"]",-3|];5,[|"&",6;"]",19|];6,[|"ID",2;"QUOTE",3;"character",7|];7,[|"&",-4;"]",-4|];8,[|"=",9|];9,[|"(",14;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"expr",10|];10,[|"%%",-5;"&",25;"*",26;"+",27;"?",28;"CAP",-5;"|",29|];11,[|"%%",-6;"CAP",-6|];12,[|"%%",31;"CAP",8;"definition",13|];13,[|"%%",-7;"CAP",-7|];14,[|"(",14;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"expr",15|];15,[|"&",25;")",16;"*",26;"+",27;"?",28;"|",29|];16,[|"%%",-8;"&",-8;")",-8;"*",-8;"+",-8;"/",-8;"?",-8;"CAP",-8;"SEMANTIC",-8;"|",-8|];17,[|"%%",-9;"&",-9;")",-9;"*",-9;"+",-9;"/",-9;"?",-9;"CAP",-9;"SEMANTIC",-9;"|",-9|];18,[|"ID",2;"QUOTE",3;"character",4;"characters",5|];19,[|"%%",-10;"&",-10;")",-10;"*",-10;"+",-10;"/",-10;"?",-10;"CAP",-10;"SEMANTIC",-10;"|",-10|];20,[|"%%",-11;"&",-11;")",-11;"*",-11;"+",-11;"/",-11;"?",-11;"CAP",-11;"SEMANTIC",-11;"|",-11|];21,[|"%%",-12;"&",-12;")",-12;"*",26;"+",27;"/",-12;"?",28;"CAP",-12;"SEMANTIC",-12;"|",-12|];22,[|"%%",-16;"&",25;")",-16;"*",26;"+",27;"/",-16;"?",28;"CAP",-16;"SEMANTIC",-16;"|",-16|];23,[|"&",25;"*",26;"+",27;"/",34;"?",28;"SEMANTIC",36;"|",29|];24,[|"&",25;"*",26;"+",27;"?",28;"SEMANTIC",35;"|",29|];25,[|"(",14;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"expr",21|];26,[|"%%",-13;"&",-13;")",-13;"*",-13;"+",-13;"/",-13;"?",-13;"CAP",-13;"SEMANTIC",-13;"|",-13|];27,[|"%%",-14;"&",-14;")",-14;"*",-14;"+",-14;"/",-14;"?",-14;"CAP",-14;"SEMANTIC",-14;"|",-14|];28,[|"%%",-15;"&",-15;")",-15;"*",-15;"+",-15;"/",-15;"?",-15;"CAP",-15;"SEMANTIC",-15;"|",-15|];29,[|"(",14;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"expr",22|];30,[|"(",14;"CAP",8;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"definition",11;"definitions",12;"expr",23;"rule",37;"rules",33|];31,[|"(",14;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"expr",23;"rule",37;"rules",32|];32,[|"",-17;"(",14;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"expr",23;"rule",38|];33,[|"",-18;"(",14;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"expr",23;"rule",38|];34,[|"(",14;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"expr",24|];35,[|"",-19;"(",-19;"HOLE",-19;"ID",-19;"QUOTE",-19;"[",-19|];36,[|"",-20;"(",-20;"HOLE",-20;"ID",-20;"QUOTE",-20;"[",-20|];37,[|"",-21;"(",-21;"HOLE",-21;"ID",-21;"QUOTE",-21;"[",-21|];38,[|"",-22;"(",-22;"HOLE",-22;"ID",-22;"QUOTE",-22;"[",-22|]|]
let kernelSymbols = [|1,"file";2,"ID";3,"QUOTE";4,"character";5,"characters";6,"&";7,"character";8,"CAP";9,"=";10,"expr";11,"definition";12,"definitions";13,"definition";14,"(";15,"expr";16,")";17,"HOLE";18,"[";19,"]";20,"character";21,"expr";22,"expr";23,"expr";24,"expr";25,"&";26,"*";27,"+";28,"?";29,"|";30,"HEADER";31,"%%";32,"rules";33,"rules";34,"/";35,"SEMANTIC";36,"SEMANTIC";37,"rule";38,"rule"|]
let semantics = [|-1,"Character s0";-2,"Character s0";-3,"[s0]";-4,"s2::s0";-5,"s0,s2";-6,"[s0]";-7,"s1::s0";-8,"s1";-9,"Hole s0";-10,"s1 |> List.rev |> List.reduce(fun a b -> Uion(a,b))";-11,"s0";-12,"Concat(s0,s2)";-13,"Natural s0";-14,"Positive s0";-15,"Maybe s0";-16,"Uion(s0,s2)";-17,"s0,List.rev s1,List.rev s3";-18,"s0,[],List.rev s1";-19,"[s0;s2],s3";-20,"[s0],s1";-21,"[s0]";-22,"s1::s0"|]
let declarations = [|"HEADER","string";"ID","string";"CAP","string";"QUOTE","string";"SEMANTIC","string";"HOLE","string";"character","RegularExpression<string>";"characters","RegularExpression<string> list";"definition","string*RegularExpression<string>";"definitions","(string*RegularExpression<string>)list";"expr","RegularExpression<string>";"file","string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list";"rule","RegularExpression<string>list*string";"rules","(RegularExpression<string>list*string)list"|]
open FslexFsyacc.Lex
open FslexFsyacc.Fslex.FslexTokenUtils
let mappers:(int*(obj[]->obj))[] = [|
    -1,fun (ss:obj[]) ->
        // character -> ID
        let s0 = unbox<string> ss.[0]
        let result:RegularExpression<string> =
            Character s0
        box result
    -2,fun (ss:obj[]) ->
        // character -> QUOTE
        let s0 = unbox<string> ss.[0]
        let result:RegularExpression<string> =
            Character s0
        box result
    -3,fun (ss:obj[]) ->
        // characters -> character
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> list =
            [s0]
        box result
    -4,fun (ss:obj[]) ->
        // characters -> characters "&" character
        let s0 = unbox<RegularExpression<string> list> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let result:RegularExpression<string> list =
            s2::s0
        box result
    -5,fun (ss:obj[]) ->
        // definition -> CAP "=" expr
        let s0 = unbox<string> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let result:string*RegularExpression<string> =
            s0,s2
        box result
    -6,fun (ss:obj[]) ->
        // definitions -> definition
        let s0 = unbox<string*RegularExpression<string>> ss.[0]
        let result:(string*RegularExpression<string>)list =
            [s0]
        box result
    -7,fun (ss:obj[]) ->
        // definitions -> definitions definition
        let s0 = unbox<(string*RegularExpression<string>)list> ss.[0]
        let s1 = unbox<string*RegularExpression<string>> ss.[1]
        let result:(string*RegularExpression<string>)list =
            s1::s0
        box result
    -8,fun (ss:obj[]) ->
        // expr -> "(" expr ")"
        let s1 = unbox<RegularExpression<string>> ss.[1]
        let result:RegularExpression<string> =
            s1
        box result
    -9,fun (ss:obj[]) ->
        // expr -> HOLE
        let s0 = unbox<string> ss.[0]
        let result:RegularExpression<string> =
            Hole s0
        box result
    -10,fun (ss:obj[]) ->
        // expr -> "[" characters "]"
        let s1 = unbox<RegularExpression<string> list> ss.[1]
        let result:RegularExpression<string> =
            s1 |> List.rev |> List.reduce(fun a b -> Uion(a,b))
        box result
    -11,fun (ss:obj[]) ->
        // expr -> character
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> =
            s0
        box result
    -12,fun (ss:obj[]) ->
        // expr -> expr "&" expr
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let result:RegularExpression<string> =
            Concat(s0,s2)
        box result
    -13,fun (ss:obj[]) ->
        // expr -> expr "*"
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> =
            Natural s0
        box result
    -14,fun (ss:obj[]) ->
        // expr -> expr "+"
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> =
            Positive s0
        box result
    -15,fun (ss:obj[]) ->
        // expr -> expr "?"
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> =
            Maybe s0
        box result
    -16,fun (ss:obj[]) ->
        // expr -> expr "|" expr
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let result:RegularExpression<string> =
            Uion(s0,s2)
        box result
    -17,fun (ss:obj[]) ->
        // file -> HEADER definitions "%%" rules
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(string*RegularExpression<string>)list> ss.[1]
        let s3 = unbox<(RegularExpression<string>list*string)list> ss.[3]
        let result:string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list =
            s0,List.rev s1,List.rev s3
        box result
    -18,fun (ss:obj[]) ->
        // file -> HEADER rules
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(RegularExpression<string>list*string)list> ss.[1]
        let result:string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list =
            s0,[],List.rev s1
        box result
    -19,fun (ss:obj[]) ->
        // rule -> expr "/" expr SEMANTIC
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let s3 = unbox<string> ss.[3]
        let result:RegularExpression<string>list*string =
            [s0;s2],s3
        box result
    -20,fun (ss:obj[]) ->
        // rule -> expr SEMANTIC
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let s1 = unbox<string> ss.[1]
        let result:RegularExpression<string>list*string =
            [s0],s1
        box result
    -21,fun (ss:obj[]) ->
        // rules -> rule
        let s0 = unbox<RegularExpression<string>list*string> ss.[0]
        let result:(RegularExpression<string>list*string)list =
            [s0]
        box result
    -22,fun (ss:obj[]) ->
        // rules -> rules rule
        let s0 = unbox<(RegularExpression<string>list*string)list> ss.[0]
        let s1 = unbox<RegularExpression<string>list*string> ss.[1]
        let result:(RegularExpression<string>list*string)list =
            s1::s0
        box result|]
open FslexFsyacc.Runtime
let parser = Parser(productions, actions, kernelSymbols, mappers)
let parse (tokens:seq<_>) =
    parser.parse(tokens, getTag, getLexeme)
    |> unbox<string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list>