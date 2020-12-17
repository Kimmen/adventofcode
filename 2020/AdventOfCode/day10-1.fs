module day10_1
    type JoltCountResult = {ones : int; twos : int; threes: int}
    let countJolts (adapters: Set<uint64>) :  JoltCountResult = 
        let (_, res) = 
            adapters 
            |> Set.add (adapters.MaximumElement + 3UL)
            |> Set.fold(fun (jolt :uint64, res:JoltCountResult) (a: uint64) -> 
                match a - jolt with
                | 1UL -> (a, {res with ones = res.ones + 1})
                | 2UL -> (a, {res with twos = res.twos + 1})
                | 3UL -> (a, {res with threes = res.threes + 1})
                | _ -> failwith "wut?"
                ) (0UL, {ones = 0; twos = 0; threes = 0}) 

        res
        
    
    let run =
        let entries =
            "input10.txt" 
            |> Utils.readLines 
            |> Seq.map(fun x -> x |> uint64)
            |> Set.ofSeq


        let res = countJolts entries
        res.ones * res.threes