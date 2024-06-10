namespace rec FslexFsyacc.TypeArguments

type SuffixType =
    | LongIdent of string list
    | ArrayTypeSuffix of int

    member this.toCode() = 
        match this with
        | LongIdent ids -> ids |> String.concat "."
        | ArrayTypeSuffix rank -> 
            let cs = String.replicate (rank-1) ","
            $"[{cs}]"

    member this.toLongIdent = 
        match this with
        | LongIdent ids -> ids
        | ArrayTypeSuffix rank -> 
            if rank = 1 then 
                ["array"]
            else
                [$"array{rank}D"]
