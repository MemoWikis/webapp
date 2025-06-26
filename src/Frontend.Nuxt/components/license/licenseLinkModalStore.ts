import { defineStore } from "pinia"

interface LicenseData {
    isDefault: boolean
    shortText: string
    fullText: string
}

interface CreatorData {
    id: number
    name: string
}

export const useLicenseLinkModalStore = defineStore('licenseLinkModalStore', {
    state: () => {
        return {
            show: false,
            license: {
                isDefault: true,
                shortText: '',
                fullText: ''
            } as LicenseData,
            creator: {
                id: 0,
                name: ''
            } as CreatorData
        }
    },
    actions: {
        openModal(license: LicenseData, creator: CreatorData) {
            this.license = { ...license }
            this.creator = { ...creator }
            this.show = true
        },
        closeModal() {
            this.show = false
        }
    }
})
