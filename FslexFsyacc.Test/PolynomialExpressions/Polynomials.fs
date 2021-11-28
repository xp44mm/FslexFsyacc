namespace PolynomialExpressions

type Term =
    | Term  of int * string * int
    | Const of int

type Polynomial = Term list