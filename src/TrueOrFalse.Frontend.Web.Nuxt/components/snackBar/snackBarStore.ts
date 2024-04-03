import { defineStore } from 'pinia'

export enum SnackbarType {
	Success,
	Error,
	Warning,
	Info
}

export interface SnackbarData{
	type?: string
	title?: string
	text?: string
	snackbarCustomAction?: SnackbarCustomAction
}

export interface SnackbarCustomAction {
	id?: number,
	label: string
	action: () => void
}

export const useSnackbarStore = defineStore('snackbarStore', {
	state: () => {
		return {
			customActions: [] as SnackbarCustomAction[]
		}
	},
	actions: {
		// add(data: SnackbarData) {
		// 	console.log(data)

		// 	if (data.snackbarCustomAction)
		// 		this.addCustomAction(data.snackbarCustomAction)

		// 	const snackbar = useSnackbar()
		// 	if (data.snackbarCustomAction) {
		// 		snackbar.add({
		// 			type: data.type,
		// 			title: data.title,
		// 			text: { message: data.text, buttonLabel: data.snackbarCustomAction?.label, buttonId: data.snackbarCustomAction?.id, hasButton: true }
		// 		})
		// 	} else 
		// 	snackbar.add({
		// 		type: data.type,
		// 		title: data.title,
		// 		text: { message: data.text }
		// 	})
		// },
		addCustomAction(newAction: SnackbarCustomAction):number {
			newAction.id = Date.now()
			this.customActions.push(newAction)
			return newAction.id
		},
		getRandomNumber(min: number, max: number): number {
			return Math.floor(Math.random() * (max - min + 1)) + min;
		},
	},
})