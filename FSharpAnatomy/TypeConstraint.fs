namespace FSharpAnatomy

type TypeConstraint = 
    | NullConstraint               of Typar
    | ValueTypeConstraint          of Typar
    | RefTypeConstraint            of Typar
    | UnmanagedConstraint          of Typar
    | EqualityConstraint           of Typar
    | ComparisonConstraint         of Typar
    | SubtypeConstraint            of Typar*TypeArgument
    | EnumConstraint               of Typar*TypeArgument
    | DefaultConstructorConstraint of Typar*TypeArgument
    | DelegateConstraint           of Typar* domain:TypeArgument * retType:TypeArgument
    | MemberConstraint             of Typar list * isStatic:bool* isOperator:bool * name:string * retType:TypeArgument

type PostfixTyparDecls = PostfixTyparDecls of Typar list * TypeConstraint list


