import { LitElement, css, html, nothing } from 'lit'
import { customElement, state } from 'lit/decorators.js'
import './card'
import { repeat } from 'lit/directives/repeat.js';
import { classMap } from 'lit/directives/class-map.js';

import { Card, Hand, parseCamelCards } from './parser';
import { button } from '../styles'
import { delay, chunkWhile } from '../helpers';

import devData from './input.dev'
import data from './input'

type RankedHand = Hand & { grouped: Card[][], rank: number }

@customElement('aoc-day-7')
export class AocDay extends LitElement {
    constructor() {  super() }
    
    @state() rankedHands: RankedHand[] = []

    @state() current?: {
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
        this.calculateTotalWinnings(devData, 6440, 500)
    }

    startPart1() {
        this.reset()
        this.calculateTotalWinnings(data, 6440, 10)
    }

    startPart2Dev() {
        this.reset()
    }

    startPart2() {
        this.reset()
    }

    async calculateTotalWinnings(data: string, expectedTotal: number, uiDelay: number) {
        this.current = { info: "", success: false }

        const game = parseCamelCards(data)
        const hands = game.hands.map(h => ({...h, grouped: this.groupCards(h.cards)} as RankedHand))
        this.rankedHands = []

        for(let i=0; i<hands.length; i++) {
            const current = hands[i]
            this.rankedHands.push(current)
            this.rankedHands.sort(this.compareHand)
            this.rankedHands.forEach((x, index) => x.rank = index + 1 )

            this.current.info = current.cards.reduce((out, c) => out + `${c.symbol}`, 'Cards: ')
            this.current.total = this.rankedHands.reduce((acc, curr) => acc + (curr.bid * curr.rank) , 0)
            if(i % 100 === 0)
            {
                await this.updateUi(uiDelay)
            }
        }

        this.current.success = this.current?.total === expectedTotal
        this.updateUi(0)
    }

    groupCards(cards: Card[]): Card[][] {
        cards.sort((a, b) => b.value - a.value)
        return chunkWhile(cards, (c, n) => c.value == (n?.value || 0)).sort((a, b) => {
            let sort = b.length - a.length
            return sort === 0 ? b[0].value - a[0].value : sort 
        })
    }

    compareHand(a: RankedHand, b: RankedHand) : number {
        const determineHandKind = (hand: RankedHand): number => {
            
            if(hand.grouped[0].length === 5) {
                return 5
            }
            if(hand.grouped[0].length === 4) {
                return 4
            }
            if(hand.grouped[0].length === 3 && hand.grouped[1].length === 2) {
                return 3.5
            }
            if(hand.grouped[0].length === 3) {
                return 3
            }
            if(hand.grouped[0].length === 2 && hand.grouped[1].length === 2) {
                return 2.5
            }

            if(hand.grouped[0].length === 2) {
                return 2
            }

            return 1
        }
        
        const kindA = determineHandKind(a)
        const kindB = determineHandKind(b)
        const kindCompare = kindA - kindB
        
        if(kindCompare !== 0){
            return kindCompare
        } 

        for(let i=0; i< a.cards.length; i++) {
            const valueCompare = a.cards[i].value - b.cards[i].value
            if(valueCompare !== 0) {
                return valueCompare
            }
        }

        throw Error("same hand, can't compare")
    }

    determineColor({value}: Card) : "red"|"blue"|"black" {
        if(value > 10) return "red"
        if(value > 7) return "blue"
        return 'black'
    }

    render() {
        return html`
        <h1>Day7</h1

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
        <section class="hands">
            ${this.rankedHands && this.rankedHands.slice().reverse().map((hand) => html`
                <div class="hand">
                    <div class="bid">${hand.rank}: 🪙 ${hand.bid}</div>
                    ${hand.cards.map(c => html`<aoc-card value=${c.symbol} color=${this.determineColor(c)}></aoc-card>`)}
                </div>
                <div class="hand">
                    ${hand.grouped.map(g => html`
                        <div class="card-group">
                        ${
                            g.map(c => html`
                                <aoc-card value=${c.symbol} color=${this.determineColor(c)}></aoc-card>
                            `)
                        }
                        </div>`)}
                </div>`)}
        </section>
        `
    }

//     g.map(c => html`
//     <aoc-card value=${c.symbol} color=${this.determineColor(c)}></aoc-card>
// `))

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


        section.hands div.hand aoc-card {
          display: inline-flex;
        }

        section.hands div.hand .bid {
          color: var(--white);
          font-size: 1rem;
        }

        section.hands div.hand .card-group {
          display: inline;
          margin-right: 0.5rem;
        }

        section.hands div.hand .card-group *:not(:first-child) {
            margin-left: -1rem;
        }
       

        section.info {
            outline: 1px dashed white;
            margin-top: var(--top-spacing);
            margin-bottom: var(--top-spacing);
            padding: 1rem;
        }

        .input-selection {
            display: flex;
            place-items: start;
            gap: 1rem;
        }

        p.success {
            color: var(--green)
        }

        p.failed {
            color: var(--red)
        }
        
        `]
}
