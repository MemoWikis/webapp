import { defineStore } from "pinia"
import { useAlertStore, AlertType, AlertMsg, messages } from "~~/components/alert/alertStore"

interface PublishTopicData {
    success: boolean,
    name: string,
    questionCount: number,
    questionIds: number[],
    key: string
}

export const usePublishTopicStore = defineStore('publishTopicStore', {
    state: () => {
        return {
            id: 0,
            name: '',
            questionCount: 0,
            questionIds: [] as number[],
            showModal: false,
            includeQuestionsToPublish: false
        }
    },
    actions: {
        async openModal(id: number) {
            if (this.id != id) {
                const result = await $fetch<PublishTopicData>(`apiVue/PublishTopic/Get?topicId=${id}`, {
                    mode: 'no-cors',
                    credentials: 'include'
                })
                if (result.success) {
                    this.name = result.name
                    this.questionCount = result.questionCount
                    this.questionIds = result.questionIds
                    this.showModal = true
                }
            }
        },
        async publish() {
            const alertStore = useAlertStore()
            const data = {
                topicId: this.id
            }
            const result = await $fetch<any>('/apiVue/PublishTopic/PublishTopic', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
            if (result.success) {
                this.showModal = false

                if (this.includeQuestionsToPublish)
                    this.publishQuestions()

                alertStore.openAlert(AlertType.Success, { text: messages.category.publish })
            } else {
                this.showModal = false
                alertStore.openAlert(AlertType.Error, { text: messages.category.parentIsPrivate })
            }
        },
        publishQuestions() {
            var data = {
                questionIds: this.questionIds,
            }
            $fetch<any>('/apiVue/PublishTopic/PublishQuestions', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
        }

    },
})