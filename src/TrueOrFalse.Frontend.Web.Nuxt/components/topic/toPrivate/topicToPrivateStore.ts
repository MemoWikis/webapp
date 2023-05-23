import { defineStore } from "pinia"
import { useAlertStore, AlertType, messages } from "~~/components/alert/alertStore"
import { Visibility } from "~~/components/shared/visibilityEnum"
import { useTopicStore } from "../topicStore"
interface TopicToPrivateData {
    success: boolean
    name?: string
    personalQuestionCount?: number
    personalQuestionIds?: number[]
    allQuestionCount?: number
    allQuestionIds?: number[]
    key?: string
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
            this.confirmedLicense = false
            this.showModal = false
            this.questionsToPrivate = false
            this.allQuestionsToPrivate = false
            const result = await $fetch<TopicToPrivateData>(`/apiVue/TopicToPrivateStore/Get?topicId=${id}`, {
                mode: 'cors',
                credentials: 'include'
            })
            if (result.success) {
                this.name = result.name!
                this.personalQuestionCount = result.personalQuestionCount!
                this.personalQuestionIds = result.personalQuestionIds!
                this.allQuestionCount = result.allQuestionCount!
                this.allQuestionIds = result.allQuestionIds!
                this.id = id
                this.showModal = true
            } else {
                const alertStore = useAlertStore()
                alertStore.openAlert(AlertType.Error, { text: messages.error.category[result.key!] })
            }
        },
        async setToPrivate() {
            const alertStore = useAlertStore()
            const data = {
                topicId: this.id
            }
            interface SetTopicPrivateResult {
                success: boolean
                key: string
            }
            const result = await $fetch<SetTopicPrivateResult>('/apiVue/TopicToPrivateStore/Set', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
            if (result.success) {
                this.showModal = false

                if (this.questionsToPrivate || this.allQuestionsToPrivate)
                    this.setQuestionsToPrivate()

                alertStore.openAlert(AlertType.Success, { text: messages.success.category.setToPrivate })
                const topicStore = useTopicStore()
                if (topicStore.id == this.id)
                    topicStore.visibility = Visibility.Owner

            } else {
                this.showModal = false
                alertStore.openAlert(AlertType.Error, { text: messages.error.category[result.key] })
            }
        },
        setQuestionsToPrivate() {
            const data = {
                questionIds: this.allQuestionsToPrivate ? this.allQuestionIds : this.personalQuestionIds,
            }
            $fetch<any>('/apiVue/TopicToPrivateStore/SetQuestionsToPrivate', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
        }
    }
})