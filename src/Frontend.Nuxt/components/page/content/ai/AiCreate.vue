<script lang="ts" setup>
import { DifficultyLevel, ContentLength, ContentType, InputMode, useAiCreateStore } from './aiCreateStore'
import { useUserStore } from '~/components/user/userStore'
import { useSnackbarStore } from '~/components/snackBar/snackBarStore'
import { usePageStore } from '../../pageStore'
import DOMPurify from 'isomorphic-dompurify'

const aiCreateStore = useAiCreateStore()
const userStore = useUserStore()
const snackbarStore = useSnackbarStore()
const pageStore = usePageStore()
const { $urlHelper } = useNuxtApp()

function sanitizeHtml(html: string): string {
    return DOMPurify.sanitize(html)
}
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
    return complexityLabels.value[aiCreateStore.difficultyLevel]
})

// Group models by provider for the dropdown
const groupedModels = computed(() => {
    const groups: { name: string; models: typeof aiCreateStore.availableModels }[] = []
    const providers = new Set(aiCreateStore.availableModels.map(model => model.provider))

    for (const provider of providers) {
        groups.push({
            name: provider,
            models: aiCreateStore.availableModels.filter(model => model.provider === provider)
        })
    }

    return groups
})


const selectedModelDisplayName = computed(() => {
    const model = aiCreateStore.availableModels.find(model => model.modelId === aiCreateStore.selectedModelId)
    return model?.displayName ?? ''
})

// Wiki with subpages is generated when: Wiki tab is selected AND content length is Long
const shouldGenerateWikiWithSubpages = computed(() => {
    return aiCreateStore.contentType === ContentType.Wiki && aiCreateStore.contentLength === ContentLength.Long
})

const promptLabel = computed(() => {
    if (aiCreateStore.contentType === ContentType.Wiki) {
        return t('page.ai.createPage.promptLabelWiki')
    }
    return t('page.ai.createPage.promptLabel')
})

const contentLengthLabels = computed(() => ({
    [ContentLength.Short]: t('page.ai.createPage.length.short'),
    [ContentLength.Medium]: t('page.ai.createPage.length.medium'),
    [ContentLength.Long]: t('page.ai.createPage.length.long')
}))

const currentContentLengthLabel = computed(() => {
    return contentLengthLabels.value[aiCreateStore.contentLength]
})

const showUrlInput = ref(false)

function sliderBackground(value: number, min: number, max: number): string {
    const percentage = ((value - min) / (max - min)) * 100
    return `linear-gradient(to right, #101010 0%, #101010 ${percentage}%, #EFEFEF ${percentage}%, #EFEFEF 100%)`
}

const complexitySliderStyle = computed(() => ({
    background: sliderBackground(aiCreateStore.difficultyLevel, 1, 5)
}))

const contentLengthSliderStyle = computed(() => ({
    background: sliderBackground(aiCreateStore.contentLength, 1, 3)
}))

const canGenerate = computed(() => {
    if (aiCreateStore.isGenerating) return false

    if (showUrlInput.value && aiCreateStore.url.trim().length > 0) {
        return aiCreateStore.isValidUrl(aiCreateStore.url.trim())
    }
    return aiCreateStore.prompt.trim().length > 0
})

const hasGeneratedContent = computed(() => {
    if (shouldGenerateWikiWithSubpages.value) {
        return aiCreateStore.generatedWikiContent !== null
    }
    return aiCreateStore.generatedContent !== null
})

const canCreate = computed(() => {
    return hasGeneratedContent.value && !aiCreateStore.isGenerating
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
    if (shouldGenerateWikiWithSubpages.value && aiCreateStore.generatedWikiContent) {
        if (aiCreateStore.selectedSubpageIndex !== null && aiCreateStore.generatedWikiContent.subpages[aiCreateStore.selectedSubpageIndex]) {
            return aiCreateStore.generatedWikiContent.subpages[aiCreateStore.selectedSubpageIndex]
        }
        return {
            title: aiCreateStore.generatedWikiContent.title,
            htmlContent: aiCreateStore.generatedWikiContent.htmlContent
        }
    }
    return aiCreateStore.generatedContent
})

// Check if quota is depleted
const isQuotaDepleted = computed(() => {
    return userStore.quotaInfo?.isQuotaDepleted ?? false
})

const showQuotaDepletedModal = ref(false)

// Load quota info when modal opens
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

    // Fetch quota info first if not available
    if (!userStore.quotaInfo) {
        await userStore.fetchQuotaInfo()
    }

    // Check if quota is depleted
    if (userStore.quotaInfo?.isQuotaDepleted) {
        showQuotaDepletedModal.value = true
        return
    }

    // Set input mode based on URL field state
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

function selectWikiOverview() {
    aiCreateStore.selectedSubpageIndex = null
}

function selectSubpage(index: number) {
    aiCreateStore.selectedSubpageIndex = index
}
</script>

<template>
    <LazyModal :show="aiCreateStore.showModal" :show-cancel-btn="false"
        :disabled="hasGeneratedContent ? !canCreate : !canGenerate" content-class="ai-create-modal"
        @close="aiCreateStore.showModal = false">
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
                <!-- Content Type Tabs -->
                <div class="content-type-tabs">
                    <button type="button" class="content-tab"
                        :class="{ active: aiCreateStore.contentType === ContentType.Page }"
                        :disabled="aiCreateStore.isGenerating" @click="aiCreateStore.contentType = ContentType.Page">
                        <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" class="tab-icon" />
                        <span class="tab-label">{{ t('page.ai.createPage.tab.page') }}</span>
                        <span class="tab-subtitle">{{ t('page.ai.createPage.tab.pageSubtitle') }}</span>
                    </button>
                    <button type="button" class="content-tab"
                        :class="{ active: aiCreateStore.contentType === ContentType.Wiki }"
                        :disabled="aiCreateStore.isGenerating" @click="aiCreateStore.contentType = ContentType.Wiki">
                        <font-awesome-icon :icon="['fas', 'file-lines']" class="tab-icon" />
                        <span class="tab-label">{{ t('page.ai.createPage.tab.wiki') }}</span>
                        <span class="tab-subtitle">{{ t('page.ai.createPage.tab.wikiSubtitle') }}</span>
                    </button>
                    <button type="button" class="content-tab"
                        :class="{ active: aiCreateStore.contentType === ContentType.Flashcards }"
                        :disabled="aiCreateStore.isGenerating"
                        @click="aiCreateStore.contentType = ContentType.Flashcards">
                        <font-awesome-icon :icon="['fas', 'book-open']" class="tab-icon" />
                        <span class="tab-label">{{ t('page.ai.createPage.tab.flashcards') }}</span>
                        <span class="tab-subtitle">{{ t('page.ai.createPage.tab.flashcardsSubtitle') }}</span>
                    </button>
                </div>

                <!-- Prompt Input Section -->
                <div class="form-group">
                    <label for="prompt-input">{{ promptLabel }}</label>
                    <textarea id="prompt-input" ref="promptTextArea" v-model="aiCreateStore.prompt"
                        class="form-control prompt-textarea" :placeholder="t('page.ai.createPage.promptPlaceholder')"
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
                        :placeholder="t('page.ai.createPage.urlPlaceholder')" :disabled="aiCreateStore.isGenerating" />
                    <small class="url-hint">{{ t('page.ai.createPage.urlHint') }}</small>
                </div>

                <!-- Complexity Level Section -->
                <div class="form-group detail-section">
                    <label>{{ t('page.ai.createPage.complexityLabel') }}</label>

                    <!-- Desktop: Slider -->
                    <div v-if="!isMobile" class="detail-slider-container">
                        <input v-model.number="aiCreateStore.difficultyLevel" type="range" min="1" max="5"
                            class="detail-slider" :style="complexitySliderStyle" :disabled="aiCreateStore.isGenerating"
                            :aria-label="t('page.ai.createPage.complexityLabel')"
                            :aria-valuetext="currentComplexityLabel" />
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
                                <div v-for="(label, level) in complexityLabels" :key="level" class="dropdown-row"
                                    :class="{ 'active': aiCreateStore.difficultyLevel === Number(level) }"
                                    @click="aiCreateStore.difficultyLevel = Number(level); hide()">
                                    {{ label }}
                                </div>
                            </div>
                        </template>
                    </VDropdown>
                </div>

                <!-- Content Length Section -->
                <div class="form-group detail-section">
                    <label>{{ t('page.ai.createPage.lengthLabel') }}</label>

                    <!-- Desktop: Slider -->
                    <div v-if="!isMobile" class="detail-slider-container">
                        <input v-model.number="aiCreateStore.contentLength" type="range" min="1" max="3"
                            class="detail-slider" :style="contentLengthSliderStyle"
                            :disabled="aiCreateStore.isGenerating" :aria-label="t('page.ai.createPage.lengthLabel')"
                            :aria-valuetext="currentContentLengthLabel" />
                        <div class="detail-labels">
                            <span class="detail-label-left">{{ t('page.ai.createPage.length.short') }}</span>
                            <span class="detail-label-current">{{ currentContentLengthLabel }}</span>
                            <span class="detail-label-right">{{ t('page.ai.createPage.length.long') }}</span>
                        </div>
                    </div>

                    <!-- Mobile: Dropdown -->
                    <VDropdown v-else :distance="0" class="detail-dropdown">
                        <div class="detail-select">
                            <span>{{ currentContentLengthLabel }}</span>
                            <font-awesome-icon :icon="['fas', 'chevron-down']" />
                        </div>

                        <template #popper="{ hide }">
                            <div class="detail-dropdown-menu detail-dropdown-popper">
                                <div v-for="(label, level) in contentLengthLabels" :key="level" class="dropdown-row"
                                    :class="{ 'active': aiCreateStore.contentLength === Number(level) }"
                                    @click="aiCreateStore.contentLength = Number(level); hide()">
                                    {{ label }}
                                </div>
                            </div>
                        </template>
                    </VDropdown>
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

                <!-- Single Page Preview Section -->
                <div v-if="aiCreateStore.generatedContent && !shouldGenerateWikiWithSubpages" class="preview-section">
                    <div class="preview-title">
                        <span>{{ t('page.ai.createPage.preview') }}</span>
                        <button type="button" class="regenerate-btn" :disabled="aiCreateStore.isGenerating"
                            :title="t('page.ai.createPage.button.regenerate')" @click="handleGenerate">
                            <font-awesome-icon :icon="['fas', 'rotate']" :spin="aiCreateStore.isGenerating" />
                        </button>
                    </div>
                    <div class="preview-header">
                        <strong>{{ aiCreateStore.generatedContent.title }}</strong>
                    </div>
                    <div class="preview-content" v-html="sanitizeHtml(aiCreateStore.generatedContent.htmlContent)" />
                    <div class="preview-source-info">
                        <span class="ai-badge">
                            <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" />
                            {{ t('page.ai.createPage.source.aiGenerated') }}
                        </span>
                    </div>
                </div>

                <!-- Wiki with Subpages Preview Section -->
                <div v-if="aiCreateStore.generatedWikiContent && shouldGenerateWikiWithSubpages"
                    class="preview-section wiki-preview">
                    <div class="preview-title">
                        <span>{{ t('page.ai.createPage.previewWiki') }}</span>
                        <button type="button" class="regenerate-btn" :disabled="aiCreateStore.isGenerating"
                            :title="t('page.ai.createPage.button.regenerate')" @click="handleGenerate">
                            <font-awesome-icon :icon="['fas', 'rotate']" :spin="aiCreateStore.isGenerating" />
                        </button>
                    </div>

                    <!-- Wiki Structure Navigation -->
                    <div class="wiki-structure">
                        <div class="wiki-nav-item wiki-main"
                            :class="{ active: aiCreateStore.selectedSubpageIndex === null }"
                            @click="selectWikiOverview()">
                            <font-awesome-icon :icon="['fas', 'book']" class="nav-icon" />
                            <span class="nav-title">{{ aiCreateStore.generatedWikiContent.title }}</span>
                            <span class="nav-badge">{{ t('page.ai.createPage.wikiMain') }}</span>
                        </div>
                        <div v-for="(subpage, index) in aiCreateStore.generatedWikiContent.subpages" :key="index"
                            class="wiki-nav-item wiki-subpage"
                            :class="{ active: aiCreateStore.selectedSubpageIndex === index }"
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
                        v-html="sanitizeHtml(currentPreviewContent.htmlContent)" />
                    <div class="preview-source-info">
                        <span class="ai-badge">
                            <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" />
                            {{ t('page.ai.createPage.source.aiGenerated') }}
                        </span>
                        <span class="subpage-count">
                            {{ t('page.ai.createPage.subpageCount', {
                                count:
                                    aiCreateStore.generatedWikiContent.subpages.length
                            })
                            }}
                        </span>
                    </div>
                </div>
            </div>
        </template>

        <template #footer>
            <div class="ai-create-footer">
                <!-- AI Model Selection -->
                <div class="ai-model-selection">
                    <VDropdown :distance="2" class="model-dropdown" placement="top-start">
                        <div class="model-select"
                            :class="{ disabled: aiCreateStore.isGenerating || aiCreateStore.isLoadingModels }">
                            <span v-if="aiCreateStore.isLoadingModels">{{ t('page.ai.createPage.loadingModels')
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
                                        :class="{ active: aiCreateStore.selectedModelId === model.modelId }"
                                        @click="aiCreateStore.setSelectedModel(model.modelId); hide()">
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
                                                {{ userStore.quotaInfo.tokensUsedThisWeek.toLocaleString() }}
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
                    <button class="memo-button btn btn-primary"
                        :disabled="isQuotaDepleted || (hasGeneratedContent ? !canCreate : !canGenerate)"
                        @click="hasGeneratedContent ? handleCreate() : handleGenerate()">
                        <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" class="generate-icon" />
                        {{ primaryButtonLabel }}
                    </button>
                </div>
            </div>
        </template>
    </LazyModal>

    <!-- Quota Depleted Modal -->
    <Teleport to="body">
        <Transition name="modal-fade">
            <div v-if="showQuotaDepletedModal" class="quota-depleted-overlay" role="dialog" aria-modal="true"
                :aria-label="t('page.ai.createPage.quotaDepleted.title')" @click.self="showQuotaDepletedModal = false">
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

#AiCreate {
    .content-type-tabs {
        display: flex;
        gap: 12px;
        margin-bottom: 24px;

        .content-tab {
            flex: 1;
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 4px;
            padding: 16px 12px;
            border: 1px solid @memo-grey-lighter;
            background: white;
            border-radius: 8px;
            cursor: pointer;
            transition: all 0.2s ease;

            .tab-icon {
                font-size: 18px;
                color: @memo-grey-dark;
                margin-bottom: 4px;
            }

            .tab-label {
                font-weight: 600;
                font-size: 14px;
            }

            .tab-subtitle {
                font-size: 11px;
                color: @memo-grey-dark;
            }

            &:hover {
                border-color: @memo-grey-light;
                background: @memo-grey-lightest;
            }

            &.active {
                border-color: @memo-blue-link;
                background: fade(@memo-blue-link, 5%);

                .tab-icon {
                    color: @memo-blue-link;
                }
            }

            &:disabled {
                opacity: 0.6;
                cursor: not-allowed;
            }
        }
    }

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
            color: @memo-grey-dark;
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
                color: @memo-grey-dark;
                cursor: pointer;
                padding: 0;

                &:hover {
                    color: @memo-grey-darker;
                }
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
        border-radius: 0px;
        padding: 12px;
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
            height: 6px;
            -webkit-appearance: none;
            appearance: none;
            background: @memo-grey-lighter;
            border-radius: 3px;
            outline: none;
            cursor: pointer;
            user-select: none;

            &::-webkit-slider-thumb {
                -webkit-appearance: none;
                appearance: none;
                width: 18px;
                height: 18px;
                background: white;
                border: 2px solid @memo-grey-darker;
                border-radius: 50%;
                cursor: pointer;
                box-shadow: 0 1px 3px rgba(0, 0, 0, 0.15);
            }

            &::-moz-range-thumb {
                width: 18px;
                height: 18px;
                background: white;
                border: 2px solid @memo-grey-darker;
                border-radius: 50%;
                cursor: pointer;
                box-shadow: 0 1px 3px rgba(0, 0, 0, 0.15);
            }

            &::-moz-range-progress {
                background: @memo-grey-darkest;
                border-radius: 3px;
                height: 6px;
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

.ai-create-footer {
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

        .memo-button {
            display: flex;
            align-items: center;
            gap: 8px;
            font-weight: 600;
            padding: 10px 24px;
            border-radius: 8px;

            .generate-icon {
                font-size: 14px;
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
