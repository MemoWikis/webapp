<script lang="ts" setup>
import { useAiCreateStore } from './aiCreateStore'
import DOMPurify from 'isomorphic-dompurify'

interface Props {
    isWikiWithSubpages: boolean
}

const props = defineProps<Props>()
const emit = defineEmits<{ regenerate: [] }>()

const aiCreateStore = useAiCreateStore()
const { t } = useI18n()

function sanitizeHtml(html: string): string {
    return DOMPurify.sanitize(html)
}

const currentPreviewContent = computed(() => {
    if (props.isWikiWithSubpages && aiCreateStore.generatedWikiContent) {
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
</script>

<template>
    <!-- Single Page Preview -->
    <div v-if="aiCreateStore.generatedContent && !props.isWikiWithSubpages" class="preview-section">
        <div class="preview-title">
            <span>{{ t('page.ai.createPage.preview') }}</span>
            <button type="button" class="regenerate-btn" :disabled="aiCreateStore.isGenerating"
                :title="t('page.ai.createPage.button.regenerate')" @click="emit('regenerate')">
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

    <!-- Wiki with Subpages Preview -->
    <div v-if="aiCreateStore.generatedWikiContent && props.isWikiWithSubpages" class="preview-section wiki-preview">
        <div class="preview-title">
            <span>{{ t('page.ai.createPage.previewWiki') }}</span>
            <button type="button" class="regenerate-btn" :disabled="aiCreateStore.isGenerating"
                :title="t('page.ai.createPage.button.regenerate')" @click="emit('regenerate')">
                <font-awesome-icon :icon="['fas', 'rotate']" :spin="aiCreateStore.isGenerating" />
            </button>
        </div>

        <!-- Wiki Structure Navigation -->
        <div class="wiki-structure">
            <div class="wiki-nav-item wiki-main" :class="{ active: aiCreateStore.selectedSubpageIndex === null }"
                @click="aiCreateStore.selectedSubpageIndex = null">
                <font-awesome-icon :icon="['fas', 'book']" class="nav-icon" />
                <span class="nav-title">{{ aiCreateStore.generatedWikiContent.title }}</span>
                <span class="nav-badge">{{ t('page.ai.createPage.wikiMain') }}</span>
            </div>
            <div v-for="(subpage, index) in aiCreateStore.generatedWikiContent.subpages" :key="index"
                class="wiki-nav-item wiki-subpage" :class="{ active: aiCreateStore.selectedSubpageIndex === index }"
                @click="aiCreateStore.selectedSubpageIndex = index">
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
                    count: aiCreateStore.generatedWikiContent.subpages.length
                }) }}
            </span>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

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
</style>
