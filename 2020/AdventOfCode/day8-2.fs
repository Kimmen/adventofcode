module day8_2

open System.Collections.Generic

type Instruction = { operation: string; steps: int }
type ExecutionResult = { acc: int; currentIndex: int; success: bool }

let buildInstruction (line:string) = 
    let parts = line.Split(' ') 
    { operation = parts.[0]; steps = parts.[1] |> int } 


let traverse (instructions : List<Instruction>) =
    let rec traverseCore (i: int) (acc: int) (visited: Set<int>) : ExecutionResult =
        let result = { acc = acc; currentIndex = i; success = false }
        let updatedVisited = visited.Add i
        match i with
        | x when instructions.Count = x -> { result with success = true }
        | x when visited.Contains(x) -> result
        | _ -> 
            match instructions.[i] with
            | { operation = "nop" } ->  traverseCore (i + 1) acc updatedVisited
            | { operation = "acc"; steps = x } -> traverseCore (i + 1) (acc + x) updatedVisited
            | { operation = "jmp"; steps = x } -> traverseCore (i + x) acc updatedVisited
            | _ -> failwith "Unhandled operation"


    let tryChange index oldOperation newOperation = 
        instructions.[index] <- { instructions.[index] with operation = newOperation }
        match traverseCore 0 0 Set.empty with
        | { success = true; acc = x } -> Some (x)
        | _ -> 
            instructions.[index] <- { instructions.[index] with operation = oldOperation }
            None
    
    [0..instructions.Count - 1]
        |> List.pick(fun i -> 
            match instructions.[i] with
            | { operation = "nop" } -> tryChange i "nop" "jmp"
            | { operation = "jmp" } -> tryChange i "jmp" "nop"  
            | _ -> None)

let run =
    let entries =
        "input8.txt" 
        |> Utils.readLines 
        |> Seq.map(buildInstruction)

    traverse (new List<Instruction>(entries))