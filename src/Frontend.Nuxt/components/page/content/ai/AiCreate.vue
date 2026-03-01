<script lang="ts" setup>
import { DifficultyLevel, ContentLength, ContentType, InputMode, useAiCreateStore } from './aiCreateStore'
import { useUserStore } from '~/components/user/userStore'
import { useSnackbarStore } from '~/components/snackBar/snackBarStore'
import { usePageStore } from '../../pageStore'
import { useLearningSessionStore } from '~/components/page/learning/learningSessionStore'
import { useLearningSessionConfigurationStore } from '~/components/page/learning/learningSessionConfigurationStore'

const aiCreateStore = useAiCreateStore()
const userStore = useUserStore()
const snackbarStore = useSnackbarStore()
const pageStore = usePageStore()
const learningSessionStore = useLearningSessionStore()
const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
const { $urlHelper } = useNuxtApp()

const { t } = useI18n()

const promptTextArea = ref<HTMLTextAreaElement>()
const minTextAreaHeight = 100

function resizeTextArea() {
    const element = promptTextArea.value
    if (element) {
        element.style.height = `${minTextAreaHeight}px`
        element.style.height = `${Math.max(element.scrollHeight, minTextAreaHeight)}px`
    }
}

const complexityLabels = computed(() => ({
    [DifficultyLevel.ELI5]: t('page.ai.createPage.complexity.simple'),
    [DifficultyLevel.Beginner]: t('page.ai.createPage.complexity.basic'),
    [DifficultyLevel.Intermediate]: t('page.ai.createPage.complexity.standard'),
    [DifficultyLevel.Advanced]: t('page.ai.createPage.complexity.advanced'),
    [DifficultyLevel.Academic]: t('page.ai.createPage.complexity.expert')
}))

const contentLengthLabels = computed(() => ({
    [ContentLength.Short]: t('page.ai.createPage.length.short'),
    [ContentLength.Medium]: t('page.ai.createPage.length.medium'),
    [ContentLength.Long]: t('page.ai.createPage.length.long')
}))

const shouldGenerateWikiWithSubpages = computed(() => {
    return aiCreateStore.contentType === ContentType.Wiki && aiCreateStore.contentLength === ContentLength.Long
})

const isFlashcards = computed(() => aiCreateStore.contentType === ContentType.Flashcards)

const promptLabel = computed(() => {
    if (aiCreateStore.contentType === ContentType.Wiki) {
        return t('page.ai.createPage.promptLabelWiki')
    }
    return t('page.ai.createPage.promptLabel')
})

const showUrlInput = ref(false)

const canGenerate = computed(() => {
    if (aiCreateStore.isGenerating) return false

    if (isFlashcards.value) return true

    if (showUrlInput.value && aiCreateStore.url.trim().length > 0) {
        return aiCreateStore.isValidUrl(aiCreateStore.url.trim())
    }
    return aiCreateStore.prompt.trim().length > 0
})

const hasGeneratedContent = computed(() => {
    if (isFlashcards.value) {
        return aiCreateStore.generatedFlashcards.length > 0
    }
    if (shouldGenerateWikiWithSubpages.value) {
        return aiCreateStore.generatedWikiContent !== null
    }
    return aiCreateStore.generatedContent !== null
})

const canCreate = computed(() => {
    return hasGeneratedContent.value && !aiCreateStore.isGenerating
})

const primaryButtonLabel = computed(() => {
    if (isFlashcards.value) {
        if (aiCreateStore.generatedFlashcards.length > 0) {
            return t('page.ai.createPage.button.createFlashcards')
        }
        return t('page.ai.createPage.button.generate')
    }
    if (hasGeneratedContent.value) {
        return shouldGenerateWikiWithSubpages.value
            ? t('page.ai.createPage.button.createWiki')
            : t('page.ai.createPage.button.create')
    }
    return t('page.ai.createPage.button.generate')
})

const isQuotaDepleted = computed(() => {
    return userStore.quotaInfo?.isQuotaDepleted ?? false
})

const showQuotaDepletedModal = ref(false)

watch(() => aiCreateStore.showModal, (isOpen) => {
    if (isOpen && userStore.isLoggedIn && !userStore.quotaInfo) {
        userStore.fetchQuotaInfo()
    }
    if (isOpen) {
        showUrlInput.value = false
    }
})

async function handleGenerate() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    if (!userStore.quotaInfo) {
        await userStore.fetchQuotaInfo()
    }

    if (userStore.quotaInfo?.isQuotaDepleted) {
        showQuotaDepletedModal.value = true
        return
    }

    if (isFlashcards.value) {
        await aiCreateStore.generateFlashcards(pageStore.id, pageStore.text)
        return
    }

    if (showUrlInput.value && aiCreateStore.url.trim().length > 0) {
        aiCreateStore.inputMode = InputMode.Url
    } else {
        aiCreateStore.inputMode = InputMode.Prompt
    }

    await aiCreateStore.generatePage(shouldGenerateWikiWithSubpages.value)
}

async function handleCreate() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    if (isFlashcards.value) {
        await handleCreateFlashcards()
        return
    }

    const title = shouldGenerateWikiWithSubpages.value
        ? aiCreateStore.generatedWikiContent?.title
        : aiCreateStore.generatedContent?.title

    const result = shouldGenerateWikiWithSubpages.value
        ? await aiCreateStore.createWiki()
        : await aiCreateStore.createPage()

    if (result.success) {
        const messageKey = shouldGenerateWikiWithSubpages.value
            ? 'page.ai.createPage.successWiki'
            : 'page.ai.createPage.success'
        snackbarStore.showSnackbar({
            type: 'success',
            text: { message: t(messageKey) },
            dismissible: true
        })
        pageStore.reloadGridItems()

        const pageId = 'wikiId' in result ? result.wikiId : ('pageId' in result ? result.pageId : undefined)
        if (pageId && title) {
            await navigateTo($urlHelper.getPageUrl(title, pageId))
        }
    } else if (result.messageKey) {
        snackbarStore.showSnackbar({
            type: 'error',
            text: { message: t(result.messageKey) },
            dismissible: true
        })
    }
}

function handlePrimaryAction() {
    if (hasGeneratedContent.value) {
        handleCreate()
    } else {
        handleGenerate()
    }
}

async function handleCreateFlashcards() {
    const sessionConfig = learningSessionConfigurationStore.buildSessionConfigJson(pageStore.id)

    interface CreateFlashcardsResult {
        success: boolean
        ids?: number[]
        messageKey?: string
        lastIndex?: number
    }

    try {
        const result = await $api<CreateFlashcardsResult>('/apiVue/AiCreateFlashcard/Create/', {
            method: 'POST',
            body: {
                pageId: pageStore.id,
                flashCards: aiCreateStore.generatedFlashcards,
                lastIndex: learningSessionStore.lastIndexInQuestionList,
                sessionConfig
            },
            mode: 'cors',
            credentials: 'include',
        })

        if (result.success) {
            pageStore.updateQuestionCount()
            learningSessionConfigurationStore.getQuestionCount()

            if (result.ids && result.lastIndex != null) {
                learningSessionStore.addNewQuestionsToListByLastIndex(result.ids.length, result.lastIndex)
            }

            snackbarStore.showSnackbar({
                type: 'success',
                text: { message: t('success.question.flashcardsAdded', result.ids?.length ?? 0) },
                dismissible: true
            })
            aiCreateStore.resetModal()
            aiCreateStore.closeModal()
        } else if (result.messageKey) {
            snackbarStore.showSnackbar({
                type: 'error',
                text: { message: t(result.messageKey) },
                dismissible: true
            })
        }
    } catch {
        snackbarStore.showSnackbar({
            type: 'error',
            text: { message: t('error.default') },
            dismissible: true
        })
    }
}
</script>

<template>
    <LazyModal :show="aiCreateStore.showModal" :show-cancel-btn="false"
        :disabled="hasGeneratedContent ? !canCreate : !canGenerate" content-class="ai-create-modal"
        container-class="wide-modal" :prevent-backdrop-close="true" @close="aiCreateStore.showModal = false">
        <template #header>
            <h4 class="modal-title">
                <span class="header-icon-wrapper">
                    <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" class="header-icon" />
                </span>
                {{ t('page.ai.createPage.title') }}
            </h4>
        </template>

        <template #body>
            <div id="AiCreate">
                <PageContentAiCreateContentTabs />

                <!-- Flashcards: separate component -->
                <PageContentAiCreateFlashcards v-if="isFlashcards" />

                <!-- Page/Wiki -->
                <template v-else>
                    <div class="form-group">
                        <label for="prompt-input">{{ promptLabel }}</label>
                        <textarea id="prompt-input" ref="promptTextArea" v-model="aiCreateStore.prompt"
                            class="form-control prompt-textarea"
                            :placeholder="t('page.ai.createPage.promptPlaceholder')"
                            :disabled="aiCreateStore.isGenerating" @input="resizeTextArea()" />
                    </div>

                    <!-- Add content from URL link -->
                    <div v-if="!showUrlInput" class="url-toggle-link">
                        <button type="button" class="add-url-btn" :disabled="aiCreateStore.isGenerating"
                            @click="showUrlInput = true">
                            <font-awesome-icon :icon="['fas', 'plus']" />
                            {{ t('page.ai.createPage.addFromUrl') }}
                        </button>
                    </div>

                    <!-- URL Input Section (expandable) -->
                    <div v-if="showUrlInput" class="form-group url-section">
                        <div class="url-header">
                            <label for="url-input">{{ t('page.ai.createPage.urlLabel') }}</label>
                            <button type="button" class="url-close-btn" :disabled="aiCreateStore.isGenerating"
                                @click="showUrlInput = false; aiCreateStore.url = ''">
                                <font-awesome-icon :icon="['fas', 'xmark']" />
                            </button>
                        </div>
                        <input id="url-input" v-model="aiCreateStore.url" type="url" class="form-control url-input"
                            :placeholder="t('page.ai.createPage.urlPlaceholder')"
                            :disabled="aiCreateStore.isGenerating" />
                        <small class="body-s">{{ t('page.ai.createPage.urlHint') }}</small>
                    </div>

                    <!-- Complexity Level & Content Length -->
                    <div class="slider-row">
                        <div class="form-group">
                            <label>{{ t('page.ai.createPage.complexityLabel') }}</label>
                            <PageContentAiCreateSlider v-model="aiCreateStore.difficultyLevel" :min="1" :max="5"
                                :labels="complexityLabels" :left-label="t('page.ai.createPage.complexity.simple')"
                                :right-label="t('page.ai.createPage.complexity.expert')"
                                :disabled="aiCreateStore.isGenerating" />
                        </div>

                        <div class="form-group">
                            <label>{{ t('page.ai.createPage.lengthLabel') }}</label>
                            <PageContentAiCreateSlider v-model="aiCreateStore.contentLength" :min="1" :max="3"
                                :labels="contentLengthLabels" :left-label="t('page.ai.createPage.length.short')"
                                :right-label="t('page.ai.createPage.length.long')"
                                :disabled="aiCreateStore.isGenerating" />
                        </div>
                    </div>

                    <!-- Loading State -->
                    <div v-if="aiCreateStore.isGenerating" class="generating-state">
                        <font-awesome-icon :icon="['fas', 'spinner']" spin />
                        <span>{{ shouldGenerateWikiWithSubpages ? t('page.ai.createPage.generatingWiki') :
                            t('page.ai.createPage.generating') }}</span>
                    </div>

                    <!-- Error Message -->
                    <div v-if="aiCreateStore.errorMessage" class="alert alert-danger">
                        {{ t(aiCreateStore.errorMessage) }}
                    </div>

                    <!-- Preview -->
                    <PageContentAiCreatePreview :is-wiki-with-subpages="shouldGenerateWikiWithSubpages"
                        @regenerate="handleGenerate" />
                </template>
            </div>
        </template>

        <template #footer>
            <PageContentAiCreateFooter :disabled="isQuotaDepleted || (hasGeneratedContent ? !canCreate : !canGenerate)"
                :button-label="primaryButtonLabel" @action="handlePrimaryAction" />
        </template>
    </LazyModal>

    <PageContentAiCreateQuotaDepletedModal :show="showQuotaDepletedModal" @close="showQuotaDepletedModal = false" />
</template>

<style lang="less" scoped>
@import '~~/assets/shared/search.less';
@import (reference) '~~/assets/includes/imports.less';

#AiCreate {
    .url-toggle-link {
        margin-top: -12px;
        margin-bottom: 24px;

        .add-url-btn {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            background: none;
            border: none;
            padding: 0;
            color: @memo-grey-darker;
            font-size: 13px;
            cursor: pointer;

            &:hover {
                color: @memo-blue-link;
            }

            &:disabled {
                opacity: 0.6;
                cursor: not-allowed;
            }
        }
    }

    .url-section {
        .url-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 8px;

            label {
                margin-bottom: 0;
            }

            .url-close-btn {
                display: flex;
                align-items: center;
                justify-content: center;
                width: 24px;
                height: 24px;
                background: none;
                border: none;
                color: @memo-grey-darker;
                cursor: pointer;
                padding: 0;

                &:hover {
                    color: @memo-grey-darkest;
                }
            }
        }
    }

    .slider-row {
        display: flex;
        gap: 24px;
        margin-bottom: 24px;

        .form-group {
            flex: 1;
            margin-bottom: 0;
        }

        @media (max-width: 767px) {
            flex-direction: column;
            gap: 0;

            .form-group {
                margin-bottom: 24px;
            }
        }
    }

    .form-group {
        margin-bottom: 24px;

        label {
            font-weight: 600;
            margin-bottom: 8px;
            display: block;
        }
    }

    .prompt-textarea {
        width: 100%;
        resize: none;
        min-height: 100px;
        height: 100px;
        overflow: hidden;
        border-radius: 0px;
        padding: 12px;
        border-color: @memo-grey-light;

        &:focus {
            border-color: @memo-green;
            outline: none;
        }
    }

    .url-input {
        width: 100%;
        border-radius: 0px;
        padding: 12px;
        border-color: @memo-grey-light;

        &:focus {
            border-color: @memo-green;
            outline: none;
        }
    }

    .generating-state {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 12px;
        padding: 24px;
        color: @memo-blue;
        font-size: 16px;
    }
}

.modal-title {
    display: flex;
    align-items: center;
    gap: 12px;
    margin: 0;

    .header-icon-wrapper {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 36px;
        height: 36px;
        background: fade(@memo-blue-link, 15%);
        border-radius: 8px;

        .header-icon {
            color: @memo-blue-link;
            font-size: 16px;
        }
    }
}
</style>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.detail-dropdown-popper {
    width: calc(100vw - 80px);

    .dropdown-row {
        &.active {
            background: @memo-grey-lightest;
            color: @memo-blue-link;
            font-weight: 600;
        }
    }
}
</style>
