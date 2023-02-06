import { defineStore } from "pinia"
import { useAlertStore, AlertType, AlertMsg, messages } from "~~/components/alert/alertStore"
interface TopicToPrivateData {
    success: boolean
    name?: string
    personalQuestionCount?: number
    personalQuestionIds?: number[]
    allQuestionCount?: number
    allQuestionIds?: number[]
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
            if (this.id != id) {
                const result = await $fetch<TopicToPrivateData>(`/apiVue/TopicToPrivateStore/Get?id=${id}`, {
                    mode: 'no-cors',
                    credentials: 'include'
                })
                if (result.success) {
                    this.name = result.name!
                    this.personalQuestionCount = result.personalQuestionCount!
                    this.personalQuestionIds = result.personalQuestionIds!
                    this.allQuestionCount = result.allQuestionCount!
                    this.allQuestionIds = result.allQuestionIds!

                    this.showModal = true
                } else {
                    const alertStore = useAlertStore()
                    alertStore.openAlert(AlertType.Error, { text: messages.error.category[result.key] })
                }
            }
        },
        async setToPrivate() {
            const alertStore = useAlertStore()
            const data = {
                topicId: this.id
            }
            const result = await $fetch<any>('/apiVue/TopicToPrivateStore/Set', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
            if (result.success) {
                this.showModal = false

                if (this.questionsToPrivate || this.allQuestionsToPrivate)
                    this.setQuestionsToPrivate()

                alertStore.openAlert(AlertType.Success, { text: messages.category.setToPrivate })
            } else {
                this.showModal = false
                alertStore.openAlert(AlertType.Error, { text: messages.category[result.key] })
            }
        },
        setQuestionsToPrivate() {
            const data = {
                questionIds: this.allQuestionsToPrivate ? this.allQuestionIds : this.personalQuestionIds,
            }
            $fetch<any>('/apiVue/topicToPrivate/SetQuestionsToPrivate', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
        }
    }
})