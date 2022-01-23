module FslexFsyacc.Fslex.FslexParseTable
//let productions = [|["";"file"];["character";"ID"];["character";"QUOTE"];["characters";"character"];["characters";"characters";"&";"character"];["definition";"CAP";"=";"expr"];["definitions";"definition"];["definitions";"definitions";"definition"];["expr";"(";"expr";")"];["expr";"HOLE"];["expr";"[";"characters";"]"];["expr";"character"];["expr";"expr";"&";"expr"];["expr";"expr";"*"];["expr";"expr";"+"];["expr";"expr";"?"];["expr";"expr";"|";"expr"];["file";"HEADER";"definitions";"%%";"rules"];["file";"HEADER";"rules"];["rule";"expr";"/";"expr";"SEMANTIC"];["rule";"expr";"SEMANTIC"];["rules";"rule"];["rules";"rules";"rule"]|]
//let closures = [|[|0,0,[||];-17,0,[||];-18,0,[||]|];[|0,1,[|""|]|];[|-1,1,[|"%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"]";"|"|]|];[|-2,1,[|"%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"]";"|"|]|];[|-3,1,[|"&";"]"|]|];[|-4,1,[||];-10,2,[||]|];[|-1,0,[||];-2,0,[||];-4,2,[||]|];[|-4,3,[|"&";"]"|]|];[|-5,1,[||]|];[|-1,0,[||];-2,0,[||];-5,2,[||];-8,0,[||];-9,0,[||];-10,0,[||];-11,0,[||];-12,0,[||];-13,0,[||];-14,0,[||];-15,0,[||];-16,0,[||]|];[|-5,3,[|"%%";"CAP"|];-12,1,[||];-13,1,[||];-14,1,[||];-15,1,[||];-16,1,[||]|];[|-6,1,[|"%%";"CAP"|]|];[|-5,0,[||];-7,1,[||];-17,2,[||]|];[|-7,2,[|"%%";"CAP"|]|];[|-1,0,[||];-2,0,[||];-8,0,[||];-8,1,[||];-9,0,[||];-10,0,[||];-11,0,[||];-12,0,[||];-13,0,[||];-14,0,[||];-15,0,[||];-16,0,[||]|];[|-8,2,[||];-12,1,[||];-13,1,[||];-14,1,[||];-15,1,[||];-16,1,[||]|];[|-8,3,[|"%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"|"|]|];[|-9,1,[|"%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"|"|]|];[|-1,0,[||];-2,0,[||];-3,0,[||];-4,0,[||];-10,1,[||]|];[|-10,3,[|"%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"|"|]|];[|-11,1,[|"%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"|"|]|];[|-12,3,[|"%%";"&";")";"/";"CAP";"SEMANTIC";"|"|];-13,1,[||];-14,1,[||];-15,1,[||]|];[|-12,1,[||];-13,1,[||];-14,1,[||];-15,1,[||];-16,3,[|"%%";")";"/";"CAP";"SEMANTIC";"|"|]|];[|-12,1,[||];-13,1,[||];-14,1,[||];-15,1,[||];-16,1,[||];-19,1,[||];-20,1,[||]|];[|-12,1,[||];-13,1,[||];-14,1,[||];-15,1,[||];-16,1,[||];-19,3,[||]|];[|-1,0,[||];-2,0,[||];-8,0,[||];-9,0,[||];-10,0,[||];-11,0,[||];-12,0,[||];-12,2,[||];-13,0,[||];-14,0,[||];-15,0,[||];-16,0,[||]|];[|-13,2,[|"%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"|"|]|];[|-14,2,[|"%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"|"|]|];[|-15,2,[|"%%";"&";")";"*";"+";"/";"?";"CAP";"SEMANTIC";"|"|]|];[|-1,0,[||];-2,0,[||];-8,0,[||];-9,0,[||];-10,0,[||];-11,0,[||];-12,0,[||];-13,0,[||];-14,0,[||];-15,0,[||];-16,0,[||];-16,2,[||]|];[|-1,0,[||];-2,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-9,0,[||];-10,0,[||];-11,0,[||];-12,0,[||];-13,0,[||];-14,0,[||];-15,0,[||];-16,0,[||];-17,1,[||];-18,1,[||];-19,0,[||];-20,0,[||];-21,0,[||];-22,0,[||]|];[|-1,0,[||];-2,0,[||];-8,0,[||];-9,0,[||];-10,0,[||];-11,0,[||];-12,0,[||];-13,0,[||];-14,0,[||];-15,0,[||];-16,0,[||];-17,3,[||];-19,0,[||];-20,0,[||];-21,0,[||];-22,0,[||]|];[|-1,0,[||];-2,0,[||];-8,0,[||];-9,0,[||];-10,0,[||];-11,0,[||];-12,0,[||];-13,0,[||];-14,0,[||];-15,0,[||];-16,0,[||];-17,4,[|""|];-19,0,[||];-20,0,[||];-22,1,[||]|];[|-1,0,[||];-2,0,[||];-8,0,[||];-9,0,[||];-10,0,[||];-11,0,[||];-12,0,[||];-13,0,[||];-14,0,[||];-15,0,[||];-16,0,[||];-18,2,[|""|];-19,0,[||];-20,0,[||];-22,1,[||]|];[|-1,0,[||];-2,0,[||];-8,0,[||];-9,0,[||];-10,0,[||];-11,0,[||];-12,0,[||];-13,0,[||];-14,0,[||];-15,0,[||];-16,0,[||];-19,2,[||]|];[|-19,4,[|"";"(";"HOLE";"ID";"QUOTE";"["|]|];[|-20,2,[|"";"(";"HOLE";"ID";"QUOTE";"["|]|];[|-21,1,[|"";"(";"HOLE";"ID";"QUOTE";"["|]|];[|-22,2,[|"";"(";"HOLE";"ID";"QUOTE";"["|]|]|]
//let actions = [|[|"HEADER",30;"file",1|];[|"",0|];[|"%%",-1;"&",-1;")",-1;"*",-1;"+",-1;"/",-1;"?",-1;"CAP",-1;"SEMANTIC",-1;"]",-1;"|",-1|];[|"%%",-2;"&",-2;")",-2;"*",-2;"+",-2;"/",-2;"?",-2;"CAP",-2;"SEMANTIC",-2;"]",-2;"|",-2|];[|"&",-3;"]",-3|];[|"&",6;"]",19|];[|"ID",2;"QUOTE",3;"character",7|];[|"&",-4;"]",-4|];[|"=",9|];[|"(",14;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"expr",10|];[|"%%",-5;"&",25;"*",26;"+",27;"?",28;"CAP",-5;"|",29|];[|"%%",-6;"CAP",-6|];[|"%%",31;"CAP",8;"definition",13|];[|"%%",-7;"CAP",-7|];[|"(",14;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"expr",15|];[|"&",25;")",16;"*",26;"+",27;"?",28;"|",29|];[|"%%",-8;"&",-8;")",-8;"*",-8;"+",-8;"/",-8;"?",-8;"CAP",-8;"SEMANTIC",-8;"|",-8|];[|"%%",-9;"&",-9;")",-9;"*",-9;"+",-9;"/",-9;"?",-9;"CAP",-9;"SEMANTIC",-9;"|",-9|];[|"ID",2;"QUOTE",3;"character",4;"characters",5|];[|"%%",-10;"&",-10;")",-10;"*",-10;"+",-10;"/",-10;"?",-10;"CAP",-10;"SEMANTIC",-10;"|",-10|];[|"%%",-11;"&",-11;")",-11;"*",-11;"+",-11;"/",-11;"?",-11;"CAP",-11;"SEMANTIC",-11;"|",-11|];[|"%%",-12;"&",-12;")",-12;"*",26;"+",27;"/",-12;"?",28;"CAP",-12;"SEMANTIC",-12;"|",-12|];[|"%%",-16;"&",25;")",-16;"*",26;"+",27;"/",-16;"?",28;"CAP",-16;"SEMANTIC",-16;"|",-16|];[|"&",25;"*",26;"+",27;"/",34;"?",28;"SEMANTIC",36;"|",29|];[|"&",25;"*",26;"+",27;"?",28;"SEMANTIC",35;"|",29|];[|"(",14;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"expr",21|];[|"%%",-13;"&",-13;")",-13;"*",-13;"+",-13;"/",-13;"?",-13;"CAP",-13;"SEMANTIC",-13;"|",-13|];[|"%%",-14;"&",-14;")",-14;"*",-14;"+",-14;"/",-14;"?",-14;"CAP",-14;"SEMANTIC",-14;"|",-14|];[|"%%",-15;"&",-15;")",-15;"*",-15;"+",-15;"/",-15;"?",-15;"CAP",-15;"SEMANTIC",-15;"|",-15|];[|"(",14;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"expr",22|];[|"(",14;"CAP",8;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"definition",11;"definitions",12;"expr",23;"rule",37;"rules",33|];[|"(",14;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"expr",23;"rule",37;"rules",32|];[|"",-17;"(",14;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"expr",23;"rule",38|];[|"",-18;"(",14;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"expr",23;"rule",38|];[|"(",14;"HOLE",17;"ID",2;"QUOTE",3;"[",18;"character",20;"expr",24|];[|"",-19;"(",-19;"HOLE",-19;"ID",-19;"QUOTE",-19;"[",-19|];[|"",-20;"(",-20;"HOLE",-20;"ID",-20;"QUOTE",-20;"[",-20|];[|"",-21;"(",-21;"HOLE",-21;"ID",-21;"QUOTE",-21;"[",-21|];[|"",-22;"(",-22;"HOLE",-22;"ID",-22;"QUOTE",-22;"[",-22|]|]
//let header = "open FslexFsyacc.Lex\r\nopen FslexFsyacc.Fslex.FslexTokenUtils"
//let semantics = [|"Character s0";"Character s0";"[s0]";"s2::s0";"s0,s2";"[s0]";"s1::s0";"s1";"Hole s0";"s1 |> List.rev |> List.reduce(fun a b -> Uion(a,b))";"s0";"Concat(s0,s2)";"Natural s0";"Positive s0";"Maybe s0";"Uion(s0,s2)";"s0,List.rev s1,List.rev s3";"s0,[],List.rev s1";"[s0;s2],s3";"[s0],s1";"[s0]";"s1::s0"|]
//let declarations = [|"HEADER","string";"ID","string";"CAP","string";"QUOTE","string";"SEMANTIC","string";"HOLE","string";"character","RegularExpression<string>";"characters","RegularExpression<string> list";"definition","string*RegularExpression<string>";"definitions","(string*RegularExpression<string>)list";"expr","RegularExpression<string>";"file","string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list";"rule","RegularExpression<string>list*string";"rules","(RegularExpression<string>list*string)list"|]
//open FslexFsyacc.Lex
//open FslexFsyacc.Fslex.FslexTokenUtils
//let mappers:(obj[]->obj)[] = [|
//    fun (ss:obj[]) ->
//        // character -> ID
//        let s0 = unbox<string> ss.[0]
//        let result:RegularExpression<string> =
//            Character s0
//        box result
//    fun (ss:obj[]) ->
//        // character -> QUOTE
//        let s0 = unbox<string> ss.[0]
//        let result:RegularExpression<string> =
//            Character s0
//        box result
//    fun (ss:obj[]) ->
//        // characters -> character
//        let s0 = unbox<RegularExpression<string>> ss.[0]
//        let result:RegularExpression<string> list =
//            [s0]
//        box result
//    fun (ss:obj[]) ->
//        // characters -> characters "&" character
//        let s0 = unbox<RegularExpression<string> list> ss.[0]
//        let s2 = unbox<RegularExpression<string>> ss.[2]
//        let result:RegularExpression<string> list =
//            s2::s0
//        box result
//    fun (ss:obj[]) ->
//        // definition -> CAP "=" expr
//        let s0 = unbox<string> ss.[0]
//        let s2 = unbox<RegularExpression<string>> ss.[2]
//        let result:string*RegularExpression<string> =
//            s0,s2
//        box result
//    fun (ss:obj[]) ->
//        // definitions -> definition
//        let s0 = unbox<string*RegularExpression<string>> ss.[0]
//        let result:(string*RegularExpression<string>)list =
//            [s0]
//        box result
//    fun (ss:obj[]) ->
//        // definitions -> definitions definition
//        let s0 = unbox<(string*RegularExpression<string>)list> ss.[0]
//        let s1 = unbox<string*RegularExpression<string>> ss.[1]
//        let result:(string*RegularExpression<string>)list =
//            s1::s0
//        box result
//    fun (ss:obj[]) ->
//        // expr -> "(" expr ")"
//        let s1 = unbox<RegularExpression<string>> ss.[1]
//        let result:RegularExpression<string> =
//            s1
//        box result
//    fun (ss:obj[]) ->
//        // expr -> HOLE
//        let s0 = unbox<string> ss.[0]
//        let result:RegularExpression<string> =
//            Hole s0
//        box result
//    fun (ss:obj[]) ->
//        // expr -> "[" characters "]"
//        let s1 = unbox<RegularExpression<string> list> ss.[1]
//        let result:RegularExpression<string> =
//            s1 |> List.rev |> List.reduce(fun a b -> Uion(a,b))
//        box result
//    fun (ss:obj[]) ->
//        // expr -> character
//        let s0 = unbox<RegularExpression<string>> ss.[0]
//        let result:RegularExpression<string> =
//            s0
//        box result
//    fun (ss:obj[]) ->
//        // expr -> expr "&" expr
//        let s0 = unbox<RegularExpression<string>> ss.[0]
//        let s2 = unbox<RegularExpression<string>> ss.[2]
//        let result:RegularExpression<string> =
//            Concat(s0,s2)
//        box result
//    fun (ss:obj[]) ->
//        // expr -> expr "*"
//        let s0 = unbox<RegularExpression<string>> ss.[0]
//        let result:RegularExpression<string> =
//            Natural s0
//        box result
//    fun (ss:obj[]) ->
//        // expr -> expr "+"
//        let s0 = unbox<RegularExpression<string>> ss.[0]
//        let result:RegularExpression<string> =
//            Positive s0
//        box result
//    fun (ss:obj[]) ->
//        // expr -> expr "?"
//        let s0 = unbox<RegularExpression<string>> ss.[0]
//        let result:RegularExpression<string> =
//            Maybe s0
//        box result
//    fun (ss:obj[]) ->
//        // expr -> expr "|" expr
//        let s0 = unbox<RegularExpression<string>> ss.[0]
//        let s2 = unbox<RegularExpression<string>> ss.[2]
//        let result:RegularExpression<string> =
//            Uion(s0,s2)
//        box result
//    fun (ss:obj[]) ->
//        // file -> HEADER definitions "%%" rules
//        let s0 = unbox<string> ss.[0]
//        let s1 = unbox<(string*RegularExpression<string>)list> ss.[1]
//        let s3 = unbox<(RegularExpression<string>list*string)list> ss.[3]
//        let result:string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list =
//            s0,List.rev s1,List.rev s3
//        box result
//    fun (ss:obj[]) ->
//        // file -> HEADER rules
//        let s0 = unbox<string> ss.[0]
//        let s1 = unbox<(RegularExpression<string>list*string)list> ss.[1]
//        let result:string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list =
//            s0,[],List.rev s1
//        box result
//    fun (ss:obj[]) ->
//        // rule -> expr "/" expr SEMANTIC
//        let s0 = unbox<RegularExpression<string>> ss.[0]
//        let s2 = unbox<RegularExpression<string>> ss.[2]
//        let s3 = unbox<string> ss.[3]
//        let result:RegularExpression<string>list*string =
//            [s0;s2],s3
//        box result
//    fun (ss:obj[]) ->
//        // rule -> expr SEMANTIC
//        let s0 = unbox<RegularExpression<string>> ss.[0]
//        let s1 = unbox<string> ss.[1]
//        let result:RegularExpression<string>list*string =
//            [s0],s1
//        box result
//    fun (ss:obj[]) ->
//        // rules -> rule
//        let s0 = unbox<RegularExpression<string>list*string> ss.[0]
//        let result:(RegularExpression<string>list*string)list =
//            [s0]
//        box result
//    fun (ss:obj[]) ->
//        // rules -> rules rule
//        let s0 = unbox<(RegularExpression<string>list*string)list> ss.[0]
//        let s1 = unbox<RegularExpression<string>list*string> ss.[1]
//        let result:(RegularExpression<string>list*string)list =
//            s1::s0
//        box result
//|]
//open FslexFsyacc.Runtime
//let parser = Parser(productions, closures, actions, mappers)
//let parse (tokens:seq<_>) =
//    parser.parse(tokens, getTag, getLexeme)
//    |> unbox<string*(string*RegularExpression<string>)list*(RegularExpression<string>list*string)list>