module FslexFsyacc.Yacc.ProductionUtils

/// Normally, the precedence of a production is taken to be the same as
/// that of its rightmost terminal.
// production -> symbol
let revTerminalsOfProduction (terminals:Set<string>) (production:string list) =
    let body = production.Tail

    let rec loop (revls:string list) ls =
        match ls with
        | [] -> revls
        | h :: t -> 
            if terminals.Contains h then
                loop (h::revls) t
            else loop revls t
    loop [] body

