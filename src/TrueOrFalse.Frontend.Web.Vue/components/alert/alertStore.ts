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
          msg: null as AlertMsg
        }
      },
      actions: {
        openAlert(type: AlertType, msg: AlertMsg) {
            this.show = true,
            this.type = type,
            this.msg = msg
        },
        closeAlert() {
            this.show = false,
            this.type = AlertType.Default,
            this.msg = null
        }
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