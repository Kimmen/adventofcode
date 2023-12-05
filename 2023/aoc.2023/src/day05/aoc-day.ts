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
        currentSeed: number
        mappings: number[]
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
        this.determineLowestLocationNumber(devData, 35, 100)
    }

    startPart1() {
        this.reset()
        this.determineLowestLocationNumber(data, 35, 10)
    }

    startPart2Dev() {
        this.reset()
        this.determineLowestLocationNumberBySeedRange(devData, 46, 100)
    }

    startPart2() {
        this.reset()
        this.determineLowestLocationNumberBySeedRange(data, 46, 100)
    }

    async determineLowestLocationNumber(data: string, expectedTotal: number, uiDelay: number) {
        this.current = {
            currentSeed: 0,
            mappings: [],
            info: '0',
            success: false,
        }
        this.almanac = parseAlmanac(data)
        await this.updateUi(0)

        for(let i=0; i<this.almanac.seeds.length; i++) {
            this.current.mappings = []
            this.current!.currentSeed = this.almanac.seeds[i]
            this.current!.info = "" + this.almanac.seeds[i]
            await this.updateUi(uiDelay)
            
            let current = this.almanac.seedToSoil(this.current!.currentSeed)
            this.current.mappings?.push(current)
            await this.updateUi(uiDelay)

            current = this.almanac.soilToFertilizer(current)
            this.current.mappings?.push(current)
            await this.updateUi(uiDelay)

            current = this.almanac.fertilizerToWater(current)
            this.current.mappings?.push(current)
            await this.updateUi(uiDelay)

            current = this.almanac.waterToLight(current)
            this.current.mappings?.push(current)
            await this.updateUi(uiDelay)

            current = this.almanac.lightToTemperature(current)
            this.current.mappings?.push(current)
            await this.updateUi(uiDelay)

            current = this.almanac.temperatureToHumidity(current)
            this.current.mappings?.push(current)
            await this.updateUi(uiDelay)

            current = this.almanac.humidityToLocation(current)
            this.current.mappings?.push(current)

            this.current.total = this.current.total === undefined
                ? current
                : Math.min(this.current.total, current)
            this.current.success = this.current.total === expectedTotal
            await this.updateUi(uiDelay)
        }
    }

    async determineLowestLocationNumberBySeedRange(data: string, expectedTotal: number, uiDelay: number) {
        this.current = {
            currentSeed: 0,
            mappings: [],
            info: '0',
            success: false,
        }
        this.almanac = parseAlmanac(data)
        await this.updateUi(0)

        for(let i=0; i<this.almanac.seeds.length; i+=2) {
            this.current.mappings = []
            this.current!.currentSeed = this.almanac.seeds[i]
            this.current!.info = "" + this.almanac.seeds[i]
            await this.updateUi(uiDelay)

            const currentLength = this.almanac.seeds[i+1]
            
            for(let j=0; j < currentLength; j++) {
                let current = this.almanac.seedToSoil(this.current!.currentSeed + j)

                current = this.almanac.soilToFertilizer(current)
                current = this.almanac.fertilizerToWater(current)
                current = this.almanac.waterToLight(current)
                current = this.almanac.lightToTemperature(current)
                current = this.almanac.temperatureToHumidity(current)
                current = this.almanac.humidityToLocation(current)
                
                this.current.total = this.current.total === undefined
                    ? current
                    : Math.min(this.current.total, current)
            }
        }

        this.current.success = this.current.total === expectedTotal
        await this.updateUi(uiDelay)
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
            <p class=${this.current?.success ? 'success' : ''}>Lowsest: ${this.current?.total}</p>
            <p>${this.current?.info || 'N/A'}</p>
        </section>
        <section class="almanac">
            <div class="seeds">${this.almanac?.seeds.map(s => html`
                <span class=${this.current?.currentSeed === s ? "seed current" : "seed"}>${s}</span>`)}
            </div>
            ${this.current?.mappings?.map(n => html`<div class="mapping">${n}</div>`)}
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
