<script lang="ts" setup>
import { usePageStore } from '~/components/page/pageStore'
import { color } from '~/constants/colors'

const pageStore = usePageStore()
const { t } = useI18n()

const past90DaysCountsAggregatedQuestions = computed(() => pageStore.viewsPast90DaysAggregatedQuestions?.map(v => v.count) as number[])

const past90DaysLabelsQuestions = computed(() => pageStore.viewsPast90DaysDirectQuestions?.map(v => {
    const [year, month, day] = v.date.split("T")[0].split("-")
    return `${month}.${day}.`
}) as string[])

const past90DaysCountsQuestions = computed(() => pageStore.viewsPast90DaysDirectQuestions?.map(v => v.count) as number[])

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

</script>

<template>
    <LayoutCard :size="LayoutCardSize.Large">
        <PageAnalyticsAnaylticsChart :class="{ 'no-subpages': pageStore.childPageCount === 0 }">
            <LazySharedChartsBar
                :datasets="combinedPast90DaysDatasetsForQuestions"
                :max-ticks-limit="5"
                :labels="past90DaysLabelsQuestions"
                :stacked="true"
                :title="t('page.analytics.directlyLinkedQuestionsTitle')" />
        </PageAnalyticsAnaylticsChart>
    </LayoutCard>
</template>
