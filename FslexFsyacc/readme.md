# FslexFsyacc

Runtime for Fslex/Fsyacc analyzer/parser generation tools. It includes several types to support `FslexFsyacc.Bootstrap`:

```F#
type Analyzer<'tok,'u>
    (
        nextStates: (uint32*(string*uint32) list) list, // state -> tag -> state
        rules: (uint32 list*uint32 list*('tok list -> 'u)) list
    ) =

type ParseTableApp =
    {
        tokens: Set<string>
        kernels: list<list<int*int>>
        kernelSymbols: list<string>
        actions: (string*int) list list
        rules: list<string list*(obj list->obj)>
    }

```
