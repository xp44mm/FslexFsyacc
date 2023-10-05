module FslexFsyacc.Prototypes.Dir

open System.IO

let testProjPath = __SOURCE_DIRECTORY__

let solutionPath = DirectoryInfo(testProjPath).Parent.FullName

let crewProjPath = Path.Combine(solutionPath, "FslexFsyacc.Prototypes")

let dllFilePath = Path.Combine(crewProjPath,@"bin\Release\net6.0\FslexFsyacc.Prototypes.dll")

let fslexFsyaccPath = Path.Combine(solutionPath, "FslexFsyacc")

let yaccFilePath = Path.Combine(fslexFsyaccPath, "Yacc")
