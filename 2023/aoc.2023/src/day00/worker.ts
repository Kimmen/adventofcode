import {readLines, chunkBy } from '../helpers'

self.onmessage = (event: MessageEvent<{input: string, takeTop: number}>) => {
    const {input, takeTop} = event.data
    const caloriesPerInventory = chunkBy(readLines(input), (l) => !l)
        .map(x => x.map(l => parseInt(l)))
    
    const totalCalories = caloriesPerInventory.map((c, index) => {
        const sumPackage = c.reduce((p, c) => p + c, 0)
        console.log(`Package ${index}: ${sumPackage}`)
        return sumPackage
    })
    .sort((a, b) => b  - a)
    .slice(0, takeTop)
    .reduce((prev, curr) => {
        const sum = prev + curr
        console.log({
            prev, curr, sum
        })
        return sum
    }, 0);
    
    self.postMessage(totalCalories)
}
