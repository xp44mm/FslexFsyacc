module FslexFsyacc.Runtime.Lex.RegularNFAUtils

/// Algorithm 3.23: The McNaughton-Yamada-Thompson algorithm to convert a regular expression to an NFA.
let rec fromRgx i = function
    | Atomic r ->
        let f = i + 1u
        {
            transition = set [i,Some r, f]
            minState = i
            maxState = f
        }
    | Either (r1,r2) ->
        let n1 = fromRgx (i+1u) r1
        let n2 = fromRgx (n1.maxState+1u) r2
        NFACombine.union i n1 n2

    | Both(r1,r2) ->
        let n1 = fromRgx i r1
        let n2 = fromRgx (n1.maxState) r2
        NFACombine.concat n1 n2

    | Natural r ->
        let n = fromRgx (i+1u) r
        NFACombine.natural i n
    | Plural r ->
        let n = fromRgx i r
        NFACombine.positive n

    | Optional r ->
        let n = fromRgx (i+1u) r
        NFACombine.maybe i n

    | abnormal -> failwithf "%A" abnormal

