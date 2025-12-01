import { defineStore } from 'pinia'

export enum DifficultyLevel {
    ELI5 = 1,
    Beginner = 2,
    Intermediate = 3,
    Advanced = 4,
    Academic = 5
}

export enum ContentLength {
    Short = 1,
    Medium = 2,
    Long = 3
}

export enum InputMode {
    Prompt = 'prompt',
    Url = 'url'
}

export interface GeneratedPageContent {
    title: string
    htmlContent: string
}

export const useAiCreatePageStore = defineStore('aiCreatePageStore', () => {
    const showModal = ref(false)
    const isGenerating = ref(false)
    const inputMode = ref<InputMode>(InputMode.Prompt)
    const prompt = ref('')
    const url = ref('')
    const difficultyLevel = ref<DifficultyLevel>(DifficultyLevel.Intermediate)
    const contentLength = ref<ContentLength>(ContentLength.Medium)
    const generatedContent = ref<GeneratedPageContent | null>(null)
    const parentId = ref(0)
    const errorMessage = ref('')

    function openModal(newParentId: number) {
        parentId.value = newParentId
        showModal.value = true
        inputMode.value = InputMode.Prompt
        prompt.value = ''
        url.value = ''
        difficultyLevel.value = DifficultyLevel.Intermediate
        contentLength.value = ContentLength.Medium
        generatedContent.value = null
        errorMessage.value = ''
    }

    function closeModal() {
        showModal.value = false
        prompt.value = ''
        url.value = ''
        generatedContent.value = null
        errorMessage.value = ''
    }

    function isValidUrl(urlString: string): boolean {
        try {
            const parsedUrl = new URL(urlString)
            return parsedUrl.protocol === 'http:' || parsedUrl.protocol === 'https:'
        } catch {
            return false
        }
    }

    async function generatePage() {
        if (inputMode.value === InputMode.Prompt && !prompt.value.trim()) {
            return
        }
        if (inputMode.value === InputMode.Url && !isValidUrl(url.value.trim())) {
            errorMessage.value = 'error.ai.invalidUrl'
            return
        }

        isGenerating.value = true
        errorMessage.value = ''

        interface GeneratePageResponse {
            success: boolean
            data?: GeneratedPageContent
            messageKey?: string
        }

        try {
            const endpoint = inputMode.value === InputMode.Url 
                ? '/apiVue/AiCreatePage/GenerateFromUrl'
                : '/apiVue/AiCreatePage/Generate'
            
            const body = inputMode.value === InputMode.Url
                ? {
                    url: url.value.trim(),
                    difficultyLevel: difficultyLevel.value,
                    contentLength: contentLength.value,
                    parentId: parentId.value
                }
                : {
                    prompt: prompt.value,
                    difficultyLevel: difficultyLevel.value,
                    contentLength: contentLength.value,
                    parentId: parentId.value
                }

            const result = await $api<GeneratePageResponse>(endpoint, {
                method: 'POST',
                body,
                mode: 'cors',
                credentials: 'include'
            })

            if (result.success && result.data) {
                generatedContent.value = result.data
            } else if (result.messageKey) {
                errorMessage.value = result.messageKey
            }
        } catch (error) {
            errorMessage.value = 'error.default'
        } finally {
            isGenerating.value = false
        }
    }

    async function createPage(): Promise<{ success: boolean; pageId?: number; messageKey?: string }> {
        if (!generatedContent.value) {
            return { success: false, messageKey: 'error.noContent' }
        }

        interface CreatePageResponse {
            success: boolean
            pageId?: number
            messageKey?: string
        }

        try {
            const result = await $api<CreatePageResponse>('/apiVue/AiCreatePage/Create', {
                method: 'POST',
                body: {
                    title: generatedContent.value.title,
                    htmlContent: generatedContent.value.htmlContent,
                    parentId: parentId.value
                },
                mode: 'cors',
                credentials: 'include'
            })

            if (result.success) {
                closeModal()
            }

            return result
        } catch (error) {
            return { success: false, messageKey: 'error.default' }
        }
    }

    return {
        showModal,
        isGenerating,
        inputMode,
        prompt,
        url,
        difficultyLevel,
        contentLength,
        generatedContent,
        parentId,
        errorMessage,
        openModal,
        closeModal,
        generatePage,
        createPage,
        isValidUrl
    }
})
