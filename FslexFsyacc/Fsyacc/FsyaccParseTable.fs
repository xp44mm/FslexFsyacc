module FslexFsyacc.Fsyacc.FsyaccParseTable
let actions = [["HEADER",16;"file",1];["",0];["%prec",-1;"(",-1;"ID",-1;"LITERAL",-1;"SEMANTIC",-1;"[",-1];["%prec",-2;"(",-2;"ID",-2;"LITERAL",-2;"SEMANTIC",-2;"[",-2];["%prec",-3;"(",-3;"ID",-3;"LITERAL",-3;"SEMANTIC",-3;"[",-3];["",-4;"%%",-4;"%left",-4;"%nonassoc",-4;"%prec",-4;"%right",-4;"%type",-4;"(",-4;")",-4;"*",-4;"+",-4;":",-4;"?",-4;"ID",-4;"LITERAL",-4;"SEMANTIC",-4;"[",-4;"]",-4];["",-5;"%%",-5;"%left",-5;"%nonassoc",-5;"%prec",-5;"%right",-5;"%type",-5;"(",-5;")",-5;"*",-5;"+",-5;":",-5;"?",-5;"ID",-5;"LITERAL",-5;"SEMANTIC",-5;"[",-5;"]",-5];["%prec",53;"SEMANTIC",-32;"{precToken?}",8];["SEMANTIC",9];["",-6;"%%",-6;"%prec",-6;"(",-6;"ID",-6;"LITERAL",-6;"SEMANTIC",-6;"[",-6;"|",-6];["(",27;"ID",5;"LITERAL",6;"[",10;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",36;"{symbol+}",11];["(",27;"ID",5;"LITERAL",6;"[",10;"]",12;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",37];["",-7;"%%",-7;"%left",-7;"%nonassoc",-7;"%prec",-7;"%right",-7;"%type",-7;"(",-7;")",-7;"*",-7;"+",-7;":",-7;"?",-7;"ID",-7;"LITERAL",-7;"SEMANTIC",-7;"[",-7;"]",-7];["TYPE_ARGUMENT",14];["(",27;"ID",5;"LITERAL",6;"[",10;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",36;"{symbol+}",15];["",-8;"%%",-8;"%type",-8;"(",27;"ID",5;"LITERAL",6;"[",10;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",37];["(",27;"ID",5;"LITERAL",6;"[",10;"atomic",42;"brackets",43;"parens",44;"repetition",45;"rule",57;"symbol",35;"{rule+}",17];["",-24;"%%",18;"(",27;"ID",5;"LITERAL",6;"[",10;"atomic",42;"brackets",43;"parens",44;"repetition",45;"rule",58;"symbol",35;"{\"%%\"?}",26];["",-25;"%left",2;"%nonassoc",3;"%right",4;"%type",13;"assoc",30;"declaration",51;"precedence",55;"{declaration+}",19;"{precedence+}",21];["",-24;"%%",46;"%type",13;"declaration",52;"{\"%%\"?}",20];["",-9];["",-24;"%%",22;"%left",2;"%nonassoc",3;"%right",4;"assoc",30;"precedence",56;"{\"%%\"?}",25];["",-25;"%type",13;"declaration",51;"{declaration+}",23];["",-24;"%%",46;"%type",13;"declaration",52;"{\"%%\"?}",24];["",-10];["",-11];["",-12];["(",27;"ID",5;"LITERAL",6;"[",10;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",36;"{symbol+}",28];["(",27;")",29;"ID",5;"LITERAL",6;"[",10;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",37];["",-13;"%%",-13;"%left",-13;"%nonassoc",-13;"%prec",-13;"%right",-13;"%type",-13;"(",-13;")",-13;"*",-13;"+",-13;":",-13;"?",-13;"ID",-13;"LITERAL",-13;"SEMANTIC",-13;"[",-13;"]",-13];["(",27;"ID",5;"LITERAL",6;"[",10;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",36;"{symbol+}",31];["",-14;"%%",-14;"%left",-14;"%nonassoc",-14;"%right",-14;"(",27;"ID",5;"LITERAL",6;"[",10;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",37];["",-15;"%%",-15;"%left",-15;"%nonassoc",-15;"%prec",-15;"%right",-15;"%type",-15;"(",-15;")",-15;"*",-15;"+",-15;":",-15;"?",-15;"ID",-15;"LITERAL",-15;"SEMANTIC",-15;"[",-15;"]",-15];["",-16;"%%",-16;"%left",-16;"%nonassoc",-16;"%prec",-16;"%right",-16;"%type",-16;"(",-16;")",-16;"*",-16;"+",-16;":",-16;"?",-16;"ID",-16;"LITERAL",-16;"SEMANTIC",-16;"[",-16;"]",-16];["",-17;"%%",-17;"%left",-17;"%nonassoc",-17;"%prec",-17;"%right",-17;"%type",-17;"(",-17;")",-17;"*",-17;"+",-17;":",-17;"?",-17;"ID",-17;"LITERAL",-17;"SEMANTIC",-17;"[",-17;"]",-17];["*",32;"+",33;":",39;"?",34;"quantifier",38];["",-40;"%%",-40;"%left",-40;"%nonassoc",-40;"%prec",-40;"%right",-40;"%type",-40;"(",-40;")",-40;"*",32;"+",33;"?",34;"ID",-40;"LITERAL",-40;"SEMANTIC",-40;"[",-40;"]",-40;"quantifier",38];["",-41;"%%",-41;"%left",-41;"%nonassoc",-41;"%prec",-41;"%right",-41;"%type",-41;"(",-41;")",-41;"*",32;"+",33;"?",34;"ID",-41;"LITERAL",-41;"SEMANTIC",-41;"[",-41;"]",-41;"quantifier",38];["",-18;"%%",-18;"%left",-18;"%nonassoc",-18;"%prec",-18;"%right",-18;"%type",-18;"(",-18;")",-18;"*",-18;"+",-18;":",-18;"?",-18;"ID",-18;"LITERAL",-18;"SEMANTIC",-18;"[",-18;"]",-18];["%prec",-26;"(",-26;"ID",-26;"LITERAL",-26;"SEMANTIC",-26;"[",-26;"{\"|\"?}",40;"|",47];["%prec",-38;"(",27;"ID",5;"LITERAL",6;"SEMANTIC",-38;"[",10;"atomic",42;"body",48;"brackets",43;"parens",44;"repetition",45;"symbol",36;"{body+}",41;"{symbol*}",7;"{symbol+}",59];["",-19;"%%",-19;"%prec",-19;"(",-19;"ID",-19;"LITERAL",-19;"SEMANTIC",-19;"[",-19;"|",49];["",-20;"%%",-20;"%left",-20;"%nonassoc",-20;"%prec",-20;"%right",-20;"%type",-20;"(",-20;")",-20;"*",-20;"+",-20;":",-20;"?",-20;"ID",-20;"LITERAL",-20;"SEMANTIC",-20;"[",-20;"]",-20];["",-21;"%%",-21;"%left",-21;"%nonassoc",-21;"%prec",-21;"%right",-21;"%type",-21;"(",-21;")",-21;"*",-21;"+",-21;":",-21;"?",-21;"ID",-21;"LITERAL",-21;"SEMANTIC",-21;"[",-21;"]",-21];["",-22;"%%",-22;"%left",-22;"%nonassoc",-22;"%prec",-22;"%right",-22;"%type",-22;"(",-22;")",-22;"*",-22;"+",-22;":",-22;"?",-22;"ID",-22;"LITERAL",-22;"SEMANTIC",-22;"[",-22;"]",-22];["",-23;"%%",-23;"%left",-23;"%nonassoc",-23;"%prec",-23;"%right",-23;"%type",-23;"(",-23;")",-23;"*",-23;"+",-23;":",-23;"?",-23;"ID",-23;"LITERAL",-23;"SEMANTIC",-23;"[",-23;"]",-23];["",-25];["%prec",-27;"(",-27;"ID",-27;"LITERAL",-27;"SEMANTIC",-27;"[",-27];["",-28;"%%",-28;"%prec",-28;"(",-28;"ID",-28;"LITERAL",-28;"SEMANTIC",-28;"[",-28;"|",-28];["%prec",-38;"(",27;"ID",5;"LITERAL",6;"SEMANTIC",-38;"[",10;"atomic",42;"body",50;"brackets",43;"parens",44;"repetition",45;"symbol",36;"{symbol*}",7;"{symbol+}",59];["",-29;"%%",-29;"%prec",-29;"(",-29;"ID",-29;"LITERAL",-29;"SEMANTIC",-29;"[",-29;"|",-29];["",-30;"%%",-30;"%type",-30];["",-31;"%%",-31;"%type",-31];["ID",54];["SEMANTIC",-33];["",-34;"%%",-34;"%left",-34;"%nonassoc",-34;"%right",-34];["",-35;"%%",-35;"%left",-35;"%nonassoc",-35;"%right",-35];["",-36;"%%",-36;"%prec",-36;"(",-36;"ID",-36;"LITERAL",-36;"SEMANTIC",-36;"[",-36];["",-37;"%%",-37;"%prec",-37;"(",-37;"ID",-37;"LITERAL",-37;"SEMANTIC",-37;"[",-37];["%prec",-39;"(",27;"ID",5;"LITERAL",6;"SEMANTIC",-39;"[",10;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",37]]
let closures = [[0,0,[];-9,0,[];-10,0,[];-11,0,[];-12,0,[]];[0,1,[""]];[-1,1,["%prec";"(";"ID";"LITERAL";"SEMANTIC";"["]];[-2,1,["%prec";"(";"ID";"LITERAL";"SEMANTIC";"["]];[-3,1,["%prec";"(";"ID";"LITERAL";"SEMANTIC";"["]];[-4,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"SEMANTIC";"[";"]"]];[-5,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"SEMANTIC";"[";"]"]];[-6,1,[];-32,0,["SEMANTIC"];-33,0,[]];[-6,2,[]];[-6,3,["";"%%";"%prec";"(";"ID";"LITERAL";"SEMANTIC";"[";"|"]];[-4,0,[];-5,0,[];-7,0,[];-7,1,[];-13,0,[];-18,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-40,0,[];-41,0,[]];[-4,0,[];-5,0,[];-7,0,[];-7,2,[];-13,0,[];-18,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-41,1,[]];[-7,3,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"SEMANTIC";"[";"]"]];[-8,1,[]];[-4,0,[];-5,0,[];-7,0,[];-8,2,[];-13,0,[];-18,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-40,0,[];-41,0,[]];[-4,0,[];-5,0,[];-7,0,[];-8,3,["";"%%";"%type"];-13,0,[];-18,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-41,1,[]];[-4,0,[];-5,0,[];-7,0,[];-9,1,[];-10,1,[];-11,1,[];-12,1,[];-13,0,[];-18,0,[];-19,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-36,0,[];-37,0,[]];[-4,0,[];-5,0,[];-7,0,[];-9,2,[];-10,2,[];-11,2,[];-12,2,[];-13,0,[];-18,0,[];-19,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-24,0,[""];-25,0,[];-37,1,[]];[-1,0,[];-2,0,[];-3,0,[];-8,0,[];-9,3,[];-10,3,[];-11,3,[];-14,0,[];-25,1,[""];-30,0,[];-31,0,[];-34,0,[];-35,0,[]];[-8,0,[];-9,4,[];-24,0,[""];-25,0,[];-31,1,[]];[-9,5,[""]];[-1,0,[];-2,0,[];-3,0,[];-10,4,[];-11,4,[];-14,0,[];-24,0,[""];-25,0,[];-35,1,[]];[-8,0,[];-10,5,[];-25,1,[""];-30,0,[];-31,0,[]];[-8,0,[];-10,6,[];-24,0,[""];-25,0,[];-31,1,[]];[-10,7,[""]];[-11,5,[""]];[-12,3,[""]];[-4,0,[];-5,0,[];-7,0,[];-13,0,[];-13,1,[];-18,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-40,0,[];-41,0,[]];[-4,0,[];-5,0,[];-7,0,[];-13,0,[];-13,2,[];-18,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-41,1,[]];[-13,3,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"SEMANTIC";"[";"]"]];[-4,0,[];-5,0,[];-7,0,[];-13,0,[];-14,1,[];-18,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-40,0,[];-41,0,[]];[-4,0,[];-5,0,[];-7,0,[];-13,0,[];-14,2,["";"%%";"%left";"%nonassoc";"%right"];-18,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-41,1,[]];[-15,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"SEMANTIC";"[";"]"]];[-16,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"SEMANTIC";"[";"]"]];[-17,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"SEMANTIC";"[";"]"]];[-15,0,[];-16,0,[];-17,0,[];-18,1,[];-19,1,[]];[-15,0,[];-16,0,[];-17,0,[];-18,1,[];-40,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"ID";"LITERAL";"SEMANTIC";"[";"]"]];[-15,0,[];-16,0,[];-17,0,[];-18,1,[];-41,2,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"ID";"LITERAL";"SEMANTIC";"[";"]"]];[-18,2,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"SEMANTIC";"[";"]"]];[-19,2,[];-26,0,["%prec";"(";"ID";"LITERAL";"SEMANTIC";"["];-27,0,[]];[-4,0,[];-5,0,[];-6,0,[];-7,0,[];-13,0,[];-18,0,[];-19,3,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-28,0,[];-29,0,[];-38,0,["%prec";"SEMANTIC"];-39,0,[];-40,0,[];-41,0,[]];[-19,4,["";"%%";"%prec";"(";"ID";"LITERAL";"SEMANTIC";"["];-29,1,[]];[-20,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"SEMANTIC";"[";"]"]];[-21,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"SEMANTIC";"[";"]"]];[-22,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"SEMANTIC";"[";"]"]];[-23,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"SEMANTIC";"[";"]"]];[-25,1,[""]];[-27,1,["%prec";"(";"ID";"LITERAL";"SEMANTIC";"["]];[-28,1,["";"%%";"%prec";"(";"ID";"LITERAL";"SEMANTIC";"[";"|"]];[-4,0,[];-5,0,[];-6,0,[];-7,0,[];-13,0,[];-18,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-29,2,[];-38,0,["%prec";"SEMANTIC"];-39,0,[];-40,0,[];-41,0,[]];[-29,3,["";"%%";"%prec";"(";"ID";"LITERAL";"SEMANTIC";"[";"|"]];[-30,1,["";"%%";"%type"]];[-31,2,["";"%%";"%type"]];[-33,1,[]];[-33,2,["SEMANTIC"]];[-34,1,["";"%%";"%left";"%nonassoc";"%right"]];[-35,2,["";"%%";"%left";"%nonassoc";"%right"]];[-36,1,["";"%%";"%prec";"(";"ID";"LITERAL";"SEMANTIC";"["]];[-37,2,["";"%%";"%prec";"(";"ID";"LITERAL";"SEMANTIC";"["]];[-4,0,[];-5,0,[];-7,0,[];-13,0,[];-18,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-39,1,["%prec";"SEMANTIC"];-41,1,[]]]

let rules:(string list*(obj list->obj))list = [
    ["file";"HEADER";"{rule+}";"{\"%%\"?}"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(string*((string list*string*string)list))list> ss.[1]
        let result:RawFsyaccFile =
            {
                header= s0;
                rules= List.rev s1;
                precedences= [];
                declarations= []
            }
        box result
    ["file";"HEADER";"{rule+}";"%%";"{precedence+}";"{\"%%\"?}"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(string*((string list*string*string)list))list> ss.[1]
        let s3 = unbox<(string*string list)list> ss.[3]
        let result:RawFsyaccFile =
            {
                header= s0;
                rules= List.rev s1;
                precedences= List.rev s3;
                declarations= []
            }
        box result
    ["file";"HEADER";"{rule+}";"%%";"{declaration+}";"{\"%%\"?}"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(string*((string list*string*string)list))list> ss.[1]
        let s3 = unbox<(string*string list)list> ss.[3]
        let result:RawFsyaccFile =
            {
                header= s0;
                rules= List.rev s1;
                precedences= [];
                declarations= List.rev s3
            }
        box result
    ["file";"HEADER";"{rule+}";"%%";"{precedence+}";"%%";"{declaration+}";"{\"%%\"?}"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<(string*((string list*string*string)list))list> ss.[1]
        let s3 = unbox<(string*string list)list> ss.[3]
        let s5 = unbox<(string*string list)list> ss.[5]
        let result:RawFsyaccFile =
            {
                header= s0;
                rules= List.rev s1;
                precedences= List.rev s3;
                declarations= List.rev s5
            }
        box result
    ["{rule+}";"rule"],fun(ss:obj list)->
        let s0 = unbox<string*((string list*string*string)list)> ss.[0]
        let result:(string*((string list*string*string)list))list =
            [s0]
        box result
    ["{rule+}";"{rule+}";"rule"],fun(ss:obj list)->
        let s0 = unbox<(string*((string list*string*string)list))list> ss.[0]
        let s1 = unbox<string*((string list*string*string)list)> ss.[1]
        let result:(string*((string list*string*string)list))list =
            s1::s0
        box result
    ["rule";"symbol";":";"{\"|\"?}";"{body+}"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol> ss.[0]
        let s3 = unbox<(string list*string*string)list> ss.[3]
        let result:string*((string list*string*string)list) =
            RegularSymbol.innerSymbol s0,List.rev s3
        box result
    ["symbol";"atomic"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:RegularSymbol =
            Atomic s0
        box result
    ["symbol";"repetition"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol*string> ss.[0]
        let result:RegularSymbol =
            match s0 with (f,q) ->
            Repetition(f,q)
        box result
    ["symbol";"brackets"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol list> ss.[0]
        let result:RegularSymbol =
            Oneof s0
        box result
    ["symbol";"parens"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol list> ss.[0]
        let result:RegularSymbol =
            Chain s0
        box result
    ["atomic";"ID"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    ["atomic";"LITERAL"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    ["repetition";"symbol";"quantifier"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol> ss.[0]
        let s1 = unbox<string> ss.[1]
        let result:RegularSymbol*string =
            s0,s1
        box result
    ["quantifier";"?"],fun(ss:obj list)->
        let result:string =
            "?"
        box result
    ["quantifier";"+"],fun(ss:obj list)->
        let result:string =
            "+"
        box result
    ["quantifier";"*"],fun(ss:obj list)->
        let result:string =
            "*"
        box result
    ["brackets";"[";"{symbol+}";"]"],fun(ss:obj list)->
        let s1 = unbox<RegularSymbol list> ss.[1]
        let result:RegularSymbol list =
            List.rev s1
        box result
    ["{symbol+}";"symbol"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol> ss.[0]
        let result:RegularSymbol list =
            [s0]
        box result
    ["{symbol+}";"{symbol+}";"symbol"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol list> ss.[0]
        let s1 = unbox<RegularSymbol> ss.[1]
        let result:RegularSymbol list =
            s1::s0
        box result
    ["parens";"(";"{symbol+}";")"],fun(ss:obj list)->
        let s1 = unbox<RegularSymbol list> ss.[1]
        let result:RegularSymbol list =
            List.rev s1
        box result
    ["{\"|\"?}"],fun(ss:obj list)->
        null
    ["{\"|\"?}";"|"],fun(ss:obj list)->
        null
    ["{body+}";"body"],fun(ss:obj list)->
        let s0 = unbox<string list*string*string> ss.[0]
        let result:(string list*string*string)list =
            [s0]
        box result
    ["{body+}";"{body+}";"|";"body"],fun(ss:obj list)->
        let s0 = unbox<(string list*string*string)list> ss.[0]
        let s2 = unbox<string list*string*string> ss.[2]
        let result:(string list*string*string)list =
            s2::s0
        box result
    ["body";"{symbol*}";"{precToken?}";"SEMANTIC"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol list> ss.[0]
        let s1 = unbox<string> ss.[1]
        let s2 = unbox<string> ss.[2]
        let result:string list*string*string =
            let s0 = s0 |> List.map RegularSymbol.innerSymbol |> List.rev
            s0,s1,s2
        box result
    ["{symbol*}"],fun(ss:obj list)->
        let result:RegularSymbol list =
            []
        box result
    ["{symbol*}";"{symbol+}"],fun(ss:obj list)->
        let s0 = unbox<RegularSymbol list> ss.[0]
        let result:RegularSymbol list =
            s0
        box result
    ["{precToken?}"],fun(ss:obj list)->
        let result:string =
            ""
        box result
    ["{precToken?}";"%prec";"ID"],fun(ss:obj list)->
        let s1 = unbox<string> ss.[1]
        let result:string =
            s1
        box result
    ["{\"%%\"?}"],fun(ss:obj list)->
        null
    ["{\"%%\"?}";"%%"],fun(ss:obj list)->
        null
    ["{precedence+}";"precedence"],fun(ss:obj list)->
        let s0 = unbox<string*string list> ss.[0]
        let result:(string*string list)list =
            [s0]
        box result
    ["{precedence+}";"{precedence+}";"precedence"],fun(ss:obj list)->
        let s0 = unbox<(string*string list)list> ss.[0]
        let s1 = unbox<string*string list> ss.[1]
        let result:(string*string list)list =
            s1::s0
        box result
    ["precedence";"assoc";"{symbol+}"],fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<RegularSymbol list> ss.[1]
        let result:string*string list =
            let s1 = s1 |> List.map RegularSymbol.innerSymbol |> List.rev
            s0,s1
        box result
    ["assoc";"%left"],fun(ss:obj list)->
        let result:string =
            "left"
        box result
    ["assoc";"%right"],fun(ss:obj list)->
        let result:string =
            "right"
        box result
    ["assoc";"%nonassoc"],fun(ss:obj list)->
        let result:string =
            "nonassoc"
        box result
    ["{declaration+}";"declaration"],fun(ss:obj list)->
        let s0 = unbox<string*string list> ss.[0]
        let result:(string*string list)list =
            [s0]
        box result
    ["{declaration+}";"{declaration+}";"declaration"],fun(ss:obj list)->
        let s0 = unbox<(string*string list)list> ss.[0]
        let s1 = unbox<string*string list> ss.[1]
        let result:(string*string list)list =
            s1::s0
        box result
    ["declaration";"%type";"TYPE_ARGUMENT";"{symbol+}"],fun(ss:obj list)->
        let s1 = unbox<string> ss.[1]
        let s2 = unbox<RegularSymbol list> ss.[2]
        let result:string*string list =
            let symbols = s2 |> List.map RegularSymbol.innerSymbol |> List.rev
            s1,symbols
        box result
]
let unboxRoot =
    unbox<RawFsyaccFile>
let theoryParser = FslexFsyacc.Runtime.TheoryParser.create(rules, actions, closures)
let stateSymbolPairs = theoryParser.getStateSymbolPairs()