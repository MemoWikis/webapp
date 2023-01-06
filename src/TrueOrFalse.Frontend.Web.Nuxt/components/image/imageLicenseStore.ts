import { defineStore } from "pinia"

export const useImageLicenseStore = defineStore('imageLicenseStore', {
    state: () => {
        return {
            show: false,
        }
    },
    actions: {
        openAlert(type: AlertType, msg: AlertMsg, label: string = 'Ok', showCancelButton: boolean = false, title: string | null = null) {
            this.show = true
            this.type = type
            this.msg = msg
            this.showCancelButton = showCancelButton
            this.label = label
            this.title = title
        },
        closeAlert(cancel = false) {
            this.show = false
            this.type = AlertType.Default
            this.msg = null
            this.cancelled = cancel
        },
    }
})