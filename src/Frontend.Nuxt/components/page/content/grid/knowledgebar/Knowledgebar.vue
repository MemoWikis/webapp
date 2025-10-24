<script lang="ts" setup>
import { KnowledgeSummary } from '~/composables/knowledgeSummary'

interface Props {
    knowledgebarData: KnowledgeSummary
    useTotal?: boolean
}
const props = withDefaults(defineProps<Props>(), {
    useTotal: true
})

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
            <!-- Total sections using backend-calculated totals -->
            <template v-if="props.useTotal">
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
                <div v-if="knowledgebarData.total?.notInWishKnowledgePercentage && knowledgebarData.total.notInWishKnowledgePercentage > 0" class="not-in-wish-knowledge total"
                    :style="{ 'width': knowledgebarData.total.notInWishKnowledgePercentage + '%' }">
                </div>
            </template>
            <template v-else>
                <div v-if="knowledgebarData.inWishKnowledge?.solidPercentage > 0" class="solid-knowledge total"
                    :style="{ 'width': knowledgebarData.inWishKnowledge.solidPercentage + '%' }">
                </div>
                <div v-if="knowledgebarData.inWishKnowledge?.needsConsolidationPercentage > 0" class="needs-consolidation total"
                    :style="{ 'width': knowledgebarData.inWishKnowledge.needsConsolidationPercentage + '%' }">
                </div>
                <div v-if="knowledgebarData.inWishKnowledge?.needsLearningPercentage > 0" class="needs-learning total"
                    :style="{ 'width': knowledgebarData.inWishKnowledge.needsLearningPercentage + '%' }">
                </div>
                <div v-if="knowledgebarData.inWishKnowledge?.notLearnedPercentage > 0" class="not-learned total"
                    :style="{ 'width': knowledgebarData.inWishKnowledge.notLearnedPercentage + '%' }">
                </div>
            </template>
        </div>
        <template #popper>
            <b>{{ t('page.grid.knowledgeStatus.title') }}</b>

            <!-- Individual status items -->
            <div v-if="knowledgebarData.inWishKnowledge?.solid > 0" class="knowledgesummary-info">
                <div class="color-container color-solid"></div>
                <div>{{ getTooltipLabel('solid', props.useTotal ? knowledgebarData.inWishKnowledge.solid : knowledgebarData.inWishKnowledge.solid) }}</div>
            </div>
            <div v-if="knowledgebarData.inWishKnowledge?.needsConsolidation > 0" class="knowledgesummary-info">
                <div class="color-container color-needsConsolidation"></div>
                <div>{{ getTooltipLabel('needsConsolidation', props.useTotal ? knowledgebarData.inWishKnowledge.needsConsolidation : knowledgebarData.inWishKnowledge.needsConsolidation) }}</div>
            </div>
            <div v-if="knowledgebarData.inWishKnowledge?.needsLearning > 0" class="knowledgesummary-info">
                <div class="color-container color-needsLearning"></div>
                <div>{{ getTooltipLabel('needsLearning', props.useTotal ? knowledgebarData.inWishKnowledge.needsLearning : knowledgebarData.inWishKnowledge.needsLearning) }}</div>
            </div>
            <div v-if="knowledgebarData.inWishKnowledge?.notLearned > 0" class="knowledgesummary-info">
                <div class="color-container color-notLearned"></div>
                <div>{{ getTooltipLabel('notLearned', props.useTotal ? knowledgebarData.inWishKnowledge.notLearned : knowledgebarData.inWishKnowledge.notLearned) }}</div>
            </div>

            <!-- NotInWish knowledge items - only show when useTotal is true -->
            <div v-if="props.useTotal && knowledgebarData.total?.notInWishKnowledgeCount && knowledgebarData.total.notInWishKnowledgeCount > 0" class="knowledgesummary-info">
                <div class="color-container color-notInWishKnowledge"></div>
                <div>{{ t('knowledgeStatus.notInWishKnowledgeCount', knowledgebarData.total.notInWishKnowledgeCount) }}</div>
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
    }

    .needs-consolidation {
        background-color: @needs-consolidation-color;
    }

    .solid-knowledge {
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

        /* Legacy colors */
        &.color-notLearned {
            background: @memo-grey-dark;
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
            background: @memo-grey-light;
        }
    }
}
</style>