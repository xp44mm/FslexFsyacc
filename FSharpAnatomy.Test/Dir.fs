module FSharpAnatomy.Dir

open System.IO

let testPath = __SOURCE_DIRECTORY__

let solutionPath = DirectoryInfo(testPath).Parent.FullName

let FSharpAnatomyPath = Path.Combine(solutionPath,"FSharpAnatomy")
