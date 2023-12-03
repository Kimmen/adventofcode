import { readLines } from "../helpers"

export type EngineSchematic  = {
    engineSize: Size
    symbols: {
        [key: string]: Symbol
    },
    parts: {
        [key: string]: Part
    }
    gears: {
        [key: string]: Symbol
    }
}

export type Size = { rows: number, columns: number }
export type Pos = { row: number, column: number }

export type Part = { position: Pos, width: number, number: number }
export type Symbol = { position: Pos, symbol: string }

export const posKey = (row: number, column: number): string => `${row}_${column}`

export const parseEngineSchematics = (data: string): EngineSchematic => {
    const lines = readLines(data)

    const size : Size = { columns: lines[0].length, rows: lines.length }
    const engine: EngineSchematic = {
        engineSize: size,
        symbols: {},
        parts: {},
        gears: {}
    }

    lines.forEach((line, row) => {
        let column = 0
        while(column < line.length) {
            const c = line[column]
            
            if(isDigit(c)) {
                column = parsePart(column, row, line, engine)
            }
            else if(isDot(c)) {
                column++
            }
            else {
                column = parseSymbol(column, row, line, engine)
            }
        }
    })

    return engine
}

const parsePart = (column: number, row: number, line: string, engine: EngineSchematic) : number => {
    const part: Part = { number: 0, width: 0, position: { row: row, column: column }}

    let rawNumber = ''
    do {
        rawNumber += line[column]
        column++
    } while(column < line.length && isDigit(line[column]))

    part.number = Number(rawNumber)
    part.width = rawNumber.length
    engine.parts[posKey(part.position.row, part.position.column)] = part

    return column
}


const parseSymbol = (column: number, row: number, line: string, engine: EngineSchematic) : number => {
    const symbol: Symbol = { symbol: line[column],  position: { row: row, column: column  } }
    const key = posKey(symbol.position.row, symbol.position.column)
    engine.symbols[key] = symbol

    if(symbol.symbol === "*") {
        engine.gears[key] = symbol
    }

    return ++column
}

const isDigit = (char: string) => {
    return !isNaN(+char)
}

const isDot = (char: string) => {
    return char === '.'
}
