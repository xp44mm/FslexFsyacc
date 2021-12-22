# FslexFsyacc Specification

## common syntax

The first section of fslex/fsyacc file is a chunk of F# code that is bounded by `%{` and `%}` limiters. This code is there to define utility functions used by later snippets of F# code and to set up the environment by opening useful modules.

The `%%` are punctuation that appears in every fslex/fsyacc grammar file to separate the sections. It is recommended that the  `%%`, `%{` and `%}` limiters be on a separate line.

Whitespace is significant only to separate symbols. You can add extra whitespace as you wish.

The Comments are either lines starting with `//` or blocks enclosed by `(*` and `*)` that is consistent with the F# language.

The symbol of the fslex/fsyacc grammar input file is a string literal. A symbol be matched by `/\w+/` can omit limitered quotation marks. for example, These are the proper symbols:

```F#
expr "+" number
```

### Token

The token type is a terminal symbol defined in the grammar input, such as `INTEGER`, `IDENTIFIER` or `","`. It tells everything you need to know to decide where the token may validly appear and how to group it with other tokens. The grammar rules know nothing about tokens except their types. Get The type of Token is obtained using `getTag`.

```F#
getTag: 'tok -> string
```

The semantic value has all the rest of the information about the meaning of the token, such as the value of an integer, or the name of an identifier. (A token such as `','` which is just punctuation doesn’t need to have any semantic value.)

Get The type of Token is obtained using `getLexeme`. Different Token semantic value types can be different, so we box the values. The `getLexeme` function returns the obj type value.

```F#
getLexeme: 'tok -> obj
```

For example, an input token might be classified as token type `INTEGER` and have the semantic value 4. Another input token might have the same token type `INTEGER` but value 3989. When a grammar rule says that `INTEGER` is allowed, either of these tokens is acceptable because each is an `INTEGER`. When the parser accepts the token, it keeps track of the token’s semantic value.

## Fsyacc specification

```fsyacc
%{
F# nested code
%}

translation rules

%%

precedence declarations

%%

type declarations

```

Each rule consists of a grammar production and the associated semantic action. A set of productions would be written in fsacc as

```fs
<head> : <body>_1 {<semantic action>_1}
       | <body>_2 {<semantic action>_2}
         ...
       | <body>_n {<semantic action>_n}
```

A fsyacc semantic action is a sequence of F# statements. In a semantic action, the symbol `s\d` refers to the value associated with the ith grammar symbol (terminal or nonterminal) of the body. The semantic action is performed whenever we reduce by the associated production, so normally the semantic action computes a value for `$$` in terms of the `s\d`'s. In the Yacc specification, we have written the two `E`-productions and their associated semantic actions as:

```fs
expr : expr '+' term { s0 + s2 }
     | term { s0 }
```

Note that the nonterminal `term` in the first production is the third grammar symbol of the body, while `'+'` is the second. The semantic action associated with the first production adds the value of the `expr` and the `term` of the body and assigns the result as the value for the nonterminal `expr` of the head.

Yacc provides a general mechanism for resolving shift/reduce conflicts. In the precedence declarations portion, we can assign precedences and associativities to terminals. The declaration

```js
%left '+' '-'
```

makes `'+'` and `'-'` be of the same precedence and be left associative. We can declare an operator to be right associative by writing

```js
%right '^'
```

and we can force an operator to be a nonassociative binary operator(i.e., two occurrences of the operator cannot be combined at all) by writing

```js
%nonassoc '<' '<=' '>=' '>'
```

The tokens are given precedences in the order in which they appear in the prec declarations portion, lowest first. Tokens in the same declaration have the same precedence. Thus, the declaration

```fs
%right UMINUS
```

gives the token UMINUS a precedence level higher than that of the five preceding terminals.

In some situations where the rightmost terminal does not supply the proper precedence to a production, we can force a precedence by appending to a production the tag

```fs
%prec <terminal>
```

The precedence and associativity of the production will then be the same as that of the terminal. Yacc does not report shift/reduce conflicts that are resolved using this precedence and associativity mechanism.

This "terminal" can be a placeholder, like `UMINUS`; this terminal is not returned by the lexical analyzer, but is declared solely to define a precedence for a production. The declaration

```fs
%right UMINUS
```

assigns to the token `UMINUS` a precedence that is higher than that of `*` and `/`. In the translation rules part, the tag:

```fs
%prec UMINUS
```

at the end of the production

```fs
expr : '-' expr
```

makes the unary-minus operator in this production have a higher precedence than any other operator.

The type declarations each have the form

```fs
symbol : "Type"
```

## Fslex specification

```fslex
%{
F# nested code
%}

definition declarations

%%

translation rules

```

In the declarations section is a sequence of regular definitions. These use the extended notation for regular expressions. Regular definitions that are used in later definitions or in the patterns of the translation rules are surrounded by angle braces.

Regular definitions have the form:

```fs
name = definition
```

The “`name`” is a word that is matched with `\w+`. The definition is following the equals(=) and continuing to the next name or `%%`. The definition can subsequently be referred to using `<name>`, which will expand to “`(definition)`”.

The translation rules each have the form

```fs
Pattern { Action }
```

Each pattern is a regular expression, which may use the regular definitions of the declaration section. The actions are fragments of F# code.

Sometimes, we want a certain pattern to be matched to the input only when it is followed by a certain other characters. If so, we may use the slash in a pattern to indicate the end of the part of the pattern that matches the lexeme. What follows `/` is additional pattern that must be matched before we can decide that the token in question was seen, but what matches this second pattern is not part of the lexeme. For example

```fs
<ID> / "=" { (* this ID is a definition name. *) }
```
