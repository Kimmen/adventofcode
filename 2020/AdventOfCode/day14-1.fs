module day14_1
    type Computation = { orMask: uint64; andMask: uint64; mem: Map<uint64, uint64>; }
   
    type TermValue =
        | Mask of andMask: uint64 * orMask: uint64 
        | Mem of index: uint64 * value: uint64   

        
    let applyMask (value: uint64) (andMask: uint64) (orMask: uint64) =
        (value &&& andMask) ||| orMask

    let createMasks (line: string ) : (uint64 * uint64) =
        let chars = line.ToCharArray() |> Array.mapi(fun i x -> (i, x))
        let orMask = 
            chars 
            |> Array.fold(fun m (i, x) -> 
                match x with 
                | '1' -> m ||| (1UL<<<(chars.Length- 1 - i))
                | _ -> m) 0UL
        let andMask = 
            chars 
            |> Array.fold(fun m (i, x) ->
                match x with 
                | '0' -> m ||| (1UL<<<(chars.Length - 1 - i))
                | _ -> m) 0UL
            
        (~~~andMask, orMask)

    let computeMemory comp index value =
        let memory = comp.mem
        { comp with mem = memory.Add(index, applyMask value comp.andMask comp.orMask) }


    let runTerm (comp: Computation) (term: TermValue) =
        match term with
        | Mask (andMask, orMask) -> { comp with andMask = andMask; orMask = orMask }
        | Mem (index, value) -> computeMemory comp index value

    let parseTerm (line: string) : TermValue = 
        let parts = line.Split('=')
        match parts.[0].Trim() with
        | Utils.Regex("mem\[(\d+)\]") index -> TermValue.Mem(index.[0] |> uint64, parts.[1] |> uint64)
        | "mask" -> TermValue.Mask(createMasks (parts.[1].Trim()))
        | _ -> failwith "wut term?"


    let run =

        let result = 
            "input14.txt" 
            |> Utils.readLines
            |> Seq.map(parseTerm)
            |> Seq.fold(runTerm) { orMask = 0UL; andMask = System.UInt64.MaxValue; mem = Map.empty }
        
        result.mem |> Map.toSeq |> Seq.map(fun (k, v) -> v) |> Seq.sum