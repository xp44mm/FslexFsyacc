## Some Recursive Descent Parsing

Sometimes, you want to tokenize and parse a nonstandard language format, such as XML or JSON. The typical task is to parse the user input into your internal representation by breaking down the input string into a sequence of tokens (“lexing”) and then constructing an instance of your internal representation based on a grammar (“parsing”). Lexing and parsing don't have to be separated, and there are often convenient .NET methods for extracting information from text in particular formats, as shown in this chapter. Nevertheless, it's often best to treat the two processes separately.

In this section, you will implement a simple tokenizer and parser for a language of polynomial expressions for inputted text fragments, such as

```
x^5 - 2x^3 + 20
```

or

```
x + 3
```

The aim is simply to produce a structured value that represents the polynomial to permit subsequent processing. For example, this may be necessary when writing an application that performs simple symbolic differentiation—say, on polynomials only. You want to read polynomials, such as `x^5 - 2x^3 + 20`, as input from your users, which in turn is converted to your internal polynomial representation so that you can perform symbolic differentiation and pretty-print the result to the screen. One way to represent polynomials is as a list of terms that are added or subtracted to form the polynomial:

```F#
type Term =
    | Term  of int * string * int
    | Const of int
type Polynomial = Term list
```

For instance, the polynomial `x^5 – 2x^3 + 20` is represented as:

```F#
[Term (1,"x",5); Term (-2,"x",3); Const 20]
```

### A Simple Tokenizer

First, you implement a tokenizer for the input, using regular expressions. See Listing 8-2.

##### Listing 8-2. Tokenizer for polynomials using regular expressions

```F#
type Token =
    | ID of string
    | INT of int
    | HAT
    | PLUS
    | MINUS
let regex s = new System.Text.RegularExpressions.Regex(s)
let tokenR = regex @"((?<token>(\d+|\w+|\^|\+|-))\s*)*"

let tokenize (s : string) =
    [for x in tokenR.Match(s).Groups.["token"].Captures do
        let token =
            match x.Value with
            | "^" -> HAT
            | "-" -> MINUS
            | "+" -> PLUS
            | s when System.Char.IsDigit s.[0] -> INT (int s)
            | s -> ID s
        yield token]
```

The inferred type of the function is:

```F#
val tokenize : s:string -> Token list
```

We can now test the tokenizer on some sample inputs:

```F#
> tokenize "x^5 - 2x^3 + 20";;
val it : Token list =
    [ID "x"; HAT; INT 5; MINUS; INT 2; ID "x"; HAT; INT 3; PLUS; INT 20]
```

The tokenizer works by simply matching the entire input string, and for each text captured by the labeled “token” pattern, we yield an appropriate token depending on the captured text. 

### Recursive-Descent Parsing

You can now turn your attention to parsing. In Listing 8-2, you built a lexer and a token type suitable for generating a token stream for the inputted text (shown as a list of tokens here):

```F#
[ID "x"; HAT; INT 5; MINUS; INT 2; ID "x"; HAT; INT 3; PLUS; INT 20]
```

Listing 8-3 shows a recursive-descent parser that consumes this token stream and converts it into the internal representation of polynomials. The parser works by generating a list for the token stream. 

##### Listing 8-3.  Recursive-descent parser for polynomials

```F#
type Term =
    | Term of int * string * int
    | Const of int

type Polynomial = Term list

type TokenStream = Token list

let tryToken (src : TokenStream) =
    match src with
    | tok :: rest -> Some(tok, rest)
    | _ -> None

let parseIndex src =
    match tryToken src with
    | Some (HAT, src) ->
        match tryToken src with
        | Some (INT num2, src) ->
            num2, src
        | _ -> failwith "expected an integer after '^'"
    | _ -> 1, src

let parseTerm src =
    match tryToken src with
    | Some (INT num, src) ->
        match tryToken src with
        | Some (ID id, src) ->
            let idx, src = parseIndex src
            Term (num, id, idx), src
        | _ -> Const num, src
    | Some (ID id, src) ->
        let idx, src = parseIndex src
        Term(1, id, idx), src
    | _ -> failwith "end of token stream in term"

let rec parsePolynomial src =
    let t1, src = parseTerm src
    match tryToken src with
    | Some (PLUS, src) ->
        let p2, src = parsePolynomial src
        (t1 :: p2), src
    | _ -> [t1], src

let parse input =
    let src = tokenize input
    let result, src = parsePolynomial src
    match tryToken src with
    | Some _ -> failwith "unexpected input at end of token stream!"
    | None -> result
```

The functions here have these types (using the type aliases you defined):

```F#
val tryToken : src:TokenStream -> (Token * Token list) option
val parseIndex : src:TokenStream -> int * Token list
val parseTerm : src:TokenStream -> Term * Token list
val parsePolynomial : src:TokenStream -> Term list * Token list
val parse : input:string -> Term list
```

Note in the previous examples that you can successfully parse either constants or complete terms, but after you locate a `HAT` symbol, a number must follow. This sort of parsing, in which you look only at the next token to guide the parsing process, is referred to as *LL(1)*, which stands for left-to-right, leftmost derivation parsing; 1 means that only one look-ahead symbol is used. To conclude, you can look at the parse function in action:

```F#
> parse "1+3";;
val it : Term list = [Const 1; Const 3]

> parse "2x^2+3x+5";;
val it : Term list = [Term (2,"x",2); Term (3,"x",1); Const 5]
```


