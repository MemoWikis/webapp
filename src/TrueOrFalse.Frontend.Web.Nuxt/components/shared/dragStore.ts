import { defineStore } from 'pinia'

export const useDragStore = defineStore('dragStore', {
	state: () => {
		return {
			active: false,
		}
	},
	actions: {
		dragStart() {
			this.active = true
		},
		dragEnd() {
			this.active = false
		},
	},
})