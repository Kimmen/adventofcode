import { readLines } from "../helpers"

export type PipeData = {
    pipes: Pipe[][]
    start: Pos
}

export type Pos = [row: number, col: number]

export abstract class Pipe {
    canConnectTo(pipe: Pipe, myPos: Pos, otherPos: Pos) : boolean {
        const myConnections = this.getConnections(myPos)
        const otherConnections = pipe.getConnections(otherPos)

        return myConnections.some(p0 => otherConnections.some(p1 => p0[0] === p1[0] && p0[1] === p1[1]))
    }
    abstract getConnections(pos: Pos): Pos[]
    abstract draw(pos: Pos, canvas: CanvasRenderingContext2D): void;
}

export class Vertical extends Pipe {
    getConnections(pos: Pos): Pos[] {
        const [row, col] = pos
        return [[row - 1, col], [row + 1, col]]
    }

    draw(pos: Pos, canvas: CanvasRenderingContext2D) {
        const [row, col] = pos
        let x = col * 9 + 5
        let y = row * 9 

        canvas!.beginPath()
        canvas!.moveTo(x, y)
        y += 10
        canvas!.lineTo(x, y)
        canvas!.stroke()
    }
}

export class Horizontal extends Pipe {
    getConnections(pos: Pos): Pos[] {
        const [row, col] = pos
        return [[row, col - 1], [row, col + 1]]
    }

    draw(pos: Pos, canvas: CanvasRenderingContext2D) {
        const [row, col] = pos
        let x = col * 9
        let y = row * 9 + 5 

        canvas!.beginPath()
        canvas!.moveTo(x, y)
        x += 10
        canvas!.lineTo(x, y)
        canvas!.stroke()
    }
}

export class NorthEast extends Pipe {
    getConnections(pos: Pos): Pos[] {
        const [row, col] = pos
        return [[row - 1, col], [row, col + 1]]
    }

    draw(pos: Pos, canvas: CanvasRenderingContext2D) {
        const [row, col] = pos
        let x = col * 9 + 5
        let y = row * 9 

        canvas!.beginPath()
        canvas!.moveTo(x, y)
        y += 5
        canvas!.lineTo(x, y)
        x += 5
        canvas!.lineTo(x, y)
        canvas!.stroke()
    }
}

export class SouthEast extends Pipe {
    getConnections(pos: Pos): Pos[] {
        const [row, col] = pos
        return [[row + 1, col], [row, col + 1]]
    }

    draw(pos: Pos, canvas: CanvasRenderingContext2D) {
        const [row, col] = pos
        let x = col * 9 + 5
        let y = row * 9 + 10

        canvas!.beginPath()
        canvas!.moveTo(x, y)
        y -= 5
        canvas!.lineTo(x, y)
        x += 5
        canvas!.lineTo(x, y)
        canvas!.stroke()
    }
}

export class NorthWest extends Pipe {
    getConnections(pos: Pos): Pos[] {
        const [row, col] = pos
        return [[row - 1, col], [row, col - 1]]
    }

    draw(pos: Pos, canvas: CanvasRenderingContext2D) {
        const [row, col] = pos
        let x = col * 9 + 5
        let y = row * 9 

        canvas!.beginPath()
        canvas!.moveTo(x, y)
        y += 5
        canvas!.lineTo(x, y)
        x -= 5
        canvas!.lineTo(x, y)
        canvas!.stroke()
    }
}

export class SouthWest extends Pipe {
    getConnections(pos: Pos): Pos[] {
        const [row, col] = pos
        return [[row + 1, col], [row, col - 1]]
        return [[row + 1, col], [row, col - 1]]
    }

    draw(pos: Pos, canvas: CanvasRenderingContext2D) {
        const [row, col] = pos
        let x = col * 9 + 5
        let y = row * 9 + 10

        canvas!.beginPath()
        canvas!.moveTo(x, y)
        y -= 5
        canvas!.lineTo(x, y)
        x -= 5
        canvas!.lineTo(x, y)
        canvas!.stroke()
    }
}

export class Ground extends Pipe {
    getConnections(pos: Pos): Pos[] {
        return []
    }

    draw() {}
}

export const parsePipes = (data: string): PipeData => {
    const lines = readLines(data)
    const result: PipeData = { pipes: Array.from({length: lines.length}), start: [0,0] }

    for(let row = 0; row<lines.length; row++) {
        const line = lines[row]
        const pipes: Pipe[] = Array.from({length: line.length})

        for(let col=0; col<line.length; col++) {
            switch(line[col]) {
                case "|": pipes[col] = new Vertical(); break
                case "-": pipes[col] = new Horizontal(); break
                case "L": pipes[col] = new NorthEast(); break
                case "J": pipes[col] = new NorthWest(); break
                case "7": pipes[col] = new SouthWest(); break
                case "F": pipes[col] = new SouthEast(); break
                case ".": pipes[col] = new Ground(); break
                case "S": 
                    result.start = [row, col]
                    pipes[col] = new Ground()
                    break
                default: throw Error("unhandled pipe symbol")
            }
        }

        result.pipes[row] = pipes
    }
        
    return result
}