import { readLines } from '../helpers'

export type Bag = CubeSet

export type CubeSet = {
    red: number
    green: number
    blue: number
}

export type Game = {
    gameId: number
    sets: CubeSet[]
    impossible: boolean | unknown
}

export type Result = {
    type: 'game' | 'eval' | 'end'
}

export type GameResult = Result & {
    type: 'game'
    game: Game
}

export type GameEvalResult = Result & {
    type: 'eval'
    game: Game
}

export type EndResult = Result & {
    type: 'end'
    sum: number
}

self.onmessage = (event: MessageEvent<{ input: string, bag: Bag }>) => {
    const { input, bag } = event.data
    const games = readLines(input).map(l => {
        // const gameMatch = l.match(/^Game (?<id>\d+): (?<subsets>.+)$/gm)!
        const gameMatch = /^Game (?<id>\d+): (?<subsets>.+)$/gm.exec(l)

        if (!gameMatch) {
            throw Error("Failed to match game: " + l)
        }

        const gameId = parseInt(gameMatch.groups!.id)
        const sets = gameMatch.groups!.subsets
            .split(';')
            .map(set => {
                const cubeSet: CubeSet = { red: 0, green: 0, blue: 0 }
                set
                    .trim()
                    .split(',')
                    .map(coloredCubes => {
                        const def = coloredCubes.trim().split(' ')
                        return { color: def[1].trim().toLowerCase(), count: parseInt(def[0].trim()) }
                    })
                    .forEach(x => {
                        switch (x.color) {
                            case 'green': cubeSet.green = x.count; break
                            case 'blue': cubeSet.blue = x.count; break
                            case 'red': cubeSet.red = x.count; break
                        }
                    })
                
                return cubeSet
            })

            const game: Game = { gameId, sets, impossible: null } 
            self.postMessage({type: 'game', game  } as GameResult)

            return game
    })

    const possibleGames = games.map(g => {
        g.impossible = g.sets.some(x => x.blue > bag.blue  || x.red > bag.red || x.green > bag.green)
        self.postMessage({type: 'eval', game: g } as GameEvalResult)

        return g
    })
    .filter(g => !g.impossible)

    const sum = possibleGames.reduce((prev, g) => prev + g.gameId , 0)
    self.postMessage({type: 'end', sum } as EndResult)
}