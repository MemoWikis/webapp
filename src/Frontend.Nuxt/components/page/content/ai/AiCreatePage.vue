<script lang="ts" setup>
import { DifficultyLevel, ContentLength, InputMode, useAiCreatePageStore } from './aiCreatePageStore'
import { useUserStore } from '~/components/user/userStore'
import type { SnackbarData } from '~/components/snackBar/snackBarStore';
import { useSnackbarStore } from '~/components/snackBar/snackBarStore'
import { usePageStore } from '../../pageStore'

const aiCreatePageStore = useAiCreatePageStore()
const userStore = useUserStore()
const snackbarStore = useSnackbarStore()
const pageStore = usePageStore()
const { t, locale } = useI18n()
const localePath = useLocalePath()
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

function formatResetDate(date: Date): string {
    return date.toLocaleDateString(locale.value, {
        weekday: 'long',
        day: 'numeric',
        month: 'long',
        year: 'numeric'
    })
}
const complexityLabels = computed(() => ({
    [DifficultyLevel.ELI5]: t('page.ai.createPage.complexity.simple'),
    [DifficultyLevel.Beginner]: t('page.ai.createPage.complexity.basic'),
    [DifficultyLevel.Intermediate]: t('page.ai.createPage.complexity.standard'),
    [DifficultyLevel.Advanced]: t('page.ai.createPage.complexity.advanced'),
    [DifficultyLevel.Academic]: t('page.ai.createPage.complexity.expert')
}))


const currentComplexityLabel = computed(() => {
    return complexityLabels.value[aiCreatePageStore.difficultyLevel]
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

// Check if quota is depleted
const isQuotaDepleted = computed(() => {
    return userStore.quotaInfo?.isQuotaDepleted ?? false
})

const showQuotaDepletedModal = ref(false)

// Load quota info when modal opens
watch(() => aiCreatePageStore.showModal, (isOpen) => {
    if (isOpen && userStore.isLoggedIn && !userStore.quotaInfo) {
        userStore.fetchQuotaInfo()
    }
})

async function handleGenerate() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    // Fetch quota info first if not available
    if (!userStore.quotaInfo) {
        await userStore.fetchQuotaInfo()
    }

    // Check if quota is depleted
    if (userStore.quotaInfo?.isQuotaDepleted) {
        showQuotaDepletedModal.value = true
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
    <LazyModal :show="aiCreatePageStore.showModal" :show-cancel-btn="false"
        :disabled="hasGeneratedContent ? !canCreate : !canGenerate" content-class="ai-create-page-modal"
        @close="aiCreatePageStore.showModal = false">
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
                    <button type="button" class="mode-btn"
                        :class="{ active: aiCreatePageStore.inputMode === InputMode.Prompt }"
                        :disabled="aiCreatePageStore.isGenerating"
                        @click="aiCreatePageStore.inputMode = InputMode.Prompt">
                        <font-awesome-icon :icon="['fas', 'pen']" />
                        {{ t('page.ai.createPage.modePrompt') }}
                    </button>
                    <button type="button" class="mode-btn"
                        :class="{ active: aiCreatePageStore.inputMode === InputMode.Url }"
                        :disabled="aiCreatePageStore.isGenerating" @click="aiCreatePageStore.inputMode = InputMode.Url">
                        <font-awesome-icon :icon="['fas', 'link']" />
                        {{ t('page.ai.createPage.modeUrl') }}
                    </button>
                </div>

                <!-- Prompt Input Section -->
                <div v-if="aiCreatePageStore.inputMode === InputMode.Prompt" class="form-group">
                    <label for="prompt-input">{{ t('page.ai.createPage.promptLabel') }}</label>
                    <textarea id="prompt-input" ref="promptTextArea" v-model="aiCreatePageStore.prompt"
                        class="form-control prompt-textarea" :placeholder="t('page.ai.createPage.promptPlaceholder')"
                        :disabled="aiCreatePageStore.isGenerating" @input="resizeTextArea()" />
                </div>

                <!-- URL Input Section -->
                <div v-else class="form-group">
                    <label for="url-input">{{ t('page.ai.createPage.urlLabel') }}</label>
                    <input id="url-input" v-model="aiCreatePageStore.url" type="url" class="form-control url-input"
                        :placeholder="t('page.ai.createPage.urlPlaceholder')"
                        :disabled="aiCreatePageStore.isGenerating" />
                    <small class="url-hint">{{ t('page.ai.createPage.urlHint') }}</small>
                </div>

                <!-- Complexity Level Section -->
                <div class="form-group detail-section">
                    <label>{{ t('page.ai.createPage.complexityLabel') }}</label>

                    <!-- Desktop: Slider -->
                    <div v-if="!isMobile" class="detail-slider-container">
                        <input v-model.number="aiCreatePageStore.difficultyLevel" type="range" min="1" max="5"
                            class="detail-slider" :disabled="aiCreatePageStore.isGenerating" />
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
                                <div class="dropdown-row"
                                    :class="{ 'active': aiCreatePageStore.difficultyLevel === DifficultyLevel.ELI5 }"
                                    @click="aiCreatePageStore.difficultyLevel = DifficultyLevel.ELI5; hide()">
                                    {{ t('page.ai.createPage.complexity.simple') }}
                                </div>
                                <div class="dropdown-row"
                                    :class="{ 'active': aiCreatePageStore.difficultyLevel === DifficultyLevel.Beginner }"
                                    @click="aiCreatePageStore.difficultyLevel = DifficultyLevel.Beginner; hide()">
                                    {{ t('page.ai.createPage.complexity.basic') }}
                                </div>
                                <div class="dropdown-row"
                                    :class="{ 'active': aiCreatePageStore.difficultyLevel === DifficultyLevel.Intermediate }"
                                    @click="aiCreatePageStore.difficultyLevel = DifficultyLevel.Intermediate; hide()">
                                    {{ t('page.ai.createPage.complexity.standard') }}
                                </div>
                                <div class="dropdown-row"
                                    :class="{ 'active': aiCreatePageStore.difficultyLevel === DifficultyLevel.Advanced }"
                                    @click="aiCreatePageStore.difficultyLevel = DifficultyLevel.Advanced; hide()">
                                    {{ t('page.ai.createPage.complexity.advanced') }}
                                </div>
                                <div class="dropdown-row"
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
                        <button type="button" class="length-btn"
                            :class="{ active: aiCreatePageStore.contentLength === ContentLength.Short }"
                            :disabled="aiCreatePageStore.isGenerating"
                            @click="aiCreatePageStore.contentLength = ContentLength.Short">
                            {{ t('page.ai.createPage.length.short') }}
                        </button>
                        <button type="button" class="length-btn"
                            :class="{ active: aiCreatePageStore.contentLength === ContentLength.Medium }"
                            :disabled="aiCreatePageStore.isGenerating"
                            @click="aiCreatePageStore.contentLength = ContentLength.Medium">
                            {{ t('page.ai.createPage.length.medium') }}
                        </button>
                        <button type="button" class="length-btn"
                            :class="{ active: aiCreatePageStore.contentLength === ContentLength.Long }"
                            :disabled="aiCreatePageStore.isGenerating"
                            @click="aiCreatePageStore.contentLength = ContentLength.Long">
                            {{ t('page.ai.createPage.length.long') }}
                        </button>
                    </div>
                </div>

                <!-- Loading State -->
                <div v-if="aiCreatePageStore.isGenerating" class="generating-state">
                    <font-awesome-icon :icon="['fas', 'spinner']" spin />
                    <span>{{ shouldGenerateWikiWithSubpages ? t('page.ai.createPage.generatingWiki') :
                        t('page.ai.createPage.generating') }}</span>
                </div>

                <!-- Error Message -->
                <div v-if="aiCreatePageStore.errorMessage" class="alert alert-danger">
                    {{ t(aiCreatePageStore.errorMessage) }}
                </div>

                <!-- Single Page Preview Section -->
                <div v-if="aiCreatePageStore.generatedContent && !shouldGenerateWikiWithSubpages"
                    class="preview-section">
                    <div class="preview-title">
                        <span>{{ t('page.ai.createPage.preview') }}</span>
                        <button type="button" class="regenerate-btn" :disabled="aiCreatePageStore.isGenerating"
                            :title="t('page.ai.createPage.button.regenerate')" @click="handleGenerate">
                            <font-awesome-icon :icon="['fas', 'rotate']" :spin="aiCreatePageStore.isGenerating" />
                        </button>
                    </div>
                    <div class="preview-header">
                        <strong>{{ aiCreatePageStore.generatedContent.title }}</strong>
                    </div>
                    <div class="preview-content" v-html="aiCreatePageStore.generatedContent.htmlContent" />
                    <div class="preview-source-info">
                        <span class="ai-badge">
                            <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" />
                            {{ t('page.ai.createPage.source.aiGenerated') }}
                        </span>
                    </div>
                </div>

                <!-- Wiki with Subpages Preview Section -->
                <div v-if="aiCreatePageStore.generatedWikiContent && shouldGenerateWikiWithSubpages"
                    class="preview-section wiki-preview">
                    <div class="preview-title">
                        <span>{{ t('page.ai.createPage.previewWiki') }}</span>
                        <button type="button" class="regenerate-btn" :disabled="aiCreatePageStore.isGenerating"
                            :title="t('page.ai.createPage.button.regenerate')" @click="handleGenerate">
                            <font-awesome-icon :icon="['fas', 'rotate']" :spin="aiCreatePageStore.isGenerating" />
                        </button>
                    </div>

                    <!-- Wiki Structure Navigation -->
                    <div class="wiki-structure">
                        <div class="wiki-nav-item wiki-main"
                            :class="{ active: aiCreatePageStore.selectedSubpageIndex === null }"
                            @click="selectWikiOverview()">
                            <font-awesome-icon :icon="['fas', 'book']" class="nav-icon" />
                            <span class="nav-title">{{ aiCreatePageStore.generatedWikiContent.title }}</span>
                            <span class="nav-badge">{{ t('page.ai.createPage.wikiMain') }}</span>
                        </div>
                        <div v-for="(subpage, index) in aiCreatePageStore.generatedWikiContent.subpages" :key="index"
                            class="wiki-nav-item wiki-subpage"
                            :class="{ active: aiCreatePageStore.selectedSubpageIndex === index }"
                            @click="selectSubpage(index)">
                            <font-awesome-icon :icon="['fas', 'file-alt']" class="nav-icon" />
                            <span class="nav-title">{{ subpage.title }}</span>
                        </div>
                    </div>

                    <!-- Selected Content Preview -->
                    <div v-if="currentPreviewContent" class="preview-header">
                        <strong>{{ currentPreviewContent.title }}</strong>
                    </div>
                    <div v-if="currentPreviewContent" class="preview-content"
                        v-html="currentPreviewContent.htmlContent" />
                    <div class="preview-source-info">
                        <span class="ai-badge">
                            <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" />
                            {{ t('page.ai.createPage.source.aiGenerated') }}
                        </span>
                        <span class="subpage-count">
                            {{ t('page.ai.createPage.subpageCount', {
                                count:
                                    aiCreatePageStore.generatedWikiContent.subpages.length
                            })
                            }}
                        </span>
                    </div>
                </div>
            </div>
        </template>

        <template #footer>
            <div class="ai-create-page-footer">
                <!-- AI Model Selection -->
                <div class="ai-model-selection">
                    <VDropdown :distance="2" class="model-dropdown" placement="top-start">
                        <div class="model-select"
                            :class="{ disabled: aiCreatePageStore.isGenerating || aiCreatePageStore.isLoadingModels }">
                            <span v-if="aiCreatePageStore.isLoadingModels">{{ t('page.ai.createPage.loadingModels')
                            }}</span>
                            <span v-else>{{ selectedModelDisplayName || t('page.ai.createPage.selectModel') }}</span>
                            <font-awesome-icon :icon="['fas', 'chevron-down']" />
                        </div>

                        <template #popper="{ hide }">
                            <div class="model-dropdown-menu model-dropdown-popper">
                                <template v-for="provider in groupedModels" :key="provider.name">
                                    <div class="provider-header">{{ provider.name }}</div>
                                    <div v-for="model in provider.models" :key="model.modelId"
                                        class="dropdown-row ai-model-option"
                                        :class="{ active: aiCreatePageStore.selectedModelId === model.modelId }"
                                        @click="aiCreatePageStore.selectedModelId = model.modelId; hide()">
                                        <span>{{ model.displayName }}</span> <span class="token-cost-multiplier">{{
                                            model.tokenCostMultiplier }}x</span>
                                    </div>
                                </template>
                            </div>
                        </template>
                    </VDropdown>

                    <VDropdown :distance="2" placement="top" class="token-balance-dropdown">
                        <div class="token-balance-btn" :title="t('page.ai.createPage.tokenBalance')"
                            :class="{ 'quota-low': (userStore.quotaInfo?.percentageUsed ?? 0) > 80, 'quota-depleted': userStore.quotaInfo?.isQuotaDepleted }"
                            @click="userStore.fetchQuotaInfo()">
                            <font-awesome-icon :icon="['fas', 'chart-pie']" />
                        </div>

                        <template #popper>
                            <div class="token-balance-popper">
                                <div class="token-balance-header">{{ t('page.ai.createPage.quota.title') }}</div>

                                <div v-if="userStore.isLoadingQuotaInfo" class="loading-state">
                                    <font-awesome-icon icon="fa-solid fa-spinner" spin />
                                </div>

                                <template v-else-if="userStore.quotaInfo">
                                    <!-- Progress Bar -->
                                    <div class="quota-progress-container">
                                        <div class="quota-progress-bar">
                                            <div class="quota-progress-fill" :class="{
                                                'low': userStore.quotaInfo.percentageUsed > 80,
                                                'depleted': userStore.quotaInfo.isQuotaDepleted
                                            }" :style="{ width: `${userStore.quotaInfo.percentageUsed}%` }" />
                                        </div>
                                        <div class="quota-values">
                                            <span class="quota-remaining">
                                                {{ userStore.quotaInfo.totalBalance.toLocaleString() }}
                                            </span>
                                            <span class="quota-separator">/</span>
                                            <span class="quota-total">
                                                {{ userStore.quotaInfo.weeklyLimit.toLocaleString() }}
                                            </span>
                                        </div>
                                    </div>

                                    <!-- Reset Date -->
                                    <div v-if="userStore.quotaInfo.hasActiveSubscription && userStore.quotaInfo.nextResetDate"
                                        class="quota-reset">
                                        <font-awesome-icon :icon="['fas', 'calendar-alt']" class="reset-icon" />
                                        <span>{{ t('page.ai.createPage.quota.resetDate') }}: {{
                                            formatResetDate(userStore.quotaInfo.nextResetDate) }}</span>
                                    </div>

                                    <!-- Depleted Warning -->
                                    <div v-if="userStore.quotaInfo.isQuotaDepleted" class="quota-depleted-warning">
                                        <font-awesome-icon :icon="['fas', 'exclamation-triangle']" />
                                        <span>{{ t('page.ai.createPage.quota.depleted') }}</span>
                                    </div>

                                    <!-- Link to Settings -->
                                    <NuxtLink :to="localePath('/Einstellungen?tab=ai-usage')"
                                        class="quota-settings-link">
                                        <font-awesome-icon :icon="['fas', 'cog']" />
                                        {{ t('page.ai.createPage.quota.settingsLink') }}
                                    </NuxtLink>
                                </template>

                                <template v-else>
                                    <div class="token-balance-value">—</div>
                                </template>
                            </div>
                        </template>
                    </VDropdown>
                </div>

                <!-- Quota Warning - Only when low or depleted -->
                <NuxtLink v-if="userStore.quotaInfo?.isQuotaDepleted" :to="localePath('/Einstellungen?tab=ai-usage')"
                    class="quota-warning-indicator depleted">
                    <font-awesome-icon :icon="['fas', 'exclamation-circle']" />
                    <span>{{ t('page.ai.createPage.quotaWarning.depleted') }}</span>
                </NuxtLink>
                <NuxtLink v-else-if="userStore.quotaInfo && userStore.quotaInfo.percentageUsed > 80"
                    :to="localePath('/Einstellungen?tab=ai-usage')" class="quota-warning-indicator low">
                    <font-awesome-icon :icon="['fas', 'exclamation-triangle']" />
                    <span>{{ t('page.ai.createPage.quotaWarning.low', {
                        percent: (100 -
                            userStore.quotaInfo.percentageUsed).toFixed(0)
                    }) }}</span>
                </NuxtLink>

                <div class="buttons">
                    <div class="wiki-toggle">
                        <label class="wiki-toggle-label"
                            @click="aiCreatePageStore.createAsWiki = !aiCreatePageStore.createAsWiki">
                            <span class="toggle-checkbox">
                                <font-awesome-icon v-if="aiCreatePageStore.createAsWiki" :icon="['fas', 'square-check']"
                                    class="checked" />
                                <font-awesome-icon v-else :icon="['far', 'square']" />
                            </span>
                            <span>{{ t('page.ai.createPage.createAsWiki') }}</span>
                        </label>
                    </div>
                    <div class="memo-button btn btn-primary" role="button" :class="{ 'disabled': isQuotaDepleted }"
                        @click="hasGeneratedContent ? handleCreate() : handleGenerate()">{{ primaryButtonLabel }}</div>
                </div>
            </div>
        </template>
    </LazyModal>

    <!-- Quota Depleted Modal -->
    <Teleport to="body">
        <Transition name="modal-fade">
            <div v-if="showQuotaDepletedModal" class="quota-depleted-overlay"
                @click.self="showQuotaDepletedModal = false">
                <div class="quota-depleted-modal">
                    <div class="modal-icon">
                        <font-awesome-icon :icon="['fas', 'hourglass-half']" />
                    </div>
                    <h3 class="modal-title">{{ t('page.ai.createPage.quotaDepleted.title') }}</h3>
                    <p class="modal-message">{{ t('page.ai.createPage.quotaDepleted.message') }}</p>

                    <div v-if="userStore.quotaInfo?.nextResetDate" class="reset-info">
                        <font-awesome-icon :icon="['fas', 'calendar-check']" />
                        <span>{{ t('page.ai.createPage.quotaDepleted.resetInfo', {
                            date:
                                formatResetDate(userStore.quotaInfo.nextResetDate)
                        }) }}</span>
                    </div>

                    <div class="modal-actions">
                        <NuxtLink :to="localePath('/Einstellungen?tab=ai-usage')" class="btn btn-primary settings-btn">
                            <font-awesome-icon :icon="['fas', 'chart-pie']" />
                            {{ t('page.ai.createPage.quotaDepleted.viewUsage') }}
                        </NuxtLink>
                        <button class="btn btn-secondary" @click="showQuotaDepletedModal = false">
                            {{ t('page.ai.createPage.quotaDepleted.close') }}
                        </button>
                    </div>
                </div>
            </div>
        </Transition>
    </Teleport>
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
            border: 1px solid @memo-grey-lighter;
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
                border-color: @memo-grey-light;
                font-weight: 600;
                color: @memo-blue;

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
        .model-row {
            display: flex;
            gap: 8px;
            align-items: stretch;
        }

        .model-dropdown {
            flex: 1;
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
                filter: brightness(0.95);
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
                    border-color: @memo-grey-light;
                    font-weight: 600;
                    color: @memo-blue;

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

    .wiki-toggle-label {
        display: flex;
        align-items: center;
        gap: 8px;
        cursor: pointer;
        font-size: 14px;
        color: @memo-grey-dark;
        user-select: none;
        margin-bottom: 0px;

        &:hover {
            color: @memo-blue;
        }

        .toggle-checkbox {
            font-size: 18px;
            color: @memo-grey-dark;

            .checked {
                color: @memo-blue-link;
            }
        }
    }
}

.ai-create-page-footer {
    display: flex;
    justify-content: space-between;
    align-items: center;

    .ai-model-selection {
        display: flex;
        align-items: center;
        flex-direction: row;

        color: @memo-grey-light;

        * {
            color: @memo-grey-dark;
            cursor: pointer;
            user-select: none;

            &:hover {
                filter: brightness(0.9);
            }
        }

        .model-select {
            display: flex;
            gap: 8px;
            align-items: center;
        }

        .token-balance-btn {
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 0 8px;
            cursor: pointer;
            color: @memo-grey-dark;
            transition: all 0.2s ease;
            height: 46px;
            width: 46px;

            &:hover {
                color: @memo-blue;
            }

            &.quota-low {
                color: @memo-yellow;
            }

            &.quota-depleted {
                color: #B13A48;
            }
        }
    }

    .quota-warning-indicator {
        display: flex;
        align-items: center;
        gap: 8px;
        padding: 8px 14px;
        border-radius: 6px;
        font-size: 13px;
        font-weight: 500;
        text-decoration: none;
        transition: all 0.2s ease;

        &.low {
            background: fade(@memo-yellow, 15%);
            color: darken(@memo-yellow, 25%);
            border: 1px solid fade(@memo-yellow, 40%);

            &:hover {
                background: fade(@memo-yellow, 25%);
            }

            svg {
                color: @memo-yellow;
            }
        }

        &.depleted {
            background: fade(#B13A48, 10%);
            color: #B13A48;
            border: 1px solid fade(#B13A48, 30%);

            &:hover {
                background: fade(#B13A48, 18%);
            }

            svg {
                color: #B13A48;
            }
        }
    }

    .buttons {
        display: flex;
        gap: 12px;
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
            color: @memo-blue-link;
            font-weight: 600;
        }
    }
}

.model-dropdown-popper {
    width: 100%;
    min-width: 300px;
    max-height: 400px;
    overflow-y: auto;

    margin-top: -12px;
    margin-bottom: -12px;

    .provider-header {
        padding: 12px;
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

.token-balance-popper {
    padding: 12px 16px;
    min-width: 200px;

    .token-balance-header {
        font-size: 12px;
        font-weight: 600;
        color: @memo-grey-dark;
        margin-bottom: 8px;
    }

    .token-balance-value {
        font-size: 18px;
        font-weight: 600;
        color: @memo-blue;
    }

    .loading-state {
        text-align: center;
        padding: 8px;
        color: @memo-grey-dark;
    }

    .quota-progress-container {
        margin-bottom: 12px;

        .quota-progress-bar {
            height: 8px;
            background: @memo-grey-lighter;
            border-radius: 4px;
            overflow: hidden;
            margin-bottom: 4px;

            .quota-progress-fill {
                height: 100%;
                background: linear-gradient(90deg, @memo-green, @memo-green);
                border-radius: 4px;
                transition: width 0.3s ease;

                &.low {
                    background: @memo-yellow;
                }

                &.depleted {
                    background: #B13A48;
                    width: 0 !important;
                }
            }
        }

        .quota-values {
            display: flex;
            align-items: baseline;
            font-size: 14px;

            .quota-remaining {
                font-weight: 600;
                color: @memo-blue;
                font-size: 16px;
            }

            .quota-separator {
                margin: 0 4px;
                color: @memo-grey-dark;
            }

            .quota-total {
                color: @memo-grey-dark;
            }
        }
    }

    .quota-reset {
        display: flex;
        align-items: center;
        gap: 6px;
        font-size: 12px;
        color: @memo-grey-dark;
        margin-bottom: 8px;

        .reset-icon {
            color: @memo-blue;
        }
    }

    .quota-depleted-warning {
        display: flex;
        align-items: center;
        gap: 6px;
        padding: 8px;
        background: fade(#B13A48, 10%);
        border-radius: 4px;
        color: #B13A48;
        font-size: 12px;
        font-weight: 500;
        margin-bottom: 8px;
    }

    .quota-settings-link {
        display: flex;
        align-items: center;
        gap: 6px;
        font-size: 12px;
        color: @memo-blue-link;
        text-decoration: none;
        padding-top: 8px;
        border-top: 1px solid @memo-grey-lighter;
        margin-top: 4px;

        &:hover {
            text-decoration: underline;
        }
    }
}

// Quota Depleted Modal Styles
.quota-depleted-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 10000;
    padding: 20px;
}

.quota-depleted-modal {
    background: white;
    border-radius: 16px;
    padding: 32px;
    max-width: 420px;
    width: 100%;
    text-align: center;
    box-shadow: 0 20px 60px rgba(0, 0, 0, 0.2);

    .modal-icon {
        width: 64px;
        height: 64px;
        background: linear-gradient(135deg, @memo-yellow 0%, darken(@memo-yellow, 15%) 100%);
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        margin: 0 auto 20px;

        svg {
            font-size: 28px;
            color: white;
        }
    }

    .modal-title {
        font-size: 20px;
        font-weight: 700;
        color: @memo-grey-darker;
        margin: 0 0 12px;
    }

    .modal-message {
        font-size: 14px;
        color: @memo-grey-dark;
        line-height: 1.6;
        margin: 0 0 20px;
    }

    .reset-info {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 8px;
        padding: 12px 16px;
        background: fade(@memo-green, 10%);
        border-radius: 8px;
        color: darken(@memo-green, 20%);
        font-size: 13px;
        font-weight: 500;
        margin-bottom: 24px;

        svg {
            color: @memo-green;
        }
    }

    .modal-actions {
        display: flex;
        flex-direction: column;
        gap: 10px;

        .settings-btn {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            text-decoration: none;
        }

        .btn {
            padding: 12px 20px;
            border-radius: 24px;
            font-weight: 500;
        }

        .btn-secondary {
            background: @memo-grey-lighter;
            color: @memo-grey-dark;
            border: none;

            &:hover {
                background: darken(@memo-grey-lighter, 5%);
            }
        }
    }
}

// Modal transition
.modal-fade-enter-active,
.modal-fade-leave-active {
    transition: opacity 0.2s ease;

    .quota-depleted-modal {
        transition: transform 0.2s ease;
    }
}

.modal-fade-enter-from,
.modal-fade-leave-to {
    opacity: 0;

    .quota-depleted-modal {
        transform: scale(0.95);
    }
}
</style>
