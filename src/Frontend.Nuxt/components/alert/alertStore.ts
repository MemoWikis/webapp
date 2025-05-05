import { defineStore } from "pinia"

export interface AlertMsg {
    text: string | null
    customHtml?: string
    customBtn?: string
    customBtnKey?: string
    customImg?: string
    customDetails?: string
    texts?: string[]
}

export enum AlertType {
    Default,
    Success,
    Error,
}

export const useAlertStore = defineStore("alertStore", {
    state: () => {
        return {
            show: false,
            type: AlertType.Default,
            msg: null as AlertMsg | null,
            showCancelButton: false,
            label: "Ok",
            cancelLabel: "Abbrechen",
            title: null as string | null,
            id: null as string | null,
        }
    },
    actions: {
        openAlert(
            type: AlertType,
            msg: AlertMsg,
            label: string = "Ok",
            showCancelButton: boolean = false,
            title: string | null = null,
            id: string | null = null,
            cancelLabel: string = "Abbrechen"
        ) {
            this.show = true
            this.type = type
            this.msg = msg
            this.showCancelButton = showCancelButton
            this.label = label
            this.title = title
            this.id = id
            this.cancelLabel = cancelLabel
        },
        closeAlert(cancel = false, customKey?: string) {
            this.show = false
            this.type = AlertType.Default
            this.msg = null
            const id = this.id
            this.id = null
            return { cancelled: cancel, customKey: customKey, id: id }
        },
    },
    getters: {
        text(): string | null {
            const text = this.msg?.text ?? null
            if (
                (text == null || (!text && this.type == AlertType.Error)) &&
                this.msg?.customHtml == null
            ) {
                const nuxtApp = useNuxtApp()
                const { $i18n } = nuxtApp
                return $i18n.t("error.default")
            }
            return text
        },
        texts(): string[] {
            return this.msg?.texts ?? []
        },
    },
})
