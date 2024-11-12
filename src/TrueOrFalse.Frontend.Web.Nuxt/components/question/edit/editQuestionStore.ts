import { defineStore } from 'pinia'
import { useUserStore } from '../../user/userStore'
import { usePageStore } from '~/components/page/pageStore'
import { nanoid } from 'nanoid'

enum Type {
	Create,
	Edit
}

export const useEditQuestionStore = defineStore('editQuestionStore', {
	state: () => {
		return {
			showModal: false,
			id: -1,
			type: null as Type | null,
			edit: false,
			sessionIndex: 0,
			questionHtml: '',
			flashCardAnswerHtml: '',
			pageId: 0,
			uploadedImagesInContent: [] as string[],
			uploadedImagesMarkedForDeletion: [] as string[],
			uploadTrackingArray: [] as string[]
		}
	},
	actions: {
		createQuestion(q: {
			pageId: number
			questionHtml: string,
			flashCardAnswerHtml: string,
		}) {
			this.pageId = q.pageId
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
				const pageStore = usePageStore()
				this.pageId = pageStore.id
				this.openModal()
			} else {
				userStore.openLoginModal()
			}
		},
		questionEdited(id: number) {
			return id;
		},
		async uploadContentImage(file: File): Promise<string> {
			const uploadId = nanoid(5)
			this.uploadTrackingArray.push(uploadId)
			
			const data = new FormData()
			data.append('file', file)
			data.append('questionId', this.id.toString())

			const result = await $api<string>('/apiVue/EditQuestionStore/UploadContentImage', {
				body: data,
				method: 'POST',
				mode: 'cors',
				credentials: 'include',
			})

			this.uploadTrackingArray = this.uploadTrackingArray.filter(id => id !== uploadId)

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
		async deletePageContentImages() {
			if (this.uploadedImagesMarkedForDeletion.length == 0)
				return

			const data = {
				pageId: this.id,
				imageUrls: this.uploadedImagesMarkedForDeletion
			}
			await $api<void>('/apiVue/EditQuestionStore/DeleteContentImages', {
				body: data,
				method: 'POST',
				mode: 'cors',
				credentials: 'include',
			})
			this.uploadedImagesMarkedForDeletion = []
		},
		async waitUntilAllUploadsComplete() {
			while (this.uploadTrackingArray.length > 0) {
				await new Promise(resolve => setTimeout(resolve, 100))
			}
		}
	},
})