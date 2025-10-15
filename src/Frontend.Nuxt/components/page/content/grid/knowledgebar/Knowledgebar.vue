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
    for (const [key, value] of Object.entries(props.knowledgebarData)) {
        if (key === 'solid' || key === 'needsConsolidation' || key === 'needsLearning' || key === 'notLearned' || key === 'notInWishknowledge')
            knowledgebarTooltipData.value.push({
                value: value,
                class: key,
            })
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
        case 'notInWishknowledge':
            return t('knowledgeStatus.notInWishknowledgeCount', count)
    }
}

onBeforeMount(() => setKnowledgebarData())
watch(() => props.knowledgebarData, () => setKnowledgebarData(), { deep: true })
const ariaId = useId()

</script>

<template>
    <VTooltip :aria-id="ariaId" class="tooltip-container">
        <div class="knowledgebar">
            <div v-if="knowledgebarData.solidPercentage > 0" class="solid-knowledge"
                :style="{ 'width': knowledgebarData.solidPercentage + '%' }">
            </div>
            <div v-if="knowledgebarData.needsConsolidationPercentage > 0" class="needs-consolidation"
                :style="{ 'width': knowledgebarData.needsConsolidationPercentage + '%' }">
            </div>
            <div v-if="knowledgebarData.needsLearningPercentage > 0" class="needs-learning"
                :style="{ 'width': knowledgebarData.needsLearningPercentage + '%' }">
            </div>
            <div v-if="knowledgebarData.notLearnedPercentage > 0" class="not-learned"
                :style="{ 'width': knowledgebarData.notLearnedPercentage + '%' }">
            </div>
            <div v-if="knowledgebarData.notInWishknowledgePercentage > 0" class="not-in-wish-knowledge"
                :style="{ 'width': knowledgebarData.notInWishknowledgePercentage + '%' }">
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


.knowledgesummary-info {

    display: flex;
    flex-wrap: nowrap;
    align-items: center;

    .color-container {
        height: 12px;
        width: 12px;
        margin-right: 4px;
        border-radius: 50%;

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
    }
}
</style>