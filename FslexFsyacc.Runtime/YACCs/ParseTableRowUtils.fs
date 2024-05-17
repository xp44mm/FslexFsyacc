module FslexFsyacc.Runtime.YACCs.ParseTableRowUtils

open System.Collections.Concurrent

let rows = ConcurrentDictionary< 
            Set<list<string>> * Map<string list,string> * Map<string,int> , 
            ParseTableRow 
            >(HashIdentity.Structural)

let getRow (
    productions: Set<list<string>>, 
    dummyTokens:Map<string list,string>,
    precedences:Map<string,int>
    ) =
    rows.GetOrAdd((productions,dummyTokens,precedences), ParseTableRow.from )
