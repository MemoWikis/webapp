import { defineStore } from "pinia"

export const useFigureExtensionStore = defineStore('figureExtensionStore', {
    state: () => {
        return {
            show: false,
            showEdit: false,
            caption: '',
            license: '',
            imageUrl: '',
            imageAlt: '',
            onSaveCallback: null as ((data: { caption: string | null, license: string | null }) => void) | null
        }
    },
    actions: {
        openModal(caption: string, license: string, imageUrl: string, imageAlt: string) {
            this.caption = caption || ''
            this.license = license || ''
            this.imageUrl = imageUrl || ''
            this.imageAlt = imageAlt || ''
            this.show = true
        },
        openEditModal(caption: string, license: string, imageUrl: string, imageAlt: string, onSave: (data: { caption: string | null, license: string | null }) => void) {
            this.caption = caption || ''
            this.license = license || ''
            this.imageUrl = imageUrl || ''
            this.imageAlt = imageAlt || ''
            this.onSaveCallback = onSave
            this.showEdit = true
        },
        closeModal() {
            this.show = false
        },
        closeEditModal() {
            this.showEdit = false
            this.onSaveCallback = null
        },
        saveEdit(data: { caption: string | null, license: string | null }) {
            if (this.onSaveCallback) {
                this.onSaveCallback(data)
            }
            this.closeEditModal()
        }
    }
})
