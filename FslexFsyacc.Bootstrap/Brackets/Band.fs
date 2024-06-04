namespace FslexFsyacc.Brackets

type Band = 
    | Bounded of int * Band list * int
    | Tick of string

