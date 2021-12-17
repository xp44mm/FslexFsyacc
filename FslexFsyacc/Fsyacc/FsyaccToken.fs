namespace FslexFsyacc.Fsyacc

type FsyaccToken =
    | HEADER of string
    | ID of string
    | QUOTE of string
    | SEMANTIC of string
    | COLON
    | SEMICOLON
    | BAR
    | PERCENT
    | LEFT
    | RIGHT
    | NONASSOC
    | PREC
    //| BOF
    //| EOF

