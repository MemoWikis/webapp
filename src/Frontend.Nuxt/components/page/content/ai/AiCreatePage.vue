<script lang="ts" setup>
import { VueElement } from 'vue'
import { DifficultyLevel, ContentLength, InputMode, useAiCreatePageStore } from './aiCreatePageStore'
import { useUserStore } from '~/components/user/userStore'
import { SnackbarData, useSnackbarStore } from '~/components/snackBar/snackBarStore'
import { usePageStore } from '../../pageStore'

const aiCreatePageStore = useAiCreatePageStore()
const userStore = useUserStore()
const snackbarStore = useSnackbarStore()
const pageStore = usePageStore()
const { t } = useI18n()
const { isMobile } = useDevice()
const detailDropdownAriaId = useId()

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

const currentComplexityLabel = computed(() => {
    return complexityLabels.value[aiCreatePageStore.difficultyLevel]
})

const currentContentLengthLabel = computed(() => {
    return contentLengthLabels.value[aiCreatePageStore.contentLength]
})

// Group models by provider for the dropdown
const groupedModels = computed(() => {
    const groups: { name: string; models: typeof aiCreatePageStore.availableModels }[] = []
    const providers = new Set(aiCreatePageStore.availableModels.map(model => model.provider))

    for (const provider of providers) {
        groups.push({
            name: provider,
            models: aiCreatePageStore.availableModels.filter(model => model.provider === provider)
        })
    }

    return groups
})

const selectedModelCostMultiplier = computed(() => {
    const model = aiCreatePageStore.availableModels.find(model => model.modelId === aiCreatePageStore.selectedModelId)
    return model?.tokenCostMultiplier ?? 1
})

const selectedModelDisplayName = computed(() => {
    const model = aiCreatePageStore.availableModels.find(model => model.modelId === aiCreatePageStore.selectedModelId)
    return model?.displayName ?? ''
})

// Wiki with subpages is generated when: createAsWiki is checked AND content length is Long
const shouldGenerateWikiWithSubpages = computed(() => {
    return aiCreatePageStore.createAsWiki && aiCreatePageStore.contentLength === ContentLength.Long
})

const canGenerate = computed(() => {
    if (aiCreatePageStore.isGenerating) return false

    if (aiCreatePageStore.inputMode === InputMode.Url) {
        return aiCreatePageStore.isValidUrl(aiCreatePageStore.url.trim())
    }
    return aiCreatePageStore.prompt.trim().length > 0
})

const hasGeneratedContent = computed(() => {
    if (shouldGenerateWikiWithSubpages.value) {
        return aiCreatePageStore.generatedWikiContent !== null
    }
    return aiCreatePageStore.generatedContent !== null
})

const canCreate = computed(() => {
    return hasGeneratedContent.value && !aiCreatePageStore.isGenerating
})

const primaryButtonLabel = computed(() => {
    if (hasGeneratedContent.value) {
        return shouldGenerateWikiWithSubpages.value
            ? t('page.ai.createPage.button.createWiki')
            : t('page.ai.createPage.button.create')
    }
    return t('page.ai.createPage.button.generate')
})

const currentPreviewContent = computed(() => {
    if (shouldGenerateWikiWithSubpages.value && aiCreatePageStore.generatedWikiContent) {
        if (aiCreatePageStore.selectedSubpageIndex !== null && aiCreatePageStore.generatedWikiContent.subpages[aiCreatePageStore.selectedSubpageIndex]) {
            return aiCreatePageStore.generatedWikiContent.subpages[aiCreatePageStore.selectedSubpageIndex]
        }
        return {
            title: aiCreatePageStore.generatedWikiContent.title,
            htmlContent: aiCreatePageStore.generatedWikiContent.htmlContent
        }
    }
    return aiCreatePageStore.generatedContent
})

async function handleGenerate() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }
    await aiCreatePageStore.generatePage(shouldGenerateWikiWithSubpages.value)
}

async function handleCreate() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    if (shouldGenerateWikiWithSubpages.value) {
        const result = await aiCreatePageStore.createWiki()
        if (result.success && result.wikiId) {
            const data: SnackbarData = {
                type: 'success',
                text: { message: t('page.ai.createPage.successWiki') },
                dismissible: true
            }
            snackbarStore.showSnackbar(data)
            pageStore.reloadGridItems()
        } else if (result.messageKey) {
            const data: SnackbarData = {
                type: 'error',
                text: { message: t(result.messageKey) },
                dismissible: true
            }
            snackbarStore.showSnackbar(data)
        }
    } else {
        const result = await aiCreatePageStore.createPage()
        if (result.success && result.pageId) {
            const data: SnackbarData = {
                type: 'success',
                text: { message: t('page.ai.createPage.success') },
                dismissible: true
            }
            snackbarStore.showSnackbar(data)
            pageStore.reloadGridItems()
        } else if (result.messageKey) {
            const data: SnackbarData = {
                type: 'error',
                text: { message: t(result.messageKey) },
                dismissible: true
            }
            snackbarStore.showSnackbar(data)
        }
    }
}

function selectWikiOverview() {
    aiCreatePageStore.selectedSubpageIndex = null
}

function selectSubpage(index: number) {
    aiCreatePageStore.selectedSubpageIndex = index
}
</script>

<template>
    <LazyModal
        :show="aiCreatePageStore.showModal"
        @close="aiCreatePageStore.closeModal()"
        :primary-btn-label="primaryButtonLabel"
        @primary-btn="hasGeneratedContent ? handleCreate() : handleGenerate()"
        :show-cancel-btn="true"
        :disabled="hasGeneratedContent ? !canCreate : !canGenerate"
        content-class="ai-create-page-modal">
        <template #header>
            <h4 class="modal-title">
                <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" class="header-icon" />
                {{ t('page.ai.createPage.title') }}
            </h4>
        </template>

        <template #body>
            <div id="AiCreatePage">
                <!-- Input Mode Toggle -->
                <div class="input-mode-toggle">
                    <button
                        type="button"
                        class="mode-btn"
                        :class="{ active: aiCreatePageStore.inputMode === InputMode.Prompt }"
                        @click="aiCreatePageStore.inputMode = InputMode.Prompt"
                        :disabled="aiCreatePageStore.isGenerating">
                        <font-awesome-icon :icon="['fas', 'pen']" />
                        {{ t('page.ai.createPage.modePrompt') }}
                    </button>
                    <button
                        type="button"
                        class="mode-btn"
                        :class="{ active: aiCreatePageStore.inputMode === InputMode.Url }"
                        @click="aiCreatePageStore.inputMode = InputMode.Url"
                        :disabled="aiCreatePageStore.isGenerating">
                        <font-awesome-icon :icon="['fas', 'link']" />
                        {{ t('page.ai.createPage.modeUrl') }}
                    </button>
                </div>

                <!-- Create as Wiki Toggle -->
                <div class="wiki-toggle">
                    <label class="wiki-toggle-label" @click="aiCreatePageStore.createAsWiki = !aiCreatePageStore.createAsWiki">
                        <span class="toggle-checkbox">
                            <font-awesome-icon v-if="aiCreatePageStore.createAsWiki" :icon="['fas', 'square-check']" class="checked" />
                            <font-awesome-icon v-else :icon="['far', 'square']" />
                        </span>
                        <span>{{ t('page.ai.createPage.createAsWiki') }}</span>
                    </label>
                </div>

                <!-- AI Model Selection -->
                <div class="form-group model-section">
                    <label>{{ t('page.ai.createPage.modelLabel') }}</label>
                    <VDropdown :distance="0" class="model-dropdown">
                        <div class="model-select" :class="{ disabled: aiCreatePageStore.isGenerating || aiCreatePageStore.isLoadingModels }">
                            <span v-if="aiCreatePageStore.isLoadingModels">{{ t('page.ai.createPage.loadingModels') }}</span>
                            <span v-else>{{ selectedModelDisplayName || t('page.ai.createPage.selectModel') }} ({{ selectedModelCostMultiplier }}x)</span>
                            <font-awesome-icon :icon="['fas', 'chevron-down']" />
                        </div>

                        <template #popper="{ hide }">
                            <div class="model-dropdown-menu model-dropdown-popper">
                                <template v-for="provider in groupedModels" :key="provider.name">
                                    <div class="provider-header">{{ provider.name }}</div>
                                    <div
                                        v-for="model in provider.models"
                                        :key="model.modelId"
                                        class="dropdown-row ai-model-option"
                                        :class="{ active: aiCreatePageStore.selectedModelId === model.modelId }"
                                        @click="aiCreatePageStore.selectedModelId = model.modelId; hide()">
                                        <span>{{ model.displayName }}</span> <span class="token-cost-multiplier">{{ model.tokenCostMultiplier }}x</span>
                                    </div>
                                </template>
                            </div>
                        </template>
                    </VDropdown>
                </div>

                <!-- Prompt Input Section -->
                <div v-if="aiCreatePageStore.inputMode === InputMode.Prompt" class="form-group">
                    <label for="prompt-input">{{ t('page.ai.createPage.promptLabel') }}</label>
                    <textarea id="prompt-input" ref="promptTextArea" v-model="aiCreatePageStore.prompt" class="form-control prompt-textarea" :placeholder="t('page.ai.createPage.promptPlaceholder')" @input="resizeTextArea()"
                        :disabled="aiCreatePageStore.isGenerating"></textarea>
                </div>

                <!-- URL Input Section -->
                <div v-else class="form-group">
                    <label for="url-input">{{ t('page.ai.createPage.urlLabel') }}</label>
                    <input id="url-input" type="url" v-model="aiCreatePageStore.url" class="form-control url-input" :placeholder="t('page.ai.createPage.urlPlaceholder')" :disabled="aiCreatePageStore.isGenerating" />
                    <small class="url-hint">{{ t('page.ai.createPage.urlHint') }}</small>
                </div>

                <!-- Complexity Level Section -->
                <div class="form-group detail-section">
                    <label>{{ t('page.ai.createPage.complexityLabel') }}</label>

                    <!-- Desktop: Slider -->
                    <div v-if="!isMobile" class="detail-slider-container">
                        <input type="range" min="1" max="5" v-model.number="aiCreatePageStore.difficultyLevel" class="detail-slider" :disabled="aiCreatePageStore.isGenerating" />
                        <div class="detail-labels">
                            <span class="detail-label-left">{{ t('page.ai.createPage.complexity.simple') }}</span>
                            <span class="detail-label-current">{{ currentComplexityLabel }}</span>
                            <span class="detail-label-right">{{ t('page.ai.createPage.complexity.expert') }}</span>
                        </div>
                    </div>

                    <!-- Mobile: Dropdown -->
                    <VDropdown v-else :aria-id="detailDropdownAriaId" :distance="0" class="detail-dropdown">
                        <div class="detail-select">
                            <span>{{ currentComplexityLabel }}</span>
                            <font-awesome-icon :icon="['fas', 'chevron-down']" />
                        </div>

                        <template #popper="{ hide }">
                            <div class="detail-dropdown-menu detail-dropdown-popper">
                                <div
                                    class="dropdown-row"
                                    :class="{ 'active': aiCreatePageStore.difficultyLevel === DifficultyLevel.ELI5 }"
                                    @click="aiCreatePageStore.difficultyLevel = DifficultyLevel.ELI5; hide()">
                                    {{ t('page.ai.createPage.complexity.simple') }}
                                </div>
                                <div
                                    class="dropdown-row"
                                    :class="{ 'active': aiCreatePageStore.difficultyLevel === DifficultyLevel.Beginner }"
                                    @click="aiCreatePageStore.difficultyLevel = DifficultyLevel.Beginner; hide()">
                                    {{ t('page.ai.createPage.complexity.basic') }}
                                </div>
                                <div
                                    class="dropdown-row"
                                    :class="{ 'active': aiCreatePageStore.difficultyLevel === DifficultyLevel.Intermediate }"
                                    @click="aiCreatePageStore.difficultyLevel = DifficultyLevel.Intermediate; hide()">
                                    {{ t('page.ai.createPage.complexity.standard') }}
                                </div>
                                <div
                                    class="dropdown-row"
                                    :class="{ 'active': aiCreatePageStore.difficultyLevel === DifficultyLevel.Advanced }"
                                    @click="aiCreatePageStore.difficultyLevel = DifficultyLevel.Advanced; hide()">
                                    {{ t('page.ai.createPage.complexity.advanced') }}
                                </div>
                                <div
                                    class="dropdown-row"
                                    :class="{ 'active': aiCreatePageStore.difficultyLevel === DifficultyLevel.Academic }"
                                    @click="aiCreatePageStore.difficultyLevel = DifficultyLevel.Academic; hide()">
                                    {{ t('page.ai.createPage.complexity.expert') }}
                                </div>
                            </div>
                        </template>
                    </VDropdown>
                </div>

                <!-- Content Length Section -->
                <div class="form-group length-section">
                    <label>{{ t('page.ai.createPage.lengthLabel') }}</label>
                    <div class="length-toggle">
                        <button type="button" class="length-btn" :class="{ active: aiCreatePageStore.contentLength === ContentLength.Short }" @click="aiCreatePageStore.contentLength = ContentLength.Short"
                            :disabled="aiCreatePageStore.isGenerating">
                            {{ t('page.ai.createPage.length.short') }}
                        </button>
                        <button type="button" class="length-btn" :class="{ active: aiCreatePageStore.contentLength === ContentLength.Medium }" @click="aiCreatePageStore.contentLength = ContentLength.Medium"
                            :disabled="aiCreatePageStore.isGenerating">
                            {{ t('page.ai.createPage.length.medium') }}
                        </button>
                        <button type="button" class="length-btn" :class="{ active: aiCreatePageStore.contentLength === ContentLength.Long }" @click="aiCreatePageStore.contentLength = ContentLength.Long"
                            :disabled="aiCreatePageStore.isGenerating">
                            {{ t('page.ai.createPage.length.long') }}
                        </button>
                    </div>
                </div>

                <!-- Loading State -->
                <div v-if="aiCreatePageStore.isGenerating" class="generating-state">
                    <font-awesome-icon :icon="['fas', 'spinner']" spin />
                    <span>{{ shouldGenerateWikiWithSubpages ? t('page.ai.createPage.generatingWiki') : t('page.ai.createPage.generating') }}</span>
                </div>

                <!-- Error Message -->
                <div v-if="aiCreatePageStore.errorMessage" class="alert alert-danger">
                    {{ t(aiCreatePageStore.errorMessage) }}
                </div>

                <!-- Single Page Preview Section -->
                <div v-if="aiCreatePageStore.generatedContent && !shouldGenerateWikiWithSubpages" class="preview-section">
                    <div class="preview-title">
                        <span>{{ t('page.ai.createPage.preview') }}</span>
                        <button type="button" class="regenerate-btn" @click="handleGenerate" :disabled="aiCreatePageStore.isGenerating" :title="t('page.ai.createPage.button.regenerate')">
                            <font-awesome-icon :icon="['fas', 'rotate']" :spin="aiCreatePageStore.isGenerating" />
                        </button>
                    </div>
                    <div class="preview-header">
                        <strong>{{ aiCreatePageStore.generatedContent.title }}</strong>
                    </div>
                    <div class="preview-content" v-html="aiCreatePageStore.generatedContent.htmlContent"></div>
                    <div class="preview-source-info">
                        <span class="ai-badge">
                            <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" />
                            {{ t('page.ai.createPage.source.aiGenerated') }}
                        </span>
                    </div>
                </div>

                <!-- Wiki with Subpages Preview Section -->
                <div v-if="aiCreatePageStore.generatedWikiContent && shouldGenerateWikiWithSubpages" class="preview-section wiki-preview">
                    <div class="preview-title">
                        <span>{{ t('page.ai.createPage.previewWiki') }}</span>
                        <button type="button" class="regenerate-btn" @click="handleGenerate" :disabled="aiCreatePageStore.isGenerating" :title="t('page.ai.createPage.button.regenerate')">
                            <font-awesome-icon :icon="['fas', 'rotate']" :spin="aiCreatePageStore.isGenerating" />
                        </button>
                    </div>

                    <!-- Wiki Structure Navigation -->
                    <div class="wiki-structure">
                        <div class="wiki-nav-item wiki-main" :class="{ active: aiCreatePageStore.selectedSubpageIndex === null }" @click="selectWikiOverview()">
                            <font-awesome-icon :icon="['fas', 'book']" class="nav-icon" />
                            <span class="nav-title">{{ aiCreatePageStore.generatedWikiContent.title }}</span>
                            <span class="nav-badge">{{ t('page.ai.createPage.wikiMain') }}</span>
                        </div>
                        <div v-for="(subpage, index) in aiCreatePageStore.generatedWikiContent.subpages" :key="index" class="wiki-nav-item wiki-subpage" :class="{ active: aiCreatePageStore.selectedSubpageIndex === index }"
                            @click="selectSubpage(index)">
                            <font-awesome-icon :icon="['fas', 'file-alt']" class="nav-icon" />
                            <span class="nav-title">{{ subpage.title }}</span>
                        </div>
                    </div>

                    <!-- Selected Content Preview -->
                    <div v-if="currentPreviewContent" class="preview-header">
                        <strong>{{ currentPreviewContent.title }}</strong>
                    </div>
                    <div v-if="currentPreviewContent" class="preview-content" v-html="currentPreviewContent.htmlContent"></div>
                    <div class="preview-source-info">
                        <span class="ai-badge">
                            <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" />
                            {{ t('page.ai.createPage.source.aiGenerated') }}
                        </span>
                        <span class="subpage-count">
                            {{ t('page.ai.createPage.subpageCount', { count: aiCreatePageStore.generatedWikiContent.subpages.length }) }}
                        </span>
                    </div>
                </div>
            </div>
        </template>
    </LazyModal>
</template>

<style lang="less" scoped>
@import '~~/assets/shared/search.less';
@import (reference) '~~/assets/includes/imports.less';

#AiCreatePage {
    .input-mode-toggle {
        display: flex;
        gap: 8px;
        margin-bottom: 24px;

        .mode-btn {
            flex: 1;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            padding: 12px 16px;
            border: 1px solid @memo-grey-light;
            background: white;
            border-radius: 0px;
            cursor: pointer;
            font-weight: 500;
            transition: all 0.2s ease;

            &:hover {
                filter: brightness(0.95);
            }

            &:active {
                filter: brightness(0.9);
            }

            &.active {
                background: @memo-blue;
                border-color: @memo-blue;
                color: white;

                &:hover {
                    filter: brightness(0.85);
                }

                &:active {
                    filter: brightness(0.7);
                }
            }

            &:disabled {
                opacity: 0.6;
                cursor: not-allowed;
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
        border-color: @memo-grey-lighter;
        box-shadow: none;

        &:focus {
            border-color: @memo-green;
            outline: none;
        }
    }

    .url-input {
        width: 100%;
        border-radius: 24px;
        padding: 12px 16px;
        border-color: @memo-grey-lighter;
        box-shadow: none;

        &:focus {
            border-color: @memo-green;
            outline: none;
        }
    }

    .url-hint {
        display: block;
        margin-top: 8px;
        color: @memo-grey-dark;
        font-size: 12px;
    }

    .model-section {
        .model-dropdown {
            width: 100%;
        }

        .model-select {
            display: flex;
            justify-content: space-between;
            align-items: center;
            width: 100%;
            padding: 12px;
            border: 1px solid @memo-grey-lighter;
            border-radius: 0px;
            background: white;
            font-size: 14px;
            font-weight: 500;
            color: inherit;
            cursor: pointer;

            &:hover:not(.disabled) {
                border-color: @memo-blue;
            }

            &.disabled {
                opacity: 0.6;
                cursor: not-allowed;
                background: @memo-grey-lighter;
            }
        }
    }

    .detail-section {
        .detail-slider-container {
            padding: 0 8px;
        }

        .detail-slider {
            width: 100%;
            height: 8px;
            -webkit-appearance: none;
            appearance: none;
            background: linear-gradient(to right, @memo-green, @memo-yellow, @memo-wish-knowledge-red);
            border-radius: 4px;
            outline: none;
            cursor: pointer;
            user-select: none;

            &::-webkit-slider-thumb {
                -webkit-appearance: none;
                appearance: none;
                width: 20px;
                height: 20px;
                background: white;
                border: 1px solid @memo-grey-lighter;
                border-radius: 50%;
                cursor: pointer;
                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
            }

            &::-moz-range-thumb {
                width: 20px;
                height: 20px;
                background: white;
                border: 2px solid @memo-blue;
                border-radius: 50%;
                cursor: pointer;
                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
            }
        }

        .detail-labels {
            display: flex;
            justify-content: space-between;
            margin-top: 8px;
            font-size: 12px;
            color: @memo-grey-dark;

            * {
                width: 33.3333%;
            }

            .detail-label-current {
                text-align: center;
                font-weight: 600;
                color: @memo-blue;
            }

            .detail-label-right {
                text-align: right;
            }
        }

        .detail-select {
            display: flex;
            justify-content: space-between;
            align-items: center;
            width: 100%;
            padding: 12px;
            border: 1px solid @memo-grey-lighter;
            border-radius: 0px;
            background: white;
            font-size: 14px;
            font-weight: 500;
            color: inherit;
            cursor: pointer;

            &:hover {
                border-color: @memo-blue;
            }
        }
    }

    .length-section {
        .length-toggle {
            display: flex;
            gap: 8px;

            .length-btn {
                flex: 1;
                padding: 10px 16px;
                border: 1px solid @memo-grey-lighter;
                background: white;
                border-radius: 0px;
                cursor: pointer;
                font-weight: 500;
                font-size: 14px;
                transition: all 0.2s ease;

                &:hover {
                    filter: brightness(0.95);
                }

                &:active {
                    filter: brightness(0.9);
                }

                &.active {
                    background: @memo-blue;
                    border-color: @memo-blue;
                    color: white;

                    &:hover {
                        filter: brightness(0.85);
                    }

                    &:active {
                        filter: brightness(0.7);
                    }
                }

                &:disabled {
                    opacity: 0.6;
                    cursor: not-allowed;
                }
            }
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

    .preview-section {
        margin-top: 24px;
        border: 1px solid @memo-grey-lighter;
        overflow: hidden;

        .preview-title {
            display: flex;
            justify-content: space-between;
            align-items: center;
            background: @memo-grey-lighter;
            padding: 12px 16px;
            margin: 0;
            font-size: 14px;
            color: @memo-grey-dark;

            .regenerate-btn {
                display: flex;
                align-items: center;
                justify-content: center;
                width: 32px;
                height: 32px;
                padding: 0;
                border: 1px solid @memo-grey-light;
                background: white;
                border-radius: 4px;
                cursor: pointer;
                color: @memo-grey-dark;
                transition: all 0.2s ease;

                &:hover:not(:disabled) {
                    border-color: @memo-grey;
                    color: @memo-grey;
                    background: @memo-grey-lighter;
                }

                &:disabled {
                    opacity: 0.6;
                    cursor: not-allowed;
                }
            }
        }

        .preview-header {
            padding: 16px;
            border-bottom: 1px solid @memo-grey-light;
            background: @memo-grey-lighter;
        }

        .preview-content {
            padding: 16px;
            max-height: 300px;
            overflow-y: auto;
            background: white;

            :deep(h1),
            :deep(h2),
            :deep(h3),
            :deep(h4),
            :deep(h5),
            :deep(h6) {
                margin-top: 16px;
                margin-bottom: 8px;
            }

            :deep(p) {
                margin-bottom: 12px;
            }

            :deep(ul),
            :deep(ol) {
                margin-bottom: 12px;
                padding-left: 24px;
            }
        }

        .preview-source-info {
            display: flex;
            flex-wrap: wrap;
            align-items: center;
            gap: 8px;
            padding: 12px 16px;
            background: linear-gradient(135deg, #f0f7ff 0%, #e8f4f8 100%);
            border-top: 1px solid @memo-grey-light;
            font-size: 13px;
            color: @memo-grey-dark;

            .ai-badge {
                display: inline-flex;
                align-items: center;
                gap: 6px;
                padding: 4px 10px;
                background: linear-gradient(135deg, @memo-blue 0%, #4a90d9 100%);
                color: white;
                border-radius: 12px;
                font-size: 12px;
                font-weight: 500;
            }

            .subpage-count {
                margin-left: auto;
                font-size: 12px;
                color: @memo-grey-dark;
            }
        }

        // Wiki structure navigation
        .wiki-structure {
            display: flex;
            flex-direction: column;
            padding: 12px;
            border-bottom: 1px solid @memo-grey-light;
            max-height: 200px;
            overflow-y: auto;

            .wiki-nav-item {
                display: flex;
                align-items: center;
                gap: 10px;
                padding: 10px 12px;
                cursor: pointer;
                transition: all 0.2s ease;
                background: white;
                border: 0.5px solid @memo-grey-lighter;
                border-left: none;
                border-right: none;

                &:hover {
                    background: fade(@memo-blue, 10%);
                    border-color: @memo-blue;
                }

                &.active {
                    background: @memo-grey-lighter;
                    // border-color: @memo-blue;
                    // color: white;

                    .nav-badge {
                        background: @memo-grey;
                        color: white;
                    }
                }

                .nav-icon {
                    font-size: 14px;
                    color: @memo-grey-dark;
                    width: 16px;
                    text-align: center;
                }

                .nav-title {
                    flex: 1;
                    font-size: 14px;
                    font-weight: 500;
                    white-space: nowrap;
                    overflow: hidden;
                    text-overflow: ellipsis;
                }

                .nav-badge {
                    font-size: 11px;
                    padding: 2px 8px;
                    background: @memo-grey-lighter;
                    border-radius: 10px;
                    color: @memo-grey-dark;
                }

                &.wiki-subpage {
                    margin-left: 20px;
                }
            }
        }
    }
}

.modal-title {
    display: flex;
    align-items: center;
    gap: 8px;
    margin: 0;

    .header-icon {
        color: @memo-blue;
    }
}

.wiki-toggle {
    display: flex;
    align-items: center;
    margin-bottom: 16px;

    .wiki-toggle-label {
        display: flex;
        align-items: center;
        gap: 8px;
        cursor: pointer;
        font-size: 14px;
        color: @memo-grey-dark;
        user-select: none;

        &:hover {
            color: @memo-blue;
        }

        .toggle-checkbox {
            font-size: 18px;
            color: @memo-grey-dark;

            .checked {
                color: @memo-blue;
            }
        }
    }
}
</style>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.ai-model-option {
    display: flex;
    justify-content: space-between;
    align-items: center;

    .token-cost-multiplier {
        font-size: 12px;
        color: @memo-grey;
    }
}

.detail-dropdown-popper {
    width: calc(100vw - 80px);

    .dropdown-row {
        &.active {
            background: @memo-grey-lightest;
            color: @memo-grey-darker;
            font-weight: 600;
        }
    }
}

.model-dropdown-popper {
    width: 100%;
    min-width: 300px;
    max-height: 400px;
    overflow-y: auto;

    .provider-header {
        padding: 8px 12px;
        font-weight: 600;
        color: @memo-grey-dark;
        background: @memo-grey-lighter;
        font-size: 12px;
        text-transform: uppercase;
        letter-spacing: 0.5px;
    }

    .dropdown-row {
        padding: 10px 16px;
        cursor: pointer;
        transition: background 0.15s ease;

        &:hover {
            background: fade(@memo-blue, 10%);
        }

        &.active {
            background: @memo-grey-lightest;
            color: @memo-grey-darker;
            font-weight: 600;
        }
    }
}
</style>
