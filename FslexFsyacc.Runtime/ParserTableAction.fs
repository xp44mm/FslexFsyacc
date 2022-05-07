namespace FslexFsyacc.Runtime

type ParserTableAction =
    | Shift of states:(int*obj) list
    | Reduce of states:(int*obj) list
    | Dead of int * string
    | Accept

    