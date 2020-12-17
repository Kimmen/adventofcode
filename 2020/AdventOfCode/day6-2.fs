module day6_2

let poplateAnswers (answers:Set<char> list) = 
    answers |> Set.intersectMany

let processLine (ongoingGroups: Set<char> list, processedGroups: Set<char> list) line =
    let possibleAnswersPattern = @"^[a-z]+$"
    match line with 
    | "" -> ([], (poplateAnswers ongoingGroups) :: processedGroups)
    | Utils.Regex possibleAnswersPattern _ -> ( (line.ToCharArray() |> Set.ofArray) :: ongoingGroups, processedGroups)
    | _ -> failwith (sprintf "Unexpected line %s" line)

let run =
    let entries =
        "input6.txt" |> Utils.readLines |> Seq.toList

    let (last, answers) = entries |> List.fold(processLine) ([], [])
    (poplateAnswers last) :: answers
        |> List.map(fun x -> x |> Set.count)
        |> List.sum