import { defineStore } from 'pinia'

export enum TargetPosition {
    Before,
    After,
    Inner,
    None
}

export enum DragAndDropType {
	GridItem
}

export interface DropZoneData {
	type: DragAndDropType,
	id: number,
	position: TargetPosition
}
export const useDragStore = defineStore('dragStore', {
	state: () => {
		return {
			active: false,
			transferData: null as any,
			dropZoneData: null as DropZoneData | null,
			clientX: 0,
			clientY: 0
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
		setMouseData(x: number, y: number) {
			const el = document.elementFromPoint(x, y) as any
			const jsonString = el.getAttribute('data-dropzonedata')
			if (jsonString)
				this.dropZoneData = JSON.parse(jsonString)
		}
	},
})