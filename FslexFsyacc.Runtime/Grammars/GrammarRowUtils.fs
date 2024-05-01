module FslexFsyacc.Runtime.Grammars.GrammarRowUtils

open System.Collections.Generic
open System.Collections.Concurrent

let rows = ConcurrentDictionary< Set<list<string>> , GrammarRow >(
            HashIdentity.Structural)

let getRow (productions: Set<list<string>>) =
    rows.GetOrAdd( productions, GrammarRow.from )
