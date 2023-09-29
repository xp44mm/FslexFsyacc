namespace FslexFsyacc.Yacc

open FSharp.Idioms
open System

[<Obsolete("FollowPrecedeCrew")>]
type Grammar =
    {
        /// 增广语法：开始符号为""空字符串
        productions: Set<string list>
    }

    //从文法生成增广文法
    static member from(mainProductions:string list list) =
        let startSymbol = mainProductions.[0].[0]
        let mainProductions = set mainProductions
        let productions =
                mainProductions
                |> Set.add ["";startSymbol]

        GrammarMemoiz.mainProductions.TryAdd(productions,mainProductions) |> ignore

        {
            productions = productions
        }

    //不包括增广规则的主体规则
    member this.mainProductions =
        let valueFactory productions = 
            productions
            |> Set.remove(this.productions.MinimumElement)
        GrammarMemoiz.mainProductions.GetOrAdd(this.productions, valueFactory)

    /// 文法的所有符号。注意：不包括增广开始符号
    member this.symbols =
        GrammarMemoiz.symbols.GetOrAdd(
            this.productions,
            Func<_,_>(fun _ -> this.mainProductions |> List.concat |> Set.ofList)
            )

    /// 文法的所有非终结符号。注意：不包括增广开始符号
    member this.nonterminals = 
        GrammarMemoiz.nonterminals.GetOrAdd(
            this.productions, 
            Func<_,_>(fun _ -> Set.map List.head this.mainProductions)
            )
    
    member this.terminals = this.symbols - this.nonterminals

    member this.nullables =
        GrammarMemoiz.nullables.GetOrAdd(
            this.productions, 
            Func<_,_>(fun _ -> NullableFactory.make this.mainProductions)
            )

    /// 符号串的first终结符集合
    member this.firsts =
        GrammarMemoiz.firsts.GetOrAdd(
            this.productions, 
            Func<_,_>(fun _ -> FirstFactory.make this.nullables this.mainProductions)
            )

    member this.lasts =
        GrammarMemoiz.lasts.GetOrAdd(
            this.productions, 
            Func<_,_>(fun _ -> LastFactory.make this.nullables this.mainProductions)
            )

    member this.follows =
        GrammarMemoiz.follows.GetOrAdd(
            this.productions, 
            Func<_,_>(FollowFactory.make this.nullables this.firsts)
            )

    member this.precedes =
        GrammarMemoiz.precedes.GetOrAdd(
            this.productions, 
            Func<_,_>(PrecedeFactory.make this.nullables this.lasts)
            )
