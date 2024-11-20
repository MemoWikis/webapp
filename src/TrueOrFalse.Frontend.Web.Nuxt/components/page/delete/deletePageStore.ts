import { defineStore } from "pinia"
import { AlertType, useAlertStore, messages } from '../../alert/alertStore'
import { PageItem } from "~/components/search/searchHelper"
import { useSnackbarStore, SnackbarData } from '~/components/snackBar/snackBarStore'

export const useDeletePageStore = defineStore('deletePageStore', {
    state() {
        return {
            id: 0,
            name: '',
            showModal: false,
            errorMsg: '',
            pageDeleted: false,
            redirectURL: '',
            redirect: false,
            suggestedNewParent: null as PageItem | null,
            hasQuestion: false,
            hasPublicQuestion: false,
            messageKey: '',
            showErrorMsg: false
        }
    },
    actions: {
        async openModal(id: number, redirect: boolean = false) {
            this.pageDeleted = false
            this.name = ''
            this.errorMsg = ''
            this.id = id
            this.redirectURL = ''
            this.redirect = redirect
            this.suggestedNewParent = null
            this.hasQuestion = false
            this.hasPublicQuestion = false
            
            if (await this.initDeleteData())
                this.showModal = true
        },
        async initDeleteData() {
            interface DeleteDataResult {
                name: string
                canBeDeleted: boolean
                hasChildren: boolean
                suggestedNewParent: PageItem | null
                hasQuestion: boolean
                hasPublicQuestion: boolean
            }
            const result = await $api<DeleteDataResult>(`/apiVue/DeletePageStore/GetDeleteData/${this.id}`, { method: 'GET', mode: 'cors', credentials: 'include' })
            if (result != null) {
                this.suggestedNewParent = result.suggestedNewParent
                this.name = result.name
                this.hasQuestion = result.hasQuestion
                this.hasPublicQuestion = result.hasPublicQuestion
                if (result.hasChildren) {
                    const alertStore = useAlertStore()
                    alertStore.openAlert(AlertType.Error, { text: messages.error.page.notLastChild }, 'Verstanden', undefined, `Die Seite: '${this.name}' kann nicht gelöscht werden`)
                    return false
                }
                return true
            }
            const snackbarStore = useSnackbarStore()
            const data: SnackbarData = {
                type: 'error',
                text: messages.error.default
            }
            snackbarStore.showSnackbar(data)

        },
        async deletePage() {
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
            const result = await $api<DeleteResult>(`/apiVue/DeletePageStore/Delete`, { 
                method: 'POST', 
                mode: 'cors', 
                credentials: 'include',
                body: {
                    pageToDeleteId: this.id,
                    parentForQuestionsId: this.suggestedNewParent?.id ? this.suggestedNewParent.id : null
                }
            })
            if (!!result && result.success) {                 
                const { $urlHelper } = useNuxtApp()
                this.redirectURL = $urlHelper.getPageUrl(result.redirectParent.name, result.redirectParent.id)
                this.pageDeleted = true
        
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