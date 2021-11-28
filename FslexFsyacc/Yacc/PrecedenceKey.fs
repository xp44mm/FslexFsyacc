namespace FslexFsyacc.Yacc

type PrecedenceKey =
    | TerminalKey of string
    | ProductionKey of string list

