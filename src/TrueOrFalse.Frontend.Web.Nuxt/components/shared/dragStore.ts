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

export interface MoveTopicTransferData {
    movingTopicId: number
    oldParentId: number
    topicName: string
}

export const useDragStore = defineStore('dragStore', {
	state: () => {
		return {
			active: false,
			transferData: null as MoveTopicTransferData | string | null,
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
	getters: {
		transferDataType(): string {
			if (typeof this.transferData === 'string') {
				return 'string'
			} else if (this.transferData === null) {
				return 'null'
			} else if (typeof this.transferData === 'object') {
				if ('movingTopicId' in this.transferData && typeof this.transferData.movingTopicId === 'number' &&
					'oldParentId' in this.transferData && typeof this.transferData.oldParentId === 'number' &&
					'topicName' in this.transferData && typeof this.transferData.topicName === 'string') {
				return 'MoveTopicTransferData'
				}
			}
			return 'unknown'
		},
		isMoveTopicTransferData(): boolean {
			return this.transferDataType == 'MoveTopicTransferData'
		}
	}
})