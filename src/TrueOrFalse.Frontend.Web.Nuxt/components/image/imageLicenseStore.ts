import { defineStore } from "pinia"

interface ImageLicenseData {
    imageCanBeDisplayed: boolean,
    url?: string,
    alt?: string,
    description?: string,
    attributionHtmlString?: string
}

export const useImageLicenseStore = defineStore('imageLicenseStore', {
    state: () => {
        return {
            show: false,
            url: '',
            id: 0,
            alt: '',
            description: '',
            attributionHtmlString: ''
        }
    },
    actions: {
        async openImage(id: number) {
            await this.loadLicenseInfo(id)
            this.show = true
        },
        async loadLicenseInfo(id: number) {
            const result = await $fetch<ImageLicenseData>(`/apiVue/ImageLicenseStore/GetLicenseInfo/${id}`, { mode: 'cors' })
            if (result.imageCanBeDisplayed) {
                this.url = result.url ?? ''
                this.alt = result.alt ?? ''
            }
            this.description = result.description ?? ''
            this.attributionHtmlString = result.attributionHtmlString ?? ''
            this.show = true
        }
    }
})