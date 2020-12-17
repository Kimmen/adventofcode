module day2_1
let inputEntries = "input2.txt" |> Utils.readLines

let run =
    fun x ->
        let passwordPattern = @"(\d+)-(\d+) (\w): (\w+)$"

        let getPasswordDescription input =
            match input with
            | Utils.Regex passwordPattern x -> x
            | _ -> []

        let composeData (x: string list) =
            (x.[0] |> int, x.[1] |> int, x.[2] |> char, x.[3] |> string)


        let passwordEntries =
            inputEntries
            |> Seq.map (fun x -> getPasswordDescription x |> composeData)
            |> Seq.map (fun (min, max, targetChar, password) ->
                let cc = Utils.count (targetChar |> char) password
                (min |> int, max |> int, cc |> int))
            |> Seq.where (fun (min, max, ccount) ->
                match ccount with
                | x when x < min || x > max -> false
                | _ -> true)

        passwordEntries |> Seq.length
