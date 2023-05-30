import { defineStore } from 'pinia'

export const useDeleteQuestionStore = defineStore('deleteQuestionStore', {
	state: () => {
		return {
			showModal: false,
			id: 0,
			deletedQuestionId: 0,
			deletedQuestionIndex: 0 as number | null,
		}
	},
	actions: {
		openModal(id: number) {
			this.showModal = true
			this.id = id
		},
		questionDeleted(id: number, index: number | null = null) {
			this.deletedQuestionId = id
			this.deletedQuestionIndex = index
		}
	},
})