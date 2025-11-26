<script lang="ts" setup>
import { DifficultyLevel, useAiCreatePageStore } from './aiCreatePageStore'
import { useUserStore } from '~/components/user/userStore'
import { SnackbarData, useSnackbarStore } from '~/components/snackBar/snackBarStore'
import { usePageStore } from '../../pageStore'

const aiCreatePageStore = useAiCreatePageStore()
const userStore = useUserStore()
const snackbarStore = useSnackbarStore()
const pageStore = usePageStore()
const { t } = useI18n()

const difficultyLabels = computed(() => ({
    [DifficultyLevel.ELI5]: t('page.ai.createPage.difficulty.eli5'),
    [DifficultyLevel.Beginner]: t('page.ai.createPage.difficulty.beginner'),
    [DifficultyLevel.Intermediate]: t('page.ai.createPage.difficulty.intermediate'),
    [DifficultyLevel.Advanced]: t('page.ai.createPage.difficulty.advanced'),
    [DifficultyLevel.Academic]: t('page.ai.createPage.difficulty.academic')
}))

const currentDifficultyLabel = computed(() => {
    return difficultyLabels.value[aiCreatePageStore.difficultyLevel]
})

const canGenerate = computed(() => {
    return aiCreatePageStore.prompt.trim().length > 0 && !aiCreatePageStore.isGenerating
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

        <template #body>
            <div id="AiCreatePage">
                <!-- Prompt Input Section -->
                <div class="form-group">
                    <label for="prompt-input">{{ t('page.ai.createPage.promptLabel') }}</label>
                    <textarea
                        id="prompt-input"
                        v-model="aiCreatePageStore.prompt"
                        class="form-control prompt-textarea"
                        :placeholder="t('page.ai.createPage.promptPlaceholder')"
                        rows="4"
                        :disabled="aiCreatePageStore.isGenerating"></textarea>
                </div>

                <!-- Difficulty Slider Section -->
                <div class="form-group difficulty-section">
                    <label>{{ t('page.ai.createPage.difficultyLabel') }}</label>
                    <div class="difficulty-slider-container">
                        <input
                            type="range"
                            min="1"
                            max="5"
                            v-model.number="aiCreatePageStore.difficultyLevel"
                            class="difficulty-slider"
                            :disabled="aiCreatePageStore.isGenerating" />
                        <div class="difficulty-labels">
                            <span class="difficulty-label-left">{{ t('page.ai.createPage.difficulty.eli5') }}</span>
                            <span class="difficulty-label-current">{{ currentDifficultyLabel }}</span>
                            <span class="difficulty-label-right">{{ t('page.ai.createPage.difficulty.academic') }}</span>
                        </div>
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
                    <h5 class="preview-title">{{ t('page.ai.createPage.preview') }}</h5>
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

        &:focus {
            border-color: @memo-blue;
            outline: none;
        }
    }

    .difficulty-section {
        .difficulty-slider-container {
            padding: 0 8px;
        }

        .difficulty-slider {
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

        .difficulty-labels {
            display: flex;
            justify-content: space-between;
            margin-top: 8px;
            font-size: 12px;
            color: @memo-grey-dark;

            .difficulty-label-current {
                font-weight: 600;
                color: @memo-blue;
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
            background: @memo-grey-lighter;
            padding: 12px 16px;
            margin: 0;
            font-size: 14px;
            color: @memo-grey-dark;
        }

        .preview-header {
            padding: 16px;
            border-bottom: 1px solid @memo-grey-light;
            background: @memo-grey-lightest;
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
</style>
