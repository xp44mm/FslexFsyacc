///表达式文法(4.1)
module FslexFsyacc.Grammars.Grammar4_1

let E = "E"
let T = "T"
let F = "F"
let id = "id"

let mainProductions = [
    [ E; E; "+"; T ]
    [ E; T ]
    [ T; T; "*"; F ]
    [ T; F ]
    [ F; "("; E; ")" ]
    [ F; id ]
    ]

let productions = set [["";"E"];["E";"E";"+";"T"];["E";"T"];["F";"(";"E";")"];["F";"id"];["T";"F"];["T";"T";"*";"F"]]
let startSymbol = "E"
let symbols = set ["";"(";")";"*";"+";"E";"F";"T";"id"]
let nonterminals = set ["E";"F";"T"]
let terminals = set ["(";")";"*";"+";"id"]
let nullables:Set<string> = set []
let firsts = Map ["E",set ["(";"id"];"F",set ["(";"id"];"T",set ["(";"id"]]
let lasts = Map ["E",set [")";"id"];"F",set [")";"id"];"T",set [")";"id"]]
let follows = Map ["(",set ["(";"id"];")",set ["";")";"*";"+"];"*",set ["(";"id"];"+",set ["(";"id"];"E",set ["";")";"+"];"F",set ["";")";"*";"+"];"T",set ["";")";"*";"+"];"id",set ["";")";"*";"+"]]
let precedes = Map ["(",set ["";"(";"*";"+"];")",set [")";"id"];"*",set [")";"id"];"+",set [")";"id"];"E",set ["";"("];"F",set ["";"(";"*";"+"];"T",set ["";"(";"+"];"id",set ["";"(";"*";"+"]]



