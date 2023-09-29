namespace FslexFsyacc.Runtime

type Production = string list

// Simple LR 的
type ItemCore =
    {
        production: string list
        dot: int
    }

