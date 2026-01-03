import { defineStore } from 'pinia'
import { AlertType, useAlertStore } from '../../alert/alertStore'
import type { PageItem } from '~/components/search/searchHelper'
import type { SnackbarData } from '~/components/snackBar/snackBarStore'
import { useSnackbarStore } from '~/components/snackBar/snackBarStore'
import { IndexeddbPersistence } from 'y-indexeddb'
import * as Y from 'yjs'
import { useUserStore } from '~/components/user/userStore'

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
            showErrorMsg: false,
            isWiki: false,
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
            this.isWiki = false

            if (await this.initDeleteData()) this.showModal = true
        },
        async initDeleteData() {
            interface DeleteDataResult {
                name: string
                canBeDeleted: boolean
                wouldHaveOrphanedChildren: boolean
                suggestedNewParent: PageItem | null
                hasQuestion: boolean
                hasPublicQuestion: boolean
                isWiki: boolean
            }
            const result = await $api<DeleteDataResult>(
                `/apiVue/DeletePageStore/GetDeleteData/${this.id}`,
                { method: 'GET', mode: 'cors', credentials: 'include' }
            )
            const nuxtApp = useNuxtApp()
            const { $i18n } = nuxtApp

            if (result != null) {
                this.suggestedNewParent = result.suggestedNewParent
                this.name = result.name
                this.hasQuestion = result.hasQuestion
                this.hasPublicQuestion = result.hasPublicQuestion
                this.isWiki = result.isWiki
                if (result.wouldHaveOrphanedChildren) {
                    const alertStore = useAlertStore()
                    alertStore.openAlert(
                        AlertType.Error,
                        { text: $i18n.t('error.page.notLastChild') },
                        $i18n.t('deletePageStore.confirm'),
                        undefined,
                        $i18n.t('deletePageStore.cantDeleteInfo', {
                            name: this.name,
                        })
                    )
                    return false
                }
                return true
            }
            const snackbarStore = useSnackbarStore()
            const data: SnackbarData = {
                type: 'error',
                text: { message: $i18n.t('error.default') },
            }
            snackbarStore.showSnackbar(data)
        },
        async deletePage() {
            interface DeleteResult {
                success: boolean
                hasChildren: boolean
                redirectParent?: {
                    name: string
                    id: number
                }
                messageKey: string
            }
            const result = await $api<DeleteResult>(
                `/apiVue/DeletePageStore/Delete`,
                {
                    method: 'POST',
                    mode: 'cors',
                    credentials: 'include',
                    body: {
                        pageToDeleteId: this.id,
                        parentForQuestionsId: this.suggestedNewParent?.id
                            ? this.suggestedNewParent.id
                            : null,
                    },
                }
            )
            if (!!result && result.success) {
                const { $urlHelper } = useNuxtApp()
                if (result.redirectParent) {
                    this.redirectURL = $urlHelper.getPageUrl(
                        result.redirectParent.name,
                        result.redirectParent.id
                    )
                }
                this.pageDeleted = true

                // Delete IndexedDB entry for this page
                // Important for development: prevents issues when page IDs are reused after database resets
                // Also good practice in production: cleans up browser storage for deleted pages
                await this.clearIndexedDB(this.id)

                return {
                    id: this.id,
                }
            } else if (!!result && result.success == false) {
                const nuxtApp = useNuxtApp()
                const { $i18n } = nuxtApp

                this.messageKey = $i18n.t(result.messageKey)
                this.showErrorMsg = true
            }
        },
        async clearIndexedDB(pageId: number) {
            try {
                const userStore = useUserStore()
                const doc = new Y.Doc()
                const persistence = new IndexeddbPersistence(
                    `${userStore.id}|document-${pageId}`,
                    doc
                )

                await persistence.clearData()
                persistence.destroy()
            } catch (error) {
                console.error(
                    'Error clearing IndexedDB for page:',
                    pageId,
                    error
                )
            }
        },
    },
})
