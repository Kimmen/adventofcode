import { readLines } from '../helpers'

export type Input = {
    data: string
    expectedTotal: number
    config: CalcMinBag | FitBag
}

export type CalcMinBag = {
    type: 'calc'
}

export type FitBag = {
    type: 'fit',
    bag:  CubeSet
}

export type CubeSet = {
    red: number
    green: number
    blue: number
}

export type Game = {
    minBag: CubeSet
    gameId: number
    sets: CubeSet[]
    impossible: boolean
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
    total: number
    isExpected: boolean
}

self.onmessage = (event: MessageEvent<Input>) => {
    const { data, config, expectedTotal } = event.data
    const games = readLines(data).map(l => {
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

            const game: Game = { gameId, sets, impossible: false, minBag: { red: 0, green: 0, blue: 0 } } 
            self.postMessage({type: 'game', game  } as GameResult)

            return game
    })

    if(config.type === 'fit') {
        evaluateFitness(config, games, expectedTotal)
    } else if(config.type === 'calc') {
        determineMinBag(games, expectedTotal)
    }
}

function determineMinBag(games: Game[], expectedTotal: number) {
    const possibleGames = games.map(g => {
        g.minBag = g.sets.reduce((prev, g) => {
            return {
                red: Math.max(g.red, prev.red),
                green: Math.max(g.green, prev.green),
                blue: Math.max(g.blue, prev.blue)
            } as CubeSet
        }, g.minBag)
        self.postMessage({ type: 'eval', game: g } as GameEvalResult)

        return g
    })

    const total = possibleGames.reduce((prev, g) => {
        const { red, green, blue } = g.minBag
        return prev + (red * green * blue)
    }, 0)
    self.postMessage({ type: 'end', total, isExpected: total === expectedTotal } as EndResult)
}

function evaluateFitness(calcType: FitBag, games: Game[], expectedTotal: number) {
    const bag = calcType.bag
    const possibleGames = games.map(g => {
        g.impossible = g.sets.some(x => x.blue > bag.blue || x.red > bag.red || x.green > bag.green)

        self.postMessage({ type: 'eval', game: g } as GameEvalResult)

        return g
    })
        .filter(g => !g.impossible)

    const total = possibleGames.reduce((prev, g) => prev + g.gameId, 0)
    self.postMessage({ type: 'end', total, isExpected: total === expectedTotal } as EndResult)
}
