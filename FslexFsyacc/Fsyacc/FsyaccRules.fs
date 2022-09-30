namespace FslexFsyacc.Fsyacc
open FslexFsyacc.Yacc
open FSharp.Idioms

type FsyaccRules =
    {
        rules:list<string list*string*string>
    }

    member this.eliminate(removed:string) =
        //保存名字，和行为
        let keeps =
            this.rules
            |> List.map(fun(a,b,c)-> a,(b,c))
            |> Map.ofList
                    
        let bnf:BNF =
            {
                productions =
                    this.rules
                    |> List.map Triple.first
            }

        let b1 = bnf.eliminate(removed)
        {
            rules =
                b1.productions
                |> List.map(fun prod -> 
                    let nm,act = 
                        if keeps.ContainsKey prod then 
                            keeps.[prod]
                        else "",""
                    prod,nm,act)
        
        }
