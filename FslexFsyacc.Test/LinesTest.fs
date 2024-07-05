namespace FslexFsyacc

open Xunit
open Xunit.Abstractions

open FSharp.Idioms.Literal
open FSharp.xUnit

open FslexFsyacc.Fsyacc
open FslexFsyacc

type LinesTest (output:ITestOutputHelper) =

    [<Fact>]
    member _.``splitLines test``() =
        let x = 
            [
                "namespace x"
                "let a = b"
                "open x"
            ]
            |> String.concat "\r\n"

        let x = SourceText.just(0, x)

        let e = Lines.just [|
            {index=0;text="namespace x\r\n"};
            {index=13;text="let a = b\r\n"};
            {index=24;text="open x"}
            |]

        let y = 
            Lines.split x
        output.WriteLine(stringify y)
        Should.equal e y

    [<Fact>]
    member this.``split lines``() =
        let x = SourceText.just(0, "xyz\r\n\nabc")
        let y = Lines.split x
        let e = Lines.just [|
            SourceText.just(0, "xyz\r\n"); 
            SourceText.just(5, "\n"); 
            SourceText.just(6, "abc")
            |]
        Should.equal e y

    [<Fact>]
    member this.``row column``() =
        let x = SourceText.just(0, "xyz\r\n\nabc")
        let lines = Lines.split x
        let pos = 7
        let coord = lines.coordinate pos
        Should.equal x.text.[pos] 'b'
        Should.equal coord.line 2
        Should.equal coord.column 1

    [<Fact>]
    member _.``getColumnAndRest``() =
        let start = 19
        let inp = "0123456789\nabc"
        let src = SourceText.just(start,inp)
        let pos = 20

        let lines = Lines.split src

        let coord = lines.coordinate pos

        let nextStart = 
            lines.lines.[coord.line].adjacent

        Should.equal coord.column 1
        Should.equal nextStart 30


