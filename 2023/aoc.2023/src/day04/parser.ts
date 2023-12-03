import { readLines } from "../helpers"

export type ScratchCardsResult = {
    cards: Card[]
}

export type Card = {
    id: number
    winnings: number[]
    lottery: number[]
}

export const parseScratchCards = (data: string): ScratchCardsResult => {
    const lines = readLines(data)
    const regex = /^Card\W+(?<cardId>\d{1,2})\W+(?<winnings>(\d| )+)|(?<lottery>(\d| )+)$/

    const result: ScratchCardsResult = { cards: [] }

    result.cards = lines.map(line => {
        const [cardId, numbers] = line.split(':')
        const [winnings, lottery] = numbers.split('|')

        const card: Card = {
            id: Number(cardId.replace("Card", "").trim()),
            winnings: parseNumbers(winnings),
            lottery: parseNumbers(lottery)
        }

        return card
    })

    return result
}

const parseNumbers = (numberList: string): number[] => {
    return numberList
        .split(' ')
        .reduce((acc, n) => n.trim().length > 0
            ? [...acc, Number(n)]
            : acc,
            new Array<number>())

} 