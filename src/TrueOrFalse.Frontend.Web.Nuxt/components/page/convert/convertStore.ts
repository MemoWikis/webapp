import { defineStore } from "pinia"
import { messages } from "~/components/alert/messages"
import { useSnackbarStore, SnackbarData } from "~/components/snackBar/snackBarStore"

interface ConversionResult {
    success: boolean
    name: string
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
    const keepParents = ref<boolean>(false)

    const openModal = async (id: number) => {
        itemId.value = id
        errorMsg.value = ''
        showErrorMsg.value = false
        keepParents.value = false

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
            conversionTarget.value = ConversionTarget.Page
            name.value = result.name
        } else if (result) {
            conversionTarget.value = ConversionTarget.Wiki
            name.value = result.name
        }
    }

    const closeModal = () => {
        showModal.value = false
    }
    
    const confirmConversion = async () => {
        if (conversionTarget.value === ConversionTarget.Wiki) {
            await convertPageToWiki()
        } else {
            await convertWikiToPage()
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
            const snackbarStore = useSnackbarStore()
			const data: SnackbarData = {
                type: 'success',
                text: messages.success.page.convertedToPage(name.value)
            }
            snackbarStore.showSnackbar(data)
        } else if (result && !result.success && result.messageKey) {
            errorMsg.value = result.messageKey
            showErrorMsg.value = true
        }
    }

    const convertPageToWiki = async () => {
        const result = await $api<ConversionResult>(`/apiVue/ConvertStore/ConvertPageToWiki/`, {
            method: 'POST',
            mode: 'cors',
            credentials: 'include',
            body: { 
                id: itemId.value,
                keepParents: keepParents.value 
            }
        })
        if (result && result.success) {
            const snackbarStore = useSnackbarStore()
			const data: SnackbarData = {
                type: 'success',
                text: messages.success.page.convertedToWiki(name.value)
            }
            snackbarStore.showSnackbar(data)
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
        keepParents,
        openModal,
        closeModal,
        confirmConversion,
        convertPageToWiki,
        convertWikiToPage
    }
})
