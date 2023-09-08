namespace FslexFsyacc.Fsyacc

type FsyaccToken =
    | HEADER of string
    | ID of string
    | LITERAL of string
    | SEMANTIC of string
    | TYPE_ARGUMENT of string
    | COLON
    | BAR
    | PERCENT
    | LEFT
    | RIGHT
    | NONASSOC
    | PREC
    | TYPE
    //qty
    | QMARK
    | PLUS
    | STAR

    | LBRACK 
    | RBRACK
    | LPAREN 
    | RPAREN

