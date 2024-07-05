
namespace FslexFsyacc

open FSharp.Idioms
open System.Text.RegularExpressions

///
type Lines = 
    {
        lines: SourceText []
    }

    static member just(lines) =
        {
            lines = lines
        }

    static member split (sourceText: SourceText) = 
        let re = Regex(@"^[^\r\n]*(\r?\n|\r)")

        let rec loop (src:SourceText) =
            seq {
                let m = re.Match(src.text)
                if m.Success then
                    let line = src.take m.Length
                    yield line
                    let rest = src.skip m.Length
                    yield! loop rest
                else
                    yield src // inp剩下最后一行了
            }
        loop sourceText
        |> Array.ofSeq
        |> Lines.just

    // line,column
    member this.coordinate (pos:int) = 
        this.lines
        |> Seq.mapi( fun r src -> r, src.index, src.endIndex )
        |> Seq.pick( fun(row,n,m) -> 
            if n <= pos && pos <= m then
                let col = pos - n
                Some(row,col)
            else None
            )
        |> Coordinate.just

    member this.sameLine (i:int) (j:int) = 
        let i = this.coordinate(i).line
        let j = this.coordinate(j).line
        i = j
