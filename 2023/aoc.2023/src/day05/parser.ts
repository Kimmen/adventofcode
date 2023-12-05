import { readLines } from "../helpers"

export type Seed = number
export type Soil = number
export type Fertilizer = number
export type Water = number
export type Light = number
export type Temperature = number
export type Humidity = number
export type Location = number

export type Almanac = {
    seeds: Seed[]

    seedToSoil: (seedNumber: Seed) => Soil
    soilToFertilizer: (number: Soil) => Fertilizer
    fertilizerToWater: (number: Fertilizer) => Water
    waterToLight: (number: Water) => Light
    lightToTemperature: (number: Light) => Temperature
    temperatureToHumidity: (number: Temperature) => Humidity
    humidityToLocation: (number: Humidity) => Location
}

export const parseAlmanac = (data: string): Almanac => {
    const lines = readLines(data)
    const result: Almanac = {
        seeds: [],
        seedToSoil: createMapFunction<Seed, Soil>([]),
        soilToFertilizer: createMapFunction<Soil, Fertilizer>([]),
        fertilizerToWater: createMapFunction<Fertilizer, Water>([]),
        waterToLight: createMapFunction<Water, Light>([]),
        lightToTemperature: createMapFunction<Light, Temperature>([]),
        temperatureToHumidity: createMapFunction<Temperature, Humidity>([]),
        humidityToLocation: createMapFunction<Humidity, Location>([]),
    }

    //TODO: parse lines
    return result
}

type MapDef = { destination: number, source: number, length: number }

const createMapFunction = <TSource extends number, TDestination extends number>(map: MapDef[]): (source: TSource) => TDestination => {
    return (source: TSource) => {
        const match = map.find(m => source >= m.source && source <= (m.source + m.length))
        if (!match) {
            throw Error("no match")
        }

        const index = match.source - source
        return (match.destination + index) as TDestination
    }
} 