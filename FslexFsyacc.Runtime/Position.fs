﻿namespace FslexFsyacc.Runtime

// band
type Position<'value> =
    {
        index :int
        length:int
        value :'value
    }

    static member from(index,length,value) =
        {
            index  = index
            length = length
            value  = value
        }

    member this.nextIndex = 
        this.index + this.length

    static member totalLength(ls:list<Position<'value>>) =
        let i0 = ls.Head.index
        let i1 =
            ls
            |> Seq.last
            |> (fun p -> p.nextIndex)
        let length = i1 - i0
        length

    //this位于rest的开头
    member this.follows(rest:string) = rest.[this.length..]
