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

</script>

<template>
    <div class="container">
        <div class="row main-page">
            <div class="mission-control-container">
                <h1>{{ t('missionControl.heading') }}</h1>
                <div class="mission-control-content" v-if="dashboardData">

                    <!-- Knowledge Status Section -->
                    <MissionControlSection :title="t('missionControl.sections.knowledgeStatus')">
                        <MissionControlKnowledgeSummary v-if="dashboardData.knowledgeStatus" :knowledgeStatus="dashboardData.knowledgeStatus" />
                    </MissionControlSection>

                    <!-- ActivityCalendar Section (placeholder) -->
                    <MissionControlSection :title="t('missionControl.sections.learnCalendar')">
                        <MissionControlLearnCalendar v-if="dashboardData.activityCalendar" :calendarData="dashboardData.activityCalendar" />
                    </MissionControlSection>
                    <!-- Wikis Section -->
                    <MissionControlSection v-if="dashboardData.wikis" :title="t('missionControl.sections.wikis')">
                        <MissionControlTable :pages="dashboardData.wikis" />
                    </MissionControlSection>
                    <!-- Favorites Section -->
                    <MissionControlSection v-if="dashboardData.favorites" :title="t('missionControl.sections.favorites')">
                        <MissionControlTable :pages="dashboardData.favorites" />
                    </MissionControlSection>


                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
.mission-control-container {
    padding: 20px 0;

    h1 {
        margin-bottom: 24px;
        font-weight: 600;
    }

    .mission-control-content {
        margin-top: 20px;
    }
}
</style>