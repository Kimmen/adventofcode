import { LitElement, css, html } from "lit";
import { customElement, property } from "lit/decorators.js";

@customElement('aoc-card')
export class card extends LitElement {
    @property() value: string = ''
    @property() color: "red"|"blue"|"black" = "black"

    render() {
        return html`
            <div class=${`card ${this.color }`}>
                ${this.value}
            </div>
        `
    }

    static styles = css`
        :host {
            --card-size: 1rem;
        }
        .card {
            display: inline-block;
            width: var(--card-size);
            height: calc(var(--card-size) * 1.4);
            border: 1px solid #666;
            border-radius: .3em;
            padding: .25em;
            margin: 0 .5em .5em 0;
            text-align: center;
            font-size: 0.7rem; /* @change: adjust this value to make bigger or smaller cards */
            font-weight: normal;
            font-family: Arial, sans-serif;
            position: relative;
            background-color: #fff;
            box-shadow: .2em .2em .5em #333;
        }  

        .card {
            display: flex;
            place-content: center;
            place-items: center;
        }

        .card.red {
            color: rgb(204, 51, 51);
        }
        .card.blue {
            color: rgb(57, 92, 198);
        }
        .card.black {
            color: rgb(48,48,48);
        }
        
    `
}