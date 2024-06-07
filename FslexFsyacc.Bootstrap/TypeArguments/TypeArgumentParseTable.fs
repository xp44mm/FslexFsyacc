module FslexFsyacc.TypeArguments.TypeArgumentParseTable
let tokens = set ["#";"(";")";"*";",";"->";".";":";":>";";";"<";">";"ARRAY_TYPE_SUFFIX";"HTYPAR";"IDENT";"QTYPAR";"_";"struct";"{|";"|}"]
let kernels = [[0,0];[0,1];[-1,1];[-1,2];[-1,3];[-2,1];[-2,2];[-3,1];[-3,2];[-3,3];[-4,1];[-4,1;-37,1];[-5,1];[-6,1];[-7,1;-8,1];[-7,2];[-7,3];[-7,4];[-8,2];[-9,1];[-9,1;-38,1];[-10,1];[-11,1];[-11,2];[-11,3];[-12,1;-13,1;-14,1];[-12,2];[-12,3];[-12,4];[-13,2];[-14,2];[-15,1;-16,1];[-16,2];[-16,3];[-17,1;-18,1];[-18,2];[-18,3];[-19,1;-20,1];[-20,2];[-20,3];[-20,4];[-21,1;-22,1];[-21,2;-40,1];[-21,3];[-22,2];[-23,1];[-23,2];[-23,3];[-24,1];[-25,1];[-27,1];[-27,2];[-28,1;-29,1];[-29,2];[-29,3];[-30,1];[-31,1];[-32,1];[-33,1];[-34,1];[-35,1;-36,1];[-36,2];[-36,3]]
let kernelSymbols = ["";"typeArgument";"{|";"recdFieldDeclList";"|}";"atomtype";"suffixTypes";"(";"typeArgument";")";"_";"_";"anonRecordType";"namedtype";"struct";"(";"tupletype";")";"anonRecordType";"typar";"typar";"longIdent";"IDENT";":";"typeArgument";"#";"(";"apptype";")";"_";"namedtype";"tupletype";"->";"funtype";"IDENT";".";"longIdent";"ctortype";"<";"typeArguments";">";"fieldDecl";";";"recdFieldDeclList";"{\";\"?}";"variabletype";":>";"apptype";"ARRAY_TYPE_SUFFIX";"ctortype";"suffixType";"suffixTypes";"apptype";"*";"tupletype";"HTYPAR";"QTYPAR";"flexibletype";"funtype";"subtype";"typeArgument";",";"typeArguments"]
let actions = [["#",25;"(",7;"HTYPAR",55;"IDENT",34;"QTYPAR",56;"_",11;"anonRecordType",12;"apptype",52;"atomtype",5;"ctortype",37;"flexibletype",57;"funtype",58;"longIdent",21;"namedtype",13;"struct",14;"subtype",59;"tupletype",31;"typar",20;"typeArgument",1;"variabletype",45;"{|",2];["",0];["IDENT",22;"fieldDecl",41;"recdFieldDeclList",3];["|}",4];["",-1;")",-1;"*",-1;",",-1;"->",-1;";",-1;">",-1;"ARRAY_TYPE_SUFFIX",-1;"IDENT",-1;"|}",-1];["",-26;")",-26;"*",-26;",",-26;"->",-26;";",-26;">",-26;"ARRAY_TYPE_SUFFIX",48;"IDENT",34;"ctortype",49;"longIdent",21;"suffixType",50;"suffixTypes",6;"|}",-26];["",-2;")",-2;"*",-2;",",-2;"->",-2;";",-2;">",-2;"|}",-2];["#",25;"(",7;"HTYPAR",55;"IDENT",34;"QTYPAR",56;"_",11;"anonRecordType",12;"apptype",52;"atomtype",5;"ctortype",37;"flexibletype",57;"funtype",58;"longIdent",21;"namedtype",13;"struct",14;"subtype",59;"tupletype",31;"typar",20;"typeArgument",8;"variabletype",45;"{|",2];[")",9];["",-3;")",-3;"*",-3;",",-3;"->",-3;";",-3;">",-3;"ARRAY_TYPE_SUFFIX",-3;"IDENT",-3;"|}",-3];["",-4;")",-4;"*",-4;",",-4;"->",-4;";",-4;">",-4;"ARRAY_TYPE_SUFFIX",-4;"IDENT",-4;"|}",-4];["",-4;")",-4;"*",-4;",",-4;"->",-4;":>",-37;";",-4;">",-4;"ARRAY_TYPE_SUFFIX",-4;"IDENT",-4;"|}",-4];["",-5;")",-5;"*",-5;",",-5;"->",-5;";",-5;">",-5;"ARRAY_TYPE_SUFFIX",-5;"IDENT",-5;"|}",-5];["",-6;")",-6;"*",-6;",",-6;"->",-6;";",-6;">",-6;"ARRAY_TYPE_SUFFIX",-6;"IDENT",-6;"|}",-6];["(",15;"anonRecordType",18;"{|",2];["(",7;"HTYPAR",55;"IDENT",34;"QTYPAR",56;"_",10;"anonRecordType",12;"apptype",52;"atomtype",5;"ctortype",37;"longIdent",21;"namedtype",13;"struct",14;"tupletype",16;"typar",19;"{|",2];[")",17];["",-7;")",-7;"*",-7;",",-7;"->",-7;";",-7;">",-7;"ARRAY_TYPE_SUFFIX",-7;"IDENT",-7;"|}",-7];["",-8;")",-8;"*",-8;",",-8;"->",-8;";",-8;">",-8;"ARRAY_TYPE_SUFFIX",-8;"IDENT",-8;"|}",-8];["",-9;")",-9;"*",-9;",",-9;"->",-9;";",-9;">",-9;"ARRAY_TYPE_SUFFIX",-9;"IDENT",-9;"|}",-9];["",-9;")",-9;"*",-9;",",-9;"->",-9;":>",-38;";",-9;">",-9;"ARRAY_TYPE_SUFFIX",-9;"IDENT",-9;"|}",-9];["",-10;")",-10;"*",-10;",",-10;"->",-10;";",-10;"<",-10;">",-10;"ARRAY_TYPE_SUFFIX",-10;"IDENT",-10;"|}",-10];[":",23];["#",25;"(",7;"HTYPAR",55;"IDENT",34;"QTYPAR",56;"_",11;"anonRecordType",12;"apptype",52;"atomtype",5;"ctortype",37;"flexibletype",57;"funtype",58;"longIdent",21;"namedtype",13;"struct",14;"subtype",59;"tupletype",31;"typar",20;"typeArgument",24;"variabletype",45;"{|",2];[";",-11;"|}",-11];["(",26;"IDENT",34;"_",29;"ctortype",37;"longIdent",21;"namedtype",30];["(",7;"HTYPAR",55;"IDENT",34;"QTYPAR",56;"_",10;"anonRecordType",12;"apptype",27;"atomtype",5;"ctortype",37;"longIdent",21;"namedtype",13;"struct",14;"typar",19;"{|",2];[")",28];["",-12;")",-12;",",-12;";",-12;">",-12;"|}",-12];["",-13;")",-13;",",-13;";",-13;">",-13;"|}",-13];["",-14;")",-14;",",-14;";",-14;">",-14;"|}",-14];["",-15;")",-15;",",-15;"->",32;";",-15;">",-15;"|}",-15];["(",7;"HTYPAR",55;"IDENT",34;"QTYPAR",56;"_",10;"anonRecordType",12;"apptype",52;"atomtype",5;"ctortype",37;"funtype",33;"longIdent",21;"namedtype",13;"struct",14;"tupletype",31;"typar",19;"{|",2];["",-16;")",-16;",",-16;";",-16;">",-16;"|}",-16];["",-17;")",-17;"*",-17;",",-17;"->",-17;".",35;";",-17;"<",-17;">",-17;"ARRAY_TYPE_SUFFIX",-17;"IDENT",-17;"|}",-17];["IDENT",34;"longIdent",36];["",-18;")",-18;"*",-18;",",-18;"->",-18;";",-18;"<",-18;">",-18;"ARRAY_TYPE_SUFFIX",-18;"IDENT",-18;"|}",-18];["",-19;")",-19;"*",-19;",",-19;"->",-19;";",-19;"<",38;">",-19;"ARRAY_TYPE_SUFFIX",-19;"IDENT",-19;"|}",-19];["#",25;"(",7;"HTYPAR",55;"IDENT",34;"QTYPAR",56;"_",11;"anonRecordType",12;"apptype",52;"atomtype",5;"ctortype",37;"flexibletype",57;"funtype",58;"longIdent",21;"namedtype",13;"struct",14;"subtype",59;"tupletype",31;"typar",20;"typeArgument",60;"typeArguments",39;"variabletype",45;"{|",2];[">",40];["",-20;")",-20;"*",-20;",",-20;"->",-20;";",-20;">",-20;"ARRAY_TYPE_SUFFIX",-20;"IDENT",-20;"|}",-20];[";",42;"{\";\"?}",44;"|}",-39];["IDENT",22;"fieldDecl",41;"recdFieldDeclList",43;"|}",-40];["|}",-21];["|}",-22];[":>",46];["(",7;"HTYPAR",55;"IDENT",34;"QTYPAR",56;"_",10;"anonRecordType",12;"apptype",47;"atomtype",5;"ctortype",37;"longIdent",21;"namedtype",13;"struct",14;"typar",19;"{|",2];["",-23;")",-23;",",-23;";",-23;">",-23;"|}",-23];["",-24;")",-24;"*",-24;",",-24;"->",-24;";",-24;">",-24;"ARRAY_TYPE_SUFFIX",-24;"IDENT",-24;"|}",-24];["",-25;")",-25;"*",-25;",",-25;"->",-25;";",-25;">",-25;"ARRAY_TYPE_SUFFIX",-25;"IDENT",-25;"|}",-25];["",-26;")",-26;"*",-26;",",-26;"->",-26;";",-26;">",-26;"ARRAY_TYPE_SUFFIX",48;"IDENT",34;"ctortype",49;"longIdent",21;"suffixType",50;"suffixTypes",51;"|}",-26];["",-27;")",-27;"*",-27;",",-27;"->",-27;";",-27;">",-27;"|}",-27];["",-28;")",-28;"*",53;",",-28;"->",-28;";",-28;">",-28;"|}",-28];["(",7;"HTYPAR",55;"IDENT",34;"QTYPAR",56;"_",10;"anonRecordType",12;"apptype",52;"atomtype",5;"ctortype",37;"longIdent",21;"namedtype",13;"struct",14;"tupletype",54;"typar",19;"{|",2];["",-29;")",-29;",",-29;"->",-29;";",-29;">",-29;"|}",-29];["",-30;")",-30;"*",-30;",",-30;"->",-30;":>",-30;";",-30;">",-30;"ARRAY_TYPE_SUFFIX",-30;"IDENT",-30;"|}",-30];["",-31;")",-31;"*",-31;",",-31;"->",-31;":>",-31;";",-31;">",-31;"ARRAY_TYPE_SUFFIX",-31;"IDENT",-31;"|}",-31];["",-32;")",-32;",",-32;";",-32;">",-32;"|}",-32];["",-33;")",-33;",",-33;";",-33;">",-33;"|}",-33];["",-34;")",-34;",",-34;";",-34;">",-34;"|}",-34];[",",61;">",-35];["#",25;"(",7;"HTYPAR",55;"IDENT",34;"QTYPAR",56;"_",11;"anonRecordType",12;"apptype",52;"atomtype",5;"ctortype",37;"flexibletype",57;"funtype",58;"longIdent",21;"namedtype",13;"struct",14;"subtype",59;"tupletype",31;"typar",20;"typeArgument",60;"typeArguments",62;"variabletype",45;"{|",2];[">",-36]]

let rules : list<string list*(obj list->obj)> = [
    ["";"typeArgument"], fun(ss:obj list)-> ss.[0]
    ["anonRecordType";"{|";"recdFieldDeclList";"|}"], fun(ss:obj list)->
        let s1 = unbox<(string*TypeArgument)list> ss.[1]
        let result:TypeArgument =
            AnonRecd(false,List.rev s1)
        box result
    ["apptype";"atomtype";"suffixTypes"], fun(ss:obj list)->
        let s0 = unbox<TypeArgument> ss.[0]
        let s1 = unbox<SuffixType list> ss.[1]
        let result:TypeArgument =
            TypeArgumentUtils.ofApp(s0,s1)
        box result
    ["atomtype";"(";"typeArgument";")"], fun(ss:obj list)->
        let s1 = unbox<TypeArgument> ss.[1]
        let result:TypeArgument =
            s1
        box result
    ["atomtype";"_"], fun(ss:obj list)->
        let result:TypeArgument =
            Anon
        box result
    ["atomtype";"anonRecordType"], fun(ss:obj list)->
        let s0 = unbox<TypeArgument> ss.[0]
        let result:TypeArgument =
            s0
        box result
    ["atomtype";"namedtype"], fun(ss:obj list)->
        let s0 = unbox<string list * TypeArgument list> ss.[0]
        let result:TypeArgument =
            match s0 with a,b -> Ctor(a,b)
        box result
    ["atomtype";"struct";"(";"tupletype";")"], fun(ss:obj list)->
        let s2 = unbox<TypeArgument list> ss.[2]
        let result:TypeArgument =
            match TypeArgumentUtils.ofTuple s2 with Tuple(_,ls) -> Tuple(true,ls) |_->failwith""
        box result
    ["atomtype";"struct";"anonRecordType"], fun(ss:obj list)->
        let s1 = unbox<TypeArgument> ss.[1]
        let result:TypeArgument =
            match s1 with AnonRecd(_,ls) -> AnonRecd(true,ls) |_->failwith""
        box result
    ["atomtype";"typar"], fun(ss:obj list)->
        let s0 = unbox<Typar> ss.[0]
        let result:TypeArgument =
            TypeArgumentUtils.ofTypar s0
        box result
    ["ctortype";"longIdent"], fun(ss:obj list)->
        let s0 = unbox<string list> ss.[0]
        let result:string list =
            List.rev s0
        box result
    ["fieldDecl";"IDENT";":";"typeArgument"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s2 = unbox<TypeArgument> ss.[2]
        let result:string*TypeArgument =
            (s0,s2)
        box result
    ["flexibletype";"#";"(";"apptype";")"], fun(ss:obj list)->
        let s2 = unbox<TypeArgument> ss.[2]
        let result:BaseOrInterfaceType =
            TypeArgumentUtils.toBaseOrInterfaceType s2
        box result
    ["flexibletype";"#";"_"], fun(ss:obj list)->
        let result:BaseOrInterfaceType =
            FlexibleAnon
        box result
    ["flexibletype";"#";"namedtype"], fun(ss:obj list)->
        let s1 = unbox<string list * TypeArgument list> ss.[1]
        let result:BaseOrInterfaceType =
            match s1 with ctor,targs -> FlexibleCtor(ctor,targs)
        box result
    ["funtype";"tupletype"], fun(ss:obj list)->
        let s0 = unbox<TypeArgument list> ss.[0]
        let result:TypeArgument list list =
            [s0]
        box result
    ["funtype";"tupletype";"->";"funtype"], fun(ss:obj list)->
        let s0 = unbox<TypeArgument list> ss.[0]
        let s2 = unbox<TypeArgument list list> ss.[2]
        let result:TypeArgument list list =
            s0::s2
        box result
    ["longIdent";"IDENT"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:string list =
            [s0]
        box result
    ["longIdent";"IDENT";".";"longIdent"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let s2 = unbox<string list> ss.[2]
        let result:string list =
            s0::s2
        box result
    ["namedtype";"ctortype"], fun(ss:obj list)->
        let s0 = unbox<string list> ss.[0]
        let result:string list * TypeArgument list =
            s0,[]
        box result
    ["namedtype";"ctortype";"<";"typeArguments";">"], fun(ss:obj list)->
        let s0 = unbox<string list> ss.[0]
        let s2 = unbox<TypeArgument list> ss.[2]
        let result:string list * TypeArgument list =
            s0,List.rev s2
        box result
    ["recdFieldDeclList";"fieldDecl";";";"recdFieldDeclList"], fun(ss:obj list)->
        let s0 = unbox<string*TypeArgument> ss.[0]
        let s2 = unbox<(string*TypeArgument)list> ss.[2]
        let result:(string*TypeArgument)list =
            s0::s2
        box result
    ["recdFieldDeclList";"fieldDecl";"{\";\"?}"], fun(ss:obj list)->
        let s0 = unbox<string*TypeArgument> ss.[0]
        let result:(string*TypeArgument)list =
            [s0]
        box result
    ["subtype";"variabletype";":>";"apptype"], fun(ss:obj list)->
        let s0 = unbox<Typar> ss.[0]
        let s2 = unbox<TypeArgument> ss.[2]
        let result:TypeArgument =
            Subtype(s0,TypeArgumentUtils.toBaseOrInterfaceType s2)
        box result
    ["suffixType";"ARRAY_TYPE_SUFFIX"], fun(ss:obj list)->
        let s0 = unbox<int> ss.[0]
        let result:SuffixType =
            ArrayTypeSuffix s0
        box result
    ["suffixType";"ctortype"], fun(ss:obj list)->
        let s0 = unbox<string list> ss.[0]
        let result:SuffixType =
            LongIdent s0
        box result
    ["suffixTypes"], fun(ss:obj list)->
        let result:SuffixType list =
            []
        box result
    ["suffixTypes";"suffixType";"suffixTypes"], fun(ss:obj list)->
        let s0 = unbox<SuffixType> ss.[0]
        let s1 = unbox<SuffixType list> ss.[1]
        let result:SuffixType list =
            s0::s1
        box result
    ["tupletype";"apptype"], fun(ss:obj list)->
        let s0 = unbox<TypeArgument> ss.[0]
        let result:TypeArgument list =
            [s0]
        box result
    ["tupletype";"apptype";"*";"tupletype"], fun(ss:obj list)->
        let s0 = unbox<TypeArgument> ss.[0]
        let s2 = unbox<TypeArgument list> ss.[2]
        let result:TypeArgument list =
            s0::s2
        box result
    ["typar";"HTYPAR"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:Typar =
            NamedTypar(true,s0)
        box result
    ["typar";"QTYPAR"], fun(ss:obj list)->
        let s0 = unbox<string> ss.[0]
        let result:Typar =
            NamedTypar(false,s0)
        box result
    ["typeArgument";"flexibletype"], fun(ss:obj list)->
        let s0 = unbox<BaseOrInterfaceType> ss.[0]
        let result:TypeArgument =
            Flexible s0
        box result
    ["typeArgument";"funtype"], fun(ss:obj list)->
        let s0 = unbox<TypeArgument list list> ss.[0]
        let result:TypeArgument =
            TypeArgumentUtils.ofFun s0
        box result
    ["typeArgument";"subtype"], fun(ss:obj list)->
        let s0 = unbox<TypeArgument> ss.[0]
        let result:TypeArgument =
            s0
        box result
    ["typeArguments";"typeArgument"], fun(ss:obj list)->
        let s0 = unbox<TypeArgument> ss.[0]
        let result:TypeArgument list =
            [s0]
        box result
    ["typeArguments";"typeArgument";",";"typeArguments"], fun(ss:obj list)->
        let s0 = unbox<TypeArgument> ss.[0]
        let s2 = unbox<TypeArgument list> ss.[2]
        let result:TypeArgument list =
            s0::s2
        box result
    ["variabletype";"_"], fun(ss:obj list)->
        let result:Typar =
            AnonTypar
        box result
    ["variabletype";"typar"], fun(ss:obj list)->
        let s0 = unbox<Typar> ss.[0]
        let result:Typar =
            s0
        box result
    ["{\";\"?}"], fun(ss:obj list)->
        null
    ["{\";\"?}";";"], fun(ss:obj list)->
        null
]
let unboxRoot =
    unbox<TypeArgument>
let app: FslexFsyacc.ParseTableApp = {
    tokens        = tokens
    kernels       = kernels
    kernelSymbols = kernelSymbols
    actions       = actions
    rules         = rules
}
