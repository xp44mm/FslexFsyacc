# 0. Token

## overview

A formal Lex/Yacc input file selects tokens only by their classifications: for example, if a rule mentions the terminal symbol ‘integer constant’, it means that any integer constant is grammatically valid in that position. The precise value of the constant is irrelevant to how to parse the input: if `x+4` is grammatical then `x+1` or `x+3989` is equally grammatical.

> Grammars are a more powerful notation than regular expressions. Every construct that can be described by a regular expression can be described by a grammar, but not vice-versa. Alternatively, every regular language is a context-free language, but not vice-versa.

But the precise value is very important for what the input means once it is recognized. A compiler is useless if it fails to distinguish between 4, 1 and 3989 as constants in the program! Therefore, each token has both a token type and a **semantic value**.

The token type is a terminal symbol defined in the grammar, such as `INTEGER`, `IDENTIFIER` or `","`. It tells everything you need to know to decide where the token may validly appear and how to group it with other tokens. The grammar rules know nothing about tokens except their types.

The semantic value has all the rest of the information about the meaning of the token, such as the value of an integer, or the name of an identifier. (A token such as `","` which is just punctuation doesn’t need to have any semantic value.)

For example, an input token might be classified as token type `INTEGER` and have the semantic value 4. Another input token might have the same token type `INTEGER` but value 3989. When a grammar rule says that `INTEGER` is allowed, either of these tokens is acceptable because each is an `INTEGER`. When the parser accepts the token, it keeps track of the token’s semantic value.

## 

A **terminal symbol** (also known as a **token type**) represents a class of syntactically equivalent tokens. You use the terminal symbol in grammar rules to mean that a token in that class is allowed. The terminal symbol is represented in the Lex analyzer/Yacc parser by a identifier string.

There are two ways of writing terminal symbols in the grammar:

- A **named token type** is written with an identifier. 

- A **literal token type** is written in the grammar using the same syntax used in JSON for string constants; for example, `"+"` is a literal token type. 

  By convention, a literal token type is used only to represent a token that consists of keywords or operators. Thus, the token type `"+"` is used to represent the operator `+` as a token. Nothing enforces this convention, but if you depart from it, your program will confuse other readers.

  All the usual escape sequences used in string literals in JSON can be used in Lex/Yacc as well, but you must not use the null string as a literal token because it is reserved for inner special tokens.

How you choose to write a terminal symbol has no effect on its grammatical meaning. That depends only on where it appears in rules and on when the parser function returns that symbol.

## Example

JSON yacc input token type definition:

```F#
type Token = 
| ADD | SUB
| MUL | DIV
| POWER
| NUMBER of float
```

the semantic value of tokens is tokens themselves. for example `NUMBER 100`, `POWER`.

The token type is a string variable. It is customary to write a token type.

```F#
    member this.tag = 
        match this with
        | COMMA       -> ","
        | COLON       -> ":"
        | LEFT_BRACK  -> "["
        | RIGHT_BRACK -> "]"
        | LEFT_BRACE  -> "{"
        | RIGHT_BRACE -> "}"
        | NULL        -> "NULL"
        | FALSE       -> "FALSE"
        | TRUE        -> "TRUE"
        | STRING    _ -> "STRING"
        | NUMBER    _ -> "NUMBER"
```

你可以将其提供给解析函数：

```F#
let parsingTree = parser.parse(tokens, fun(tok:JsonToken) -> tok.tag)
```

in Lex/Yacc input file, token is token type. token type is just a string.  token type can 表示为字符串字面量，例如lamda表达式在F#中，规则描述为：

```F#
lamda : "fun" pattern "->" body
```

其中，`"fun"`和`"->"`是token，在yacc中又称为terminal symbol. 在Lex/Yacc中，如果token符合word characters时，可以省略两侧的双引号。上面代码可以精简为：

```F#
lamda : fun pattern "->" body
```

这样每次解析器读到一个token，它可以根据你提供的函数求得token type. for example, 当输入流提供 `STRING "hello"`, `NUMBER 100`, `COMMA`时，解析器将会求得token type为字符串序列`STRING`, `NUMBER`, `","`。

提示，当token的语义值仅有一个时，尽量使用字符串字面量表示token type，这比使用名称表示更加直观。



