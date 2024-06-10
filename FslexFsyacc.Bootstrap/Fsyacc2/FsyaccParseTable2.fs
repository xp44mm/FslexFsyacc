module FslexFsyacc.Fsyacc.FsyaccParseTable2
let tokens = set ["%%";"%left";"%nonassoc";"%prec";"%right";"%type";"(";")";"*";"+";":";"<";">";"?";"HEADER";"ID";"LITERAL";"REDUCER";"TYPE_ARGUMENT";"[";"]";"|"]
let kernels = [[0,0];[0,1];[-1,1];[-2,1];[-3,1];[-4,1];[-5,1];[-6,1];[-6,2;-41,1];[-6,3];[-7,1];[-7,2];[-7,3];[-7,4];[-7,5;-41,1];[-8,1;-9,1;-10,1;-11,1];[-8,2;-9,2;-10,2;-11,2;-37,1];[-8,3;-9,3;-10,3;-25,1];[-8,4;-29,1];[-8,5];[-9,4;-10,4;-31,1];[-9,5;-25,1];[-9,6;-29,1];[-9,7];[-10,5];[-11,3];[-12,1];[-12,2;-41,1];[-13,1];[-13,2;-41,1];[-13,3];[-14,1];[-15,1];[-16,1];[-17,1;-19,1];[-17,1;-40,1];[-17,1;-41,2];[-17,2];[-18,1];[-18,2];[-18,3];[-19,2];[-19,3];[-19,4;-35,1];[-20,1];[-21,1];[-22,1];[-23,1];[-25,1];[-27,1];[-28,1];[-29,2];[-30,1];[-31,2];[-33,1];[-33,2];[-34,1];[-35,2];[-35,3];[-36,1];[-37,2];[-39,1;-41,1]]
let kernelSymbols = ["";"file";"%left";"%nonassoc";"%right";"ID";"LITERAL";"[";"{symbol+}";"]";"%type";"<";"TYPE_ARGUMENT";">";"{symbol+}";"HEADER";"{ruleGroup+}";"%%";"{declaration+}";"{\"%%\"?}";"{operatorsLine+}";"%%";"{declaration+}";"{\"%%\"?}";"{\"%%\"?}";"{\"%%\"?}";"assoc";"{symbol+}";"(";"{symbol+}";")";"*";"+";"?";"symbol";"symbol";"symbol";"quantifier";"{symbol*}";"{precToken?}";"REDUCER";":";"{\"|\"?}";"{ruleBody+}";"atomic";"brackets";"parens";"repetition";"%%";"|";"declaration";"declaration";"operatorsLine";"operatorsLine";"%prec";"ID";"ruleBody";"|";"ruleBody";"ruleGroup";"ruleGroup";"{symbol+}"]
let actions = [["HEADER",15;"file",1];["",0];["%prec",-1;"(",-1;"ID",-1;"LITERAL",-1;"REDUCER",-1;"[",-1];["%prec",-2;"(",-2;"ID",-2;"LITERAL",-2;"REDUCER",-2;"[",-2];["%prec",-3;"(",-3;"ID",-3;"LITERAL",-3;"REDUCER",-3;"[",-3];["",-4;"%%",-4;"%left",-4;"%nonassoc",-4;"%prec",-4;"%right",-4;"%type",-4;"(",-4;")",-4;"*",-4;"+",-4;":",-4;"?",-4;"ID",-4;"LITERAL",-4;"REDUCER",-4;"[",-4;"]",-4];["",-5;"%%",-5;"%left",-5;"%nonassoc",-5;"%prec",-5;"%right",-5;"%type",-5;"(",-5;")",-5;"*",-5;"+",-5;":",-5;"?",-5;"ID",-5;"LITERAL",-5;"REDUCER",-5;"[",-5;"]",-5];["(",28;"ID",5;"LITERAL",6;"[",7;"atomic",44;"brackets",45;"parens",46;"repetition",47;"symbol",35;"{symbol+}",8];["(",28;"ID",5;"LITERAL",6;"[",7;"]",9;"atomic",44;"brackets",45;"parens",46;"repetition",47;"symbol",36];["",-6;"%%",-6;"%left",-6;"%nonassoc",-6;"%prec",-6;"%right",-6;"%type",-6;"(",-6;")",-6;"*",-6;"+",-6;":",-6;"?",-6;"ID",-6;"LITERAL",-6;"REDUCER",-6;"[",-6;"]",-6];["<",11];["TYPE_ARGUMENT",12];[">",13];["(",28;"ID",5;"LITERAL",6;"[",7;"atomic",44;"brackets",45;"parens",46;"repetition",47;"symbol",35;"{symbol+}",14];["",-7;"%%",-7;"%type",-7;"(",28;"ID",5;"LITERAL",6;"[",7;"atomic",44;"brackets",45;"parens",46;"repetition",47;"symbol",36];["(",28;"ID",5;"LITERAL",6;"[",7;"atomic",44;"brackets",45;"parens",46;"repetition",47;"ruleGroup",59;"symbol",34;"{ruleGroup+}",16];["",-24;"%%",17;"(",28;"ID",5;"LITERAL",6;"[",7;"atomic",44;"brackets",45;"parens",46;"repetition",47;"ruleGroup",60;"symbol",34;"{\"%%\"?}",25];["",-25;"%left",2;"%nonassoc",3;"%right",4;"%type",10;"assoc",26;"declaration",50;"operatorsLine",52;"{declaration+}",18;"{operatorsLine+}",20];["",-24;"%%",48;"%type",10;"declaration",51;"{\"%%\"?}",19];["",-8];["",-24;"%%",21;"%left",2;"%nonassoc",3;"%right",4;"assoc",26;"operatorsLine",53;"{\"%%\"?}",24];["",-25;"%type",10;"declaration",50;"{declaration+}",22];["",-24;"%%",48;"%type",10;"declaration",51;"{\"%%\"?}",23];["",-9];["",-10];["",-11];["(",28;"ID",5;"LITERAL",6;"[",7;"atomic",44;"brackets",45;"parens",46;"repetition",47;"symbol",35;"{symbol+}",27];["",-12;"%%",-12;"%left",-12;"%nonassoc",-12;"%right",-12;"(",28;"ID",5;"LITERAL",6;"[",7;"atomic",44;"brackets",45;"parens",46;"repetition",47;"symbol",36];["(",28;"ID",5;"LITERAL",6;"[",7;"atomic",44;"brackets",45;"parens",46;"repetition",47;"symbol",35;"{symbol+}",29];["(",28;")",30;"ID",5;"LITERAL",6;"[",7;"atomic",44;"brackets",45;"parens",46;"repetition",47;"symbol",36];["",-13;"%%",-13;"%left",-13;"%nonassoc",-13;"%prec",-13;"%right",-13;"%type",-13;"(",-13;")",-13;"*",-13;"+",-13;":",-13;"?",-13;"ID",-13;"LITERAL",-13;"REDUCER",-13;"[",-13;"]",-13];["",-14;"%%",-14;"%left",-14;"%nonassoc",-14;"%prec",-14;"%right",-14;"%type",-14;"(",-14;")",-14;"*",-14;"+",-14;":",-14;"?",-14;"ID",-14;"LITERAL",-14;"REDUCER",-14;"[",-14;"]",-14];["",-15;"%%",-15;"%left",-15;"%nonassoc",-15;"%prec",-15;"%right",-15;"%type",-15;"(",-15;")",-15;"*",-15;"+",-15;":",-15;"?",-15;"ID",-15;"LITERAL",-15;"REDUCER",-15;"[",-15;"]",-15];["",-16;"%%",-16;"%left",-16;"%nonassoc",-16;"%prec",-16;"%right",-16;"%type",-16;"(",-16;")",-16;"*",-16;"+",-16;":",-16;"?",-16;"ID",-16;"LITERAL",-16;"REDUCER",-16;"[",-16;"]",-16];["*",31;"+",32;":",41;"?",33;"quantifier",37];["",-40;"%%",-40;"%left",-40;"%nonassoc",-40;"%prec",-40;"%right",-40;"%type",-40;"(",-40;")",-40;"*",31;"+",32;"?",33;"ID",-40;"LITERAL",-40;"REDUCER",-40;"[",-40;"]",-40;"quantifier",37];["",-41;"%%",-41;"%left",-41;"%nonassoc",-41;"%prec",-41;"%right",-41;"%type",-41;"(",-41;")",-41;"*",31;"+",32;"?",33;"ID",-41;"LITERAL",-41;"REDUCER",-41;"[",-41;"]",-41;"quantifier",37];["",-17;"%%",-17;"%left",-17;"%nonassoc",-17;"%prec",-17;"%right",-17;"%type",-17;"(",-17;")",-17;"*",-17;"+",-17;":",-17;"?",-17;"ID",-17;"LITERAL",-17;"REDUCER",-17;"[",-17;"]",-17];["%prec",54;"REDUCER",-32;"{precToken?}",39];["REDUCER",40];["",-18;"%%",-18;"%prec",-18;"(",-18;"ID",-18;"LITERAL",-18;"REDUCER",-18;"[",-18;"|",-18];["%prec",-26;"(",-26;"ID",-26;"LITERAL",-26;"REDUCER",-26;"[",-26;"{\"|\"?}",42;"|",49];["%prec",-38;"(",28;"ID",5;"LITERAL",6;"REDUCER",-38;"[",7;"atomic",44;"brackets",45;"parens",46;"repetition",47;"ruleBody",56;"symbol",35;"{ruleBody+}",43;"{symbol*}",38;"{symbol+}",61];["",-19;"%%",-19;"%prec",-19;"(",-19;"ID",-19;"LITERAL",-19;"REDUCER",-19;"[",-19;"|",57];["",-20;"%%",-20;"%left",-20;"%nonassoc",-20;"%prec",-20;"%right",-20;"%type",-20;"(",-20;")",-20;"*",-20;"+",-20;":",-20;"?",-20;"ID",-20;"LITERAL",-20;"REDUCER",-20;"[",-20;"]",-20];["",-21;"%%",-21;"%left",-21;"%nonassoc",-21;"%prec",-21;"%right",-21;"%type",-21;"(",-21;")",-21;"*",-21;"+",-21;":",-21;"?",-21;"ID",-21;"LITERAL",-21;"REDUCER",-21;"[",-21;"]",-21];["",-22;"%%",-22;"%left",-22;"%nonassoc",-22;"%prec",-22;"%right",-22;"%type",-22;"(",-22;")",-22;"*",-22;"+",-22;":",-22;"?",-22;"ID",-22;"LITERAL",-22;"REDUCER",-22;"[",-22;"]",-22];["",-23;"%%",-23;"%left",-23;"%nonassoc",-23;"%prec",-23;"%right",-23;"%type",-23;"(",-23;")",-23;"*",-23;"+",-23;":",-23;"?",-23;"ID",-23;"LITERAL",-23;"REDUCER",-23;"[",-23;"]",-23];["",-25];["%prec",-27;"(",-27;"ID",-27;"LITERAL",-27;"REDUCER",-27;"[",-27];["",-28;"%%",-28;"%type",-28];["",-29;"%%",-29;"%type",-29];["",-30;"%%",-30;"%left",-30;"%nonassoc",-30;"%right",-30];["",-31;"%%",-31;"%left",-31;"%nonassoc",-31;"%right",-31];["ID",55];["REDUCER",-33];["",-34;"%%",-34;"%prec",-34;"(",-34;"ID",-34;"LITERAL",-34;"REDUCER",-34;"[",-34;"|",-34];["%prec",-38;"(",28;"ID",5;"LITERAL",6;"REDUCER",-38;"[",7;"atomic",44;"brackets",45;"parens",46;"repetition",47;"ruleBody",58;"symbol",35;"{symbol*}",38;"{symbol+}",61];["",-35;"%%",-35;"%prec",-35;"(",-35;"ID",-35;"LITERAL",-35;"REDUCER",-35;"[",-35;"|",-35];["",-36;"%%",-36;"%prec",-36;"(",-36;"ID",-36;"LITERAL",-36;"REDUCER",-36;"[",-36];["",-37;"%%",-37;"%prec",-37;"(",-37;"ID",-37;"LITERAL",-37;"REDUCER",-37;"[",-37];["%prec",-39;"(",28;"ID",5;"LITERAL",6;"REDUCER",-39;"[",7;"atomic",44;"brackets",45;"parens",46;"repetition",47;"symbol",36]]
open FslexFsyacc.Precedences
open FslexFsyacc.YACCs
open FslexFsyacc.TypeArguments
let rules : list<string list*(obj list->obj)> = [
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
        let s1 = unbox<list<RegularSymbol>> ss.[1]
        let result:list<RegularSymbol> =
            List.rev s1
        box result
    ["declaration";"%type";"<";"TYPE_ARGUMENT";">";"{symbol+}"], fun(ss:obj list)->
        let s2 = unbox<TypeArgument> ss.[2]
        let s4 = unbox<list<RegularSymbol>> ss.[4]
        let result:TypeArgument*list<string> =
            let symbols =
                s4
                |> List.map RegularSymbolUtils.innerSymbol
                |> List.rev
            s2,symbols
        box result
    ["file";"HEADER";"{ruleGroup+}";"%%";"{declaration+}";"{\"%%\"?}"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s1 = unbox<list<RuleGroup>> ss.[1]
        let s3 = unbox<list<TypeArgument*list<string>>> ss.[3]
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
        let s1 = unbox<list<RuleGroup>> ss.[1]
        let s3 = unbox<list<Associativity*list<string>>> ss.[3]
        let s5 = unbox<list<TypeArgument*list<string>>> ss.[5]
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
        let s1 = unbox<list<RuleGroup>> ss.[1]
        let s3 = unbox<list<Associativity*list<string>>> ss.[3]
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
        let s1 = unbox<list<RuleGroup>> ss.[1]
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
        let s1 = unbox<list<RegularSymbol>> ss.[1]
        let result:Associativity*list<string> =
            let s1 = s1 |> List.map RegularSymbolUtils.innerSymbol |> List.rev
            s0,s1
        box result
    ["parens";"(";"{symbol+}";")"], fun(ss:obj list)->
        let s1 = unbox<list<RegularSymbol>> ss.[1]
        let result:list<RegularSymbol> =
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
        let s0 = unbox<list<RegularSymbol>> ss.[0]
        let s1 = unbox<string> ss.[1]
        let s2 = unbox<string> ss.[2]
        let result:RuleBody =
            let s0 = s0 |> List.map RegularSymbolUtils.innerSymbol |> List.rev
            { rhs = s0; dummy = s1; reducer = s2 }
        box result
    ["ruleGroup";"symbol";":";"{\"|\"?}";"{ruleBody+}"], fun(ss:obj list)->
        let s0 = unbox<RegularSymbol> ss.[0]
        let s3 = unbox<list<RuleBody>> ss.[3]
        let result:RuleGroup =
            let lhs = RegularSymbolUtils.innerSymbol s0
            let bodies = List.rev s3
            { lhs = lhs; bodies = bodies }
        box result
    ["symbol";"atomic"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:RegularSymbol =
            Atomic s0
        box result
    ["symbol";"brackets"], fun(ss:obj list)->
        let s0 = unbox<list<RegularSymbol>> ss.[0]
        let result:RegularSymbol =
            Oneof s0
        box result
    ["symbol";"parens"], fun(ss:obj list)->
        let s0 = unbox<list<RegularSymbol>> ss.[0]
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
        let s0 = unbox<TypeArgument*list<string>> ss.[0]
        let result:list<TypeArgument*list<string>> =
            [s0]
        box result
    ["{declaration+}";"{declaration+}";"declaration"], fun(ss:obj list)->
        let s0 = unbox<list<TypeArgument*list<string>>> ss.[0]
        let s1 = unbox<TypeArgument*list<string>> ss.[1]
        let result:list<TypeArgument*list<string>> =
            s1::s0
        box result
    ["{operatorsLine+}";"operatorsLine"], fun(ss:obj list)->
        let s0 = unbox<Associativity*list<string>> ss.[0]
        let result:list<Associativity*list<string>> =
            [s0]
        box result
    ["{operatorsLine+}";"{operatorsLine+}";"operatorsLine"], fun(ss:obj list)->
        let s0 = unbox<list<Associativity*list<string>>> ss.[0]
        let s1 = unbox<Associativity*list<string>> ss.[1]
        let result:list<Associativity*list<string>> =
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
        let result:list<RuleBody> =
            [s0]
        box result
    ["{ruleBody+}";"{ruleBody+}";"|";"ruleBody"], fun(ss:obj list)->
        let s0 = unbox<list<RuleBody>> ss.[0]
        let s2 = unbox<RuleBody> ss.[2]
        let result:list<RuleBody> =
            s2::s0
        box result
    ["{ruleGroup+}";"ruleGroup"], fun(ss:obj list)->
        let s0 = unbox<RuleGroup> ss.[0]
        let result:list<RuleGroup> =
            [s0]
        box result
    ["{ruleGroup+}";"{ruleGroup+}";"ruleGroup"], fun(ss:obj list)->
        let s0 = unbox<list<RuleGroup>> ss.[0]
        let s1 = unbox<RuleGroup> ss.[1]
        let result:list<RuleGroup> =
            s1::s0
        box result
    ["{symbol*}"], fun(ss:obj list)->
        let result:list<RegularSymbol> =
            []
        box result
    ["{symbol*}";"{symbol+}"], fun(ss:obj list)->
        let s0 = unbox<list<RegularSymbol>> ss.[0]
        let result:list<RegularSymbol> =
            s0
        box result
    ["{symbol+}";"symbol"], fun(ss:obj list)->
        let s0 = unbox<RegularSymbol> ss.[0]
        let result:list<RegularSymbol> =
            [s0]
        box result
    ["{symbol+}";"{symbol+}";"symbol"], fun(ss:obj list)->
        let s0 = unbox<list<RegularSymbol>> ss.[0]
        let s1 = unbox<RegularSymbol> ss.[1]
        let result:list<RegularSymbol> =
            s1::s0
        box result
]
let unboxRoot =
    unbox<RawFsyaccFile>
let app: FslexFsyacc.ParseTableApp = {
    tokens        = tokens
    kernels       = kernels
    kernelSymbols = kernelSymbols
    actions       = actions
    rules         = rules
}