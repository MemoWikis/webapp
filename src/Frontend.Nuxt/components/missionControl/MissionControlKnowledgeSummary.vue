<script lang="ts" setup>
import { KnowledgebarData } from '~/components/page/content/grid/knowledgebar/knowledgebarData'
import { ChartData } from '~/components/chart/chartData'

interface Props {
    knowledgeStatus: KnowledgebarData
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

const knowledgeStatusItems = computed(() => [
    {
        label: t('knowledgeStatus.solid'),
        value: props.knowledgeStatus.solid,
        percentage: props.knowledgeStatus.solidPercentage,
        class: 'solid'
    },
    {
        label: t('knowledgeStatus.needsConsolidation'),
        value: props.knowledgeStatus.needsConsolidation,
        percentage: props.knowledgeStatus.needsConsolidationPercentage,
        class: 'needsConsolidation'
    },
    {
        label: t('knowledgeStatus.needsLearning'),
        value: props.knowledgeStatus.needsLearning,
        percentage: props.knowledgeStatus.needsLearningPercentage,
        class: 'needsLearning'
    },
    {
        label: t('knowledgeStatus.notLearned'),
        value: props.knowledgeStatus.notLearned,
        percentage: props.knowledgeStatus.notLearnedPercentage,
        class: 'notLearned'
    }
])
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
        <div class="summary-details">
            <div
                v-for="(item, index) in knowledgeStatusItems"
                :key="index"
                class="status-item">
                <div class="status-info">
                    <span class="status-dot" :class="`dot-${item.class}`"></span>
                    <span class="status-label">{{ item.label }}</span>
                </div>
                <div class="status-value">
                    <span class="value">{{ item.value }}</span>
                    <span class="percentage">({{ item.percentage }}%)</span>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.knowledge-summary {
    display: flex;
    flex-direction: column;
    width: 100%;
    min-width: 500px;
    background: white;
    border-radius: 8px;
    padding: 16px 20px;

    @media (max-width:576px) {
        min-width: unset;
    }

    @media (min-width: 768px) {
        flex-direction: row;
        align-items: center;
        width: 50%;
        flex-wrap: wrap;
        justify-content: center;
    }

    .summary-visualization {
        position: relative;
        margin: 0 auto 20px;

        @media (min-width: 768px) {
            margin: 0 40px 0 0;
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

    .summary-details {
        flex: 1;

        .status-item {
            display: flex;
            justify-content: space-between;
            margin-bottom: 12px;

            &:last-child {
                margin-bottom: 0;
            }

            .status-info {
                display: flex;
                align-items: center;

                .status-dot {
                    width: 12px;
                    height: 12px;
                    border-radius: 50%;
                    margin-right: 8px;

                    &.dot-solid {
                        background-color: @memo-green;
                    }

                    &.dot-needsConsolidation {
                        background-color: @memo-yellow;
                    }

                    &.dot-needsLearning {
                        background-color: @memo-salmon;
                    }

                    &.dot-notLearned {
                        background-color: @memo-grey-light;
                    }
                }

                .status-label {
                    font-size: 14px;
                    color: @memo-grey-dark;
                }
            }

            .status-value {
                font-size: 14px;

                .value {
                    font-weight: 600;
                    color: @memo-grey-darker;
                }

                .percentage {
                    margin-left: 4px;
                    color: @memo-grey-dark;
                }
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
            width: 100%;
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