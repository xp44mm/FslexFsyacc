# Chapter 4 The Context

Lex and Yacc files can be extended to handle the context sensitive information. For example, suppose we want to require that, in Simple, we require that variables be declared before they are referenced. Therefore the parser must be able to compare variable references with the variable declarations.

One way to accomplish this is to construct a list of the variables during the parse of the declaration section and then check variable references against the those on the list. Such a list is called a *symbol table*. Symbol tables may be implemented using lists, trees, and hash-tables.

We modify the Lex file to assign the global variable `yylval` to the identifier string since the information will be needed by the attribute grammar.

## The Symbol Table Module

To hold the information required by the attribute grammar we construct a symbol table. A symbol table contains the environmental information concerning the attributes of various programming language constructs. In particular, the type and scope information for each variable.

The symbol table will be developed as a module to be included in the yacc/bison file.

The symbol table for Simple consists of a linked list of identifiers, initially empty. Here is the declaration of a node, initialization of the list to empty and

```C
struct symrec
{
    char *name; /* name of symbol */
    struct symrec *next; /* link field */
};
typedef struct symrec symrec;
symrec *sym_table = (symrec *) 0;
symrec *putsym ();
symrec *getsym ();
```

and two operations: `putsym` to put an identifier into the table,

```C
symrec *putsym ( char *sym_name )
{
    symrec *ptr;
    ptr = (symrec *) malloc (sizeof(symrec));
    ptr -> name = (char *) malloc (strlen(sym_name)+1);
    strcpy (ptr -> name,sym_name);
    ptr -> next = (struct symrec *)sym_table;
    sym_table = ptr;
    return ptr;
}
```

and `getsym` which returns a pointer to the symbol table entry corresponding to an identifier.

```C
symrec *getsym ( char *sym_name )
{
   symrec *ptr;
   for (ptr = sym_table; ptr != (symrec *) 0;
       ptr = (symrec *)ptr -> next)
       if (strcmp (ptr->name,sym_name) == 0)
           return ptr;
   return 0;
}
```

## The Parser Modifications

The Yacc/Bison file is modified to include the symbol table and with routines to perform the installation of an identifier in the symbol table and to perform context checking.

```livescript
%{
#include <stdlib.h> /* For malloc in symbol table */
#include <string.h> /* For strcmp in symbol table */
#include <stdio.h> /* For error messages */
#include "ST.h" /* The Symbol Table Module */
#define YYDEBUG 1 /* For debugging */
install ( char *sym_name )
{ symrec *s;
    s = getsym (sym_name);
    if (s == 0)
        s = putsym (sym_name);
    else { errors++;
           printf( "%s is already defined\n", sym_name );
    }
}
context_check( char *sym_name )
{ if ( getsym( sym_name ) == 0 )
    printf( "%s is an undeclared identifier\n", sym_name );
}
%}
Parser declarations
%%
Grammar rules and actions
%%
C subroutines
```

Since the scanner (the Lex file) will be returning identifiers, a semantic record (static semantics) is required to hold the value and `IDENT` is associated with that semantic record.

```livescript
C declarations
%union { /* SEMANTIC RECORD */
   char *id; /* For returning identifiers */
}
%token NUMBER SKIP IF THEN ELSE FI WHILE DO END
%token <id> IDENT /* Simple identifier */
%left '-' '+'
%left '*' '/'
%right '^'
%%
Grammar rules and actions
%%
C subroutines
```

The context free-grammar is modified to include calls to the install and context checking functions. `$n` is a variable internal to Yacc which refers to the semantic record corresponding the the nth symbol on the right hand side of a production.

```livescript
C and parser declarations
%%
...
declarations : /* empty */
             | INTEGER id_seq IDENTIFIER '.' { install( $3 ); }
;
id_seq : /* empty */
       | id_seq IDENTIFIER ','               { install( $2 ); }
;
command : SKIP
        | READ IDENTIFIER                    { context_check( $2 ); }
        | IDENT ASSGNOP exp                  { context_check( $2 ); }
        ...
;
exp : NUMBER
    | IDENT                                  { context_check( $2 ); }
    ...
;
%%
C subroutines
```

In this implementation the parse tree is implicitly annotated with the information concerning whether a variable is assigned to a value before it is referenced in an expression. The annotations to the parse tree are collected into the symbol table.

## The Scanner Modifications

The scanner must be modified to return the literal string associated each identifier (the semantic value of the token). The semantic value is returned in the global variable `yylval`. `yylval`’s type is a union made from the `%union` declaration in the parser file. The semantic value must be stored in the proper member of the union. Since the union declaration is:

```livescript
%union { char *id;
}
```

the semantic value is copied from the global variable `yytext` (which contains the input text) to `yylval.id`. Since the function `strdup` is used (from the library `string.h`) the library must be included. The resulting scanner file is:

```livescript
%{
#include <string.h> /* for strdup */
#include "Simple.tab.h" /* for token definitions and yylval */
%}
DIGIT [0-9]
ID [a-z][a-z0-9]*
%%
":="     { return(ASSGNOP); }
{DIGIT}+ { return(NUMBER); }
do       { return(DO); }
else     { return(ELSE); }
end      { return(END); }
fi       { return(FI); }
if       { return(IF); }
in       { return(IN); }
integer  { return(INTEGER); }
let      { return(LET); }
read     { return(READ); }
skip     { return(SKIP); }
then     { return(THEN); }
while    { return(WHILE); }
write    { return(WRITE); }
{ID}     { yylval.id = (char *) strdup(yytext);
           return(IDENTIFIER);}
[ \t\n]+ /* eat up whitespace */
.        { return(yytext[0]);}
%%
```

## Intermediate Representation

Most compilers convert the source code to an intermediate representation during this phase. In this example, the intermediate representation is a parse tree. The parse tree is held in the stack but it could be made explicit. Other popular choices for intermediate representation include abstract parse trees, three-address code, also known as quadruples, and post fix code. In this example we have chosen to bypass the generation of an intermediate representation and go directly to code generation. The principles illustrated in the section on code generation also apply to the generation of intermediate code.

