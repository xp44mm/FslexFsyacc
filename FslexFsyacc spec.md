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

rules

%%

prec

%%

type declarations

```

## Fslex specification

```fslex
%{
F# nested code
%}

definitions

%%

rules

```
