namespace FslexFsyacc.Fslex
open FslexFsyacc.Runtime
open FslexFsyacc.Runtime.Lex

type FslexFile = 
    {
        header:string
        definitions: (string*RegularExpression<string>)list
        rules: (RegularExpression<string>list*string)list
    }
