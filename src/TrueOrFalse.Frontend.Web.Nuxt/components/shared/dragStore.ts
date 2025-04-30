import { defineStore } from "pinia"
import { GridPageItem } from "../page/content/grid/item/gridPageItem"

export enum TargetPosition {
    Before,
    After,
    Inner,
    None,
}

export enum DragAndDropType {
    GridItem,
}

export interface DropZoneData {
    type: DragAndDropType
    id: number
    position: TargetPosition
    parentId: number
}

export interface MovePageTransferData {
    page: GridPageItem
    oldParentId: number
}

export const useDragStore = defineStore("dragStore", {
    state: () => {
        return {
            active: false,
            transferData: null as MovePageTransferData | string | null,
            dropZoneData: null as DropZoneData | null,
            x: 0,
            y: 0,
            touchX: 0,
            touchY: 0,
            showTouchSpinner: false,
            touchClientX: 0,
            touchClientY: 0,
            isDraggable: true,
        }
    },
    actions: {
        setTransferData(e: MovePageTransferData | string) {
            this.transferData = e
        },
        dragStart(e: MovePageTransferData | string) {
            this.active = true
            this.transferData = e
        },
        dragEnd() {
            this.active = false
            this.transferData = null
            this.dropZoneData = null
        },
        setMouseData(x: number, y: number, touchX?: number, touchY?: number) {
            this.setMousePosition(x, y, touchX, touchY)
            if (touchX && touchY) {
                const el = document.elementFromPoint(x, y) as any
                const jsonString = el?.getAttribute("data-dropzonedata")
                if (jsonString) this.dropZoneData = JSON.parse(jsonString)
                else this.dropZoneData = null
            }
        },
        setMousePosition(
            x: number,
            y: number,
            touchX?: number,
            touchY?: number
        ) {
            this.x = x
            this.y = y

            if (touchX && touchY) {
                this.touchX = touchX
                this.touchY = touchY
            }
        },
        setTouchPositionForDrag(x: number, y: number) {
            this.touchClientX = x
            this.touchClientY = y
        },
        disableDrag() {
            this.isDraggable = false
            console.log("disableDrag")
        },
        enableDrag() {
            this.isDraggable = true
            console.log("enableDrag")
        },
    },
    getters: {
        transferDataType(): string {
            if (typeof this.transferData === "string") {
                return "string"
            } else if (this.transferData === null) {
                return "null"
            } else if (typeof this.transferData === "object") {
                if (
                    "page" in this.transferData &&
                    typeof this.transferData.page === "object" &&
                    "oldParentId" in this.transferData &&
                    typeof this.transferData.oldParentId === "number"
                ) {
                    return "MovePageTransferData"
                }
            }
            return "unknown"
        },
        isMovePageTransferData(): boolean {
            return this.transferDataType == "MovePageTransferData"
        },
    },
})
