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
            <LayoutPanel>
                <LayoutCard :full-width="false" :title="t('page.analytics.yourKnowledgeStatus')">
                    <PageAnalyticsKnowledgeSummarySection />
                </LayoutCard>

                <LayoutCard :full-width="false" :title="t('page.analytics.pageData')">
                    <PageAnalyticsPageData />
                </LayoutCard>

            </LayoutPanel>

            <LayoutPanel :title="t('page.analytics.statistics')">
                <PageAnalyticsStatisticsSection />
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
</style>