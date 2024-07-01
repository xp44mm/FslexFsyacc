namespace FslexFsyacc

open FSharp.Idioms.Line

/// Line and Column is Position2
type Position2 = 
    {
        // Visual Studio uses line counts starting at 0, F# uses them starting at 1
        ///基于0
        Line: int
        ///基于0
        Column: int
    }

    /// q p
    member p.IsAdjacentTo(q: Position2) =
        p.Line = q.Line && p.Column + 1 = q.Column

    member pos.stringOfPos () = sprintf "(%d,%d)" pos.Line pos.Column
