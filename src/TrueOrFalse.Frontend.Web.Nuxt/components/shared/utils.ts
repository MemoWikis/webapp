import { lowlight } from 'lowlight/lib/core'
import { toHtml } from 'hast-util-to-html'

export function getHighlightedCode(oldHtml: string) {
    var lowlightNode = lowlight.highlightAuto(oldHtml)
    var newHtml = toHtml(lowlightNode)
    return newHtml
}

export function random(minVal: any, maxVal: any, floatVal: any = 'undefined'): number {
    var randVal = minVal + (Math.random() * (maxVal - minVal));
    return <number>(typeof floatVal == 'undefined' ? Math.round(randVal) : randVal.toFixed(floatVal));
}

export function handleNewLine(str: string = '') {
    return str.replace(/(\\r)*\\n/g, '<br>')
}

export function getElementAtPath<T>(arr: NestedArray<T>, indexPath: IndexPath): T | undefined {
    let item: NestedArray<T> = arr

    for (let index of indexPath) {
        if (Array.isArray(item)) {
            item = item[index]
        } else {
            return undefined
        }
    }

    return item as T
}

export function removeElementAtPath<T>(arr: NestedArray<T>, indexPath: IndexPath): ElementAndNestedArray<T> | undefined {
    let pathCopy = [...indexPath]
    let targetIndex = pathCopy.pop()

    let targetArray: NestedArray<T> = arr
    for (let index of pathCopy) {
        if (Array.isArray(targetArray)) {
            targetArray = targetArray[index]
        } else {
            return undefined
        }
    }

    if (Array.isArray(targetArray) && typeof targetIndex === "number") {
        let removedElement = targetArray.splice(targetIndex, 1)
        return { element: removedElement[0] as T, array: arr }
    } else {
        return undefined
    }
}

export function addElementAtPath<T>(arr: NestedArray<T>, indexPath: IndexPath, element: T): void {
    let pathCopy = [...indexPath]
    let targetIndex = pathCopy.pop()

    let targetArray: NestedArray<T> = arr
    for (let index of pathCopy) {
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
    let removed = removeElementAtPath(arr, fromIndexPath)
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