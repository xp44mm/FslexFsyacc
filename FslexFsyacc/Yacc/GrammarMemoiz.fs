module FslexFsyacc.Yacc.GrammarMemoiz

open System.Collections.Concurrent

let mainProductions = ConcurrentDictionary<Set<string list>, Set<string list>>(HashIdentity.Structural)
let symbols         = ConcurrentDictionary<Set<string list>, Set<string>>(HashIdentity.Structural)
let nonterminals    = ConcurrentDictionary<Set<string list>, Set<string>>(HashIdentity.Structural)
let nullables       = ConcurrentDictionary<Set<string list>, Set<string>>(HashIdentity.Structural)
let firsts          = ConcurrentDictionary<Set<string list>, Map<string,Set<string>>>(HashIdentity.Structural)
let lasts           = ConcurrentDictionary<Set<string list>, Map<string,Set<string>>>(HashIdentity.Structural)
let follows         = ConcurrentDictionary<Set<string list>, Map<string,Set<string>>>(HashIdentity.Structural)
