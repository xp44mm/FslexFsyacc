namespace OptimizationLex
open Compiler

open Xunit
open Xunit.Abstractions

/// 3.9.3
type RegExpToDFATest(output:ITestOutputHelper) =
    // Fig. 3.56 (a|b)*abb#
    let r1 = Union(Leaf('a'),Leaf('b'))
    let r2 = Natural r1
    let r3 = Concat(r2, Leaf('a'))
    let r4 = Concat(r3, Leaf('b'))
    let r5 = Concat(r4, Leaf('b'))
    let r6 = Concat(r5, Leaf('#'))

    let regWithPos = RegExpPositions.from r6
    let dfaMaker = RegExpToDFA(regWithPos) // DFAMaker(r6,'#')

    let expAugmentDtrans = [
        (set [1; 2; 3], 'a', set [1; 2; 3; 4]); 
        (set [1; 2; 3], 'b', set [1; 2; 3]);
        (set [1; 2; 3; 4], 'a', set [1; 2; 3; 4]);
        (set [1; 2; 3; 4], 'b', set [1; 2; 3; 5]);
        (set [1; 2; 3; 5], 'a', set [1; 2; 3; 4]);
        (set [1; 2; 3; 5], 'b', set [1; 2; 3; 6]);
        (set [1; 2; 3; 6], 'a', set [1; 2; 3; 4]);
        (set [1; 2; 3; 6], 'b', set [1; 2; 3]); 
        (set [1; 2; 3; 6], '#', set [])]

    let expDtrans = [
        (set [1; 2; 3], 'a', set [1; 2; 3; 4]); 
        (set [1; 2; 3], 'b', set [1; 2; 3]);
        (set [1; 2; 3; 4], 'a', set [1; 2; 3; 4]);
        (set [1; 2; 3; 4], 'b', set [1; 2; 3; 5]);
        (set [1; 2; 3; 5], 'a', set [1; 2; 3; 4]);
        (set [1; 2; 3; 5], 'b', set [1; 2; 3; 6]);
        (set [1; 2; 3; 6], 'a', set [1; 2; 3; 4]);
        (set [1; 2; 3; 6], 'b', set [1; 2; 3])]

    //[<Fact>]
    //member this.``test dtrans``() =
    //    let res = dfaMaker.Dtrans
    //    //output.WriteLine(sprintf "%A" res)

    //    Should.equal res expAugmentDtrans

    [<Fact>]
    member this.``prepare data for DFARepresentation``() =
        Should.equal dfaMaker.Dstart (set [1; 2; 3])

        //接受状态,根据增广结尾符号
        let finalState = 
            expAugmentDtrans
            |> List.filter(fun(src,lbl,tgt)-> lbl = '#' && tgt = set [])
            |> List.exactlyOne
            |> fun (src,_,_) -> src

        Should.equal finalState (set [1; 2; 3; 6])

        //删除增广边,增广边仅仅用于标记接受状态
        let dtrans =
            expAugmentDtrans
            |> List.filter(fun(_,lbl,tgt)-> lbl <> '#' && tgt <> set [])

        //output.WriteLine(sprintf "%A" dtrans)
        Should.equal expDtrans dtrans

    [<Fact>]
    member this.``usage of DFARepresentation``() =
        let startState = set [1; 2; 3]
        let finalState = set [1; 2; 3; 6]

        //let repr = DFARepresentation expDtrans
        
        //let states = 
        //    repr.entireStates 
        //    |> Map.toSeq
        //    |> Seq.toList

        let expectStates = [
            (set [1; 2; 3], 0); 
            (set [1; 2; 3; 4], 1); 
            (set [1; 2; 3; 5], 2);
            (set [1; 2; 3; 6], 3)]

        //Should.equal expectStates states

        //let dtrans = repr.trans

        //output.WriteLine(sprintf "%A" dtrans)
        let expDtrans = [
            (0, 'a', 1); 
            (0, 'b', 0); 
            (1, 'a', 1); 
            (1, 'b', 2); 
            (2, 'a', 1); 
            (2, 'b', 3);
            (3, 'a', 1); 
            (3, 'b', 0)]

        //Should.equal expDtrans dtrans

        //let start = repr.entireStates.[startState]
        //let final = repr.entireStates.[finalState]

        //Should.equal 0 start
        //Should.equal 3 final

        ()