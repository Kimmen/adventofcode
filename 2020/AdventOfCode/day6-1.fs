module day6_1

let poplateAnswers (line:string) (group:Set<char>) = line.ToCharArray() |> Array.fold(fun g c -> Set.add c g) group
 
let processLine (group: Set<char>, processedGroups: Set<char> list) line =
    let possibleAnswersPattern = @"^[a-z]+$"
    match line with 
    | "" -> (Set.empty, group :: processedGroups)
    | Utils.Regex possibleAnswersPattern _ -> ( poplateAnswers line group, processedGroups)
    | _ -> (group, processedGroups)

let run =
    let entries =
        "input6.txt" |> Utils.readLines |> Seq.toList

    let (last, answers) = entries |> List.fold(processLine) (Set.empty, [])
    last :: answers
        |> List.map(fun x -> x |> Set.count)
        |> List.sum
    
