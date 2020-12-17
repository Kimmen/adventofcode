module day9_2

    let findContiguousTerms (values: uint64 list) (sum: uint64) : Set<uint64> =
        let rec contiousTerms startIndex endIndex =
            if startIndex >= values.Length || endIndex >= values.Length 
            then Set.empty
            else 
                let contiguousSet = values |> List.skip startIndex |> List.take (endIndex - startIndex)
                let setSum = contiguousSet |> List.sum
                match setSum with
                | x when x = sum -> contiguousSet |> Set.ofList
                | x when x < sum -> contiousTerms startIndex (endIndex + 1) 
                | x when x > sum -> contiousTerms (startIndex + 1)  endIndex
                | _ -> failwith "wut?"

        contiousTerms 0 0
        

    let run =
        let entries =
            "input9.txt" 
            |> Utils.readLines 
            |> Seq.map(fun x -> x |> uint64)
            |> Seq.toList

        let terms = findContiguousTerms entries 675280050UL
        //let terms = findContiguousTerms entries 127UL

        terms.MinimumElement + terms.MaximumElement