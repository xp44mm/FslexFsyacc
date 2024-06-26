﻿
namespace FslexFsyacc.Grammars

type GrammarRow =
    {    
    productions: Set<list<string>> /// augmented Productions
    
    symbols: Set<string>
    nonterminals: Set<string>
    terminals: Set<string>
    nullables: Set<string>

    firsts: Map<string,Set<string>>
    lasts: Map<string,Set<string>>
    
    follows: Map<string,Set<string>>
    precedes: Map<string,Set<string>>

    }

    static member from (productions:Set<list<string>>) =
        let startSymbol, mainProductions = 
            match productions.MinimumElement with
            | ["";s0] as p -> s0, Set.remove p productions
            | _ -> raise(System.ArgumentException("S'->S"))

        let symbols =
            productions
            |> Set.map Set.ofList
            |> Set.unionMany
            //|> Set.remove ""

        let nonterminals = 
            productions
            |> Set.map List.head
            |> Set.remove ""
    
        let terminals = 
            Set.difference symbols nonterminals
            |> Set.remove ""

        let nullables = 
            mainProductions
            |> NullableUtils.make 

        let firsts = FirstUtils.make nullables mainProductions
        let lasts  = LastUtils.make nullables mainProductions

        let follows  = FollowUtils.make nullables firsts productions
        let precedes = PrecedeUtils.make nullables lasts productions

        {
            productions = productions
            symbols = symbols
            nonterminals = nonterminals
            terminals = terminals
            nullables = nullables
            firsts = firsts
            lasts = lasts
            follows = follows
            precedes = precedes
        }



