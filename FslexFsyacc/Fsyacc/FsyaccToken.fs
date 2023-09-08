namespace FslexFsyacc.Fsyacc

[<System.Obsolete("FsyaccToken2")>]
type FsyaccToken =
    | HEADER of string
    | ID of string
    | LITERAL of string
    | SEMANTIC of string
    | COLON
    | BAR
    | PERCENT
    | LEFT
    | RIGHT
    | NONASSOC
    | PREC

    | QMARK
    | PLUS
    | STAR

    | LBRACK | RBRACK
    | LPAREN | RPAREN
