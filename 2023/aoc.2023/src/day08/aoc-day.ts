import { LitElement, css, html, nothing } from 'lit'
import { customElement, state } from 'lit/decorators.js'
import { repeat } from 'lit/directives/repeat.js';
import { classMap } from 'lit/directives/class-map.js';

import { parseMap } from './parser';
import { button } from '../styles'
import { delay, chunkWhile } from '../helpers';
import { lcm } from './calculator'

import devData from './input.dev'
import devData2 from './input.dev.2'
import data from './input'

type Path = {
    rendering: DirectionRending
    current: string
    stepsToZ: number
}
type DirectionRending = {
    directions: number[][]
    color: number[]
    currentDir: number
    pos: number[]
}

@customElement('aoc-day-8')
export class AocDay extends LitElement {
    constructor() {  super() }

    private mapCanvas?: CanvasRenderingContext2D;
    
    @state() current?: {
        info: string
        total?: number
        success: boolean
    }

    async updateUi(ms: number) {
        await delay(ms)
        this.requestUpdate()
    }

    reset() {
        this.current = undefined
    }

    startPart1Dev() {
        this.reset()
        this.traverse(devData, 6, 100)
    }

    startPart1() {
        this.reset()
        this.traverse(data, 6, 1)
    }

    startPart2Dev() {
        this.reset()
        this.traverse2(devData2, 6, 500)
    }

    startPart2() {
        this.reset()
        this.traverse2(data, 6, 1)
    }

    async traverse(data: string, expectedTotal: number, uiDelay: number) {
        const map = parseMap(data)
        this.current = { info: "", total: 0, success: false}
        let current = "AAA"
        let stillGoing = true
        let instructionIndex = 0

        this.mapCanvas!.clearRect(0, 0, 2000, 2000);

        const rendering: DirectionRending = {
            directions: [[1, 0], [0, 1], [-1, 0], [0, -1]],
            color: [67,152,224],
            currentDir: 1,
            pos: [400, 400]
        }

        const visited = new Set<string>

        do {
            const instruction = map.instructions[instructionIndex]
            const { left, right } = map.steps.get(current)!
            const next = instruction === "L" ? left : right
            this.current.total!++
            this.current.info = next
            stillGoing = next !== "ZZZ"
           
            const visitNode = current + instructionIndex + next
           
            visited.add(visitNode)
            instructionIndex = (instructionIndex + 1) % map.instructions.length
            current = next
        
            this.renderDirection(rendering, instruction, instructionIndex)
            if((rendering.pos[0] % 100) < 10) await this.updateUi(uiDelay)
        } while(stillGoing)
        
        this.current.success = this.current.total === expectedTotal
        await this.updateUi(0)
    }

    async traverse2(data: string, expectedTotal: number, uiDelay: number) {
        const map = parseMap(data)
        this.current = { info: "", total: 0, success: false}

        let stillGoing = true
        let instructionIndex = 0

        this.mapCanvas!.clearRect(0, 0, 2000, 2000);

        const paths: Path[] = Array
            .from(map.steps.keys())
            .filter(x => x[2] === "A")
            .map((x, index) => ({
                current: x, 
                stepsToZ: 0,
                rendering: {
                    directions: [[1, 0], [0, 1], [-1, 0], [0, -1]],
                    color: [67,152,224],
                    currentDir: index % 4,
                    pos: [400, 400]
                }
            } as Path))

        let steps = 1;
        do {
            const instruction = map.instructions[instructionIndex]
            this.current.info = ""

            paths.forEach(p => {
                if(p.stepsToZ > 0) {
                    return;
                }

                const { left, right } = map.steps.get(p.current)!
                const next = instruction === "L" ? left : right

                p.current = next
                this.renderDirection(p.rendering, instruction, instructionIndex)
                this.current!.info += " " + next

                if(p.current[2] === "Z") {
                    p.stepsToZ = steps
                }
            })

            stillGoing = paths.some(p => p.stepsToZ === 0)
            instructionIndex = (instructionIndex + 1) % map.instructions.length
    
            if((steps % 100) < 2) 
                await this.updateUi(uiDelay)

            steps++
        } while(stillGoing)

        this.current!.info = paths.reduce((o, p) => o + p.stepsToZ + ",", "")
        this.current.total = lcm(paths.map(x => x.stepsToZ))
        this.current.success = this.current.total === expectedTotal
        await this.updateUi(0)
    }


    renderDirection(rendering: DirectionRending, instruction: string, colorSalt: number) {
        let {directions, color, currentDir, pos} = rendering
        currentDir += instruction === "R" ? 1 : -1;
        currentDir = currentDir % directions.length;
        currentDir = currentDir < 0 ? directions.length - 1 : currentDir;

        const [xDir, yDir] = directions[currentDir];
        let [x, y] = pos

        this.mapCanvas!.beginPath();
        color[0] = (color[0] + colorSalt) % 255;
        color[1] = (color[2] + colorSalt + 2) % 255;
        color[2] = (color[2] + colorSalt + 5) % 255;
        this.mapCanvas!.strokeStyle = `rgb(${color[0]}, ${color[1]}, ${color[2]})`;
        this.mapCanvas!.moveTo(x, y);
        x += xDir * 5;
        y += yDir * 5;
        this.mapCanvas!.lineTo(x, y);
        this.mapCanvas!.stroke();
        
        rendering.color = color
        rendering.currentDir = currentDir
        rendering.pos = [x, y]
    }


    updated(): void {
        const canvas = this.renderRoot.querySelector("#map") as HTMLCanvasElement 
        this.mapCanvas = canvas.getContext("2d")!
    }

    render() {
        return html`
        <h1>Day8</h1

        <div class="input-selection">
            <button @click=${this.startPart1Dev}>Part1.Dev</button>
            <button @click=${this.startPart1}>Part1</button>
            <button @click=${this.startPart2Dev} >Part2.Dev</button>
            <button @click=${this.startPart2} >Part2</button>
        </div>
        <section class="info">
            <p class=${this.current?.success ? 'success' : ''}>${this.current?.total}</p>
            <p>${this.current?.info || 'N/A'}</p>
        </section>
        <section>
            <canvas id="map" width="2000" height="2000" ></canvas>
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

        .input-selection {
            display: flex;
            place-items: start;
            gap: 1rem;
        }

        p.success {
            color: var(--green)
        }

        p.failed {
            color: var(--red)
        }
        
        `]
}
