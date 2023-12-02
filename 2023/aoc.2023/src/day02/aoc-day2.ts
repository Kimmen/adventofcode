import { LitElement, css, html } from 'lit'
import { customElement, state } from 'lit/decorators.js'
import {repeat} from 'lit/directives/repeat.js';
import devData from './input.dev'
import devData2 from './input.dev.two'
import data from './input'

import {button} from '../styles'

import MyWorker from './worker?worker'


@customElement('aoc-day-2')
export class AocDay2 extends LitElement {
    private worker = new MyWorker()

    constructor() {
        super();
    }

    connectedCallback() {
        super.connectedCallback();

        this.worker.onmessage = ({data}: MessageEvent<string>) => {
            
            
            this.requestUpdate()
         };
    }

    startPart1Dev() {
       
        this.worker.postMessage({ input: devData })
    }

    startPart1() {
        this.worker.postMessage({ input: devData })
    }

    startPart2Dev() {
        this.worker.postMessage({ input: devData })
    }

    startPart2() {
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
        <section>
           
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

            /* CSS */
        
        `]
}