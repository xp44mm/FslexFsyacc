﻿namespace FslexFsyacc.Fslex

type FslexToken =
    | EOF
    | HEADER of string
    /// \w+
    | ID of string
    | CAP of string
    /// quote string literal(unquoted)
    | LITERAL of string

    /// rename to MAPPER
    | REDUCER of string

    | HOLE of string

    /// =
    | EQUALS
    /// (
    | LPAREN
    /// )
    | RPAREN
    /// [
    | LBRACK
    /// ]
    | RBRACK
    /// +
    | PLUS
    /// *
    | STAR
    /// /
    | SLASH
    /// |
    | BAR
    /// ?
    | QMARK
    /// &，concat运算符，连接两个正则表达式
    | AMP
    /// %%
    | PERCENT

