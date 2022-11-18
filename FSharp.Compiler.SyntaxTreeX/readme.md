`FSharp.Compiler.SyntaxTreeX` is a subtree of `FSharp.Compiler.SyntaxTree`. You can use it to determine whether two pieces of code are equal.
`FSharp.Compiler.SyntaxTreeX` removes the xmldoc and range information from `FSharp.Compiler.SyntaxTree`.

when anonymous module:

```Fsharp
let decls:XModuleDecl list = 
    Parser.getDecls("header.fsx",text)
```

when named module:

```Fsharp
let decls:XModuleDecl list = 
    Parser.getDecls("header.fs",text)
```
