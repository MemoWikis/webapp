<script setup lang="ts">
import { usePageStore } from '../pageStore'
import { useTabsStore, Tab } from './tabsStore'

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
</script>

<template>
    <div class="row analytics">
        <div class="col-xs-12">
            <LayoutPanel :title="t('page.analytics.yourKnowledgeStatus')">
                <LayoutCard>
                    <PageAnalyticsKnowledgeSummarySection />
                </LayoutCard>
            </LayoutPanel>
            <LayoutPanel :title="t('page.analytics.pageData')">
                <LayoutCard :size="LayoutCardSize.Tiny">
                    <LayoutCounter
                        :value="pageStore.questionCount"
                        :label="t('page.analytics.includedQuestionsLabel')" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Tiny">
                    <LayoutCounter
                        :value="pageStore.directQuestionCount"
                        :label="t('page.analytics.directlyLinkedQuestionsLabel')" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.views">
                    <LayoutCounter
                        :value="pageStore.views"
                        :label="t('page.analytics.pageViewsLabel')" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Tiny" v-if="pageStore.views">
                    <LayoutCounter
                        :value="213481912"
                        :label="t('page.analytics.pageViewsLabel')" />
                </LayoutCard>
            </LayoutPanel>

            <!-- <LayoutPanel :title="t('page.analytics.statistics')" v-if="pageStore.analyticsLoaded">
                <PageAnalyticsStatisticsSection />
            </LayoutPanel> -->

            <PageAnalyticsStatisticsSection v-if="pageStore.analyticsLoaded" />

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
</style>