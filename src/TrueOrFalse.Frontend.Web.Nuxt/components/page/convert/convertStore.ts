import { defineStore } from "pinia"

interface ConversionResult {
    success: boolean
    messageKey?: string
}

export enum ConversionTarget {
    Wiki,
    Page
}

export const useConvertStore = defineStore('convertStore', () => {

    const errorMsg = ref('')
    const showErrorMsg = ref(false)
    const showModal = ref(false)
    const itemId = ref<number>(0)
    const conversionTarget = ref<ConversionTarget>(ConversionTarget.Wiki)
    const name = ref('')

    const openModal = async (id: number) => {
        itemId.value = id
        errorMsg.value = ''
        showErrorMsg.value = false

        await initConvertData()

        showModal.value = true

    }

    const initConvertData = async () => {
        interface ConvertDataResult {
            isWiki: boolean
            name: string
            messageKey?: string
        }
        const result = await $api<ConvertDataResult>(`/apiVue/ConvertStore/GetConvertData/${itemId.value}`, {
            method: 'GET',
            mode: 'cors',
            credentials: 'include'
        })
        if (result && result.isWiki) {
            conversionTarget.value = ConversionTarget.Wiki
            name.value = result.name
        } else if (result) {
            conversionTarget.value = ConversionTarget.Page
            name.value = result.name
        }
    }

    const closeModal = () => {
        showModal.value = false
    }
    
    const confirmConversion = () => {
        if (conversionTarget.value === ConversionTarget.Wiki) {
            convertWikiToPage()
        } else {
            convertPageToWiki()
        }
        closeModal()
    }

    const convertWikiToPage = async () => {
        const result = await $api<ConversionResult>(`/apiVue/ConvertStore/ConvertWikiToPage/${itemId.value}`, {
            method: 'POST',
            mode: 'cors',
            credentials: 'include',
        })
        if (result && result.success) {
console.log('success')
        } else if (result && !result.success && result.messageKey) {
            errorMsg.value = result.messageKey
            showErrorMsg.value = true
        }
    }

    const convertPageToWiki = async () => {
        const result = await $api<ConversionResult>(`/apiVue/ConvertStore/ConvertPageToWiki/${itemId.value}`, {
            method: 'POST',
            mode: 'cors',
            credentials: 'include'
        })
        if (result && result.success) {
console.log('success')

        } else if (result && !result.success && result.messageKey) {
            errorMsg.value = result.messageKey
            showErrorMsg.value = true
        }
    }

    return {
        errorMsg,
        showErrorMsg,
        showModal,
        itemId,
        conversionTarget,
        name,
        openModal,
        closeModal,
        confirmConversion,
        convertPageToWiki,
        convertWikiToPage
    }
})
