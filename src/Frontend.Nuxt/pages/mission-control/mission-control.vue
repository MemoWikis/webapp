<script lang="ts" setup>
import { SiteType } from '~/components/shared/siteEnum'
import { KnowledgebarData } from '~/components/page/content/grid/knowledgebar/knowledgebarData'
import { ActivityCalendarData } from '~/composables/missionControl/learnCalendar'
import { PageData } from '~/composables/missionControl/pageData'

const emit = defineEmits(['setPage'])
emit('setPage', SiteType.MissionControl) // Geändert von SiteType.Overview zu SiteType.MissionControl

const { t } = useI18n()

useHead({
    title: t('missionControl.title')
})

interface UserDashboardData {
    wikis: PageData[],
    favorites: PageData[],
    knowledgeStatus: KnowledgebarData,
    activityCalendar: ActivityCalendarData
}

// Mock data for now - this would be replaced with an actual API call
const dashboardData = ref<UserDashboardData>()

const getAllData = async () => {
    try {
        const { data } = await useFetch<UserDashboardData>('/apiVue/MissionControl/GetAll')
        if (data.value === null) {
            console.error('No data received from API')
            return
        }
        dashboardData.value = data.value

        console.log("response", data.value)
    } catch (error) {
        console.error('Failed to fetch dashboard data:', error)
    }
}

onMounted(() => {
    getAllData()
})

const { isMobile } = useDevice()

</script>

<template>
    <div class="container">
        <div class="row main-page">
            <div class="col-xs-12">
                <div class="mission-control-container">
                    <h1 class="col-sm-offset-2 col-sm-8">{{ t('missionControl.heading') }}</h1>
                    <div class="mission-control-content" v-if="dashboardData">

                        <!-- Knowledge Status Section -->
                        <MissionControlSection :title="t('missionControl.sections.knowledgeStatus')">
                            <MissionControlKnowledgeSummary v-if="dashboardData.knowledgeStatus" :knowledgeStatus="dashboardData.knowledgeStatus" />
                        </MissionControlSection>

                        <DevOnly>
                            <!-- LearnCalendar Section -->
                            <MissionControlSection :title="t('missionControl.sections.learnCalendar')">
                                <MissionControlLearnCalendar v-if="dashboardData.activityCalendar" :calendarData="dashboardData.activityCalendar" />
                            </MissionControlSection>
                        </DevOnly>

                        <template v-if="isMobile">
                            <!-- Wikis Section -->
                            <MissionControlSection v-if="dashboardData.wikis" :title="t('missionControl.sections.wikis')">
                                <MissionControlGrid :pages="dashboardData.wikis" />
                            </MissionControlSection>

                            <!-- Favorites Section -->
                            <MissionControlSection v-if="dashboardData.favorites" :title="t('missionControl.sections.favorites')">
                                <MissionControlGrid :pages="dashboardData.favorites" />
                            </MissionControlSection>
                        </template>
                        <template v-else>
                            <!-- Wikis Section -->
                            <MissionControlSection v-if="dashboardData.wikis" :title="t('missionControl.sections.wikis')">
                                <MissionControlTable :pages="dashboardData.wikis" />
                            </MissionControlSection>

                            <!-- Favorites Section -->
                            <MissionControlSection v-if="dashboardData.favorites" :title="t('missionControl.sections.favorites')">
                                <MissionControlTable :pages="dashboardData.favorites" />
                            </MissionControlSection>
                        </template>

                        <!-- LearnCalendar Section with Coming Soon overlay -->
                        <MissionControlSection :title="t('missionControl.sections.learnCalendar')">
                            <div class="coming-soon-container">
                                <MissionControlLearnCalendar v-if="dashboardData.activityCalendar" :calendarData="dashboardData.activityCalendar" />
                                <div class="coming-soon-overlay">
                                    <div class="coming-soon-content">
                                        <div class="coming-soon-text">{{ t('general.comingSoon') }}</div>
                                    </div>
                                </div>
                            </div>
                        </MissionControlSection>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.mission-control-container {
    padding: 20px 0;

    h1 {
        margin-bottom: 24px;
        font-weight: 600;
    }

    .mission-control-content {
        margin-top: 20px;
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


}
</style>