import { LitElement, css, html } from 'lit'
import { customElement, state } from 'lit/decorators.js'
import { button } from '../styles'
import { delay } from '../helpers';

import devData from './input.dev'
import data from './input'

import { classMap } from 'lit/directives/class-map.js';
import { GalaxyMap, parseGalaxyMap } from './parser';

type Point = [x: number, y: number]

@customElement('aoc-day-11')
export class AocDay extends LitElement {
    constructor() {  super() }
    

    @state() current?: {
        map: GalaxyMap
        progress: {step: number, max: number}
        info: string
        total?: number
        success?: boolean
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
        this.part1(devData, 374, 1, 100)
    }

    startPart1() {
        this.reset()
        this.part1(data, 9648398, 1, 1)
    }

    startPart2Dev() {
        this.reset()
        // this.part1(devData, 1030, 10 - 1, 100)
        this.part1(devData, 8410, 100 - 1, 100)
    }

    startPart2() {
        this.reset()
        this.part1(data, 618800410814, 1_000_000 - 1, 1)
    }


    async part1(data: string, expectedTotal: number, expansionRate: number, uiDelay: number) {
        const map = parseGalaxyMap(data)
        this.current = {
            map,
            progress: { step: 0, max: map.galaxies.length },
            info: '',
            total: 0
        }

        let [width, height] = map.size
        const galaxies = map.galaxies

        for(let i=0; i<width; i++) {
            const hasGalaxy = galaxies.some(([x]) => x == i)
            if(hasGalaxy) {
                continue
            }

            galaxies.filter(([x]) => x > i).forEach(g => g[0] += expansionRate)
            width+= expansionRate
            i+= expansionRate
        }

        for(let i=0; i<height; i++) {
            const hasGalaxy = galaxies.some(([_, y]) => y == i)
            if(hasGalaxy) {
                continue
            }

            galaxies.filter(([_, y]) => y > i).forEach(g => g[1] += expansionRate)
            height+= expansionRate
            i+= expansionRate
        }

        this.current.total = 0

        for(let i=0; i<map.galaxies.length; i++) {
            const g1 = map.galaxies[i]
            for(let j=i+1; j<map.galaxies.length; j++) {
                const g2 = map.galaxies[j]
                const distance = this.manhattanDistance(g1, g2)

                this.current.total += distance
                this.current.info = `${g1[0]},${g1[1]} -> ${g2[0]},${g2[1]} : ${distance}` 

                if (j % 1000 == 0) {
                    await this.updateUi(uiDelay)
                }
            }
            this.current.progress.step = i
            await this.updateUi(0)
        }

        this.current.success = this.current.total === expectedTotal
        await this.updateUi(0)
    }

    manhattanDistance(point1: Point, point2: Point): number {
        const [x1, y1] = point1;
        const [x2, y2] = point2;
    
        const dx = Math.abs(x1 - x2);
        const dy = Math.abs(y1 - y2);
    
        return dx + dy;
    }

    render() {
        return html`
        <h1>Day11</h1>

        <div class="input-selection">
            <button @click=${this.startPart1Dev}>Part1.Dev</button>
            <button @click=${this.startPart1}>Part1</button>
            <button @click=${this.startPart2Dev} >Part2.Dev</button>
            <button @click=${this.startPart2} >Part2</button>
        </div>
        <section class="info">
            <p class=${classMap({done: this.current?.success !== undefined, success: !!this.current?.success, failed: !this.current?.success })}>
                ${this.current?.total}
            </p>
            <p>${this.current?.info || 'N/A'}</p>
            <progress class="progress" value=${this.current?.progress.step!} max=${this.current?.progress.max!}></progress>
        </section>
        <section>
        </section>
        `
    }

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

        #map {
            background-color: var(--white)
        }
        
        section.info {
            outline: 1px dashed white;
            margin-top: var(--top-spacing);
            margin-bottom: var(--top-spacing);
            padding: 1rem;
        }

        section.info .progress {
            width: 100%;
        }

        .input-selection {
            display: flex;
            place-items: start;
            gap: 1rem;
        }

        p.success.done {
            color: var(--green)
        }

        p.failed.done {
            color: var(--red)
        }
        
        `]
}
