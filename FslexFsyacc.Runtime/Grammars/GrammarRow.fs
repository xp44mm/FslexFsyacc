
namespace FslexFsyacc.Runtime.Grammars

type GrammarRow =
    {    
    productions: Set<list<string>> /// augmented Productions
    
//type NullableCrew = {
    symbols: Set<string>
    nonterminals: Set<string>
    terminals: Set<string>
    nullables: Set<string>

//type FirstLastCrew = {
    firsts: Map<string,Set<string>>
    lasts: Map<string,Set<string>>
    
//type FollowPrecedeCrew = {
    follows: Map<string,Set<string>>
    precedes: Map<string,Set<string>>

    }

    static member from (productions:Set<list<string>>) =
        let startSymbol, mainProductions = 
            match productions.MinimumElement with
            | ["";s0] as p -> s0, Set.remove p productions
            | _ -> raise(System.ArgumentException("S'->S"))

        //带扩展开始符号
        let symbols =
            productions
            |> Set.map Set.ofList
            |> Set.unionMany
            //|> Set.remove ""

        //不带扩展开始符号
        let nonterminals = 
            productions
            |> Set.map List.head
            |> Set.remove ""
    
        //不带扩展开始符号
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



