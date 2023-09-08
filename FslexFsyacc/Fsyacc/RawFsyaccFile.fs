namespace FslexFsyacc.Fsyacc

type RawFsyaccFile = 
    {
        header:string
        rules:(string*((string list*string*string)list))list
        precedences:(string*string list)list
        declarations:(string*string list)list
    }

