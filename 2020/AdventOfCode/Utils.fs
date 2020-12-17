module Utils

open System.Text.RegularExpressions

let readLines filePath = System.IO.File.ReadLines(filePath)

let (|Regex|_|) pattern input =
    let m = Regex.Match(input, pattern)
    if m.Success
    then Some(List.tail [ for g in m.Groups -> g.Value ])
    else None


let count x xs =
    xs |> Seq.filter (fun x' -> x' = x) |> Seq.length


let areSame (a: char [,]) (b: char[,]) = 
    seq { for r in 0 .. (a |> Array2D.length1) - 1 do
            for c in 0 .. (a |> Array2D.length2) - 1 do
               yield a.[r, c] = b.[r, c] }
    |> Seq.forall id

let print (a: char [,]) = 
    for r in 0 .. (a |> Array2D.length1) - 1 do
        for c in 0 .. (a |> Array2D.length2) - 1 do
            printf "%c" a.[r, c]
        printfn ""
    printfn ""