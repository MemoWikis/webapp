<script setup lang="ts">
import { ChartData } from '~~/components/chart/chartData'
import { usePageStore } from '../../page/pageStore'

const pageStore = usePageStore()
const knowledgeSummaryData = ref<ChartData[]>([])

function setKnowledgeSummaryData() {
    knowledgeSummaryData.value = []
    for (const [key, value] of Object.entries(pageStore.knowledgeSummarySlim)) {
        knowledgeSummaryData.value.push({
            value: value,
            class: key,
        })
    }
    knowledgeSummaryData.value = knowledgeSummaryData.value.slice().reverse()
}

const { t } = useI18n()

onBeforeMount(() => setKnowledgeSummaryData())
</script>

<template>
    <div class="knowledgesummary-section">
        <div class="knowledgesummary-container">
            <div v-if="knowledgeSummaryData.some(d => d.value > 0)">
                <div class="knowledgesummary-sub-label">
                    {{ t('page.analytics.questionsGroupedByStatus') }}
                </div>
                <div class="knowledgesummary-content">
                    <SharedKnowledgeSummaryPie :knowledge-summary="pageStore.knowledgeSummary" />
                    <SharedKnowledgeSummary :knowledge-summary="pageStore.knowledgeSummary" />
                </div>
            </div>

            <div v-else class="knowledgesummary-no-questions-answered">
                {{ t('page.analytics.noQuestionsAnswered') }}
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.knowledgesummary-section {

    .knowledgesummary-section-header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 16px 20px;

        .knowledgesummary-section-title {
            font-size: 18px;
            font-weight: 600;
            color: @memo-grey-dark;
            margin: 0;
        }
    }

    .knowledgesummary-container {

        .knowledgesummary-sub-label {
            font-size: 1.6rem;
            color: @memo-grey-darker;
            font-weight: 600;
        }

        .knowledgesummary-info {
            display: flex;
            flex-wrap: nowrap;
            align-items: center;
            padding: 4px 0;

            .color-container {
                height: 24px;
                width: 24px;
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

            .knowledgesummary-label {
                padding-left: 20px;
            }
        }

        .knowledgesummary-no-questions-answered {
            font-size: 1.4rem;
            color: @memo-grey-dark;
            padding: 20px 0;
        }

        .knowledgesummary-content {
            display: flex;
            flex-direction: row;
            border-radius: 8px;
            padding: 16px 20px;
            gap: 2rem;

            // @media (max-width:576px) {
            //     min-width: unset;
            // }

            // @media (min-width: 768px) {
            //     flex-direction: row;
            //     align-items: center;
            //     flex-wrap: wrap;
            //     justify-content: center;
            // }
        }
    }

    @media screen and (max-width: 991px) {
        width: 100%;
    }
}

h3 {
    margin-top: 36px;
}
</style>
