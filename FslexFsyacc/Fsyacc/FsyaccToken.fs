namespace FslexFsyacc.Fsyacc

type FsyaccToken =
    | HEADER of string
    | ID of string
    | LITERAL of string
    | SEMANTIC of string
    | COLON
    | SEMICOLON
    | BAR
    | PERCENT
    | LEFT
    | RIGHT
    | NONASSOC
    | PREC

    //| QMARK
    //| PLUS
    //| STAR

