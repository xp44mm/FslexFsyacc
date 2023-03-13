namespace FslexFsyacc.Fsyacc
open System

[<Obsolete("=> RawFsyaccFile2")>]
type RawFsyaccFile = 
    {
        header:string
        rules:(string*((string list*string*string)list))list
        precedences:(string*string list)list
        declarations:(string*string)list
    }

    /// 合并重复的lhs左手边符号
    member this.normRules() =
        let rules = 
            this.rules
            |> List.groupBy fst // lhs可能分成多个组，合并到第一次出现的顺序
            |> List.map(fun(lhs,rules)-> 
                match rules with
                | [rule] -> rule //没有重复
                | _ -> //有重复，依次合并
                    let bodies = 
                        rules
                        |> List.collect snd
                    lhs,bodies
            )
        {
            this with
                rules = rules
        }

    ///打印`*.fsyacc`文件
    member this.render() =
        FsyaccFileRender.renderFsyacc
            this.header
            this.rules
            this.precedences
            this.declarations

    ///从`*.fsyacc`文件中解析成本类型的数据
    static member parse(sourceText:string) =
        let header,rules,precedences,declarations = 
            FsyaccCompiler.compile sourceText

        {
            header = header
            rules = rules
            precedences = precedences
            declarations = declarations
        }
            
    
