import { LitElement, css, html } from 'lit'
import { customElement, state } from 'lit/decorators.js'
import { button } from '../styles'
import { chunkWhile, delay } from '../helpers';

import devData from './input.dev'
import data from './input'

import { classMap } from 'lit/directives/class-map.js';
import { parseSpringRows } from './parser';
import { repeat } from 'lit/directives/repeat.js';

@customElement('aoc-day-12')
export class AocDay extends LitElement {
    constructor() {  super() }
    

    @state() current?: {
        arrangements: string[]
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
        this.part1(devData, 21, 100)
    }

    startPart1() {
        this.reset()
        this.part1(data, 7032, 1)
    }

    startPart2Dev() {
        this.reset()
    }

    startPart2() {
        this.reset()
    }


    async part1(data: string, expectedTotal: number, uiDelay: number) {
        const rows = parseSpringRows(data)
        this.current = {
            arrangements: [],
            progress: { step: 0, max: rows.length },
            info: '',
            total: 0
        }
    
        this.current.total = 0
        for(let i=0; i<rows.length; i++) {
            this.current.progress.step = i
            this.current.arrangements = []
            const { springs, conditionRecord } = rows[i]
            this.current.info = springs + conditionRecord.reduce((acc, n) => acc + n, '')
            this.requestUpdate()

            this.generateStrings(springs, conditionRecord)

            this.current.total += this.current.arrangements.length
            await this.updateUi(uiDelay)
        }

        this.current.progress.step += 1
        this.current.success = this.current.total === expectedTotal
        this.requestUpdate()
    }

    generateStrings(text: string, constraints: number[], currentString: string = "") : void {
        // Base case: if the text is empty, return the current string
        if (!text) {
            if(this.isValid(currentString, constraints)) {
                this.current?.arrangements.push(currentString)
                this.requestUpdate()
                return 
            }
            else {
                return
            }
        }

        if (!this.canBeValid(currentString, constraints)) {
            return
        }

        // If the current character is '?', add '.' and '#' to the string
        if (text[0] === '?') {
            this.generateStrings(text.slice(1), constraints, currentString + '.')
            this.generateStrings(text.slice(1), constraints, currentString + '#')
            return
        }

        // If the current character is '#', check if it is possible to add it to the current group
        if (text[0] === '#' && currentString) {
            if (currentString[currentString.length - 1] === '.') {
                this.generateStrings(text.slice(1), constraints, currentString + '#')
                return
            } else {
                for (let groupSize of constraints) {
                    if (currentString[currentString.length - 1] === '#' &&
                        currentString.split('.').pop()!.length === groupSize) {
                        continue
                    }

                    this.generateStrings(text.slice(1), constraints, currentString + '#')
                    return 
                }
            }
        }

        // If the current character is not '?' or '#', add it to the string
        this.generateStrings(text.slice(1), constraints, currentString + text[0])
    }

    canBeValid(text: string, constraints: number[]) : boolean {
        const damagedChunks = text.split('.')
            .filter(c => c.length > 0)

        if(damagedChunks.length > constraints.length) {
            return false
        }
        
        for(let i=0; i<constraints.length && i<damagedChunks.length; i++) {
            const chunk = damagedChunks[i]
            if(chunk.length > constraints[i]) return false;
        }

        return true;
    }

    isValid(text: string, constraints: number[]) : boolean {
        const damagedChunks = text.split('.')
            .filter(c => c.length > 0)

        if(damagedChunks.length !== constraints.length) {
            return false
        }
        
        return damagedChunks.every((c, i) => c.length === constraints[i])
    }

    render() {
        return html`
        <h1>Day12</h1>

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
        <section class="arrangements">
            ${this.current?.arrangements && repeat(this.current.arrangements, n => n, n => html`
                <div class="arrangement">${n}</div>
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

        #map {
            background-color: var(--white)
        }

        section.arrangments .arrangement {
            font-size: 0.76rem;
            letter-spacing: 1rem;
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
