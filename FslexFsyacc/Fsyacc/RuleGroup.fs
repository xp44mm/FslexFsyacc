namespace FslexFsyacc.Fsyacc
open System


type RuleGroup =
    {
        lhs: string // left hand side
        bodies: RuleBody list
    }

