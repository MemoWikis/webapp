<script setup lang="ts">
import { ChartData } from '~~/components/chart/chartData'
import { useTopicStore } from '../topicStore'
import { useTabsStore, Tab } from './tabsStore'
import { color } from '~~/components/shared/colors'

const topicStore = useTopicStore()
const tabsStore = useTabsStore()
const knowledgeSummaryData = ref<ChartData[]>([])

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

const past90DaysLabelsAggregatedTopics = computed(() => topicStore.viewsPast90DaysAggregatedTopics?.map(v => {
    const [year, month, day] = v.date.split("T")[0].split("-")
    return `${year}-${month}-${day}`
}) as string[])
const past90DaysCountsAggregatedTopics = computed(() => topicStore.viewsPast90DaysAggregatedTopics?.map(v => v.count) as number[])

const past90DaysLabelsTopics = computed(() => topicStore.viewsPast90DaysTopic?.map(v => {
    const [year, month, day] = v.date.split("T")[0].split("-")
    return `${year}-${month}-${day}`
}) as string[])
const past90DaysCountsTopics = computed(() => topicStore.viewsPast90DaysTopic?.map(v => v.count) as number[])

const past90DaysLabelsAggregatedQuestions = computed(() => topicStore.viewsPast90DaysAggregatedQuestions?.map(v => {
    const [year, month, day] = v.date.split("T")[0].split("-")
    return `${year}-${month}-${day}`
}) as string[])
const past90DaysCountsAggregatedQuestions = computed(() => topicStore.viewsPast90DaysAggregatedQuestions?.map(v => v.count) as number[])

const past90DaysLabelsQuestions = computed(() => topicStore.viewsPast90DaysDirectQuestions?.map(v => {
    const [year, month, day] = v.date.split("T")[0].split("-")
    return `${year}-${month}-${day}`
}) as string[])
const past90DaysCountsQuestions = computed(() => topicStore.viewsPast90DaysDirectQuestions?.map(v => v.count) as number[])

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
onMounted(() => {
    if (import.meta.client) {
        watch(() => tabsStore.activeTab, (newTab) => {
            if (newTab === Tab.Analytics) {
                topicStore.getAnalyticsData()
            }
        })

        if (tabsStore.activeTab == Tab.Analytics) {
            topicStore.getAnalyticsData()
        }
    }

})

</script>

<template>
    <div class="row">
        <div class="col-xs-12">
            <div class="data-section">
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
            </div>
            <div class="statistics-section" v-if="topicStore.analyticsLoaded">
                <h3>Statistiken</h3>
                <div class="statistics-sub-label">
                    Themenaufrufe der letzten 90 Tage
                </div>
                <div class="statistics-container">
                    <div class="statistics-content">

                        <div class="statistics-chart-section">
                            <LazySharedChartsBar :labels="past90DaysLabelsTopics" :datasets="past90DaysCountsTopics" :color="color.middleBlue"
                                title="Ohne Unterthemen" />
                        </div>

                        <div class="statistics-chart-section">
                            <LazySharedChartsBar :labels="past90DaysLabelsAggregatedTopics" :datasets="past90DaysCountsAggregatedTopics" :color="color.darkBlue"
                                :title="`Inkl. ${topicStore.childTopicCount} Unterthemen`" />
                        </div>

                    </div>
                </div>
                <div class="statistics-sub-label">
                    Fragenaufrufe der letzten 90 Tage
                </div>
                <div class="statistics-container">
                    <div class="statistics-content">

                        <div class="statistics-chart-section">
                            <LazySharedChartsBar :labels="past90DaysLabelsQuestions" :datasets="past90DaysCountsQuestions" :color="color.memoGreen"
                                :title="`Direkt verknüpfte Fragen`" />
                        </div>

                        <div class="statistics-chart-section">
                            <LazySharedChartsBar :labels="past90DaysLabelsAggregatedQuestions" :datasets="past90DaysCountsAggregatedQuestions" :color="color.darkGreen"
                                :title="`Eingeschlossene Fragen (${topicStore.childTopicCount} Unterthemen)`" />
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

.knowledgesummary-section,
.topicdata-section,
.statistics-section {
    margin-bottom: 40px;
    font-size: 18px;
    width: 100%;

    .knowledgesummary-container,
    .topicdata-container,
    .statistics-container {

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

.statistics-content {
    display:flex;
    flex-wrap: wrap;
    margin-bottom: 60px;

    .statistics-chart-section {
        width: 50%;
        position: relative; 
        height: 100%;
        min-height: 300px;
        @media screen and (max-width: 991px) {
            width: 100%;
        }
    }
}

.statistics-sub-label{
    margin-bottom: 16px;
    text-align: center;
    margin-top: 24px;
}

.data-section {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
}

.knowledgesummary-section,
.topicdata-section {
    width: 50%; 
        @media screen and (max-width: 991px) {
            width: 100%;
        }
    }

h3 {
    margin-top: 36px;
}
</style>