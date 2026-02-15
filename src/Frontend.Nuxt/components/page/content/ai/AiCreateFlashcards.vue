<script lang="ts" setup>
import { DifficultyLevel, useAiCreateStore } from './aiCreateStore'

const aiCreateStore = useAiCreateStore()
const { t } = useI18n()
const { isMobile } = useDevice()
const detailDropdownAriaId = useId()

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

function sliderBackground(value: number, min: number, max: number): string {
    const percentage = ((value - min) / (max - min)) * 100
    return `linear-gradient(to right, #101010 0%, #101010 ${percentage}%, #EFEFEF ${percentage}%, #EFEFEF 100%)`
}

const complexitySliderStyle = computed(() => ({
    background: sliderBackground(aiCreateStore.difficultyLevel, 1, 5)
}))
</script>

<template>
    <div class="ai-create-flashcards">
        <!-- Info Banner -->
        <div class="flashcards-info-banner">
            <font-awesome-icon :icon="['fas', 'book-open']" class="info-icon" />
            <span>{{ t('page.ai.createPage.flashcardsInfo') }}</span>
        </div>

        <!-- Complexity Level Section -->
        <div class="form-group detail-section">
            <label>{{ t('page.ai.createPage.complexityLabel') }}</label>

            <!-- Desktop: Slider -->
            <div v-if="!isMobile" class="detail-slider-container">
                <input v-model.number="aiCreateStore.difficultyLevel" type="range" min="1" max="5" class="detail-slider"
                    :style="complexitySliderStyle" :disabled="aiCreateStore.isGenerating"
                    :aria-label="t('page.ai.createPage.complexityLabel')" :aria-valuetext="currentComplexityLabel" />
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

        <!-- Loading State -->
        <div v-if="aiCreateStore.isGenerating" class="generating-state">
            <font-awesome-icon :icon="['fas', 'spinner']" spin />
            <span>{{ t('page.ai.createPage.generatingFlashcards') }}</span>
        </div>

        <!-- Error Message -->
        <div v-if="aiCreateStore.errorMessage" class="alert alert-danger">
            {{ t(aiCreateStore.errorMessage) }}
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.ai-create-flashcards {
    .flashcards-info-banner {
        display: flex;
        align-items: center;
        gap: 12px;
        padding: 14px 18px;
        background: fade(@memo-blue-link, 8%);
        border: 1px solid fade(@memo-blue-link, 20%);
        border-radius: 8px;
        color: @memo-grey-darker;
        font-size: 14px;
        margin-bottom: 24px;

        .info-icon {
            color: @memo-blue-link;
            font-size: 18px;
            flex-shrink: 0;
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
}
</style>
