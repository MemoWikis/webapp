<script lang="ts" setup>
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

const detailLabels = computed(() => ({
    [DifficultyLevel.ELI5]: t('page.ai.createPage.detail.overview'),
    [DifficultyLevel.Beginner]: t('page.ai.createPage.detail.basic'),
    [DifficultyLevel.Intermediate]: t('page.ai.createPage.detail.standard'),
    [DifficultyLevel.Advanced]: t('page.ai.createPage.detail.detailed'),
    [DifficultyLevel.Academic]: t('page.ai.createPage.detail.comprehensive')
}))

const contentLengthLabels = computed(() => ({
    [ContentLength.Short]: t('page.ai.createPage.length.short'),
    [ContentLength.Medium]: t('page.ai.createPage.length.medium'),
    [ContentLength.Long]: t('page.ai.createPage.length.long')
}))

const currentDetailLabel = computed(() => {
    return detailLabels.value[aiCreatePageStore.difficultyLevel]
})

const currentContentLengthLabel = computed(() => {
    return contentLengthLabels.value[aiCreatePageStore.contentLength]
})

const canGenerate = computed(() => {
    if (aiCreatePageStore.isGenerating) return false

    if (aiCreatePageStore.inputMode === InputMode.Url) {
        return aiCreatePageStore.isValidUrl(aiCreatePageStore.url.trim())
    }
    return aiCreatePageStore.prompt.trim().length > 0
})

const canCreate = computed(() => {
    return aiCreatePageStore.generatedContent !== null && !aiCreatePageStore.isGenerating
})

async function handleGenerate() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }
    await aiCreatePageStore.generatePage()
}

async function handleCreate() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    const result = await aiCreatePageStore.createPage()
    if (result.success && result.pageId) {
        const data: SnackbarData = {
            type: 'success',
            text: { message: t('page.ai.createPage.success') },
            dismissible: true
        }
        snackbarStore.showSnackbar(data)

        // Reload the grid to show the new page
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
</script>

<template>
    <LazyModal
        :show="aiCreatePageStore.showModal"
        @close="aiCreatePageStore.closeModal()"
        :primary-btn-label="aiCreatePageStore.generatedContent ? t('page.ai.createPage.button.create') : t('page.ai.createPage.button.generate')"
        @primary-btn="aiCreatePageStore.generatedContent ? handleCreate() : handleGenerate()"
        :show-cancel-btn="true"
        :disabled="aiCreatePageStore.generatedContent ? !canCreate : !canGenerate"
        content-class="ai-create-page-modal">
        <template #header>
            <h4 class="modal-title">
                <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" class="header-icon" />
                {{ t('page.ai.createPage.title') }}
            </h4>
        </template>

        <template #footer-text>
            <div class="wiki-toggle">
                <label class="wiki-toggle-label" @click="aiCreatePageStore.createAsWiki = !aiCreatePageStore.createAsWiki">
                    <span class="toggle-checkbox">
                        <font-awesome-icon v-if="aiCreatePageStore.createAsWiki" :icon="['fas', 'square-check']" class="checked" />
                        <font-awesome-icon v-else :icon="['far', 'square']" />
                    </span>
                    <span>{{ t('page.ai.createPage.createAsWiki') }}</span>
                </label>
            </div>
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

                <!-- Prompt Input Section -->
                <div v-if="aiCreatePageStore.inputMode === InputMode.Prompt" class="form-group">
                    <label for="prompt-input">{{ t('page.ai.createPage.promptLabel') }}</label>
                    <textarea
                        id="prompt-input"
                        v-model="aiCreatePageStore.prompt"
                        class="form-control prompt-textarea"
                        :placeholder="t('page.ai.createPage.promptPlaceholder')"
                        rows="4"
                        :disabled="aiCreatePageStore.isGenerating"></textarea>
                </div>

                <!-- URL Input Section -->
                <div v-else class="form-group">
                    <label for="url-input">{{ t('page.ai.createPage.urlLabel') }}</label>
                    <input
                        id="url-input"
                        type="url"
                        v-model="aiCreatePageStore.url"
                        class="form-control url-input"
                        :placeholder="t('page.ai.createPage.urlPlaceholder')"
                        :disabled="aiCreatePageStore.isGenerating" />
                    <small class="url-hint">{{ t('page.ai.createPage.urlHint') }}</small>
                </div>

                <!-- Detail Level Section -->
                <div class="form-group detail-section">
                    <label>{{ t('page.ai.createPage.detailLabel') }}</label>

                    <!-- Desktop: Slider -->
                    <div v-if="!isMobile" class="detail-slider-container">
                        <input
                            type="range"
                            min="1"
                            max="5"
                            v-model.number="aiCreatePageStore.difficultyLevel"
                            class="detail-slider"
                            :disabled="aiCreatePageStore.isGenerating" />
                        <div class="detail-labels">
                            <span class="detail-label-left">{{ t('page.ai.createPage.detail.overview') }}</span>
                            <span class="detail-label-current">{{ currentDetailLabel }}</span>
                            <span class="detail-label-right">{{ t('page.ai.createPage.detail.comprehensive') }}</span>
                        </div>
                    </div>

                    <!-- Mobile: Dropdown -->
                    <VDropdown v-else :aria-id="detailDropdownAriaId" :distance="0" class="detail-dropdown">
                        <div class="detail-select">
                            <span>{{ currentDetailLabel }}</span>
                            <font-awesome-icon :icon="['fas', 'chevron-down']" />
                        </div>

                        <template #popper="{ hide }">
                            <div class="detail-dropdown-menu detail-dropdown-popper">
                                <div
                                    class="dropdown-row"
                                    :class="{ 'active': aiCreatePageStore.difficultyLevel === DifficultyLevel.ELI5 }"
                                    @click="aiCreatePageStore.difficultyLevel = DifficultyLevel.ELI5; hide()">
                                    {{ t('page.ai.createPage.detail.overview') }}
                                </div>
                                <div
                                    class="dropdown-row"
                                    :class="{ 'active': aiCreatePageStore.difficultyLevel === DifficultyLevel.Beginner }"
                                    @click="aiCreatePageStore.difficultyLevel = DifficultyLevel.Beginner; hide()">
                                    {{ t('page.ai.createPage.detail.basic') }}
                                </div>
                                <div
                                    class="dropdown-row"
                                    :class="{ 'active': aiCreatePageStore.difficultyLevel === DifficultyLevel.Intermediate }"
                                    @click="aiCreatePageStore.difficultyLevel = DifficultyLevel.Intermediate; hide()">
                                    {{ t('page.ai.createPage.detail.standard') }}
                                </div>
                                <div
                                    class="dropdown-row"
                                    :class="{ 'active': aiCreatePageStore.difficultyLevel === DifficultyLevel.Advanced }"
                                    @click="aiCreatePageStore.difficultyLevel = DifficultyLevel.Advanced; hide()">
                                    {{ t('page.ai.createPage.detail.detailed') }}
                                </div>
                                <div
                                    class="dropdown-row"
                                    :class="{ 'active': aiCreatePageStore.difficultyLevel === DifficultyLevel.Academic }"
                                    @click="aiCreatePageStore.difficultyLevel = DifficultyLevel.Academic; hide()">
                                    {{ t('page.ai.createPage.detail.comprehensive') }}
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
                    <span>{{ t('page.ai.createPage.generating') }}</span>
                </div>

                <!-- Error Message -->
                <div v-if="aiCreatePageStore.errorMessage" class="alert alert-danger">
                    {{ t(aiCreatePageStore.errorMessage) }}
                </div>

                <!-- Preview Section -->
                <div v-if="aiCreatePageStore.generatedContent" class="preview-section">
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
        resize: vertical;
        min-height: 100px;
        border-radius: 0px;
        padding: 12px;
        border-color: @memo-grey-lighter;

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

    .detail-section {
        .detail-slider-container {
            padding: 0 8px;
        }

        .detail-slider {
            width: 100%;
            height: 8px;
            -webkit-appearance: none;
            appearance: none;
            background: linear-gradient(to right, @memo-green, @memo-yellow, @memo-wuwi-red);
            border-radius: 4px;
            outline: none;
            cursor: pointer;

            &::-webkit-slider-thumb {
                -webkit-appearance: none;
                appearance: none;
                width: 20px;
                height: 20px;
                background: white;
                border: 2px solid @memo-blue;
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
                width: 90px;
            }

            .detail-label-current {
                text-align: center;
                font-weight: 600;
                color: @memo-blue;
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
        border: 1px solid @memo-grey-light;
        border-radius: 8px;
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
    margin-top: 12px;

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
</style>
