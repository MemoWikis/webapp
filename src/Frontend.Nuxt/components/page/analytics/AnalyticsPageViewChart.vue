<script lang="ts" setup>
import { usePageStore } from '~/components/page/pageStore'
import { color } from '~/constants/colors'

const pageStore = usePageStore()
const { t } = useI18n()

const past90DaysLabelsPage = computed(() => pageStore.viewsPast90DaysPage?.map(v => {
    const [month, day] = v.date.split("T")[0].split("-")
    return `${month}.${day}.`
}) as string[])

const past90DaysCountsAggregatedPages = computed(() => pageStore.viewsPast90DaysAggregatedPages?.map(v => v.count) as number[])

const past90DaysCountsPage = computed(() => pageStore.viewsPast90DaysPage?.map(v => v.count) as number[])

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
</script>

<template>
    <LayoutCard :size="LayoutCardSize.Large">
        <PageAnalyticsAnaylticsChart :class="{ 'no-subpages': pageStore.childPageCount === 0 }">
            <LazySharedChartsBar :datasets="combinedPast90DaysDatasetsForPage" :max-ticks-limit="5" :labels="past90DaysLabelsPage" :stacked="true" :title="t('page.analytics.viewDistributionTitle')" />
        </PageAnalyticsAnaylticsChart>
    </LayoutCard>
</template>