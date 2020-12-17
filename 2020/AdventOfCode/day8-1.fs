module day8_1

type Instruction = { operation: string; steps: int }

let buildInstruction (line:string) = 
    let parts = line.Split(' ') 
    { operation = parts.[0]; steps = parts.[1] |> int } 


let traverse (instructions : Instruction list) =
    let rec traverseCore (i: int) (acc: int) (visited: Set<int>) : int =
        if visited.Contains(i) || instructions.Length <= i then acc
        else        
            match instructions.[i] with
            | { operation = "nop" } ->  traverseCore (i + 1) acc (visited.Add i)
            | { operation = "acc"; steps = x } -> traverseCore (i + 1) (acc + x) (visited.Add i)
            | { operation = "jmp"; steps = x } -> traverseCore (i + x) acc (visited.Add i)
            | _ -> acc

    traverseCore 0 0 Set.empty
let run =
    let entries =
        "input8.txt" 
        |> Utils.readLines 
        |> Seq.map(buildInstruction)
        |> Seq.toList

    traverse entries