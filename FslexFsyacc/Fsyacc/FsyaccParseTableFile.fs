namespace FslexFsyacc.Fsyacc

/// 
type FsyaccParseTableFile =
    {
        rules:(string list*string)list
        actions:(string*int)list list
        closures:(int*int*string list)list list
        header: string
        declarations:(string*string)list
    }

