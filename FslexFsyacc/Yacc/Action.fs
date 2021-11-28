namespace FslexFsyacc.Yacc

/// SLR && LALR
type Action = 
    | Shift of kernel: Set<ItemCore>
    | Reduce of production: string list
    | DeadState