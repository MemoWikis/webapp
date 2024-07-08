import { defineStore } from "pinia"
import { JSONContent } from "@tiptap/vue-3"

interface OutlineElement {
    id: string
    text: string
    level: number
}

export const useOutlineStore = defineStore('outlineStore', () => {

    const headings = ref<OutlineElement[]>([])

    function extractHeadings(contentArray: JSONContent[]) {
        let previousLevel: number | null = null
        contentArray.forEach((item) => {
            if (item.type === 'heading') {
                const outlineElement: OutlineElement = {
                    id: item.attrs!.id!,
                    text: item.content![0].text!,
                    level: item.attrs!.level!,
                }
                headings.value.push(outlineElement)
                previousLevel = item.attrs!.level!
            }
        })
    }

    function updateHeadings(contentArray: JSONContent[]) {
        headings.value = []
        extractHeadings(contentArray)
    }

    return { headings, updateHeadings }
})