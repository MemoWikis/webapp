import { defineStore } from 'pinia'
import { useUserStore } from '../../user/userStore'
import { useTopicStore } from '~~/components/topic/topicStore'

enum Type {
	Create,
	Edit
}

export const useEditQuestionStore = defineStore('editQuestionStore', {
	state: () => {
		return {
			showModal: false,
			id: 0,
			type: null as Type | null,
			edit: false,
			sessionIndex: 0,
			questionHtml: '',
			flashCardAnswerHtml: '',
			topicId: 0,
			uploadedImagesInContent: [] as string[],
			uploadedImagesMarkedForDeletion: [] as string[]
		}
	},
	actions: {
		createQuestion(q: {
			topicId: number
			questionHtml: string,
			flashCardAnswerHtml: string,
		}) {
			this.topicId = q.topicId
			this.questionHtml = q.questionHtml
			this.flashCardAnswerHtml = q.flashCardAnswerHtml

			this.edit = false
			this.openModal()
		},
		openModal() {
			this.showModal = true
		},
		editQuestion(id: number, sessionIndex: number | null = null) {
			this.id = id
			this.edit = true
			if (sessionIndex != null)
				this.sessionIndex = sessionIndex
			this.openModal()
		},
		create() {
			const userStore = useUserStore()
			if (userStore.isLoggedIn) {
				this.edit = false
				const topicStore = useTopicStore()
				this.topicId = topicStore.id
				this.openModal()
			} else {
				userStore.openLoginModal()
			}
		},
		questionEdited(id: number) {
			return id;
		},
		async uploadContentImage(file: File): Promise<string> {
			const data = new FormData()
			data.append('file', file)
			data.append('topicId', this.id.toString())
			const result = await $api<string>('/apiVue/EditQuestionStore/UploadContentImage', {
				body: data,
				method: 'POST',
				mode: 'cors',
				credentials: 'include',
			})
			return result
		},
		addImageUrlToDeleteList(url: string) {
			if (!this.uploadedImagesMarkedForDeletion.includes(url))
				this.uploadedImagesMarkedForDeletion.push(url)
		},
		refreshDeleteImageList() {
			const imagesToKeep = this.uploadedImagesInContent
			this.uploadedImagesMarkedForDeletion = this.uploadedImagesMarkedForDeletion.filter(url => imagesToKeep.includes(url))
		},
		async deleteTopicContentImages() {
			if (this.uploadedImagesMarkedForDeletion.length == 0)
				return

			const data = {
				topicId: this.id,
				imageUrls: this.uploadedImagesMarkedForDeletion
			}
			await $api<void>('/apiVue/EditQuestionStore/DeleteContentImages', {
				body: data,
				method: 'POST',
				mode: 'cors',
				credentials: 'include',
			})
		}
	},
})