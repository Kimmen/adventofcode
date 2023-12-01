import { LitElement, css, html } from 'lit'
import { customElement, state } from 'lit/decorators.js'
import {repeat} from 'lit/directives/repeat.js';
import devData from './input.dev'
import devData2 from './input.dev.two'
import data from './input'

import MyWorker from './worker?worker'

type WorkerResult = {
    type: string
    index: number
    line: string
    numbers: number[]
    first: number
    last: number
    value: number
}

type ParsedResult = {
    type: string
    index: number
    line: string
    numbers: number[]
}

type CalibrationResult = {
    type: string
    index: number
    line: string
    numbers: number[]
}

type CalculationResult = {
    type: string
    sum: number
}

@customElement('aoc-day-1')
export class AocDay1 extends LitElement {
    private worker = new MyWorker()

    @state()
    lines: WorkerResult[] = []

    @state()
    total: number | null = null
    expected: number = 0

    constructor() {
        super();
    }

    connectedCallback() {
        super.connectedCallback();

        this.worker.onmessage = ({data}: MessageEvent<ParsedResult | CalibrationResult | CalculationResult>) => {
            
            switch(data.type) {
                case 'parsed': 
                    const parsed = data as ParsedResult
                    this.lines.push({...parsed} as WorkerResult)
                    break;
                case 'calibration':
                    const calibration = data as CalibrationResult
                    const line = this.lines[calibration.index]
                    this.lines[calibration.index] = {...line, ...calibration}
                    break;
                case 'calculation': 
                    const calculation = data as CalculationResult
                    this.total = calculation.sum
                    break;
            }
            
            this.requestUpdate()
         };
    }

    startPart1Dev() {
        this.lines = []
        this.total = null
        this.expected = 142
        this.worker.postMessage({ input: devData, numberExtractor: 'pureDigitExtractor' })
    }

    startPart1() {
        this.lines = []
        this.total = null
        this.expected = 56397
        this.worker.postMessage({ input: data, numberExtractor: 'pureDigitExtractor' })
    }

    startPart2Dev() {
        this.lines = []
        this.total = null
        this.expected = 281
        this.worker.postMessage({ input: devData2, numberExtractor: 'readableExtractor' })
    }

    startPart2() {
        this.lines = []
        this.total = null
        this.expected = 281
        this.worker.postMessage({ input: data, numberExtractor: 'readableExtractor' })
    }

    render() {
        return html`
        <h1>Day1</h1>
        <div class="input-selection">
            <button @click=${this.startPart1Dev}>Part1.Dev</button>
            <button @click=${this.startPart1}>Part1</button>
            <button @click=${this.startPart2Dev}>Part2.Dev</button>
            <button @click=${this.startPart2}>Part2</button>
        </div>
        <section class="${this.expected == this.total ? 'correct' : 'incorrect'}">
            Calculation: ${this.total} 
        </section>
        <section>
            ${repeat(this.lines, (line) => line.index, (line, index)=> html`
                <div>${index}: ${line.line} | ${line.numbers} | ${line.first}:${line.last} == ${line.value} </div>
            `)}
        </section>
        
        `
    }

    static styles = css`
        :host {
            flex: 0 0 auto;
        }

        .input-selection {
            display: flex;
            place-items: start;
            gap: 1rem;
        }

        .correct {
            color: green;
        }

        .incorrect {
            color: red;
        }

            /* CSS */
        button {
            min-width: 100px;
            align-items: center;
            appearance: none;
            background-color: #FCFCFD;
            border-radius: 4px;
            border-width: 0;
            box-shadow: rgba(45, 35, 66, 0.4) 0 2px 4px,rgba(45, 35, 66, 0.3) 0 7px 13px -3px,#D6D6E7 0 -3px 0 inset;
            box-sizing: border-box;
            color: #36395A;
            cursor: pointer;
            display: inline-flex;
            font-family: "JetBrains Mono",monospace;
            height: 48px;
            justify-content: center;
            line-height: 1;
            list-style: none;
            overflow: hidden;
            padding-left: 16px;
            padding-right: 16px;
            position: relative;
            text-align: left;
            text-decoration: none;
            transition: box-shadow .15s,transform .15s;
            user-select: none;
            -webkit-user-select: none;
            touch-action: manipulation;
            white-space: nowrap;
            will-change: box-shadow,transform;
            font-size: 18px;
        }

        button:focus {
            box-shadow: #D6D6E7 0 0 0 1.5px inset, rgba(45, 35, 66, 0.4) 0 2px 4px, rgba(45, 35, 66, 0.3) 0 7px 13px -3px, #D6D6E7 0 -3px 0 inset;
        }

        button:hover {
            box-shadow: rgba(45, 35, 66, 0.4) 0 4px 8px, rgba(45, 35, 66, 0.3) 0 7px 13px -3px, #D6D6E7 0 -3px 0 inset;
            transform: translateY(-2px);
        }

        button:active {
            box-shadow: #D6D6E7 0 3px 7px inset;
            transform: translateY(2px);
        }
        `
}