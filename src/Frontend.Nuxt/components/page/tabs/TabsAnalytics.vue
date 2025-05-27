<script setup lang="ts">
import { ActivityCalendarData } from '~/composables/missionControl/learnCalendar'
import { usePageStore } from '../pageStore'
import { useTabsStore, Tab } from './tabsStore'
import PageAnalytics from '~/constants/pageAnalyticsSections'
import { P } from 'pino'

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

const mockActivity = ref<ActivityCalendarData | null>(null)

const getMockActivityCalendar = async () => {
    const result = await $api<ActivityCalendarData>('/apiVue/MissionControl/GetMockActivityCalendar')
    if (result)
        mockActivity.value = result
}

onBeforeMount(() => {
    getMockActivityCalendar()
})

</script>

<template>
    <div class="row analytics">
        <div class="col-xs-12">
            <LayoutPanel :title="t(PageAnalytics.KNOWLEDGE_SECTION.translationKey)" :id="PageAnalytics.KNOWLEDGE_SECTION.id">
                <LayoutCard class="analytics-knowledgesummary-section">
                    <PageAnalyticsKnowledgeSummarySection />
                </LayoutCard>
            </LayoutPanel>

            <LayoutPanel :title="t(PageAnalytics.CONTENT_SECTION.translationKey)" :id="PageAnalytics.CONTENT_SECTION.id">
                <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.childPageCount > 0">
                    <LayoutCounter
                        :value="pageStore.childPageCount"
                        :label="t('page.analytics.childPageCount')" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.directVisibleChildPageCount > 0">
                    <LayoutCounter
                        :value="pageStore.directVisibleChildPageCount"
                        :label="t('page.analytics.directVisibleChildPageLabel')" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.parentPageCount > 0">
                    <LayoutCounter
                        :value="pageStore.parentPageCount"
                        :label="t('page.analytics.parentPageLabel')" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.questionCount > 0">
                    <LayoutCounter
                        :value="pageStore.questionCount"
                        :label="t('page.analytics.includedQuestionsLabel')" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.directQuestionCount > 0">
                    <LayoutCounter
                        :value="pageStore.directQuestionCount"
                        :label="t('page.analytics.directlyLinkedQuestionsLabel')" />
                </LayoutCard>
            </LayoutPanel>

            <LayoutPanel :title="t(PageAnalytics.VIEWS_SECTION.translationKey)" :id="PageAnalytics.VIEWS_SECTION.id">
                <LayoutCard :size="LayoutCardSize.Tiny">
                    <LayoutCounter
                        :value="pageStore.views"
                        :label="t('page.analytics.pageViewsLabel')" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.childPageCount > 0">
                    <LayoutCounter
                        :value="pageStore.views"
                        :label="t('page.analytics.pageViewsLabel')" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.questionCount > 0">
                    <LayoutCounter
                        :value="pageStore.views"
                        :label="t('page.analytics.directlyLinkedQuestionsLabel')" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.views">
                    <LayoutCounter
                        :value="pageStore.views"
                        :label="t('page.analytics.pageViewsLabel')" />
                </LayoutCard>
            </LayoutPanel>

            <template v-if="pageStore.analyticsLoaded">
                <LayoutPanel :title="t(PageAnalytics.PAGE_VIEWS_SECTION.translationKey)" :id="PageAnalytics.PAGE_VIEWS_SECTION.id">
                    <PageAnalyticsPageViewChart />
                </LayoutPanel>

                <LayoutPanel v-if="pageStore.questionCount > 0" :title="t(PageAnalytics.QUESTION_VIEWS_SECTION.translationKey)" :id="PageAnalytics.QUESTION_VIEWS_SECTION.id">
                    <PageAnalyticsQuestionViewChart />
                </LayoutPanel>
            </template>

            <!-- LearnCalendar Section with Coming Soon overlay -->
            <LayoutPanel :title="t(PageAnalytics.LEARN_CALENDAR_SECTION.translationKey)" :id="PageAnalytics.LEARN_CALENDAR_SECTION.id">
                <div class="coming-soon-container">
                    <MissionControlLearnCalendar v-if="mockActivity" :calendarData="mockActivity" />
                    <div class="coming-soon-overlay">
                        <div class="coming-soon-content">
                            <div class="coming-soon-text">{{ t('general.comingSoon') }}</div>
                        </div>
                    </div>
                </div>
            </LayoutPanel>

        </div>
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

.coming-soon-container {
    position: relative;
    width: 100%;

    .coming-soon-overlay {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(255, 255, 255, 0.85);
        display: flex;
        justify-content: center;
        align-items: center;
        border-radius: 8px;
        z-index: 10;
    }

    .coming-soon-content {
        text-align: center;
        padding: 20px;
        border-radius: 8px;
        background-color: white;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
        display: flex;
    }

    .coming-soon-text {
        font-size: 24px;
        font-weight: 600;
        color: @memo-blue;
    }
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
</style>