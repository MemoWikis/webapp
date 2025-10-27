<script lang="ts" setup>
import { KnowledgeSummary } from '~/composables/knowledgeSummary'

interface Props {
    knowledgebarData: KnowledgeSummary
    useTotal?: boolean
}
const props = withDefaults(defineProps<Props>(), {
    useTotal: true
})

interface KnowledgeItem {
    key: string
    value: number
    percentage: number
    sharedClass: string
    translationKey: string
}

const { t } = useI18n()

const knowledgeItems = computed<KnowledgeItem[]>(() => {
    const items: KnowledgeItem[] = []

    if (props.useTotal && props.knowledgebarData.total) {
        const totalItems = [
            {
                key: 'solid',
                value: props.knowledgebarData.inWishKnowledge?.solid || 0,
                percentage: props.knowledgebarData.total.solidPercentage,
                sharedClass: 'solid',
                translationKey: 'knowledgeStatus.solidCount'
            },
            {
                key: 'needsConsolidation',
                value: props.knowledgebarData.inWishKnowledge?.needsConsolidation || 0,
                percentage: props.knowledgebarData.total.needsConsolidationPercentage,
                sharedClass: 'needs-consolidation',
                translationKey: 'knowledgeStatus.needsConsolidationCount'
            },
            {
                key: 'needsLearning',
                value: props.knowledgebarData.inWishKnowledge?.needsLearning || 0,
                percentage: props.knowledgebarData.total.needsLearningPercentage,
                sharedClass: 'needs-learning',
                translationKey: 'knowledgeStatus.needsLearningCount'
            },
            {
                key: 'notLearned',
                value: props.knowledgebarData.inWishKnowledge?.notLearned || 0,
                percentage: props.knowledgebarData.total.notLearnedPercentage,
                sharedClass: 'not-learned',
                translationKey: 'knowledgeStatus.notLearnedCount'
            },
            {
                key: 'notInWishKnowledge',
                value: props.knowledgebarData.total.notInWishKnowledgeCount || 0,
                percentage: props.knowledgebarData.total.notInWishKnowledgePercentage,
                sharedClass: 'not-in-wish-knowledge',
                translationKey: 'knowledgeStatus.notInWishKnowledgeCount'
            }
        ]

        for (const item of totalItems) {
            if (item.percentage && item.percentage > 0) {
                items.push({
                    key: item.key,
                    value: item.value,
                    percentage: item.percentage,
                    sharedClass: item.sharedClass,
                    translationKey: item.translationKey
                })
            }
        }
    } else if (props.knowledgebarData.inWishKnowledge) {
        const inWishItems = [
            {
                key: 'solid',
                value: props.knowledgebarData.inWishKnowledge.solid,
                percentage: props.knowledgebarData.inWishKnowledge.solidPercentage,
                sharedClass: 'solid',
                translationKey: 'knowledgeStatus.solidCount'
            },
            {
                key: 'needsConsolidation',
                value: props.knowledgebarData.inWishKnowledge.needsConsolidation,
                percentage: props.knowledgebarData.inWishKnowledge.needsConsolidationPercentage,
                sharedClass: 'needs-consolidation',
                translationKey: 'knowledgeStatus.needsConsolidationCount'
            },
            {
                key: 'needsLearning',
                value: props.knowledgebarData.inWishKnowledge.needsLearning,
                percentage: props.knowledgebarData.inWishKnowledge.needsLearningPercentage,
                sharedClass: 'needs-learning',
                translationKey: 'knowledgeStatus.needsLearningCount'
            },
            {
                key: 'notLearned',
                value: props.knowledgebarData.inWishKnowledge.notLearned,
                percentage: props.knowledgebarData.inWishKnowledge.notLearnedPercentage,
                sharedClass: 'not-learned',
                translationKey: 'knowledgeStatus.notLearnedCount'
            }
        ]

        for (const item of inWishItems) {
            if (item.percentage && item.percentage > 0) {
                items.push({
                    key: item.key,
                    value: item.value,
                    percentage: item.percentage,
                    sharedClass: item.sharedClass,
                    translationKey: item.translationKey
                })
            }
        }
    }

    return items
})

const barSegments = computed(() => knowledgeItems.value.map(item => ({
    key: item.key,
    percentage: item.percentage,
    cssClass: `${item.sharedClass} total`
})))

const tooltipItems = computed(() => knowledgeItems.value.filter(item => item.value > 0).map(item => ({
    key: item.key,
    value: item.value,
    colorClass: item.sharedClass,
    translationKey: item.translationKey
})))

const ariaId = useId()

</script>

<template>
    <VTooltip :aria-id="ariaId" class="tooltip-container">
        <div class="knowledgebar">
            <div v-for="segment in barSegments" :key="segment.key" :class="segment.cssClass"
                :style="{ 'width': segment.percentage + '%' }">
            </div>
        </div>
        <template #popper>
            <b>{{ t('page.grid.knowledgeStatus.title') }}</b>

            <div v-for="item in tooltipItems" :key="item.key" class="knowledgesummary-info">
                <div class="color-container" :class="item.colorClass"></div>
                <div>{{ t(item.translationKey, item.value) }}</div>
            </div>
        </template>
    </VTooltip>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.knowledgebar {
    display: inline-flex;
    height: 6px;
    min-width: 0px;
    width: 100%;
    max-width: 180px;
    border-radius: 6px;
    overflow: hidden;

    cursor: help;

    .solid,
    .needs-learning,
    .needs-consolidation,
    .not-learned,
    .not-in-wish-knowledge {
        height: inherit;
        float: left;
    }

    .needs-learning {
        background-color: @needs-learning-color;
    }

    .needs-consolidation {
        background-color: @needs-consolidation-color;
    }

    .solid {
        background-color: @solid-knowledge-color;
    }

    .not-learned {
        background-color: @not-learned-color;
    }

    .not-in-wish-knowledge {
        background-color: @not-in-wish-knowledge-color;
    }
}

.tooltip-section {
    margin-bottom: 8px;

    &:last-child {
        margin-bottom: 0;
    }

    h4 {
        margin: 4px 0 4px 0;
        font-size: 12px;
        font-weight: 600;
        color: #666;
    }
}


.knowledgesummary-info {

    display: flex;
    flex-wrap: nowrap;
    align-items: center;

    .color-container {
        height: 12px;
        width: 12px;
        margin-right: 4px;
        border-radius: 50%;

        &.not-learned {
            background: @memo-grey-dark;
        }

        &.needs-learning {
            background: @memo-salmon;
        }

        &.needs-consolidation {
            background: @memo-yellow;
        }

        &.solid {
            background: @memo-green;
        }

        &.not-in-wish-knowledge {
            background: @memo-grey-light;
        }
    }
}
</style>