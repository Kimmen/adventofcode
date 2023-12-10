import { LitElement, css, html } from 'lit'
import { customElement, state } from 'lit/decorators.js'
import { button } from '../styles'
import { delay } from '../helpers';

import devData from './input.dev'
import data from './input'

import { Pipe, PipeData, SouthEast, Vertical, parsePipes } from './parser';
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
        this.part1(devData, 114, 100, new SouthEast())
    }

    startPart1() {
        this.reset()
        this.part1(data, 114, 1, new Vertical())
    }

    startPart2Dev() {
        this.reset()
    }

    startPart2() {
        this.reset()
    }


    async part1(data: string, expectedTotal: number, uiDelay: number, startPipe: Pipe) {
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

        this.current.success = this.current.total === expectedTotal
        await this.updateUi(0)
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
