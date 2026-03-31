import { defineStore } from 'pinia'

export interface SideSheetWiki {
    id: number
    name: string
    hasParents: boolean
    imgUrl: string
}

export interface SideSheetPage {
    id: number
    name: string
    imgUrl: string
}

export const useSideSheetStore = defineStore('sideSheetStore', () => {
    const showSideSheet = ref(false)

    const wikis = ref<SideSheetWiki[]>([])
    const addToFavoriteWikis = (name: string, id: number, imgUrl: string = '') => {
        if (wikis.value) {
            wikis.value.push({
                name: name,
                id: id,
                hasParents: false,
                imgUrl: imgUrl,
            })
        } else {
            wikis.value = [
                {
                    name: name,
                    id: id,
                    hasParents: false,
                    imgUrl: imgUrl,
                },
            ]
        }
    }

    const favorites = ref<SideSheetPage[]>([])
    const addToFavoritePages = (name: string, id: number, imgUrl: string = '') => {
        if (favorites.value) {
            favorites.value.push({
                name: name,
                id: id,
                imgUrl: imgUrl,
            })
        } else {
            favorites.value = [
                {
                    name: name,
                    id: id,
                    imgUrl: imgUrl,
                },
            ]
        }
    }
    const removeFromFavoritePages = (id: number) => {
        if (favorites.value)
            favorites.value = favorites.value.filter((page) => page.id !== id)
    }

    const recentPages = ref<SideSheetPage[]>([])
    const recentPagesCount = ref(15)
    const recentPagesTotalAvailable = ref(0)

    const handleRecentPage = (name: string, id: number, imgUrl: string = '') => {
        const sideSheetPage = {
            id: id,
            name: name,
            imgUrl: imgUrl,
        } as SideSheetPage

        if (recentPages.value) {
            recentPages.value = recentPages.value.filter(
                (page) => page.id !== sideSheetPage.id
            )

            recentPages.value.unshift(sideSheetPage)
        } else {
            recentPages.value = [sideSheetPage]
        }
    }

    const sharedPages = ref<SideSheetPage[]>([])

    return {
        wikis,
        favorites,
        recentPages,
        recentPagesCount,
        recentPagesTotalAvailable,
        sharedPages,
        addToFavoriteWikis,
        addToFavoritePages,
        removeFromFavoritePages,
        handleRecentPage,
        showSideSheet,
    }
})
