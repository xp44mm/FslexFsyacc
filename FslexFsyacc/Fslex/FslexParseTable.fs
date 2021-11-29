module FslexFsyacc.Fslex.FslexParseTable
let header = "open FslexFsyacc.Lex\r\nopen FslexFsyacc.Fslex.FslexToken"
let productions = Map [-22,["rules";"rules";"\n";"rule"];-21,["rules";"rule"];-20,["rule";"expr";"SEMANTIC"];-19,["rule";"expr";"/";"expr";"SEMANTIC"];-18,["file";"HEADER";"rules"];-17,["file";"HEADER";"definitions";"%%";"rules"];-16,["expr";"expr";"|";"expr"];-15,["expr";"expr";"?"];-14,["expr";"expr";"+"];-13,["expr";"expr";"*"];-12,["expr";"expr";"&";"expr"];-11,["expr";"character"];-10,["expr";"[";"characters";"]"];-9,["expr";"HOLE"];-8,["expr";"(";"expr";")"];-7,["definitions";"definitions";"definition"];-6,["definitions";"definition"];-5,["definition";"ID";"=";"expr";"\n"];-4,["characters";"characters";"&";"character"];-3,["characters";"character"];-2,["character";"QUOTE"];-1,["character";"ID"];0,["";"file"]]
let actions = Map [0,Map ["HEADER",32;"file",1];1,Map ["",0];2,Map ["\n",-1;"&",-1;")",-1;"*",-1;"+",-1;"/",-1;"?",-1;"SEMANTIC",-1;"]",-1;"|",-1];3,Map ["&",-1;"*",-1;"+",-1;"/",-1;"=",10;"?",-1;"SEMANTIC",-1;"|",-1];4,Map ["\n",-2;"&",-2;")",-2;"*",-2;"+",-2;"/",-2;"?",-2;"SEMANTIC",-2;"]",-2;"|",-2];5,Map ["&",-3;"]",-3];6,Map ["&",7;"]",21];7,Map ["ID",2;"QUOTE",4;"character",8];8,Map ["&",-4;"]",-4];9,Map ["=",10];10,Map ["(",16;"HOLE",19;"ID",2;"QUOTE",4;"[",20;"character",22;"expr",11];11,Map ["\n",12;"&",27;"*",28;"+",29;"?",30;"|",31];12,Map ["%%",-5;"ID",-5];13,Map ["%%",-6;"ID",-6];14,Map ["%%",33;"ID",9;"definition",15];15,Map ["%%",-7;"ID",-7];16,Map ["(",16;"HOLE",19;"ID",2;"QUOTE",4;"[",20;"character",22;"expr",17];17,Map ["&",27;")",18;"*",28;"+",29;"?",30;"|",31];18,Map ["\n",-8;"&",-8;")",-8;"*",-8;"+",-8;"/",-8;"?",-8;"SEMANTIC",-8;"|",-8];19,Map ["\n",-9;"&",-9;")",-9;"*",-9;"+",-9;"/",-9;"?",-9;"SEMANTIC",-9;"|",-9];20,Map ["ID",2;"QUOTE",4;"character",5;"characters",6];21,Map ["\n",-10;"&",-10;")",-10;"*",-10;"+",-10;"/",-10;"?",-10;"SEMANTIC",-10;"|",-10];22,Map ["\n",-11;"&",-11;")",-11;"*",-11;"+",-11;"/",-11;"?",-11;"SEMANTIC",-11;"|",-11];23,Map ["\n",-12;"&",-12;")",-12;"*",28;"+",29;"/",-12;"?",30;"SEMANTIC",-12;"|",-12];24,Map ["\n",-16;"&",27;")",-16;"*",28;"+",29;"/",-16;"?",30;"SEMANTIC",-16;"|",-16];25,Map ["&",27;"*",28;"+",29;"/",36;"?",30;"SEMANTIC",38;"|",31];26,Map ["&",27;"*",28;"+",29;"?",30;"SEMANTIC",37;"|",31];27,Map ["(",16;"HOLE",19;"ID",2;"QUOTE",4;"[",20;"character",22;"expr",23];28,Map ["\n",-13;"&",-13;")",-13;"*",-13;"+",-13;"/",-13;"?",-13;"SEMANTIC",-13;"|",-13];29,Map ["\n",-14;"&",-14;")",-14;"*",-14;"+",-14;"/",-14;"?",-14;"SEMANTIC",-14;"|",-14];30,Map ["\n",-15;"&",-15;")",-15;"*",-15;"+",-15;"/",-15;"?",-15;"SEMANTIC",-15;"|",-15];31,Map ["(",16;"HOLE",19;"ID",2;"QUOTE",4;"[",20;"character",22;"expr",24];32,Map ["(",16;"HOLE",19;"ID",3;"QUOTE",4;"[",20;"character",22;"definition",13;"definitions",14;"expr",25;"rule",39;"rules",35];33,Map ["(",16;"HOLE",19;"ID",2;"QUOTE",4;"[",20;"character",22;"expr",25;"rule",39;"rules",34];34,Map ["",-17;"\n",40];35,Map ["",-18;"\n",40];36,Map ["(",16;"HOLE",19;"ID",2;"QUOTE",4;"[",20;"character",22;"expr",26];37,Map ["",-19;"\n",-19];38,Map ["",-20;"\n",-20];39,Map ["",-21;"\n",-21];40,Map ["(",16;"HOLE",19;"ID",2;"QUOTE",4;"[",20;"character",22;"expr",25;"rule",41];41,Map ["",-22;"\n",-22]]
let kernelSymbols = Map [1,"file";2,"ID";3,"ID";4,"QUOTE";5,"character";6,"characters";7,"&";8,"character";9,"ID";10,"=";11,"expr";12,"\n";13,"definition";14,"definitions";15,"definition";16,"(";17,"expr";18,")";19,"HOLE";20,"[";21,"]";22,"character";23,"expr";24,"expr";25,"expr";26,"expr";27,"&";28,"*";29,"+";30,"?";31,"|";32,"HEADER";33,"%%";34,"rules";35,"rules";36,"/";37,"SEMANTIC";38,"SEMANTIC";39,"rule";40,"\n";41,"rule"]
let semantics = Map [-22,"s2::s0";-21,"[s0]";-20,"[s0],s1";-19,"[s0;s2],s3";-18,"s0,[],List.rev s1";-17,"s0,List.rev s1,List.rev s3";-16,"Uion(s0,s2)";-15,"Maybe s0";-14,"Positive s0";-13,"Natural s0";-12,"Concat(s0,s2)";-11,"s0";-10,"s1 |> List.rev |> List.reduce(fun a b -> Uion(a,b))";-9,"Hole s0";-8,"s1";-7,"s1::s0";-6,"[s0]";-5,"s0,s2";-4,"s2::s0";-3,"[s0]";-2,"Character s0";-1,"Character s0"]
let declarations = ["HEADER","string";"ID","string";"QUOTE","string";"SEMANTIC","string";"HOLE","string";"character","RegularExpression<string>";"characters","RegularExpression<string> list";"definition","string*RegularExpression<string>";"definitions","(string*RegularExpression<string>)list";"expr","RegularExpression<string>";"file","string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list";"rule","RegularExpression<string>list*string";"rules","(RegularExpression<string>list*string)list"]
open FslexFsyacc.Lex
open FslexFsyacc.Fslex.FslexToken
let mappers:Map<int,(obj[]->obj)> = Map [
    -22,fun (ss:obj[]) ->
        // rules : rules "\n" rule
        let s0 = unbox<(RegularExpression<string>list*string)list> ss.[0]
        let s2 = unbox<RegularExpression<string>list*string> ss.[2]
        let result:(RegularExpression<string>list*string)list =
            s2::s0
        box result
    -21,fun (ss:obj[]) ->
        // rules : rule
        let s0 = unbox<RegularExpression<string>list*string> ss.[0]
        let result:(RegularExpression<string>list*string)list =
            [s0]
        box result
    -20,fun (ss:obj[]) ->
        // rule : expr SEMANTIC
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let s1 = unbox<string> ss.[1]
        let result:RegularExpression<string>list*string =
            [s0],s1
        box result
    -19,fun (ss:obj[]) ->
        // rule : expr "/" expr SEMANTIC
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let s3 = unbox<string> ss.[3]
        let result:RegularExpression<string>list*string =
            [s0;s2],s3
        box result
    -18,fun (ss:obj[]) ->
        // file : HEADER rules
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(RegularExpression<string>list*string)list> ss.[1]
        let result:string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list =
            s0,[],List.rev s1
        box result
    -17,fun (ss:obj[]) ->
        // file : HEADER definitions "%%" rules
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(string*RegularExpression<string>)list> ss.[1]
        let s3 = unbox<(RegularExpression<string>list*string)list> ss.[3]
        let result:string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list =
            s0,List.rev s1,List.rev s3
        box result
    -16,fun (ss:obj[]) ->
        // expr : expr "|" expr
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let result:RegularExpression<string> =
            Uion(s0,s2)
        box result
    -15,fun (ss:obj[]) ->
        // expr : expr "?"
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> =
            Maybe s0
        box result
    -14,fun (ss:obj[]) ->
        // expr : expr "+"
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> =
            Positive s0
        box result
    -13,fun (ss:obj[]) ->
        // expr : expr "*"
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> =
            Natural s0
        box result
    -12,fun (ss:obj[]) ->
        // expr : expr "&" expr
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let result:RegularExpression<string> =
            Concat(s0,s2)
        box result
    -11,fun (ss:obj[]) ->
        // expr : character
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> =
            s0
        box result
    -10,fun (ss:obj[]) ->
        // expr : "[" characters "]"
        let s1 = unbox<RegularExpression<string> list> ss.[1]
        let result:RegularExpression<string> =
            s1 |> List.rev |> List.reduce(fun a b -> Uion(a,b))
        box result
    -9,fun (ss:obj[]) ->
        // expr : HOLE
        let s0 = unbox<string> ss.[0]
        let result:RegularExpression<string> =
            Hole s0
        box result
    -8,fun (ss:obj[]) ->
        // expr : "(" expr ")"
        let s1 = unbox<RegularExpression<string>> ss.[1]
        let result:RegularExpression<string> =
            s1
        box result
    -7,fun (ss:obj[]) ->
        // definitions : definitions definition
        let s0 = unbox<(string*RegularExpression<string>)list> ss.[0]
        let s1 = unbox<string*RegularExpression<string>> ss.[1]
        let result:(string*RegularExpression<string>)list =
            s1::s0
        box result
    -6,fun (ss:obj[]) ->
        // definitions : definition
        let s0 = unbox<string*RegularExpression<string>> ss.[0]
        let result:(string*RegularExpression<string>)list =
            [s0]
        box result
    -5,fun (ss:obj[]) ->
        // definition : ID "=" expr "\n"
        let s0 = unbox<string> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let result:string*RegularExpression<string> =
            s0,s2
        box result
    -4,fun (ss:obj[]) ->
        // characters : characters "&" character
        let s0 = unbox<RegularExpression<string> list> ss.[0]
        let s2 = unbox<RegularExpression<string>> ss.[2]
        let result:RegularExpression<string> list =
            s2::s0
        box result
    -3,fun (ss:obj[]) ->
        // characters : character
        let s0 = unbox<RegularExpression<string>> ss.[0]
        let result:RegularExpression<string> list =
            [s0]
        box result
    -2,fun (ss:obj[]) ->
        // character : QUOTE
        let s0 = unbox<string> ss.[0]
        let result:RegularExpression<string> =
            Character s0
        box result
    -1,fun (ss:obj[]) ->
        // character : ID
        let s0 = unbox<string> ss.[0]
        let result:RegularExpression<string> =
            Character s0
        box result]
open FslexFsyacc.Runtime
let parser = Parser(productions, actions, kernelSymbols, mappers)
let parse (tokens:seq<_>) =
    parser.parse(tokens, getTag, getLexeme)
    |> unbox<string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list>