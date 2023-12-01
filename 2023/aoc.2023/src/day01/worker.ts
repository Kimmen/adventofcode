import {readLines } from '../helpers'

self.onmessage = (event: MessageEvent<{input: string, numberExtractor: string}>) => {
    const {input, numberExtractor } = event.data
    const lines = readLines(input)

    const calibrations = lines.map((x, i) => {
        let extractor = (_: string) => [0]
        switch(numberExtractor) {
            case 'pureDigitExtractor' : 
                extractor = pureDigitExtractor
                break
            case 'readableExtractor':
                extractor = readableExtractor
                break
        }
        const numbers = extractor(x).filter(x => !isNaN(x))

        const result = {index: i, line: x, numbers }

        self.postMessage({type: 'parsed', ...result })

        return result
    })


    const sum = calibrations.map(({ numbers, index }) => {
        let calibration = { index: index, first: 0, last: 0, value: 0 } 
        if(numbers.length > 0) {
            const first = numbers[0]
            const last = numbers[numbers.length - 1]
            calibration = { index: index, first, last, value: first * 10 + last } 
        } 
       
        self.postMessage({type: 'calibration', ...calibration})
        return calibration;
    }).reduce((prev, current) => prev + current.value, 0)

    self.postMessage({type: 'calculation', sum})
}

export const pureDigitExtractor = (line: string) => line.match(/\d/g)!.map(x => parseInt(x))

export const readableExtractor = (line: string) =>  {
    return (line
        .replace('one', '1')
        .replace('two', '2')
        .replace('three', '3')
        .replace('four', '4')
        .replace('five', '5')
        .replace('six', '6')
        .replace('seven', '7')
        .replace('eight', '8')
        .replace('nine', '9')
        .match(/\d/g) || []).map(x => parseInt(x))
}
