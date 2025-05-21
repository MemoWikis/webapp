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
    <div class="row">
        <div class="col-xs-12">
            <div class="data-section">
                <PageAnalyticsKnowledgeSummarySection />
                <PageAnalyticsPageData />
            </div>
            <PageAnalyticsStatisticsSection />
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

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