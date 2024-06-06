namespace FslexFsyacc.Fsyacc

type FsyaccToken2 =
    | HEADER of string
    | ID of string
    | LITERAL of string
    | REDUCER of string
    | TYPE_ARGUMENT of string
    | COLON
    | BAR
    | PERCENT
    | LEFTASSOC
    | RIGHTASSOC
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
    | LANGLE
    | RANGLE
