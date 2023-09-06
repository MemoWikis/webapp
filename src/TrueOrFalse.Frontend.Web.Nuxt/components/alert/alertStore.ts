import { defineStore } from 'pinia'
import { messages } from './messages'
export { messages } from './messages'

export interface AlertMsg {
	text: string | null,
	reload?: boolean,
	customHtml?: string,
	customBtn?: string,
	customBtnKey?: string
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
			title: null as string | null,
			id: null as string | null
		}
	},
	actions: {
		openAlert(type: AlertType, msg: AlertMsg, label: string = 'Ok', showCancelButton: boolean = false, title: string | null = null, id: string | null = null) {
			this.show = true
			this.type = type
			this.msg = msg
			this.showCancelButton = showCancelButton
			this.label = label
			this.title = title
			this.id = id
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
		text(): string {
			const text = this.msg?.text ?? null
			if (text == null || !text && this.type == AlertType.Error) {
				return messages.error.default
			}
			return text;
		},
	}
})