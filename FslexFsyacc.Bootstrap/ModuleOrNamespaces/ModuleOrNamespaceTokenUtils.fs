module FslexFsyacc.ModuleOrNamespaces.ModuleOrNamespaceTokenUtils

//open FslexFsyacc.TypeArguments
open FslexFsyacc
open FslexFsyacc.SourceText

open FSharp.Idioms

open FSharp.Idioms.ActivePatterns
open FSharp.Idioms.StringOps
open FSharp.Idioms.Literal

open System
open System.Text.RegularExpressions

let getTag (token:Position<ModuleOrNamespaceToken>) =
    token.value.getTag()

let getLexeme (token:Position<ModuleOrNamespaceToken>) =
    token.value.getLexeme()

let tokenize (exit:string->bool) (offset:int) (input:string) =
    let rec loop backs pos rest =
        match backs with
        | { value = tok } :: _ when tok = TYPE -> seq {
            let targ, epos, erest = 
                let exit (rest:string) = 
                    exit rest || 
                    Regex.IsMatch(rest, @"^ *[\r\n]")
                TypeArguments.TypeArgumentCompiler.compile exit pos rest

            let tta = Position.from(pos, epos-pos+1, TARG targ)

            yield tta
            yield! loop [tta] epos erest
            }
        | _ -> seq {
            match rest with
            | "" -> ()

            | Rgx @"^\s+" m ->
                yield! loop backs (pos+m.Length) rest.[m.Length..]

            | On tryFSharpIdent x ->
                let tok =
                    {
                        index = pos
                        length = x.Length
                        value =
                            match x.Value with
                            | "open" -> OPEN
                            | "type" -> TYPE
                            | id -> IDENT id
                    }
                yield tok
                yield! loop [tok] tok.nextIndex rest.[tok.length..]

            | First '.' _ ->
                let tok =
                    {
                        index = pos
                        length = 1
                        value = DOT
                    }
                yield tok
                yield! loop [tok] tok.nextIndex rest.[tok.length..]

            | rest -> 
                failwith $"unimpl tokenize case{stringify(pos,rest)}"

            }

    loop [] offset input
