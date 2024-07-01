namespace FslexFsyacc

type Range =
    {
        Start: Position2
        End: Position2
    }

    /// m n
    member m.IsAdjacentTo(n: Range) =
        m.End = n.Start

    member r.stringOfRange () =
        sprintf "%s-%s" (r.Start.stringOfPos()) (r.End.stringOfPos())
