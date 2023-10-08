namespace FslexFsyacc.Fsyacc

/// 
type FsyaccParseTableFile =
    {
        header: string
        rules:(string list*string)list
        actions:(string*int)list list
        closures:(int*int*string list)list list
        declarations:Map<string,string> // symbol -> type of symbol
    }

