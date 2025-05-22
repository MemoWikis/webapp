<script setup lang="ts">
import { usePageStore } from '../../page/pageStore'
import { color } from '~~/components/shared/colors'

const pageStore = usePageStore()

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

const pageViewsPast90Days = computed(() => {
    return pageStore.viewsPast90DaysPage?.reduce((acc, v) => acc + v.count, 0) || 0
})

const questionViewsPast90Days = computed(() => {
    return pageStore.viewsPast90DaysDirectQuestions?.reduce((acc, v) => acc + v.count, 0) || 0
})

const { t } = useI18n()
</script>

<template>
    <div class="statistics-section" v-if="pageStore.analyticsLoaded">

        <LayoutCard>
            <div class="statistics-sub-label">
                {{ pageViewsPast90Days }} {{ t('page.analytics.pageViewsLast90Days') }}
            </div>
            <div class="statistics-container">
                <div class="statistics-content">
                    <div class="statistics-chart-section" :class="{ 'no-subpages': pageStore.childPageCount === 0 }">
                        <LazySharedChartsBar :labels="past90DaysLabelsPages" :datasets="past90DaysCountsPages" :color="color.middleBlue"
                            :title="t('page.analytics.withoutChildPages')" />
                    </div>

                    <div class="statistics-chart-section" v-if="pageStore.childPageCount > 0">
                        <LazySharedChartsBar :labels="past90DaysLabelsAggregatedPages" :datasets="past90DaysCountsAggregatedPages" :color="color.darkBlue"
                            :title="t('page.analytics.withChildPages', { count: pageStore.childPageCount })" />
                    </div>
                </div>
            </div>
        </LayoutCard>

        <LayoutCard v-if="pageStore.questionCount > 0">
            <div class="statistics-sub-label">
                {{ questionViewsPast90Days }} {{ t('page.analytics.questionViewsLast90Days') }}
            </div>
            <div class="statistics-container">
                <div class="statistics-content">
                    <div class="statistics-chart-section" :class="{ 'no-subpages': pageStore.childPageCount === 0 }">
                        <LazySharedChartsBar :labels="past90DaysLabelsQuestions" :datasets="past90DaysCountsQuestions" :color="color.memoGreen"
                            :title="t('page.analytics.directlyLinkedQuestionsTitle')" />
                    </div>

                    <div class="statistics-chart-section" v-if="pageStore.childPageCount > 0">
                        <LazySharedChartsBar :labels="past90DaysLabelsAggregatedQuestions" :datasets="past90DaysCountsAggregatedQuestions" :color="color.darkGreen"
                            :title="t('page.analytics.includedQuestions90Days', { count: pageStore.childPageCount })" />
                    </div>
                </div>
            </div>
        </LayoutCard>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.statistics-section {
    font-size: 1.8rem;
    width: 100%;

    .statistics-container {
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

                &.no-subpages {
                    width: 100%;
                }
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
