In traditional JavaScript compilers, the parser and lexer are intertwined. Rather than run the entire program through the lexer once to get a sequence of tokens, the parser calls out to the lexer from a given grammatical context with a flag to indicate if the lexer should accept a regular expression or a divide operator, and the input character stream is tokenized accordingly. So if the parser is in a context that accepts a regular expression, the characters “/x/” will be lexed into the single token /x/ otherwise it will lex into the individual tokens /, x, and /.

```
        feedback
lexer<->parser->AST
        Token*

```

Our JavaScript macro system, sweet.js, includes a separate reader that converts a sequence of tokens into a sequence of token trees (a little analogous to Symbolic-expressions) without feedback from the parser.

```
lexer → reader→ expander/parser→
```

This enables us to finally separate the JavaScript lexer and parser and build a fully hygienic macro system for JavaScript. The reader records sufficient history information in the form of token trees in order to correctly decide whether to parse a sequence of tokens `/x/g` as a regular expression or as division operators (as in `4.0/x/g`). Surprisingly, this history information needs to be remembered from arbitrarily far back in the token stream.

While the algorithm for resolving ambiguities we present in this paper is specific to JavaScript, the technique of recording history in the reader with token trees can be applied to other languages with ambiguous grammars.

Once JavaScript source has been correctly read, there are still a number of challenges to building an expressive macro system. The lack of parentheses in particular make writing declarative macro definitions difficult. For example, the if statement in JavaScript allows undelimited then and else branches:

```js
if ( denom > 0)
x / denom ;
else
throw " divide by zero";
```

It is necessary to know where the then and else branches end to correctly implement an if macro but this is complicated by the lack of delimiters.

The solution to this problem that we take is by progressively building a partial AST during macro expansion. Macros can then match against and manipulate this partial AST. For example, an `if` macro could indicate that the `then` and `else` branches must be single statements and then manipulate them appropriately.

This approach, called *enforestation*, was pioneered by Honu [29, 30], which we adapt here for JavaScript. 

enforestation parsing step, which converts a flat stream of tokens into an S-expression-like tree, in addition to the initial "read" phase of parsing and interleaved with the "macro-expand" phase.


# 2. Reading JavaScript

Parsers give structure to unstructured source code. In parsers without macro systems this is usually accomplished by a lexer (which converts a character stream to a token stream) and a parser (which converts the token stream into an AST according to a context-free grammar). A system with macros must implement a macro expander that sits between the lexer and parser. Some macros systems, such as the C preprocessor, work over just the token stream. However, to implement truly expressive Scheme-like macros that can manipulate groups of unparsed tokens, it is necessary to structure the token stream via a *reader*, which performs delimiter matching and enables macros to manipulate delimiter-grouped tokens.

As mentioned in the introduction, the design of a correct reader for JavaScript is surprisingly subtle due to ambiguities between lexing regular expression literals and the divide operator. This disambiguation is critical to the correct implementation of read because delimiters can appear inside of a regular expression literal. If the reader failed to distinguish between a regular expression/divide operator, it could result in incorrectly matched delimiters.

```js
function makeRegex () {
// results in a parse error if the
// first / is incorrectly read as divide
return /}/;
}
```

A key novelty in sweet.js is the design and implementation of a reader that correctly distinguishes between regular expression literals and the divide operator for full ES5 JavaScript. For clarity of presentation, this paper describes the implementation of read for the subset of JavaScript shown in Figure 4, which retains the essential complications of the full version of read.

Figure 4: Simplified ES5 Grammar

```
PrimaryExpr x ::= x
PrimaryExpr /r/ ::= /·x·/
PrimaryExpr {x:e} ::= {·x·:·AssignExpr e·}
PrimaryExpr (e) ::= (·AssignExpr e· )

MemberExpr e ::= PrimaryExpr e
MemberExpr e ::= Function e
MemberExpr e.x ::= MemberExpr e·.·x

CallExpr e (e') ::= MemberExpr e· (·AssignExpr e'·)
CallExpr e (e') ::= CallExpr e· (·AssignExpr e'·)
CallExpr e.x ::= CallExpr e.x

BinaryExpr e::= CallExpr e
BinaryExpr e / e' ::= BinaryExpr e· /·BinaryExpr e'
BinaryExpr e + e' ::= BinaryExpr e· +·BinaryExpr e'

AssignExpr e::= BinaryExpr e
AssignExpr e = e' ::= CallExpr e· =·AssignExpr e'

StmtList e ::= Stmt e
StmtList e e' ::= StmtList e·Stmt e'

Stmt {e} ::= {·StmtList e·}
Stmt x: e ::= x·:·Stmt e
Stmt e ::= AssignExpr e· ; where lookahead != { or function
Stmt if (e) e' ::= if·(·AssignExpr e· )·Stmt e'
Stmt return ::= return
Stmt return e ::= return·[no line terminator here] AssignExpr e· ;

Function function x (x') {e} ::= function·x·(·x'·)·{·SourceElements e·}

SourceElement e ::= Stmt e
SourceElement e ::= Function e

SourceElements e ::= SourceElement e
SourceElements e e' ::= SourceElements e·SourceElement e'

Program e::= SourceElements e
Program ::= empty
```

## 2.1 Formalism

In our formalism in Figure 3, `read` takes a *Token* sequence. Tokens are the output of a very simple lexer, which we do not define here. This lexer does not receive feedback from the parser like the ES5 lexer does, and so does not distinguish between regular expressions and the divide operator. Rather it simply lexes slashes into the ambiguous `/` token. Tokens also include keywords, punctuators, the (unmatched) delimiters, and variable identifiers.

Figure 3: Simplified Read Algorithm

```
Punctuator ::= / | + | : | ; | = | .
Keyword ::= return | function | if
Token ::= x | Punctuator | Keyword | { | } | ( | )
k ∈ TokenTree ::= x | Punctuator | Keyword | /r/ | (t) | {t}
r ∈ RegexPat ::= x | { | } | ( | )
x ∈ Variable
s ∈ Token∗
t, p ∈ TokenTree∗

isExprPrefix: TokenTree∗→ Bool → Int → Bool
isExprPrefix(empty, true, l) = true
isExprPrefix(p·/, b, l) = true
isExprPrefix(p·+, b, l) = true
isExprPrefix(p·=, b, l) = true
isExprPrefix(p·:, b, l) = b
isExprPrefix(p·return^l, b, l') = false if l != l'
isExprPrefix(p·return^l, b, l') = true if l = l'
isExprPrefix(p, b, l) = false

read: Token∗ → TokenTree∗ → Bool → TokenTree∗
read(/·r·/·s, empty, b) 
= /r/·read(s, /r/, b)

read(/·r·/·s, p·k, b) if k ∈ Punctuator∪Keyword 
= /r/·read(s, p·k·/r/, b)

read(/·r·/·s, p·if·(t), b) 
= /r/·read(s, p·if·(t)·/r/, b)

read(/·r·/·s, p·function^l·x·(t)·{t'}, b) if isExprPrefix(p, b, l) = false
= /r/·read(s, p·function^l·x·(t)·{t'}·/r/, b)
   
read(/·r·/·s, p·{t}l, b) if isExprPrefix(p, b, l) = false
= /r/·read(s, p·{t}l·/r/, b)
   
read(/·s, p·x, b) 
= /·read(s, p·x·/, b)
read(/·s, p·/x/, b) 
= /·read(s, p·/x/·/, b)
read(/·s, p·(t), b) 
= /·read(s, p·(t)·/, b)

read(/·s, p·function^l·x·(t)·{t'}, b) if isExprPrefix(p, b, l) = true 
= /·read(s, p·function^l·x·(t)·{t'}·/, b)
   
read(/·s, p·{t}l, b) if isExprPrefix(p, b, l) = true
= /·read(s, p·{t}l·/, b)
    
read((·s·)·s', p, b) where s contains no unmatched ) 
= (t)·read(s', p·(t), b) where t = read(s, empty, false)

read({^l·s·}·s', p, b) where s contains no unmatched }
= {t}^l·read(s', p·{t}^l, b) where t = read(s, empty, isExprPrefix(p, b, l))

read(x·s, p, b) = x·read(s, p·x, b)
read(empty, p, b) = empty
```

The job of `read` is then to produce a correct `TokenTree` sequence. Token trees include regular expression literals `/r/`, where `r` is the regular expression body. We simplify regular expression bodies to just a variable and the individual delimiters, which captures the essential problems of parsing regular expressions. Token trees also include fully matched delimiters with nested token tree sequences `_(t)_` and `_{t}_` rather than individual delimiters (we write token tree delimiters with an underline to distinguish them from token delimiters).

Each token and token tree also carries their line number from the original source string. Line numbers are needed because there are edge cases in the JavaScript grammar where newlines influence parsing. For example, the following function returns the object literal `{x: y}` as expected.

```js
function f(y) {
return { x: y }
}
```

However, adding a newline causes this function to return undefined, because the grammar calls for an implicit semicolon to be inserted after the return keyword.

```js
function g(y) {
return
{ x: y }
}
```

For clarity of presentation, we leave token line numbers implicit unless we require them, in which case we use the notation `{l` where `l` is a line number. We write a token sequence by separating elements with a dot so the source string `foo(/)/)` is lexed into a sequence of six tokens `foo·(·/·)·/·)`. The equivalent token tree sequence is `foo·_(·/)/·)_`.

## 2.2 Read Algorithm

The key idea of read is to maintain a prefix of already read token trees. When the reader comes to a slash and needs to decide if it should read the slash as a divide token or the start of a regular expression literal, it consults the prefix. Looking back at most five tokens trees in the prefix is sufficient to disambiguate the slash token. Note that this may correspond to looking back an unbounded distance in the original token stream.

Some of the cases of read are relatively obvious. For example, if the token just read was one of the binary operators (e.g. the `+` in `f·+·/·}·/`) the slash will always be the start of a regular expression literal.

Other cases require additional context to disambiguate. For example, if the previous token tree was a parentheses (e.g. `foo·(·x·)·/·y`) then slash will be the divide operator, unless the token tree before the parentheses was the keyword `if`, in which case it is actually the start of a regular expression (since single statement if bodies do not require braces).

```js
if (x) /}/ // regex
```

One of the most complicated cases is a slash after curly braces. Part of the complication here is that curly braces can either indicate an object literal (in which case the slash should be a divide) or a block (in which case the slash should be a regular expression), but even more problematic is that both object literals and blocks with labeled statements can nest. For example, in the following code snippet the outer curly brace is a block with a labeled statement `x`, which is another block with a labeled statement `y` followed by a regular expression literal.

```js
{
x:{y: z} /}/ // regex
}
```

But if we change the code slightly, the outer curly braces become an object literal and `x` is a property so the inner curly braces are also an object literal and thus the slash is a divide operator.

```js
o = {
x:{y: z} /x/g // divide
}
```

While it is unlikely that a programmer would attempt to intentionally perform division on an object literal, it is not a parse error. In fact, this is not even a runtime error since JavaScript will implicitly convert the object to a number (technically `NaN`) and then perform the division (yielding `NaN`).

The reader handles these cases by checking if the prefix of a curly brace forces the brace to be an object literal or a statement block and then setting a boolean flag to be used while reading the tokens inside of the braces.

Based on this discussion, our reader is implemented as a function that takes a sequence of tokens, a prefix of previously read token trees, a boolean indicating if the token stream currently being read is inside an object literal, and returns a sequence of token trees.

```js
read : Token*->TokenTree*->Bool->TokenTree*
```

The implementation of read shown in Figure 3 includes an auxiliary function `isExprPrefix` used to determine if the prefix for a curly brace indicates that the braces should be part of an expression (i.e. the braces are an object literal) or if they should be a block statement.

Interestingly, the `isExprPrefix` function must also be used when the prefix before a slash contains a function definition. This is because there are two kinds of function definitions in JavaScript, function expressions and function declarations, and these also affect how slash is read. For example, a slash following a function declaration is always the start of a regular expression:

```js
function f() {}
/}/ // regex
```

However, a slash following a function expression is a divide operator:

```js
x = function f() { }
/y/g // divide
```

As in the object literal case, it is unlikely that a programmer would attempt to intentionally divide a function expression but it is not an error to do so.

## 2.3 Proving Read

To show that our read algorithm correctly distinguishes divide operations from regular expression literals, we show that a parser defined over normal tokens produces the same AST as a parser defined over the token trees produced from read.

Figure 2: AST for Simplified JavaScript

```
e ∈ AST ::= x | /r/ | {x: e} | (e) | e.x | e(e)
    | e / e | e + e | e = e | {e} | x:e | if (e) e
    | return | return e
    | function x (x) {e} | e e
```

The parser for normal tokens is defined in Figure 4, and generates ASTs in the abstract syntax shown in Figure 2. A parser for the nonterminal Program is a function from a sequence of tokens to an AST.

```
Program :: Token* -> AST
```

We use notation whereby the grammar production `Program e ::= SourceElements e` means to match the input sequence with `SourceElements e` and produce the resulting `AST e`. 

Note that the grammar we present here is a simplified version of the grammar specified in the ECMAScript 5.1 standard and many nonterminal names are shortened versions of nonterminals in the standard. It is mostly straightforward to extend the algorithm presented here to the full sweet.js implementation for ES5 JavaScript. 

In addition to the *Program* parser just described, we also define a parser *Program'* that works over token trees. The rules of the two parsers are identical, except that all rules with delimiters and regular expression literals change as follows:

```
PrimaryExpr /r/ ::= /·r·/
PrimaryExpr'/r/ ::= /r/
PrimaryExpr (e) ::= (·AssignExpre·)
PrimaryExpr'(e) ::= (AssignExpr'e)
```

To prove that read is correct, we show that the following two parsing strategies give identical behavior:

- The traditional parsing strategy takes a token sequence s and parses s into an AST e using the traditional parser Program e.

- The second parsing strategy first reads s into a token tree sequence `read(s, empty, false)`, and then parses this token tree sequence into an AST e via Program'e.

Theorem 1 (Parse Equivalence).

```
∀s ∈ Token
∗
.
s ∈ Program
e
⇔ read(s, empty, false) ∈ Program’
e
```
Proof. The proof proceeds by induction on ASTs to show that parse equivalence holds between all corresponding nonterminals in the two grammars. We present the details of this proof in an appendix available online.







