namespace rec FslexFsyacc.TypeArguments

type SuffixType =
    | LongIdent of string list
    | ArrayTypeSuffix of int

    member this.toString() = 
        match this with
        | LongIdent ids -> ids |> String.concat "."
        | ArrayTypeSuffix rank -> 
            let cs = String.replicate rank ","
            $"[{cs}]"

    member this.toLongIdent = 
        match this with
        | LongIdent ids -> ids
        | ArrayTypeSuffix rank -> 
            if rank = 0 then 
                ["array"]
            else
                [$"array{rank+1}D"]
