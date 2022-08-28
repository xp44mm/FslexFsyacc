# FslexFsyacc & Runtime

Tools and Runtime for Fslex/Fsyacc analyzer/parser generation tools.

Fslex is a code generator that uses regular expression syntax as a rule to generate a function, which divides the input token sequence into groups at a higher level. The Fslex is often used to remove redundant delimiters, add omitted delimiters or other syntax components, and so on. The Fslex is also used to determine context somewhere in the stream.

Fsyacc is a code generator that use BNF productions and precedences as a rule to generate a function, which resolves the input token sequence to an abstract syntax tree.

## Fsyacc Example

Dragon book fig 4-59 example `expr.fsyacc`, fsyacc input file:

```fsyacc
%{
open Expr.ExprToken
%}
expr : expr "+" expr         { s0 + s2 }
     | expr "-" expr         { s0 - s2 }
     | expr "*" expr         { s0 * s2 }
     | expr "/" expr         { s0 / s2 }
     | "(" expr ")"          { s1 }
     | "-" expr %prec UMINUS { -s1 }
     | NUMBER                { s0 }
%%
%left "+" "-"
%left "*" "/"
%right UMINUS
%%
NUMBER : "float"
expr   : "float"
```

After `ParseTable` module is generated, you can use the parse function to work:

```F#
let inp = "2 + 3 * 5"
let y = 
    inp
    |> ExprToken.tokenize
    |> ExprParseTable.parse
Should.equal y 17.0
```

## Fslex Example

The ch8.6 Some Recursive Descent Parsing in Expert F# 4.0, the fslex input file:

```fslex
%{
open PolynomialExpressions.Tokenizer
type token = int*int*Token
%}
index = "**" INT
sign = [ "+" "-" ]
%%
<sign>? INT { // multiline test
              toConst lexbuf }
<sign>? INT? ID <index>? { toTerm lexbuf }
```

After `DFA` module is generated, you can use the split function to work:

```F#
let x = "2x**2+3x-5"
let y = 
    x 
    |> Tokenizer.tokenize
    |> TermDFA.analyze
    |> Seq.toList

Should.equal y [Term(2,"x",2);Term(3,"x",1);Const -5]
```

## Why use this package?

- You can use your existing handwriting tokenizer.

- This package uses standard lex/yacc syntax to minimize your learning costs.

- fslex/fsyacc generates respectively an independent, side-effect-free function that can be called flexibly.

- The method of generating code is simple, without command lines and without the need to configure projects.

- The result code is data-driven and highly readable.

- Flexiblely compose of tokenize, regular expressions, BNF technology.


