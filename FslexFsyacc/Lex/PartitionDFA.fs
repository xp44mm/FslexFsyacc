namespace FslexFsyacc.Lex
open FSharp.Idioms

/// DFA，他的状态是由NFA状态的集合表示
type PartitionDFA<'dstate,'a when 'dstate: comparison and 'a: comparison> =
    {
        dtran:Set<'dstate*'a*'dstate>
        allStates:Set<'dstate>
        dfinals:Set<'dstate> list
    }

    static member create<'nstate when 'nstate: comparison>(dfa:SubsetDFA<'nstate,'a>,nfinals:'nstate list) =
        {
            dtran = dfa.dtran
            allStates = dfa.allStates
            dfinals = dfa.getAcceptingStates(nfinals)
        }

    /// example 3-41
    member this.firstPartitions() =
        let rest = this.dfinals |> List.fold(fun res st -> res - st) this.allStates

        this.dfinals
        |> Set.ofList
        |> Set.add rest

    member this.encode() =
        let allStates = [ 0u .. uint32(this.allStates.Count - 1)]

        let orderedAllStates = Set.toList this.allStates

        // 狀態從集合編碼成整數
        let encodes =
            orderedAllStates
            |> List.zip <| allStates
            |> Map.ofList

        let dfa =
            {
                dtran =
                    this.dtran
                    |> Set.map(fun(s,a,t)-> encodes.[s],a,encodes.[t])
                dfinals =
                    this.dfinals
                    |> List.map(fun st -> st |> Set.map(fun i -> encodes.[i]))
                allStates =
                    Set.ofList allStates

            }

        let decodes =
            orderedAllStates
            |> List.zip allStates
            |> Map.ofList

        dfa,decodes,encodes

    member encodeDFA.minimize(decodes:Map<'dstate, Set<'nstate>>) =
        let partitions = 
            encodeDFA.firstPartitions()
            |> Partition.splitPartitions encodeDFA.dtran
                
        let locatePartition = 
            partitions
            |> Partition.findPartition

        let decode partition = 
            partition 
            |> Set.map(fun dstate -> decodes.[dstate]) 
            |> Set.unionMany

        let dtran =
            encodeDFA.dtran
            |> Set.map(fun (src,lbl,tgt) ->
                let src = locatePartition src
                let tgt = locatePartition tgt
                src,lbl,tgt
            )
            |> Set.map(fun(s,a,t) -> decode s,a, decode t)

        let allStates = 
            partitions
            |> Set.map decode

        let dfinals =
            encodeDFA.dfinals
            |> List.map(fun st -> st |> Set.map (locatePartition>>decode))
        {
            dtran = dtran
            allStates = allStates
            dfinals = dfinals
        }