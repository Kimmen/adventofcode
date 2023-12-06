import { readLines } from "../helpers"

export type RaceCompetition = {
    times: number[]
    distances: number[]
}

export const parseCompetition = (data: string): RaceCompetition => {
    const lines = readLines(data)

    return {
        times: lines[0].replace('Time:', '').trim().split(' ').filter(n => !!n).map(x => Number(x.trim())),
        distances: lines[1].replace('Distance:', '').trim().split(' ').filter(n => !!n).map(x => Number(x.trim()))
    }
}