namespace FslexFsyacc.Runtime

type ParserTableAction =
    | Accept
    | Dead
    | Shift of states:int list*trees:obj list
    | Reduce of states:int list*trees:obj list

