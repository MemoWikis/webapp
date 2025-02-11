import { toHtml } from 'hast-util-to-html'
import { all, createLowlight } from 'lowlight'
import { max } from 'underscore'

const lowlight = createLowlight(all)

export function getHighlightedCode(oldHtml: string) {
    const root = lowlight.highlightAuto(oldHtml)
    const newHtml = toHtml(root)

    if (newHtml.length < oldHtml.length)
        return oldHtml
    else 
        return newHtml
}

export function random(minVal: any, maxVal: any, floatVal: any = 'undefined'): number {
    const randVal = minVal + (Math.random() * (maxVal - minVal));
    return <number>(typeof floatVal == 'undefined' ? Math.round(randVal) : randVal.toFixed(floatVal));
}

export function handleNewLine(str: string = '') {
    return str.replace(/(\\r)*\\n/g, '<br>')
}

export function getElementAtPath<T>(arr: NestedArray<T>, indexPath: IndexPath): T | undefined {
    let item: NestedArray<T> = arr

    for (const index of indexPath) {
        if (Array.isArray(item)) {
            item = item[index]
        } else {
            return undefined
        }
    }

    return item as T
}

export function removeElementAtPath<T>(arr: NestedArray<T>, indexPath: IndexPath): ElementAndNestedArray<T> | undefined {
    const pathCopy = [...indexPath]
    const targetIndex = pathCopy.pop()

    let targetArray: NestedArray<T> = arr
    for (const index of pathCopy) {
        if (Array.isArray(targetArray)) {
            targetArray = targetArray[index]
        } else {
            return undefined
        }
    }

    if (Array.isArray(targetArray) && typeof targetIndex === "number") {
        const removedElement = targetArray.splice(targetIndex, 1)
        return { element: removedElement[0] as T, array: arr }
    } else {
        return undefined
    }
}

export function addElementAtPath<T>(arr: NestedArray<T>, indexPath: IndexPath, element: T): void {
    const pathCopy = [...indexPath]
    const targetIndex = pathCopy.pop()

    let targetArray: NestedArray<T> = arr
    for (const index of pathCopy) {
        if (Array.isArray(targetArray)) {
            targetArray = targetArray[index]
        } else {
            throw new Error("Invalid index path: encountered non-array element before reaching target location");
        }
    }

    if (Array.isArray(targetArray) && typeof targetIndex === "number") {
        targetArray.splice(targetIndex, 0, element)
    } else {
        throw new Error("Invalid index path: did not resolve to array element")
    }
}

export function moveElement<T>(arr: NestedArray<T>, fromIndexPath: IndexPath, toIndexPath: IndexPath): void {
    const removed = removeElementAtPath(arr, fromIndexPath)
    if (removed) {
        addElementAtPath(removed.array, toIndexPath, removed.element)
    }
}

export function isValidEmail(email: string): boolean {
    const regex = /^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$/
    return regex.test(email)
}

export function abbreviateNumberToM(number: number, localeString: string = 'de-DE', mString: string = 'Mio'): string {
    let newNumber
    if (number < 1000000) {
        return number.toLocaleString(localeString)
    }
    else if (number >= 1000000 && number < 1000000000) {
        newNumber = number / 1000000
        return `${parseInt(newNumber.toFixed(2)).toLocaleString(localeString)} ${mString}.`
    }
    return ''
}

export function getLastElement<T>(arr: T[]): T | undefined {
    if (arr.length === 0) {
        return undefined;
    }
    return arr[arr.length - 1];
}

export function slugify(text:string) {
 return text
    .toString()
    .toLowerCase()
    .replace(/\s+/g, '-')
    .replace(/ä/g, 'ae')
    .replace(/ö/g, 'oe')
    .replace(/ü/g, 'ue')
    .replace(/ß/g, 'ss')
    .replace(/[^\w-]+/g, '')
    .replace(/--+/g, '-')
    .replace(/^-+/, '')
    .replace(/-+$/, '')
}

export function getRandomColor() {
    const letters = '0123456789ABCDEF'
    let color = '#'
    for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)]
    }
    return color
}

export function getRandomBrightColor() {
    let color = '#'

    const channels = [
        Math.floor(Math.random() * 128 + 128).toString(16).padStart(2, '0'), // Bright channel
        Math.floor(Math.random() * 128 + 128).toString(16).padStart(2, '0'), // Bright channel
        Math.floor(Math.random() * 128).toString(16).padStart(2, '0')        // Lower channel
    ]
    channels.sort(() => Math.random() - 0.5)
    color += channels[0] + channels[1] + channels[2]

    return color;
}

export function resizeBase64Img(base64: string, maxWidth: number): Promise<string> {
    return new Promise((resolve, reject) => {
        
        const img = new Image()
        
        img.onload = () => {
            if (maxWidth >= img.width)
                return resolve(base64)

            const ratio = img.width / img.height
            const newWidth = Math.min(maxWidth, img.width)
            const newHeight = newWidth / ratio

            const canvas = document.createElement('canvas')
            canvas.width = newWidth
            canvas.height = newHeight

            const ctx = canvas.getContext('2d')
            if (!ctx)
                return null
            ctx.drawImage(img, 0, 0, newWidth, newHeight)

            const resizedBase64 = canvas.toDataURL('image/png')

            resolve(resizedBase64)
        }

        img.onerror = (err) => {
            reject(err)
        }

        img.src = base64
    })
}

export const delay = (ms: number): Promise<void> => {
  return new Promise(resolve => setTimeout(resolve, ms));
}