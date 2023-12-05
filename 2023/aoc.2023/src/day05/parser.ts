import { chunkBy, readLines } from "../helpers"

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

    chunkBy(lines, l => !l.trim()).forEach(chunk => {
        const line = chunk[0]
        if(line.startsWith("seeds:")) {
            result.seeds = line.split(':')[1].trim().split(' ').map(n => Number(n))
        }
        else if(line.startsWith("seed-to-soil map:")) {
            result.seedToSoil = parseMapper<Seed, Soil>(chunk)
        }
        else if(line.startsWith("soil-to-fertilizer map:")) {
            result.soilToFertilizer = parseMapper<Soil, Fertilizer>(chunk)
        }
        else if(line.startsWith("fertilizer-to-water map:")) {
            result.fertilizerToWater = parseMapper<Fertilizer, Water>(chunk)
        }
        else if(line.startsWith("water-to-light map:")) {
            result.waterToLight = parseMapper<Water, Light>(chunk)
        }
        else if(line.startsWith("light-to-temperature map:")) {
            result.lightToTemperature = parseMapper<Light, Temperature>(chunk)
        }
        else if(line.startsWith("temperature-to-humidity map:")) {
            result.temperatureToHumidity = parseMapper<Temperature, Humidity>(chunk)
        }
        else if(line.startsWith("humidity-to-location map:")) {
            result.humidityToLocation = parseMapper<Humidity, Location>(chunk)
        }
        else{
            throw new Error("Unmapped chunk")
        }
    })
    return result
}

const parseMapper = <TSource extends number, TDestination extends number>(chunk: string[]): (source: TSource) => TDestination => {
    chunk.shift()
    const mapDefs = chunk.map(l => {
        const numbers = l.split(' ').map(n => Number(n))
        return { destination: numbers[0], source: numbers[1], length: numbers[2] } as MapDef
    })

    return createMapFunction<TSource, TDestination>(mapDefs)
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