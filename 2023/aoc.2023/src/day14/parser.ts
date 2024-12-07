import { readLines } from "../helpers"

export type Dish = {
    size: [width: number, height: number]
    stonesOnCol: Map<number, Set<number>>
    rocksOnCol: Map<number, Set<number>>
}

export class Rocks {

    private emptySet = new Set<number>()
    
    private stonesOnCol = new Map<number, Set<number>>()
    private stonesOnRow = new Map<number, Set<number>>()
    private rocksOnCol = new Map<number, Set<number>>()
    private rocksOnRow = new Map<number, Set<number>>()

    getRoundedForCol(col: number): Set<number> {
        return this.stonesOnCol.get(col) || this.emptySet 
    }

    getRoundedForRow(row: number): Set<number> {
        return this.stonesOnRow.get(row) || this.emptySet 
    }

    getSquaredForCol(col: number): Set<number> {
        return this.rocksOnCol.get(col) || this.emptySet 
    }

    getSquaredForRow(row: number): Set<number> {
        return this.rocksOnRow.get(row) || this.emptySet 
    }

    moveStone(from: [row: number, col: number], to: [row: number, col: number]) {
        const [fRow, fCol] = from
        const [tRow, tCol] = to

        this.ensureSet(fCol, this.stonesOnCol).delete(fRow)
        this.ensureSet(fRow, this.stonesOnRow).delete(fCol)

        this.ensureSet(tCol, this.stonesOnCol).add(tRow)
        this.ensureSet(tRow, this.stonesOnRow).add(tCol)
    }

    private ensureSet(key: number, map : Map<number, Set<number>>) : Set<number> {
        if(!map.has(key)) {
            map.set(key, new Set<number>())
        }

        return map.get(key)!
    }
}

export const parseDish = (data: string): Dish => {
    const lines = readLines(data)

    const dish: Dish = {
        size: [lines[0].length, lines.length],
        stonesOnCol: new Map<number, Set<number>>(),
        rocksOnCol: new Map<number, Set<number>>()
    }
    for(let row=0; row<lines.length;row++) {
        const line = lines[row]
        for(let col=0; col<lines.length;col++) {
            const symbol = line[col]
            switch(symbol) {
                case 'O': addTo(dish.stonesOnCol, col, row); break 
                case '#': addTo(dish.rocksOnCol, col, row); break 
            }
        }
    }

    return dish
}

const addTo = (map: Map<number, Set<number>>, col: number, row: number) => {
    if(!map.has(col)) {
        map.set(col, new Set<number>())
    }

    map.get(col)?.add(row)
}