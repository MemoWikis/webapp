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
			x: 0,
			y: 0,
			screenX: 0,
			screenY: 0,
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
		setMouseData(x: number, y: number, screenX?: number, screenY?: number) {
			this.setMousePosition(x,y,screenX,screenY)
			if (screenX && screenY) {
				const el = document.elementFromPoint(x, y) as any
				const jsonString = el?.getAttribute('data-dropzonedata')
				if (jsonString)
					this.dropZoneData = JSON.parse(jsonString)
			}
		},
		setMousePosition(x: number, y: number, screenX?: number, screenY?: number) {
			this.x = x
			this.y = y

			if (screenX && screenY) {
				this.screenX = screenX
				this.screenY = screenY
			}
		}
	},
})