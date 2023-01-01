namespace FSharpAnatomy

type FSharpToken =
    | EOF

    | HASH
    | LPAREN | RPAREN
    | STAR
    | COMMA
    | RARROW
    | DOT
    | COLON
    | COLON_GREATER
    | SEMICOLON
    | LBRACK | RBRACK
    | ARRAY_TYPE_SUFFIX of rank:int
    | HTYPAR of string
    | IDENT of string
    | OPERATOR_NAME of string
    | QTYPAR of string
    | UNDERSCORE

    | LBRACE_BAR | BAR_RBRACE
    | LESS
    | GREATER

    | WHITESPACE of string
    | COMMENT of string

    | AND
    | COMPARISON
    | DELEGATE
    | ENUM
    | EQUALITY
    | MEMBER
    | NEW
    | NOT
    | NULL
    | OR
    | STATIC
    | STRUCT
    | UNMANAGED
    | WHEN

    | TYPE_ARGUMENT of TypeArgument


