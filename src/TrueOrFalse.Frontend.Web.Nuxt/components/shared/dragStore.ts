import { defineStore } from 'pinia'

export const useDragStore = defineStore('dragStore', {
	state: () => {
		return {
			active: false,
			transferData: null as any
		}
	},
	actions: {
		dragStart(e: any) {
			this.active = true
			this.transferData = e
		},
		dragEnd() {
			this.active = false
			this.transferData = null
		},
	},
})