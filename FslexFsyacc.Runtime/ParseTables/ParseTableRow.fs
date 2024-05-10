namespace FslexFsyacc.Runtime.ParseTables

open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.BNFs
open FslexFsyacc.Runtime.ItemCores

open FSharp.Idioms
open FSharp.Idioms.Literal

type ParseTableRow = 
    {
    bnf: BNF
    dummyTokens:Map<string list,string>
    precedences:Map<string,int>
    //unambiguousItemCores: Map<Set<ItemCore>,Map<string,Set<ItemCore>>>
    actions: Map<Set<ItemCore>,Map<string,Action>>
    //resolvedClosures: Map<int,Map<ItemCore,Set<string>>>
    encodeActions: list<list<string*int>>
    //encodedClosures: list<list<int*int*string list>>
    }

    static member from (bnf:BNF) (dummyTokens:Map<string list,string>) (precedences:Map<string,int>) =
        let tryGetDummy = Precedence.tryGetDummy dummyTokens bnf.grammar.terminals

        let tryGetPrecedenceCode = Precedence.tryGetPrecedenceCode tryGetDummy precedences

        let actions =
            bnf.actions
            |> Map.map(fun src mp ->
                mp
                |> Seq.choose(fun(KeyValue(sym, acts)) ->
                    acts
                    |> Action.disambiguate tryGetPrecedenceCode
                    |> Option.map (Pair.prepend sym)
                )
                |> Map.ofSeq
            )
            |> Map.filter( fun src mp -> not mp.IsEmpty )

        let encoder =
            {
                productions =
                    ParseTableEncoder.getProductions bnf.grammar.productions
                kernels = bnf.kernels |> Seq.mapi(fun i k -> k,i) |> Map.ofSeq
            } : ParseTableEncoder

        let encodeActions = encoder.getEncodedActions actions

        {
        bnf                  = bnf
        dummyTokens          = dummyTokens
        precedences          = precedences
        //unambiguousItemCores = unambiguousItemCores
        actions              = actions             
        //resolvedClosures: Map<int,Map<ItemCore,Set<string>>>
        encodeActions = encodeActions
        //encodedClosures: list<list<int*int*string list>>
        }
