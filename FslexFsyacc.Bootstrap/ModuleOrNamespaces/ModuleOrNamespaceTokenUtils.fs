module FslexFsyacc.ModuleOrNamespaces.ModuleOrNamespaceTokenUtils

//open FslexFsyacc.TypeArguments
open FslexFsyacc
open FslexFsyacc.SourceTextTry

open FSharp.Idioms

open FSharp.Idioms.ActivePatterns
open FSharp.Idioms.StringOps
open FSharp.Idioms.Literal

open System
open System.Text.RegularExpressions

let getTag (token:PositionWith<ModuleOrNamespaceToken>) =
    token.value.getTag()

let getLexeme (token:PositionWith<ModuleOrNamespaceToken>) =
    token.value.getLexeme()

let tokenize (exit:string->bool) (sourceText:SourceText) = // (offset:int) (input:string)
    let rec loop backs (src:SourceText) =
        match backs with
        | { value = tok } :: _ when tok = EQUALS -> seq {
            let targ, length = 
                let exit (rest:string) = 
                    exit rest || 
                    Regex.IsMatch(rest, @"^ *[\r\n]")

                TypeArguments.TypeArgumentCompiler.compile exit src

            
            let tok = PositionWith<_>.just(src.index, length, TARG targ)

            yield tok
            let src = src.skip length
            yield! loop (tok::backs) src
            }

        | { value = tok1 } :: {value = tok2 } :: _ when tok1 = TYPE && tok2 = OPEN -> seq {
            let targ, length = 
                let exit (rest:string) = 
                    exit rest || 
                    Regex.IsMatch(rest, @"^ *[\r\n]")
                TypeArguments.TypeArgumentCompiler.compile exit src

            let tok = PositionWith<_>.just(src.index, length, TARG targ)

            yield tok
            let src = src.skip length
            yield! loop (tok::backs) src
            }

        | _ -> seq {
            match src.text with
            | "" -> ()

            | Rgx @"^\s+" m ->
                let src = src.skip m.Length
                yield! loop backs src

            | On tryFSharpIdent x ->
                let tok =
                    {
                        index = src.index
                        length = x.Length
                        value =
                            match x.Value with
                            | "open" -> OPEN
                            | "type" -> TYPE
                            | id -> IDENT id
                    }
                yield tok
                let src = src.skip tok.length
                yield! loop (tok::backs) src

            | On tryFSharpTypar x ->
                let tok = {
                    index = src.index
                    length = x.Length
                    value = TYPAR x.Value
                }
                yield tok
                let src = src.skip tok.length
                yield! loop (tok::backs) src

            | First '.' _ ->
                let tok =
                    {
                        index = src.index
                        length = 1
                        value = DOT
                    }
                yield tok
                let src = src.skip tok.length
                yield! loop (tok::backs) src

            | First '=' _ ->
                let tok =
                    {
                        index = src.index
                        length = 1
                        value = EQUALS
                    }
                yield tok
                let src = src.skip tok.length
                yield! loop (tok::backs) src

            | First '<' _ ->
                let tok =
                    {
                        index = src.index
                        length = 1
                        value = LANGLE
                    }
                yield tok
                let src = src.skip tok.length
                yield! loop (tok::backs) src

            | First '>' _ ->
                let tok =
                    {
                        index = src.index
                        length = 1
                        value = RANGLE
                    }
                yield tok
                let src = src.skip tok.length
                yield! loop (tok::backs) src

            | First ',' _ ->
                let tok =
                    {
                        index = src.index
                        length = 1
                        value = COMMA
                    }
                yield tok
                let src = src.skip tok.length
                yield! loop (tok::backs) src

            | _ -> 
                failwith $"unimpl tokenize case{stringify src}"
            }
    loop [] sourceText
