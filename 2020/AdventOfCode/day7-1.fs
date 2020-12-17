module day7_1

type Bag = { count: int; name: string}

let normalizeLine (line: string) = 
    line.Replace(" bags", "").Replace(" bag", "").Replace(" contain ", "=").Replace(", ", ",").Replace(".", "")

let extractBagData (normalizedLine: string) : string * Bag list =
    let parts = normalizedLine.Split '='
    let containerBag = parts.[0]
    let contains = parts.[1]

    match contains with 
    | "no other" -> (containerBag, [])
    | x -> (containerBag, x.Split ',' 
        |> Seq.map (fun p -> 
            match p with 
            | Utils.Regex @"(\d+) ([a-z ]+)" b -> { count = b.[0] |> int; name = b.[1] }
            | _ -> failwith "Invalid inner bag description" )
        |> Seq.toList)

let buildMap (parentBagMap: Map<string, Bag list>) (outerBagName: string, containsBags: Bag list) : Map<string, Bag list> =
    let getParents (key: string) (m: Map<string, Bag list>) : Bag list = 
        match m.TryFind(key) with 
        | Some (bags) -> bags
        | None -> []

    containsBags 
        |> List.fold(fun (m: Map<string, Bag list>) (c: Bag) -> 
            let parentLink = { name = outerBagName; count = c.count}
            let parents = getParents c.name m

            m |> Map.remove c.name |> Map.add c.name (parentLink :: parents)) parentBagMap


let countOuterBags (startBag: string) (map: Map<string, Bag list>) : int =
    let rec traverse current visited =
        let updated = visited |> Set.add current
        match map.TryFind(current) with
        | None -> updated 
        | Some x -> x |> List.fold(fun acc b -> traverse b.name acc) updated    
         
    let result = traverse startBag Set.empty
    result.Count - 1

let run =
    let entries =
        "input7.txt" 
        |> Utils.readLines 
        |> Seq.toList
        
    let bagsMap = 
        entries
        |> List.map(normalizeLine)
        |> List.map(extractBagData)
        |> List.fold(buildMap) Map.empty

    bagsMap |> Map.iter(fun x v -> printfn "%s [%s]" x (v |> List.map(fun b -> sprintf "%d %s" b.count b.name) |> String.concat ", " )) 

    countOuterBags "shiny gold" bagsMap
        