namespace Prototypes

open System

type CrewInfo = {
    typeName:string
    prototype:CrewInfo option
    fields:list<string*Type>
}

