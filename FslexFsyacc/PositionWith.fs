namespace FslexFsyacc
open System

type PositionWith<'value> =
    {
        index :int
        length:int
        value :'value
    }

    static member just(index,length,value) =
        {
            index  = index
            length = length
            value  = value
        }

    /// value's immediately next index
    member this.adjacent = 
        this.index + this.length

    /// value's last char index
    member this.endIndex =
        this.adjacent - 1

    [<Obsolete("SourceText.skip(this.length)")>]
    member this.follows(rest:string) = 
        // this位于rest的开头
        rest.[this.length..]

module PositionWith =
    /// union first .. last
    let totalLength(ls:list<PositionWith<'value>>) =
        let i0 = ls.Head.index
        let i1 =
            ls
            |> Seq.last
            |> (fun p -> p.index+p.length)
        let length = i1 - i0
        length

