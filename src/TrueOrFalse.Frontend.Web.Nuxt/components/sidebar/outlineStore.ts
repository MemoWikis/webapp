import { defineStore } from "pinia"
import { JSONContent } from "@tiptap/vue-3"

interface OutlineElement {
    id: string
    text: string[]
    level: number
    index: number
}

interface HeadingContent {
    type: string
    text?: string
}

export const useOutlineStore = defineStore('outlineStore', () => {

    const headings = ref<OutlineElement[]>([])
    const nodeIndex = ref<number | null>(null)
    const editorIsFocused = ref<boolean>(false)
    const titleIsFocused = ref<boolean>(false)

    function extractHeadings(contentArray: JSONContent[]) {
        contentArray.forEach((item, index) => {
            if (item.type === 'heading' && item.content && item.attrs) {
                const outlineElement: OutlineElement = {
                    id: item.attrs.id ? item.attrs.id : '',
                    text: getHeadingText(item.content as HeadingContent[]),
                    level: item.attrs.level!,
                    index: index
                }
                headings.value.push(outlineElement)
            }
        })
    }

    function getHeadingText(content: HeadingContent[]) {
        const textArray: string[] = []
        content.forEach(item => {
            if (item.text) {
                textArray.push(item.text)
            }
        })
        return textArray
    }

    function setHeadings(contentArray: JSONContent[]) {
        headings.value = []
        extractHeadings(contentArray)
    }

    return { headings, nodeIndex, editorIsFocused, titleIsFocused, setHeadings }
})