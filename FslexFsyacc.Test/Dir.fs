module FslexFsyacc.Dir

open System.IO

let solutionPath = DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName
let projPath = Path.Combine(solutionPath,"FslexFsyacc")
