module Interpolation.Tokenizer
open Interpolation.SourceText

open System.Collections
open System.Text.RegularExpressions

open FSharp.Idioms
open FSharp.Idioms.StringOps
open FslexFsyacc.Runtime

let tokenize (index:int) (inp:string) =
    let rec loop (i:int) (x:string) =
        seq {
            match x with
            | "" -> ()

            | On (tryWhiteSpace) (raw,rest) ->
                yield { 
                    index = i
                    length = raw.Length
                    value = WhiteSpace raw
                    }
                yield! loop (i+raw.Length) rest

            | On (tryLineTerminatorSequence) (raw,rest) ->
                yield { 
                    index = i
                    length = raw.Length
                    value = LineTerminatorSequence raw
                    }
                yield! loop (i+raw.Length) rest

            | On (trySingleLineComment) (raw,rest) ->
                yield { 
                    index = i
                    length = raw.Length
                    value = SingleLineComment raw
                    }
                yield! loop (i+raw.Length) rest

            | On (tryMultiLineComment) (raw,rest) ->
                yield { 
                    index = i
                    length = raw.Length
                    value = MultiLineComment raw
                    }
                yield! loop (i+raw.Length) rest

            | On (tryIdentifierName) (raw,rest) ->
                yield { 
                    index = i
                    length = raw.Length
                    value = IdentifierName raw
                    }
                yield! loop (i+raw.Length) rest

            | On (tryLongestPrefix Punctuators) (raw,rest) ->
                yield { 
                    index = i
                    length = raw.Length
                    value = Punctuator raw
                    }
                yield! loop (i+raw.Length) rest

            | On (tryNumericLiteral) (raw,rest) ->
                yield { 
                    index = i
                    length = raw.Length
                    value = NumericLiteral raw
                    }
                yield! loop (i+raw.Length) rest

            | On (trySingleStringLiteral) (raw,rest) ->
                yield { 
                    index = i
                    length = raw.Length
                    value = SingleStringLiteral raw
                    }
                yield! loop (i+raw.Length) rest

            | On (tryDoubleStringLiteral) (raw,rest) ->
                yield { 
                    index = i
                    length = raw.Length
                    value = DoubleStringLiteral raw
                    }
                yield! loop (i+raw.Length) rest

            | _ -> failwith $"tokenize:{x}"
                
        }
    loop 0 inp