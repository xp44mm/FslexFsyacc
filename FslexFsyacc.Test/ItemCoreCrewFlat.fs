namespace FslexFsyacc

type ItemCoreCrewFlat = 
    {
        production:string list
        leftside  :string
        body      :string list
        dot:int
        backwards:string list
        forwards:string list
        dotmax:bool
        isKernel:bool
    }

    static member from(crew:FslexFsyacc.Yacc.ItemCoreCrew) = 
        {
            production = crew.production
            leftside   = crew.leftside  
            body       = crew.body      
            dot        = crew.dot       
            backwards  = crew.backwards 
            forwards   = crew.forwards  
            dotmax     = crew.dotmax    
            isKernel   = crew.isKernel  
        }
