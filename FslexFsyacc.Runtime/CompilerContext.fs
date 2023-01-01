namespace FslexFsyacc.Runtime

type CompilerContext<'tok when 'tok : comparison> = {
    tokens: list<Position<'tok>>
    states: (int*obj)list
}

module CompilerContext =
    let nextIndex (context:CompilerContext<_>) =
        match context.tokens with
        | [] -> 0
        | tok::_ -> tok.nextIndex
