<script lang="ts" setup>
import { SiteType } from '~/components/shared/siteEnum'
import { KnowledgeSummary } from '~/composables/knowledgeSummary'
import { ActivityCalendarData } from '~/composables/missionControl/learnCalendar'
import { PageData } from '~/composables/missionControl/pageData'
import { useUserStore } from '~/components/user/userStore'

const userStore = useUserStore()

const emit = defineEmits(['setPage'])
emit('setPage', SiteType.MissionControl)

const { t } = useI18n()

useHead({
    title: t('missionControl.heading')
})

interface UserDashboardData {
    wikis: PageData[],
    favorites: PageData[],
    knowledgeStatus: KnowledgeSummary,
    activityCalendar: ActivityCalendarData
}

const { data: dashboardData } = await useFetch<UserDashboardData>('/apiVue/MissionControl/GetAll')

const { isMobile } = useDevice()

const { locale, setLocale } = useI18n()
const route = useRoute()

onBeforeMount(async () => {
    if (userStore.isLoggedIn) {
        await navigateToLocaleMissionControl()
    } else {
        switch (route.name) {
            case 'missionControlPageDE':
                setLocale('de')
                break
            case 'missionControlPageFR':
                setLocale('fr')
                break
            case 'missionControlPageES':
                setLocale('es')
                break
            default:
                setLocale('en')
                break
        }
    }
})

const navigateToLocaleMissionControl = async () => {
    const localeUrl = `/${t('url.missionControl')}`
    await navigateTo(localeUrl)
}

watch(() => locale.value, async () => {
    const localeUrl = `/${t('url.missionControl')}`

    if (route.path !== localeUrl)
        await navigateToLocaleMissionControl()
})

</script>

<template>

    <div class="mission-control-container">
        <h1>{{ t('missionControl.heading') }}</h1>
        <div class="mission-control-content" v-if="dashboardData">
            <!-- Knowledge Status Section -->
            <LayoutPanel :title="t('missionControl.sections.knowledgeStatus')">
                <LayoutCard :size="LayoutContentSize.Flex">
                    <MissionControlKnowledgeSummary v-if="dashboardData.knowledgeStatus"
                        :knowledgeStatus="dashboardData.knowledgeStatus" />
                </LayoutCard>
            </LayoutPanel>

            <DevOnly>
                <!-- LearnCalendar Section -->
                <LayoutPanel :title="t('missionControl.sections.learnCalendar')">
                    <LayoutCard>
                        <MissionControlLearnCalendar v-if="dashboardData.activityCalendar"
                            :calendarData="dashboardData.activityCalendar" />
                    </LayoutCard>
                </LayoutPanel>
            </DevOnly>

            <template v-if="isMobile">
                <!-- Wikis Section -->
                <LayoutPanel v-if="dashboardData.wikis"
                    :title="t('missionControl.sections.wikis')">
                    <MissionControlGrid :pages="dashboardData.wikis" :no-pages-text="t('missionControl.pageTable.noWikis')" />
                </LayoutPanel>
                <!-- Favorites Section -->
                <LayoutPanel v-if="dashboardData.favorites"
                    :title="t('missionControl.sections.favorites')">
                    <MissionControlGrid :pages="dashboardData.favorites" :no-pages-text="t('missionControl.pageTable.noFavorites')" />
                </LayoutPanel>
            </template>

            <template v-else>
                <!-- Wikis Section -->
                <LayoutPanel v-if="dashboardData.wikis" :title="t('missionControl.sections.wikis')">
                    <LayoutCard :no-padding="true">
                        <MissionControlTable :pages="dashboardData.wikis" :no-pages-text="t('missionControl.pageTable.noWikis')" />
                    </LayoutCard>
                </LayoutPanel>
                <!-- Favorites Section -->
                <LayoutPanel v-if="dashboardData.favorites" :title="t('missionControl.sections.favorites')">
                    <LayoutCard :no-padding="true">
                        <MissionControlTable :pages="dashboardData.favorites" :no-pages-text="t('missionControl.pageTable.noFavorites')" />
                    </LayoutCard>
                </LayoutPanel>
            </template>

            <!-- LearnCalendar Section with Coming Soon overlay -->
            <LayoutPanel :title="t('missionControl.sections.learnCalendar')">
                <div class="coming-soon-container">
                    <MissionControlLearnCalendar v-if="dashboardData.activityCalendar" :calendarData="dashboardData.activityCalendar" />
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

.mission-control-container {
    width: 100%;

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