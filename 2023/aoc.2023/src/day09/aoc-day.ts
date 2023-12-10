import { LitElement, css, html, nothing } from 'lit'
import { customElement, state } from 'lit/decorators.js'
import { button } from '../styles'
import { delay } from '../helpers';

import devData from './input.dev'
import data from './input'

import { History, parseSensoryData } from './parser';
import { classMap } from 'lit/directives/class-map.js';

type HistoryProcess = {
    entries: Entry[]
}

type Entry = {
    value: number
    isNew: boolean
}

@customElement('aoc-day-9')
export class AocDay extends LitElement {
    constructor() {  super() }
    
    @state() current?: {
        history: HistoryProcess[]
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
        this.part1(devData, 114, 100)
    }

    startPart1() {
        this.reset()
        this.part1(data, 2008960228, 1)
    }

    startPart2Dev() {
        this.reset()
        this.part2(devData, 2, 100)
    }

    startPart2() {
        this.reset()
        this.part2(data, 1097, 1)
    }


    async part1(data: string, expectedTotal: number, uiDelay: number) {
        const sensoryData = parseSensoryData(data)

        this.current = {
            history: [],
            info: "",
            progress: { step: 0, max: sensoryData.histories.length }
            
        }

        const nextHistoryProcess = (history: History) : HistoryProcess => {
            return {
                entries: [...history.values.map(x => ({ value: x, isNew: false} as Entry))]
            } 
        }

        for(let i=0; i<sensoryData.histories.length; i++) {
            
            let history = nextHistoryProcess(sensoryData.histories[i])
            this.current.history = [history]

            do {
                const next: Entry[] = [] 
                for(let j=0; j<history.entries.length - 1; j++) {
                    const c = history.entries[j]
                    const n = history.entries[j+1]
                    next.push({ value: n.value - c.value, isNew: false})
                }
                history = { entries: next }
                
                this.current.history.push(history)

                this.current.info = history.entries.reduce((a, n) => a + n.value + ", ", "")
                await this.updateUi(uiDelay)
            } while(history.entries.some(x => x.value != 0))

            const last = this.current.history.length - 1
            this.current.history[last].entries.push({ value: 0, isNew: true})

            for(let j=last-1; j >= 0; j--) {
                const curr = this.current.history[j]
                const prev = this.current.history[j+1]

                const last = curr.entries[curr.entries.length - 1]
                const incr = prev.entries[prev.entries.length - 1]
                curr.entries.push( { value: last.value + incr.value , isNew: true })

                this.current.info = curr.entries.reduce((a, n) => a + n.value + ", ", "")
                await this.updateUi(uiDelay)
            }

            const firstHistory = this.current.history[0]
            const entries = firstHistory.entries
            this.current.total = (this.current.total || 0) + Number(entries[0].value)
            this.current.progress.step = i + 1
            await this.updateUi(uiDelay)
        }

        this.current.success = this.current.total === expectedTotal
        await this.updateUi(0)
    }

    async part2(data: string, expectedTotal: number, uiDelay: number) {
        const sensoryData = parseSensoryData(data)

        this.current = {
            history: [],
            info: "",
            progress: { step: 0, max: sensoryData.histories.length }
            
        }

        const nextHistoryProcess = (history: History) : HistoryProcess => {
            return {
                entries: [...history.values.map(x => ({ value: x, isNew: false} as Entry))]
            } 
        }

        for(let i=0; i<sensoryData.histories.length; i++) {
            
            let history = nextHistoryProcess(sensoryData.histories[i])
            this.current.history = [history]

            do {
                const next: Entry[] = [] 
                for(let j=0; j<history.entries.length - 1; j++) {
                    const c = history.entries[j]
                    const n = history.entries[j+1]
                    next.push({ value: n.value - c.value, isNew: false})
                }
                history = { entries: next }
                
                this.current.history.push(history)

                this.current.info = history.entries.reduce((a, n) => a + n.value + ", ", "")
                await this.updateUi(uiDelay)
            } while(history.entries.some(x => x.value != 0))

            const last = this.current.history.length - 1
            this.current.history[last].entries.unshift({ value: 0, isNew: true})

            for(let j=last-1; j >= 0; j--) {
                const curr = this.current.history[j]
                const prev = this.current.history[j+1]

                const last = curr.entries[0]
                const incr = prev.entries[0]
                curr.entries.unshift( { value: last.value - incr.value , isNew: true })

                this.current.info = curr.entries.reduce((a, n) => a + n.value + ", ", "")
                await this.updateUi(uiDelay)
            }

            const firstHistory = this.current.history[0]
            const entries = firstHistory.entries
            this.current.total = (this.current.total || 0) + Number(entries[0].value)
            this.current.progress.step = i + 1
            await this.updateUi(uiDelay)
        }

        this.current.success = this.current.total === expectedTotal
        await this.updateUi(0)
    }


    render() {
        return html`
        <h1>Day9</h1

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
        <section class="history">
            ${this.current?.history.map(h => html`
                <div class="line">
                    ${h.entries.map(e => html`
                        <span class=${classMap({ entry: true, "is-new": e.isNew})}>
                            ${e.value}
                        </span>
                    `)}
                </div>
            `)}
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

        section.history {
            display: flex;
            flex-direction: column;
            place-items: center;
        }

        section.history div.line {
            margin-bottom: 1rem;
            display: flex;
            justify-content: space-evenly;
            width: 50dvw;
        }

        section.history span.entry {
            font-size: 1.5rem;
        }

        section.history span.entry.is-new {
            font-weight: bold;
            text-shadow:
            0 0 2px #fff,
            0 0 3px #fff,
            0 0 5px #fff;
        }

        section.history span.entry.placeholder {
            color: transparent
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
