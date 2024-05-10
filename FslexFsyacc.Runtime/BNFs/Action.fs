namespace FslexFsyacc.Runtime.BNFs

open FslexFsyacc.Runtime.Grammars
open FslexFsyacc.Runtime.ItemCores
open FSharp.Idioms

/// SLR && LALR
type Action = 
    /// 这个shift包括龙书中的 *shift* lookahead and *goto* nonterminal
    | Shift of kernel: Set<ItemCore>
    | Reduce of production: string list
