import { defineStore } from "pinia"

export const useDeleteTopicStore = defineStore('deleteTopicStore', {
    state() {
        return {
            id: 0,
            name: ''
        }
    },
    actions: {
        openModal(id: number) {

        }
    }
})