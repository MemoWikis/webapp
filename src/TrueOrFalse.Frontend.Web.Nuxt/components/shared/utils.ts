import { lowlight } from 'lowlight/lib/core' 
import { toHtml } from 'hast-util-to-html'

export function getHighlightedCode(oldHtml: string) {
    var lowlightNode = lowlight.highlightAuto(oldHtml)
    var newHtml = toHtml(lowlightNode)
    return newHtml
}