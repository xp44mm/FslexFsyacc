namespace FslexFsyacc.ItemCores

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Idioms
open FSharp.Idioms.Literal

type ItemCoreRowTest (output:ITestOutputHelper) =

    [<Fact>]
    member _.``01 - empty production test``() =
        let production = ["e"]
        let y = ItemCoreRow.spread production
        output.WriteLine(stringify y)
        let e = [{production= ["e"];dot= 0;backwards= [];forwards= [];dotmax= true;isKernel= false}]
        Should.equal e y

    [<Fact>]
    member _.``02 - augment production test``() =
        let production = ["";"s"]
        let y = ItemCoreRow.spread production
        output.WriteLine(stringify y)
        let e = [
            {production= ["";"s"];dot= 0;backwards= [];forwards= ["s"];dotmax= false;isKernel= true};
            {production= ["";"s"];dot= 1;backwards= ["s"];forwards= [];dotmax= true;isKernel= true}]

        Should.equal e y

    [<Fact>]
    member _.``03 - pointer production test``() =
        let production = ["L";"*";"R"]
        let y = ItemCoreRow.spread production
        output.WriteLine(stringify y)
        let e = [
            {production= ["L";"*";"R"];dot= 0;backwards= [];forwards= ["*";"R"];dotmax= false;isKernel= false};
            {production= ["L";"*";"R"];dot= 1;backwards= ["*"];forwards= ["R"];dotmax= false;isKernel= true};
            {production= ["L";"*";"R"];dot= 2;backwards= ["R";"*"];forwards= [];dotmax= true;isKernel= true}]

        Should.equal e y

    [<Fact>]
    member _.``04 - assign production test``() =
        let production = ["S";"L";"=";"R"]
        let y = ItemCoreRow.spread production
        output.WriteLine(stringify y)
        let e = [
            {production= ["S";"L";"=";"R"];dot= 0;backwards= [];forwards= ["L";"=";"R"];dotmax= false;isKernel= false};
            {production= ["S";"L";"=";"R"];dot= 1;backwards= ["L"];forwards= ["=";"R"];dotmax= false;isKernel= true};
            {production= ["S";"L";"=";"R"];dot= 2;backwards= ["=";"L"];forwards= ["R"];dotmax= false;isKernel= true};
            {production= ["S";"L";"=";"R"];dot= 3;backwards= ["R";"=";"L"];forwards= [];dotmax= true;isKernel= true}]

        Should.equal e y
