module day5_1

type Range = { lower: int; higher: int }

let calcMidpoint range = (range.higher - range.lower) / 2 + range.lower

let traverseRow row direction =

    match direction with
    | 'F' -> { row with higher = calcMidpoint row }
    | 'B' -> { row with lower = calcMidpoint row + 1 }
    | _ -> row

let traverseColumn column direction =
    match direction with
    | 'R' ->
        { column with
              lower = calcMidpoint column + 1 }
    | 'L' ->
        { column with
              higher = calcMidpoint column }
    | _ -> column

let calculateSeatId row column = row.higher * 8 + column.higher

let run =
    let entries =
        "input5.txt" |> Utils.readLines |> Seq.toList

    entries
    |> List.map (fun line ->
        let rIn = line.[0..6]
        let sIn = line.[7..9]

        let row =
            rIn.ToCharArray()
            |> Array.fold (fun r d -> traverseRow r d) { lower = 0; higher = 127 }

        let column =
            sIn.ToCharArray()
            |> Array.fold (fun c d -> traverseColumn c d) { lower = 0; higher = 7 }

        if row.higher <> row.lower then failwith "row not complete"
        if column.higher <> column.lower then failwith "column not complete"

        calculateSeatId row column)
    |> List.max

