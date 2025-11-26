import { defineStore } from 'pinia'

export enum DifficultyLevel {
    ELI5 = 1,
    Beginner = 2,
    Intermediate = 3,
    Advanced = 4,
    Academic = 5
}

export interface GeneratedPageContent {
    title: string
    htmlContent: string
}

export const useAiCreatePageStore = defineStore('aiCreatePageStore', () => {
    const showModal = ref(false)
    const isGenerating = ref(false)
    const prompt = ref('')
    const difficultyLevel = ref<DifficultyLevel>(DifficultyLevel.Intermediate)
    const generatedContent = ref<GeneratedPageContent | null>(null)
    const parentId = ref(0)
    const errorMessage = ref('')

    function openModal(newParentId: number) {
        parentId.value = newParentId
        showModal.value = true
        prompt.value = ''
        difficultyLevel.value = DifficultyLevel.Intermediate
        generatedContent.value = null
        errorMessage.value = ''
    }

    function closeModal() {
        showModal.value = false
        prompt.value = ''
        generatedContent.value = null
        errorMessage.value = ''
    }

    async function generatePage() {
        if (!prompt.value.trim()) {
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
            const result = await $api<GeneratePageResponse>('/apiVue/AiCreatePage/Generate', {
                method: 'POST',
                body: {
                    prompt: prompt.value,
                    difficultyLevel: difficultyLevel.value,
                    parentId: parentId.value
                },
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
        prompt,
        difficultyLevel,
        generatedContent,
        parentId,
        errorMessage,
        openModal,
        closeModal,
        generatePage,
        createPage
    }
})
