module day13_2

let inSequence (x: uint64) (ids: (uint64 * uint64) list) = 
    ids |> Seq.forall (fun (i, b) -> 
        ((x + i) % b) = 0UL)

let findNextStopTime timestamp offset inc busId = 
    let mutable curr = timestamp
    while ((curr + offset) % busId) <> 0UL do   
        curr <- curr + inc
        
    curr

let run =
    let input = 
        "input13.txt" 
        |> Utils.readLines
        |> List.ofSeq

    let busIds = 
        input.[1].Split ',' 
        |> Seq.mapi (fun i x -> (i, x))
        |> Seq.where (fun (_, x) -> x <> "x")
        |> Seq.map (fun (i, x) -> (i |> uint64, x |> uint64))
        |> Seq.toList

    let (_, timestamp) = 
        busIds
        |> Seq.fold(fun (inc, timestamp) (offset, busId) ->
            let nextInc = busId * inc
            let nextTimestamp = findNextStopTime timestamp offset inc busId
            (nextInc, nextTimestamp)
        )(1UL, 100000000000000UL) 
            

    //let mutable timestamp = 0 //100000000000000UL
    //let mutable con = true
    //while con do
    //    timestamp <- timestamp + 1UL
    //    con <- (not (inSequence timestamp busIds) && timestamp < 200000000000002UL)

        

    timestamp
    
        

