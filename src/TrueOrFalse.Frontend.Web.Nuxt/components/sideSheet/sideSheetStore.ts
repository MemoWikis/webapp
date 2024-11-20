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


    const favoriteWikis = ref<SideSheetPage[]>([])

    const addToFavoriteWikis = (name: string, id: number) => addToList(favoriteWikis.value, name, id)

    const favoritePages = ref<SideSheetPage[]>([])

    const addToFavoritePages = (name: string, id: number) => addToList(favoritePages.value, name, id)

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


    return { favoriteWikis, favoritePages, recentPages, addToFavoriteWikis, addToFavoritePages, handleRecentPage, showSideSheet }
})