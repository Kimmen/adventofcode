module day4_1

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

    let inputEntries = filePath |> Utils.readLines |> Seq.toList

    let setPassportProp name value p = 
        let updated = 
            match name with
            | "byr" -> { p with byr = value |> Some }
            | "iyr" -> { p with iyr = value |> Some }
            | "eyr" -> { p with eyr = value |> Some }
            | "hgt" -> { p with hgt = value |> Some}
            | "hcl" -> { p with hcl = value |> Some }
            | "ecl" -> { p with ecl = value |> Some }
            | "pid" -> { p with pid = value |> Some }
            | "cid" -> { p with cid = value |> Some}
            | _ -> p

        updated

    let populatePassport (passport:Passport) line = 
        let props = (string line).Split(' ') |> List.ofArray |> List.map(fun x -> x.Split(':')) |> List.map(fun x -> (x.[0], x.[1])) 
        
        props |> Seq.fold (fun p (name, value) -> setPassportProp name value p) passport

    let isValidPassport (passport: Passport) = 
        let propsThatCannotBeNone = [
                passport.byr; 
                passport.hgt;
                passport.iyr;
                passport.pid;
                passport.ecl;
                passport.eyr;
                passport.hcl]

        propsThatCannotBeNone 
            |> Seq.where (fun x -> 
                match x with
                | Some _ -> false
                | None -> true)
            |> Seq.length = 0

    let (last, passports) = inputEntries |> Seq.fold (fun ((proto:Passport), (passports: Passport list)) line -> 
        match line with 
        | "" -> (Passport.Empty, proto :: passports)
        | _ ->  (populatePassport proto line, passports)) (Passport.Empty, [])

    let validPassports = (passports @ [last]) |> Seq.where(fun p -> isValidPassport p) |> Seq.toList
    validPassports.Length