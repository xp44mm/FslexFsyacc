namespace FslexFsyacc.ItemCores

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open FSharp.Idioms
open FSharp.Idioms.Literal

type ItemCoreTest(output:ITestOutputHelper) =
    [<Fact>]
    member _.``01 - empty production test``() =
        let e = [{production= ["e"];dot= 0;backwards= [];forwards= [];dotmax= true;isKernel= false}]
        let x0 = {production= e.[0].production;dot= e.[0].dot}

        Should.equal x0.backwards e.[0].backwards
        Should.equal x0.forwards e.[0].forwards
        Should.equal x0.dotmax e.[0].dotmax
        Should.equal x0.isKernel e.[0].isKernel

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    member _.``02 - augment production test``(dot:int) =
        let e = [
            {production= ["";"s"];dot= 0;backwards= [];forwards= ["s"];dotmax= false;isKernel= true};
            {production= ["";"s"];dot= 1;backwards= ["s"];forwards= [];dotmax= true;isKernel= true}]
        let i = e.[dot]
        let x = {production= i.production;dot= i.dot}
        Should.equal x.backwards i.backwards
        Should.equal x.forwards i.forwards
        Should.equal x.dotmax i.dotmax
        Should.equal x.isKernel i.isKernel

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    member _.``03 - pointer production test``(dot:int) =
        let e = [
            {production= ["L";"*";"R"];dot= 0;backwards= [];forwards= ["*";"R"];dotmax= false;isKernel= false};
            {production= ["L";"*";"R"];dot= 1;backwards= ["*"];forwards= ["R"];dotmax= false;isKernel= true};
            {production= ["L";"*";"R"];dot= 2;backwards= ["R";"*"];forwards= [];dotmax= true;isKernel= true}]
        let i = e.[dot]
        let x = {production= i.production;dot= i.dot}
        Should.equal x.backwards i.backwards
        Should.equal x.forwards i.forwards
        Should.equal x.dotmax i.dotmax
        Should.equal x.isKernel i.isKernel

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    member _.``04 - assign production test``(dot:int) =
        let e = [
            {production= ["S";"L";"=";"R"];dot= 0;backwards= [];forwards= ["L";"=";"R"];dotmax= false;isKernel= false};
            {production= ["S";"L";"=";"R"];dot= 1;backwards= ["L"];forwards= ["=";"R"];dotmax= false;isKernel= true};
            {production= ["S";"L";"=";"R"];dot= 2;backwards= ["=";"L"];forwards= ["R"];dotmax= false;isKernel= true};
            {production= ["S";"L";"=";"R"];dot= 3;backwards= ["R";"=";"L"];forwards= [];dotmax= true;isKernel= true}]

        let i = e.[dot]
        let x = {production= i.production;dot= i.dot}
        Should.equal x.backwards i.backwards
        Should.equal x.forwards i.forwards
        Should.equal x.dotmax i.dotmax
        Should.equal x.isKernel i.isKernel
