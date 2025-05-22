<script lang="ts" setup>
import { KnowledgeSummary } from '~/composables/knowledgeSummary'
import { ChartData } from '~/components/chart/chartData'
import SharedKnowledgeSummary from '~/components/shared/SharedKnowledgeSummary.vue'

interface Props {
    knowledgeStatus: KnowledgeSummary
}

const props = defineProps<Props>()
const { t } = useI18n()

const pieData = computed<ChartData[]>(() => {
    const data: ChartData[] = []

    if (props.knowledgeStatus.solid > 0) {
        data.push({
            value: props.knowledgeStatus.solid,
            class: 'solid'
        })
    }

    if (props.knowledgeStatus.needsConsolidation > 0) {
        data.push({
            value: props.knowledgeStatus.needsConsolidation,
            class: 'needsConsolidation'
        })
    }

    if (props.knowledgeStatus.needsLearning > 0) {
        data.push({
            value: props.knowledgeStatus.needsLearning,
            class: 'needsLearning'
        })
    }

    if (props.knowledgeStatus.notLearned > 0) {
        data.push({
            value: props.knowledgeStatus.notLearned,
            class: 'notLearned'
        })
    }

    return data
})

const totalQuestions = computed(() => {
    return props.knowledgeStatus.solid +
        props.knowledgeStatus.needsConsolidation +
        props.knowledgeStatus.needsLearning +
        props.knowledgeStatus.notLearned
})
</script>

<template>
    <div class="knowledge-summary">
        <div class="summary-visualization">
            <div class="pie-chart-wrapper">
                <ChartPie :data="pieData" :width="150" :height="150" />
            </div>
            <div class="total-questions">
                <span class="count">{{ totalQuestions }}</span>
                <span class="total-questions-label">{{ t('label.questionCountAsText', totalQuestions) }}</span>
            </div>
        </div>
        <SharedKnowledgeSummary :knowledge-status="knowledgeStatus" />
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.knowledge-summary {
    display: flex;
    flex-direction: column;
    border-radius: 8px;
    padding: 16px 20px;
    gap: 2rem;

    @media (max-width:576px) {
        min-width: unset;
    }

    @media (min-width: 768px) {
        flex-direction: row;
        align-items: center;
        flex-wrap: wrap;
        justify-content: center;
    }

    .summary-visualization {
        position: relative;
        margin: 0 auto 20px;
        display: flex;
        justify-content: center;
        align-items: center;

        @media (min-width: 768px) {
            margin: 0 10px 0 10px;
            flex: 0 0 150px;
        }

        .pie-chart-wrapper {
            width: 150px;
            height: 150px;
        }

        .total-questions {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            text-align: center;
            max-width: 100px;
            height: 100px;
            display: flex;
            justify-content: center;
            align-items: center;
            flex-direction: column;
            background: white;
            border-radius: 50px;
            width: 100px;

            .count {
                display: block;
                font-size: 24px;
                font-weight: 600;
                color: @memo-grey-darker;
                line-height: 1.2;
            }

            .total-questions-label {
                display: block;
                font-size: 12px;
                color: @memo-grey-darker;
            }
        }
    }
}
</style>

<style lang="less">
.sidesheet-open {
    .knowledge-summary {
        @media (max-width:976px) {
            flex-direction: row;
            align-items: center;
            min-width: unset;
            flex-wrap: wrap;
            justify-content: center;

            .summary-visualization {
                position: relative;
                margin: 0 auto 20px;
            }
        }
    }
}
</style>