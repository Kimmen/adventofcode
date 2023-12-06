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
        this.getWinningCount(devData, 288, 100)
    }

    startPart1() {
        this.reset()
        this.getWinningCount(data, 288, 100)
    }

    startPart2Dev() {
        this.reset()
    }

    startPart2() {
        this.reset()
    }

    async getWinningCount(data: string, expectedTotal: number, uiDelay: number) {
        this.current = {
            info: '',
            success: false,
            total: 1
        }
        this.competition = parseCompetition(data)
        await this.updateUi(0)

        const chartOptions: ApexCharts.ApexOptions = { 
            chart: {
                type: 'line'
            },
            series: [{
                data: []
            }],
            xaxis: {
                categories: []
            }
        };

        const chart = new ApexCharts(this.renderRoot.querySelector("#chart"), chartOptions);
        chart.render();

        for (let i = 0; i < this.competition.times.length; i++) {
            const time = this.competition.times[i]
            const distance = this.competition.distances[i]
            this.current.traveled = []
            let winCount = 0

            this.current.time = time

            await this.updateUi(uiDelay)
            chartOptions.xaxis = { categories: Array(time).fill(0).map((_, index) => index ) }
            chart.updateOptions(chartOptions)

            const racePoints: number[] = []
            for (let w = 0; w <= time; w++) {
                const travel = w * (time - w)
                racePoints.push(travel)
                chart.updateSeries([{ data: racePoints}], true)

                winCount += travel > distance ? 1 : 0
                this.current.info = "" + winCount;
                await this.updateUi(10)
            }

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
