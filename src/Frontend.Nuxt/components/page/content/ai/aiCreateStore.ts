import { defineStore } from 'pinia'

export enum DifficultyLevel {
    ELI5 = 1,
    Beginner = 2,
    Intermediate = 3,
    Advanced = 4,
    Academic = 5,
}

export enum ContentLength {
    Short = 1,
    Medium = 2,
    Long = 3,
}

export enum InputMode {
    Prompt = 'prompt',
    Url = 'url',
}

export enum ContentType {
    Page = 'page',
    Wiki = 'wiki',
    Flashcards = 'flashcards',
}

export interface GeneratedPageContent {
    title: string
    htmlContent: string
}

export interface GeneratedSubpage {
    title: string
    htmlContent: string
}

export interface GeneratedWikiContent {
    title: string
    htmlContent: string
    subpages: GeneratedSubpage[]
}

export interface GeneratedFlashcard {
    front: string
    back: string
}

export interface AiModel {
    modelId: string
    displayName: string
    provider: string
    tokenCostMultiplier: number
    isDefault: boolean
}

export const useAiCreateStore = defineStore('aiCreateStore', () => {
    const showModal = ref(false)
    const isGenerating = ref(false)
    const inputMode = ref<InputMode>(InputMode.Prompt)
    const prompt = ref('')
    const url = ref('')
    const difficultyLevel = ref<DifficultyLevel>(DifficultyLevel.Intermediate)
    const contentLength = ref<ContentLength>(ContentLength.Medium)
    const generatedContent = ref<GeneratedPageContent | null>(null)
    const generatedWikiContent = ref<GeneratedWikiContent | null>(null)
    const parentId = ref(0)
    const errorMessage = ref('')
    const contentType = ref<ContentType>(ContentType.Page)
    const createAsWiki = computed(() => contentType.value === ContentType.Wiki)
    const selectedSubpageIndex = ref<number | null>(null)
    const generatedFlashcards = ref<GeneratedFlashcard[]>([])

    // AI Model selection
    const availableModels = ref<AiModel[]>([])
    const selectedModelId = ref<string>('')
    const isLoadingModels = ref(false)

    /**
     * Selects the best default model when no user preference exists.
     * Prefers models with tokenCostMultiplier close to 1 (middle-tier).
     */
    const selectDefaultModel = (models: AiModel[]): string => {
        if (models.length === 0) {
            return ''
        }

        // Sort by distance from 1.0 (prefer middle-tier models)
        const sortedByMiddle = [...models].sort((a, b) => {
            const distanceA = Math.abs(a.tokenCostMultiplier - 1)
            const distanceB = Math.abs(b.tokenCostMultiplier - 1)
            return distanceA - distanceB
        })

        return sortedByMiddle[0].modelId
    }

    /**
     * Saves the user's preferred model to the backend.
     */
    const savePreferredModel = async (modelId: string) => {
        try {
            await $api('/apiVue/AiCreatePage/SavePreferredModel', {
                method: 'POST',
                body: { modelId },
                mode: 'cors',
                credentials: 'include',
            })
        } catch {
            console.error('Failed to save preferred model')
        }
    }

    /**
     * Sets the selected model and saves it as the user's preference.
     */
    const setSelectedModel = (modelId: string) => {
        selectedModelId.value = modelId
        savePreferredModel(modelId)
    }

    async function fetchModels() {
        if (availableModels.value.length > 0) {
            return // Already loaded
        }

        isLoadingModels.value = true
        try {
            interface GetModelsResponse {
                success: boolean
                models: AiModel[]
                preferredModelId?: string | null
            }

            const result = await $api<GetModelsResponse>(
                '/apiVue/AiCreatePage/GetModels',
                {
                    method: 'GET',
                    mode: 'cors',
                    credentials: 'include',
                },
            )

            if (result.success && result.models) {
                availableModels.value = result.models

                // Priority: 1. User's saved preference, 2. Middle-tier model (multiplier ~1)
                if (
                    result.preferredModelId &&
                    result.models.some(
                        (m) => m.modelId === result.preferredModelId,
                    )
                ) {
                    selectedModelId.value = result.preferredModelId
                } else {
                    selectedModelId.value = selectDefaultModel(result.models)
                }
            }
        } catch (error) {
            console.error('Failed to fetch AI models:', error)
        } finally {
            isLoadingModels.value = false
        }
    }

    function openModal(newParentId: number) {
        parentId.value = newParentId
        showModal.value = true
        inputMode.value = InputMode.Prompt
        prompt.value = ''
        url.value = ''
        difficultyLevel.value = DifficultyLevel.Intermediate
        contentLength.value = ContentLength.Medium
        generatedContent.value = null
        generatedWikiContent.value = null
        generatedFlashcards.value = []
        errorMessage.value = ''
        contentType.value = ContentType.Page
        selectedSubpageIndex.value = null

        // Fetch available models when opening modal
        fetchModels()
    }

    function closeModal() {
        showModal.value = false
        prompt.value = ''
        url.value = ''
        generatedContent.value = null
        generatedWikiContent.value = null
        generatedFlashcards.value = []
        errorMessage.value = ''
        contentType.value = ContentType.Page
        selectedSubpageIndex.value = null
    }

    function isValidUrl(urlString: string): boolean {
        try {
            const parsedUrl = new URL(urlString)
            return (
                parsedUrl.protocol === 'http:' ||
                parsedUrl.protocol === 'https:'
            )
        } catch {
            return false
        }
    }

    async function generatePage(generateWikiWithSubpages: boolean = false) {
        if (inputMode.value === InputMode.Prompt && !prompt.value.trim()) {
            return
        }
        if (
            inputMode.value === InputMode.Url &&
            !isValidUrl(url.value.trim())
        ) {
            errorMessage.value = 'error.ai.invalidUrl'
            return
        }

        isGenerating.value = true
        errorMessage.value = ''
        generatedContent.value = null
        generatedWikiContent.value = null
        selectedSubpageIndex.value = null

        try {
            if (generateWikiWithSubpages) {
                await generateWikiWithSubpagesApi()
            } else {
                await generateSinglePage()
            }
        } catch {
            errorMessage.value = 'error.default'
        } finally {
            isGenerating.value = false
        }
    }

    async function generateSinglePage() {
        interface GeneratePageResponse {
            success: boolean
            data?: GeneratedPageContent
            messageKey?: string
        }

        const endpoint =
            inputMode.value === InputMode.Url
                ? '/apiVue/AiCreatePage/GenerateFromUrl'
                : '/apiVue/AiCreatePage/Generate'

        const body =
            inputMode.value === InputMode.Url
                ? {
                      url: url.value.trim(),
                      difficultyLevel: difficultyLevel.value,
                      contentLength: contentLength.value,
                      parentId: parentId.value,
                      modelId: selectedModelId.value || undefined,
                  }
                : {
                      prompt: prompt.value,
                      difficultyLevel: difficultyLevel.value,
                      contentLength: contentLength.value,
                      parentId: parentId.value,
                      modelId: selectedModelId.value || undefined,
                  }

        const result = await $api<GeneratePageResponse>(endpoint, {
            method: 'POST',
            body,
            mode: 'cors',
            credentials: 'include',
        })

        if (result.success && result.data) {
            generatedContent.value = result.data
        } else if (result.messageKey) {
            errorMessage.value = result.messageKey
        }
    }

    async function generateWikiWithSubpagesApi() {
        interface GenerateWikiResponse {
            success: boolean
            data?: GeneratedWikiContent
            messageKey?: string
        }

        const endpoint =
            inputMode.value === InputMode.Url
                ? '/apiVue/AiCreatePage/GenerateWikiFromUrl'
                : '/apiVue/AiCreatePage/GenerateWiki'

        const body =
            inputMode.value === InputMode.Url
                ? {
                      url: url.value.trim(),
                      difficultyLevel: difficultyLevel.value,
                      parentId: parentId.value,
                      modelId: selectedModelId.value || undefined,
                  }
                : {
                      prompt: prompt.value,
                      difficultyLevel: difficultyLevel.value,
                      parentId: parentId.value,
                      modelId: selectedModelId.value || undefined,
                  }

        const result = await $api<GenerateWikiResponse>(endpoint, {
            method: 'POST',
            body,
            mode: 'cors',
            credentials: 'include',
        })

        if (result.success && result.data) {
            generatedWikiContent.value = result.data
        } else if (result.messageKey) {
            errorMessage.value = result.messageKey
        }
    }

    async function createPage(): Promise<{
        success: boolean
        pageId?: number
        messageKey?: string
    }> {
        if (!generatedContent.value) {
            return { success: false, messageKey: 'error.noContent' }
        }

        interface CreatePageResponse {
            success: boolean
            pageId?: number
            messageKey?: string
        }

        try {
            const result = await $api<CreatePageResponse>(
                '/apiVue/AiCreatePage/Create',
                {
                    method: 'POST',
                    body: {
                        title: generatedContent.value.title,
                        htmlContent: generatedContent.value.htmlContent,
                        parentId: parentId.value,
                        isWiki: createAsWiki.value,
                    },
                    mode: 'cors',
                    credentials: 'include',
                },
            )

            if (result.success) {
                closeModal()
            }

            return result
        } catch {
            return { success: false, messageKey: 'error.default' }
        }
    }

    async function generateFlashcards(pageId: number, pageText: string) {
        isGenerating.value = true
        errorMessage.value = ''
        generatedFlashcards.value = []

        try {
            interface GenerateFlashcardResponse {
                flashcards: GeneratedFlashcard[]
                messageKey: string
            }

            const result = await $api<GenerateFlashcardResponse>('/apiVue/PageStore/GenerateFlashCard/', {
                method: 'POST',
                body: { pageId, text: pageText },
                mode: 'cors',
                credentials: 'include',
            })

            if (result.flashcards && result.flashcards.length > 0) {
                generatedFlashcards.value = result.flashcards
            }
            if (result.messageKey) {
                errorMessage.value = result.messageKey
            }
        } catch {
            errorMessage.value = 'error.default'
        } finally {
            isGenerating.value = false
        }
    }

    function deleteFlashcard(index: number) {
        generatedFlashcards.value.splice(index, 1)
    }

    async function createWiki(): Promise<{
        success: boolean
        wikiId?: number
        subpageIds?: number[]
        messageKey?: string
    }> {
        if (!generatedWikiContent.value) {
            return { success: false, messageKey: 'error.noContent' }
        }

        interface CreateWikiResponse {
            success: boolean
            wikiId?: number
            subpageIds?: number[]
            messageKey?: string
        }

        try {
            const result = await $api<CreateWikiResponse>(
                '/apiVue/AiCreatePage/CreateWiki',
                {
                    method: 'POST',
                    body: {
                        title: generatedWikiContent.value.title,
                        htmlContent: generatedWikiContent.value.htmlContent,
                        subpages: generatedWikiContent.value.subpages.map(
                            (s) => ({
                                title: s.title,
                                htmlContent: s.htmlContent,
                            }),
                        ),
                        parentId: parentId.value,
                    },
                    mode: 'cors',
                    credentials: 'include',
                },
            )

            if (result.success) {
                closeModal()
            }

            return result
        } catch {
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
        contentType,
        generatedContent,
        generatedWikiContent,
        generatedFlashcards,
        parentId,
        errorMessage,
        createAsWiki,
        selectedSubpageIndex,
        availableModels,
        selectedModelId,
        isLoadingModels,
        openModal,
        closeModal,
        generatePage,
        generateFlashcards,
        deleteFlashcard,
        createPage,
        createWiki,
        isValidUrl,
        fetchModels,
        setSelectedModel,
    }
})
