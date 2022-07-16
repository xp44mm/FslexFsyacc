# Chapter 7 Code Generation

As the source program is processed, it is converted to an internal form. The internal representation in the example is that of an implicit parse tree. Other internal forms may be used which resemble assembly code. The internal form is translated by the code generator into object code. Typically, the object code is a program for a virtual machine. The virtual machine chosen for Simple consists of three segments. A data segment, a code segment and an expression stack.

The data segment contains the values associated with the variables. Each variable is assigned to a location which holds the associated value. Thus, part of the activity of code generation is to associate an address with each variable. The code segment consists of a sequence of operations. Program constants are incorporated in the code segment since their values do not change. The expression stack is a stack which is used to hold intermediate values in the evaluation of expressions. The presence of the expression stack indicates that the virtual machine for Simple is a “stack machine”.

## Declaration translation

Declarations define an environment. To reserve space for the data values, the DATA instruction is used.

```
integer x,y,z. DATA 2
```

## Statement translation

The assignment, if, while, read and write statements are translated as follows:

```
x := expr    code for expr
             STORE X
             
if cond then code for cond
   S1        BR_FALSE L1
else         code for S1
   S2        BR L2
end          L1: code for S2
             L2:
             
while cond do L1: code for cond
    S             BR_FALSE L2
end               code for S
                  BR L1
               L2:
               
read X         IN_NUM X
write expr     code for expr
               OUT_NUM
```

If the code is placed in an array, then the label addresses must be *back-patched* into the code when they become available.

## Expression translation

Expressions are evaluated on an expression stack. Expressions are translated as follows:

```C
constant        LD_NUM constant
    
variable        LD variable
    
e1 op e2        code for e1
                code for e2
                code for op
```

## The Code Generator Module

The data segment begins with an offset of zero and space is reserved, in the data segment, by calling the function `data_location` which returns the address of the reserved location.

```C
int data_offset = 0;
int data_location() { return data_offset++; }
```

The code segment begins with an offset of zero. Space is reserved, in the code segment, by calling the function `reserve_loc` which returns the address of the reserved location. The function `gen_label` returns the value of the code offset.

```C
int code_offset = 0;
int reserve_loc()
{
return code_offset++;
}
int gen_label()
{
return code_offset;
}
```

The functions `reserve_loc` and `gen_label` are used for backpatching code.

The functions `gen_code` and `back_patch` are used to generate code. `gen_code` generates code at the current offset while `back_patch` is used to generate code at some previously reserved address.

```C
void gen_code( enum code_ops operation, int arg )
{ code[code_offset].op = operation;
  code[code_offset++].arg = arg;
}
void back_patch( int addr, enum code_ops operation, int arg )
{
    code[addr].op = operation;
    code[addr].arg = arg;
}
```

## The Symbol Table Modifications

The symbol table record is extended to contain the offset from the base address of the data segment (the storage area which is to contain the values associated with each variable) and the `putsym` function is extended to place the offset into the record associated with the variable.

```C
struct symrec
{
  char *name; /* name of symbol */
  int offset; /* data offset */
  struct symrec *next; /* link field */
};
//...
symrec * putsym (char *sym_name)
{
  symrec *ptr;
  ptr = (symrec *) malloc (sizeof(symrec));
    
  ptr->name = (char *) malloc (strlen(sym_name)+1);
  strcpy (ptr->name,sym_name);
    
  ptr->offset = data_location();
    
  ptr->next = (struct symrec *)sym_table;
  sym_table = ptr;
  return ptr;
}
//...
```

## The Parser Modifications

As an example of code generation, we extend our Lex and Yacc files for Simple to generate code for a stack machine. First, we must extend the Yacc and Lex files to pass the values of constants from the scanner to the parser. The definition of the semantic record in the Yacc file is modified that the constant may be returned as part of the semantic record. and to hold two label identifiers since two labels will be required for the `if` and `while` commands. The token type of `IF` and `WHILE` is `<lbls>` to provide label storage for backpatching. The function `newlblrec` generates the space to hold the labels used in generating code for the If and While statements. The `context_check` routine is extended to generate code.

```C
%{#include <stdio.h> /* For I/O */
#include <stdlib.h> /* For malloc here and in symbol table */
#include <string.h> /* For strcmp in symbol table */
#include "ST.h" /* Symbol Table */
#include "SM.h" /* Stack Machine */
#include "CG.h" /* Code Generator */
#define YYDEBUG 1 /* For Debugging */
int errors; /* Error Count-incremented in CG, ckd here */
struct lbs /* For labels: if and while */
{
int for_goto;
int for_jmp_false;
};
struct lbs * newlblrec() /* Allocate space for the labels */
{
return (struct lbs *) malloc(sizeof(struct lbs));
}
install ( char *sym_name )
{
symrec *s;
s = getsym (sym_name);
if (s == 0)
s = putsym (sym_name);
else { errors++;
printf( "%s is already defined\n", sym_name );
}
}
context_check( enum code_ops operation, char *sym_name )
{ symrec *identifier;
identifier = getsym( sym_name );
if ( identifier == 0 )
{ errors++;
printf( "%s", sym_name );
printf( "%s\n", " is an undeclared identifier" );
}
else gen_code( operation, identifier->offset );
}
%}
%union semrec /* The Semantic Records */
{
int intval; /* Integer values */
char *id; /* Identifiers */
struct lbs *lbls /* For backpatching */
}
%start program
%token <intval> NUMBER /* Simple integer */
%token <id> IDENTIFIER /* Simple identifier */
%token <lbls> IF WHILE /* For backpatching labels */
%token SKIP THEN ELSE FI DO END
%token INTEGER READ WRITE LET IN
%token ASSGNOP
%left ’-’ ’+’
%left ’*’ ’/’
%right ’^’
%%
/* Grammar Rules and Actions */
%%
/* C subroutines */
```

The parser is extended to generate and assembly code. The code implementing the `if` and `while` commands must contain the correct jump addresses. In this example, the jump destinations are labels. Since the destinations are not known until the entire command is processed, *back-patching* of the destination information is required. In this example, the label identifier is generated when it is known that an address is required. The label is placed into the code when its position is known. An alternative solution is to store the code in an array and back-patch actual addresses.

The actions associated with code generation for a stack-machine based architecture are added to the grammar section. The code generated for the declaration section must reserve space for the variables.

```C
/* C and Parser declarations */
%%
program : LET
             declarations
          IN { gen_code( DATA, sym_table->offset ); }
             commands
          END { gen_code( HALT, 0 ); YYACCEPT; }
;
declarations : /* empty */
    | INTEGER id_seq IDENTIFIER ’.’ { install( $3 ); }
;
id_seq : /* empty */
    | id_seq IDENTIFIER ’,’ { install( $2 ); }
;
```

The `IF` and `WHILE` commands require backpatching.

```C
commands : /* empty */
| commands command ’;’
;
command : SKIP
| READ IDENTIFIER { context_check( READ_NUM, $2 ); }
| WRITE exp { gen_code( WRITE_NUM, 0 ); }
| IDENTIFIER ASSGNOP exp { context_check( STORE, $1 ); }
| IF exp { $1 = (struct lbs *) newlblrec();
$1->for_jmp_false = reserve_loc(); }
THEN commands { $1->for_goto = reserve_loc(); }
ELSE { back_patch( $1->for_jmp_false,
JMP_FALSE,
gen_label() ); }
commands
FI { back_patch( $1->for_goto, GOTO, gen_label() ); }
| WHILE { $1 = (struct lbs *) newlblrec();
$1->for_goto = gen_label(); }
exp { $1->for_jmp_false = reserve_loc(); }
DO
commands
END { gen_code( GOTO, $1->for_goto );
back_patch( $1->for_jmp_false,
JMP_FALSE,
gen_label() ); }
;
```

The code generated for expressions is straight forward.

```C
exp : NUMBER { gen_code( LD_NUM, $1 ); }
| IDENTIFIER { context_check( LD_VAR, $1 ); }
| exp ’<’ exp { gen_code( LT, 0 ); }
| exp ’=’ exp { gen_code( EQ, 0 ); }
| exp ’>’ exp { gen_code( GT, 0 ); }
| exp ’+’ exp { gen_code( ADD, 0 ); }
| exp ’-’ exp { gen_code( SUB, 0 ); }
| exp ’*’ exp { gen_code( MULT, 0 ); }
| exp ’/’ exp { gen_code( DIV, 0 ); }
| exp ’^’ exp { gen_code( PWR, 0 ); }
| ’(’ exp ’)’
;
%%
/* C subroutines */
```

## The Scanner Modifications

Then the Lex file is extended to place the value of the constant into the semantic record.

```C
%{
#include <string.h> /* for strdup */
#include "simple.tab.h" /* for token definitions and yylval */
%}
DIGIT [0-9]
ID [a-z][a-z0-9]*
%%
{DIGIT}+ { yylval.intval = atoi( yytext );
           return(NUMBER); }
...
{ID}     { yylval.id = (char *) strdup(yytext);
           return(IDENT); }
[ \t\n]+ /* eat up whitespace */
.        { return(yytext[0]); }
%%
```

## An Example

To illustrate the code generation capabilities of the compiler, Figure 7.1 is a program in **Simple** and Figure 7.2.

```F#
let
integer n,x,n.
in
read n;
if n < 10 then x := 1; else skip; fi;
while n < 10 do x := 5*x; n := n+1; end;
skip;
write n;
write x;
end
```

Figure 7.1: A Simple program

```
0: data 1
1: in_int 0
2: ld_var 0
3: ld_int 10
4: lt 0
5: jmp_false 9
6: ld_int 1
7: store 1
8: goto 9
9: ld_var 0
10: ld_int 10
11: lt 0
12: jmp_false 22
13: ld_int 5
14: ld_var 1
15: mult 0
16: store 1
17: ld_var 0
18: ld_int 1
19: add 0
20: store 0
21: goto 9
22: ld_var 0
23: out_int 0
24: ld_var 1
25: out_int 0
26: halt 0
```

Figure 7.2: Stack code