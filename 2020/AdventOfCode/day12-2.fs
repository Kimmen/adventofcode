module day12_2

  
    
    type Position = { vertical: int; horizontal: int }
    type Waypoint = Position
    type Ship = { position: Position; waypoint: Waypoint }

    let rotateWaypoint (waypoint: Position) (value: int) : Position =
        let rotation = (360 + value) % 360
        match rotation with
        | 0 -> waypoint
        | 90 -> { horizontal = -waypoint.vertical; vertical = waypoint.horizontal }
        | 180 -> { horizontal = -waypoint.horizontal; vertical = -waypoint.vertical }
        | 270 -> { horizontal = waypoint.vertical; vertical = -waypoint.horizontal }
        | _ -> failwith "wut rotation?"

    let moveForward (ship: Ship) (factor: int) : Position = 
        { horizontal = ship.position.horizontal + (ship.waypoint.horizontal * factor); vertical = ship.position.vertical + (ship.waypoint.vertical * factor) }
        

    let runAction (ship: Ship) (action : char, value: int) : Ship = 
        match action with
        | 'N' -> { ship with waypoint = { ship.waypoint with vertical = ship.waypoint.vertical + value  } }
        | 'S' -> { ship with waypoint = { ship.waypoint with vertical = ship.waypoint.vertical - value  } }
        | 'E' -> { ship with waypoint = { ship.waypoint with horizontal = ship.waypoint.horizontal + value  } }
        | 'W' -> { ship with waypoint = { ship.waypoint with horizontal = ship.waypoint.horizontal - value  } }
        | 'L' -> { ship with waypoint = rotateWaypoint ship.waypoint value}
        | 'R' -> { ship with waypoint = rotateWaypoint ship.waypoint -value }
        | 'F' -> { ship with position = moveForward ship value }
        | _ -> failwith "wut action?"

    let traverse (inital: Waypoint) (actions: (char*int) seq) : Ship = 
        actions |> Seq.fold(runAction) { position = {horizontal = 0; vertical = 0 }; waypoint = inital}

    let calcManhattanDistance (ship: Ship) : int =
        let pos = ship.position
        (pos.horizontal |> abs) + (pos.vertical |> abs)

    let run =
        "input12.txt" 
        |> Utils.readLines 
        |> Seq.map(fun l -> 
            let action = l.[0];
            let value = l.Substring(1) |> int
            (action, value))
        |> traverse { horizontal = 10; vertical = 1 }
        |> calcManhattanDistance  
