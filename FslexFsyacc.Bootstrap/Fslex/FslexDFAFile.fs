namespace FslexFsyacc.Fslex

type FslexDFAFile = 
    {
        header: string
        rules: ( uint32 list * uint32 list * string ) list
        nextStates: ( uint32 * ( string * uint32 ) list ) list
    }

