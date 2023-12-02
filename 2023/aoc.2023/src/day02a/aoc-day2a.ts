import { LitElement, css, html, nothing } from 'lit'
import { customElement, state } from 'lit/decorators.js'

import { parseGame } from './parser';
import { button } from '../styles'
import { delay, readLines } from '../helpers';

import devData from './input.dev'
import data from './input'

type GameViewModel = {
    reds: number[]
    greens: number[]
    blues: number[]

    id: number
    runningTotal: number
    isExpectgedTotal: boolean
}

@customElement('aoc-day-2a')
export class AocDay2a extends LitElement {
    constructor() { super(); }

    @state() current: GameViewModel = {
        id: 0,
        runningTotal: 0,
        isExpectgedTotal: false,
        reds: [],
        greens: [],
        blues: []
    }

    reset() {
        this.current = {
            id: 0,
            runningTotal: 0,
            isExpectgedTotal: false,
            reds: [],
            greens: [],
            blues: []
        }
    }

    async updateUi(ms: number) {
        await delay(ms)
        this.requestUpdate()
    }

    async calculateFitness(input: string, expectedTotal: number) {
        const games = readLines(input).map(parseGame)
        for (let i = 0; i < games.length; i++) {
            const g = games[i]

            this.current.id = g.id

            g.sets.forEach(async s => {
                this.current.reds = [s.red]
                this.current.greens = [s.green]
                this.current.blues = [s.blue]
            })

            const impossible = g.sets.some(x => x.red > 12 || x.green > 13 || x.blue > 14)
            this.current.runningTotal += impossible ? 0 : g.id

            await this.updateUi(50)
        }

        this.current.isExpectgedTotal = this.current.runningTotal === expectedTotal
        this.requestUpdate()
    }

    async calculateMinBag(input: string, expectedTotal: number) {
        const games = readLines(input).map(parseGame)
        for (let i = 0; i < games.length; i++) {
            const g = games[i]

            this.current.id = g.id

            g.sets.forEach(async s => {
                this.current.reds = [s.red]
                this.current.greens = [s.green]
                this.current.blues = [s.blue]
            })

            const minBag = g.sets.reduce((prev, curr) => ({ 
                red: Math.max(prev.red, curr.red),
                green: Math.max(prev.green, curr.green),
                blue: Math.max(prev.blue, curr.blue),
            }) , {red: 0, green: 0, blue: 0})

            this.current.runningTotal += minBag.red * minBag.green * minBag.blue

            await this.updateUi(50)
        }

        this.current.isExpectgedTotal = this.current.runningTotal === expectedTotal
        this.requestUpdate()
    }

    startPart1Dev() {
        this.reset()
        this.calculateFitness(devData, 8)
    }

    startPart1() {
        this.reset()
        this.calculateFitness(data, 2416)
    }

    startPart2Dev() {
        this.reset()
        this.calculateMinBag(devData, 2286)
    }

    startPart2() {
        this.reset()
        this.calculateMinBag(data, 63307)
    }

    render() {
        return html`
        <h1>Day2</h1>
        <div class="input-selection">
            <button @click=${this.startPart1Dev}>Part1.Dev</button>
            <button @click=${this.startPart1} >Part1</button>
            <button @click=${this.startPart2Dev} >Part2.Dev</button>
            <button @click=${this.startPart2} >Part2</button>
        </div>
        <section class="game">
            ${!this.current
                ? nothing
                : html`
                    <div class="bag reds">${this.renderColoredCubes(this.current.reds)}</div>
                    <div class="bag greens">${this.renderColoredCubes(this.current.greens)}</div>
                    <div class="bag blues">${this.renderColoredCubes(this.current.blues)}</div>
                `}
        </section>
        <section class="info">
            <p class=${this.current.isExpectgedTotal ? 'success' : 'failed'}>Total: ${this.current?.runningTotal}</p>
            <p>GameId: ${this.current?.id}</p>
        </section>
        `
    }

    renderColoredCubes(sets: number[]) {
        const setTemplates = sets.map(c => html`<ul class="bag">${this.renderCubeSet(c)}</ul>`)

        return html`<span class="set-info">${sets.length}:</span> ${setTemplates}`
    }

    renderCubeSet(count: number) {
        return Array.from({ length: count }, () => html`<li class="cube"></li>`)
    }

    static styles = [button, css`
        :host {
            flex: 0 0 auto;
            --top-spacing: 2rem;
            --red: rgb(204, 51, 51);
            --green: rgb(64, 191, 96);
            --blue: rgb(57, 92, 198);
            --cube-size: 0.5rem;
        }

        .input-selection {
            display: flex;
            place-items: start;
            gap: 1rem;
        }

        section.game {
            margin-top: var(--top-spacing);
        }

        div.bag {
            display: flex;
            place-items: center;
            gap: var(--cube-size);
        }

        div.reds li.cube {
            background-color: var(--red);
        }
        div.greens li.cube {
            background-color: var(--green);
        }
        div.blues li.cube {
            background-color: var(--blue);
        }

        ul.bag {
            list-style: none;
            display: inline-flex;
            flex-wrap: wrap;
            gap: 3px;
            padding: 0;
        }

        li.cube {
            width: var(--cube-size);
            height: var(--cube-size);
        }
        span.set-info {
            text-align: center;
            height: 3rem;
            display: flex;
            place-items: center;
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
