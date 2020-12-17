module day3_1

open System.IO;

type Position = {x: int; y: int}

let run =  
    let filePath = "input3.txt"
   
    let inputEntries = filePath |> Utils.readLines

    let traverseX width x = (x + 3) % width
    let traverseY y = y + 1

    inputEntries |> Seq.fold (fun (x, y, treeCount) line -> 
        let nextCount = 
            match (y, line.[x]) with
            | (0, _) -> treeCount
            | (_, '#') -> treeCount + 1
            | _ -> treeCount
        (x |> traverseX line.Length, y |> traverseY, nextCount)) (0, 0, 0)