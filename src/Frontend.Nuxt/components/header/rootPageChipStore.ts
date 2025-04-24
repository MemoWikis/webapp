import { defineStore } from 'pinia'
import { Page } from '../page/pageStore'
export const useRootPageChipStore = defineStore('rootPageChipStore', {
    state: () => {
        return {
            showRootPageChip: false,
            name: 'Globales Wiki',
            id: 1,
            imgUrl: '/Images/Categories/1_50s.jpg'
        }
    },
    actions: {
        setRootPageData(page: Page) {
            this.name = page.name
            this.id = page.id
            if (page.imageUrl.length > 0)
                this.imgUrl = page.imageUrl
        }
    }
})