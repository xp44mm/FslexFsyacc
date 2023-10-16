namespace FslexFsyacc.Fsyacc

type RawFsyaccFile = 
    {
        header:string
        inputRules:(string*((string list*string*string)list))list
        precedenceLines:(string*string list)list
        declarationLines:(string*string list)list
    }

