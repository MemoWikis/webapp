import { defineStore } from "pinia"

interface OutlineElement {
    id: string
    text: string
    level: number
}


export const useOutlineStore = defineStore('outlineStore', () => {

    const allHeadingsIds = ref<string[]>([])

    function addHeadingId(headingId: string) {
        allHeadingsIds.value.push(headingId)
    }

    const headings = ref<OutlineElement[]>([])

    return {allHeadingsIds, addHeadingId, }
})