namespace FslexFsyacc.Fsyacc
open System
open FslexFsyacc.Runtime.YACCs

//内嵌在*.yacc源文件的reducer中，表示yacc的返回结果。
type RawFsyaccFile = 
    {
        header:string
        inputRules:(string*((string list*string*string)list))list
        precedenceLines:(string*string list)list
        declarationLines:(string*string list)list
    }

    member this.migrate() =
        let ruleGroups: RuleGroup list =
            this.inputRules
            |> List.map(fun (h,bs) ->
                let bodies =
                    bs
                    |> List.map(fun (body,dummy,reducer) -> 
                        {
                            body = body
                            dummy = dummy
                            reducer = reducer
                        }              
                    )
                {
                    head = h
                    bodies = bodies
                }
            )
        let operatorsLines =
            this.precedenceLines
            |> List.map(fun (assoc,ls) ->
                let assoc =
                    match assoc with
                    | "left" -> LeftAssoc
                    | "right" -> RightAssoc
                    | "nonassoc" -> NonAssoc
                    | _ -> failwith assoc
                assoc,ls
            )
        {
            header = this.header
            ruleGroups = ruleGroups
            operatorsLines = operatorsLines
            declarationsLines = this.declarationLines
        }
