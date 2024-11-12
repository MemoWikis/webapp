import { defineStore } from "pinia"
import { useAlertStore, AlertType, messages } from "~~/components/alert/alertStore"
import { Visibility } from "~~/components/shared/visibilityEnum"
import { usePageStore } from "../pageStore"
import { useUserStore } from "~~/components/user/userStore"

interface PublishPageData {
    success: boolean,
    name?: string,
    questionCount?: number,
    questionIds?: number[],
    key?: string
}

export const usePublishPageStore = defineStore('publishPageStore', {
    state: () => {
        return {
            id: 0,
            name: '',
            questionCount: 0,
            questionIds: [] as number[],
            showModal: false,
            includeQuestionsToPublish: false,
            confirmLicense: false
        }
    },
    actions: {
        async openModal(id: number) {
            this.includeQuestionsToPublish = false
            this.confirmLicense = false
            const result = await $api<PublishPageData>(`/apiVue/PublishPageStore/Get/${id}`, {
                mode: 'cors',
                credentials: 'include'
            })
            if (result.success) {
                this.name = result.name!
                this.questionCount = result.questionCount!
                this.questionIds = result.questionIds!
                this.showModal = true
                this.id = id
            }
        },
        async publish() {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }

            const alertStore = useAlertStore()
            const data = {
                id: this.id
            }
            const result = await $api<FetchResult<number[]>>('/apiVue/PublishPageStore/PublishPage', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
            if (result.success) {
                this.showModal = false

                if (this.includeQuestionsToPublish)
                    this.publishQuestions()

                alertStore.openAlert(AlertType.Success, { text: messages.success.category.publish })

                const pageStore = usePageStore()
                if (pageStore.id == this.id)
                    pageStore.visibility = Visibility.All

                return {
                    success: true,
                    id: this.id
                }
            } else {
                this.showModal = false
                alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey(result.messageKey) })
                return {
                    success: false,
                    id: this.id
                }
            }
        },
        publishQuestions() {
            const data = {
                questionIds: this.questionIds,
            }
            $api('/apiVue/PublishPageStore/PublishQuestions', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
        }

    },
})