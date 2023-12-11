import { readLines } from "../helpers"

export type GalaxyMap = {
    size: [width: number, height: number]
    galaxies: [x: number, y: number][]
}

export const parseGalaxyMap = (data: string): GalaxyMap => {
    const lines = readLines(data)
    const height = lines.length
    const width = lines[0].length
    const galaxies:[x: number, y: number][] = []

    lines.forEach((l, y) => {
        let x = 0
        x = l.indexOf("#", x)
        while(x > -1) {
            galaxies.push([x, y])
            x = l.indexOf("#", x + 1)
        }
    })
        
    return { size: [width, height], galaxies } as GalaxyMap
}