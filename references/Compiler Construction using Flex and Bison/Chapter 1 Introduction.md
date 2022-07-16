# Chapter 1 Introduction

A language translator is a program which translates programs written in a source language into an equivalent program in an object language. The source language is usually a high-level programming language and the object language is usually the machine language of an actual computer. From the pragmatic point of view, the translator defines the semantics of the programming language, it transforms operations specified by the syntax into operations of the computational model to some real or virtual machine. This chapter shows how context-free grammars are used in the construction of language translators. Since the translation is guided by the syntax of the source language, the translation is said to be *syntax-directed*.

A *compiler* is a translator whose source language is a high-level language and whose object language is close to the machine language of an actual computer. The typical compiler consists of several phases each of which passes its output to the next phase

- The *lexical phase* (scanner) groups characters into lexical units or tokens. The input to the lexical phase is a character stream. The output is a stream of tokens. Regular expressions are used to define the tokens recognized by a scanner (or lexical analyzer). The scanner is implemented as a finite state machine.

  Lex and Flex are tools for generating scanners: programs which recognize lexical patterns in text. ~~Flex is a faster version of Lex. In this chapter Lex/Flex refers to either of the tools. The appendix on Lex/Flex is a condensation of the manual page "flexdoc" by Vern Paxon.~~

- The *parser* groups tokens into syntactical units. The output of the parser is a parse tree representation of the program. Context-free grammars are used to define the program structure recognized by a parser. The parser is implemented as a push-down automata.

  Yacc and Bison are tools for generating parsers: programs which recognize the grammatical structure of programs. ~~Bison is a faster version of Yacc. In this chapter, Yacc/Bison refers to either of these tools. The sections on Yacc/Bison are a condensation and extension of the document “BISON the Yacc-compatible Parser Generator” by Charles Donnelly and Richard Stallman.~~

- The *semantic analysis phase* analyzes the parse tree for context-sensitive information often called the *static semantics*. The output of the semantic analysis phase is an annotated parse tree. Attribute grammars are used to describe the static semantics of a program.

  This phase is often combined with the parser. During the parse, information concerning variables and other objects is stored in a *symbol table*. The information is utilized to perform the context-sensitive checking.

- The *optimizer* applies semantics preserving transformations to the annotated parse tree to simplify the structure of the tree and to facilitate the generation of more efficient code.

- The *code generator* transforms the simplified annotated parse tree into object code using rules which denote the semantics of the source language. The code generator may be integrated with the parser.

- The *peep-hole* optimizer examines the object code, a few instructions at a time, and attempts to do machine dependent code improvements.



In contrast with compilers an *interpreter* is a program which simulates the execution of programs written in a source language. Interpreters may be used either at the source program level or an interpreter may be used it interpret an object code for an idealized machine. This is the case when a compiler generates code for an idealized machine whose architecture more closely resembles the source code.

There are several other types of translators that are often used in conjunction with a compiler to facilitate the execution of programs. An *assembler* is a translator whose source language (an assembly language) represents a one-to-one transliteration of the object machine code. Some compilers generate assembly code which is then assembled into machine code by an assembler. A *loader* is a translator whose source and object languages are machine language. The source language programs contain tables of data specifying points in the program which must be modified if the program is to be executed. A *link editor* takes collections of executable programs and links them together for actual execution. A *preprocessor* is a translator whose source language is an extended form of some high-level language and whose object language is the standard form of the high-level language.



For illustration purposes, we will construct a compiler for a simple imperative programming language called `Simple`. The context-free grammar for Simple is given in Figure 1.1 where the non-terminal symbols are given in all lower case, the terminal symbols are given in all caps or as literal symbols and, where the literal symbols conflict with the meta symbols of the EBNF, they are enclosed with single quotes. The start symbol is `program`. While the grammar uses upper-case to high-light terminal symbols, they are to be implemented in lower case.

```livescript
program          ::= LET [ declarations ] IN command sequence END
declarations     ::= INTEGER [ id_seq ] IDENTIFIER .
id_seq           ::= id_seq... IDENTIFIER ,
command_sequence ::= command... command
command ::= SKIP ;
          | IDENTIFIER := expression ;
          | IF exp THEN command_sequence ELSE command_sequence FI ;
          | WHILE exp DO command_sequence END ;
          | READ IDENTIFIER ;
          | WRITE expression ;
expression ::= NUMBER | IDENTIFIER | '(' expression ')'
             | expression + expression | expression − expression
             | expression * expression | expression / expression
             | expression ˆ expression
             | expression = expression
             | expression < expression
             | expression > expression
```

Figure 1.1: Simple

There are two context sensitive requirements; variables must be declared before they are referenced and a variable may be declared only once.

