import { defineStore } from 'pinia'
export { messages } from './messages'

export interface AlertMsg {
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
      msg: null as AlertMsg | null,
      showCancelButton: false,
      label: 'Ok',
      title: null as string | null
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
      return { cancelled: cancel }
    },
  },
  getters: {
    text(): string {
      return !!this.msg ? this.msg.text : ''
    },
  }
})