import { defineStore } from "pinia"

export interface SideSheetWiki {
    id: number
    name: string
    hasParents: boolean
}

export interface SideSheetPage {
    id: number
    name: string
}

export const useSideSheetStore = defineStore('sideSheetStore', () => {

    const showSideSheet = ref(false)

    const wikis = ref<SideSheetWiki[]>([])
    const addToFavoriteWikis = (name: string, id: number) => {
        if (wikis.value) {
            wikis.value.push({
                name: name,
                id: id,
                hasParents: false
            })
        } else {
            wikis.value = [{
                name: name,
                id: id,
                hasParents: false}]
            }
    }

    const favorites = ref<SideSheetPage[]>([])
    const addToFavoritePages = (name: string, id: number) => {
        if (favorites.value) {
            favorites.value.push({
                name: name,
                id: id
            })
        } else {
            favorites.value = [{
                name: name,
                id: id}]
            }
    }

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