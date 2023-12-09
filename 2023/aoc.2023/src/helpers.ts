export const readLines = (text: string): string[] => {
    return text.split("\n")
}

export const chunkBy = <T>(source: T[], match: (line: T) => boolean): Array<T[]> =>  {
    const dest: Array<T[]> = [];
    let index = 0;
    let chunkStart = 0;

    while(index < source.length) {
        if(match(source[index])) {
            dest.push(source.slice(chunkStart, index));
            chunkStart = index + 1;
        }

        index++;
    } 


    return dest;
}

export const chunkWhile = <T>(source: T[], keep: (current: T, next: T) => boolean): Array<T[]> =>  {
    const dest: Array<T[]> = [];
    let index = 0;
    let chunkStart = 0;

    while(index < source.length) {
        if(!keep(source[index], source[index  + 1])) {
            dest.push(source.slice(chunkStart, index + 1));
            chunkStart = index + 1;
        }

        index++
    }

    dest.push(source.slice(chunkStart, source.length));

    return dest;
}

export const groupBy = <T, K extends keyof any>(arr: T[], key: (i: T) => K) =>
  arr.reduce((groups, item) => {
    (groups[key(item)] ||= []).push(item);
    return groups;
  }, {} as Record<K, T[]>);

export const delay = (ms: number) => new Promise(res => setTimeout(res, ms));

(Array.prototype as any).groupBy = groupBy