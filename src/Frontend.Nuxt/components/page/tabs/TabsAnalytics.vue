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
                    <i18n-t keypath="page.analytics.includedQuestions" tag="span" :plural="pageStore.questionCount">
                        <template #count>
                            <b>{{ pageStore.questionCount }}</b>
                        </template>
                    </i18n-t>
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Tiny">
                    <i18n-t keypath="page.analytics.directlyLinkedQuestions" tag="span" :plural="pageStore.directQuestionCount">
                        <template #count>
                            <b>{{ pageStore.directQuestionCount }}</b>
                        </template>
                    </i18n-t>
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