<script setup lang="ts">
import { ChartData } from '~~/components/chart/chartData'
import { usePageStore } from '../pageStore'
import { useTabsStore, Tab } from './tabsStore'
import { color } from '~~/components/shared/colors'

const pageStore = usePageStore()
const tabsStore = useTabsStore()
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

const past90DaysLabelsAggregatedPages = computed(() => pageStore.viewsPast90DaysAggregatedPages?.map(v => {
    const [year, month, day] = v.date.split("T")[0].split("-")
    return `${year}-${month}-${day}`
}) as string[])
const past90DaysCountsAggregatedPages = computed(() => pageStore.viewsPast90DaysAggregatedPages?.map(v => v.count) as number[])

const past90DaysLabelsPages = computed(() => pageStore.viewsPast90DaysPage?.map(v => {
    const [year, month, day] = v.date.split("T")[0].split("-")
    return `${year}-${month}-${day}`
}) as string[])
const past90DaysCountsPages = computed(() => pageStore.viewsPast90DaysPage?.map(v => v.count) as number[])

const past90DaysLabelsAggregatedQuestions = computed(() => pageStore.viewsPast90DaysAggregatedQuestions?.map(v => {
    const [year, month, day] = v.date.split("T")[0].split("-")
    return `${year}-${month}-${day}`
}) as string[])
const past90DaysCountsAggregatedQuestions = computed(() => pageStore.viewsPast90DaysAggregatedQuestions?.map(v => v.count) as number[])

const past90DaysLabelsQuestions = computed(() => pageStore.viewsPast90DaysDirectQuestions?.map(v => {
    const [year, month, day] = v.date.split("T")[0].split("-")
    return `${year}-${month}-${day}`
}) as string[])
const past90DaysCountsQuestions = computed(() => pageStore.viewsPast90DaysDirectQuestions?.map(v => v.count) as number[])

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
                pageStore.getAnalyticsData()
            }
        })

        if (tabsStore.activeTab == Tab.Analytics) {
            pageStore.getAnalyticsData()
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
                            Du hast noch keine Fragen auf dieser Seite beantwortet.
                        </div>
                    </div>
                </div>

                <div class="pagedata-section">
                    <h3>Daten zu dieser Seite</h3>
                    <div class="pagedata-container">
                        <div class="pagedata-sub-label">
                            Fragen:
                        </div>
                        <div class="pagedata-content">
                            <ul>
                                <li>
                                    <b>{{ pageStore.questionCount }}</b> eingeschlossene Fragen
                                </li>
                                <li>
                                    <b>{{ pageStore.directQuestionCount }}</b> direkt verknüpfte Fragen
                                </li>
                            </ul>
                        </div>

                    </div>
                    <div class="pagedata-container">
                        <div class="pagedata-sub-label">
                            Seiten:
                        </div>
                        <div class="pagedata-content">
                            <ul>
                                <li>
                                    <b>{{ pageStore.childPageCount }} </b> eingeschlossene Seiten
                                </li>
                                <li>
                                    <b>{{ pageStore.directVisibleChildPageCount }}</b> direkt verknüpfte UnterSeiten
                                </li>
                                <li>
                                    <b> {{ pageStore.parentPageCount }} </b> übergeordnete Seiten
                                </li>
                            </ul>
                        </div>

                    </div>
                </div>
            </div>
            <div class="statistics-section" v-if="pageStore.analyticsLoaded">
                <h3>Statistiken</h3>
                <div class="statistics-sub-label">
                    Seitenaufrufe der letzten 90 Tage
                </div>
                <div class="statistics-container">
                    <div class="statistics-content">

                        <div class="statistics-chart-section">
                            <LazySharedChartsBar :labels="past90DaysLabelsPages" :datasets="past90DaysCountsPages" :color="color.middleBlue"
                                title="Ohne UnterSeiten" />
                        </div>

                        <div class="statistics-chart-section">
                            <LazySharedChartsBar :labels="past90DaysLabelsAggregatedPages" :datasets="past90DaysCountsAggregatedPages" :color="color.darkBlue"
                                :title="`Inkl. ${pageStore.childPageCount} UnterSeiten`" />
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
                                :title="`Eingeschlossene Fragen (${pageStore.childPageCount} UnterSeiten)`" />
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
.pagedata-section,
.statistics-section {
    margin-bottom: 40px;
    font-size: 18px;
    width: 100%;

    .knowledgesummary-container,
    .pagedata-container,
    .statistics-container {

        .knowledgesummary-content,
        .pagedata-content {
            margin-bottom: 24px;
        }

        .knowledgesummary-sub-label,
        .pagedata-sub-label {
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
    display: flex;
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

.statistics-sub-label {
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
.pagedata-section {
    width: 50%;

    @media screen and (max-width: 991px) {
        width: 100%;
    }
}

h3 {
    margin-top: 36px;
}
</style>