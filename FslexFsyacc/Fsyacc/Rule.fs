namespace FslexFsyacc.Fsyacc

type Rule =
    {
        production: string list
        dummy:string
        reducer:string
    }

    static member just (production,dummy,reducer) =
        {
            production = production
            dummy = dummy
            reducer = reducer
        }


    static member augment (startSymbol) =
        {
            production = [""; startSymbol]
            dummy = ""
            reducer = ""
        }

