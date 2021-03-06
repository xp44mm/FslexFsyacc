# Chapter 2 The Parser

A parser is a program which determines if its input is syntactically valid and determines its structure. Parsers ~~may be hand written or~~ may be automatically generated by a parser generator from descriptions of valid syntactical structures. The descriptions are in the form of a *context-free grammar*. Parser generators may be used to develop a wide range of language parsers, from those used in simple desk calculators to complex programming languages.

Yacc is a program which given a context-free grammar, constructs a C program which will parse input according to the grammar rules. ~~Yacc was developed by S. C. Johnson an others at AT&T Bell Laboratories.~~ Yacc provides for semantic stack manipulation and the specification of semantic routines. A input file for Yacc is of the form:

```livescript
C and parser declarations
%%
Grammar rules and actions
%%
C subroutines
```

The first section of the Yacc file consists of a list of tokens (other than single characters) that are expected by the parser and the specification of the start symbol of the grammar. This section of the Yacc file may contain specification of the precedence and associativity of operators. This permits greater flexibility in the choice of a context-free grammar. Addition and subtraction are declared to be left associative and of lowest precedence while exponentiation is declared to be right associative and to have the highest precedence.

```livescript
%start program
%token LET INTEGER IN
%token SKIP IF THEN ELSE END WHILE DO READ WRITE
%token NUMBER
%token IDENTIFIER
%left '-' '+'
%left '*' '/'
%right 'ˆ'
%%
Grammar rules and actions
%%
C subroutines
```

The second section of the Yacc file consists of the context-free grammar for the language. Productions are separated by semicolons, the `::=` symbol of the BNF is replaced with `:`, the empty production is left empty, nonterminals are written in all lower case, and the multicharacter terminal symbols in all upper case. Notice the simplification of the expression grammar due to the separation of precedence from the grammar.

```livescript
C and parser declarations
%%
program : LET declarations IN commands END ;
declarations : /* empty */
             | INTEGER id_seq IDENTIFIER '.'
             ;
id_seq : /* empty */
       | id_seq IDENTIFIER ','
       ;
commands : /* empty */
         | commands command ';'
         ;
command : SKIP
        | READ IDENTIFIER
        | WRITE exp
        | IDENTIFIER ASSGNOP exp
        | IF exp THEN commands ELSE commands FI
        | WHILE exp DO commands END
        ;
exp : NUMBER
    | IDENTIFIER
    | exp '<' exp
    | exp '=' exp
    | exp '>' exp
    | exp '+' exp
    | exp '−' exp
    | exp '∗' exp
    | exp '/' exp
    | exp 'ˆ ' exp
    | '(' exp ')'
    ;
%%
C subroutines
```

The third section of the Yacc file consists of C code. There must be a `main()` routine which calls the function `yyparse()`. The function `yyparse()` is the driver routine for the parser. There must also be the function `yyerror()` which is used to report on errors during the parse. Simple examples of the function `main()` and `yyerror()` are:

```c
C and parser declarations
%%
Grammar rules and actions
%%
main( int argc, char *argv[] )
{ 
    extern FILE *yyin;
    ++argv; −−argc;
    yyin = fopen( argv[0], "r" );
    yydebug = 1;
    errors = 0;
    yyparse();
}

yyerror (char *s) /* Called by yyparse on error */
{
    printf ("%s\n", s);
}
```

The parser, as written, has no output however, the parse tree is implicitly constructed during the parse. As the parser executes, it builds an internal representation of the the structure of the program. The internal representation is based on the right hand side of the production rules. When a right hand side is recognized, it is reduced to the corresponding left hand side. Parsing is complete when the entire program has been reduced to the start symbol of the grammar.



~~Compiling the Yacc file with the command `yacc -vd file.y` (`bison -vd file.y`) causes the generation of two files `file.tab.h` and `file.tab.c`. The `file.tab.h` contains the list of tokens is included in the file which defines the scanner. The file `file.tab.c` defines the C function `yyparse()` which is the parser.~~

~~Yacc is distributed with the Unix operating system while Bison is a product of the Free Software Foundation, Inc.~~

~~For more information on using Yacc/Bison see the appendix, consult the manual pages for bison, the paper Programming Utilities and Libraries LR Parsing by A. V. Aho and S. C. Johnson, Computing Surveys, June, 1974 and the document “BISON the Yacc-compatible Parser Generator” by Charles Donnelly and Richard Stallman.~~
