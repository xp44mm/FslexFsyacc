namespace FslexFsyacc.Fsyacc

type RegularSymbol =
    /// a
    | Atomic of string

    /// a?*+ :repetition
    | Repetition of RegularSymbol*string

    /// [ a b c ]
    | Oneof of RegularSymbol list

    /// (a b c)
    | Chain of RegularSymbol list
