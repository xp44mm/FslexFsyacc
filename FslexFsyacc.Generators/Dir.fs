module FslexFsyacc.Dir

open System.IO

let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName

let bootstrap = Path.Combine(solutionPath,"FslexFsyacc.Bootstrap")
