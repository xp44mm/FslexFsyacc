namespace OptimizationLex
open Compiler

open Xunit
open Xunit.Abstractions

/// 3.9.3
type PositionTest(output:ITestOutputHelper) =
    // Fig. 3.56
    let r1 = Union(Leaf('a'),Leaf('b'))
    let r2 = Natural r1
    let r3 = Concat(r2, Leaf('a'))
    let r4 = Concat(r3, Leaf('b'))
    let r5 = Concat(r4, Leaf('b'))

    [<Fact>]
    member this.``Position``() =
        let res = RegExpPositions.from r5
        res
        //Should.equal expect res

    [<Fact>]
    member this.``firstpos``() =
        let f1 = RegExpPositions.firstpos <| RegExpPositions.from r1
        let f2 = RegExpPositions.firstpos <| RegExpPositions.from r2
        let f3 = RegExpPositions.firstpos <| RegExpPositions.from r3
        let f4 = RegExpPositions.firstpos <| RegExpPositions.from r4
        let f5 = RegExpPositions.firstpos <| RegExpPositions.from r5

        Should.equal f1 (set [1;2])
        Should.equal f2 (set [1;2])
        Should.equal f3 (set [1;2;3])
        Should.equal f4 (set [1;2;3])
        Should.equal f5 (set [1;2;3])

    [<Fact>]
    member this.``lastpos``() =
        let l1 = RegExpPositions.lastpos <| RegExpPositions.from r1
        let l2 = RegExpPositions.lastpos <| RegExpPositions.from r2
        let l3 = RegExpPositions.lastpos <| RegExpPositions.from r3
        let l4 = RegExpPositions.lastpos <| RegExpPositions.from r4
        let l5 = RegExpPositions.lastpos <| RegExpPositions.from r5

        Should.equal l1 (set [1;2])
        Should.equal l2 (set [1;2])
        Should.equal l3 (set [3])
        Should.equal l4 (set [4])
        Should.equal l5 (set [5])

    [<Fact>]
    member this.``followpos``() =
        let mp = RegExpPositions.followpos <| RegExpPositions.from r5

        let r1 = mp.[1]
        let r2 = mp.[2]
        let r3 = mp.[3]
        let r4 = mp.[4]
        let r5 = if mp.ContainsKey 5 then failwith "" else Set.empty

        //output.WriteLine(sprintf "%A" mp)

        Should.equal r1 (set [1;2;3])
        Should.equal r2 (set [1;2;3])
        Should.equal r3 (set [4])
        Should.equal r4 (set [5])
        Should.equal r5 (set [])
