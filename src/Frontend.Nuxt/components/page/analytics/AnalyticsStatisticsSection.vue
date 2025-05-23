<script setup lang="ts">
import { usePageStore } from '../../page/pageStore'
import { color } from '~~/components/shared/colors'
import { LayoutCardSize } from '~~/composables/layoutCardSize'

const pageStore = usePageStore()

const past90DaysLabelsAggregatedPages = computed(() => pageStore.viewsPast90DaysAggregatedPages?.map(v => {
    const [year, month, day] = v.date.split("T")[0].split("-")
    return `${month}.${day}.`
}) as string[])
const past90DaysCountsAggregatedPages = computed(() => pageStore.viewsPast90DaysAggregatedPages?.map(v => v.count) as number[])
const aggregatedPagesDatasets = computed(() => {
    return [
        {
            label: t('page.analytics.withChildPages', { count: pageStore.childPageCount }),
            data: past90DaysCountsAggregatedPages.value,
            backgroundColor: color.darkBlue,
        },
    ]
})

const past90DaysLabelsPage = computed(() => pageStore.viewsPast90DaysPage?.map(v => {
    const [year, month, day] = v.date.split("T")[0].split("-")
    return `${month}.${day}.`
}) as string[])
const past90DaysCountsPage = computed(() => pageStore.viewsPast90DaysPage?.map(v => v.count) as number[])
const pagesDatasets = computed(() => {
    return [
        {
            label: t('page.analytics.withoutChildPages'),
            data: past90DaysCountsPage.value,
            backgroundColor: color.middleBlue,
        },
    ]
})

const past90DaysLabelsAggregatedQuestions = computed(() => pageStore.viewsPast90DaysAggregatedQuestions?.map(v => {
    const [year, month, day] = v.date.split("T")[0].split("-")
    return `${month}.${day}.`
}) as string[])
const past90DaysCountsAggregatedQuestions = computed(() => pageStore.viewsPast90DaysAggregatedQuestions?.map(v => v.count) as number[])
const aggregatedQuestionsDatasets = computed(() => {
    return [
        {
            label: t('page.analytics.includedQuestions90Days', { count: pageStore.childPageCount }),
            data: past90DaysCountsAggregatedQuestions.value,
            backgroundColor: color.darkGreen,
        },
    ]
})

const past90DaysLabelsQuestions = computed(() => pageStore.viewsPast90DaysDirectQuestions?.map(v => {
    const [year, month, day] = v.date.split("T")[0].split("-")
    return `${month}.${day}.`
}) as string[])
const past90DaysCountsQuestions = computed(() => pageStore.viewsPast90DaysDirectQuestions?.map(v => v.count) as number[])
const questionsDatasets = computed(() => {
    return [
        {
            label: t('page.analytics.directlyLinkedQuestionsTitle'),
            data: past90DaysCountsQuestions.value,
            backgroundColor: color.memoGreen,
        },
    ]
})

const pageViewsPast90Days = computed(() => {
    return pageStore.viewsPast90DaysPage?.reduce((acc, v) => acc + v.count, 0) || 0
})

const questionViewsPast90Days = computed(() => {
    return pageStore.viewsPast90DaysDirectQuestions?.reduce((acc, v) => acc + v.count, 0) || 0
})

const combinedPast90DaysDatasetsForPage = computed(() => {
    // Calculate views for child pages only by subtracting current page views from aggregated views
    const childPagesOnlyData = past90DaysCountsAggregatedPages.value && past90DaysCountsPage.value ?
        past90DaysCountsAggregatedPages.value.map((count, index) => {
            // Make sure we don't get negative values if data is inconsistent
            return Math.max(0, count - (past90DaysCountsPage.value[index] || 0))
        }) : []

    return [
        {
            label: pageStore.name,
            data: past90DaysCountsPage.value,
            backgroundColor: color.darkBlue,
        },
        {
            label: t('page.analytics.withChildPages', { count: pageStore.childPageCount }),
            data: childPagesOnlyData,
            backgroundColor: color.middleBlue,
        }
    ]
})

const combinedPast90DaysDatasetsForQuestions = computed(() => {
    // Calculate views for child pages only by subtracting current page views from aggregated views
    const childPagesOnlyData = past90DaysCountsAggregatedQuestions.value && past90DaysCountsQuestions.value ?
        past90DaysCountsAggregatedQuestions.value.map((count, index) => {
            // Make sure we don't get negative values if data is inconsistent
            return Math.max(0, count - (past90DaysCountsQuestions.value[index] || 0))
        }) : []

    return [
        {
            label: t('page.analytics.directlyLinkedQuestionsTitle'),
            data: past90DaysCountsQuestions.value,
            backgroundColor: color.darkGreen,
        },
        {
            label: t('page.analytics.includedQuestions90Days', { count: pageStore.childPageCount }),
            data: childPagesOnlyData,
            backgroundColor: color.memoGreen,
        }
    ]
})

const { t } = useI18n()
</script>

<template>
    <div class="statistics-section" v-if="pageStore.analyticsLoaded">

        <!-- <div class="statistics-sub-label">
            {{ pageViewsPast90Days }} {{ t('page.analytics.pageViewsLast90Days') }}
        </div> -->

        <LayoutPanel :title="t('page.analytics.pageViewsLast90Days')">
            <div class="statistics-container">
                <div class="statistics-content">
                    <LayoutCard :size="LayoutCardSize.Large">
                        <div class="statistics-chart-section" :class="{ 'no-subpages': pageStore.childPageCount === 0 }">
                            <LazySharedChartsBar
                                :datasets="combinedPast90DaysDatasetsForPage"
                                :max-ticks-limit="5"
                                :labels="past90DaysLabelsPage"
                                :stacked="true"
                                :title="t('page.analytics.viewDistributionTitle')" />
                        </div>
                    </LayoutCard>
                </div>
            </div>
        </LayoutPanel>


        <LayoutPanel v-if="pageStore.questionCount > 0" :title="t('page.analytics.questionViewsLast90Days')">
            <!-- <div class="statistics-sub-label">
                {{ questionViewsPast90Days }} {{ t('page.analytics.questionViewsLast90Days') }}
            </div> -->
            <div class="statistics-container">
                <div class="statistics-content">
                    <LayoutCard :size="LayoutCardSize.Large">
                        <div class="statistics-chart-section" :class="{ 'no-subpages': pageStore.childPageCount === 0 }">

                            <LazySharedChartsBar
                                :datasets="combinedPast90DaysDatasetsForQuestions"
                                :max-ticks-limit="5"
                                :labels="past90DaysLabelsQuestions"
                                :stacked="true"
                                :title="t('page.analytics.directlyLinkedQuestionsTitle')" />
                        </div>
                    </LayoutCard>
                </div>
            </div>
        </LayoutPanel>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.statistics-section {
    font-size: 1.8rem;
    width: 100%;

    .statistics-container {
        width: 100%;

        .statistics-content {
            display: flex;
            flex-wrap: wrap;
            gap: 1rem;

            .statistics-chart-section {
                width: 100%;
                position: relative;
                height: 100%;
                min-height: 300px;
            }
        }
    }
}

.statistics-sub-label {
    margin-bottom: 16px;
    text-align: center;
    margin-top: 24px;
    font-size: 1.6rem;
    font-weight: 600;
    color: @memo-grey-dark;
}

h3 {
    margin-top: 36px;
}
</style>
