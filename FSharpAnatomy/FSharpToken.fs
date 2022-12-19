namespace FSharpAnatomy

type FSharpToken =
    | HASH
    | LBRACK | RBRACK
    | LPAREN | RPAREN
    | LBRACE_BAR | BAR_RBRACE
    | STAR
    | COMMA
    | RARROW
    | DOT
    | COLON
    | COLON_GREATER
    | SEMICOLON
    | LESS
    | GREATER
    | UNDERSCORE
    | IDENT of string
    | TYPAR of string
    | INLINE_TYPAR of string
    | STRUCT
    | WHITESPACE of string
    | COMMENT of string
    | ARRAY_TYPE_SUFFIX of rank:int
