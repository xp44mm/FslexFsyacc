module FslexFsyacc.Fsyacc.FsyaccParseTable
let header = "open FslexFsyacc.Fsyacc.FsyaccToken"
let productions = Map [-25,["symbols";"symbols";"symbol"];-24,["symbols";"symbol"];-23,["symbol";"QUOTE"];-22,["symbol";"IDENTIFIER"];-21,["rules";"rules";"rule"];-20,["rules";"rule"];-19,["rule";"IDENTIFIER";":";"bodies"];-18,["precedences";"precedences";"precedence"];-17,["precedences";"precedence"];-16,["precedence";"assoc";"symbols"];-15,["file";"HEADER";"rules";"%%";"precedences";"%%";"declarations"];-14,["file";"HEADER";"rules";"%%";"precedences"];-13,["file";"HEADER";"rules";"%%";"declarations"];-12,["file";"HEADER";"rules"];-11,["declarations";"declarations";";";"declaration"];-10,["declarations";"declaration"];-9,["declaration";"symbol";"QUOTE"];-8,["body";"symbols";"SEMANTIC"];-7,["body";"symbols";"%prec";"IDENTIFIER";"SEMANTIC"];-6,["body";"SEMANTIC"];-5,["bodies";"body"];-4,["bodies";"bodies";"|";"body"];-3,["assoc";"%right"];-2,["assoc";"%nonassoc"];-1,["assoc";"%left"];0,["";"file"]]
let actions = Map [0,Map ["HEADER",22;"file",1];1,Map ["",0];2,Map ["IDENTIFIER",-1;"QUOTE",-1];3,Map ["IDENTIFIER",-2;"QUOTE",-2];4,Map ["IDENTIFIER",-3;"QUOTE",-3];5,Map ["",-19;"%%",-19;"IDENTIFIER",-19;"|",6];6,Map ["IDENTIFIER",35;"QUOTE",36;"SEMANTIC",9;"body",7;"symbol",37;"symbols",10];7,Map ["",-4;"%%",-4;"IDENTIFIER",-4;"|",-4];8,Map ["",-5;"%%",-5;"IDENTIFIER",-5;"|",-5];9,Map ["",-6;"%%",-6;"IDENTIFIER",-6;"|",-6];10,Map ["%prec",11;"IDENTIFIER",35;"QUOTE",36;"SEMANTIC",14;"symbol",38];11,Map ["IDENTIFIER",12];12,Map ["SEMANTIC",13];13,Map ["",-7;"%%",-7;"IDENTIFIER",-7;"|",-7];14,Map ["",-8;"%%",-8;"IDENTIFIER",-8;"|",-8];15,Map ["QUOTE",16];16,Map ["",-9;";",-9];17,Map ["",-10;";",-10];18,Map ["",-13;";",20];19,Map ["",-15;";",20];20,Map ["IDENTIFIER",35;"QUOTE",36;"declaration",21;"symbol",15];21,Map ["",-11;";",-11];22,Map ["IDENTIFIER",31;"rule",33;"rules",23];23,Map ["",-12;"%%",24;"IDENTIFIER",31;"rule",34];24,Map ["%left",2;"%nonassoc",3;"%right",4;"IDENTIFIER",35;"QUOTE",36;"assoc",27;"declaration",17;"declarations",18;"precedence",29;"precedences",25;"symbol",15];25,Map ["",-14;"%%",26;"%left",2;"%nonassoc",3;"%right",4;"assoc",27;"precedence",30];26,Map ["IDENTIFIER",35;"QUOTE",36;"declaration",17;"declarations",19;"symbol",15];27,Map ["IDENTIFIER",35;"QUOTE",36;"symbol",37;"symbols",28];28,Map ["",-16;"%%",-16;"%left",-16;"%nonassoc",-16;"%right",-16;"IDENTIFIER",35;"QUOTE",36;"symbol",38];29,Map ["",-17;"%%",-17;"%left",-17;"%nonassoc",-17;"%right",-17];30,Map ["",-18;"%%",-18;"%left",-18;"%nonassoc",-18;"%right",-18];31,Map [":",32];32,Map ["IDENTIFIER",35;"QUOTE",36;"SEMANTIC",9;"bodies",5;"body",8;"symbol",37;"symbols",10];33,Map ["",-20;"%%",-20;"IDENTIFIER",-20];34,Map ["",-21;"%%",-21;"IDENTIFIER",-21];35,Map ["",-22;"%%",-22;"%left",-22;"%nonassoc",-22;"%prec",-22;"%right",-22;"IDENTIFIER",-22;"QUOTE",-22;"SEMANTIC",-22];36,Map ["",-23;"%%",-23;"%left",-23;"%nonassoc",-23;"%prec",-23;"%right",-23;"IDENTIFIER",-23;"QUOTE",-23;"SEMANTIC",-23];37,Map ["",-24;"%%",-24;"%left",-24;"%nonassoc",-24;"%prec",-24;"%right",-24;"IDENTIFIER",-24;"QUOTE",-24;"SEMANTIC",-24];38,Map ["",-25;"%%",-25;"%left",-25;"%nonassoc",-25;"%prec",-25;"%right",-25;"IDENTIFIER",-25;"QUOTE",-25;"SEMANTIC",-25]]
let kernelSymbols = Map [1,"file";2,"%left";3,"%nonassoc";4,"%right";5,"bodies";6,"|";7,"body";8,"body";9,"SEMANTIC";10,"symbols";11,"%prec";12,"IDENTIFIER";13,"SEMANTIC";14,"SEMANTIC";15,"symbol";16,"QUOTE";17,"declaration";18,"declarations";19,"declarations";20,";";21,"declaration";22,"HEADER";23,"rules";24,"%%";25,"precedences";26,"%%";27,"assoc";28,"symbols";29,"precedence";30,"precedence";31,"IDENTIFIER";32,":";33,"rule";34,"rule";35,"IDENTIFIER";36,"QUOTE";37,"symbol";38,"symbol"]
let semantics = Map [-25,"s1::s0";-24,"[s0]";-23,"s0";-22,"s0";-21,"s1::s0";-20,"[s0]";-19,"s0,List.rev s2";-18,"s1::s0";-17,"[s0]";-16,"s0,List.rev s1";-15,"s0, List.rev s1,List.rev s3,List.rev s5";-14,"s0, List.rev s1,List.rev s3,[]";-13,"s0, List.rev s1,[],List.rev s3";-12,"s0, List.rev s1,[],[]";-11,"s2::s0";-10,"[s0]";-9,"s0,s1";-8,"List.rev s0,\"\",s1";-7,"List.rev s0,s2,s3";-6,"[],\"\",s0";-5,"[s0]";-4,"s2::s0";-3,"\"right\"";-2,"\"nonassoc\"";-1,"\"left\""]
let declarations = ["HEADER","string";"IDENTIFIER","string";"QUOTE","string";"SEMANTIC","string";"assoc","string";"bodies","(string list*string*string)list";"body","string list*string*string";"declaration","string*string";"declarations","(string*string)list";"file","string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list";"precedence","string*string list";"precedences","(string*string list)list";"rule","string*((string list*string*string)list)";"rules","(string*((string list*string*string)list))list";"symbol","string";"symbols","string list"]
open FslexFsyacc.Fsyacc.FsyaccToken
let mappers:Map<int,(obj[]->obj)> = Map [
    -25,fun (ss:obj[]) ->
        // symbols : symbols symbol
        let s0 = unbox<string list> ss.[0]
        let s1 = unbox<string> ss.[1]
        let result:string list =
            s1::s0
        box result
    -24,fun (ss:obj[]) ->
        // symbols : symbol
        let s0 = unbox<string> ss.[0]
        let result:string list =
            [s0]
        box result
    -23,fun (ss:obj[]) ->
        // symbol : QUOTE
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    -22,fun (ss:obj[]) ->
        // symbol : IDENTIFIER
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    -21,fun (ss:obj[]) ->
        // rules : rules rule
        let s0 = unbox<(string*((string list*string*string)list))list> ss.[0]
        let s1 = unbox<string*((string list*string*string)list)> ss.[1]
        let result:(string*((string list*string*string)list))list =
            s1::s0
        box result
    -20,fun (ss:obj[]) ->
        // rules : rule
        let s0 = unbox<string*((string list*string*string)list)> ss.[0]
        let result:(string*((string list*string*string)list))list =
            [s0]
        box result
    -19,fun (ss:obj[]) ->
        // rule : IDENTIFIER ":" bodies
        let s0 = unbox<string> ss.[0]
        let s2 = unbox<(string list*string*string)list> ss.[2]
        let result:string*((string list*string*string)list) =
            s0,List.rev s2
        box result
    -18,fun (ss:obj[]) ->
        // precedences : precedences precedence
        let s0 = unbox<(string*string list)list> ss.[0]
        let s1 = unbox<string*string list> ss.[1]
        let result:(string*string list)list =
            s1::s0
        box result
    -17,fun (ss:obj[]) ->
        // precedences : precedence
        let s0 = unbox<string*string list> ss.[0]
        let result:(string*string list)list =
            [s0]
        box result
    -16,fun (ss:obj[]) ->
        // precedence : assoc symbols
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<string list> ss.[1]
        let result:string*string list =
            s0,List.rev s1
        box result
    -15,fun (ss:obj[]) ->
        // file : HEADER rules "%%" precedences "%%" declarations
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(string*((string list*string*string)list))list> ss.[1]
        let s3 = unbox<(string*string list)list> ss.[3]
        let s5 = unbox<(string*string)list> ss.[5]
        let result:string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list =
            s0, List.rev s1,List.rev s3,List.rev s5
        box result
    -14,fun (ss:obj[]) ->
        // file : HEADER rules "%%" precedences
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(string*((string list*string*string)list))list> ss.[1]
        let s3 = unbox<(string*string list)list> ss.[3]
        let result:string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list =
            s0, List.rev s1,List.rev s3,[]
        box result
    -13,fun (ss:obj[]) ->
        // file : HEADER rules "%%" declarations
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(string*((string list*string*string)list))list> ss.[1]
        let s3 = unbox<(string*string)list> ss.[3]
        let result:string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list =
            s0, List.rev s1,[],List.rev s3
        box result
    -12,fun (ss:obj[]) ->
        // file : HEADER rules
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(string*((string list*string*string)list))list> ss.[1]
        let result:string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list =
            s0, List.rev s1,[],[]
        box result
    -11,fun (ss:obj[]) ->
        // declarations : declarations ";" declaration
        let s0 = unbox<(string*string)list> ss.[0]
        let s2 = unbox<string*string> ss.[2]
        let result:(string*string)list =
            s2::s0
        box result
    -10,fun (ss:obj[]) ->
        // declarations : declaration
        let s0 = unbox<string*string> ss.[0]
        let result:(string*string)list =
            [s0]
        box result
    -9,fun (ss:obj[]) ->
        // declaration : symbol QUOTE
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<string> ss.[1]
        let result:string*string =
            s0,s1
        box result
    -8,fun (ss:obj[]) ->
        // body : symbols SEMANTIC
        let s0 = unbox<string list> ss.[0]
        let s1 = unbox<string> ss.[1]
        let result:string list*string*string =
            List.rev s0,"",s1
        box result
    -7,fun (ss:obj[]) ->
        // body : symbols "%prec" IDENTIFIER SEMANTIC
        let s0 = unbox<string list> ss.[0]
        let s2 = unbox<string> ss.[2]
        let s3 = unbox<string> ss.[3]
        let result:string list*string*string =
            List.rev s0,s2,s3
        box result
    -6,fun (ss:obj[]) ->
        // body : SEMANTIC
        let s0 = unbox<string> ss.[0]
        let result:string list*string*string =
            [],"",s0
        box result
    -5,fun (ss:obj[]) ->
        // bodies : body
        let s0 = unbox<string list*string*string> ss.[0]
        let result:(string list*string*string)list =
            [s0]
        box result
    -4,fun (ss:obj[]) ->
        // bodies : bodies "|" body
        let s0 = unbox<(string list*string*string)list> ss.[0]
        let s2 = unbox<string list*string*string> ss.[2]
        let result:(string list*string*string)list =
            s2::s0
        box result
    -3,fun (ss:obj[]) ->
        // assoc : "%right"
        let result:string =
            "right"
        box result
    -2,fun (ss:obj[]) ->
        // assoc : "%nonassoc"
        let result:string =
            "nonassoc"
        box result
    -1,fun (ss:obj[]) ->
        // assoc : "%left"
        let result:string =
            "left"
        box result]
open FslexFsyacc.Runtime
let parser = Parser(productions, actions, kernelSymbols, mappers)
let parse (tokens:seq<_>) =
    parser.parse(tokens, getTag, getLexeme)
    |> unbox<string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list>