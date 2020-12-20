module day14_2
    type TermValue =
        | Mask of string
        | Mem of index: uint64 * value: uint64   


    type Computation = { mask: string; mem: Map<uint64, uint64>; }   

    let createDistributedAddresses (line: string) (address: uint64) : (uint64 list) =
        let setBit x bi =  x ||| (1UL <<< bi)
        let unsetBit x bi = x &&& (~~~(1UL <<< bi))
        
        let floatingIndecies = 
            line.ToCharArray() 
            |> Array.mapi(fun i x -> (i, x))
            |> Array.where(fun (_, x ) -> x = 'X')
            |> Array.map(fun (i, _) -> line.Length - 1 - i)
            |> Array.rev

        let initialMask = System.Convert.ToUInt64(line.Replace('X', '0'), 2);

        let maxDistrubtionBits =  System.Convert.ToInt32(new string(System.Linq.Enumerable.Repeat('1', floatingIndecies.Length) |> Seq.toArray), 2)
        let result = 
            [0..maxDistrubtionBits]
            |> Seq.map(fun v -> 
                let mask = new System.Collections.BitArray(System.BitConverter.GetBytes(v))

                [0..floatingIndecies.Length - 1]
                |> Seq.fold(fun m i -> 
                    let isSet = mask.[i]
                    let bitIndex = floatingIndecies.[i]
                    if isSet then setBit m bitIndex else unsetBit m bitIndex
                ) address ||| initialMask)
            
        result |> List.ofSeq

    let appendValueToAddress (memory: Map<uint64, uint64>) (address: uint64) (value: uint64) : Map<uint64, uint64> =
        memory.Add(address, value)
        
    let computeMemory (comp :Computation) (address: uint64) (value: uint64) =
        let addresses = createDistributedAddresses comp.mask address
        let updatedMemory = addresses |> Seq.fold(fun mem a -> appendValueToAddress mem a value ) comp.mem
        {comp with mem = updatedMemory}


    let runTerm (comp: Computation) (term: TermValue) =
        match term with
        | Mask x -> { comp with mask = x }
        | Mem (address, value) -> computeMemory comp address value

    let parseTerm (line: string) : TermValue = 
        let parts = line.Split('=')
        match parts.[0].Trim() with
        | Utils.Regex("mem\[(\d+)\]") index -> TermValue.Mem(index.[0] |> uint64, parts.[1] |> uint64)
        | "mask" -> TermValue.Mask(parts.[1].Trim())
        | _ -> failwith "wut term?"


    let run =

        let result = 
            "input14.txt" 
            |> Utils.readLines
            |> Seq.map(parseTerm)
            |> Seq.fold(runTerm) { mask = ""; mem = Map.empty }
    
        result.mem |> Map.toSeq |> Seq.map(fun (_, v) -> v) |> Seq.sum