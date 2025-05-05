import { defineStore } from "pinia"

export interface SnackbarData {
    type?: "warning" | "error" | "info" | "success"
    title?: string
    text?: SnackbarMessage
    snackbarCustomAction?: SnackbarCustomAction
    duration?: number
    dismissible?: boolean
}

export interface SnackbarMessage {
    message: string
    buttonId?: number
    buttonLabel?: string
    buttonIcon?: string[]
}

export interface SnackbarCustomAction {
    id?: number
    label: string
    action: () => void
    icon?: string[]
}

export const useSnackbarStore = defineStore("snackbarStore", {
    state: () => {
        return {
            customActions: [] as SnackbarCustomAction[],
        }
    },
    actions: {
        addCustomAction(newAction: SnackbarCustomAction): number {
            newAction.id = Date.now()
            this.customActions.push(newAction)
            return newAction.id
        },
        getRandomNumber(min: number, max: number): number {
            return Math.floor(Math.random() * (max - min + 1)) + min
        },
        showSnackbar(snackbarData: SnackbarData) {
            return snackbarData
        },
    },
})
