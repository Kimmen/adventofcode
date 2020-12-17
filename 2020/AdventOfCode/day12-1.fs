module day12_1
    
    type Direction = 
        | North
        | East
        | South
        | West
    
    type Position = { direction: Direction; horizontal: int; vertical: int }

    let calcNewDirection (pos: Position) (value: int) : Direction =
        let dirValue = 
            match pos.direction with
            | East -> 0
            | North -> 90
            | West -> 180
            | South -> 270

        let newDirValue = (360 + dirValue + value) % 360

        match newDirValue with
        | 0 -> East
        | 90 -> North
        | 180  -> West 
        | 270 -> South
        | _ -> failwith "wut direction?"

    let moveForward (pos: Position) (value: int) : Position = 
        match pos.direction with
        | East -> { pos with horizontal = pos.horizontal + value }
        | North -> { pos with vertical = pos.vertical + value }
        | West -> { pos with horizontal = pos.horizontal - value }
        | South -> { pos with vertical = pos.vertical - value } 

    let runAction (pos: Position) (action : char, value: int) : Position = 
        match action with
        | 'N' -> { pos with vertical = pos.vertical + value }
        | 'S' -> { pos with vertical = pos.vertical - value } 
        | 'E' -> { pos with horizontal = pos.horizontal + value }
        | 'W' -> { pos with horizontal = pos.horizontal - value }
        | 'L' -> { pos with direction = calcNewDirection pos value }
        | 'R' -> { pos with direction = calcNewDirection pos -value }
        | 'F' -> moveForward pos value
        | _ -> failwith "wut action?"

    let traverse (start: Direction) (actions: (char*int) seq) : Position = 
        actions |> Seq.fold(runAction) { direction = start; horizontal = 0; vertical = 0 }

    let calcManhattanDistance (pos: Position) : int =
        (pos.horizontal |> abs) + (pos.vertical |> abs)

    let run =
        "input12.txt" 
        |> Utils.readLines 
        |> Seq.map(fun l -> 
            let action = l.[0];
            let value = l.Substring(1) |> int
            (action, value))
        |> traverse Direction.East
        |> calcManhattanDistance  
