# FslexFsyacc.Runtime

Runtime for Fslex/Fsyacc analyzer/parser generation tools. It includes several types to support `FslexFsyacc`:

```F#
Analyzer(nextStates, lexemesFromFinal, universalFinals, finalMappers)

type Parser
    (
        productions: (string list)[],
        closures   : (int*int*string[])[][],
        actions    : (string*int)[][],
        mappers    : (obj[]->obj)[]
    )

```
