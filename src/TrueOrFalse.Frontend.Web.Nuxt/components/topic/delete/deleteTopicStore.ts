import { defineStore } from "pinia"
import { AlertType, useAlertStore, messages } from '../../alert/alertStore'
import { TopicItem } from "~/components/search/searchHelper"

export const useDeleteTopicStore = defineStore('deleteTopicStore', {
    state() {
        return {
            id: 0,
            name: '',
            showModal: false,
            errorMsg: '',
            topicDeleted: false,
            redirectURL: '',
            redirect: false,
            suggestedNewParent: null as TopicItem | null,
            hasPublicQuestion: false,
            messageKey: '',
            showErrorMsg: false
        }
    },
    actions: {
        async openModal(id: number, redirect: boolean = false) {
            this.topicDeleted = false
            this.name = ''
            this.errorMsg = ''
            this.id = id
            this.redirectURL = ''
            this.redirect = redirect
            this.suggestedNewParent = null
            this.hasPublicQuestion = false
            
            if (await this.initDeleteData())
                this.showModal = true
        },
        async initDeleteData() {
            interface DeleteDataResult {
                name: string
                canBeDeleted: boolean
                hasChildren: boolean
                suggestedNewParent: TopicItem | null
                hasPublicQuestion: boolean
            }
            const result = await $fetch<DeleteDataResult>(`/apiVue/DeleteTopicStore/GetDeleteData/${this.id}`, { method: 'GET', mode: 'cors', credentials: 'include' })
            if (result != null) {
                this.suggestedNewParent = result.suggestedNewParent
                this.name = result.name
                this.hasPublicQuestion = result.hasPublicQuestion
                if (result.hasChildren) {
                    const alertStore = useAlertStore()
                    alertStore.openAlert(AlertType.Error, { text: messages.error.category.notLastChild }, 'Verstanden', undefined, `Das Thema '${this.name}' kann nicht gelöscht werden`)
                    return false
                }
                return true
            }
        },
        async deleteTopic() {
            interface DeleteResult {
                success: boolean,
                hasChildren: boolean
                isNotCreatorOrAdmin: boolean
                redirectParent: { 
                    name: string
                    id: number
                },
                messageKey: string
            }
            const result = await $fetch<DeleteResult>(`/apiVue/DeleteTopicStore/Delete`, { 
                method: 'POST', 
                mode: 'cors', 
                credentials: 'include',
                body: JSON.stringify({
                    id: this.id,
                    parentForQuestionsId: this.suggestedNewParent?.id
                })
            });
            if (!!result && result.success) {                 
                const { $urlHelper } = useNuxtApp()
                this.redirectURL = $urlHelper.getTopicUrl(result.redirectParent.name, result.redirectParent.id)
                this.topicDeleted = true
        
                return {
                    id: this.id 
                }
            }else if (!!result && result.success == false) {
                this.messageKey = messages.getByCompositeKey(result.messageKey)
                this.showErrorMsg = true
            }  
        },
    }
})