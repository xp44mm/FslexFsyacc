namespace OptimizationLex
open Compiler

open Xunit
open Xunit.Abstractions

type RegExpTest(output:ITestOutputHelper) =
    // Fig. 3.43
    let r1 = Leaf 'a'
    let r2 = Leaf 'b'
    let r3 = Union (r1,r2)
    let r5 = Natural r3
    let r7 = Concat(r5, Leaf 'a')
    let r9 = Concat(r7, Leaf 'b')
    let r11= Concat(r9, Leaf 'b')


    [<Fact>]
    member this.``leaves``() =
        let res = RegExpModule.leaves r11
        //output.WriteLine(sprintf "%A" res)
        let expect = ['a'; 'b'; 'a'; 'b'; 'b']
        Should.equal expect res
        Should.equal 1 (RegExpModule.leaves r1).Length
        Should.equal 2 (RegExpModule.leaves r3).Length

    [<Fact>]
    member this.``nullable``() =
        Assert.False(RegExpModule.nullable r1)
        Assert.False(RegExpModule.nullable r2)
        Assert.False(RegExpModule.nullable r3)
        Assert.True (RegExpModule.nullable r5)
        Assert.False(RegExpModule.nullable r7)