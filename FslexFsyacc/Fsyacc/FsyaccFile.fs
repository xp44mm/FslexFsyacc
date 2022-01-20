namespace FslexFsyacc.Fsyacc

type FsyaccFile = 
    {
        header:string
        rules:(string*((string list*string*string)list))list
        precedences:(string*string list)list
        declarations:(string*string)list
    }

    static member parse(sourceText:string) =
        let header,rules,precedences,declarations = 
            FsyaccCompiler.compile sourceText
        {
            header = header
            rules = rules
            precedences = precedences
            declarations = declarations

        }

