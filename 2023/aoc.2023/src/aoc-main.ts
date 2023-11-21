import { LitElement, css, html } from 'lit'
import { customElement, property, state } from 'lit/decorators.js'
import './day00/aoc-day0'
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
  selectedDay = 0;


  render() {
    return html`

    <nav >
      <a href="#">Advent of Code 2023</a>
      <ul class="list">
        <li><a href="#" @click=${() => this.selectedDay = 0}>Day 0</a></li>
        <li><a href="#" @click=${() => this.selectedDay = 1}>Day 1</a></li>
      </ul>
    </nav>
    <div class="day-container">
       <aoc-day-0 ></aoc-day-0>
    </div>
    `
  }

  static styles = css`
    :host {
      background-image: url('./src/assets/background.jpg');
      background-size: cover;
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
      flex: 1 1 auto;
      
      background: rgba(10, 10, 10, 0.9);
      border-radius: 16px;
      box-shadow: 0 4px 30px rgba(0, 0, 0, 0.1);
      backdrop-filter: blur(6.1px);
      -webkit-backdrop-filter: blur(6.1px);
      border: 1px solid rgba(75, 75, 75, 0.37);

      padding: 8rem;
      display: flex;
      place-items: stretch;
    }
  `
}

declare global {
  interface HTMLElementTagNameMap {
    'aoc-main': AocMain
  }
}
