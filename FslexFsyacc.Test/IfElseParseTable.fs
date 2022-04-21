module FslexFsyacc.IfElseParseTable
let rules = [|["Statement";"if";"(";"Expression";")";"Statement";"else";"Statement"],"\"if else\"";["Statement";"if";"(";"Expression";")";"Statement"],"\"if\"";["Statement";"Another"],"\"another\""|]
let actions = [|[|"Another",2;"Statement",1;"if",3|];[|"",0|];[|"",-1;"else",-1|];[|"(",4|];[|"Expression",5|];[|")",6|];[|"Another",2;"Statement",7;"if",3|];[|"",-2;"else",8|];[|"Another",2;"Statement",9;"if",3|];[|"",-3;"else",-3|]|]
let closures = [|[|0,0,[||];-1,0,[||];-2,0,[||];-3,0,[||]|];[|0,1,[|""|]|];[|-1,1,[|"";"else"|]|];[|-2,1,[||];-3,1,[||]|];[|-2,2,[||];-3,2,[||]|];[|-2,3,[||];-3,3,[||]|];[|-1,0,[||];-2,0,[||];-2,4,[||];-3,0,[||];-3,4,[||]|];[|-2,5,[|""|];-3,5,[||]|];[|-1,0,[||];-2,0,[||];-3,0,[||];-3,6,[||]|];[|-3,7,[|"";"else"|]|]|]
let header = "// if else"
let declarations = [|"Statement","string";"Another","string";"Expression","string"|]
// if else
let fxRules:(string list*(obj[]->obj))[] = [|
    ["Statement";"if";"(";"Expression";")";"Statement";"else";"Statement"],fun (ss:obj[]) ->
            let s2 = unbox<string> ss.[2]
            let s4 = unbox<string> ss.[4]
            let s6 = unbox<string> ss.[6]
            let result:string =
                "if else"
            box result
    ["Statement";"if";"(";"Expression";")";"Statement"],fun (ss:obj[]) ->
            let s2 = unbox<string> ss.[2]
            let s4 = unbox<string> ss.[4]
            let result:string =
                "if"
            box result
    ["Statement";"Another"],fun (ss:obj[]) ->
            let s0 = unbox<string> ss.[0]
            let result:string =
                "another"
            box result
|]
open FslexFsyacc.Runtime
let parser = Parser(fxRules, actions, closures)
let parse (tokens:seq<_>) =
    parser.parse(tokens, getTag, getLexeme)
    |> unbox<string>