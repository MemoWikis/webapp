<script lang="ts" setup>
import { SiteType } from '~/components/shared/siteEnum'

// Set the page type
const emit = defineEmits(['setPage'])
emit('setPage', SiteType.MissionControl) // Geändert von SiteType.Overview zu SiteType.MissionControl

// Basic setup for the mission control page
const { t } = useI18n()

// Use head to set page title
useHead({
    title: t('missionControl.title')
})
</script>

<template>
    <div class="mission-control-container">
        <h1>{{ t('missionControl.heading') }}</h1>
        <div class="mission-control-content">
            <!-- Mission control content will go here -->
            <p>{{ t('missionControl.description') }}</p>
        </div>
    </div>
</template>

<style lang="less" scoped>
.mission-control-container {
    padding: 20px;

    .mission-control-content {
        margin-top: 20px;
    }
}
</style>