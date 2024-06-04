namespace FslexFsyacc.Fslex
open FslexFsyacc
open FslexFsyacc.Lex

type FslexFile = 
    {
        header:string
        definitions: (string*RegularExpression<string>)list
        rules: (RegularExpression<string>list*string)list
    }
