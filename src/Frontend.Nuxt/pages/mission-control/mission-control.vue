<script lang="ts" setup>
import { SiteType } from '~/components/shared/siteEnum'
import { KnowledgeSummary } from '~/composables/knowledgeSummary'
import { ActivityCalendarData } from '~/composables/missionControl/learnCalendar'
import { PageData } from '~/composables/missionControl/pageData'
import { useUserStore } from '~/components/user/userStore'
import { LayoutCardSize } from '~/composables/layoutCardSize'
import missionControlSections from '~/constants/missionControlSections'

const userStore = useUserStore()

const emit = defineEmits(['setPage'])
emit('setPage', SiteType.MissionControl)

const { t, locale, setLocale } = useI18n()

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

const hasWikis = computed(() => {
    return dashboardData.value?.wikis && dashboardData.value.wikis.length > 0
})

const hasFavorites = computed(() => {
    return dashboardData.value?.favorites && dashboardData.value.favorites.length > 0
})

const missionControlHeader = ref()
const missionControlHeaderHeight = ref(123) // Default height

const sideBarMarginTop = computed(() => {
    const defaultHeaderHeight = 123
    if (missionControlHeaderHeight.value > defaultHeaderHeight) {
        return missionControlHeaderHeight.value - defaultHeaderHeight + 25
    }
    return 25 // Default margin top if no header is available
})

// Watch for header height changes
onMounted(() => {
    if (missionControlHeader.value) {
        // Initial height measurement
        missionControlHeaderHeight.value = missionControlHeader.value.offsetHeight

        // Set up ResizeObserver to watch for height changes
        const resizeObserver = new ResizeObserver((entries) => {
            for (const entry of entries) {
                missionControlHeaderHeight.value = entry.contentRect.height
            }
        })

        resizeObserver.observe(missionControlHeader.value)

        // Cleanup on unmount
        onUnmounted(() => {
            resizeObserver.disconnect()
        })
    }
})

</script>

<template>

    <div class="mission-control-container">
        <div class="mission-control-content">
            <div class="mission-control-header" ref="missionControlHeader">
                <h1>{{ t('missionControl.heading') }}</h1>
            </div>

            <div v-if="dashboardData">
                <!-- Knowledge Status Section -->
                <LayoutPanel :id="missionControlSections.KNOWLEDGE_STATUS_SECTION.id" :title="t(missionControlSections.KNOWLEDGE_STATUS_SECTION.translationKey)">
                    <LayoutCard :size="LayoutCardSize.Flex">
                        <MissionControlKnowledgeSummary v-if="dashboardData.knowledgeStatus"
                            :knowledgeStatus="dashboardData.knowledgeStatus" />
                    </LayoutCard>
                </LayoutPanel>

                <DevOnly>
                    <!-- LearnCalendar Section -->
                    <LayoutPanel :id="missionControlSections.LEARN_CALENDAR_SECTION.id" :title="t(missionControlSections.LEARN_CALENDAR_SECTION.translationKey)">
                        <LayoutCard>
                            <MissionControlLearnCalendar v-if="dashboardData.activityCalendar"
                                :calendarData="dashboardData.activityCalendar" />
                        </LayoutCard>
                    </LayoutPanel>
                </DevOnly>

                <template v-if="isMobile">
                    <!-- Wikis Section -->
                    <LayoutPanel v-if="dashboardData.wikis" :id="missionControlSections.WIKIS_SECTION.id"
                        :title="t(missionControlSections.WIKIS_SECTION.translationKey)">
                        <MissionControlGrid :pages="dashboardData.wikis" :no-pages-text="t('missionControl.pageTable.noWikis')" />
                    </LayoutPanel>
                    <!-- Favorites Section -->
                    <LayoutPanel v-if="dashboardData.favorites" :id="missionControlSections.FAVORITES_SECTION.id"
                        :title="t(missionControlSections.FAVORITES_SECTION.translationKey)">
                        <MissionControlGrid :pages="dashboardData.favorites" :no-pages-text="t('missionControl.pageTable.noFavorites')" />
                    </LayoutPanel>
                </template>

                <template v-else>
                    <!-- Wikis Section -->
                    <LayoutPanel v-if="dashboardData.wikis" :id="missionControlSections.WIKIS_SECTION.id" :title="t(missionControlSections.WIKIS_SECTION.translationKey)">
                        <LayoutCard :no-padding="true">
                            <MissionControlTable :pages="dashboardData.wikis" :no-pages-text="t('missionControl.pageTable.noWikis')" />
                        </LayoutCard>
                    </LayoutPanel>
                    <!-- Favorites Section -->
                    <LayoutPanel v-if="dashboardData.favorites" :id="missionControlSections.FAVORITES_SECTION.id" :title="t(missionControlSections.FAVORITES_SECTION.translationKey)">
                        <LayoutCard :no-padding="true">
                            <MissionControlTable :pages="dashboardData.favorites" :no-pages-text="t('missionControl.pageTable.noFavorites')" />
                        </LayoutCard>
                    </LayoutPanel>
                </template>

                <!-- LearnCalendar Section with Coming Soon overlay -->
                <LayoutPanel :id="missionControlSections.LEARN_CALENDAR_SECTION.id" :title="t(missionControlSections.LEARN_CALENDAR_SECTION.translationKey)">
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

        <SidebarMissionControl :show-wikis="hasWikis" :show-favorites="hasFavorites" :margin-top="sideBarMarginTop" />
    </div>

</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.mission-control-container {
    display: flex;
    flex-wrap: nowrap;
    gap: 0 1rem;
    width: 100%;

    .mission-control-content {
        max-width: 1200px;
        width: calc(75% - 1rem);
        flex-grow: 2;

        .mission-control-header {
            margin: 25px 0;

            h1 {
                margin-bottom: 24px;
                font-weight: 600;
            }
        }
    }

    @media (max-width: 900px) {
        .mission-control-content {
            width: 100%;
        }
    }

    .panel-divider {
        height: 1px;
        background: @memo-grey-lighter;
        width: 100%;
        margin-bottom: 20px;
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