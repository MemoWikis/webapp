import { defineStore } from "pinia"
import { useAlertStore, AlertType, messages } from "~~/components/alert/alertStore"
import { Visibility } from "~~/components/shared/visibilityEnum"
import { useTopicStore } from "../topicStore"
import { useUserStore } from "~~/components/user/userStore"

interface TopicToPrivateData {
    name: string
    personalQuestionCount: number
    personalQuestionIds: number[]
    allQuestionCount: number
    allQuestionIds: number[]
    key: string
}

export const useTopicToPrivateStore = defineStore('topicToPrivateStore', {
    state: () => {
        return {
            id: 0,
            name: '',
            personalQuestionCount: 0,
            personalQuestionIds: [] as number[],
            allQuestionCount: 0,
            allQuestionIds: [] as number[],
            confirmedLicense: false,
            showModal: false,
            questionsToPrivate: false,
            allQuestionsToPrivate: false,
        }
    },
    actions: {
        async openModal(id: number) {
            const userStore = useUserStore()

            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }

            this.confirmedLicense = false
            this.showModal = false
            this.questionsToPrivate = false
            this.allQuestionsToPrivate = false
            const result = await $api<FetchResult<TopicToPrivateData>>(`/apiVue/TopicToPrivateStore/Get/${id}`, {
                mode: 'cors',
                credentials: 'include'
            })
            if (result.success) {
                this.name = result.data.name
                this.personalQuestionCount = result.data.personalQuestionCount
                this.personalQuestionIds = result.data.personalQuestionIds
                this.allQuestionCount = result.data.allQuestionCount
                this.allQuestionIds = result.data.allQuestionIds
                this.id = id
                this.showModal = true
            } else {
                const alertStore = useAlertStore()
                alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey(result.messageKey) })
            }
        },
        async setToPrivate() {
            const alertStore = useAlertStore()
            const result = await $api<FetchResult<null>>(`/apiVue/TopicToPrivateStore/Set/${this.id}`, { method: 'POST', mode: 'cors', credentials: 'include' })
            if (result.success) {
                this.showModal = false

                if (this.questionsToPrivate || this.allQuestionsToPrivate)
                    this.setQuestionsToPrivate()

                alertStore.openAlert(AlertType.Success, { text: messages.success.category.setToPrivate })
                const topicStore = useTopicStore()
                if (topicStore.id == this.id)
                    topicStore.visibility = Visibility.Owner

                return {
                    success: true,
                    id: this.id
                }

            } else {
                this.showModal = false
                alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey(result.messageKey) })

                return {
                    success: false
                }
            }
        },
        setQuestionsToPrivate() {
            const data = {
                questionIds: this.allQuestionsToPrivate ? this.allQuestionIds : this.personalQuestionIds,
            }
            $api<any>('/apiVue/TopicToPrivateStore/SetQuestionsToPrivate', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
        }
    }
})