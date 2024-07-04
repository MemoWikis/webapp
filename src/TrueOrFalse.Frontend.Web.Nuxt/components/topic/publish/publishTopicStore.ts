import { defineStore } from "pinia"
import { useAlertStore, AlertType, messages } from "~~/components/alert/alertStore"
import { Visibility } from "~~/components/shared/visibilityEnum"
import { useTopicStore } from "../topicStore"
import { useUserStore } from "~~/components/user/userStore"

interface PublishTopicData {
    success: boolean,
    name?: string,
    questionCount?: number,
    questionIds?: number[],
    key?: string
}

export const usePublishTopicStore = defineStore('publishTopicStore', {
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
            const result = await $api<PublishTopicData>(`/apiVue/PublishTopicStore/Get/${id}`, {
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
            const result = await $api<FetchResult<number[]>>('/apiVue/PublishTopicStore/PublishTopic', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
            if (result.success) {
                this.showModal = false

                if (this.includeQuestionsToPublish)
                    this.publishQuestions()

                alertStore.openAlert(AlertType.Success, { text: messages.success.category.publish })

                const topicStore = useTopicStore()
                if (topicStore.id == this.id)
                    topicStore.visibility = Visibility.All

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
            $api('/apiVue/PublishTopicStore/PublishQuestions', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
        }

    },
})