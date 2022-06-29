import { defineStore } from 'pinia'
import { AlertType, AlertMsg } from './alertCollection'

export const useAlertStore = defineStore('alertStore', {
    state: () => {
        return {
          isOpen: false,
          type: AlertType.Default,
          msg: null as AlertMsg
        }
      },
      actions: {
        openAlert(type: AlertType, msg: AlertMsg) {
            this.isOpen = true,
            this.type = type,
            this.msg = msg
        },
        closeAlert() {
            this.isOpen = false,
            this.type = AlertType.Default,
            this.msg = null
        }
      },
})