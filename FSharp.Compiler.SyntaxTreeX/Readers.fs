namespace FSharp.Compiler.SyntaxTreeX.Readers

open FSharp.Compiler.SyntaxTreeX

module ModuleOrNamespace =
    let getDecls(this:XModuleOrNamespace) =
        match this with
        | XModuleOrNamespace(
            longId: LongIdent,
            modifiers: string list ,
            kind,
            decls: XModuleDecl list ,
            attribs: XAttributes
            ) -> decls

module XModuleDecl =
    let getLetBindings(src:XModuleDecl) =
        match src with
        | XModuleDecl.Let(_,bindings) ->
            bindings
        | _ -> failwith $"{src}"

    let getExpr(src:XModuleDecl) =
        match src with
        | XModuleDecl.Expr(expr) ->
            expr
        | _ -> failwith $"{src}"