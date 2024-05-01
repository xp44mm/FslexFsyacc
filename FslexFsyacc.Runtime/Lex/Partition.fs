module FslexFsyacc.Runtime.Lex.Partition

open FSharp.Idioms

//找到DFA状态所在的分区，第一个分区
let findPartition (partitions:Set<Set<'dstate>>) (state:'dstate) =
    partitions 
    |> Seq.find(fun p -> p.Contains state)

/// 最初的分区最少，通过迭代，将其分割。
let splitPartitions<'dstate,'a when 'dstate:comparison and 'a:comparison>
    (dtran:Set<'dstate*'a*'dstate>)
    (firstPartitions:Set<Set<'dstate>>)
    =

    let rec loop (stateActions:Set<'dstate*Set<'a*'dstate>>) (partitions:Set<Set<'dstate>>) =

        let findPartitionIn = findPartition partitions

        //singleton起点的分区一定不可以再分割，所以丢弃
        let stateActions =
            stateActions
            |> Set.filter(fun(s,_) -> (findPartitionIn s).Count > 1)

        //转换的开始状态，结束状态，分别对应到各自的分区
        let distinguishable =
            stateActions
            |> Set.groupBy(fst>>findPartitionIn) // 以开始分区为组
            |> Set.map(fun(currentStartPartition,subStateActions)->
                let startStates = Set.map fst subStateActions
                let diff = currentStartPartition - startStates //不可以省略的死状态

                // 检测分区中的每个状态的动作是否完全一致，一致的归到同组，不一致归到不同组。每个组将转化为新的小分区。
                let newStartPartitions =
                    subStateActions
                    |> Set.groupBy(snd >> Set.map(fun(a, t) -> a, findPartitionIn t))
                    |> Set.map(snd >> Set.map fst) // 在分组值中，取每个转换行的开始状态，合并为一个新分区
                    |> fun partitions -> // 增加分区中不在开始分区中的状态。
                        if diff.IsEmpty then partitions else Set.add diff partitions

                //一个现有分区将分解为一个以上的新分区
                currentStartPartition,newStartPartitions
            )
            |> Set.filter(fun(_,newStartPartitions)-> newStartPartitions.Count > 1)

        if distinguishable.IsEmpty then
            // 所有分区已经分解完成
            partitions
        else
            let partitions = 
                (partitions,distinguishable)
                ||> Seq.fold (fun partitions (oldPartition,newPartitions) ->
                    partitions
                    |> Set.remove oldPartition
                    |> Set.union newPartitions
                )

            loop stateActions partitions

    // 同一状态的路径分组到一个Set中。
    let stateActions = 
        dtran
        |> Set.groupBy Triple.first
        |> Set.map(fun(s,ls)-> 
            let actions =  ls |> Set.map Triple.lastTwo
            s,actions)

    loop stateActions firstPartitions

