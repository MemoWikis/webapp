<script setup lang="ts">
import { ChartData } from '~~/components/chart/chartData'
import { usePageStore } from '../../page/pageStore'

const pageStore = usePageStore()
const knowledgeSummaryData = ref<ChartData[]>([])

function setKnowledgeSummaryData() {
    knowledgeSummaryData.value = []
    for (const [key, value] of Object.entries(pageStore.knowledgeSummary)) {
        knowledgeSummaryData.value.push({
            value: value,
            class: key,
        })
    }
    knowledgeSummaryData.value = knowledgeSummaryData.value.slice().reverse()
}

const { t } = useI18n()

function getLabel(key: string) {
    switch (key) {
        case 'solid':
            return t('knowledgeStatus.solid')
        case 'needsConsolidation':
            return t('knowledgeStatus.needsConsolidation')
        case 'needsLearning':
            return t('knowledgeStatus.needsLearning')
        case 'notLearned':
            return t('knowledgeStatus.notLearned')
    }
}

onBeforeMount(() => setKnowledgeSummaryData())
</script>

<template>
    <div class="knowledgesummary-section">
        <h3>{{ t('page.analytics.yourKnowledgeStatus') }}</h3>
        <div class="knowledgesummary-container">
            <div v-if="knowledgeSummaryData.some(d => d.value > 0)">
                <div class="knowledgesummary-sub-label">
                    {{ t('page.analytics.questionsGroupedByStatus') }}
                </div>
                <div class="knowledgesummary-content">
                    <div v-for="d in knowledgeSummaryData" class="knowledgesummary-info" :key="d.value">
                        <div class="color-container" :class="`color-${d.class}`"></div>
                        <div class="knowledgesummary-label"><b>{{ d.value }}</b> {{ getLabel(d.class!) }}</div>
                    </div>
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
    width: 50%;

    .knowledgesummary-container {
        .knowledgesummary-content {
            margin-bottom: 24px;
        }

        .knowledgesummary-sub-label {
            margin-bottom: 16px;
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
