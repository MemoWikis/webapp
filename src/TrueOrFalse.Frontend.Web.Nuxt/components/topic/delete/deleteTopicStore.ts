import { defineStore } from "pinia"
import { AlertType, useAlertStore, messages } from '../../alert/alertStore'

import { Parent } from '~~/components/topic/delete/parent';


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
            parent: <Parent | null>(null),
            selectedParent: 0
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
            
            if (await this.initDeleteData())
                this.showModal = true
        },
        async initDeleteData() {
            interface DeleteDataResult {
                name: string
                canBeDeleted: boolean
                hasChildren: boolean
                parent: Parent
            }
            const result = await $fetch<DeleteDataResult>(`/apiVue/DeleteTopicStore/GetDeleteData/${this.id}`, { method: 'GET', mode: 'cors', credentials: 'include' })
            if (result != null) {
                this.parent = result.parent
                this.name = result.name
                if (result.hasChildren) {
                    const alertStore = useAlertStore()
                    alertStore.openAlert(AlertType.Error, { text: messages.error.category.notLastChild }, 'Verstanden', undefined, `Das Thema '${this.name}' kann nicht gelöscht werden`)
                    return false
                }
                return true
            }
        },
        async deleteTopic(selectedParentId: number) {
            console.log(selectedParentId)
            interface DeleteResult {
                success: boolean
                hasChildren: boolean
                isNotCreatorOrAdmin: boolean
                redirectParent: {
                    name: string
                    id: number
                }
            }
        const result = await $fetch<DeleteResult>(`/apiVue/DeleteTopicStore/Delete`, { 
            method: 'POST', 
            mode: 'cors', 
            credentials: 'include',
            body: JSON.stringify({
            id: this.id,
            parentForQuestionsId: selectedParentId
            })
      });
            if (!!result && result.success) {                 
                const { $urlHelper } = useNuxtApp()
                this.redirectURL = $urlHelper.getTopicUrl(result.redirectParent.name, result.redirectParent.id)
                this.topicDeleted = true
 
                return {
                    id: this.id 
                }
            }
        },
        setSelectedParent(id: number) { 
            this.selectedParent = id;
        }
    }
})