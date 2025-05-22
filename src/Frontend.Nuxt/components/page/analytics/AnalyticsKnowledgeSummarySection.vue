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
                    <SharedKnowledgeSummary :knowledgeStatus="pageStore.knowledgeSummarySlim" />
                </div>
            </div>

            <div v-else>
                {{ t('page.analytics.noQuestionsAnswered') }}
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.knowledgesummary-section {
    margin-bottom: 40px;
    font-size: 18px;

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
        .knowledgesummary-content {
            margin-bottom: 24px;
        }

        .knowledgesummary-sub-label {
            margin-bottom: 16px;
            font-size: 1em;
            color: @memo-grey-dark;
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
    }

    @media screen and (max-width: 991px) {
        width: 100%;
    }
}

h3 {
    margin-top: 36px;
}
</style>
