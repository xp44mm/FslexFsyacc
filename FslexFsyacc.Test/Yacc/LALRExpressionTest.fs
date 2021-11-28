namespace FslexFsyacc.Yacc

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit

type LALRExpressionTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Literal.stringify
        |> output.WriteLine

    ///表达式文法(4.1)
    let E = "E"
    let T = "T"
    let F = "F"
    let id = "id"

    let mainProductions = [
        [ E; E; "+"; T ]
        [ E; T ]
        [ T; T; "*"; F ]
        [ T; F ]
        [ F; "("; E; ")" ]
        [ F; id ]
    ]

    [<Fact>]
    member this.``fig4-38: parsing table yacc test``() =

        let precedences =
            [
                RightAssoc, [ProductionKey mainProductions.[5]]
                LeftAssoc , [TerminalKey "*";TerminalKey "/"]
                LeftAssoc , [TerminalKey "+";TerminalKey "-"]
            ]

        let yacc = ParseTable.create(mainProductions,precedences)

        //show yacc.originalTable
        //show yacc.encodeTable
        //let y = set [
        //    0,"(",6;
        //    0,"E",1;
        //    0,"F",9;
        //    0,"T",5;
        //    0,"id",8;
        //    1,"",0;1,"+",3;2,")",7;2,"+",3;3,"(",6;3,"F",9;3,"T",4;3,"id",8;4,"",-1;4,")",-1;4,"*",10;4,"+",-1;5,"",-2;5,")",-2;5,"*",10;5,"+",-2;6,"(",6;6,"E",2;6,"F",9;6,"T",5;6,"id",8;7,"",-3;7,")",-3;7,"*",-3;7,"+",-3;8,"",-4;8,")",-4;8,"*",-4;8,"+",-4;9,"",-5;9,")",-5;9,"*",-5;9,"+",-5;10,"(",6;10,"F",11;10,"id",8;11,"",-6;11,")",-6;11,"*",-6;11,"+",-6]
        
        //Should.equal y yacc.parsingTable
        ()