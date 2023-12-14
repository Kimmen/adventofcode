import { LitElement, css, html, nothing } from 'lit'
import { customElement, state } from 'lit/decorators.js'
import { button } from '../styles'
import { chunkWhile, delay } from '../helpers';

import devData from './input.dev'
import data from './input'

import { classMap } from 'lit/directives/class-map.js';
import { Dish, parseDish } from './parser';
import { repeat } from 'lit/directives/repeat.js';
import { styleMap } from 'lit/directives/style-map.js';

@customElement('aoc-day-14')
export class AocDay extends LitElement {
    constructor() {  super() }
    

    @state() current?: {
        dish: Dish
        colInProgress: number
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
    }

    startPart1Dev() {
        this.reset()
        this.part1(devData, 136, 100)
    }

    startPart1() {
        this.reset()
        this.part1(data, 105249, 1)
    }

    startPart2Dev() {
        this.reset()
    }

    startPart2() {
        this.reset()
    }


    async part1(data: string, expectedTotal: number, uiDelay: number) {
        const dish = parseDish(data)
        const [width, height] = dish.size

        this.current = {
            colInProgress: 0,
            dish: dish,
            progress: { step: 0, max: width * height },
            info: '',
            total: 0
        }
        await this.updateUi(uiDelay)
        this.current.total = 0
        for(let col=0; col<width; col++) {
            const stones = dish.stonesOnCol.get(col)
            this.current.colInProgress = col
            if(!stones) {
                continue
            }

            const rocks = dish.rocksOnCol.get(col) || new Set<number>()
            const eachStone = Array.from(stones.values())

            for(let i=0; i<eachStone.length; i++) {
                const s = eachStone[i]
                let y=s
                for(; y > 0; y--) {
                    if(stones.has(y - 1) || rocks.has(y - 1)) {
                        break
                    }
                }

                stones.delete(s)
                stones.add(y)
                this.current.progress.step++
                
                // await this.updateUi(uiDelay)
            }

            this.current.total += Array.from(stones.values()).reduce((s, y) => s + (height - y), 0)
            this.current.progress.step = (col + 1) * height;
            await this.updateUi(0)
        }

        this.current.colInProgress = -1
        this.current.progress.step += 1
        this.current.success = this.current.total === expectedTotal
        this.requestUpdate()
    }

    render() {
        const [width, height] = this.current?.dish?.size || [0, 0]
         
        return html`
        <style>
            .schematics {
                --dish-column: ${width};
                --dish-rows: ${height};
            }
        </style>
        <h1>Day14</h1>

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
        <section class="dish">
            ${this.renderGrid()}
        </section>
        `
    }

    renderGrid() {
        if (!this.current?.dish) {
            return nothing
        }

        const stones = this.current.dish.stonesOnCol
        const rocks = this.current.dish.rocksOnCol
        const selectedCol = this.current.colInProgress

        return html`
            ${Array.from(stones.keys()).map((col) => {
                const colStones = stones.get(col)!
                return Array.from(colStones.values()).map((row) => html`
                <span class=${classMap({stone: true, selected: col === selectedCol })} 
                    style=${styleMap({
                        "grid-column": `${col + 1} / span 1`,
                        "grid-row": `${row + 1} / span 1`
                    })}>
                    💎
                </span>`)
            })}

            ${Array.from(rocks.keys()).map((col) => {
                const colRocks = rocks.get(col)!
                return Array.from(colRocks.values()).map((row) => html`
                <span class=${classMap({rock: true, selected: col === selectedCol })} 
                    style=${styleMap({
                        "grid-column": `${col + 1} / span 1`,
                        "grid-row": `${row + 1} / span 1`
                    })}>
                    🪨
                </span>`)
            })}     
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

        section.dish  {
            margin-top: var(--top-spacing);
            display: grid;
            grid-template-columns: repeat(var(--dish-column), 2rem);
            grid-template-rows: repeat(var(--dish-rows), 2rem);
            outline: 1px solid white;
            min-height: 100px;
            font-size: 1rem;
        }
        section.dish span  {
            text-align: center;
        }

        section.dish span.selected  {
            outline: 1px solid white;
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
