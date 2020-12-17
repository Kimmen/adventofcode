module day9_1

    let findSumInRange (start: int) (count: int) (sum: uint64) (values: uint64 list) =
        let availableTerms = 
            values
            |> List.skip start
            |> List.take count 
            |> Set.ofList

        availableTerms
        |> Set.toSeq
        |> Seq.exists(fun x -> availableTerms.Contains(sum - x))

    let findNonSum (values: uint64 list) (preamble: int) =       
        [0..values.Length - 1]
        |> Seq.pick(fun index ->
            let targetIndex = index + preamble
            if targetIndex >= values.Length 
            then None
            else 
                let sum = values.[targetIndex]
                match findSumInRange index (preamble) sum values with
                | true -> None
                | false -> Some(sum) )
        
    let run =
        let entries =
            "input9.txt" 
            |> Utils.readLines 
            |> Seq.map(fun x -> x |> uint64)
            |> Seq.toList

        let preamble = 25 //5 if test
        findNonSum entries preamble