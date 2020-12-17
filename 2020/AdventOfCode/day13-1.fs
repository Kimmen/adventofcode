module day13_1
    let run =
        let input = 
            "input13.txt" 
            |> Utils.readLines
            |> List.ofSeq

        let timestamp = input.[0] |> int
        let (id, minLeft) = 
            input.[1].Split ',' 
            |> Seq.where (fun x -> x <> "x")
            |> Seq.map (fun x -> x |> int)
            |> Seq.map (fun x -> (x, (x - (timestamp % x))))
            |> Seq.sortBy (fun (_, minLeft) -> minLeft)
            |> Seq.head

        id * minLeft


