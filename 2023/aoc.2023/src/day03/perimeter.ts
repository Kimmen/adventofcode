import { Part, Pos } from "./parser"

export const determinePerimeter = (part: Part): Pos[] => {
    let perimeter: Pos[] = [];

    const rowOver = part.position.row - 1
    const rowCurr = part.position.row
    const rowUnder = part.position.row + 1

    for (let i = -1; i <= part.width; i++) {
        const column = part.position.column + i
        perimeter = [...perimeter, {
            row: rowOver, column
        } as Pos, {
            row: rowUnder, column
        } as Pos]
    }

    return [...perimeter, 
        { row: rowCurr, column: part.position.column - 1 } as Pos, 
        { row: rowCurr, column: part.position.column + part.width } as Pos];
}