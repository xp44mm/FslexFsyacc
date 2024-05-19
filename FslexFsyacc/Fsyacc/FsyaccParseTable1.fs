module FslexFsyacc.Fsyacc.FsyaccParseTable1
let actions = [["HEADER",13;"file",1];["",0];["%prec",-1;"(",-1;"ID",-1;"LITERAL",-1;"REDUCER",-1;"[",-1];["%prec",-2;"(",-2;"ID",-2;"LITERAL",-2;"REDUCER",-2;"[",-2];["%prec",-3;"(",-3;"ID",-3;"LITERAL",-3;"REDUCER",-3;"[",-3];["",-4;"%%",-4;"%left",-4;"%nonassoc",-4;"%prec",-4;"%right",-4;"%type",-4;"(",-4;")",-4;"*",-4;"+",-4;":",-4;"?",-4;"ID",-4;"LITERAL",-4;"REDUCER",-4;"[",-4;"]",-4];["",-5;"%%",-5;"%left",-5;"%nonassoc",-5;"%prec",-5;"%right",-5;"%type",-5;"(",-5;")",-5;"*",-5;"+",-5;":",-5;"?",-5;"ID",-5;"LITERAL",-5;"REDUCER",-5;"[",-5;"]",-5];["(",26;"ID",5;"LITERAL",6;"[",7;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",33;"{symbol+}",8];["(",26;"ID",5;"LITERAL",6;"[",7;"]",9;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",34];["",-6;"%%",-6;"%left",-6;"%nonassoc",-6;"%prec",-6;"%right",-6;"%type",-6;"(",-6;")",-6;"*",-6;"+",-6;":",-6;"?",-6;"ID",-6;"LITERAL",-6;"REDUCER",-6;"[",-6;"]",-6];["TYPE_ARGUMENT",11];["(",26;"ID",5;"LITERAL",6;"[",7;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",33;"{symbol+}",12];["",-7;"%%",-7;"%type",-7;"(",26;"ID",5;"LITERAL",6;"[",7;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",34];["(",26;"ID",5;"LITERAL",6;"[",7;"atomic",42;"brackets",43;"parens",44;"repetition",45;"ruleGroup",57;"symbol",32;"{ruleGroup+}",14];["",-24;"%%",15;"(",26;"ID",5;"LITERAL",6;"[",7;"atomic",42;"brackets",43;"parens",44;"repetition",45;"ruleGroup",58;"symbol",32;"{\"%%\"?}",23];["",-25;"%left",2;"%nonassoc",3;"%right",4;"%type",10;"assoc",24;"declaration",48;"operatorsLine",50;"{declaration+}",16;"{operatorsLine+}",18];["",-24;"%%",46;"%type",10;"declaration",49;"{\"%%\"?}",17];["",-8];["",-24;"%%",19;"%left",2;"%nonassoc",3;"%right",4;"assoc",24;"operatorsLine",51;"{\"%%\"?}",22];["",-25;"%type",10;"declaration",48;"{declaration+}",20];["",-24;"%%",46;"%type",10;"declaration",49;"{\"%%\"?}",21];["",-9];["",-10];["",-11];["(",26;"ID",5;"LITERAL",6;"[",7;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",33;"{symbol+}",25];["",-12;"%%",-12;"%left",-12;"%nonassoc",-12;"%right",-12;"(",26;"ID",5;"LITERAL",6;"[",7;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",34];["(",26;"ID",5;"LITERAL",6;"[",7;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",33;"{symbol+}",27];["(",26;")",28;"ID",5;"LITERAL",6;"[",7;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",34];["",-13;"%%",-13;"%left",-13;"%nonassoc",-13;"%prec",-13;"%right",-13;"%type",-13;"(",-13;")",-13;"*",-13;"+",-13;":",-13;"?",-13;"ID",-13;"LITERAL",-13;"REDUCER",-13;"[",-13;"]",-13];["",-14;"%%",-14;"%left",-14;"%nonassoc",-14;"%prec",-14;"%right",-14;"%type",-14;"(",-14;")",-14;"*",-14;"+",-14;":",-14;"?",-14;"ID",-14;"LITERAL",-14;"REDUCER",-14;"[",-14;"]",-14];["",-15;"%%",-15;"%left",-15;"%nonassoc",-15;"%prec",-15;"%right",-15;"%type",-15;"(",-15;")",-15;"*",-15;"+",-15;":",-15;"?",-15;"ID",-15;"LITERAL",-15;"REDUCER",-15;"[",-15;"]",-15];["",-16;"%%",-16;"%left",-16;"%nonassoc",-16;"%prec",-16;"%right",-16;"%type",-16;"(",-16;")",-16;"*",-16;"+",-16;":",-16;"?",-16;"ID",-16;"LITERAL",-16;"REDUCER",-16;"[",-16;"]",-16];["*",29;"+",30;":",39;"?",31;"quantifier",35];["",-40;"%%",-40;"%left",-40;"%nonassoc",-40;"%prec",-40;"%right",-40;"%type",-40;"(",-40;")",-40;"*",29;"+",30;"?",31;"ID",-40;"LITERAL",-40;"REDUCER",-40;"[",-40;"]",-40;"quantifier",35];["",-41;"%%",-41;"%left",-41;"%nonassoc",-41;"%prec",-41;"%right",-41;"%type",-41;"(",-41;")",-41;"*",29;"+",30;"?",31;"ID",-41;"LITERAL",-41;"REDUCER",-41;"[",-41;"]",-41;"quantifier",35];["",-17;"%%",-17;"%left",-17;"%nonassoc",-17;"%prec",-17;"%right",-17;"%type",-17;"(",-17;")",-17;"*",-17;"+",-17;":",-17;"?",-17;"ID",-17;"LITERAL",-17;"REDUCER",-17;"[",-17;"]",-17];["%prec",52;"REDUCER",-32;"{precToken?}",37];["REDUCER",38];["",-18;"%%",-18;"%prec",-18;"(",-18;"ID",-18;"LITERAL",-18;"REDUCER",-18;"[",-18;"|",-18];["%prec",-26;"(",-26;"ID",-26;"LITERAL",-26;"REDUCER",-26;"[",-26;"{\"|\"?}",40;"|",47];["%prec",-38;"(",26;"ID",5;"LITERAL",6;"REDUCER",-38;"[",7;"atomic",42;"brackets",43;"parens",44;"repetition",45;"ruleBody",54;"symbol",33;"{ruleBody+}",41;"{symbol*}",36;"{symbol+}",59];["",-19;"%%",-19;"%prec",-19;"(",-19;"ID",-19;"LITERAL",-19;"REDUCER",-19;"[",-19;"|",55];["",-20;"%%",-20;"%left",-20;"%nonassoc",-20;"%prec",-20;"%right",-20;"%type",-20;"(",-20;")",-20;"*",-20;"+",-20;":",-20;"?",-20;"ID",-20;"LITERAL",-20;"REDUCER",-20;"[",-20;"]",-20];["",-21;"%%",-21;"%left",-21;"%nonassoc",-21;"%prec",-21;"%right",-21;"%type",-21;"(",-21;")",-21;"*",-21;"+",-21;":",-21;"?",-21;"ID",-21;"LITERAL",-21;"REDUCER",-21;"[",-21;"]",-21];["",-22;"%%",-22;"%left",-22;"%nonassoc",-22;"%prec",-22;"%right",-22;"%type",-22;"(",-22;")",-22;"*",-22;"+",-22;":",-22;"?",-22;"ID",-22;"LITERAL",-22;"REDUCER",-22;"[",-22;"]",-22];["",-23;"%%",-23;"%left",-23;"%nonassoc",-23;"%prec",-23;"%right",-23;"%type",-23;"(",-23;")",-23;"*",-23;"+",-23;":",-23;"?",-23;"ID",-23;"LITERAL",-23;"REDUCER",-23;"[",-23;"]",-23];["",-25];["%prec",-27;"(",-27;"ID",-27;"LITERAL",-27;"REDUCER",-27;"[",-27];["",-28;"%%",-28;"%type",-28];["",-29;"%%",-29;"%type",-29];["",-30;"%%",-30;"%left",-30;"%nonassoc",-30;"%right",-30];["",-31;"%%",-31;"%left",-31;"%nonassoc",-31;"%right",-31];["ID",53];["REDUCER",-33];["",-34;"%%",-34;"%prec",-34;"(",-34;"ID",-34;"LITERAL",-34;"REDUCER",-34;"[",-34;"|",-34];["%prec",-38;"(",26;"ID",5;"LITERAL",6;"REDUCER",-38;"[",7;"atomic",42;"brackets",43;"parens",44;"repetition",45;"ruleBody",56;"symbol",33;"{symbol*}",36;"{symbol+}",59];["",-35;"%%",-35;"%prec",-35;"(",-35;"ID",-35;"LITERAL",-35;"REDUCER",-35;"[",-35;"|",-35];["",-36;"%%",-36;"%prec",-36;"(",-36;"ID",-36;"LITERAL",-36;"REDUCER",-36;"[",-36];["",-37;"%%",-37;"%prec",-37;"(",-37;"ID",-37;"LITERAL",-37;"REDUCER",-37;"[",-37];["%prec",-39;"(",26;"ID",5;"LITERAL",6;"REDUCER",-39;"[",7;"atomic",42;"brackets",43;"parens",44;"repetition",45;"symbol",34]]
let closures = [[0,0,[];-8,0,[];-9,0,[];-10,0,[];-11,0,[]];[0,1,[""]];[-1,1,["%prec";"(";"ID";"LITERAL";"REDUCER";"["]];[-2,1,["%prec";"(";"ID";"LITERAL";"REDUCER";"["]];[-3,1,["%prec";"(";"ID";"LITERAL";"REDUCER";"["]];[-4,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"REDUCER";"[";"]"]];[-5,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"REDUCER";"[";"]"]];[-4,0,[];-5,0,[];-6,0,[];-6,1,[];-13,0,[];-17,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-40,0,[];-41,0,[]];[-4,0,[];-5,0,[];-6,0,[];-6,2,[];-13,0,[];-17,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-41,1,[]];[-6,3,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"REDUCER";"[";"]"]];[-7,1,[]];[-4,0,[];-5,0,[];-6,0,[];-7,2,[];-13,0,[];-17,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-40,0,[];-41,0,[]];[-4,0,[];-5,0,[];-6,0,[];-7,3,["";"%%";"%type"];-13,0,[];-17,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-41,1,[]];[-4,0,[];-5,0,[];-6,0,[];-8,1,[];-9,1,[];-10,1,[];-11,1,[];-13,0,[];-17,0,[];-19,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-36,0,[];-37,0,[]];[-4,0,[];-5,0,[];-6,0,[];-8,2,[];-9,2,[];-10,2,[];-11,2,[];-13,0,[];-17,0,[];-19,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-24,0,[""];-25,0,[];-37,1,[]];[-1,0,[];-2,0,[];-3,0,[];-7,0,[];-8,3,[];-9,3,[];-10,3,[];-12,0,[];-25,1,[""];-28,0,[];-29,0,[];-30,0,[];-31,0,[]];[-7,0,[];-8,4,[];-24,0,[""];-25,0,[];-29,1,[]];[-8,5,[""]];[-1,0,[];-2,0,[];-3,0,[];-9,4,[];-10,4,[];-12,0,[];-24,0,[""];-25,0,[];-31,1,[]];[-7,0,[];-9,5,[];-25,1,[""];-28,0,[];-29,0,[]];[-7,0,[];-9,6,[];-24,0,[""];-25,0,[];-29,1,[]];[-9,7,[""]];[-10,5,[""]];[-11,3,[""]];[-4,0,[];-5,0,[];-6,0,[];-12,1,[];-13,0,[];-17,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-40,0,[];-41,0,[]];[-4,0,[];-5,0,[];-6,0,[];-12,2,["";"%%";"%left";"%nonassoc";"%right"];-13,0,[];-17,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-41,1,[]];[-4,0,[];-5,0,[];-6,0,[];-13,0,[];-13,1,[];-17,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-40,0,[];-41,0,[]];[-4,0,[];-5,0,[];-6,0,[];-13,0,[];-13,2,[];-17,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-41,1,[]];[-13,3,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"REDUCER";"[";"]"]];[-14,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"REDUCER";"[";"]"]];[-15,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"REDUCER";"[";"]"]];[-16,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"REDUCER";"[";"]"]];[-14,0,[];-15,0,[];-16,0,[];-17,1,[];-19,1,[]];[-14,0,[];-15,0,[];-16,0,[];-17,1,[];-40,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"ID";"LITERAL";"REDUCER";"[";"]"]];[-14,0,[];-15,0,[];-16,0,[];-17,1,[];-41,2,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"ID";"LITERAL";"REDUCER";"[";"]"]];[-17,2,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"REDUCER";"[";"]"]];[-18,1,[];-32,0,["REDUCER"];-33,0,[]];[-18,2,[]];[-18,3,["";"%%";"%prec";"(";"ID";"LITERAL";"REDUCER";"[";"|"]];[-19,2,[];-26,0,["%prec";"(";"ID";"LITERAL";"REDUCER";"["];-27,0,[]];[-4,0,[];-5,0,[];-6,0,[];-13,0,[];-17,0,[];-18,0,[];-19,3,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-34,0,[];-35,0,[];-38,0,["%prec";"REDUCER"];-39,0,[];-40,0,[];-41,0,[]];[-19,4,["";"%%";"%prec";"(";"ID";"LITERAL";"REDUCER";"["];-35,1,[]];[-20,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"REDUCER";"[";"]"]];[-21,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"REDUCER";"[";"]"]];[-22,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"REDUCER";"[";"]"]];[-23,1,["";"%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"?";"ID";"LITERAL";"REDUCER";"[";"]"]];[-25,1,[""]];[-27,1,["%prec";"(";"ID";"LITERAL";"REDUCER";"["]];[-28,1,["";"%%";"%type"]];[-29,2,["";"%%";"%type"]];[-30,1,["";"%%";"%left";"%nonassoc";"%right"]];[-31,2,["";"%%";"%left";"%nonassoc";"%right"]];[-33,1,[]];[-33,2,["REDUCER"]];[-34,1,["";"%%";"%prec";"(";"ID";"LITERAL";"REDUCER";"[";"|"]];[-4,0,[];-5,0,[];-6,0,[];-13,0,[];-17,0,[];-18,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-35,2,[];-38,0,["%prec";"REDUCER"];-39,0,[];-40,0,[];-41,0,[]];[-35,3,["";"%%";"%prec";"(";"ID";"LITERAL";"REDUCER";"[";"|"]];[-36,1,["";"%%";"%prec";"(";"ID";"LITERAL";"REDUCER";"["]];[-37,2,["";"%%";"%prec";"(";"ID";"LITERAL";"REDUCER";"["]];[-4,0,[];-5,0,[];-6,0,[];-13,0,[];-17,0,[];-20,0,[];-21,0,[];-22,0,[];-23,0,[];-39,1,["%prec";"REDUCER"];-41,1,[]]]
open FslexFsyacc.Runtime.YACCs
let rules:list<string list*(obj list->obj)> = [
    ["";"file"], fun(ss:obj list)-> ss.[0]
    ["assoc";"%left"], fun(ss:obj list)->
        let result:Associativity =
            LeftAssoc
        box result
    ["assoc";"%nonassoc"], fun(ss:obj list)->
        let result:Associativity =
            NonAssoc
        box result
    ["assoc";"%right"], fun(ss:obj list)->
        let result:Associativity =
            RightAssoc
        box result
    ["atomic";"ID"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    ["atomic";"LITERAL"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:string =
            s0
        box result
    ["brackets";"[";"{symbol+}";"]"], fun(ss:obj list)->
        let s1 = unbox<RegularSymbol list> ss.[1]
        let result:RegularSymbol list =
            List.rev s1
        box result
    ["declaration";"%type";"TYPE_ARGUMENT";"{symbol+}"], fun(ss:obj list)->
        let s1 = unbox<string> ss.[1]
        let s2 = unbox<RegularSymbol list> ss.[2]
        let result:string*string list =
            let symbols = s2 |> List.map RegularSymbolUtils.innerSymbol |> List.rev
            s1,symbols
        box result
    ["file";"HEADER";"{ruleGroup+}";"%%";"{declaration+}";"{\"%%\"?}"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<RuleGroup list> ss.[1]
        let s3 = unbox<(string*string list)list> ss.[3]
        let result:RawFsyaccFile =
            {
                header = s0
                ruleGroups = List.rev s1
                operatorsLines = []
                declarationsLines = List.rev s3
            }
        box result
    ["file";"HEADER";"{ruleGroup+}";"%%";"{operatorsLine+}";"%%";"{declaration+}";"{\"%%\"?}"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<RuleGroup list> ss.[1]
        let s3 = unbox<(Associativity*string list)list> ss.[3]
        let s5 = unbox<(string*string list)list> ss.[5]
        let result:RawFsyaccFile =
            {
                header = s0
                ruleGroups = List.rev s1
                operatorsLines = List.rev s3
                declarationsLines = List.rev s5
            }
        box result
    ["file";"HEADER";"{ruleGroup+}";"%%";"{operatorsLine+}";"{\"%%\"?}"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<RuleGroup list> ss.[1]
        let s3 = unbox<(Associativity*string list)list> ss.[3]
        let result:RawFsyaccFile =
            {
                header = s0
                ruleGroups = List.rev s1
                operatorsLines = List.rev s3
                declarationsLines = []
            }
        box result
    ["file";"HEADER";"{ruleGroup+}";"{\"%%\"?}"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<RuleGroup list> ss.[1]
        let result:RawFsyaccFile =
            {
                header = s0
                ruleGroups = List.rev s1
                operatorsLines = []
                declarationsLines = []
            }
        box result
    ["operatorsLine";"assoc";"{symbol+}"], fun(ss:obj list)->
        let s0 = unbox<Associativity> ss.[0]
        let s1 = unbox<RegularSymbol list> ss.[1]
        let result:Associativity*string list =
            let s1 = s1 |> List.map RegularSymbolUtils.innerSymbol |> List.rev
            s0,s1
        box result
    ["parens";"(";"{symbol+}";")"], fun(ss:obj list)->
        let s1 = unbox<RegularSymbol list> ss.[1]
        let result:RegularSymbol list =
            List.rev s1
        box result
    ["quantifier";"*"], fun(ss:obj list)->
        let result:string =
            "*"
        box result
    ["quantifier";"+"], fun(ss:obj list)->
        let result:string =
            "+"
        box result
    ["quantifier";"?"], fun(ss:obj list)->
        let result:string =
            "?"
        box result
    ["repetition";"symbol";"quantifier"], fun(ss:obj list)->
        let s0 = unbox<RegularSymbol> ss.[0]
        let s1 = unbox<string> ss.[1]
        let result:RegularSymbol*string =
            s0,s1
        box result
    ["ruleBody";"{symbol*}";"{precToken?}";"REDUCER"], fun(ss:obj list)->
        let s0 = unbox<RegularSymbol list> ss.[0]
        let s1 = unbox<string> ss.[1]
        let s2 = unbox<string> ss.[2]
        let result:RuleBody =
            let s0 = s0 |> List.map RegularSymbolUtils.innerSymbol |> List.rev
            { body = s0; dummy = s1; reducer = s2 }
        box result
    ["ruleGroup";"symbol";":";"{\"|\"?}";"{ruleBody+}"], fun(ss:obj list)->
        let s0 = unbox<RegularSymbol> ss.[0]
        let s3 = unbox<RuleBody list> ss.[3]
        let result:RuleGroup =
            let head = RegularSymbolUtils.innerSymbol s0
            let bodies = List.rev s3
            { head = head; bodies = bodies }
        box result
    ["symbol";"atomic"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:RegularSymbol =
            Atomic s0
        box result
    ["symbol";"brackets"], fun(ss:obj list)->
        let s0 = unbox<RegularSymbol list> ss.[0]
        let result:RegularSymbol =
            Oneof s0
        box result
    ["symbol";"parens"], fun(ss:obj list)->
        let s0 = unbox<RegularSymbol list> ss.[0]
        let result:RegularSymbol =
            Chain s0
        box result
    ["symbol";"repetition"], fun(ss:obj list)->
        let s0 = unbox<RegularSymbol*string> ss.[0]
        let result:RegularSymbol =
            match s0 with (f,q) ->
            Repetition(f,q)
        box result
    ["{\"%%\"?}"], fun(ss:obj list)->
        null
    ["{\"%%\"?}";"%%"], fun(ss:obj list)->
        null
    ["{\"|\"?}"], fun(ss:obj list)->
        null
    ["{\"|\"?}";"|"], fun(ss:obj list)->
        null
    ["{declaration+}";"declaration"], fun(ss:obj list)->
        let s0 = unbox<string*string list> ss.[0]
        let result:(string*string list)list =
            [s0]
        box result
    ["{declaration+}";"{declaration+}";"declaration"], fun(ss:obj list)->
        let s0 = unbox<(string*string list)list> ss.[0]
        let s1 = unbox<string*string list> ss.[1]
        let result:(string*string list)list =
            s1::s0
        box result
    ["{operatorsLine+}";"operatorsLine"], fun(ss:obj list)->
        let s0 = unbox<Associativity*string list> ss.[0]
        let result:(Associativity*string list)list =
            [s0]
        box result
    ["{operatorsLine+}";"{operatorsLine+}";"operatorsLine"], fun(ss:obj list)->
        let s0 = unbox<(Associativity*string list)list> ss.[0]
        let s1 = unbox<Associativity*string list> ss.[1]
        let result:(Associativity*string list)list =
            s1::s0
        box result
    ["{precToken?}"], fun(ss:obj list)->
        let result:string =
            ""
        box result
    ["{precToken?}";"%prec";"ID"], fun(ss:obj list)->
        let s1 = unbox<string> ss.[1]
        let result:string =
            s1
        box result
    ["{ruleBody+}";"ruleBody"], fun(ss:obj list)->
        let s0 = unbox<RuleBody> ss.[0]
        let result:RuleBody list =
            [s0]
        box result
    ["{ruleBody+}";"{ruleBody+}";"|";"ruleBody"], fun(ss:obj list)->
        let s0 = unbox<RuleBody list> ss.[0]
        let s2 = unbox<RuleBody> ss.[2]
        let result:RuleBody list =
            s2::s0
        box result
    ["{ruleGroup+}";"ruleGroup"], fun(ss:obj list)->
        let s0 = unbox<RuleGroup> ss.[0]
        let result:RuleGroup list =
            [s0]
        box result
    ["{ruleGroup+}";"{ruleGroup+}";"ruleGroup"], fun(ss:obj list)->
        let s0 = unbox<RuleGroup list> ss.[0]
        let s1 = unbox<RuleGroup> ss.[1]
        let result:RuleGroup list =
            s1::s0
        box result
    ["{symbol*}"], fun(ss:obj list)->
        let result:RegularSymbol list =
            []
        box result
    ["{symbol*}";"{symbol+}"], fun(ss:obj list)->
        let s0 = unbox<RegularSymbol list> ss.[0]
        let result:RegularSymbol list =
            s0
        box result
    ["{symbol+}";"symbol"], fun(ss:obj list)->
        let s0 = unbox<RegularSymbol> ss.[0]
        let result:RegularSymbol list =
            [s0]
        box result
    ["{symbol+}";"{symbol+}";"symbol"], fun(ss:obj list)->
        let s0 = unbox<RegularSymbol list> ss.[0]
        let s1 = unbox<RegularSymbol> ss.[1]
        let result:RegularSymbol list =
            s1::s0
        box result
]
let unboxRoot =
    unbox<RawFsyaccFile>
let baseParser = FslexFsyacc.Runtime.BaseParser.create(rules, actions, closures)
let stateSymbolPairs = baseParser.getStateSymbolPairs()