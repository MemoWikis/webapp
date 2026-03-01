<script lang="ts" setup>
import { ContentType, useAiCreateStore } from './aiCreateStore'

const aiCreateStore = useAiCreateStore()
const { t } = useI18n()
</script>

<template>
    <div class="content-type-tabs">
        <button type="button" class="content-tab" :class="{ active: aiCreateStore.contentType === ContentType.Page }"
            :disabled="aiCreateStore.isGenerating" @click="aiCreateStore.contentType = ContentType.Page">
            <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" class="tab-icon" />
            <span class="tab-label">{{ t('page.ai.createPage.tab.page') }}</span>
        </button>
        <button type="button" class="content-tab" :class="{ active: aiCreateStore.contentType === ContentType.Wiki }"
            :disabled="aiCreateStore.isGenerating" @click="aiCreateStore.contentType = ContentType.Wiki">
            <font-awesome-icon :icon="['fas', 'file-lines']" class="tab-icon" />
            <span class="tab-label">{{ t('page.ai.createPage.tab.wiki') }}</span>
        </button>
        <button type="button" class="content-tab"
            :class="{ active: aiCreateStore.contentType === ContentType.Flashcards }"
            :disabled="aiCreateStore.isGenerating" @click="aiCreateStore.contentType = ContentType.Flashcards">
            <font-awesome-icon :icon="['fas', 'book-open']" class="tab-icon" />
            <span class="tab-label">{{ t('page.ai.createPage.tab.flashcards') }}</span>
        </button>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

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
</style>
