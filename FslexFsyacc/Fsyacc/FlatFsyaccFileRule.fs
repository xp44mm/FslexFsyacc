﻿module FslexFsyacc.Fsyacc.FlatFsyaccFileRule

open FslexFsyacc.Yacc
open FSharp.Idioms

open System

let filterDummyProductions (rules:list<string list*string*string>) =
    rules
    |> List.filter(fun (prod,dummy,act) -> dummy > "" )
    |> List.map(Triple.firstTwo)
