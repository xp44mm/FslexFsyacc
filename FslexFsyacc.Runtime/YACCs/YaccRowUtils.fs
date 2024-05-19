module FslexFsyacc.Runtime.YACCs.YaccRowUtils

open System.Collections.Concurrent

let rows = ConcurrentDictionary< 
            Set<list<string>> * Map<string list,string> * Map<string,int*Associativity> , 
            YaccRow 
            >(HashIdentity.Structural)

let getRow (
    productions: Set<list<string>>, 
    dummyTokens:Map<string list,string>,
    precedences:Map<string,int*Associativity>
    ) =
    rows.GetOrAdd((productions,dummyTokens,precedences), YaccRow.from )
