<script lang="ts" setup>
import { SiteType } from '~/components/shared/siteEnum'
import { KnowledgebarData } from '~/components/page/content/grid/knowledgebar/knowledgebarData'
import { ActivityCalendarData } from '~/composables/missionControl/activityCalendar'

const emit = defineEmits(['setPage'])
emit('setPage', SiteType.MissionControl) // Geändert von SiteType.Overview zu SiteType.MissionControl

const { t } = useI18n()

useHead({
    title: t('missionControl.title')
})

interface WikiData {
    id: number
    name: string
    imgUrl?: string
    questionCount: number
    knowledgebarData: KnowledgebarData
}

interface UserDashboardData {
    wikis: WikiData[],
    knowledgeStatus: KnowledgebarData,
    activityCalendar: ActivityCalendarData
}

// Mock data for now - this would be replaced with an actual API call
const dashboardData = ref<UserDashboardData>()

const getAllData = async () => {
    try {
        const { data } = await useFetch('/apiVue/MissionControl/GetAll')
        dashboardData.value = data.value
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
                    <!-- Wikis Section -->
                    <MissionControlSection v-if="dashboardData.wikis" :title="t('missionControl.sections.wikis')">

                        <MissionControlWikisGrid :wikis="dashboardData.wikis" />
                    </MissionControlSection>

                    <!-- Knowledge Status Section (placeholder) -->
                    <MissionControlSection :title="t('missionControl.sections.knowledgeStatus')">
                        <p>{{ t('missionControl.knowledgeStatus.comingSoon') }}</p>
                    </MissionControlSection>

                    <!-- ActivityCalendar Section (placeholder) -->
                    <MissionControlSection :title="t('missionControl.sections.actvityCalendar')">
                        <template #actions>
                            <button class="btn btn-sm btn-outline-primary">
                                {{ t('missionControl.actvityCalendar.startLearning') }}
                            </button>
                        </template>
                        <p>{{ t('missionControl.actvityCalendar.comingSoon') }}</p>
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