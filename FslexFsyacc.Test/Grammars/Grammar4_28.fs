///表达式文法(4.28)
module FslexFsyacc.Runtime.Grammars.Grammar4_28

let E = "E"
let E' = "E'"
let T = "T"
let T' = "T'"
let F = "F"
let id = "id"

///表达式语法(4.28)
let mainProductions = [
    [ E   ; T   ; E' ]
    [ E'  ; "+" ; T   ; E' ]
    [ E' ]
    [ T   ; F   ; T' ]
    [ T'  ; "*" ; F   ; T' ]
    [ T' ]
    [ F   ; "(" ; E   ; ")" ]
    [ F   ; id ]
    ]

let productions = set [["";"E"];["E";"T";"E'"];["E'"];["E'";"+";"T";"E'"];["F";"(";"E";")"];["F";"id"];["T";"F";"T'"];["T'"];["T'";"*";"F";"T'"]]
let startSymbol = "E"
let symbols = set ["(";")";"*";"+";"E";"E'";"F";"T";"T'";"id"]
let nonterminals = set ["E";"E'";"F";"T";"T'"]
let terminals = set ["(";")";"*";"+";"id"]
let nullables = set ["E'";"T'"]
let firsts = Map ["E",set ["(";"id"];"E'",set ["+"];"F",set ["(";"id"];"T",set ["(";"id"];"T'",set ["*"]]
let lasts = Map ["E",set [")";"id"];"E'",set [")";"id"];"F",set [")";"id"];"T",set [")";"id"];"T'",set [")";"id"]]
let follows = Map ["(",set ["(";"id"];")",set ["";")";"*";"+"];"*",set ["(";"id"];"+",set ["(";"id"];"E",set ["";")"];"E'",set ["";")"];"F",set ["";")";"*";"+"];"T",set ["";")";"+"];"T'",set ["";")";"+"];"id",set ["";")";"*";"+"]]
let precedes = Map ["(",set ["";"(";"*";"+"];")",set [")";"id"];"*",set [")";"id"];"+",set [")";"id"];"E",set ["";"("];"E'",set [")";"id"];"F",set ["";"(";"*";"+"];"T",set ["";"(";"+"];"T'",set [")";"id"];"id",set ["";"(";"*";"+"]]
