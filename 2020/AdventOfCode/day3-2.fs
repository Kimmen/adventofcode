module day3_2

type Position = { x : int; y : int}

let run =  
    let filePath = "input3.txt"
   
    let inputEntries = filePath |> Utils.readLines

    let countTrees (traverser: Position -> Position) = 
        let start = {x = 0; y = 0}

        let countTree (pos: Position) (line:string) = 
            let spot = line.[pos.x % line.Length]
            if spot = '#' then 1 else 0

        inputEntries |> Seq.skip 1 |> Seq.fold (fun (count, prev, target) line -> 
            let curr = { prev with x = target.x; y = prev.y + 1 }

            match curr with
            | { y = y } when y = target.y -> (count + countTree curr line, curr, traverser curr) 
            | _ ->  (count, curr, target)
            ) (0, start, traverser start)

    let (count1, _, _) = countTrees (fun p -> { x = p.x + 1; y = p.y + 1  })
    let (count2, _, _) = countTrees (fun p -> { x = p.x + 3; y = p.y + 1  })
    let (count3, _, _) = countTrees (fun p -> { x = p.x + 5; y = p.y + 1  })
    let (count4, _, _) = countTrees (fun p -> { x = p.x + 7; y = p.y + 1  })
    let (count5, _, _) = countTrees (fun p -> { x = p.x + 1; y = p.y + 2  })

    (count1, count2, count3, count4, count5, (count1 * count2 * count3 * count4 * count5))