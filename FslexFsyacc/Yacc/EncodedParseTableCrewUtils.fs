module FslexFsyacc.Yacc.EncodedParseTableCrewUtils

open FSharp.Idioms
open FslexFsyacc.Runtime.ItemCores
//open FslexFsyacc.Runtime.ParseTables
open FslexFsyacc.Runtime.BNFs

let fromActionParseTableCrew (tbl:ActionParseTableCrew) =
    let encoder =
        {
            productions =
                ParseTableEncoder.getProductions tbl.augmentedProductions
            kernels = tbl.kernels |> Seq.mapi(fun i k -> k,i) |> Map.ofSeq
        } : ParseTableEncoder

    //let encoder  = encoder
    let encodedActions = encoder.getEncodedActions tbl.actions
    let encodedClosures = encoder.getEncodedClosures tbl.resolvedClosures
    EncodedParseTableCrew(tbl,encodedActions,encodedClosures)

let getEncodedParseTableCrew(
    mainProductions:string list list,
    dummyTokens:Map<string list,string>, // %prec dummy-token
    precedences:Map<string,int>
    ) =

    // 动作无歧义的表
    let tbl =
        ActionParseTableCrewUtils.getActionParseTableCrew
            mainProductions
            dummyTokens
            precedences
    fromActionParseTableCrew tbl
