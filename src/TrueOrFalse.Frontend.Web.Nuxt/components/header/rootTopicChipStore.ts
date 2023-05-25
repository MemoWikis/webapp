import { defineStore } from 'pinia'
export const useRootTopicChipStore = defineStore('rootTopicChipStore', {
    state: () => {
        return {
            showRootTopicChip: false,
            encodedName: 'Globales-Wiki',
            name: 'Globales Wiki',
            id: 1,
            imgUrl: '/Images/Categories/1_50s.jpg'
        }
    },
})