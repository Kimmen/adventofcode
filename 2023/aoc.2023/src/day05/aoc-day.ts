import { LitElement, css, html, nothing } from 'lit'
import { customElement, state } from 'lit/decorators.js'
import { repeat } from 'lit/directives/repeat.js';
import { classMap } from 'lit/directives/class-map.js';

import { Almanac, parseAlmanac } from './parser';
import { button } from '../styles'
import { delay } from '../helpers';

import devData from './input.dev'
import data from './input'


@customElement('aoc-day-5')
export class AocDay extends LitElement {
    constructor() { super(); }

    @state() almanac?: Almanac
    @state() current?: {
        currentSeed?: number
        info: string
        total: number
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
        this.determineLowestLocationNumber(devData, 35, 100)
    }

    startPart1() {
        this.reset()
    }

    startPart2Dev() {
        this.reset()
    }

    startPart2() {
        this.reset()
    }

    async determineLowestLocationNumber(data: string, expectedTotal: number, uiDelay: number) {
        this.current = {
            currentSeed: 0,
            info: '0',
            total: 0,
            success: false,
        }
        this.almanac = parseAlmanac(data)
        await this.updateUi(0)

        for(let i=0; i<this.almanac.seeds.length; i++) {
            this.current!.currentSeed = this.almanac.seeds[i]
            this.current!.info = "" + this.almanac.seeds[i]
            await this.updateUi(uiDelay)
        }
    }

    render() {
        return html`
        <h1>Day5</h1

        <div class="input-selection">
            <button @click=${this.startPart1Dev}>Part1.Dev</button>
            <button @click=${this.startPart1}>Part1</button>
            <button @click=${this.startPart2Dev} >Part2.Dev</button>
            <button @click=${this.startPart2} >Part2</button>
        </div>
        <section class="info">
            <p class=${this.current?.success ? 'success' : ''}>${this.current?.total}</p>
            <p>${this.current?.info}</p>
        </section>
        <section class="almanac">
            <div class="seeds">${this.almanac?.seeds.map(s => html`
                <span class=${this.current?.currentSeed === s ? "seed current" : "seed"}>${s}</span>`)}
            </div>
        </section>
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
            --dark: rgb(48,48,48);
            --purple: rgb(113, 57, 198);
        }

        .input-selection {
            display: flex;
            place-items: start;
            gap: 1rem;
        }

        section.almanac div.seeds {
            display: flex;
            gap: 1rem;
        }

        section.almanac span.seed.current {
            font-weight: bold;
        }

        section.info {
            outline: 1px dashed white;
            margin-top: var(--top-spacing);
            margin-bottom: var(--top-spacing);
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
