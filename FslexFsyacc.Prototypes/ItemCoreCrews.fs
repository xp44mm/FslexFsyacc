namespace FslexFsyacc.Prototypes

type ProductionCrew = {
    production:string list
    leftside  :string
    body      :string list
    }

type ItemCoreCrew = {
    prototype:ProductionCrew
    dot:int
    backwards:string list
    forwards:string list
    dotmax:bool
    isKernel:bool
    }


