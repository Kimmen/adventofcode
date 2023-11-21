export const readLines = (text: string): string[] => {
    return text.split("\n")
}

export const chunkBy = <T>(source: T[], match: (line: T) => boolean ): Array<T[]> =>  {
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

export const delay = (ms: number) => new Promise(res => setTimeout(res, ms));