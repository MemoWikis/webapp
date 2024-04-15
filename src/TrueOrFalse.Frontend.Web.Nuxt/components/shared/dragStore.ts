import { defineStore } from 'pinia'
import { GridTopicItem } from '../topic/content/grid/item/gridTopicItem'

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
	position: TargetPosition,
	parentId: number
}

export interface MoveTopicTransferData {
	topic: GridTopicItem
    oldParentId: number
}

export const useDragStore = defineStore('dragStore', {
	state: () => {
		return {
			active: false,
			transferData: null as MoveTopicTransferData | string | null,
			dropZoneData: null as DropZoneData | null,
			x: 0,
			y: 0,
			touchX: 0,
			touchY: 0,
		}
	},
	actions: {
		dragStart(e: MoveTopicTransferData | string) {
			this.active = true
			this.transferData = e
		},
		dragEnd() {
			this.active = false
			this.transferData = null
			this.dropZoneData = null
		},
		setMouseData(x: number, y: number, touchX?: number, touchY?: number) {
			this.setMousePosition(x,y,touchX,touchY)
			if (touchX && touchY) {
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
				this.touchX = screenX
				this.touchY = screenY
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
				if ('topic' in this.transferData && typeof this.transferData.topic === 'object' &&
					'oldParentId' in this.transferData && typeof this.transferData.oldParentId === 'number') {
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