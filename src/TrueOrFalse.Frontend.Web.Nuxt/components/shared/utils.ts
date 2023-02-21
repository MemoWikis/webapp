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