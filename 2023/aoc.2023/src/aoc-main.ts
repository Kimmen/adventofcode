import { LitElement, css, html, nothing } from 'lit'
import { customElement, property, state } from 'lit/decorators.js'
import './day00/aoc-day0'
import './day01/aoc-day1'
import './day02/aoc-day2'
import './day03/aoc-day'
import './day04/aoc-day'
import './day05/aoc-day'
import './day06/aoc-day'
import './day07/aoc-day'
import './day08/aoc-day'
import './day09/aoc-day'
import './day10/aoc-day'
import './day11/aoc-day'
/**
 * An example element.
 *
 * @slot - This element has a slot
 * @csspart button - The button
 */
@customElement('aoc-main')
export class AocMain extends LitElement {
  /**
   * Copy for the read the docs hint.
   */
  @property()
  docsHint = 'Click on the Vite and Lit logos to learn more'

  @state()
  selectedDay = 11;


  render() {
    return html`

    <nav >
      <a href="#">Advent of Code 2023</a>
      <ul class="list">
        <li><a href="#" @click=${() => this.selectedDay = 0}>Day 0</a></li>
        <li><a href="#" @click=${() => this.selectedDay = 1}>Day 1</a></li>
        <li><a href="#" @click=${() => this.selectedDay = 2}>Day 2</a></li>
        <li><a href="#" @click=${() => this.selectedDay = 3}>Day 3</a></li>
        <li><a href="#" @click=${() => this.selectedDay = 4}>Day 4</a></li>
        <li><a href="#" @click=${() => this.selectedDay = 5}>Day 5</a></li>
        <li><a href="#" @click=${() => this.selectedDay = 6}>Day 6</a></li>
        <li><a href="#" @click=${() => this.selectedDay = 7}>Day 7</a></li>
        <li><a href="#" @click=${() => this.selectedDay = 8}>Day 8</a></li>
        <li><a href="#" @click=${() => this.selectedDay = 9}>Day 9</a></li>
        <li><a href="#" @click=${() => this.selectedDay = 10}>Day 10</a></li>
        <li><a href="#" @click=${() => this.selectedDay = 11}>Day 11</a></li>
      </ul>
    </nav>
    <div class="day-container">
      ${this.selectedDay == 0 ? html`<aoc-day-0 ></aoc-day-0>` : nothing }
      ${this.selectedDay == 1 ? html`<aoc-day-1 ></aoc-day-1>` : nothing }
      ${this.selectedDay == 2 ? html`<aoc-day-2 ></aoc-day-2>` : nothing }
      ${this.selectedDay == 3 ? html`<aoc-day-3 ></aoc-day-3>` : nothing }
      ${this.selectedDay == 4 ? html`<aoc-day-4 ></aoc-day-4>` : nothing }
      ${this.selectedDay == 5 ? html`<aoc-day-5 ></aoc-day-5>` : nothing }
      ${this.selectedDay == 6 ? html`<aoc-day-6 ></aoc-day-6>` : nothing }
      ${this.selectedDay == 7 ? html`<aoc-day-7 ></aoc-day-7>` : nothing }
      ${this.selectedDay == 8 ? html`<aoc-day-8 ></aoc-day-8>` : nothing }
      ${this.selectedDay == 9 ? html`<aoc-day-9 ></aoc-day-9>` : nothing }
      ${this.selectedDay == 10 ? html`<aoc-day-10 ></aoc-day-10>` : nothing }
      ${this.selectedDay == 11 ? html`<aoc-day-11 ></aoc-day-11>` : nothing }
    </div>
    `
  }

  static styles = css`
    :host {
      background-image: url('./src/assets/background.jpg');
      background-size: contain;
      display: flex;
      place-items: stretch;
      padding: 10rem 3rem 3rem 3rem;
    }

    nav {
      padding: 1rem;
      padding-right: 3rem;
      position: fixed;
      top: 0;
      left: 0;
      width: 100vw;
      display: flex;
      align-items: center;
      transition: 0.3s ease-out;
      backdrop-filter: blur(8px) brightness(1.2);
      text-shadow: 0 0 5px rgba(0,0,0,0.5);
      color: white;
      font-size: 16px;
      z-index: 1;
    }
    .list {
      list-style-type: none;
      margin-left: auto;
      display: flex;
      
    }
    .list li {
      margin-right: 1rem;
    }
    .list li:last-child {
      margin-right: 2rem;
    }

    .day-container {
      flex: 1 0 auto;
      
      background: rgba(10, 10, 10, 0.9);
      border-radius: 16px;
      box-shadow: 0 4px 30px rgba(0, 0, 0, 0.1);
      backdrop-filter: blur(6.1px);
      -webkit-backdrop-filter: blur(6.1px);
      border: 1px solid rgba(75, 75, 75, 0.37);

      padding: 8rem;

    }
  `
}

declare global {
  interface HTMLElementTagNameMap {
    'aoc-main': AocMain
  }
}
