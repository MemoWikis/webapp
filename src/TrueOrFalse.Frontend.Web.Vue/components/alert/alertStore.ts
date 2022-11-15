import { defineStore } from 'pinia'
export { messages } from './messages'

export type AlertMsg = {
  text: string,
  reload?: boolean,
  customHtml?: string,
  customBtn?: string,
}

export enum AlertType {
  Default,
  Success,
  Error
}

export const useAlertStore = defineStore('alertStore', {
    state: () => {
        return {
          show: false,
          type: AlertType.Default,
          msg: null as AlertMsg,
          showCancelButton: false,
          label: 'Ok',
          cancelled: false,
          title: null
        }
      },
      actions: {
        openAlert(type: AlertType, msg: AlertMsg, label: string = 'Ok', showCancelButton: boolean = false, title: string = null) {
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
      },
      getters: {
        text(): string {
          return !!this.msg ? this.msg.text : ''
        },
        html() {
          return !!this.msg && !!this.msg.customHtml ? this.msg.customHtml : null
        },
        btn(){
          return !!this.msg && !!this.msg.customBtn ? this.msg.customBtn : null
        }
      }
})