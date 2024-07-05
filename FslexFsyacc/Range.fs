namespace FslexFsyacc

type Range =
    {
        Start: Coordinate
        End: Coordinate
    }

    /// m n
    member m.IsAdjacentTo(n: Range) =
        m.End = n.Start

    member r.stringOfRange () =
        sprintf "%s-%s" (r.Start.stringOfPos()) (r.End.stringOfPos())
