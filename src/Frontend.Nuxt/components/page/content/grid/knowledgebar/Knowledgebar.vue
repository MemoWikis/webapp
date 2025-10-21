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

    // Add wuwi (wishknowledge) items
    if (props.knowledgebarData.inWishKnowledge) {
        const wuwiItems = [
            { key: 'solidWishKnowledge', value: props.knowledgebarData.inWishKnowledge.solid },
            { key: 'needsConsolidationWishKnowledge', value: props.knowledgebarData.inWishKnowledge.needsConsolidation },
            { key: 'needsLearningWishKnowledge', value: props.knowledgebarData.inWishKnowledge.needsLearning },
            { key: 'notLearnedWishKnowledge', value: props.knowledgebarData.inWishKnowledge.notLearned }
        ]

        for (const item of wuwiItems) {
            if (item.value > 0) {
                knowledgebarTooltipData.value.push({
                    value: item.value,
                    class: item.key,
                })
            }
        }
    }

    // Add notInWishKnowledge (not in wishknowledge) items
    if (props.knowledgebarData.notInWishKnowledge) {
        const notInWishKnowledgeItems = [
            { key: 'solidNotInWishKnowledge', value: props.knowledgebarData.notInWishKnowledge.solid },
            { key: 'needsConsolidationNotInWishKnowledge', value: props.knowledgebarData.notInWishKnowledge.needsConsolidation },
            { key: 'needsLearningNotInWishKnowledge', value: props.knowledgebarData.notInWishKnowledge.needsLearning },
            { key: 'notLearnedNotInWishKnowledge', value: props.knowledgebarData.notInWishKnowledge.notLearned }
        ]

        for (const item of notInWishKnowledgeItems) {
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
        // WishKnowledge (wishknowledge) categories
        case 'solidWishKnowledge':
            return t('knowledgeStatus.solidCount', count) + ' (Wunschwissen)'
        case 'needsConsolidationWishKnowledge':
            return t('knowledgeStatus.needsConsolidationCount', count) + ' (Wunschwissen)'
        case 'needsLearningWishKnowledge':
            return t('knowledgeStatus.needsLearningCount', count) + ' (Wunschwissen)'
        case 'notLearnedWishKnowledge':
            return t('knowledgeStatus.notLearnedCount', count) + ' (Wunschwissen)'

        // Not in wuwi (not in wishknowledge) categories
        case 'solidNotInWishKnowledge':
            return t('knowledgeStatus.solidCount', count) + ' (Nicht im Wunschwissen)'
        case 'needsConsolidationNotInWishKnowledge':
            return t('knowledgeStatus.needsConsolidationCount', count) + ' (Nicht im Wunschwissen)'
        case 'needsLearningNotInWishKnowledge':
            return t('knowledgeStatus.needsLearningCount', count) + ' (Nicht im Wunschwissen)'
        case 'notLearnedNotInWishKnowledge':
            return t('knowledgeStatus.notLearnedCount', count) + ' (Nicht im Wunschwissen)'

        // Legacy cases (for backward compatibility)
        case 'solid':
            return t('knowledgeStatus.solidCount', count)
        case 'needsConsolidation':
            return t('knowledgeStatus.needsConsolidationCount', count)
        case 'needsLearning':
            return t('knowledgeStatus.needsLearningCount', count)
        case 'notLearned':
            return t('knowledgeStatus.notLearnedCount', count)
        case 'notInWishKnowledge':
            return t('knowledgeStatus.notInWishKnowledgeCount', count)
    }
}

onBeforeMount(() => setKnowledgebarData())
watch(() => props.knowledgebarData, () => setKnowledgebarData(), { deep: true })
const ariaId = useId()

</script>

<template>
    <VTooltip :aria-id="ariaId" class="tooltip-container">
        <div class="knowledgebar">
            <!-- WishKnowledge (wishknowledge) sections -->
            <div v-if="knowledgebarData.inWishKnowledge?.solidPercentage > 0" class="solid-knowledge wuwi"
                :style="{ 'width': knowledgebarData.inWishKnowledge.solidPercentage + '%' }">
            </div>
            <div v-if="knowledgebarData.inWishKnowledge?.needsConsolidationPercentage > 0" class="needs-consolidation wuwi"
                :style="{ 'width': knowledgebarData.inWishKnowledge.needsConsolidationPercentage + '%' }">
            </div>
            <div v-if="knowledgebarData.inWishKnowledge?.needsLearningPercentage > 0" class="needs-learning wuwi"
                :style="{ 'width': knowledgebarData.inWishKnowledge.needsLearningPercentage + '%' }">
            </div>
            <div v-if="knowledgebarData.inWishKnowledge?.notLearnedPercentage > 0" class="not-learned wuwi"
                :style="{ 'width': knowledgebarData.inWishKnowledge.notLearnedPercentage + '%' }">
            </div>

            <!-- Not in wuwi (not in wishknowledge) sections -->
            <div v-if="knowledgebarData.notInWishKnowledge?.solidPercentage > 0" class="solid-knowledge not-in-wuwi"
                :style="{ 'width': knowledgebarData.notInWishKnowledge.solidPercentage + '%' }">
            </div>
            <div v-if="knowledgebarData.notInWishKnowledge?.needsConsolidationPercentage > 0" class="needs-consolidation not-in-wuwi"
                :style="{ 'width': knowledgebarData.notInWishKnowledge.needsConsolidationPercentage + '%' }">
            </div>
            <div v-if="knowledgebarData.notInWishKnowledge?.needsLearningPercentage > 0" class="needs-learning not-in-wuwi"
                :style="{ 'width': knowledgebarData.notInWishKnowledge.needsLearningPercentage + '%' }">
            </div>
            <div v-if="knowledgebarData.notInWishKnowledge?.notLearnedPercentage > 0" class="not-learned not-in-wuwi"
                :style="{ 'width': knowledgebarData.notInWishKnowledge.notLearnedPercentage + '%' }">
            </div>
        </div>
        <template #popper>
            <b>{{ t('page.grid.knowledgeStatus.title') }}</b>
            <div v-for="d in knowledgebarTooltipData" v-if="knowledgebarTooltipData.some(d => d.value > 0)"
                class="knowledgesummary-info">
                <div class="color-container" :class="`color-${d.class}`"></div>
                <div>{{ getTooltipLabel(d.class!, d.value) }}</div>
            </div>
            <div v-else>
                {{ t('page.grid.knowledgeStatus.noQuestions') }}
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

        &.not-in-wuwi {
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

        &.not-in-wuwi {
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

        &.not-in-wuwi {
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

        &.not-in-wuwi {
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

        // WishKnowledge (wishknowledge) colors - solid colors
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

        // Not in wuwi colors - striped patterns
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
    }
}
</style>