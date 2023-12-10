import { readLines } from "../helpers"

export type SensoryData = {
    histories: History[]
}

export type History = {
    values: number[]
}

export const parseSensoryData = (data: string): SensoryData => {
    const histories = readLines(data)
        .map(l => l.split(' ').map(n => Number(n)))
        .map(v => ({ values: v } as History))

    return { histories }
}