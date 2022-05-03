namespace Interpolation

open FSharp.Idioms
open FSharp.Idioms.StringOps
open System.Text.RegularExpressions

type TemplateToken =
    | TemplateChars of string
    | Backtick
    | Dollar_Brace
    | Rest of string

    static member tokenize (inp:string) =
        let rec loop x =
            seq {
                match x with
                | "" -> failwith inp

                | On (tryFirst '\\') y ->
                    match y with
                    | "" -> failwith inp
                    | _ -> 
                        yield TemplateChars x.[0..1]
                        yield! loop y.[1..]

                | On (tryFirst '`') y ->
                    yield Backtick
                    yield Rest y

                | On (tryStart "${") y ->
                    yield Dollar_Brace
                    yield Rest y

                | _ ->
                    yield TemplateChars x.[0..0]
                    yield! loop x.[1..]

            }
        loop inp