import { defineStore } from 'pinia'

export const useDeleteQuestionStore = defineStore('deleteQuestionStore', {
	state: () => {
		return {
			showModal: false,
			id: 0,
			deletedQuestionId: 0,
		}
	},
	actions: {
		openModal(id: number) {
			this.showModal = true
			this.id = id
		},
		questionDeleted(id: number) {
			this.deletedQuestionId = id
			return id
		}
	},
})