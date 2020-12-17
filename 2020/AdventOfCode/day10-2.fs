module day10_2

let countCombinations (adapters: Set<uint64>) = 
    let tryGet (n: uint64) : uint64 option = if adapters.Contains(n) then Some(n) else None

    let prepareConnectionsFor (destination: Map<uint64, uint64 list>) (a: uint64) = 
        let connectedAdapters = 
            [tryGet(a + 1UL); tryGet(a + 2UL); tryGet(a + 3UL)]
            |> List.where( fun x -> 
                match x with
                | None -> false
                | Some _ -> true)
            |> List.map( fun x -> x.Value)

        destination.Add(a, connectedAdapters)

    let allConnections = adapters |> Set.fold(prepareConnectionsFor) Map.empty
                   
    let countJunctionsAt (a: uint64) (destination: Map<uint64, uint64>) = 
        let connections =  allConnections.[a]
        match connections with
        | [] -> destination.Add(a, 1UL)
        | _ -> 
            let sum = connections |> List.map(fun c -> destination.[c]) |> List.sum
            destination.Add(a, sum)

    let counts = Set.foldBack(countJunctionsAt) adapters Map.empty 
    counts.[0UL]
    
let run =
    let entries =
        "input10.txt" 
        |> Utils.readLines 
        |> Seq.map(fun x -> x |> uint64)
        |> Set.ofSeq


    countCombinations (entries |> Set.add 0UL |> Set.add (entries.MaximumElement + 3UL))