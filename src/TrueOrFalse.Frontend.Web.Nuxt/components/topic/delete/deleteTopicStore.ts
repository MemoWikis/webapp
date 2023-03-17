import { defineStore } from "pinia"
import { AlertType, useAlertStore, messages } from '../../alert/alertStore'

export const useDeleteTopicStore = defineStore('deleteTopicStore', {
    state() {
        return {
            id: 0,
            name: '',
            showModal: false,
            errorMsg: '',
            topicDeleted: false,
            redirectURL: ''
        }
    },
    actions: {
        async openModal(id: number) {
            this.topicDeleted = false
            this.name = ''
            this.errorMsg = ''
            this.id = id
            this.redirectURL = ''
            if (await this.initDeleteData())
                this.showModal = true
        },
        async initDeleteData() {
            interface DeleteDataResult {
                name: string
                canBeDeleted: boolean
                hasChildren: boolean
            }
            var result = await $fetch<DeleteDataResult>(`/apiVue/DeleteTopicStore/GetDeleteData/${this.id}`, { method: 'GET', mode: 'cors', credentials: 'include' })

            if (result != null) {
                this.name = result.name
                if (result.hasChildren) {
                    const alertStore = useAlertStore()
                    alertStore.openAlert(AlertType.Error, { text: messages.error.category.notLastChild }, 'Verstanden', undefined, `Das Thema '${this.name}' kann nicht gelößcht werden`)
                    return false
                }
                return true
            }
        },
        async deleteTopic() {
            interface DeleteResult {
                success: boolean
                hasChildren: boolean
                isNotCreatorOrAdmin: boolean
                redirectURL: string
            }
            var result = await $fetch<DeleteResult>(`/apiVue/DeleteTopicStore/Delete/${this.id}`, { method: 'POST', mode: 'cors', credentials: 'include' })
            if (!!result && result.success) {
                this.redirectURL = result.redirectURL
                this.topicDeleted = true
            }
        }
    }
})