namespace FslexFsyacc

type ParseTableApp =
    {
        tokens: Set<string>
        kernels: list<list<int*int>>
        kernelSymbols: list<string>
        actions: (string*int) list list
        rules: list<string list*(obj list->obj)>
    }

    member this.getParser<'tok>(getTag, getLexeme) =
        let getTag (tok:'tok) =
            let tag = getTag tok
            if this.tokens.Contains tag then
                tag
            else raise (invalidArg "tok" $"遇到一个yacc中没有的token:{tag}")
        MoreParser<'tok>.from(this.rules, this.actions, getTag, getLexeme)

    member this.getTable<'tok> (parser:MoreParser<'tok>) = 
        DecompressedParseTable.decompress(
            parser.baseParser.rules, 
            this.kernels, 
            this.kernelSymbols,
            parser.baseParser.actions
            )

//用法
//let app: ParseTableApp = {
//    tokens = tokens
//    kernels = kernels
//    kernelSymbols = kernelSymbols
//    actions = actions
//    rules = rules
//}
