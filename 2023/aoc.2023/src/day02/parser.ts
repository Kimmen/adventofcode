export type GameParseModel = {
    id: number,
    sets: SetParseModel[]
    
}
export type SetParseModel = {
    red: number,
    green: number,
    blue: number
}

export const parseGame = (line: string): GameParseModel => {
    const gameMatch = /^Game (?<id>\d+): (?<subsets>.+)$/gm.exec(line)
    if (!gameMatch) {
        throw Error("Failed to match game: " + line)
    }

    const result: GameParseModel = {
        id: parseInt(gameMatch.groups!.id),
        sets: []
    }

    gameMatch.groups!.subsets.split(';')
        .forEach(set => {
            const current: SetParseModel= { red: 0, green: 0, blue: 0}
            result.sets.push(current)

            set.trim().split(',')
                .map(coloredCubes => {
                    const def = coloredCubes.trim().split(' ')
                    return {
                        color: def[1].trim().toLowerCase(),
                        count: parseInt(def[0].trim())
                    }
                })
                .forEach(x => {
                    switch (x.color) {
                        case 'red': current.red = x.count; break
                        case 'green': current.green = x.count; break
                        case 'blue': current.blue = x.count; break
                    }
                })
        });

    return result
}