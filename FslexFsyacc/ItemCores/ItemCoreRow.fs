namespace FslexFsyacc.ItemCores

type ItemCoreRow = 
    {
    production : string list
    dot        : int
    backwards  : string list
    forwards   : string list
    dotmax     : bool
    isKernel   : bool
    }

    static member spread (production : string list) =
        let head = production.Head
        let body = production.Tail
        let len = body.Length

        let rec loop dot backwards forwards =
            [
                yield dot, backwards, forwards
                match forwards with
                | [] -> ()
                | h::t -> yield! loop (dot+1) (h::backwards) t
            ]

        loop 0 [] body
        |> List.map(fun (dot, backwards, forwards) ->
            {
                production = production
                dot        = dot
                backwards  = backwards
                forwards   = forwards
                dotmax     = forwards.IsEmpty
                isKernel   = production.Head = "" || dot > 0
            }
        )
