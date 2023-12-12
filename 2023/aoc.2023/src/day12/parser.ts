import { readLines } from "../helpers"

export type ArrangedRow = {
    springs: string
    conditionRecord: number[]
}

export const parseSpringRows = (data: string): ArrangedRow[] => {
    return readLines(data)
    .map(l => {
        const [springs, rawRecord] = l.split(' ')
        return { 
            springs, 
            conditionRecord: rawRecord.split(',').map(x => Number(x)) 
        } as ArrangedRow
    })
}