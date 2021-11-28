namespace FslexFsyacc.Runtime

open System.Collections.Generic

///解析带数据的对象
type LexicalAnalyzer<'tok,'u>
    (
        nextStates:Map<uint32,Map<string,uint32>>,
        lexemesFromFinal:Map<uint32,Set<uint32>>,
        universalFinals:Set<uint32>,
        indicesFromFinal:Map<uint32,int>,
        mappers: ('tok list -> 'u) []
    ) =
    let tagAnalyzer = 
        TagLexicalAnalyzer (
            nextStates, 
            lexemesFromFinal, 
            universalFinals, 
            indicesFromFinal)

    member this.split(inputs:seq<'tok>, getTag:'tok -> string) =
        let inputBuffer = Queue<'tok>()

        let rec takeFromBuffer len acc =
            if len = 0 then
                acc |> List.rev
            else
                let x = inputBuffer.Dequeue()
                takeFromBuffer (len-1) (x::acc)

        inputs
        |> Seq.map(fun tok -> 
            inputBuffer.Enqueue(tok)
            getTag tok)
        |> tagAnalyzer.simulate
        |> Seq.map(fun (si,ls)->
            let tokens = takeFromBuffer ls.Length []
            //si,tokens
            mappers.[si] tokens
        )
        
