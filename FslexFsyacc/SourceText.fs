namespace FslexFsyacc

open System
open FSharp.Idioms
open FSharp.Idioms.ActivePatterns

type SourceText =
    {
        /// text's first char Position
        index : int
        text : string
    }

    static member just(index: int,text: string) =
        {
            index = index
            text = text
        }

    /// text's immediately next char position
    member this.adjacent =
        this.index + this.text.Length

    /// text's last char Position
    member this.endIndex =
        this.adjacent - 1

    /// 从头开始略过多少长度: Tumbling tumble
    member this.skip(length:int) =
        SourceText.just(this.index + length, this.text.[length..])

    member this.take(length:int) =
        SourceText.just(this.index, this.text.[..length-1])

    /// 从新的索引位置开始截取剩余文本: Hopping hop jump
    member this.jump(index:int) =
        if index < this.index || this.adjacent < index then
            raise <| ArgumentOutOfRangeException("start < index < adjacent")
        this.skip(index - this.index)
    [<Obsolete("Lines")>]
    member line.getColumnAndNextLine (index:int) = 
        Line.getColumnAndLpos (line.index,line.text) index

