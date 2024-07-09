import { defineStore } from "pinia"
import { JSONContent } from "@tiptap/vue-3"

interface OutlineElement {
    id: string
    text: string
    level: number
    index: number
}

export const useOutlineStore = defineStore('outlineStore', () => {

    const headings = ref<OutlineElement[]>([])
    const nodeIndex = ref<number | null>(null)
    const editorIsFocused = ref<boolean>(false)

    function extractHeadings(contentArray: JSONContent[]) {
        contentArray.forEach((item, index) => {
            if (item.type === 'heading' && item.content && item.attrs) {
                const outlineElement: OutlineElement = {
                    id: item.attrs.id ? item.attrs.id : '',
                    text: item.content[0].text!,
                    level: item.attrs.level!,
                    index: index
                }
                headings.value.push(outlineElement)
            }
        })
    }

    function setHeadings(contentArray: JSONContent[]) {
        headings.value = []
        extractHeadings(contentArray)
    }

    function updateHeadings() {

    }

    return { headings, nodeIndex, editorIsFocused, setHeadings, updateHeadings }
})