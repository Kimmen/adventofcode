import { LitElement, css, html, nothing } from 'lit'
import { customElement, state } from 'lit/decorators.js'
import { repeat } from 'lit/directives/repeat.js';

import { EngineSchematic, parseEngineSchematics, posKey, Part, Pos } from './parser';
import { button } from '../styles'
import { delay } from '../helpers';

import devData from './input.dev'
import data from './input'
import { determinePerimeter } from './perimeter';


@customElement('aoc-day-3')
export class AocDay extends LitElement {
    constructor() { super(); }

    @state() schematics?: EngineSchematic
    @state() current?: {
        permiter: Pos[];
        key: string,
        part: Part,
        total: number,
        success: boolean
    }

    async updateUi(ms: number) {
        await delay(ms)
        this.requestUpdate()
    }

    reset() {
        this.schematics = undefined
        this.current = undefined
    }

    startPart1Dev() {
        this.reset()
        this.calculatePartNumberSums(devData, 4361)
    }

    startPart1() {
        this.reset()
        this.calculatePartNumberSums(data, 537732)
    }

    startPart2Dev() {
        this.reset()
    }

    startPart2() {
        this.reset()
    }

    async calculatePartNumberSums(data: string, expectedSum: number) {
        this.schematics = parseEngineSchematics(data)
        await this.updateUi(0)

        this.current = { key: '', part: {} as Part, permiter: [], total: 0, success: false }

        const symbols = []
        for (let s in this.schematics.symbols) {
            symbols.push(this.schematics.symbols[s].position)
        }

        for (let k in this.schematics.parts) {
            const part = this.schematics.parts[k];
            this.current.key = posKey(part.position.row, part.position.column)
            this.current.part = part

            this.current.permiter = determinePerimeter(part)

            if (symbols.some(({ row, column }) => this.current?.permiter.some(p => p.row === row && p.column === column))) {
                this.current.total += part.number
            }

            await this.updateUi(10)
        }

        this.current.success = this.current.total === expectedSum
        await this.updateUi(0)
    }

    render() {
        return html`
        <style>
            .schematics {
                --schematics-column: ${this.schematics ? this.schematics.engineSize.columns : 0};
                --schematics-rows: ${this.schematics ? this.schematics.engineSize.rows : 0};
            }
        </style>
        <h1>Day3</h1

        <div class="input-selection">
            <button @click=${this.startPart1Dev}>Part1.Dev</button>
            <button @click=${this.startPart1}>Part1</button>
            <button @click=${this.startPart2Dev} disabled>Part2.Dev</button>
            <button @click=${this.startPart2} disabled>Part2</button>
        </div>
        <section class="schematics">
          ${this.renderGrid()}
        </section>
        <section class="info">
            <p class=${this.current?.success ? 'success' : ''}>${this.current?.total}</p>
            <p>${this.current?.part.number}</p>
            <p>${this.current?.part.position.row} ${this.current?.part.position.column}</p>
        </section>
        `
    }

    renderGrid() {
        if (!this.schematics) {
            return nothing
        }

        const cells = []

        for (let row = 0; row < this.schematics.engineSize.rows; row++) {
            for (let column = 0; column < this.schematics.engineSize.columns; column++) {
                const key = posKey(row, column)
                const part = this.schematics.parts[key]
                const symbol = this.schematics.symbols[key]

                const isInPerimeter = this.current?.permiter.some(p => p.row === row && p.column === column)

                if (part) {
                    const isCurrentPart = this.current?.key == key

                    for (let w = 0; w < part.width; w++) {
                        cells.push({
                            key: posKey(row, column + w),
                            row: row,
                            column: column + w,
                            content: `${part.number}`[w],
                            class: isCurrentPart ? "part current" : "part"
                        })
                    }
                    column += part.width - 1
                }
                else if (symbol) {
                    cells.push({
                        key: key,
                        row: column,
                        column: row,
                        content: symbol.symbol,
                        class: isInPerimeter ? "symbol in-perimeter" : "symbol"
                    })
                }
                else {
                    cells.push({
                        key: key,
                        row: column,
                        column: row,
                        content: ".",
                        class: isInPerimeter ? "empty in-perimeter" : "empty"
                    })
                }

            }
        }

        return html`
            <div>
                ${repeat(cells, (c) => c.key, (c) => {
            return html`
                        <span class=${c.class}>
                                ${c.content}
                        </span>`})}
            </div>
        `
    }


    static styles = [button, css`
        :host {
            flex: 0 0 auto;
            --top-spacing: 2rem;
            --white: rgb(204, 51, 51);
            --red: rgb(204, 51, 51);
            --green: rgb(64, 191, 96);
            --yellow: rgb(217, 172, 38);
            --blue: rgb(57, 92, 198);
            --dark: rgb(48,48,48)
        }

        .input-selection {
            display: flex;
            place-items: start;
            gap: 1rem;
        }

        section.schematics div {
            margin-top: var(--top-spacing);
            display: grid;
            grid-template-columns: repeat(var(--schematics-column, 4), 1fr);
            grid-template-rows: repeat(var(--schematics-rows, 4), 1fr);
            outline: 1px solid white;
            min-height: 100px;
        }

        section.schematics span.symbol {
            font-style: italic; 
        }

        section.schematics span.part {
            font-weight: bold;
        }

        section.schematics span.empty {
            color: var(--dark);
        }

        section.schematics span.current {
            color: var(--green);
        }

        section.schematics span.in-perimeter {
            color: var(--yellow);
        }

        section.schematics span.in-perimeter.symbol {
            color: var(--blue);
        }
        
        section.info {
            outline: 1px dashed white;
            margin-top: var(--top-spacing);
            padding: 1rem;
        }

        p.success {
            color: var(--green)
        }

        p.failed {
            color: var(--red)
        }
        
        `]
}
