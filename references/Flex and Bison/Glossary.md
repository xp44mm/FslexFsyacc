# Glossary

## action

The C or C++ code associated with a flex pattern or a bison rule. When the pattern or rule matches an input sequence, the **action code** is executed.

## alphabet

A set of distinct symbols. For example, the ASCII character set is a collection of 128 different symbols. In a flex specification, the **alphabet** is the native character set of the computer. In a bison grammar, the **alphabet** is the set of tokens and nonterminals used in the grammar.

## ambiguity

An **ambiguous grammar** is one with more than one rule or set of rules that match the same input. In a bison grammar, ambiguous rules cause **shift/reduce** or **reduce/reduce** conflicts. The parsing mechanism that bison normally uses cannot handle ambiguous grammars. The programmer can use `%prec` declarations and bison’s own internal rules to resolve conflicts when creating a parser, or the programmer can use a GLR parser, which can handle ambiguous grammars directly.

## ASCII

American Standard Code for Information Interchange; a collection of 128 symbols representing the common symbols found in the American alphabet: lowercase and uppercase letters, digits, and punctuation, plus additional characters for formatting and control of data communication links. Most systems that run flex and bison use ASCII or extended 8-bit codes in the ISO-8859 series that include ASCII as a subset.

## bison

A program that translates a dialect of BNF into LALR(1) or GLR parsers.

## BNF

**Backus-Naur Form**; a method of representing context-free grammars. It is commonly used to specify formal grammars of programming languages. The input syntax of bison is a simplified version of **BNF**.

## compiler

A program that translates a set of instructions (a program) in one language into some other representation; typically, the output of a **compiler** is in the native binary language that can be run directly on a computer. Compare to **interpreter**.

## conflict

An error within the bison grammar in which two (or more) parsing actions are possible when parsing the same input token. There are two types of **conflicts**: **shift/reduce** and **reduce/reduce**. (See also ambiguity.)

## context-free grammar

A grammar in which each rule has a single symbol on the LHS; hence, one in which the RHS can match input regardless of what might precede or follow the material it matches. Also called a **phrase structure grammar**. **Context-sensitive grammars**, containing rules with several symbols on the LHS, are not practical for parsing computer languages.

## empty

The special case of a string with zero symbols, sometimes written as a Greek epsilon. Bison rules can match the **empty** string, but flex patterns cannot.

## finite automaton

An abstract machine that consists of a finite number of instructions (or transitions). **Finite automata** are useful in modeling many commonly occurring computer processes and have useful mathematical properties. Flex and bison create scanners and parsers based on finite automata.

## flex

A program for producing lexical analyzers, also known as **scanners**, that match patterns defined by regular expressions to a character stream.

## GLR

**Generalized Left to Right**; a powerful parsing technique that bison can optionally use. Unlike LALR(1), it can parse grammars that are ambiguous or need **indefinite lookahead** by maintaining all possible parses in parallel of the input read so far.

## grammar

A set of rules that together define a language.

## input

A stream of data read by a program. For instance, the **input to a flex scanner** is a sequence of bytes, while the **input to a bison parser** is a sequence of tokens.

## interpreter

A program that reads instructions in a language (a program) and decodes and acts on them one at a time. Compare to compiler.

## LALR(1)

**Look Ahead Left to Right**; the parsing technique that bison normally uses. The (1) denotes that the lookahead is limited to a single token.

## language

Formally, a well-defined set of strings over some alphabet; informally, some set of instructions for describing tasks that can be executed by a computer.

## left-hand side (LHS)

The **left-hand side** or **LHS** of a bison rule is the symbol that precedes the colon. During a parse, when the input matches the sequence of symbols on the RHS of the rule, that sequence is reduced to the LHS symbol.

## lex

A program that produced lexical analyzers that match patterns defined by regular expressions to a character stream. Now superseded by flex, which is more reliable and more powerful.

## lexical analyzer

A program that converts a character stream into a token stream. Flex takes a description of individual tokens as regular expressions, divides the character stream into tokens, and determines the types and values of the tokens. For example, it might turn the character stream 

```js
a = 17;
```

 into a token stream consisting of the name `a`, the operator `=`, the number `17`, and the single character token `;`. Also called a lexer or scanner.

## lookahead

Input read by a parser or scanner but not yet matched to a pattern or rule. Bison parsers have a single token of **lookahead**, while flex scanners can have indefinitely long **lookahead**.

## nonterminal

Symbols in a bison grammar that do not appear in the input but instead are defined by rules. Contrast to tokenizing.

## parser stack

In a bison parser, the symbols for partially matched rules are stored on an internal stack. Symbols are added to the stack when the parser shifts and are removed when it reduces.

## parsing

The process of taking a stream of tokens and logically grouping them into statements within some language.

## pattern

In a flex scanner, a regular expression that the scanner matches against the input.

## precedence

The order in which some particular operation is performed; for example, when interpreting mathematical statements, multiplication and division are assigned higher precedence than addition and subtraction.

Thus, the statement `3+4*5` is `23` as opposed to `35`.

## production

See **rule**.

## program

A set of instructions that perform a certain defined task.

## reduce

In a bison parser, when the input matches the list of symbols on the RHS of a rule, the parser reduces the rule by removing the RHS symbols from the parser stack and replacing them with the LHS symbol.

## reduce/reduce conflict

In a bison grammar, the situation where two or more rules match the same string of tokens. Bison resolves the conflict by reducing the rule that occurs earlier in the grammar.

## regular expression

A language for specifying patterns that match a sequence of characters. Regular expressions consist of **normal characters**, which match the same character in the input; **character classes**, which match any single character in the class; and **other characters**, which specify the way that parts of the expression are to be matched against the input.

## right-hand side (RHS)

The **right-hand side** or **RHS** of a bison rule is the list of symbols that follow the colon. During a parse, when the input matches the sequence of symbols on the RHS of the rule, that sequence is reduced to the LHS symbol.

## rule

In bison, **rules** are the abstract description of the grammar. Bison rules are also called **productions**. A rule is a single nonterminal called the LHS, a colon, and a possibly empty set of symbols called the RHS. Whenever the input matches the RHS of a rule, the parser reduces the rule.

## semantic meaning

See **value**.

## shift

A bison parser **shifts** an input symbol, placing it onto the parser stack in expectation that the symbol will match one of the rules in the grammar.

## shift/reduce conflict

In a bison grammar, the situation where a symbol completes the RHS of one rule, which the parser **needs to reduce**, and is an intermediate symbol in the RHS of other rules, for which the parser **needs to shift** the symbol. **Shift/reduce conflicts** occur either because the grammar is ambiguous or because the parser would need to look more than one token ahead to decide whether to reduce the rule that the symbol completes. Bison resolves the conflict by doing the shift.

## specification

A flex **specification** is a set of patterns to be matched against an input stream. Flex turns a specification into a scanner.

## start

The single symbol to which a bison parser reduces a valid input stream. Rules with the **start symbol** on the LHS are called **start rules**.

## start state

In a flex specification, patterns can be tagged with start states. At any point one start state is active, and patterns tagged with that start state can match the input. In an **exclusive start state**, only tagged patterns can match, while in an **inclusive state**, untagged patterns can also match the input.

## symbol

In bison terminology, **symbols** are either tokens or nonterminals. In the rules for the grammar, any name found on the right-hand side of a rule is always a symbol.

In bison terminology, tokens or terminals are the symbols provided to the parser by the scanner. Compare to a nonterminal, which is defined within the parser.

## symbol table

A data structure containing information about names occurring in the input so that all references to the same name can be related to the same object.

## tokenizing

The process of converting a stream of characters into a stream of tokens is termed **tokenizing**. A scanner tokenizes its input.

## value

Each token in a bison grammar has **both a syntactic and a semantic value**; its semantic value is the actual data contents of the token. For instance, the syntactic type of a certain operation may be `INTEGER`, but its semantic value might be `3`.

## yacc

**Yet Another Compiler Compiler**; the predecessor to bison, a program that generates a parser from a list of rules in BNF-like format.



