<script setup lang="ts">
import { ChartData } from '~~/components/chart/chartData'
import { useTopicStore } from '../topicStore'

const topicStore = useTopicStore()
const knowledgeSummaryData = ref<ChartData[]>([])
const showAnnualTopicBarchart = ref(false)

function setKnowledgeSummaryData() {

    knowledgeSummaryData.value = []
    for (const [key, value] of Object.entries(topicStore.knowledgeSummary)) {
        knowledgeSummaryData.value.push({
            value: value,
            class: key,
        })
    }
    knowledgeSummaryData.value = knowledgeSummaryData.value.slice().reverse()

}

function getLabel(key: string) {
    switch (key) {
        case 'solid':
            return `sicher gewusst`
        case 'needsConsolidation':
            return `zu festigen`
        case 'needsLearning':
            return `zu lernen`
        case 'notLearned':
            return `noch nicht gelernt`
    }
}

onBeforeMount(() => setKnowledgeSummaryData())

</script>

<template>
    <div class="row">
        <div class="col-xs-12">
            <div class="knowledgesummary-section">
                <h3>Dein Wissenstand</h3>
                <div class="knowledgesummary-container">
                    <div v-if="knowledgeSummaryData.some(d => d.value > 0)">
                        <div class="knowledgesummary-sub-label">
                            Fragen nach Status gruppiert
                        </div>
                        <div class="knowledgesummary-content">
                            <div v-for="d in knowledgeSummaryData" class="knowledgesummary-info" :key="d.value">
                                <div class="color-container" :class="`color-${d.class}`"></div>
                                <div class="knowledgesummary-label"><b>{{ d.value }}</b> {{ getLabel(d.class!) }}</div>
                            </div>
                        </div>

                    </div>

                    <div v-else>
                        Du hast noch keine Fragen in diesem Thema
                    </div>
                </div>
            </div>

            <div class="topicdata-section">
                <h3>Daten zum Thema</h3>
                <div class="topicdata-container">
                    <div class="topicdata-sub-label">
                        Fragen:
                    </div>
                    <div class="topicdata-content">
                        <ul>
                            <li>
                                <b>{{ topicStore.questionCount }}</b> eingeschlossene Fragen
                            </li>
                            <li>
                                <b>{{ topicStore.directQuestionCount }}</b> direkt verknüpfte Fragen
                            </li>
                        </ul>
                    </div>

                </div>
                <div class="topicdata-container">
                    <div class="topicdata-sub-label">
                        Themen:
                    </div>
                    <div class="topicdata-content">
                        <ul>
                            <li>
                                <b>{{ topicStore.childTopicCount }} </b> eingeschlossene Themen
                            </li>
                            <li>
                                <b>{{ topicStore.directVisibleChildTopicCount }}</b> direkt verknüpfte Unterthemen
                            </li>
                            <li>
                                <b> {{ topicStore.parentTopicCount }} </b> übergeordnete Themen
                            </li>
                        </ul>
                    </div>

                </div>
            </div>
            <div class="topicdata-section">
                <h3>Statistiken</h3>
                <div class="topicdata-container">
                    <div class="topicdata-sub-label">
                        Views:
                    </div>
                    <div class="topicdata-content">
                        <div v-if="showAnnualTopicBarchart">
                            <div class="row content">
                                <button @click="showAnnualTopicBarchart = !showAnnualTopicBarchart;">
                                    {{ showAnnualTopicBarchart ? 'Verstecken' : 'Jahresübersicht Logins' }}
                                </button>
                            </div>
                            <div v-if="showAnnualTopicBarchart">
                                <LazyOverviewBarChart :labels="annualLoginsLabels" :datasets="annualLoginsCount"
                                    :title="'Jahresübersicht Logins'" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.col-xs-12 {
    width: 100%;
}

.topicdata-section {
    margin-top: 36px;
}

.knowledgesummary-section,
.topicdata-section {
    margin-bottom: 40px;
    font-size: 18px;
    width: 100%;

    .knowledgesummary-container,
    .topicdata-container {


        .knowledgesummary-content,
        .topicdata-content {
            margin-bottom: 24px;
        }

        .knowledgesummary-sub-label,
        .topicdata-sub-label {
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

}
</style>