module day2_2

let run =
    let passwordPattern = @"(\d+)-(\d+) (\w): (\w+)$"
    let inputEntries = "input2.txt" |> Utils.readLines

    let getPasswordDescription input =
        match input with
        | Utils.Regex passwordPattern x -> x
        | _ -> []

    let passwordEntries =
        inputEntries
        |> Seq.map (fun x -> getPasswordDescription x)
        |> Seq.map (fun [ min; max; targetChar; password ] -> (min |> int, max |> int, targetChar |> char, password))
        |> Seq.map (fun (min, max, targetChar, password) ->
            (password.[min - 1] |> char, password.[max - 1] |> char, targetChar))
        |> Seq.where (fun (charMin, charMax, targetChar) ->
            let test = sprintf "%c %c" charMin charMax
            let occurences = test |> Utils.count targetChar
            occurences = 1)

    passwordEntries |> Seq.length
