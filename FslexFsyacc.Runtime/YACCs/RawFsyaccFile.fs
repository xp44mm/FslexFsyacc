﻿namespace FslexFsyacc.Runtime.YACCs
open System

//内嵌在*.yacc源文件的reducer中，表示yacc的返回结果。
type RawFsyaccFile = 
    {
        header: string
        ruleGroups: RuleGroup list
        operatorsLines: list<Associativity*string list>
        declarationsLines: list<string*string list>
    }
