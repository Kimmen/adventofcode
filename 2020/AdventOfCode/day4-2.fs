module day4_2

type Passport =
    { byr: string option
      iyr: string option
      eyr: string option
      hgt: string option
      hcl: string option
      ecl: string option
      pid: string option
      cid: string option }

    static member Empty =
        { byr = None
          iyr = None
          eyr = None
          hgt = None
          hcl = None
          ecl = None
          pid = None
          cid = None }

let run =
    let filePath = "input4.txt"

    let inputEntries =
        filePath |> Utils.readLines |> Seq.toList

    let validateByr value = value >= 1920 && value <= 2002

    let validateIyr value = value >= 2010 && value <= 2020

    let validateEyr value = value >= 2020 && value <= 2030

    let validateHgt (value: string) =
        let validate (unit: string) min max =
            let pattern = @"^\d{2,3}" + unit + "$"
            match value with
            | Utils.Regex pattern _ ->
                let height = value.Replace(unit, "") |> int
                height >= min && height <= max
            | _ -> false

        validate "cm" 150 193 || validate "in" 59 76

    let validateHcl value =
        match value with
        | Utils.Regex @"^#[0-9a-f]{6}$" _ -> true
        | _ -> false

    let validateEcl value =
        [ "amb"
          "blu"
          "brn"
          "gry"
          "grn"
          "hzl"
          "oth" ]
        |> Seq.contains value

    let validatePid value =
        match value with
        | Utils.Regex @"^\d{9}$" _ -> true
        | _ -> false

    let setPassportProp name value p =
        let updated =
            match name with
            | "byr" when validateByr (value |> int) -> { p with byr = value |> Some }
            | "iyr" when validateIyr (value |> int) -> { p with iyr = value |> Some }
            | "eyr" when validateEyr (value |> int) -> { p with eyr = value |> Some }
            | "hgt" when validateHgt value -> { p with hgt = value |> Some }
            | "hcl" when validateHcl value -> { p with hcl = value |> Some }
            | "ecl" when validateEcl value -> { p with ecl = value |> Some }
            | "pid" when validatePid value -> { p with pid = value |> Some }
            | "cid" -> { p with cid = value |> Some }
            | _ -> p

        updated

    let populatePassport (passport: Passport) line =
        let props =
            (string line).Split(' ')
            |> List.ofArray
            |> List.map (fun x -> x.Split(':'))
            |> List.map (fun x -> (x.[0], x.[1]))

        props
        |> Seq.fold (fun p (name, value) -> setPassportProp name value p) passport

    let isValidPassport (passport: Passport) =
        let propsThatCannotBeNone =
            [ passport.byr
              passport.hgt
              passport.iyr
              passport.pid
              passport.ecl
              passport.eyr
              passport.hcl ]

        propsThatCannotBeNone
        |> Seq.where (fun x ->
            match x with
            | Some _ -> false
            | None -> true)
        |> Seq.length = 0

    let (last, passports) =
        inputEntries
        |> Seq.fold (fun (proto: Passport, passports: Passport list) line ->
            match line with
            | "" -> (Passport.Empty, proto :: passports)
            | _ -> (populatePassport proto line, passports)) (Passport.Empty, [])

    let validPassports =
        (passports @ [ last ])
        |> Seq.where (fun p -> isValidPassport p)
        |> Seq.toList

    validPassports.Length