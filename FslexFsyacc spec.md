# FslexFsyacc Specification

## common syntax

Whitespace is significant only to separate symbols. You can add extra whitespace as you wish.

The Comments are either lines starting with `//` or blocks enclosed by `(*` and `*)`.

The `%%`, `%{` and `%}` are punctuation that appears in every fslex/fsyacc grammar file to separate the sections. It is recommended that the  `%%`, `%{` and `%}` limiters be on a separate line.

The first section is on required chunk of F# code that is bounded by `%{` and `%}` limiters. This code is there to define utility functions used by later snippets of F# code and to set up the environment by opening useful modules.

## Fsyacc specification

```fsyacc
%{
F# nested code
%}

rules

%%

prec

%%

type declarations

```

## Fslex specification

```fslex
%{
F# nested code
%}

definitions

%%

rules

```
