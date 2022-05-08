# FslexFsyacc.Runtime

Runtime for Fslex/Fsyacc analyzer/parser generation tools. It includes several types to support `FslexFsyacc`:

```F#
type Analyzer<'tok,'u>
    (
        nextStates: (uint32*(string*uint32)[])[], // state -> tag -> state
        rules: (uint32[]*uint32[]*('tok list -> 'u))[]
    ) =

type Parser
    (
        rules: (string list*(obj[]->obj))[],
        actions: (string*int)[][],        
        closures: (int*int*string[])[][],
        getTag:'tok -> string,
        getLexeme:'tok->obj
    ) =

```
