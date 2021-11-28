# Appendix C Yacc/Bison

In order for Yacc/Bison to parse a language, the language must be described by a **context-free grammar**. The most common formal system for presenting such rules for humans to read is **Backus-Naur Form** or “BNF”, which was developed in order to specify the language Algol 60. Any grammar expressed in BNF is a context-free grammar. The input to Yacc/Bison is essentially machine-readable BNF.

Not all context-free languages can be handled by Yacc/Bison, only those that are **LALR(1)**. In brief, this means that it must be possibly to tell how to parse any portion of an input string with just a single token of look-ahead. Strictly speaking, that is a description of an LR(1) grammar, and LALR(1) involves additional restrictions that are hard to explain simply; but it is rare in actual practice to find an LR(1) grammar that fails to be LALR(1).

## C.1 An Overview

A formal grammar selects tokens only by their classifications: for example, if a rule mentions the terminal symbol ‘integer constant’, it means that any integer constant is grammatically valid in that position. The precise value of the constant is irrelevant to how to parse the input: if `x+4` is grammatical then `x+1` or `x+3989` is equally grammatical.

But the precise value is very important for what the input means once it is parsed. A compiler is useless if it fails to distinguish between 4, 1 and 3989 as constants in the program! Therefore, each token has both a token type and a **semantic value**.

The token type is a terminal symbol defined in the grammar, such as INTEGER, IDENTIFIER or ','. It tells everything you need to know to decide where the token may validly appear and how to group it with other tokens. The grammar rules know nothing about tokens except their types.

The semantic value has all the rest of the information about the meaning of the token, such as the value of an integer, or the name of an identifier. (A token such as ',' which is just punctuation doesn’t need to have any semantic value.)

For example, an input token might be classified as token type INTEGER and have the semantic value 4. Another input token might have the same token type INTEGER but value 3989. When a grammar rule says that INTEGER is allowed, either of these tokens is acceptable because each is an INTEGER. When the parser accepts the token, it keeps track of the token’s semantic value.

Each grouping can also have a semantic value as well as its nonterminal symbol. For example, in a calculator, an expression typically has a semantic value that is a number. In a compiler for a programming language, an expression typically has a semantic value that is a tree structure describing the meaning of the expression.

As Yacc/Bison reads tokens, it pushes them onto a stack along with their semantic values. The stack is called the **parser stack**. Pushing a token is traditionally called **shifting**.

But the stack does not always have an element for each token read. When the last n tokens and groupings shifted match the components of a grammar rule, they can be combined according to that rule. This is called **reduction**. Those tokens and groupings are replaced on the stack by a single grouping whose symbol is the result (left hand side) of that rule. Running the rule’s **action** is part of the process of reduction, because this is what computes the semantic value of the resulting grouping.

The Yacc/Bison parser reads a sequence of tokens as its input, and groups the tokens using the grammar rules. If the input is valid, the end result is that the entire token sequence reduces to a single grouping whose symbol is the grammar’s **start symbol**. If we use a grammar for C, the entire input must be a ‘sequence of definitions and declarations’. If not, the parser reports a syntax error.

The parser tries, by shifts and reductions, to reduce the entire input down to a single grouping whose symbol is the grammar’s start symbol. 

This kind of parser is known in the literature as a bottom-up parser. 

The function `yyparse` is implemented using a finite-state machine. The values pushed on the parser stack are not simply token type codes; they represent the entire sequence of terminal and nonterminal symbols at or near the top of the stack. The current state collects all the information about previous input which is relevant to deciding what to do next.

Each time a look-ahead token is read, the current parser state together with the type of look-ahead token are looked up in a table. This table entry can say, “Shift the look-ahead token.” In this case, it also specifies the new parser state, which is pushed onto the top of the parser stack. Or it can say, “Reduce using rule number n.” This means that a certain of tokens or groupings are taken off the top of the stack, and replaced by one grouping. In other words, that number of states are popped from the stack, and one new state is pushed.

There is one other alternative: the table can say that the look-ahead token is erroneous in the current state. This causes error processing to begin.

## C.2 A Yacc/Bison Example

The following is a Yacc/Bison input file which defines a reverse Polish notation calculator. The file created by Yacc/Bison simulates the calculator. The details of the example are explained in later sections.

```c
/* Reverse Polish notation calculator. */
%{
#define YYSTYPE double
#include <math.h>
%}
%token NUM
%% /* Grammar rules and actions follow */
input : /* empty */
| input line
;
line : '\n'
| exp '\n' { printf ("\t%.10g\n", $1); }
;
exp : NUM { $$ = $1; }
| exp exp '+' { $$ = $1 + $2; }
| exp exp '-' { $$ = $1 - $2; }
| exp exp '*' { $$ = $1 * $2; }
| exp exp '/' { $$ = $1 / $2; }
/* Exponentiation */
| exp exp '^' { $$ = pow ($1, $2); }
/* Unary minus */
| exp 'n' { $$ = -$1; }
;

%%
/* Lexical analyzer returns a double floating point
number on the stack and the token NUM, or the ASCII
character read if not a number. Skips all blanks
and tabs, returns 0 for EOF. */
#include <ctype.h>
yylex ()
{ int c;
/* skip white space */
while ((c = getchar ()) == ' ' || c == '\t')
;
/* process numbers */
if (c == '.' || isdigit (c))
{
ungetc (c, stdin);
scanf ("%lf", &yylval);
return NUM;
}
/* return end-of-file */
if (c == EOF)
return 0;
/* return single chars */
return c;
}
main () /* The ‘‘Main’’ function to make this stand-alone */
{
yyparse ();
}
#include <stdio.h>
yyerror (s) /* Called by yyparse on error */
char *s;
{
printf ("%s\n", s);
}
```

## C.3 The Yacc/Bison Input File

Yacc/Bison takes as input a context-free grammar specification and produces a C-language function that recognizes correct instances of the grammar. The input file for the Yacc/Bison utility is a Yacc/Bison **grammar file**. The Yacc/Bison grammar input file conventionally has a name ending in `.y`.

A Yacc/Bison grammar file has four main sections, shown here with the appropriate delimiters:

```
%{
C declarations
%}
Yacc/Bison declarations
%%
Grammar rules
%%
Additional C code
```

Comments enclosed in `/* . . . */` may appear in any of the sections. The `%%`, `%{` and `%}` are punctuation that appears in every Yacc/Bison grammar file to separate the sections.

The C declarations may define types and variables used in the actions. You can also use preprocessor commands to define macros used there, and use `#include` to include header files that do any of these things.

The Yacc/Bison declarations declare the names of the terminal and nonterminal symbols, and may also describe operator precedence and the data types of semantic values of various symbols.

The grammar rules define how to construct each nonterminal symbol from its parts.

The additional C code can contain any C code you want to use. Often the definition of the lexical analyzer `yylex` goes here, plus subroutines called by the actions in the grammar rules. In a simple program, all the rest of the program can go here.

### C.3.1 The Declarations Section

#### The C Declarations Section

The C declarations section contains macro definitions and declarations of functions and variables that are used in the actions in the grammar rules. These are copied to the beginning of the parser file so that they precede the definition of `yylex`. You can use `#include` to get the declarations from a header file. If you don’t need any C declarations, you may omit the `%{` and `%}` delimiters that bracket this section.

#### The Yacc/Bison Declarations Section

The Yacc/Bison declarations section defines symbols of the grammar. **Symbols** in Yacc/Bison grammars represent the grammatical classifications of the language.

Definitions are provided for the terminal and nonterminal symbols, to specify the precedence and associativity of the operators, and the data types of semantic values.

The first rule in the file also specifies the start symbol, by default. If you want some other symbol to be the start symbol, you must declare it explicitly. Symbol names can contain letters, digits (not at the beginning), underscores and periods. Periods make sense only in nonterminals.

A **terminal symbol** (also known as a **token type**) represents a class of syntactically equivalent tokens. You use the symbol in grammar rules to mean that a token in that class is allowed. The symbol is represented in the Yacc/Bison parser by a numeric code, and the `yylex` function returns a token type code to indicate what kind of token has been read. You don’t need to know what the code value is; you can use the symbol to stand for it. By convention, it should be all upper case. All token type names (but not single-character literal tokens such as '+' and '*') must be declared.

There are two ways of writing terminal symbols in the grammar:

• A **named token type** is written with an identifier, it should be all upper case such as, INTEGER, IDENTIFIER, IF or RETURN. A terminal symbol that stands for a particular keyword in the language should be named after that keyword converted to upper case. Each such name must be defined with a Yacc/Bison declaration such as

```
%token INTEGER IDENTIFIER
```

The terminal symbol `error` is reserved for error recovery. In particular, `yylex` should never return this value.

• A **character token type** (or **literal token**) is written in the grammar using the same syntax used in C for character constants; for example, '+' is a character token type. A character token type doesn’t need to be declared unless you need to specify its semantic value data type, associativity, or precedence.

By convention, a character token type is used only to represent a token that consists of that particular character. Thus, the token type '+' is used to represent the character + as a token. Nothing enforces this convention, but if you depart from it, your program will confuse other readers.

All the usual escape sequences used in character literals in C can be used in Yacc/Bison as well, but you must not use the null character as a character literal because its ASCII code, zero, is the code `yylex` returns for end-of-input.

How you choose to write a terminal symbol has no effect on its grammatical meaning. That depends only on where it appears in rules and on when the parser function returns that symbol.

The value returned by `yylex` is always one of the terminal symbols (or 0 for end-of-input). Whichever way you write the token type in the grammar rules, you write it the same way in the definition of `yylex`. The numeric code for a character token type is simply the ASCII code for the character, so `yylex` can use the identical character constant to generate the requisite code. Each named token type becomes a C macro in the parser file, so `yylex` can use the name to stand for the code. (This is why periods don’t make sense in terminal symbols.) 

If `yylex` is defined in a separate file, you need to arrange for the token-type macro definitions to be available there. Use the `-d` option when you run Yacc/Bison, so that it will write these macro definitions into a separate header file `name.tab.h` which you can include in the other source files that need it.

A **nonterminal symbol** stands for a class of syntactically equivalent groupings. The symbol name is used in writing grammar rules. By convention, it should be all lower case, such as `expr`, `stmt` or `declaration`. Nonterminal symbols must be declared if you need to specify which data type to use for the semantic value.

#### Token Type Names

The basic way to declare a token type name (terminal symbol) is as follows:

```
%token name
```

Yacc/Bison will convert this into a `#define` directive in the parser, so that the function `yylex` (if it is in this file) can use the name `name` to stand for this token type’s code.

Alternatively you can use `%left`, `%right`, or `%nonassoc` instead of `%token`, if you wish to specify precedence.

You can explicitly specify the numeric code for a token type by appending an integer value in the field immediately following the token name:

```
%token NUM 300
```

It is generally best, however, to let Yacc/Bison choose the numeric codes for all token types. Yacc/Bison will automatically select codes that don’t conflict with each other or with ASCII characters.

In the event that the stack type is a union, you must augment the `%token` or other token declaration to include the data type alternative delimited by angle-brackets. For example:

```C
%union {/* define stack type */
double val;
symrec *tptr;
}
%token <val> NUM /* define token NUM and its type */
```

#### Operator Precedence

Use the `%left`, `%right` or `%nonassoc` declaration to declare a token and specify its precedence and associativity, all at once. These are called precedence declarations.

The syntax of a precedence declaration is the same as that of `%token`: either

```C
%left symbols. . .
```

or

```c
%left <type> symbols. . .
```

And indeed any of these declarations serves the purposes of `%token`. But in addition, they specify the associativity and relative precedence for all the symbols:

• The associativity of an operator `op` determines how repeated uses of the operator nest: whether `x op y op z` is parsed by grouping x with y first or by grouping y with z first. `%left` specifies left-associativity (grouping x with y first) and `%right` specifies right-associativity (grouping y with z first). `%nonassoc` specifies no associativity, which means that `x op y op z` is considered a syntax error.

• The precedence of an operator determines how it nests with other operators. All the tokens declared in a single precedence declaration have equal precedence and nest together according to their associativity. When two tokens declared in different precedence declarations associate, the one declared later has the higher precedence and is grouped first.

#### The Collection of Value Types

The `%union` declaration specifies the entire collection of possible data types for semantic values. The keyword `%union` is followed by a pair of braces containing the same thing that goes inside a union in C. For example:

```C
%union {
double val;
symrec *tptr;
}
```

This says that the two alternative types are `double` and `symrec *`. They are given names `val` and `tptr`; these names are used in the `%token` and `%type` declarations to pick one of the types for a terminal or nonterminal symbol.

Note that, unlike making a union declaration in C, you do not write a semicolon after the closing brace.

#### Yacc/Bison Declaration Summary

Here is a summary of all Yacc/Bison declarations:

`%union` Declare the collection of data types that semantic values may have.

`%token` Declare a terminal symbol (token type name) with no precedence or associativity specified.

`%right` Declare a terminal symbol (token type name) that is right-associative.

`%left` Declare a terminal symbol (token type name) that is left-associative.

`%nonassoc` Declare a terminal symbol (token type name) that is nonassociative (using it in a way that would be associative is a syntax error).

`%type <non-terminal>` Declare the type of semantic values for a nonterminal symbol. When you use `%union` to specify multiple value types, you must declare the value type of each nonterminal symbol for which values are used. This is done with a `%type` declaration. Here nonterminal is the name of a nonterminal symbol, and type is the name given in the `%union` to the alternative that you want. You can give any number of nonterminal symbols in the same `%type` declaration, if they have the same value type. Use spaces to separate the symbol names.

`%start <non-terminal>` Specify the grammar’s start symbol. Yacc/Bison assumes by default that the start symbol for the grammar is the first nonterminal specified in the grammar specification section. The programmer may override this restriction with the `%start` declaration.

### C.3.2 The Grammar Rules Section

The grammar rules section contains one or more Yacc/Bison grammar rules, and nothing else.

There must always be at least one grammar rule, and the first `%%` (which precedes the grammar rules) may never be omitted even if it is the first thing in the file.

A Yacc/Bison grammar rule has the following general form:

```
result : components. . . ;
```

where `result` is the nonterminal symbol that this rule describes and `components` are various terminal and nonterminal symbols that are put together by this rule. For example,

```
exp : exp '+' exp ;
```

says that two groupings of type `exp`, with a `+` token in between, can be combined into a larger grouping of type `exp`.

Whitespace in rules is significant only to separate symbols. You can add extra whitespace as you wish.

Scattered among the components can be actions that determine the semantics of the rule. An action looks like this:

```
{C statements}
```

Usually there is only one action and it follows the components.

Multiple rules for the same result can be written separately or can be joined with the vertical-bar character `|` as follows:

```
result : rule1-components. . .
| rule2-components. . .
. . .
;
```

They are still considered distinct rules even when joined in this way.

If components in a rule is empty, it means that result can match the empty string. For example, here is how to define a comma-separated sequence of zero or more `exp` groupings:

```C
expseq : /* empty */
| expseq1
;
expseq1 : exp
| expseq1 ',' exp
;
```

It is customary to write a comment `/* empty */` in each rule with no components.

A rule is called recursive when its result nonterminal appears also on its right hand side. Nearly all Yacc/Bison grammars need to use recursion, because that is the only way to define a sequence of any number of somethings. Consider this recursive definition of a comma-separated sequence of one or more expressions:

```
expseq1 : exp
| expseq1 ',' exp
;
```

Since the recursive use of `expseq1` is the leftmost symbol in the right hand side, we call this **left recursion**. By contrast, here the same construct is defined using right recursion:

```
expseq1 : exp
| exp ',' expseq1 ;
```

Any kind of sequence can be defined using either left recursion or right recursion, but you should always use **left recursion**, because it can parse a sequence of any number of elements with bounded stack space. Right recursion uses up space on the Yacc/Bison stack in proportion to the number of elements in the sequence, because all the elements must be shifted onto the stack before the rule can be applied even once.

Indirect or mutual recursion occurs when the result of the rule does not appear directly on its right hand side, but does appear in rules for other nonterminals which do appear on its right hand side. For example:

```
expr : primary
| primary '+' primary
;
primary : constant
| '(' expr ')'
;
```

defines two mutually-recursive nonterminals, since each refers to the other.

#### Semantic Actions

In order to be useful, a program must do more than parse input; it must also produce some output based on the input. In a Yacc/Bison grammar, a grammar rule can have an action made up of C statements. Each time the parser recognizes a match for that rule, the action is executed.

Most of the time, the purpose of an action is to compute the semantic value of the whole construct from the semantic values of its parts. For example, suppose we have a rule which says an expression can be the sum of two expressions. When the parser recognizes such a sum, each of the subexpressions has a semantic value which describes how it was built up. The action for this rule should create a similar sort of value for the newly recognized larger expression.

For example, here is a rule that says an expression can be the sum of two subexpressions:

```C
expr : expr '+' expr { $$ = $1 + $3; }
;
```

The action says how to produce the semantic value of the sum expression from the values of the two subexpressions.

### C.3.3 Defining Language Semantics

The grammar rules for a language determine only the syntax. The semantics are determined by the semantic values associated with various tokens and groupings, and by the actions taken when various groupings are recognized.

For example, the calculator calculates properly because the value associated with each expression is the proper number; it adds properly because the action for the grouping `x + y` is to add the numbers associated with `x` and `y`.

#### Data Types of Semantic Values

In a simple program it may be sufficient to use the same data type for the semantic values of all language constructs. Yacc/Bison’s default is to use type `int` for all semantic values. To specify some other type, define `YYSTYPE` as a macro, like this:

```C
#define YYSTYPE double
```

This macro definition must go in the C declarations section of the grammar file.

#### More Than One Value Type

In most programs, you will need different data types for different kinds of tokens and groupings. For example, a numeric constant may need type `int` or `long`, while a string constant needs type `char *`, and an identifier might need a pointer to an entry in the symbol table.

To use more than one data type for semantic values in one parser, Yacc/Bison requires you to do two things:

• Specify the entire collection of possible data types, with the `%union` Yacc/Bison declaration.

• Choose one of those types for each symbol (terminal or nonterminal) for which semantic values are used. This is done for tokens with the `%token` Yacc/Bison declaration and for groupings with the `%type` Yacc/Bison declaration.

An action accompanies a syntactic rule and contains C code to be executed each time an instance of that rule is recognized. The task of most actions is to compute a semantic value for the grouping built by the rule from the semantic values associated with tokens or smaller groupings.

An action consists of C statements surrounded by braces, much like a compound statement in C. It can be placed at any position in the rule; it is executed at that position. Most rules have just one action at the end of the rule, following all the components. Actions in the middle of a rule are tricky and used only for special purposes.

The C code in an action can refer to the semantic values of the components matched by the rule with the construct `$n`, which stands for the value of the **nth** component. The semantic value for the grouping being constructed is `$$`. (Yacc/Bison translates both of these constructs into array element references when it copies the actions into the parser file.)

Here is a typical example:

```
exp : . . .
| exp '+' exp
{ $$ = $1 + $3; }
```

This rule constructs an `exp` from two smaller `exp` groupings connected by a plus-sign token. In the action, `$1` and `$3` refer to the semantic values of the two component `exp` groupings, which are the first and third symbols on the right hand side of the rule. The sum is stored into `$$` so that it becomes the semantic value of the addition-expression just recognized by the rule. If there were a useful semantic value associated with the `+` token, it could be referred to as `$2`.

`$n` with n zero or negative is allowed for reference to tokens and groupings on the stack before those that match the current rule. This is a very risky practice, and to use it reliably you must be certain of the context in which the rule is applied. Here is a case in which you can use this reliably:

```
foo : expr bar '+' expr { . . . }
| expr bar '-' expr { . . . }
;
bar : /* empty */
{ previous expr = $0; }
;
```

As long as bar is used only in the fashion shown here, `$0` always refers to the `expr` which precedes bar in the definition of foo.

#### Data Types of Values in Actions

If you have chosen a single data type for semantic values, the `$$` and `$n` constructs always have that data type.

If you have used `%union` to specify a variety of data types, then you must declare a choice among these types for each terminal or nonterminal symbol that can have a semantic value. Then each time you use `$$` or `$n`, its data type is determined by which symbol it refers to in the rule. In this example

```
exp : . . .
| exp '+' exp
{ $$ = $1 + $3; }
```

`$1` and `$3` refer to instances of `exp`, so they all have the data type declared for the nonterminal symbol `exp`. If `$2` were used, it would have the data type declared for the terminal symbol ’`+`’, whatever that might be.


Alternatively, you can specify the data type when you refer to the value, by inserting `<type>` after the `$` at the beginning of the reference. For example, if you have defined types as shown here:

```c
%union {
int itype;
double dtype;
}
```

then you can write `$<itype>1` to refer to the first subunit of the rule as an integer, or `$<dtype>1` to refer to it as a double.

#### Actions in Mid-Rule

Occasionally it is useful to put an action in the middle of a rule. These actions are written just like usual end-of-rule actions, but they are executed before the parser even recognizes the following components.

A mid-rule action may refer to the components preceding it using `$n`, but it may not refer to subsequent components because it is run before they are parsed.

The mid-rule action itself counts as one of the components of the rule. This makes a difference when there is another action later in the same rule (and usually there is another at the end): you have to count the actions along with the symbols when working out which number n to use in `$n`.

The mid-rule action can also have a semantic value. This can be set within that action by an assignment to `$$`, and can referred to by later actions using `$n`. Since there is no symbol to name the action, there is no way to declare a data type for the value in advance, so you must use the `$<...>` construct to specify a data type each time you refer to this value.

Here is an example from a hypothetical compiler, handling a `let` statement that looks like `let (variable) statement` and serves to create a variable named variable temporarily for the duration of statement. To parse this construct, we must put variable into the symbol table while statement is parsed, then remove it afterward. Here is how it is done:

```C
stmt : LET '(' var ')'
{ $<context>$ = push context ();
  declare variable ($3); }
stmt { $$ = $6;
       pop context ($<context>5); }
```

As soon as `let (variable)` has been recognized, the first action is run. It saves a copy of the current semantic context (the list of accessible variables) as its semantic value, using alternative context in the data-type union. Then it calls declare variable to add the new variable to that list. Once the first action is finished, the embedded statement `stmt` can be parsed. Note that the mid-rule action is component number 5, so the `stmt` is component number 6.

After the embedded statement is parsed, its semantic value becomes the value of the entire let-statement. Then the semantic value from the earlier action is used to restore the prior list of variables. This removes the temporary let-variable from the list so that it won’t appear to exist while the rest of the program is parsed.

Taking action before a rule is completely recognized often leads to conflicts since the parser must commit to a parse in order to execute the action. For example, the following two rules, without mid-rule actions, can coexist in a working parser because the parser can shift the open-brace token and look at what follows before deciding whether there is a declaration or not:

```
compound : '{' declarations statements '}'
| '{' statements '}'
;
```

But when we add a mid-rule action as follows, the rules become nonfunctional:

```
compound : { prepare for local variables (); }
'{' declarations statements '}'
| '{' statements '}'
;
```

Now the parser is forced to decide whether to run the mid-rule action when it has read no farther than the open-brace. In other words, it must commit to using one rule or the other, without sufficient information to do it correctly. (The open-brace token is what is called the *look-ahead* token at this time, since the parser is still deciding what to do about it.)

You might think that you could correct the problem by putting identical actions into the two rules, like this:

```
compound : { prepare for local variables (); }
'{' declarations statements '}'
| { prepare for local variables (); }
'{' statements '}'
;
```

But this does not help, because Yacc/Bison does not realize that the two actions are identical. (Yacc/Bison never tries to understand the C code in an action.) If the grammar is such that a declaration can be distinguished from a statement by the first token (which is true in C), then one solution which does work is to put the action after the open-brace, like this:

```
compound : '{' { prepare for local variables (); }
declarations statements '}'
| '{' statements '}'
;
```

Now the first token of the following declaration or statement, which would in any case tell Yacc/Bison which rule to use, can still do so.

Another solution is to bury the action inside a nonterminal symbol which serves as a subroutine:

```
subroutine : /* empty */
{ prepare for local variables (); }
;
compound : subroutine
'{' declarations statements '}'
| subroutine
'{' statements '}'
;
```

Now Yacc/Bison can execute the action in the rule for subroutine without deciding which rule for compound it will eventually use. Note that the action is now at the end of its rule. Any mid-rule action can be converted to an end-of-rule action in this way, and this is what Yacc/Bison actually does to implement mid-rule actions.

### C.3.4 The Additional C Code Section

The additional C code section is copied verbatim to the end of the parser file, just as the C declarations section is copied to the beginning. This is the most convenient place to put anything that you want to have in the parser file but which need not come before the definition of `yylex`. For example, the definitions of `yylex` and `yyerror` often go here.

If the last section is empty, you may omit the `%%` that separates it from the grammar rules.

The Yacc/Bison parser itself contains many static variables whose names start with `yy` and many macros whose names start with `YY`. It is a good idea to avoid using any such names (except those documented in this manual) in the additional C code section of the grammar file.

It is not usually acceptable to have a program terminate on a parse error. For example, a compiler should recover sufficiently to parse the rest of the input file and check it for errors.

## C.4 Yacc/Bison Output: the Parser File

When you run Yacc/Bison, you give it a Yacc/Bison grammar file as input. The output is a C source file that parses the language described by the grammar. This file is called a Yacc/Bison **parser**. Keep in mind that the Yacc/Bison utility and the Yacc/Bison parser are two distinct programs: the Yacc/Bison utility is a program whose output is the Yacc/Bison parser that becomes part of your program.

The job of the Yacc/Bison parser is to group tokens into groupings according to the grammar rules — for example, to build identifiers and operators into expressions. As it does this, it runs the actions for the grammar rules it uses. 

The tokens come from a function called the **lexical analyzer** that you must supply in some fashion (such as by writing it in C or using Lex/Flex). The Yacc/Bison parser calls the lexical analyzer each time it wants a new token. It doesn’t know what is “inside” the tokens (though their semantic values may reflect this). Typically the lexical analyzer makes the tokens by parsing characters of text, but Yacc/Bison does not depend on this.

The Yacc/Bison parser file is C code which defines a function named `yyparse` which implements that grammar. This function does not make a complete C program: you must supply some additional functions. One is the lexical analyzer. Another is an error-reporting function which the parser calls to report an error. In addition, a complete C program must start with a function called `main`; you have to provide this, and arrange for it to call `yyparse` or the parser will never run.

Aside from the token type names and the symbols in the actions you write, all variable and function names used in the Yacc/Bison parser file begin with `yy` or `YY`. This includes interface functions such as the lexical analyzer function `yylex`, the error reporting function `yyerror` and the parser function `yyparse` itself. This also includes numerous identifiers used for internal purposes. Therefore, you should avoid using C identifiers starting with `yy` or `YY` in the Yacc/Bison grammar file except for the ones defined in this manual.

## C.5 Parser C-Language Interface

The Yacc/Bison parser is actually a C function named `yyparse`. Here we describe the interface conventions of `yyparse` and the other functions that it needs to use.

Keep in mind that the parser uses many C identifiers starting with `yy` and `YY` for internal purposes. If you use such an identifier (aside from those in this manual) in an action or in additional C code in the grammar file, you are likely to run into trouble.

### The Parser Function yyparse

You call the function `yyparse` to cause parsing to occur. This function reads tokens, executes actions, and ultimately returns when it encounters end-of-input or an unrecoverable syntax error. You can also write an action which directs `yyparse` to return immediately without reading further.

The value returned by `yyparse` is 0 if parsing was successful (return is due to end-of-input).

The value is 1 if parsing failed (return is due to a syntax error).

In an action, you can cause immediate return from `yyparse` by using these macros:

`YYACCEPT` Return immediately with value 0 (to report success).

`YYABORT` Return immediately with value 1 (to report failure).

### The Lexical Analyzer Function yylex

The lexical analyzer function, `yylex`, recognizes tokens from the input stream and returns them to the parser. Yacc/Bison does not create this function automatically; you must write it so that `yyparse` can call it. The function is sometimes referred to as a lexical scanner.

In simple programs, `yylex` is often defined at the end of the Yacc/Bison grammar file. If `yylex` is defined in a separate source file, you need to arrange for the token-type macro definitions to be available there. To do this, use the `-d` option when you run Yacc/Bison, so that it will write these macro definitions into a separate header file `name.tab.h` which you can include in the other source files that need it.

### Calling Convention for yylex

The value that `yylex` returns must be the numeric code for the type of token it has just found, or 0 for end-of-input.

When a token is referred to in the grammar rules by a name, that name in the parser file becomes a C macro whose definition is the proper numeric code for that token type. So `yylex` can use the name to indicate that type.

When a token is referred to in the grammar rules by a character literal, the numeric code for that character is also the code for the token type. So `yylex` can simply return that character code. The null character must not be used this way, because its code is zero and that is what signifies end-of-input.

Here is an example showing these things:

```C
yylex()
{
. . .
if (c == EOF) /* Detect end of file. */
return 0;
. . .
if (c == '+' || c == '-')
return c; /* Assume token type for ‘+’ is '+'. */
. . .
return INT; /* Return the type of the token. */
. . .
}
```

This interface has been designed so that the output from the lex utility can be used without change as the definition of `yylex`.

### Semantic Values of Tokens

In an ordinary (non-reentrant) parser, the semantic value of the token must be stored into the global variable `yylval`. When you are using just one data type for semantic values, `yylval` has that type. Thus, if the type is `int` (the default), you might write this in `yylex`:

```C
. . .
yylval = value; /* Put value onto Yacc/Bison stack. */
return INT; /* Return the type of the token. */
. . .
```

When you are using multiple data types, `yylval`’s type is a union made from the `%union` declaration. So when you store a token’s value, you must use the proper member of the union. If the `%union` declaration looks like this:

```c
%union {
int intval;
double val;
symrec *tptr;
}
```

then the code in `yylex` might look like this:

```c
. . .
yylval.intval = value; /* Put value onto Yacc/Bison stack. */
return INT; /* Return the type of the token. */
. . .
```

### Textual Positions of Tokens

If you are using the @n-feature in actions to keep track of the textual locations of tokens and groupings, then you must provide this information in `yylex`. The function `yyparse` expects to find the textual location of a token just parsed in the global variable `yylloc`. So `yylex` must store the proper data in that variable. The value of `yylloc` is a structure and you need only initialize the members that are going to be used by the actions. The four members are called first line, first column, last line and last column. Note that the use of this feature makes the parser noticeably slower.

The data type of `yylloc` has the name `YYLTYPE`.

### The Error Reporting Function yyerror

The Yacc/Bison parser detects a parse error or syntax error whenever it reads a token which cannot satisfy any syntax rule. A action in the grammar can also explicitly proclaim an error, using the macro `YYERROR`.

The Yacc/Bison parser expects to report the error by calling an error reporting function named `yyerror`, which you must supply. It is called by `yyparse` whenever a syntax error is found, and it receives one argument. For a parse error, the string is always "parse error".

The following definition suffices in simple programs:

```c
yyerror (s)
char *s;
{
  fprintf (stderr, "%s", s);
}
```

After `yyerror` returns to `yyparse`, the latter will attempt error recovery if you have written suitable error recovery grammar rules. If recovery is impossible, `yyparse` will immediately return 1.

## C.6 Debugging Your Parser

### Shift/Reduce Conflicts

Suppose we are parsing a language which has if-then and if-then-else statements, with a pair of rules like this:

```c
if stmt : IF expr THEN stmt
| IF expr THEN stmt ELSE stmt
;
```

(Here we assume that IF, THEN and ELSE are terminal symbols for specific keyword tokens.)

When the ELSE token is read and becomes the look-ahead token, the contents of the stack (assuming the input is valid) are just right for reduction by the first rule. But it is also legitimate to shift the ELSE, because that would lead to eventual reduction by the second rule.

This situation, where either a shift or a reduction would be valid, is called a shift/reduce conflict. Yacc/Bison is designed to resolve these conflicts by choosing to shift, unless otherwise directed by operator precedence declarations. To see the reason for this, let’s contrast it with the other alternative.

Since the parser prefers to shift the ELSE, the result is to attach the else-clause to the innermost if-statement, making these two inputs equivalent:

```F#
if x then if y then win(); else lose;
if x then do; if y then win(); else lose; end;
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

### Specifying Operator Precedence

Yacc/Bison allows you to specify these choices with the operator precedence declarations. Each such declaration contains a list of tokens, which are operators whose precedence and associativity is being declared. The `%left` declaration makes all those operators left-associative and the `%right` declaration makes them right-associative. A third alternative is `%nonassoc`, which declares that it is a syntax error to find the same operator twice “in a row”.

The relative precedence of different operators is controlled by the order in which they are declared. The first `%left` or `%right` declaration in the file declares the operators whose precedence is lowest, the next such declaration declares the operators whose precedence is a little higher, and so on.

### Precedence Examples

In our example, we would want the following declarations:

```C
%left '<'
%left '-'
%left '*'
```

In a more complete example, which supports other operators as well, we would declare them in groups of equal precedence. For example, ’`+`’ is declared with ’`-`’:

```C
%left '<' '>' '=' NE LE GE
%left '+' '-'
%left '*' '/'
```

(Here NE and so on stand for the operators for “not equal” and so on. We assume that these tokens are more than one character long and therefore are represented by names, not character literals.)

Often the precedence of an operator depends on the context. For example, a minus sign typically has a very high precedence as a unary operator, and a somewhat lower precedence (lower than multiplication) as a binary operator.

The Yacc/Bison precedence declarations, `%left`, `%right` and `%nonassoc`, can only be used once for a given token; so a token has only one precedence declared in this way. For context-dependent precedence, you need to use an additional mechanism: the `%prec` modifier for rules.

The `%prec` modifier declares the precedence of a particular rule by specifying a terminal symbol whose precedence should be used for that rule. It’s not necessary for that symbol to appear otherwise in the rule. The modifier’s syntax is:

```
%prec terminal-symbol
```

and it is written after the components of the rule. Its effect is to assign the rule the precedence of terminal-symbol, overriding the precedence that would be deduced for it in the ordinary way. The altered rule precedence then affects how conflicts involving that rule are resolved.

Here is how `%prec` solves the problem of unary minus. First, declare a precedence for a fictitious terminal symbol named UMINUS. There are no tokens of this type, but the symbol serves to stand for its precedence:

```C
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

### Reduce/Reduce Conflicts

A reduce/reduce conflict occurs if there are two or more rules that apply to the same sequence of input. This usually indicates a serious error in the grammar.

Yacc/Bison resolves a reduce/reduce conflict by choosing to use the rule that appears first in the grammar, but it is very risky to rely on this. Every reduce/reduce conflict must be studied and usually eliminated.

### Error Recovery

You can define how to recover from a syntax error by writing rules to recognize the special token `error`. This is a terminal symbol that is always defined (you need not declare it) and reserved for error handling. The Yacc/Bison parser generates an `error` token whenever a syntax error happens; if you have provided a rule to recognize this token in the current context, the parse can continue. For example:

```c
stmnts : /* empty string */
| stmnts '\'
| stmnts exp '\'
| stmnts error '\'
```


The fourth rule in this example says that an error followed by a newline makes a valid addition to any `stmnts`.

What happens if a syntax error occurs in the middle of an `exp`? The error recovery rule, interpreted strictly, applies to the precise sequence of a `stmnts`, an `error` and a newline. If an error occurs in the middle of an `exp`, there will probably be some additional tokens and subexpressions on the stack after the last `stmnts`, and there will be tokens to read before the next newline. So the rule is not applicable in the ordinary way.

But Yacc/Bison can force the situation to fit the rule, by discarding part of the semantic context and part of the input. First it discards states and objects from the stack until it gets back to a state in which the `error` token is acceptable. (This means that the subexpressions already parsed are discarded, back to the last complete `stmnts`.) At this point the `error` token can be shifted. Then, if the old look-ahead token is not acceptable to be shifted next, the parser reads tokens and discards them until it finds a token which is acceptable. In this example, Yacc/Bison reads and discards input until the next newline so that the fourth rule can apply.

The choice of error rules in the grammar is a choice of strategies for error recovery. A simple and useful strategy is simply to skip the rest of the current input line or current statement if an error is detected:

```c
stmnt : error ';' /* on error, skip until ';' is read */
```

It is also useful to recover to the matching close-delimiter of an opening-delimiter that has already been parsed. Otherwise the close-delimiter will probably appear to be unmatched, and generate another, spurious error message:

```C
primary : '(' expr ')'
| '(' error ')'
. . .
;
```

Error recovery strategies are necessarily guesses. When they guess wrong, one syntax error often leads to another. To prevent an outpouring of error messages, the parser will output no error message for another syntax error that happens shortly after the first; only after three consecutive input tokens have been successfully shifted will error messages resume.

### Further Debugging

If a Yacc/Bison grammar compiles properly but doesn’t do what you want when it runs, the `yydebug` parser-trace feature can help you figure out why.

To enable compilation of trace facilities, you must define the macro `YYDEBUG` when you compile the parser. You could use `-DYYDEBUG=1` as a compiler option or you could put `#define YYDEBUG 1` in the C declarations section of the grammar file. Alternatively, use the `-t` option when you run Yacc/Bison. We always define `YYDEBUG` so that debugging is always possible.

The trace facility uses `stderr`, so you must add `#include <stdio.h>` to the C declarations section unless it is already there.

Once you have compiled the program with trace facilities, the way to request a trace is to store a nonzero value in the variable `yydebug`. You can do this by making the C code do it (in `main`).

Each step taken by the parser when `yydebug` is nonzero produces a line or two of trace information, written on `stderr`. The trace messages tell you these things:

• Each time the parser calls `yylex`, what kind of token was read.

• Each time a token is shifted, the depth and complete contents of the state stack.

• Each time a rule is reduced, which rule it is, and the complete contents of the state stack afterward.

To make sense of this information, it helps to refer to the listing file produced by the Yacc/Bison `-v` option. This file shows the meaning of each state in terms of positions in various rules, and also what each state will do with each possible input token. As you read the successive trace messages, you can see that the parser is functioning according to its specification in the listing file. Eventually you will arrive at the place where something undesirable happens, and you will see which parts of the grammar are to blame.

## C.7 Stages in Using Yacc/Bison

The actual language-design process using Yacc/Bison, from grammar specification to a working compiler or interpreter, has these parts:

1. Formally specify the grammar in a form recognized by Yacc/Bison. For each grammatical rule in the language, describe the action that is to be taken when an instance of that rule is recognized. The action is described by a sequence of C statements.

2. Write a lexical analyzer to process input and pass tokens to the parser. The lexical analyzer may be written by hand in C. It could also be produced using Lex.

3. Write a controlling function that calls the Yacc/Bison-produced parser.

4. Write error-reporting routines.

To turn this source code as written into a runnable program, you must follow these steps:

1. Run Yacc/Bison on the grammar to produce the parser. The usual way to invoke Yacc/Bison is as follows:

```
bison infile
```

Here `infile` is the grammar file name, which usually ends in `.y`. The parser file’s name is made by replacing the `.y` with `.tab.c`. Thus, the bison `foo.y` filename yields `foo.tab.c`.

2. Compile the code output by Yacc/Bison, as well as any other source files.

3. Link the object files to produce the finished product.