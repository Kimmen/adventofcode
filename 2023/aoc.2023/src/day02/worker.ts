import {readLines } from '../helpers'

self.onmessage = (event: MessageEvent<{input: string }>) => {
    const {input } = event.data
    const lines = readLines(input)

   
}