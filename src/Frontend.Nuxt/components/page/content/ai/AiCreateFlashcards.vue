<script lang="ts" setup>
import { DifficultyLevel, useAiCreateStore } from './aiCreateStore'

const aiCreateStore = useAiCreateStore()
const { t } = useI18n()

const complexityLabels = computed(() => ({
    [DifficultyLevel.ELI5]: t('page.ai.createPage.complexity.simple'),
    [DifficultyLevel.Beginner]: t('page.ai.createPage.complexity.basic'),
    [DifficultyLevel.Intermediate]: t('page.ai.createPage.complexity.standard'),
    [DifficultyLevel.Advanced]: t('page.ai.createPage.complexity.advanced'),
    [DifficultyLevel.Academic]: t('page.ai.createPage.complexity.expert')
}))
</script>

<template>
    <div class="ai-create-flashcards">
        <!-- Info Banner -->
        <div v-if="aiCreateStore.generatedFlashcards.length === 0 && !aiCreateStore.isGenerating" class="flashcards-info-banner">
            <font-awesome-icon :icon="['fas', 'book-open']" class="info-icon" />
            <span>{{ t('page.ai.createPage.flashcardsInfo') }}</span>
        </div>

        <!-- Complexity Level Section -->
        <div class="form-group">
            <label>{{ t('page.ai.createPage.complexityLabel') }}</label>
            <PageContentAiCreateSlider v-model="aiCreateStore.difficultyLevel" :min="1" :max="5" :labels="complexityLabels"
                :left-label="t('page.ai.createPage.complexity.simple')"
                :right-label="t('page.ai.createPage.complexity.expert')" :disabled="aiCreateStore.isGenerating" />
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

        <!-- Generated Flashcards -->
        <div v-if="aiCreateStore.generatedFlashcards.length > 0" id="AiFlashcard" class="generated-flashcards">
            <PageLearningAiFlashCard v-for="(flashcard, i) in aiCreateStore.generatedFlashcards" :key="i"
                :flashcard="flashcard" :index="i" @delete-flashcard="aiCreateStore.deleteFlashcard(i)" />
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
