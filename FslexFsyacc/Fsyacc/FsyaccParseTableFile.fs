namespace FslexFsyacc.Fsyacc

/// 表示解析表文件，生成用的数据树：
type FsyaccParseTableFile =
    {
        header: string
        rules: (string list*string)list // rename to reducers
        actions: (string*int)list list
        closures: (int*int*string list)list list
        declarations: Map<string,string> // symbol -> type of symbol
    }

