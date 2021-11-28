# Yacc Input File

Yacc takes as input a context-free grammar specification and produces a C-language function that recognizes correct instances of the grammar. The input file for the Yacc utility is a Yacc grammar file. The Yacc grammar input file conventionally has a name ending in `.yacc`.

A Yacc grammar file has two main sections, shown here with the appropriate delimiters:

```livescript
Grammar rules
%%
precedences declarations
```

Comments enclosed in `/* . . . */` may appear in any of the sections. The line with just `%%` in it are punctuation that appears in every Yacc grammar file to separate the sections.

The precedences declarations describe operator precedence.

The grammar rules define how to construct each nonterminal symbol from its parts.

## C.3.2 The Grammar Rules Section

The grammar rules section contains one or more Yacc grammar rules, and nothing else.

A Yacc grammar rule has the following general form:

```c
result : components. . . ;
```

where **result** is the nonterminal symbol that this rule describes and **components** are various terminal and nonterminal symbols that are put together by this rule. For example,

```c
exp : exp '+' exp ;
```

says that two groupings of type `exp`, with a `+` token in between, can be combined into a larger grouping of type `exp`.

Whitespace in rules is significant only to separate symbols. You can add extra whitespace as you wish.

Multiple rules for the same result can be written separately or can be joined with the vertical-bar character `|` as follows:

```c
result : rule1-components. . .
       | rule2-components. . .
       . . .
;
```

They are still considered distinct rules even when joined in this way.

If components in a rule is empty, it means that result can match the empty string. For example, here is how to define a comma-separated sequence of zero or more `exp` groupings:

```c
expseq : /* empty */
       | expseq1
;
expseq1 : exp
        | expseq1 ',' exp
;
```

It is customary to write a comment `/* empty */` in each rule with no components.

A rule is called recursive when its result nonterminal appears also on its right hand side. Nearly all Yacc grammars need to use recursion, because that is the only way to define a sequence of any number of somethings. Consider this recursive definition of a comma-separated sequence of one or more expressions:

```c
expseq1 : exp
        | expseq1 ',' exp
;
```

Since the recursive use of `expseq1` is the leftmost symbol in the right hand side, we call this **left recursion**. By contrast, here the same construct is defined using **right recursion**:

```c
expseq1 : exp
        | exp ',' expseq1 
;
```

Any kind of sequence can be defined using either left recursion or right recursion, but you should always use **left recursion**, because it can parse a sequence of any number of elements with bounded stack space. Right recursion uses up space on the Yacc stack in proportion to the number of elements in the sequence, because all the elements must be shifted onto the stack before the rule can be applied even once.

Indirect or mutual recursion occurs when the result of the rule does not appear directly on its right hand side, but does appear in rules for other nonterminals which do appear on its right hand side. For example:

```c
expr : primary
     | primary '+' primary
;
primary : constant
        | '(' expr ')'
;
```

defines two mutually-recursive nonterminals, since each refers to the other.




## Operator Precedence

Use the `%left`, `%right` or `%nonassoc` declaration to declare a token and specify its precedence and associativity, all at once. These are called **precedence declarations**.

The syntax of a precedence declaration is the same as that of `%token`: 

```livescript
%left symbols. . .
```

they specify the associativity and relative precedence for all the symbols:

- The associativity of an operator `op` determines how repeated uses of the operator nest: whether `x op y op z` is parsed by grouping `x` with `y` first or by grouping `y` with `z` first. `%left` specifies left-associativity (grouping `x` with `y` first) and `%right` specifies right-associativity (grouping `y` with `z` first). `%nonassoc` specifies no associativity, which means that `x op y op z` is considered a syntax error.
- The precedence of an operator determines how it nests with other operators. All the tokens declared in a single precedence declaration have equal precedence and nest together according to their associativity. When two tokens declared in different precedence declarations associate, the one declared later has the higher precedence and is grouped first.



### Specifying Operator Precedence

Yacc/Bison allows you to specify these choices with the operator precedence declarations. Each such declaration contains a list of tokens, which are operators whose precedence and associativity is being declared. The `%left` declaration makes all those operators left-associative and the `%right` declaration makes them right-associative. A third alternative is `%nonassoc`, which declares that it is a syntax error to find the same operator twice “in a row”.

The relative precedence of different operators is controlled by the order in which they are declared. The first `%left` or `%right` declaration in the file declares the operators whose precedence is lowest, the next such declaration declares the operators whose precedence is a little higher, and so on.

### Precedence Examples

In our example, we would want the following declarations:

```livescript
%left '<'
%left '-'
%left '*'
```

In a more complete example, which supports other operators as well, we would declare them in groups of equal precedence. For example, `'+'` is declared with `'-'`:

```livescript
%left '<' '>' '=' NE LE GE
%left '+' '-'
%left '*' '/'
```

(Here NE and so on stand for the operators for “not equal” and so on. We assume that these tokens are more than one character long and therefore are represented by names, not character literals.)

Often the precedence of an operator depends on the context. For example, a minus sign typically has a very high precedence as a unary operator, and a somewhat lower precedence (lower than multiplication) as a binary operator.

The Yacc/Bison precedence declarations, `%left`, `%right` and `%nonassoc`, can only be used once for a given token; so a token has only one precedence declared in this way. For context-dependent precedence, you need to use an additional mechanism: the `%prec` modifier for rules.

The `%prec` modifier declares the precedence of a particular rule by specifying a terminal symbol whose precedence should be used for that rule. It’s not necessary for that symbol to appear otherwise in the rule. The modifier’s syntax is:

```livescript
%prec terminal-symbol
```

and it is written after the components of the rule. Its effect is to assign the rule the precedence of `terminal-symbol`, overriding the precedence that would be deduced for it in the ordinary way. The altered rule precedence then affects how conflicts involving that rule are resolved.

Here is how `%prec` solves the problem of unary minus. First, declare a precedence for a fictitious terminal symbol named `UMINUS`. There are no tokens of this type, but the symbol serves to stand for its precedence:

```livescript
. . .
%left '+' '-'
%left '*'
%left UMINUS
```

Now the precedence of `UMINUS` can be used in specific rules:

```c
exp : . . .
    | exp '-' exp
    . . .
    | '-' exp %prec UMINUS
```



#### Yacc Declaration Summary

Here is a summary of all Yacc declarations:

`%right` 

Declare a terminal symbol (token type name) that is right-associative.

`%left` 

Declare a terminal symbol (token type name) that is left-associative.

`%nonassoc` 

Declare a terminal symbol (token type name) that is nonassociative (using it in a way that would be associative is a syntax error).

## C.6 Debugging Your Parser

### Shift/Reduce Conflicts

Suppose we are parsing a language which has if-then and if-then-else statements, with a pair of rules like this:

```c
if_stmt : IF expr THEN stmt
        | IF expr THEN stmt ELSE stmt
;
```

(Here we assume that `IF`, `THEN` and `ELSE` are terminal symbols for specific keyword tokens.)

When the `ELSE` token is read and becomes the look-ahead token, the contents of the stack (assuming the input is valid) are just right for reduction by the first rule. But it is also legitimate to shift the `ELSE`, because that would lead to eventual reduction by the second rule.

This situation, where either a shift or a reduction would be valid, is called a *shift/reduce conflict*. Yacc/Bison is designed to resolve these conflicts by choosing to shift, unless otherwise directed by operator precedence declarations. To see the reason for this, let’s contrast it with the other alternative.

Since the parser prefers to shift the `ELSE`, the result is to attach the else-clause to the innermost if-statement, making these two inputs equivalent:

```F#
if x then if y then win(); else lose;
if x then do;
    if y then win(); else lose; 
end;
```

But if the parser chose to reduce when possible rather than shift, the result would be to attach the else-clause to the outermost if-statement. The conflict exists because the grammar as written is ambiguous: either parsing of the simple nested if-statement is legitimate. The established convention is that these ambiguities are resolved by attaching the else-clause to the innermost if-statement; this is what Yacc/Bison accomplishes by choosing to shift rather than reduce. This particular ambiguity is called the “dangling else” ambiguity.

### Operator Precedence

Another situation where shift/reduce conflicts appear is in arithmetic expressions. Here shifting is not always the preferred resolution; the Yacc/Bison declarations for operator precedence allow you to specify when to shift and when to reduce.

Consider the following ambiguous grammar fragment (ambiguous because the input `1 - 2 * 3` can be parsed in two different ways):

```c
expr : expr '-' expr
     | expr '*' expr
     | expr '<' expr
     | '(' expr ')'
     . . .
;
```

Suppose the parser has seen the tokens `1`, `-` and `2`; should it reduce them via the rule for the addition operator? It depends on the next token. Of course, if the next token is `)`, we must reduce; shifting is invalid because no single rule can reduce the token sequence `- 2 )` or anything starting with that. But if the next token is `*` or `<`, we have a choice: either shifting or reduction would allow the parse to complete, but with different results.

What about input such as `1 - 2 - 5`; should this be `(1 - 2) - 5` or should it be `1 - (2 - 5)`? For most operators we prefer the former, which is called left association. The latter alternative, right association, is desirable for assignment operators. The choice of left or right association is a matter of whether the parser chooses to shift or reduce when the stack contains `1 - 2` and the look-ahead token is `-`: shifting makes right-associativity.

### Reduce/Reduce Conflicts

A reduce/reduce conflict occurs if there are two or more rules that apply to the same sequence of input. This usually indicates a serious error in the grammar.

Yacc/Bison resolves a reduce/reduce conflict by choosing to use the rule that appears first in the grammar, but it is very risky to rely on this. Every reduce/reduce conflict must be studied and usually eliminated.
