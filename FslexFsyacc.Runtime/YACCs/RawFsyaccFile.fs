namespace FslexFsyacc.Runtime.YACCs
open System

type RuleBody =
    {
        body:string list
        dummy:string
        reducer:string
    }

type RuleGroup =
    {
        head: string
        bodies: RuleBody list
    }

//内嵌在*.yacc源文件的reducer中，表示yacc的返回结果。
type RawFsyaccFile = 
    {
        header:string
        ruleGroups: RuleGroup list
        operatorsLines:(Associativity*string list)list
        declarationsLines:(string*string list)list
    }
