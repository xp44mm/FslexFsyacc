namespace FslexFsyacc.Fsyacc
open System
open FslexFsyacc.Precedences
open FslexFsyacc.TypeArguments

//内嵌在*.yacc源文件的reducer中，表示yacc的返回结果。
type RawFsyaccFile = 
    {
        header: string
        ruleGroups: RuleGroup list
        operatorsLines: list<Associativity*string list>
        declarationsLines: list<TypeArgument*string list>
    }
