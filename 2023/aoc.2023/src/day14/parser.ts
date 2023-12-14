import { readLines } from "../helpers"

export type Dish = {
    size: [width: number, height: number]
    stonesOnCol: Map<number, Set<number>>
    rocksOnCol: Map<number, Set<number>>
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