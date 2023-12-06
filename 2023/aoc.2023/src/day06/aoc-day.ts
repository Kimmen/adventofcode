import { LitElement, css, html, nothing } from 'lit'
import { customElement, state } from 'lit/decorators.js'
import { repeat } from 'lit/directives/repeat.js';
import { classMap } from 'lit/directives/class-map.js';

import { RaceCompetition, parseCompetition } from './parser';
import { button } from '../styles'
import { delay } from '../helpers';

import devData from './input.dev'
import data from './input'
import ApexCharts from 'apexcharts';


@customElement('aoc-day-6')
export class AocDay extends LitElement {
    constructor() {  super() }
    
    @state() competition?: RaceCompetition
    @state() current?: {
        time?: number
        traveled?: number[]
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
        this.getWinningCount(devData,r => r, 288, 100)
    }

    startPart1() {
        this.reset()
        this.getWinningCount(data, r => r, 1083852, 10)
    }

    startPart2Dev() {
        this.reset()
        this.getWinningCount(devData, this.aggregateRaces, 71503, 100)
    }

    startPart2() {
        this.reset()
        this.getWinningCount(data, this.aggregateRaces, 1083852, 10)
    }

    aggregateRaces(r: RaceCompetition) : RaceCompetition {
        return {
            times: [ Number(r.times.reduce((a, n) => `${a}${n}` , ''))],
            distances: [ Number(r.distances.reduce((a, n) => `${a}${n}` , ''))]
        }
    }

    async getWinningCount(data: string, transformParse: (input: RaceCompetition) => RaceCompetition,  expectedTotal: number, uiDelay: number) {
        this.current = {
            info: '',
            success: false,
            total: 1
        }
        this.competition = transformParse(parseCompetition(data))
        await this.updateUi(0)


        for (let i = 0; i < this.competition.times.length; i++) {
            const time = this.competition.times[i]
            const distance = this.competition.distances[i]
            this.current.traveled = []
            let winCount = 0

            this.current.time = time

            await this.updateUi(uiDelay)
           
            const pickIndex = (range: number[]) => {
                return range[0] + Math.floor((range[1] - range[0])/2)
            } 

            let searchRange = [1, Math.ceil(time / 2)]
            let done = false
        
            do {
                const w = pickIndex(searchRange)
                const v = w-1
                const travelCurr = w * (time - w)
                const travelAfter = v * (time - v)

                if(travelCurr > distance && travelAfter <= distance) {
                    done = true
                    winCount = time - (w-1) * 2 - 1
                }
                else if(travelCurr < distance) {
                    searchRange = [w, searchRange[1]]
                }
                else {
                    searchRange = [searchRange[0], w]
                }

                this.current.info = `${w}: ${travelCurr}`
                await this.updateUi(uiDelay)
            } while(!done)

            this.current.total! *= winCount
            await this.updateUi(uiDelay)
        }

        this.current.success = expectedTotal === this.current.total
        await this.updateUi(0)
    }

    render() {
        return html`
        <h1>Day6</h1

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
        <section class="competition">
            <div class="times"><span class="label">Time:</span>${this.competition && this.competition.times.map(n => html`
                <span class=${this.current?.time === n ? "ms current" : "ms"}>${n}</span>
            `)}</div>
            <div class="distances"><span class="label">Distance:</span>${this.competition && this.competition.distances.map(n => html`
                <span class="mm">${n}</span>
            `)}</div>

            <div id="chart" />
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

        section.competition > div {
            display: flex;
            justify-items: end;
        }

        section.competition > div span {
            flex: 1 1 auto;
        }

        section.competition > div span.current {
            font-weight: bold;
        }

        section.competition > div .label {
            width: 5rem;
        }

        p.success {
            color: var(--green)
        }

        p.failed {
            color: var(--red)
        }
        
        `]
}
