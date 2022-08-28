namespace FslexFsyacc.Fsyacc
open System

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

    [<Obsolete("FlatFsyaccFile.start")>]
    ///根据startSymbol提取相关规则，无用规则被无视忽略。
    member this.start(startSymbol:string, terminals:Set<string>) =
        let o = 
            this.rules
            |> List.filter(fun (lhs,_) -> 
                lhs 
                |> terminals.Contains
                |> not)
            |> FsyaccFileStart.extractRules <| startSymbol

        let rules = o.rules
        let symbols = o.symbols

        let precedences =
            this.precedences
            |> List.map(fun(assoc,ls)->
                assoc,ls|>List.filter symbols.Contains
            )
            |> List.filter(fun (_,ls)-> not ls.IsEmpty)

        let declarations =
            this.declarations
            |> List.filter(fst>>symbols.Contains)

        {
            this with
                rules = rules
                precedences = precedences
                declarations = declarations
        }

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
            
    
