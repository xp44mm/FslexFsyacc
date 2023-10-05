namespace FslexFsyacc.Fsyacc

type RawFsyaccFile = 
    {
        header:string
        rules:(string*((string list*string*string)list))list
        precedences:(string*string list)list
        declarations:(string*string list)list
    }

//type RuleBranch = {
//    productionBody:string list
//    dummyToken:string option
//    sematic:string
//}

//type RuleGroup = {
//    productionHead:string
//    branches:list<RuleBranch>
//}

//type FsyaccFile = 
//    {
//        header:string
//        rules:list<RuleGroup>
//        precedences:(string*string list)list
//        declarations:(string*string list)list
//    }
