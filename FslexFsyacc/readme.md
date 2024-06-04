# FslexFsyacc.Runtime

Runtime for Fslex/Fsyacc analyzer/parser generation tools. It includes several types to support `FslexFsyacc`:

```F#
type Analyzer<'tok,'u>
    (
        nextStates: (uint32*(string*uint32) list) list, // state -> tag -> state
        rules: (uint32 list*uint32 list*('tok list -> 'u)) list
    ) =

type Parser
    (
        rules: (string list*(obj list->obj)) list,
        actions: (string*int) list list,
        closures: (int*int*string list) list list,
        getTag:'tok -> string,
        getLexeme:'tok->obj
    ) =

```
