# Expression Simplification and Differentiation

A classic application of symbolic programming is working with algebraic expressions like the kind you find in high school mathematics. In this section you will learn how to do this kind of programming in F#.

Let's take it easy at first and assume you're dealing with simple algebraic expressions that can consist only of numbers, a single variable (it doesn't matter what it is, but let's assume it's `x`), sums, and products. Listing 12-5 shows the implementation of symbolic differentiation over this simple expression type.

##### Listing 12-5.  Symbolic differentiation over a simple expression type

```F#
open System

type Expr =
    | Var
    | Num of int
    | Sum of Expr * Expr
    | Prod of Expr * Expr

let rec deriv expr =
    match expr with
    | Var -> Num 1
    | Num _ -> Num 0
    | Sum (e1, e2)  -> Sum (deriv e1, deriv e2)
    | Prod (e1, e2) -> Sum (Prod (e1, deriv e2), Prod (e2, deriv e1))
```

The type of the `deriv` function is as follows:

```F#
val deriv : expr:Expr -> Expr
```

Now, let's find the derivative of a simple expression, say `1+2x`:

```F#
> let e1 = Sum (Num 1, Prod (Num 2, Var));;
val e1 : Expr = Sum (Num 1,Prod (Num 2,Var))
> deriv e1;;
val it : Expr = Sum (Num 0,Sum (Prod (Num 2,Num 1),Prod (Var,Num 0)))
```

The resulting expression is a symbolic representation of `0+(2*1+x*0)`, which indeed is 2—so it's right. You should do a couple of things next. First, install a custom printer so that F# Interactive responds using expressions that you're more used to using. Before you apply brute force and put parentheses around the expressions in each sum and product, let's contemplate it a bit. Parentheses are usually needed to give precedence to operations that would otherwise be applied later in the sequence of calculations. For instance, `2+3*4` is calculated as `2+(3*4)` because the product has a higher precedence; if you wanted to find `(2+3)*4`, you would need to use parentheses to designate the new order of calculation. Taking this argument further, you can formulate the rule for using parentheses: they're needed in places where an operator has lower precedence than the one surrounding it. You can apply this reasoning to the expression printer by passing a context-precedence parameter:

```F#
let precSum = 10
let precProd = 20
let rec stringOfExpr prec expr =
    match expr with
    | Var   -> "x"
    | Num i -> i.ToString()
    | Sum (e1, e2) ->
        let sum = stringOfExpr precSum e1 + "+" + stringOfExpr precSum e2
        if prec > precSum then
            "(" + sum + ")"
        else
            sum
    | Prod (e1, e2) ->
        stringOfExpr precProd e1 + "*" + stringOfExpr precProd e2
```

You can add this as a custom printer for this expression type:

```F#
> fsi.AddPrinter (fun expr -> stringOfExpr 0 expr);;
> let e3 = Prod (Var, Prod (Var, Num 2));;
val e3 : Expr = x*x*2
> deriv e3;;
val it : Expr = x*(x*0+2*1)+x*2*1
```

Parentheses are omitted only when a sum is participating in an expression that has a higher precedence, which in this simplified example means products. If you didn't add precedence to the pretty-printer, you'd get `x*x*0+2*1+x*2*1` for the last expression, which is incorrect.

## Implementing Local Simplifications

The next thing to do is to get your symbolic manipulator to simplify expressions so you don't have to do so. One easy modification is to replace the use of the `Sum` and `Prod` constructors in `deriv` with local functions that perform local simplifications, such as removing identity operations, performing arithmetic, bringing forward constants, and simplifying across two operations. Listing 12-6 shows how to do this.

##### Listing 12-6. Symbolic differentiation with local simplifications

```F#
let simpSum (a, b) =
    match a, b with
    | Num n, Num m -> Num (n+m)  // constants!
    | Num 0, e | e, Num 0 -> e   // 0+e = e+0 = e
    | e1, e2 -> Sum(e1, e2)

let simpProd (a, b) =
    match a, b with
    | Num n, Num m -> Num (n*m)    // constants!
    | Num 0, e | e, Num 0 -> Num 0 // 0*e=0
    | Num 1, e | e, Num 1 -> e     // 1*e = e*1 = e
    | e1, e2 -> Prod(e1, e2)

let rec simpDeriv e =
    match e with
    | Var -> Num 1
    | Num _ -> Num 0
    | Sum (e1, e2)  -> simpSum (simpDeriv e1, simpDeriv e2)
    | Prod (e1, e2) -> simpSum (simpProd (e1, simpDeriv e2),
                                simpProd (e2, simpDeriv e1))
```

These measures produce a significant improvement over the previous naïve approach, but they don't place the result in a normal form, as the following shows:

```F#
> simpDeriv e3;;
val it : Expr = x*2+x*2
```

However, you can't implement all simplifications using local rules; for example, collecting like terms across a polynomial involves looking at every term of the polynomial.

## A Richer Language of Algebraic Expressions

This section goes beyond the approach presented so far and shows a richer language of algebraic expressions with which to simplify and differentiate. This project uses the `FsLexYacc` package for parsing and lexing. For convenience, you can add the lexer `ExprLexer.fsl` and the parser `ExprParser.fsy` to the project so you can quickly edit them if necessary.

The main `Expr` type that represents algebraic expressions is contained in `Expr.fs`. Although you can use the expression constructors defined in this type to create expression values on the fly, the most convenient method for embedding complex expressions into this representation is by parsing them. Armed with the ability to encode and parse algebraic expressions, you place the derivation and simplification logic in its own module and file `ExprUtil.fs`. A parser is added in `ExprParser.fsy`, and a tokenizer in `ExprLexer.fsl`. A simple driver added to `Main.fs` completes the application.

Listing 12-7 shows the definition of the abstract syntax representation of expressions using a single `Expr` type. Expressions contain numbers, variables, negation, sums, differences, products, fractions, exponents, basic trigonometric functions (`sin x`, `cos x`), and `e^x`.

Let's look at this abstract syntax design more closely. In Chapter 9, you saw that choosing an abstract syntax often involves design choices, and that these choices often relate to the roles the abstract syntax representation should serve. In this case, you use the abstract syntax to compute symbolic derivatives and simplifications (using techniques similar to those seen earlier in this chapter) and also to graphically visualize the resulting expressions in a way that is pleasant for the human user. For this reason, you don't use an entirely minimalistic abstract syntax (for example, by replacing quotients with an inverse node), because it's helpful to maintain some additional structure in the input.

Here, you represent sums and differences not as binary terms (as you do for products and quotients) but instead as a list of expression terms. The `Sub` term also carries the *minuend*, the term that is to be reduced, separately. As a result, you have to apply different strategies when simplifying them.

##### Listing 12-7. Expr.fs: the core expression type for the visual symbolic differentiation application

```F#
namespace Symbolic.Expressions
type Expr =
    | Num  of decimal
    | Var  of string
    | Neg  of Expr
    | Add  of Expr list
    | Sub  of Expr * Expr list
    | Prod of Expr * Expr
    | Frac of Expr * Expr
    | Pow  of Expr * decimal
    | Sin  of Expr
    | Cos  of Expr
    | Exp  of Expr

    static member StarNeeded e1 e2 =
        match e1, e2 with
        | Num _, Neg _ | _, Num _ -> true
        | _ -> false

    member self.IsNumber =
        match self with
        | Num _ -> true | _ -> false

    member self.NumOf =
        match self with
        | Num num -> num | _ -> failwith "NumOf: Not a Num"

    member self.IsNegative =
        match self with
        | Num num | Prod (Num num, _) -> num < 0M
        | Neg e -> true | _ -> false

    member self.Negate =
        match self with
        | Num num -> Num (-num)
        | Neg e -> e
        | exp -> Neg exp
```

Listing 12-7 also shows the definition of some miscellaneous augmentations on the expression type, mostly related to visual layout and presentation. The `StarNeeded` member is used internally to determine whether the multiplication operator (the star symbol, or asterisk) is needed in the product of two expressions, `e1` and `e2`. You may want to extend this simple rule: any product whose right side is a number requires the explicit operator, and all other cases don't. Thus, expressions such as `2(x+1)` and `2x` are rendered without the asterisk.

The `IsNumber` member returns true if the expression at hand is numeric and is used in conjunction with `NumOf`, which returns this numeric component. Similarly, the `IsNegative` and `Negate` members determine whether you have an expression that starts with a negative sign, and they negate it on demand.

## Parsing Algebraic Expressions

This sample uses a lexer and a parser generated by the F# tools **fsyacc.exe** and **fslex.exe**, available as part of the **FsLexYacc** NuGet package. This chapter skips over the details of how the tools work; instead, it assumes that you have these tools already installed. Listings 12-8 and 12-9 show the code for the lexer and parser respectively. You need to manually build the lexer (generating `ExprLexer.fs`) and parser (generating `ExprParser.fs`) from the Windows command line as follows:

```bash
C:\samples> fsyacc ExprParser.fsy --module Symbolic.Expressions.ExprParser
C:\samples> fslex ExprLexer.fsl --unicode
```

##### Listing 12-8. ExprLexer.fsl: tokenizing the concrete syntax for algebraic expressions
```F#
{
module Symbolic.Expressions.ExprLexer

open System
open Symbolic.Expressions
open Symbolic.Expressions.ExprParser
open FSharp.Text.Lexing

let lexeme = LexBuffer<_>.LexemeString
let special s =
    match s with
    | "+" -> PLUS    | "-" -> MINUS
    | "*" -> TIMES   | "/" -> DIV
    | "(" -> LPAREN  | ")" -> RPAREN  | "^" -> HAT
    | _   -> failwith "Invalid operator"
let id s =
    match s with
    | "sin" -> SIN   | "cos" -> COS
    | "e"   -> E     | id    -> ID id
}

let digit     = ['0'-'9']
let int = digit+
let float     = int ('.' int)? (['e' 'E'] int)?
let alpha     = ['a'-'z' 'A'-'Z']
let id = alpha+ (alpha | digit | ['_' '$'])*
let ws = ' ' | '\t'
let nl = '\n' | '\r' '\n'
let special   = '+' | '-' | '*' | '/' | '(' | ')' | '^'
rule main = parse
    | int { INT (Convert.ToInt32(lexeme lexbuf)) }
    | float      { FLOAT (Convert.ToDouble(lexeme lexbuf)) }
    | id { id (lexeme lexbuf) }
    | special    { special (lexeme lexbuf) }
    | ws | nl    { main lexbuf }
    | eof { EOF }
    | _ { failwith (lexeme lexbuf) }
```

The parser has some syntactic sugar for polynomial terms, so it can parse `2x`, `2x^3`, or `x^4` without requiring you to add an explicit multiplication symbol after the coefficient.

##### Listing 12-9. ExprParser.fsy: parsing the concrete syntax for algebraic expressions

```F#
%{
open System
open Symbolic.Expressions
%}

%token <int> INT
%token <float> FLOAT
%token <string> ID
%token EOF LPAREN RPAREN PLUS MINUS TIMES DIV HAT SIN COS E

%left ID
%left prec_negate
%left LPAREN
%left PLUS MINUS
%left TIMES DIV
%left HAT

%start expr
%type <Expr> expr
%%
expr:
    | exp EOF { $1 }
number:
    | INT                           { Convert.ToDecimal($1) }
    | FLOAT                         { Convert.ToDecimal($1) }
    | MINUS INT %prec prec_negate   { Convert.ToDecimal(-$2) }
    | MINUS FLOAT %prec prec_negate { Convert.ToDecimal(-$2) }
exp:
    | number { Num $1 }
    | ID { Var $1 }
    | exp PLUS exp { Add [$1; $3] }
    | exp MINUS exp { Sub ($1, [$3]) }
    | exp TIMES exp { Prod ($1, $3) }
    | exp DIV exp { Frac ($1, $3) }
    | SIN LPAREN exp RPAREN   { Sin $3 }
    | COS LPAREN exp RPAREN   { Cos $3 }
    | E HAT exp { Exp $3 }
    | term { $1 }
    | exp HAT number { Pow ($1, $3) }
    | LPAREN exp RPAREN { $2 }
    | MINUS LPAREN exp RPAREN { Neg $3 }
term:
    | number ID { Prod (Num $1, Var $2) }
    | number ID HAT number    { Prod (Num $1, Pow (Var $2, $4)) }
    | ID HAT number { Prod (Num 1M, Pow (Var $1, $3)) }
```

## Simplifying Algebraic Expressions

At the start of this chapter, you simplified expressions using local techniques, but you also saw the limitations of this approach. Listing 12-10 shows a more complete implementation of a separate function (`Simplify`) that performs some nonlocal simplifications as well. Both this function and the one for derivation shown in the subsequent section are placed in a separate file (`ExprUtil.fs`).

`Simplify` uses two helper functions (`collect` and `negate`). The former collects constants from products using a bottom-up strategy that reduces constant subproducts and factors out constants by bringing them outward (to the left). Recall that product terms are binary.

##### Listing 12-10. ExprUtils.fs: simplifying algebraic expressions

```F#
module Symbolic.Expressions.Utils
open Symbolic.Expressions

/// A helper function to map/select across a list while threading state
/// through the computation
let collectFold f l s =
    let l,s2 = (s, l) ||>  List.mapFold (fun z x -> f x z)
    List.concat l,s2

/// Collect constants
let rec collect e =
    match e with
    | Prod (e1, e2) ->
        match collect e1, collect e2 with
        | Num n1, Num n2       -> Num (n1 * n2)
        | Num n1, Prod (Num n2, e)
        | Prod (Num n2, e), Num n1 -> Prod (Num (n1 * n2), e)
        | Num n, e | e, Num n      -> Prod (Num n, e)
        | Prod (Num n1, e1), Prod (Num n2, e2) ->
            Prod (Num (n1 * n2), Prod (e1, e2))
        | e1', e2'                 -> Prod (e1', e2')
    | Num _ | Var _ as e   -> e
    | Neg e -> Neg (collect e)
    | Add exprs -> Add (List.map collect exprs)
    | Sub (e1, exprs)      -> Sub (collect e1, List.map collect exprs)
    | Frac (e1, e2) -> Frac (collect e1, collect e2)
    | Pow (e1, n) -> Pow (collect e1, n)
    | Sin e -> Sin (collect e)
    | Cos e -> Cos (collect e)
    | Exp _ as e -> e

/// Push negations through an expression
let rec negate e =
    match e with
    | Num n -> Num (-n)
    | Var v as exp      -> Neg exp
    | Neg e -> e
    | Add exprs -> Add (List.map negate exprs)
    | Sub _ -> failwith "unexpected Sub"
    | Prod (e1, e2)     -> Prod (negate e1, e2)
    | Frac (e1, e2)     -> Frac (negate e1, e2)
    | exp -> Neg exp

let filterNums (e:Expr) n =
    if e.IsNumber
    then [], n + e.NumOf
    else [e], n

let summands e =
    match e with
    | Add es -> es
    | e -> [e]

/// Simplify an expression
let rec simp e =
    match e with
    | Num n -> Num n
    | Var v -> Var v
    | Neg e -> negate (simp e)
    | Add exprs ->
        let exprs2, n =
            (exprs, 0M) ||> collectFold (simp >> summands >> collectFold filterNums)
        match exprs2 with
        | [] -> Num n
        | [e] when n = 0M -> e
        | _ when n = 0M -> Add exprs2
        | _ -> Add (exprs2 @ [Num n])
    | Sub (e1, exprs) -> simp (Add (e1 :: List.map Neg exprs))
    | Prod (e1, e2) ->
        match simp e1, simp e2 with
        | Num 0M, _ | _, Num 0M -> Num 0M
        | Num 1M, e | e, Num 1M -> e
        | Num n1, Num n2 -> Num (n1 * n2)
        | e1, e2 -> Prod (e1, e2)
    | Frac (e1, e2) ->
        match simp e1, simp e2 with
        | Num 0M, _ -> Num 0M
        | e1, Num 1M -> e1
        | Num n, Frac (Num n2, e) -> Prod (Frac (Num n, Num n2), e)
        | Num n, Frac (e, Num n2) -> Frac (Prod (Num n, Num n2), e)
        | e1, e2 -> Frac (e1, e2)
    | Pow (e, 1M) -> simp e
    | Pow (e, n) -> Pow (simp e, n)
    | Sin e -> Sin (simp e)
    | Cos e -> Cos (simp e)
    | Exp e -> Exp (simp e)

let simplify e = e |> simp |> simp |> collect
```

The main simplification algorithm works as follows:

* Constants and variables are passed through verbatim. You use `negate` when simplifying a negation, which assumes the expression at hand no longer contains differences and that sums were flattened (see the next item in this list).

* Sums are traversed and nested sums are flattened; at the same time all constants are collected and added up. This reduces the complexity of further simplification considerably.

* Differences are converted to sums: for instance, `A-B-C` is converted to `A+(-B)+(-C)`. Thus, the first element is preserved without negation.

* When simplifying a product, you first simplify its factors, and then you remove identity operations (multiplying by zero or one) and reduce products of constants.

* Fractions are handled similarly. Zero divided by anything is 0, anything divided by 1 is itself, and multiline fractions can be collapsed if you find numeric denominators or numerators.

* The rest of the match cases deal with simplifying subexpressions.

## Symbolic Differentiation of Algebraic Expressions

Applying symbolic differentiation is a straightforward translation of the mathematical rules of differentiation into code. You could use local functions that act as constructors and perform local simplifications, but with the simplification function described earlier, this isn't needed. Listing 12-11 shows the implementation of symbolic differentiation for the `Expr` type. Note how beautifully and succinctly the code follows the math behind it: the essence of the symbolic processing is merely 20 lines of code!

##### Listing 12-11. ExprUtil.fs (continued): symbolic fifferentiation for algebraic expressions

```F#
let rec diff v e =
    match e with
    | Num _ -> Num 0M
    | Var v2 when v2=v -> Num 1M
    | Var _ -> Num 0M
    | Neg e -> diff v (Prod ((Num -1M), e))
    | Add exprs -> Add (List.map (diff v) exprs)
    | Sub (e1, exprs)  -> Sub (diff v e1, List.map (diff v) exprs)
    | Prod (e1, e2) -> Add [Prod (diff v e1, e2); Prod (e1, diff v e2)]
    | Frac (e1, e2) -> Frac (Sub (Prod (diff v e1, e2), [Prod (e1, diff v e2)]),Pow (e2, 2M))
    | Pow (e1, n) -> Prod (Prod (Num n, Pow (e1, n - 1M)), diff v e1)
    | Sin e -> Prod (Cos e, diff v e)
    | Cos e -> Neg (Prod (Sin e, diff v e))
    | Exp (Var v2) as e when v2=v  -> e
    | Exp (Var v2) -> Num 0M
    | Exp e -> Prod (Exp e, diff v e) 
```

## The Driver

Listing 12-12 is the next piece: the command-line driver (`main.fsx`). It reads lines from the input, tokenizes them, parses them, and applies the symbolic simplification and differentiation functions.

##### Listing 12-12. Main.fsx: the driver code for the symbolic differentiation application

```F#
#r "packages/FsLexYacc.Runtime/lib/net40/FsLexYacc.Runtime.dll"
#load "Expr.fs" "ExprParser.fs" "ExprLexer.fs" "ExprUtils.fs"

open System
open Symbolic.Expressions
open Microsoft.FSharp.Text.Lexing

let ProcessOneLine text =
    let lex = LexBuffer<char>.FromString text
    let e1 = ExprParser.expr ExprLexer.main lex
    printfn "After parsing: %A" e1
    let e2 = Utils.simplify e1
    printfn "After simplifying: %A" e2
    let e3 = Utils.diff "x" e2
    printfn "After differentiating: %A" e3
    let e4 = Utils.simplify e3
    printfn "After simplifying: %A" e4
let main () =
    while true do
        let text = Console.ReadLine()
        try
            ProcessOneLine text
        with e -> printfn "Error: %A" e
main()
```

## The Web API

Listing 12-13 is the final piece: the symbolic analysis delivered as a web server using the script `server.fsx`. It does much the same thing as the command-line interface but uses **Suave** (see Chapter 2) to process requests:

##### Listing 12-13. server.fsx: the symbolic differentiation application as a simplistic web API

```F#
#r "packages/FsLexYacc.Runtime/lib/net40/FsLexYacc.Runtime.dll"
#r "packages/Suave/lib/net40/Suave.dll"
#load "Expr.fs" "ExprParser.fs" "ExprLexer.fs" "ExprUtils.fs"

open Symbolic.Expressions
open Symbolic.Expressions.ExprUtils
open Microsoft.FSharp.Text.Lexing
open Suave
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Web

let parse text =
    let lex = LexBuffer<char>.FromString text
    ExprParser.expr ExprLexer.main lex

let toJson x =
    OK (sprintf """{ "result": "%A" }""" x)
    >>= Writers.setMimeType "application/json"

let webServerSpec () =
  choose
    [ path "/" >>= OK "Welcome to the analyzer"
      pathScan "/simp/%s" (parse >> simp >> toJson)
      pathScan "/diff/%s" (parse >> diff "x" >> toJson)
      pathScan "/diffsimp/%s" (parse >> diff "x" >> simp >> toJson)
      pathScan "/parse/%s" (parse >> toJson) ]
startWebServer defaultConfig (webServerSpec())
```

After running this code in F# Interactive, you will see the website running the following:

```F#
[I] ...listener started ...with binding 127.0.0.1:8083 [Suave.Tcp.tcpIpServer]
```

Once running, you can make requests using URLs such as the following:

```F#
http://localhost:8083/simp/x+0
http://localhost:8083/diff/x*x
```

For each of these, you will get a JSON file served as a result that contains a textual form of the result of the parse. For example

```F#
http://localhost:8083/diff/x+x+x+x
```

performs differentiation, returning

```json
{ "result": "Add [Add [Add [Num 1M; Num 1M]; Num 1M]; Num 1M]" }
```

and

```F#
http://localhost:8083/diffsimp/x+x+x+x
```

performs both differentiation and simplification, returning

```json
{ "result": "Num 4M" }
```

We leave it as an exercise for the reader to format the expression result as more idiomatic JSON content. To recap, in this example you've seen the following:

* Two abstract syntax representations for different classes of algebraic expressions: one simple, and one more realistic

* How to implement simplification and symbolic differentiation routines on these representations of algebraic expressions

* How to implement parsing and lexing for concrete representations of algebraic expressions

* How to put this together into both a final console application and prototype web service

# 编程

新建Visual F# 类库，命名：AlgebraicExpressions

```
install-package fslexyacc
```

删除新建项目中的占位示例文件。

添加文件：Listing 12-7. Expr.fs

添加文件：Listing 12-8. ExprLexer.fsl

添加文件：Listing 12-9. ExprParser.fsy

从解决方案资源管理器中，依赖项，获取`FsLexYacc`库所在的位置：

```
C:\Users\cuishengli\.nuget\packages\fslexyacc\9.0.2
```

在项目目录下执行命令：

```bash
>"C:\Users\cuishengli\.nuget\packages\fslexyacc\9.0.2\build\fsyacc\net46\fsyacc.exe" ExprParser.fsy --module Symbolic.Expressions.ExprParser
>"C:\Users\cuishengli\.nuget\packages\fslexyacc\9.0.2\build\fslex\net46\fslex.exe" ExprLexer.fsl --unicode
```

也可以将其保存在`fsyacc.cmd`和`fslex.cmd`中，双击或在命令行窗口键入文件名执行。

执行完命令后，目录下会生成两个文件`ExprParser.fs`、`ExprLexer.fs`。添加这两个现有项到项目。

对于生成的词法文件，语法文件匹配下面模式的行用于跟踪`.fsy`和`.fsl`中的错误，当编译通过后，可以手动用`//$0`注释掉：

```
^# \d+\b
```

继续添加文件，以完成功能。

添加`ExprUtils.fs`文件：Listing 12-10. ExprUtils.fs，Listing 12-11. ExprUtil.fs

新建测试项目：

添加对主项目的引用，添加对runtime的nuget包。下面是`Tester.fs`测试文件：

```F#
namespace Symbolic.Expressions

open Xunit
open Xunit.Abstractions
open FSharp.Text.Lexing

type Tests(output : ITestOutputHelper) =
    let parse text =
        let lex = LexBuffer<char>.FromString text
        ExprParser.expr ExprLexer.main lex

    let ProcessOneLine text =
        let e1 = parse text

        output.WriteLine <| sprintf "After parsing: %A" e1

        let e2 = Utils.simplify e1
        output.WriteLine <| sprintf "After simplifying: %A" e2

        let e3 = Utils.diff "x" e2
        output.WriteLine <| sprintf "After differentiating: %A" e3

        let e4 = Utils.simplify e3
        output.WriteLine <| sprintf "After simplifying: %A" e4

    [<Fact>]
    member this.exec() =
        ProcessOneLine "x+0"
```

