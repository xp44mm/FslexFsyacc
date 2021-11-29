module FslexFsyacc.Fsyacc.FsyaccCompiler

//let normalize (tokens:seq<FsyaccToken>) = 
//    FsyaccDFA.analyzer.split(tokens,getTag)
//    |> Seq.concat

//let parse (tokens)= 
//    FsyaccParseTable.parser.parse(
//        tokens,
//        FsyaccToken.getTag,
//        FsyaccToken.getLexeme)

let compile (inp:string) =
    inp
    |> FsyaccToken.tokenize
    |> FsyaccDFA.split
    |> Seq.concat
    |> FsyaccParseTable.parse
    //|> unbox<string*(string*((string list*string*string)list))list*(string*string list)list*(string*string)list>
