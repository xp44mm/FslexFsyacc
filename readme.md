# FslexFsyacc & Runtime

Tools and Runtime for Fslex/Fsyacc analyzer/parser generation tools. Fslex is a code generator that uses regular expression techniques to divide the token sequence into groups at a higher level. Fsyacc is a code generator that use BNF productions and precedences to resolve the token sequence to an abstract syntax tree.

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
%}
index = "**" INT
sign = [ "+" "-" ]
%%
<sign>? INT              { toConst lexbuf }
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

## Why use these packages

Decoupling with token. You can use modern language handwriting tokenize program.

Minimize syntax rules, minimal information input, and be more compatible with standard lex and yacc standards.

The method of generating code is simple, without command lines and without the need to configure projects.

The resulting code is highly readable.

Flexiblely compose of tokenize, regular expressions, BNF technology.
