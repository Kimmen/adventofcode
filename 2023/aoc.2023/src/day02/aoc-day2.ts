import { LitElement, css, html } from 'lit'
import { customElement, state } from 'lit/decorators.js'
import {repeat} from 'lit/directives/repeat.js';
import devData from './input.dev'
import devData2 from './input.dev.two'
import data from './input'

import {button} from '../styles'

import MyWorker from './worker?worker'
import {Bag, Game, CubeSet, GameResult, Result, GameEvalResult, EndResult } from './worker'



@customElement('aoc-day-2')
export class AocDay2 extends LitElement {
    private worker = new MyWorker()

    constructor() {
        super();
    }
    @state() games: Game[] = []
    @state() sum: number | null = null 
    @state() isExpectedSum: boolean = false 

    connectedCallback() {
        super.connectedCallback();

        this.worker.onmessage = ({data}: MessageEvent<Result>) => {
            
            switch(data.type) {
                case 'game' : 
                    const gr = data as GameResult
                    this.games.push(gr.game)
                    break
                case 'eval': 
                    const er = data as GameEvalResult
                    const game = this.games.find(g => g.gameId == er.game.gameId)
                    if(game) {
                        game.impossible = er.game.impossible
                    }
                    break

                case 'end': 
                    const end = data as EndResult
                    this.sum = end.sum
                    this.isExpectedSum = end.isExpected
                    break
            }
            
            this.requestUpdate()
         };
    }

    startPart1Dev() {
       this.games = []
       this.sum = 0
       this.isExpectedSum = false
        this.worker.postMessage({ input: devData, bag: { red: 12, green: 13, blue: 14 } as Bag, expectedSum: 8 })
    }

    startPart1() {
        this.games = []
        this.sum = 0
       this.isExpectedSum = false

       this.worker.postMessage({ input: data, bag: { red: 12, green: 13, blue: 14 } as Bag, expectedSum: 8 })
    }

    startPart2Dev() {
        this.games = []
        this.sum = 0
       this.isExpectedSum = false

        this.worker.postMessage({ input: devData })
    }

    startPart2() {
        this.games = []
        this.sum = 0
       this.isExpectedSum = false

        this.worker.postMessage({ input: devData })
    }

    render() {
        return html`
        <h1>Day2</h1>
        <div class="input-selection">
            <button @click=${this.startPart1Dev}>Part1.Dev</button>
            <button @click=${this.startPart1}>Part1</button>
            <button @click=${this.startPart2Dev} disabled>Part2.Dev</button>
            <button @click=${this.startPart2} disabled>Part2</button>
        </div>
        <section class=${this.isExpectedSum ? 'success' : ''}>
            IdSum: ${this.sum} ${this.isExpectedSum}
            </section>
        <section class="games">
            ${repeat(this.games, (g) => g.gameId, (g, index)=> html`
                <div class=${g.impossible ? 'impossible' : ''}>
                    <p>${index}-${g.gameId}</p>
                        ${g.sets.map(s => html`
                            <ul>
                                <li class='green'>${s.green}</li>
                                <li class='red'>${s.red}</li>
                                <li class='blue'>${s.blue}</li>
                            </ul>
                        `)}
                    </ul>
                </div>
            `)}
        </section>
        
        `
    }

    static styles = [button, css`
        :host {
            flex: 0 0 auto;
        }

        .input-selection {
            display: flex;
            place-items: start;
            gap: 1rem;
        }

        section.games div.impossible {
            text-decoration: line-through;
            color: rgb(64,64,64)
        }

        section.success {
            color: rgb(64, 191, 96);
        }

        section.games p {
            font-weight: bold;
        }

        section.games ul {
            list-style: none;
            display: inline-flex;
            padding-left: 1rem;
            margin-right: 1rem;
            gap: 0.5rem;
        }

        section.games li.green {
            color: rgb(64, 191, 96);
        }

        section.games li.blue {
            color: rgb(57, 92, 198);
        }

        section.games li.red {
            color: rgb(204, 51, 51);
        }
        
        `]
}