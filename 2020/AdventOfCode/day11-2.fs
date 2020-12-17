module day11_2
    let getFirstSeatInDirection (row: int) (col:int) (rowDir: int) (colDir:int) (seats: char [,]) : char = 
        let maxRows = (Array2D.length1 seats) - 1
        let maxCols = (Array2D.length2 seats) - 1
    
        let rec traverse pos = 
            match pos with 
            | (r, _) when (r < 0 || r > maxRows) -> '.' 
            | (_, c) when (c < 0 || c > maxCols) -> '.' 
            | (r, c) -> 
                match seats.[r, c] with
                | '.' -> traverse ((r + rowDir), (c + colDir))
                | c -> c 

        traverse ((row + rowDir), (col + colDir))

    let hasOccupiedSeatsAdjacentTo (row: int) (col:int) (seats: char [,]) (predicate: int -> bool) : bool = 
        let noOccupiedAdjacentSeats = 
            [
                getFirstSeatInDirection row col -1 -1 seats;
                getFirstSeatInDirection row col -1 0 seats;
                getFirstSeatInDirection row col -1 1 seats;
                getFirstSeatInDirection row col 0 -1 seats;
                getFirstSeatInDirection row col 0 1 seats;
                getFirstSeatInDirection row col 1 -1 seats;
                getFirstSeatInDirection row col 1 0 seats;
                getFirstSeatInDirection row col 1 1 seats;] 
            |> List.where (fun s -> s = '#')
            |> List.length

        predicate(noOccupiedAdjacentSeats)

    let hasNoOccupiedSeatsAdjacentTo (row: int) (col:int) (seats: char [,]) =
        hasOccupiedSeatsAdjacentTo row col seats (fun (n: int) -> n = 0)
    

    let updateSeats (seats: char [,]) : char [,] = 
        Array2D.init (Array2D.length1 seats) (Array2D.length2 seats)
            (fun row col -> 
                match seats.[row, col] with
                | 'L' when hasNoOccupiedSeatsAdjacentTo row col seats -> '#'
                | '#' when hasOccupiedSeatsAdjacentTo row col seats (fun (n: int) -> n >= 5) -> 'L'
                | _ -> seats.[row, col])

    let run =
        let entries =
            "input11.txt" 
            |> Utils.readLines 
            |> Seq.map(fun l -> l.ToCharArray() |> List.ofArray)
            |> List.ofSeq
            |> array2D

        let mutable doContinue = true
        let mutable curr = entries
    
        while doContinue do 
            let seats = updateSeats curr
            //Utils.print seats
            doContinue <- not (Utils.areSame curr seats)
            curr <- seats

        let mutable occupiedSeats = 0
        curr |> Array2D.iter(fun x -> if x = '#' then occupiedSeats <- occupiedSeats + 1 else occupiedSeats <- occupiedSeats)

        occupiedSeats
        