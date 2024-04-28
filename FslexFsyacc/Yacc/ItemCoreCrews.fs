namespace FslexFsyacc.Yacc
open FslexFsyacc.Runtime

// ProductionCrew(production,leftside,body)
type ProductionCrew(production:list<string>,leftside:string,body:list<string>) =
    member _.production = production
    member _.leftside = leftside
    member _.body = body


// ItemCoreCrew(prototype,dot,backwards,forwards,dotmax,isKernel)
[<System.Obsolete("useful")>]
type ItemCoreCrew(prototype:ProductionCrew,dot:int,backwards:list<string>,forwards:list<string>,dotmax:bool,isKernel:bool) =
    inherit ProductionCrew(prototype.production,prototype.leftside,prototype.body)
    member _.dot = dot
    member _.backwards = backwards
    member _.forwards = forwards
    member _.dotmax = dotmax
    member _.isKernel = isKernel
