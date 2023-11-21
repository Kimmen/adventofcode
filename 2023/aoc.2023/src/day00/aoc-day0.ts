import { LitElement, css, html } from 'lit'
import { customElement, state } from 'lit/decorators.js'
import devData from './input.dev'
import data from './input'

import MyWorker from './worker?worker'

@customElement('aoc-day-0')
export class AocDay0 extends LitElement {
    private worker = new MyWorker()

    @state()
    calculatedSums: number[] = []

    constructor() {
        super();
    }

    connectedCallback() {
        super.connectedCallback();

        this.worker.onmessage = (event: MessageEvent<number>) => {
           this.calculatedSums.push(event.data)
           this.requestUpdate()
        };

        this.worker.postMessage({ input: data, takeTop: 3 })
    }

    render() {
        return html`
            <p>
                Day 0
            </p>

            ${this.calculatedSums.map(s => html`<div> ${s} </div>` )}
        `
    }

    static styles = css`
        :host {
            flex: 1 1 auto;
        }
        p {
            font-size: 48pt;
            text-align: center;
        }
        `
}