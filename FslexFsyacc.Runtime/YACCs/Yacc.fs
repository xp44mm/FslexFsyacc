namespace FslexFsyacc.Runtime.YACCs

open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.ItemCores
open FslexFsyacc.Runtime.BNFs

open FSharp.Idioms.Literal

type Yacc = 
    {
        productions:Set<string list> // augment productions
        dummyTokens:Map<string list,string> // production -> dummy-token
        precedences:Map<string,int*Associativity> // token:string -> precedence:int
    }

    static member just (
        productions:Set<string list>, 
        dummyTokens:Map<string list,string>, 
        precedences:Map<string,int*Associativity >
        ) =
        {
            productions = productions
            dummyTokens = dummyTokens
            precedences = precedences
        }

    member this.BNF = BNF.just this.productions

    member this.actions =
        let row = YaccRowUtils.getRow(this.productions,this.dummyTokens,this.precedences)
        row.actions
        
    member this.encodeActions =
        let row = YaccRowUtils.getRow(this.productions,this.dummyTokens,this.precedences)
        row.encodeActions

    member this.resolvedClosures =
        let row = YaccRowUtils.getRow(this.productions,this.dummyTokens,this.precedences)
        row.resolvedClosures

    member this.encodeClosures =
        let row = YaccRowUtils.getRow(this.productions,this.dummyTokens,this.precedences)
        row.encodeClosures
