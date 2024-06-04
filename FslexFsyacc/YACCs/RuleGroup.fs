namespace FslexFsyacc.Runtime.YACCs
open System


type RuleGroup =
    {
        lhs: string // left hand side
        bodies: RuleBody list
    }

