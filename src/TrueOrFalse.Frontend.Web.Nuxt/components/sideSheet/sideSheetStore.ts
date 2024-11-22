import { defineStore } from "pinia"

export interface SideSheetPage {
    id: number
    name: string
}

export const useSideSheetStore = defineStore('sideSheetStore', () => {

    const showSideSheet = ref(false)

    const addToList = (list: SideSheetPage[], name: string, id: number) => {
        if (list) {
            list.push({
                name: name,
                id: id
            })
        } else {
            list = [{
                name: name,
                id: id
            }]
        }
    }


    const wikis = ref<SideSheetPage[]>([])

    const addToFavoriteWikis = (name: string, id: number) => addToList(wikis.value, name, id)

    const favorites = ref<SideSheetPage[]>([])

    const addToFavoritePages = (name: string, id: number) => addToList(favorites.value, name, id)

    const recentPages = ref<SideSheetPage[]>([])

    const handleRecentPage = (name: string, id: number) => {
    const sideSheetPage = {
        id: id,
        name: name,
    } as SideSheetPage

    if (recentPages.value) {
        recentPages.value = recentPages.value.filter((page) => page.id !== sideSheetPage.id)

        if (recentPages.value.length > 5) {
            recentPages.value.pop()
        }

        recentPages.value.unshift(sideSheetPage)
    } else {
        recentPages.value = [sideSheetPage]
    }
}


    return { wikis, favorites, recentPages, addToFavoriteWikis, addToFavoritePages, handleRecentPage, showSideSheet }
})