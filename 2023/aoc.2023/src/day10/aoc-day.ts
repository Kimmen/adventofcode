import { LitElement, css, html } from 'lit'
import { customElement, state } from 'lit/decorators.js'
import { button } from '../styles'
import { delay } from '../helpers';

import classifyPoint from 'robust-point-in-polygon'

import devData from './input.dev'
import devData2 from './input.dev.2'
import data from './input'

import { Horizontal, NorthEast, NorthWest, Pipe, PipeData, Pos, SouthEast, SouthWest, Vertical, parsePipes } from './parser';
import { classMap } from 'lit/directives/class-map.js';

@customElement('aoc-day-10')
export class AocDay extends LitElement {
    constructor() {  super() }
    
    private canvas?: CanvasRenderingContext2D;

    @state() current?: {
        schematics: PipeData
        progress: {step: number, max: number}
        info: string
        total?: number
        success?: boolean
    }

    async updateUi(ms: number) {
        await delay(ms)
        this.requestUpdate()
    }

    reset() {
        this.current = undefined
        this.canvas!.clearRect(0, 0, 1400, 1400);
    }

    startPart1Dev() {
        this.reset()
        this.part1(devData, 8, 100, new SouthEast(), false)
    }

    startPart1() {
        this.reset()
        this.part1(data, 6717, 0, new Vertical(), false)
    }

    startPart2Dev() {
        this.reset()
        this.part1(devData2, 8, 10, new SouthEast(), true)
    }

    startPart2() {
        this.reset()
        this.part1(data, 381, 0, new Vertical(), true)
    }


    async part1(data: string, expectedTotal: number, uiDelay: number, startPipe: Pipe, findInside: boolean) {
        this.current = {
            schematics: parsePipes(data),
            progress: { step: 0, max: 140 * 140},
            info: ""
        }
        const [startRow, startCol] = this.current.schematics.start
        this.current.schematics.pipes[startRow][startCol] = startPipe

        for(let row = 0; row < this.current.schematics.pipes.length; row++) {
            const line = this.current.schematics.pipes[row]
            for(let col=0; col < line.length; col++) {
                const pipe = line[col]
                pipe.draw([row, col], this.canvas!)
            }
        }

        const buildKey = ([row, col]: Pos) => `${row.toString().padStart(4, "0")}${col.toString().padStart(4, "0")}` 
        const pipes = this.current.schematics.pipes
        const pos: Pos = this.current.schematics.start
        const visited = new Set<string>([buildKey(pos)])
        const loopPolygon: [x: number, y:number][] = []

        this.drawStep(pos)

        let steps = 1
        let pipe = pipes[pos[0]][pos[1]]
        let points = pipe.getConnectedPoints(pos)
        let next = this.getAdjecantPipes(pos, pipes)[0]
        loopPolygon.push(...points)

        while(!visited.has(buildKey(next))) {
            const kn = buildKey(next)
            if(visited.has(kn)) continue

            visited.add(kn)
            steps++
            this.drawStep(next)

            const [p1, p2] = this.getAdjecantPipes(next, pipes)
            const k1 = buildKey(p1)

            next = visited.has(k1) ? p2 : p1
            points = pipes[next[0]][next[1]].getConnectedPoints(next)
            const first = points[0]
            const last = points[points.length - 1]
            const lastPoint = loopPolygon[loopPolygon.length - 1]
            this.drawPolygon(first, last)

            if(first[0] != lastPoint[0] || first[1] != lastPoint[1]) {
                loopPolygon.push(first)
            }
            if(last[0] != lastPoint[0] || last[1] != lastPoint[1]) {
                loopPolygon.push(last)
            }
            
            if(steps % 100 == 0) {
                await this.updateUi(uiDelay)
            }
            
            this.current.info = `${next[0]}, ${next[1]}`
            this.current.total = steps / 2
        }

        if(findInside) {
            this.current.total = 0
            for(let row = 0; row < this.current.schematics.pipes.length; row++) {
                const line = this.current.schematics.pipes[row]
                for(let col=0; col < line.length; col++) {
                    if(visited.has(buildKey([row, col]))) {
                        continue
                    }
                    let point: [x: number, y: number] = [col * 9 + 4, row * 9 + 4]
                    
                    let isInside = classifyPoint(loopPolygon, point) == -1
                    this.current.total += isInside ? 1 : 0
                    this.current.info = `${row}, ${col} : ${point[0]}, ${point[1]}`
    
                    this.drawStep([row, col],isInside ? 'rgb(64, 191, 96, 0.2)' : 'rgb(48,48,48, 0.2)')
                    if(this.current.total % 10 == 0) {
                        await this.updateUi(uiDelay)
                    }
                }
            }
        }

        this.current.success = this.current.total === expectedTotal
        await this.updateUi(0)
    }

    drawStep(pos: Pos, color?: string) {
        const [row, col] = pos
        const canvas = this.canvas!
        let x = col * 9
        let y = row * 9 
        canvas.fillStyle = color || 'rgba(113, 57, 198, 0.2)'
        canvas.fillRect(x,y,9,9)
    }

    drawPolygon([x1, y1]: [x: number, y:number], [x2, y2]: [x: number, y: number]) {
        const canvas = this.canvas!
        canvas.beginPath()
        canvas.moveTo(x1, y1)
        canvas.lineTo(x2, y2)
        canvas.fillStyle = 'rgba(57, 92, 198, 0.4)'
        canvas.stroke()
    }

    getAdjecantPipes(pos: Pos, pipes: Pipe[][]): [p1: Pos, p2: Pos] {
        const [row, col] = pos
        const pipe = pipes[row][col]

        if(pipe instanceof Vertical) {
            return [[row-1, col], [row+1, col]]
        } else if(pipe instanceof Horizontal) {
            return [[row, col-1], [row, col+1]]
        } else if(pipe instanceof NorthEast) {
            return [[row-1, col], [row, col+1]]
        } else if(pipe instanceof NorthWest) {
            return [[row-1, col], [row, col-1]]
        } else if(pipe instanceof SouthEast) {
            return [[row+1, col], [row, col+1]]
        } else if(pipe instanceof SouthWest) {
            return [[row+1, col], [row, col-1]]
        } 
        
        throw Error("Unhandled pipe")
    }

    
    updated(): void {
        const canvas = this.renderRoot.querySelector("#map") as HTMLCanvasElement 
        if(this.canvas || !canvas) return

        
        this.canvas = canvas.getContext("2d")!
        this.canvas!.strokeStyle = "rgb(48,48,48)"
    }

    render() {
        return html`
        <h1>Day10</h1>

        <div class="input-selection">
            <button @click=${this.startPart1Dev}>Part1.Dev</button>
            <button @click=${this.startPart1}>Part1</button>
            <button @click=${this.startPart2Dev} >Part2.Dev</button>
            <button @click=${this.startPart2} >Part2</button>
        </div>
        <section class="info">
            <p class=${classMap({done: this.current?.success !== undefined, success: !!this.current?.success, failed: !this.current?.success })}>
                ${this.current?.total}
            </p>
            <p>${this.current?.info || 'N/A'}</p>
            <progress class="progress" value=${this.current?.progress.step!} max=${this.current?.progress.max!}></progress>
        </section>
         <section></section>
            <canvas id="map" width=${1400} height=${1400} ></canvas>
        </section>
        `
    }

    static styles = [button, css`
        :host {
            flex: 0 0 auto;
            --top-spacing: 2rem;
            --white: rgb(242, 242, 242);
            --red: rgb(204, 51, 51);
            --green: rgb(64, 191, 96);
            --yellow: rgb(217, 172, 38);
            --blue: rgb(57, 92, 198);
            --dark: rgb(48,48,48);
            --purple: rgb(113, 57, 198);
        }

        #map {
            background-color: var(--white)
        }
        
        section.info {
            outline: 1px dashed white;
            margin-top: var(--top-spacing);
            margin-bottom: var(--top-spacing);
            padding: 1rem;
        }

        section.info .progress {
            width: 100%;
        }

        .input-selection {
            display: flex;
            place-items: start;
            gap: 1rem;
        }

        p.success.done {
            color: var(--green)
        }

        p.failed.done {
            color: var(--red)
        }
        
        `]
}
