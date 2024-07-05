namespace FslexFsyacc

open FSharp.Idioms.Line

/// Visual Studio uses line counts starting at 0, F# uses them starting at 1
type Coordinate = 
    {
        line: int
        column: int
    }

    static member just(line,column) =
        {
            line = line
            column = column
        }

    /// q p
    member p.IsAdjacentTo(q: Coordinate) =
        p.line = q.line && p.column + 1 = q.column

    member pos.stringOfPos () = sprintf "(%d,%d)" pos.line pos.column
