module FslexFsyacc.Fsyacc.FsyaccParseTable
//let productions = [|["";"file"];["assoc";"%left"];["assoc";"%nonassoc"];["assoc";"%right"];["bodies";"bodies";"|";"body"];["bodies";"body"];["body";"SEMANTIC"];["body";"symbols";"%prec";"ID";"SEMANTIC"];["body";"symbols";"SEMANTIC"];["declaration";"symbol";":";"symbol"];["declarations";"declaration"];["declarations";"declarations";"declaration"];["derive";":"];["derive";":";"|"];["file";"HEADER";"rules"];["file";"HEADER";"rules";"%%";"declarations"];["file";"HEADER";"rules";"%%";"precedences"];["file";"HEADER";"rules";"%%";"precedences";"%%";"declarations"];["precedence";"assoc";"symbols"];["precedences";"precedence"];["precedences";"precedences";"precedence"];["rule";"ID";"derive";"bodies"];["rules";"rule"];["rules";"rules";"rule"];["symbol";"ID"];["symbol";"QUOTE"];["symbols";"symbol"];["symbols";"symbols";"symbol"]|]
//let closures = [|[|0,0,[||];-14,0,[||];-15,0,[||];-16,0,[||];-17,0,[||]|];[|0,1,[|""|]|];[|-1,1,[|"ID";"QUOTE"|]|];[|-2,1,[|"ID";"QUOTE"|]|];[|-3,1,[|"ID";"QUOTE"|]|];[|-4,1,[||];-21,3,[|"";"%%";"ID"|]|];[|-4,2,[||];-6,0,[||];-7,0,[||];-8,0,[||];-24,0,[||];-25,0,[||];-26,0,[||];-27,0,[||]|];[|-4,3,[|"";"%%";"ID";"|"|]|];[|-5,1,[|"";"%%";"ID";"|"|]|];[|-6,1,[|"";"%%";"ID";"|"|]|];[|-7,1,[||];-8,1,[||];-24,0,[||];-25,0,[||];-27,1,[||]|];[|-7,2,[||]|];[|-7,3,[||]|];[|-7,4,[|"";"%%";"ID";"|"|]|];[|-8,2,[|"";"%%";"ID";"|"|]|];[|-9,1,[||]|];[|-9,2,[||];-24,0,[||];-25,0,[||]|];[|-9,3,[|"";"ID";"QUOTE"|]|];[|-10,1,[|"";"ID";"QUOTE"|]|];[|-9,0,[||];-11,1,[||];-15,4,[|""|];-24,0,[||];-25,0,[||]|];[|-9,0,[||];-11,1,[||];-17,6,[|""|];-24,0,[||];-25,0,[||]|];[|-11,2,[|"";"ID";"QUOTE"|]|];[|-12,1,[|"ID";"QUOTE";"SEMANTIC"|];-13,1,[||]|];[|-13,2,[|"ID";"QUOTE";"SEMANTIC"|]|];[|-14,1,[||];-15,1,[||];-16,1,[||];-17,1,[||];-21,0,[||];-22,0,[||];-23,0,[||]|];[|-14,2,[|""|];-15,2,[||];-16,2,[||];-17,2,[||];-21,0,[||];-23,1,[||]|];[|-1,0,[||];-2,0,[||];-3,0,[||];-9,0,[||];-10,0,[||];-11,0,[||];-15,3,[||];-16,3,[||];-17,3,[||];-18,0,[||];-19,0,[||];-20,0,[||];-24,0,[||];-25,0,[||]|];[|-1,0,[||];-2,0,[||];-3,0,[||];-16,4,[|""|];-17,4,[||];-18,0,[||];-20,1,[||]|];[|-9,0,[||];-10,0,[||];-11,0,[||];-17,5,[||];-24,0,[||];-25,0,[||]|];[|-18,1,[||];-24,0,[||];-25,0,[||];-26,0,[||];-27,0,[||]|];[|-18,2,[|"";"%%";"%left";"%nonassoc";"%right"|];-24,0,[||];-25,0,[||];-27,1,[||]|];[|-19,1,[|"";"%%";"%left";"%nonassoc";"%right"|]|];[|-20,2,[|"";"%%";"%left";"%nonassoc";"%right"|]|];[|-12,0,[||];-13,0,[||];-21,1,[||]|];[|-4,0,[||];-5,0,[||];-6,0,[||];-7,0,[||];-8,0,[||];-21,2,[||];-24,0,[||];-25,0,[||];-26,0,[||];-27,0,[||]|];[|-22,1,[|"";"%%";"ID"|]|];[|-23,2,[|"";"%%";"ID"|]|];[|-24,1,[|"";"%%";"%left";"%nonassoc";"%prec";"%right";":";"ID";"QUOTE";"SEMANTIC"|]|];[|-25,1,[|"";"%%";"%left";"%nonassoc";"%prec";"%right";":";"ID";"QUOTE";"SEMANTIC"|]|];[|-26,1,[|"";"%%";"%left";"%nonassoc";"%prec";"%right";"ID";"QUOTE";"SEMANTIC"|]|];[|-27,2,[|"";"%%";"%left";"%nonassoc";"%prec";"%right";"ID";"QUOTE";"SEMANTIC"|]|]|]
//let actions = [|[|"HEADER",24;"file",1|];[|"",0|];[|"ID",-1;"QUOTE",-1|];[|"ID",-2;"QUOTE",-2|];[|"ID",-3;"QUOTE",-3|];[|"",-21;"%%",-21;"ID",-21;"|",6|];[|"ID",37;"QUOTE",38;"SEMANTIC",9;"body",7;"symbol",39;"symbols",10|];[|"",-4;"%%",-4;"ID",-4;"|",-4|];[|"",-5;"%%",-5;"ID",-5;"|",-5|];[|"",-6;"%%",-6;"ID",-6;"|",-6|];[|"%prec",11;"ID",37;"QUOTE",38;"SEMANTIC",14;"symbol",40|];[|"ID",12|];[|"SEMANTIC",13|];[|"",-7;"%%",-7;"ID",-7;"|",-7|];[|"",-8;"%%",-8;"ID",-8;"|",-8|];[|":",16|];[|"ID",37;"QUOTE",38;"symbol",17|];[|"",-9;"ID",-9;"QUOTE",-9|];[|"",-10;"ID",-10;"QUOTE",-10|];[|"",-15;"ID",37;"QUOTE",38;"declaration",21;"symbol",15|];[|"",-17;"ID",37;"QUOTE",38;"declaration",21;"symbol",15|];[|"",-11;"ID",-11;"QUOTE",-11|];[|"ID",-12;"QUOTE",-12;"SEMANTIC",-12;"|",23|];[|"ID",-13;"QUOTE",-13;"SEMANTIC",-13|];[|"ID",33;"rule",35;"rules",25|];[|"",-14;"%%",26;"ID",33;"rule",36|];[|"%left",2;"%nonassoc",3;"%right",4;"ID",37;"QUOTE",38;"assoc",29;"declaration",18;"declarations",19;"precedence",31;"precedences",27;"symbol",15|];[|"",-16;"%%",28;"%left",2;"%nonassoc",3;"%right",4;"assoc",29;"precedence",32|];[|"ID",37;"QUOTE",38;"declaration",18;"declarations",20;"symbol",15|];[|"ID",37;"QUOTE",38;"symbol",39;"symbols",30|];[|"",-18;"%%",-18;"%left",-18;"%nonassoc",-18;"%right",-18;"ID",37;"QUOTE",38;"symbol",40|];[|"",-19;"%%",-19;"%left",-19;"%nonassoc",-19;"%right",-19|];[|"",-20;"%%",-20;"%left",-20;"%nonassoc",-20;"%right",-20|];[|":",22;"derive",34|];[|"ID",37;"QUOTE",38;"SEMANTIC",9;"bodies",5;"body",8;"symbol",39;"symbols",10|];[|"",-22;"%%",-22;"ID",-22|];[|"",-23;"%%",-23;"ID",-23|];[|"",-24;"%%",-24;"%left",-24;"%nonassoc",-24;"%prec",-24;"%right",-24;":",-24;"ID",-24;"QUOTE",-24;"SEMANTIC",-24|];[|"",-25;"%%",-25;"%left",-25;"%nonassoc",-25;"%prec",-25;"%right",-25;":",-25;"ID",-25;"QUOTE",-25;"SEMANTIC",-25|];[|"",-26;"%%",-26;"%left",-26;"%nonassoc",-26;"%prec",-26;"%right",-26;"ID",-26;"QUOTE",-26;"SEMANTIC",-26|];[|"",-27;"%%",-27;"%left",-27;"%nonassoc",-27;"%prec",-27;"%right",-27;"ID",-27;"QUOTE",-27;"SEMANTIC",-27|]|]
//let header = "open FslexFsyacc.Fsyacc\r\nopen FslexFsyacc.Fsyacc.FsyaccTokenUtils"
//let semantics = [|"\"left\"";"\"nonassoc\"";"\"right\"";"s2::s0";"[s0]";"[],\"\",s0";"List.rev s0,s2,s3";"List.rev s0,\"\",s1";"s0,s2.Trim()";"[s0]";"s1::s0";"";"";"s0, List.rev s1,[],[]";"s0, List.rev s1,[],List.rev s3";"s0, List.rev s1,List.rev s3,[]";"s0, List.rev s1,List.rev s3,List.rev s5";"s0,List.rev s1";"[s0]";"s1::s0";"s0,List.rev s2";"[s0]";"s1::s0";"s0";"s0";"[s0]";"s1::s0"|]
//let declarations = [|"HEADER","string";"ID","string";"QUOTE","string";"SEMANTIC","string";"assoc","string";"bodies","(string list*string*string)list";"body","string list*string*string";"declaration","string*string";"declarations","(string*string)list";"file","string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list";"precedence","string*string list";"precedences","(string*string list)list";"rule","string*((string list*string*string)list)";"rules","(string*((string list*string*string)list))list";"symbol","string";"symbols","string list"|]
//open FslexFsyacc.Fsyacc
//open FslexFsyacc.Fsyacc.FsyaccTokenUtils
//let mappers:(obj[]->obj)[] = [|
//    fun (ss:obj[]) ->
//        // assoc -> "%left"
//        let result:string =
//            "left"
//        box result
//    fun (ss:obj[]) ->
//        // assoc -> "%nonassoc"
//        let result:string =
//            "nonassoc"
//        box result
//    fun (ss:obj[]) ->
//        // assoc -> "%right"
//        let result:string =
//            "right"
//        box result
//    fun (ss:obj[]) ->
//        // bodies -> bodies "|" body
//        let s0 = unbox<(string list*string*string)list> ss.[0]
//        let s2 = unbox<string list*string*string> ss.[2]
//        let result:(string list*string*string)list =
//            s2::s0
//        box result
//    fun (ss:obj[]) ->
//        // bodies -> body
//        let s0 = unbox<string list*string*string> ss.[0]
//        let result:(string list*string*string)list =
//            [s0]
//        box result
//    fun (ss:obj[]) ->
//        // body -> SEMANTIC
//        let s0 = unbox<string> ss.[0]
//        let result:string list*string*string =
//            [],"",s0
//        box result
//    fun (ss:obj[]) ->
//        // body -> symbols "%prec" ID SEMANTIC
//        let s0 = unbox<string list> ss.[0]
//        let s2 = unbox<string> ss.[2]
//        let s3 = unbox<string> ss.[3]
//        let result:string list*string*string =
//            List.rev s0,s2,s3
//        box result
//    fun (ss:obj[]) ->
//        // body -> symbols SEMANTIC
//        let s0 = unbox<string list> ss.[0]
//        let s1 = unbox<string> ss.[1]
//        let result:string list*string*string =
//            List.rev s0,"",s1
//        box result
//    fun (ss:obj[]) ->
//        // declaration -> symbol ":" symbol
//        let s0 = unbox<string> ss.[0]
//        let s2 = unbox<string> ss.[2]
//        let result:string*string =
//            s0,s2.Trim()
//        box result
//    fun (ss:obj[]) ->
//        // declarations -> declaration
//        let s0 = unbox<string*string> ss.[0]
//        let result:(string*string)list =
//            [s0]
//        box result
//    fun (ss:obj[]) ->
//        // declarations -> declarations declaration
//        let s0 = unbox<(string*string)list> ss.[0]
//        let s1 = unbox<string*string> ss.[1]
//        let result:(string*string)list =
//            s1::s0
//        box result
//    fun (ss:obj[]) ->
//        // derive -> ":"
//        null
//    fun (ss:obj[]) ->
//        // derive -> ":" "|"
//        null
//    fun (ss:obj[]) ->
//        // file -> HEADER rules
//        let s0 = unbox<string> ss.[0]
//        let s1 = unbox<(string*((string list*string*string)list))list> ss.[1]
//        let result:string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list =
//            s0, List.rev s1,[],[]
//        box result
//    fun (ss:obj[]) ->
//        // file -> HEADER rules "%%" declarations
//        let s0 = unbox<string> ss.[0]
//        let s1 = unbox<(string*((string list*string*string)list))list> ss.[1]
//        let s3 = unbox<(string*string)list> ss.[3]
//        let result:string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list =
//            s0, List.rev s1,[],List.rev s3
//        box result
//    fun (ss:obj[]) ->
//        // file -> HEADER rules "%%" precedences
//        let s0 = unbox<string> ss.[0]
//        let s1 = unbox<(string*((string list*string*string)list))list> ss.[1]
//        let s3 = unbox<(string*string list)list> ss.[3]
//        let result:string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list =
//            s0, List.rev s1,List.rev s3,[]
//        box result
//    fun (ss:obj[]) ->
//        // file -> HEADER rules "%%" precedences "%%" declarations
//        let s0 = unbox<string> ss.[0]
//        let s1 = unbox<(string*((string list*string*string)list))list> ss.[1]
//        let s3 = unbox<(string*string list)list> ss.[3]
//        let s5 = unbox<(string*string)list> ss.[5]
//        let result:string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list =
//            s0, List.rev s1,List.rev s3,List.rev s5
//        box result
//    fun (ss:obj[]) ->
//        // precedence -> assoc symbols
//        let s0 = unbox<string> ss.[0]
//        let s1 = unbox<string list> ss.[1]
//        let result:string*string list =
//            s0,List.rev s1
//        box result
//    fun (ss:obj[]) ->
//        // precedences -> precedence
//        let s0 = unbox<string*string list> ss.[0]
//        let result:(string*string list)list =
//            [s0]
//        box result
//    fun (ss:obj[]) ->
//        // precedences -> precedences precedence
//        let s0 = unbox<(string*string list)list> ss.[0]
//        let s1 = unbox<string*string list> ss.[1]
//        let result:(string*string list)list =
//            s1::s0
//        box result
//    fun (ss:obj[]) ->
//        // rule -> ID derive bodies
//        let s0 = unbox<string> ss.[0]
//        let s2 = unbox<(string list*string*string)list> ss.[2]
//        let result:string*((string list*string*string)list) =
//            s0,List.rev s2
//        box result
//    fun (ss:obj[]) ->
//        // rules -> rule
//        let s0 = unbox<string*((string list*string*string)list)> ss.[0]
//        let result:(string*((string list*string*string)list))list =
//            [s0]
//        box result
//    fun (ss:obj[]) ->
//        // rules -> rules rule
//        let s0 = unbox<(string*((string list*string*string)list))list> ss.[0]
//        let s1 = unbox<string*((string list*string*string)list)> ss.[1]
//        let result:(string*((string list*string*string)list))list =
//            s1::s0
//        box result
//    fun (ss:obj[]) ->
//        // symbol -> ID
//        let s0 = unbox<string> ss.[0]
//        let result:string =
//            s0
//        box result
//    fun (ss:obj[]) ->
//        // symbol -> QUOTE
//        let s0 = unbox<string> ss.[0]
//        let result:string =
//            s0
//        box result
//    fun (ss:obj[]) ->
//        // symbols -> symbol
//        let s0 = unbox<string> ss.[0]
//        let result:string list =
//            [s0]
//        box result
//    fun (ss:obj[]) ->
//        // symbols -> symbols symbol
//        let s0 = unbox<string list> ss.[0]
//        let s1 = unbox<string> ss.[1]
//        let result:string list =
//            s1::s0
//        box result
//|]
//open FslexFsyacc.Runtime
//let parser = Parser(productions, closures, actions, mappers)
//let parse (tokens:seq<_>) =
//    parser.parse(tokens, getTag, getLexeme)
//    |> unbox<string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list>