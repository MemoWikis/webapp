<script setup lang="ts">
import { ActivityCalendarData } from '~/composables/missionControl/learnCalendar'
import { usePageStore } from '../pageStore'
import { useTabsStore, Tab } from './tabsStore'
import PageAnalytics from '~/constants/pageAnalyticsSections'

const pageStore = usePageStore()
const tabsStore = useTabsStore()
const { t } = useI18n()

onMounted(() => {
    if (import.meta.client) {
        watch(() => tabsStore.activeTab, (newTab) => {
            if (newTab === Tab.Analytics) {
                pageStore.getAnalyticsData()
            }
        })

        if (tabsStore.activeTab === Tab.Analytics) {
            pageStore.getAnalyticsData()
        }
    }
})

const activityCalendar = ref<ActivityCalendarData | null>(null)

const getActivityCalendar = async () => {
    const result = await $api<ActivityCalendarData>(`/apiVue/MissionControl/GetPageActivityCalendar/${pageStore.id}`)
    if (result) {
        activityCalendar.value = result
    }
}

onBeforeMount(() => {
    getActivityCalendar()
})

</script>

<template>
    <div class="analytics">
        <LayoutPanel :title="t(PageAnalytics.KNOWLEDGE_SECTION.translationKey)"
            :id="PageAnalytics.KNOWLEDGE_SECTION.id">
            <LayoutCard class="analytics-knowledgesummary-section" :size="LayoutCardSize.Flex">
                <PageAnalyticsKnowledgeSummarySection />
            </LayoutCard>
        </LayoutPanel>

        <!-- LearnCalendar Section -->
        <LayoutPanel :title="t(PageAnalytics.LEARN_CALENDAR_SECTION.translationKey)"
            :id="PageAnalytics.LEARN_CALENDAR_SECTION.id">
            <LayoutCard>
                <MissionControlLearnCalendar v-if="activityCalendar" :calendarData="activityCalendar" />
            </LayoutCard>
        </LayoutPanel>

        <LayoutPanel :title="t(PageAnalytics.CONTENT_SECTION.translationKey)" :id="PageAnalytics.CONTENT_SECTION.id">
            <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.hasVisibleDirectChildren">
                <LayoutCounter :value="pageStore.directVisibleChildPageCount"
                    :label="t('page.analytics.directVisibleChildPageLabel')" />
            </LayoutCard>

            <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.childPageCount > 0">
                <LayoutCounter :value="pageStore.childPageCount" :label="t('page.analytics.childPageCount')" />
            </LayoutCard>

            <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.directQuestionCount > 0">
                <LayoutCounter :value="pageStore.directQuestionCount"
                    :label="t('page.analytics.directlyLinkedQuestionsLabel')" />
            </LayoutCard>

            <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.questionCount > 0">
                <LayoutCounter :value="pageStore.questionCount" :label="t('page.analytics.includedQuestionsLabel')" />
            </LayoutCard>

            <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.parentPageCount > 0">
                <LayoutCounter :value="pageStore.parentPageCount" :label="t('page.analytics.parentPageLabel')" />
            </LayoutCard>


        </LayoutPanel>

        <LayoutPanel :title="t(PageAnalytics.VIEWS_SECTION.translationKey)" :id="PageAnalytics.VIEWS_SECTION.id">
            <LayoutCard :size="LayoutCardSize.Tiny">
                <LayoutCounter :value="pageStore.views" :label="pageStore.name" />
            </LayoutCard>

            <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.childPageCount > 0">
                <LayoutCounter :value="pageStore.subpageViews" :label="t('page.analytics.subpageViewsLabel')" />
            </LayoutCard>

            <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.questionCount > 0">
                <LayoutCounter :value="pageStore.directQuestionViews"
                    :label="t('page.analytics.directQuestionViewsLabel')" />
            </LayoutCard>

            <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.totalQuestionViews">
                <LayoutCounter :value="pageStore.totalQuestionViews"
                    :label="t('page.analytics.aggregatedQuestionViewsLabel')" />
            </LayoutCard>
        </LayoutPanel>

        <template v-if="pageStore.analyticsLoaded">
            <LayoutPanel :title="t(PageAnalytics.PAGE_VIEWS_SECTION.translationKey)"
                :id="PageAnalytics.PAGE_VIEWS_SECTION.id">
                <PageAnalyticsPageViewChart />
            </LayoutPanel>

            <LayoutPanel v-if="pageStore.questionCount > 0"
                :title="t(PageAnalytics.QUESTION_VIEWS_SECTION.translationKey)"
                :id="PageAnalytics.QUESTION_VIEWS_SECTION.id">
                <PageAnalyticsQuestionViewChart />
            </LayoutPanel>
        </template>



    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.analytics {
    margin-top: 20px;
}

.col-xs-12 {
    width: 100%;
}

.data-section {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
}

h3 {
    margin-top: 36px;
}
</style>

<style lang="less">
.sidesheet-open {
    .analytics-knowledgesummary-section {
        .knowledgesummary-container {
            .knowledgesummary-content {
                @media (max-width:1150px) {
                    flex-direction: column;
                }
            }
        }
    }
}

.analytics {

    h1,
    h2,
    h3,
    h4 {
        scroll-margin-top: 10rem;
    }
}
</style>