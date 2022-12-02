module FslexFsyacc.Dir

open System.IO

let testPath = __SOURCE_DIRECTORY__

let solutionPath = DirectoryInfo(testPath).Parent.FullName
let projPath = Path.Combine(solutionPath,"FslexFsyacc")
