import { defineStore } from "pinia"
import { useAlertStore, AlertType } from "~~/components/alert/alertStore"
import { Visibility } from "~~/components/shared/visibilityEnum"
import { usePageStore } from "../pageStore"
import { useUserStore } from "~~/components/user/userStore"

interface PageToPrivateData {
    name: string
    personalQuestionCount: number
    personalQuestionIds: number[]
    allQuestionCount: number
    allQuestionIds: number[]
    key: string
}

export const usePageToPrivateStore = defineStore("pageToPrivateStore", {
    state: () => {
        return {
            id: 0,
            name: "",
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
            const result = await $api<FetchResult<PageToPrivateData>>(
                `/apiVue/PageToPrivateStore/Get/${id}`,
                {
                    mode: "cors",
                    credentials: "include",
                }
            )
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
                const nuxtApp = useNuxtApp()
                const { $i18n } = nuxtApp

                alertStore.openAlert(AlertType.Error, {
                    text: $i18n.t(result.messageKey),
                })
            }
        },
        async setToPrivate() {
            const alertStore = useAlertStore()
            const result = await $api<FetchResult<null>>(
                `/apiVue/PageToPrivateStore/Set/${this.id}`,
                { method: "POST", mode: "cors", credentials: "include" }
            )
            const nuxtApp = useNuxtApp()
            const { $i18n } = nuxtApp

            if (result.success) {
                this.showModal = false

                if (this.questionsToPrivate || this.allQuestionsToPrivate)
                    this.setQuestionsToPrivate()

                alertStore.openAlert(AlertType.Success, {
                    text: $i18n.t("success.page.setToPrivate"),
                })
                const pageStore = usePageStore()
                if (pageStore.id == this.id)
                    pageStore.visibility = Visibility.Private

                return {
                    success: true,
                    id: this.id,
                }
            } else {
                this.showModal = false
                alertStore.openAlert(AlertType.Error, {
                    text: $i18n.t(result.messageKey),
                })

                return {
                    success: false,
                }
            }
        },
        setQuestionsToPrivate() {
            const data = {
                questionIds: this.allQuestionsToPrivate
                    ? this.allQuestionIds
                    : this.personalQuestionIds,
            }
            $api<any>("/apiVue/PageToPrivateStore/SetQuestionsToPrivate", {
                method: "POST",
                body: data,
                mode: "cors",
                credentials: "include",
            })
        },
    },
})
