import { readLines } from "../helpers"

export type WastelandMap = {
    instructions: string[]
    steps: Map<string, {left: string, right: string}>
}

export const parseMap = (data: string): WastelandMap => {
    const lines = readLines(data)
    
    const instructions = lines[0].split('')
    const steps = new Map<string, {left: string, right: string}>()

    for(let i=2; i < lines.length; i++) {
        const match = /^(?<from>[A-Z]{3}) = \((?<toLeft>[A-Z]{3}), (?<toRight>[A-Z]{3})\)$/gm.exec(lines[i])
        const { from, toLeft, toRight } = match?.groups!

        steps.set(from, {left: toLeft, right: toRight})
    }
    
    return { instructions, steps}
}