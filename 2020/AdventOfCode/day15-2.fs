module day15_2

    type PrevTurns = { last: int; spokenValues: Map<int, int> }

    let computeTurn (prevTurns: PrevTurns) (turn: int) = 
        let last = prevTurns.last
        let spokenValues = prevTurns.spokenValues

        let current = if spokenValues.ContainsKey(last) then turn - (spokenValues.[last]) else 0
        { last = current; spokenValues = spokenValues.Add(last, turn) }

    let run =
        let maxTurns = 30000000 - 1 // as we the last result will be in the "last" value and not in the spoken values list.
        let startNumbers = [| 16;12;1;0;15;7 |] 
        let computetTurns = startNumbers |> Seq.mapi(fun i v -> (v, i + 1)) |> Map.ofSeq 
  
        let calculatedTurns = 
            [startNumbers.Length + 1..maxTurns]
            |> Seq.fold(computeTurn) { spokenValues = computetTurns; last = 11}
            
    
        calculatedTurns.last