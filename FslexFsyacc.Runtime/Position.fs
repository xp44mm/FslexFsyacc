namespace FslexFsyacc.Runtime

type Position<'value> =
    {
        index :int
        length:int
        value :'value
    }

    member this.nextIndex = 
        this.index + this.length

    member this.raw(source:string)=
        source.Substring(this.index,this.length)

    member this.rest(source:string) = 
        source.Substring(this.nextIndex)

    // Location : Line,Column