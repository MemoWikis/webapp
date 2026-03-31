import { defineStore } from 'pinia'

export interface SideSheetWiki {
    id: number
    name: string
    hasParents: boolean
    imgUrl: string
    childrenCount: number
}

export interface SideSheetPage {
    id: number
    name: string
    imgUrl: string
    childrenCount?: number
}

export interface SideSheetChildPage {
    id: number
    name: string
    imgUrl: string
    childrenCount: number
}

export const useSideSheetStore = defineStore('sideSheetStore', () => {
    const showSideSheet = ref(false)

    const wikis = ref<SideSheetWiki[]>([])
    const addToFavoriteWikis = (
        name: string,
        id: number,
        imgUrl: string = '',
    ) => {
        if (wikis.value) {
            wikis.value.push({
                name: name,
                id: id,
                hasParents: false,
                imgUrl: imgUrl,
                childrenCount: 0,
            })
        } else {
            wikis.value = [
                {
                    name: name,
                    id: id,
                    hasParents: false,
                    imgUrl: imgUrl,
                    childrenCount: 0,
                },
            ]
        }
    }

    const favorites = ref<SideSheetPage[]>([])
    const addToFavoritePages = (
        name: string,
        id: number,
        imgUrl: string = '',
    ) => {
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

    const handleRecentPage = (
        name: string,
        id: number,
        imgUrl: string = '',
    ) => {
        const sideSheetPage = {
            id: id,
            name: name,
            imgUrl: imgUrl,
        } as SideSheetPage

        if (recentPages.value) {
            recentPages.value = recentPages.value.filter(
                (page) => page.id !== sideSheetPage.id,
            )

            recentPages.value.unshift(sideSheetPage)
        } else {
            recentPages.value = [sideSheetPage]
        }
    }

    const sharedPages = ref<SideSheetPage[]>([])

    const expandedPages = ref<Map<string, Set<number>>>(new Map())
    const childrenMap = ref<Map<number, SideSheetChildPage[]>>(new Map())

    const isExpanded = (section: string, pageId: number) => {
        return expandedPages.value.get(section)?.has(pageId) ?? false
    }

    const toggleExpanded = (section: string, pageId: number) => {
        const newMap = new Map(expandedPages.value)
        const sectionSet = new Set(newMap.get(section) ?? [])
        if (sectionSet.has(pageId)) {
            sectionSet.delete(pageId)
        } else {
            sectionSet.add(pageId)
        }
        newMap.set(section, sectionSet)
        expandedPages.value = newMap
    }

    const setChildren = (pageId: number, children: SideSheetChildPage[]) => {
        const newMap = new Map(childrenMap.value)
        newMap.set(pageId, children)
        childrenMap.value = newMap
    }

    return {
        wikis,
        favorites,
        recentPages,
        recentPagesCount,
        recentPagesTotalAvailable,
        sharedPages,
        expandedPages,
        childrenMap,
        isExpanded,
        toggleExpanded,
        setChildren,
        addToFavoriteWikis,
        addToFavoritePages,
        removeFromFavoritePages,
        handleRecentPage,
        showSideSheet,
    }
})
