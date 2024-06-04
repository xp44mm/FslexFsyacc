namespace FslexFsyacc.Runtime

type NextState =
    | NoSource of int
    | NoSymbol of string
    | Accepted
    | NoZero of string
    | Shifted of states: list<int*obj>
    | Reduced of states: list<int*obj>
