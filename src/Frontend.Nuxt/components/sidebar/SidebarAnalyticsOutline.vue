<script lang="ts" setup>
import { PAGE_ANALYTICS_SECTIONS } from '~/constants/pageAnalyticsSections'
</script>

<template>
    <SidebarOutline
        :sections="PAGE_ANALYTICS_SECTIONS"
        container-id="AnalyticsOutline" />
</template>

<style lang="less" scoped>
@import '~~/assets/sidebar.less';
</style>