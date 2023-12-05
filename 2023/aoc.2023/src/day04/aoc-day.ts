import { LitElement, css, html, nothing } from 'lit'
import { customElement, state } from 'lit/decorators.js'
import { repeat } from 'lit/directives/repeat.js';

import { parseScratchCards, Card } from './parser';
import { button } from '../styles'
import { delay } from '../helpers';

import devData from './input.dev'
import data from './input'
import { classMap } from 'lit/directives/class-map.js';

type CardViewModel = Card & {
    matchings: number[]
    metric?: number
}
@customElement('aoc-day-4')
export class AocDay extends LitElement {
    constructor() { super(); }

    @state() scratchCards: CardViewModel[] = []
    @state() current?: {
        info: string,
        total: number,
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
        this.calculateWinningPoints(devData, 13, 100)
    }

    startPart1() {
        this.reset()
        this.calculateWinningPoints(data, 24733, 10)
    }

    startPart2Dev() {
        this.reset()
        this.countAccumulatedScrachedCards(devData, 30, 100)
    }

    startPart2() {
        this.reset()
        this.countAccumulatedScrachedCards(data, 5422730, 10)
    }

    async calculateWinningPoints(data: string, expectedTotal: number, uiDelay: number) {
        this.scratchCards = parseScratchCards(data).cards.map(c => ({ ...c, matchings: [] }))
        this.current = {
            info: "",
            total: 0,
            success: false
        }

        for (let i = 0; i < this.scratchCards.length; i++) {
            const card = this.scratchCards[i]
            card.matchings = card.lottery.filter(n => card.winnings.includes(n))
            card.metric = card.matchings.length > 0 ? Math.pow(2, card.matchings.length - 1) : 0

            this.current.info = `Card ${card.id}: ${card.metric}`
            this.current.total += card.metric ?? 0
            this.current.success = this.current.total === expectedTotal
            await this.updateUi(uiDelay)
        }
    }

    async countAccumulatedScrachedCards(data: string, expectedTotal: number, uiDelay: number) {
        this.scratchCards = parseScratchCards(data).cards.map(c => ({ ...c, matchings: [], metric: 1 }))
        this.current = {
            info: "",
            total: 0,
            success: false
        }

        for (let i = 0; i < this.scratchCards.length; i++) {
            const card = this.scratchCards[i]
            card.matchings = card.lottery.filter(n => card.winnings.includes(n))
            
            for(let m = 1; m <= card.matchings.length && (m+i) < this.scratchCards.length; m++) {
                this.scratchCards[m+i].metric!+= card.metric!
            }
            
            this.current.info = `Card ${card.id}: ${card.metric}`
            this.current.total += card.metric ?? 0
            this.current.success = this.current.total === expectedTotal
            await this.updateUi(uiDelay)
        }
    }

    render() {
        return html`
        <h1>Day4</h1

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
        <section class="scratch-cards">
            ${repeat(this.scratchCards, (c) => c.id, (c) => html`
                <div class="card">
                    <span class="id">Card ${c.id}</span>
                    <span class="winnings">${this.renderNumbers(c, c.winnings)}</span>
                    <span class="lottery">${this.renderNumbers(c, c.lottery)}</span>
                    <span class="points">${c.metric}</span>
                </div>
            `)}
        </section>
        `
    }

    renderNumbers(card: CardViewModel, numbers: number[]) {
        return numbers.map(n => {
            const isWinningLotteryNumber = card.matchings.includes(n)

            return html`<span class=${classMap({ number: true, winning: isWinningLotteryNumber })}>${n}</span>`
        })
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

        section.scratch-cards .card {
            margin-bottom: 0.5rem;
            display: flex;
            place-items: start;
            gap: 1rem;
        }

        section.scratch-cards .card .winnings {
            color: var(--yellow);
            display: flex; 
            gap: 5px;
        }

        section.scratch-cards .card .lottery {
            color: var(--purple);
            display: flex; 
            gap: 5px;
        }


        section.scratch-cards .card .id {
            margin-bottom: 0.5rem;
        }

        section.scratch-cards .card .winning {
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
