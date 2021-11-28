module FslexFsyacc.Lex.RegularExpressionNFA

/// Algorithm 3.23: The McNaughton-Yamada-Thompson algorithm to convert a regular expression to an NFA.
let rec convertToNFA i = function
    | Character r ->
        let f = i + 1u
        {
            transition = set [i,Some r, f]
            minState = i
            maxState = f
        }
    | Uion (r1,r2) ->
        let n1 = convertToNFA (i+1u) r1
        let n2 = convertToNFA (n1.maxState+1u) r2
        NFACombine.union i n1 n2

    | Concat(r1,r2) ->
        let n1 = convertToNFA i r1
        let n2 = convertToNFA (n1.maxState) r2
        NFACombine.concat n1 n2

    | Natural r ->
        let n = convertToNFA (i+1u) r
        NFACombine.natural i n
    | Positive r ->
        let n = convertToNFA i r
        NFACombine.positive n

    | Maybe r ->
        let n = convertToNFA (i+1u) r
        NFACombine.maybe i n

    | abnormal -> failwithf "%A" abnormal

