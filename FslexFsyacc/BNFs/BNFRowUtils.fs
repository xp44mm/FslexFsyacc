module FslexFsyacc.BNFs.BNFRowUtils

open System.Collections.Concurrent

let rows = ConcurrentDictionary< Set<list<string>> , BNFRow >(
            HashIdentity.Structural)

let getRow (productions: Set<list<string>>) =
    rows.GetOrAdd( productions, BNFRow.from )
