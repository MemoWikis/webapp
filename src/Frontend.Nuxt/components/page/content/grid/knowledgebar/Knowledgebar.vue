<script lang="ts" setup>
import { KnowledgeSummary } from '~/composables/knowledgeSummary'

interface Props {
    knowledgebarData: KnowledgeSummary
}
const props = defineProps<Props>()

interface KnowledgebarTooltipData {
    value: number
    class: string
}

const knowledgebarTooltipData = ref<KnowledgebarTooltipData[]>([])

function setKnowledgebarData() {
    knowledgebarTooltipData.value = []

    // Use total data for the knowledge bar
    if (props.knowledgebarData.total) {
        const totalItems = [
            { key: 'solid', value: props.knowledgebarData.total.solid },
            { key: 'needsConsolidation', value: props.knowledgebarData.total.needsConsolidation },
            { key: 'needsLearning', value: props.knowledgebarData.total.needsLearning },
            { key: 'notLearned', value: props.knowledgebarData.total.notLearned }
        ]

        for (const item of totalItems) {
            if (item.value > 0) {
                knowledgebarTooltipData.value.push({
                    value: item.value,
                    class: item.key,
                })
            }
        }
    }

    knowledgebarTooltipData.value = knowledgebarTooltipData.value.slice().reverse()
}

const { t } = useI18n()

function getTooltipLabel(key: string, count: number) {
    switch (key) {
        case 'solid':
            return t('knowledgeStatus.solidCount', count)
        case 'needsConsolidation':
            return t('knowledgeStatus.needsConsolidationCount', count)
        case 'needsLearning':
            return t('knowledgeStatus.needsLearningCount', count)
        case 'notLearned':
            return t('knowledgeStatus.notLearnedCount', count)
    }
}

onBeforeMount(() => setKnowledgebarData())
watch(() => props.knowledgebarData, () => setKnowledgebarData(), { deep: true })
const ariaId = useId()

</script>

<template>
    <VTooltip :aria-id="ariaId" class="tooltip-container">
        <div class="knowledgebar">
            <!-- Total sections using backend-calculated totals -->
            <div v-if="knowledgebarData.total?.solidPercentage > 0" class="solid-knowledge total"
                :style="{ 'width': knowledgebarData.total.solidPercentage + '%' }">
            </div>
            <div v-if="knowledgebarData.total?.needsConsolidationPercentage > 0" class="needs-consolidation total"
                :style="{ 'width': knowledgebarData.total.needsConsolidationPercentage + '%' }">
            </div>
            <div v-if="knowledgebarData.total?.needsLearningPercentage > 0" class="needs-learning total"
                :style="{ 'width': knowledgebarData.total.needsLearningPercentage + '%' }">
            </div>
            <div v-if="knowledgebarData.total?.notLearnedPercentage > 0" class="not-learned total"
                :style="{ 'width': knowledgebarData.total.notLearnedPercentage + '%' }">
            </div>
        </div>
        <template #popper>
            <!-- Two-section tooltip with breakdown -->
            <b>{{ t('page.grid.knowledgeStatus.title') }}</b>

            <!-- In Wish Knowledge Section -->
            <div class="tooltip-section">
                <h4>{{ t('label.inWishKnowledge') }}</h4>
                <div v-if="knowledgebarData.inWishKnowledge?.solid > 0" class="knowledgesummary-info">
                    <div class="color-container color-solid"></div>
                    <div>{{ getTooltipLabel('solid', knowledgebarData.inWishKnowledge.solid) }}</div>
                </div>
                <div v-if="knowledgebarData.inWishKnowledge?.needsConsolidation > 0" class="knowledgesummary-info">
                    <div class="color-container color-needsConsolidation"></div>
                    <div>{{ getTooltipLabel('needsConsolidation', knowledgebarData.inWishKnowledge.needsConsolidation) }}</div>
                </div>
                <div v-if="knowledgebarData.inWishKnowledge?.needsLearning > 0" class="knowledgesummary-info">
                    <div class="color-container color-needsLearning"></div>
                    <div>{{ getTooltipLabel('needsLearning', knowledgebarData.inWishKnowledge.needsLearning) }}</div>
                </div>
                <div v-if="knowledgebarData.inWishKnowledge?.notLearned > 0" class="knowledgesummary-info">
                    <div class="color-container color-notLearned"></div>
                    <div>{{ getTooltipLabel('notLearned', knowledgebarData.inWishKnowledge.notLearned) }}</div>
                </div>
            </div>

            <!-- Not in Wish Knowledge Section -->
            <div class="tooltip-section" v-if="knowledgebarData.notInWishKnowledge">
                <h4>{{ t('label.notInWishKnowledge') }}</h4>
                <div v-if="knowledgebarData.notInWishKnowledge?.solid > 0" class="knowledgesummary-info">
                    <div class="color-container color-solid-striped"></div>
                    <div>{{ getTooltipLabel('solid', knowledgebarData.notInWishKnowledge.solid) }}</div>
                </div>
                <div v-if="knowledgebarData.notInWishKnowledge?.needsConsolidation > 0" class="knowledgesummary-info">
                    <div class="color-container color-needsConsolidation-striped"></div>
                    <div>{{ getTooltipLabel('needsConsolidation', knowledgebarData.notInWishKnowledge.needsConsolidation) }}</div>
                </div>
                <div v-if="knowledgebarData.notInWishKnowledge?.needsLearning > 0" class="knowledgesummary-info">
                    <div class="color-container color-needsLearning-striped"></div>
                    <div>{{ getTooltipLabel('needsLearning', knowledgebarData.notInWishKnowledge.needsLearning) }}</div>
                </div>
                <div v-if="knowledgebarData.notInWishKnowledge?.notLearned > 0" class="knowledgesummary-info">
                    <div class="color-container color-notLearned-striped"></div>
                    <div>{{ getTooltipLabel('notLearned', knowledgebarData.notInWishKnowledge.notLearned) }}</div>
                </div>
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

    .solid-knowledge,
    .needs-learning,
    .needs-consolidation,
    .not-learned,
    .not-in-wish-knowledge {
        height: inherit;
        float: left;
    }

    .needs-learning {
        background-color: @needs-learning-color;

        &.not-in-wishKnowledge {
            opacity: 0.6;
            background: repeating-linear-gradient(45deg,
                    @needs-learning-color,
                    @needs-learning-color 2px,
                    fadeout(@needs-learning-color, 30%) 2px,
                    fadeout(@needs-learning-color, 30%) 4px);
        }
    }

    .needs-consolidation {
        background-color: @needs-consolidation-color;

        &.not-in-wishKnowledge {
            opacity: 0.6;
            background: repeating-linear-gradient(45deg,
                    @needs-consolidation-color,
                    @needs-consolidation-color 2px,
                    fadeout(@needs-consolidation-color, 30%) 2px,
                    fadeout(@needs-consolidation-color, 30%) 4px);
        }
    }

    .solid-knowledge {
        background-color: @solid-knowledge-color;

        &.not-in-wishKnowledge {
            opacity: 0.6;
            background: repeating-linear-gradient(45deg,
                    @solid-knowledge-color,
                    @solid-knowledge-color 2px,
                    fadeout(@solid-knowledge-color, 30%) 2px,
                    fadeout(@solid-knowledge-color, 30%) 4px);
        }
    }

    .not-learned {
        background-color: @not-learned-color;

        &.not-in-wishKnowledge {
            opacity: 0.6;
            background: repeating-linear-gradient(45deg,
                    @not-learned-color,
                    @not-learned-color 2px,
                    fadeout(@not-learned-color, 30%) 2px,
                    fadeout(@not-learned-color, 30%) 4px);
        }
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

        // Legacy colors
        &.color-notLearned {
            background: @memo-grey-light;
        }

        &.color-needsLearning {
            background: @memo-salmon;
        }

        &.color-needsConsolidation {
            background: @memo-yellow;
        }

        &.color-solid {
            background: @memo-green;
        }

        &.color-notInWishKnowledge {
            background: @memo-grey-dark;
        }

        // WishKnowledge (wishKnowledge) colors - solid colors
        &.color-solidWishKnowledge {
            background: @memo-green;
        }

        &.color-needsConsolidationWishKnowledge {
            background: @memo-yellow;
        }

        &.color-needsLearningWishKnowledge {
            background: @memo-salmon;
        }

        &.color-notLearnedWishKnowledge {
            background: @memo-grey-light;
        }

        // Not in wishKnowledge colors - striped patterns
        &.color-solidNotInWishKnowledge {
            background: repeating-linear-gradient(45deg,
                    @memo-green,
                    @memo-green 2px,
                    fadeout(@memo-green, 50%) 2px,
                    fadeout(@memo-green, 50%) 4px);
        }

        &.color-needsConsolidationNotInWishKnowledge {
            background: repeating-linear-gradient(45deg,
                    @memo-yellow,
                    @memo-yellow 2px,
                    fadeout(@memo-yellow, 50%) 2px,
                    fadeout(@memo-yellow, 50%) 4px);
        }

        &.color-needsLearningNotInWishKnowledge {
            background: repeating-linear-gradient(45deg,
                    @memo-salmon,
                    @memo-salmon 2px,
                    fadeout(@memo-salmon, 50%) 2px,
                    fadeout(@memo-salmon, 50%) 4px);
        }

        &.color-notLearnedNotInWishKnowledge {
            background: repeating-linear-gradient(45deg,
                    @memo-grey-light,
                    @memo-grey-light 2px,
                    fadeout(@memo-grey-light, 50%) 2px,
                    fadeout(@memo-grey-light, 50%) 4px);
        }

        // Striped colors for two-section tooltip
        &.color-solid-striped {
            background: repeating-linear-gradient(45deg,
                    @memo-green,
                    @memo-green 2px,
                    fadeout(@memo-green, 50%) 2px,
                    fadeout(@memo-green, 50%) 4px);
        }

        &.color-needsConsolidation-striped {
            background: repeating-linear-gradient(45deg,
                    @memo-yellow,
                    @memo-yellow 2px,
                    fadeout(@memo-yellow, 50%) 2px,
                    fadeout(@memo-yellow, 50%) 4px);
        }

        &.color-needsLearning-striped {
            background: repeating-linear-gradient(45deg,
                    @memo-salmon,
                    @memo-salmon 2px,
                    fadeout(@memo-salmon, 50%) 2px,
                    fadeout(@memo-salmon, 50%) 4px);
        }

        &.color-notLearned-striped {
            background: repeating-linear-gradient(45deg,
                    @memo-grey-light,
                    @memo-grey-light 2px,
                    fadeout(@memo-grey-light, 50%) 2px,
                    fadeout(@memo-grey-light, 50%) 4px);
        }
    }
}
</style>