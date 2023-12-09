import { readLines } from "../helpers"

export type CamelCards = {
    hands: Hand[]
}

export type CardSymbol =  'A' | 'K' | 'Q' | 'J' | 'T' | '9' | '8' | '7' | '6' | '5' | '4' | '3' | '2';

export type Card = {
    symbol: CardSymbol
    value: number
}

export type Hand = {
    bid: number
    cards: Card[]
}

export const parseCamelCards = (data: string): CamelCards => {
    const lines = readLines(data)
    const validRanks: CardSymbol[] = ['A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2']
    validRanks.reverse()
    const hands = lines
        .map(l => l.split(' '))
        .map(([cards, bid]) => {
            
            return {
                bid: Number(bid),
                cards: parseCards(cards, validRanks)
            } as Hand
        })

    return { hands }
}


const parseCards = (cardsString: string, validRanks: CardSymbol[]): Card[] => {
    const cards: Card[] = []
    for(let i = 0; i < cardsString.length; i++) {
        const symbol = cardsString[i] as CardSymbol
        const value = validRanks.indexOf(symbol)

        if(value < 0) throw new Error("no card rank found")

        cards.push({ symbol, value })
    }

    return cards
}