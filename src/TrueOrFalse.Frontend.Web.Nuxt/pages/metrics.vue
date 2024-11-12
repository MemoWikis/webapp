<script lang="ts" setup>
import { PageEnum } from '~/components/shared/pageEnum'
import { color } from '~/components/shared/colors'

const { $logger } = useNuxtApp()

interface ViewsResult {
    dateTime: string
    views: number
}

interface GetAllDataResponse {

    todaysActiveUserCount: number
    monthlyActiveUsersOfPastYear: ViewsResult[]
    dailyActiveUsersOfPastYear: ViewsResult[]

    todaysRegistrationCount: number
    monthlyRegistrationsOfPastYear: ViewsResult[]
    dailyRegistrationsOfPastYear: ViewsResult[]

    todaysPublicPageCreatedCount: number
    monthlyPublicCreatedPagesOfPastYear: ViewsResult[]

    createdPrivatePageCount: number
    monthlyPrivateCreatedPagesOfPastYear: ViewsResult[]
    dailyPrivateCreatedPagesOfPastYear: ViewsResult[]

    todaysPageViewCount: number
    pageViewsOfPastYear: ViewsResult[]

    todaysQuestionViewCount: number
    questionViewsOfPastYear: ViewsResult[]
}

const { data: overviewData } = await useFetch<GetAllDataResponse>('/apiVue/Metrics/GetAllData', {
    mode: 'cors',
    credentials: 'include',
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        throw createError({ statusMessage: context.error?.message })
    },
})

// Registrations
const monthlyRegistrationsOfPastYearLabels = computed(() => overviewData.value?.monthlyRegistrationsOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const monthlyRegistrationsOfPastYearCounts = computed(() => overviewData.value?.monthlyRegistrationsOfPastYear?.map(v => v.views) as number[])
const showMonthlyRegistrationsAsBars = ref(true)

const dailyRegistrationsOfPastYearLabels = computed(() => overviewData.value?.dailyRegistrationsOfPastYear?.map(v => {
    const [year, month, day] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}-${day}`
}) as string[])
const dailyRegistrationsOfPastYearCounts = computed(() => overviewData.value?.dailyRegistrationsOfPastYear?.map(v => v.views) as number[])
const showDailyRegistrationsAsBars = ref(true)

//ActiveUsers
const monthlyActiveUsersOfPastYearLabels = computed(() => overviewData.value?.monthlyActiveUsersOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const monthlyActiveUsersOfPastYearCounts = computed(() => overviewData.value?.monthlyActiveUsersOfPastYear?.map(v => v.views) as number[])
const showMonthlyActiveUsersAsBars = ref(true)

const dailyActiveUsersOfPastYearLabels = computed(() => overviewData.value?.dailyActiveUsersOfPastYear?.map(v => {
    const [year, month, day] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}-${day}`
}) as string[])
const dailyActiveUsersOfPastYearCounts = computed(() => overviewData.value?.dailyActiveUsersOfPastYear?.map(v => v.views) as number[])
const showDailyActiveUsersAsBars = ref(true)

//PublicPages Creation
const monthlyPublicCreatedPagesOfPastYearLabels = computed(() => overviewData.value?.monthlyPublicCreatedPagesOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const monthlyPublicCreatedPagesOfPastYearCounts = computed(() => overviewData.value?.monthlyPublicCreatedPagesOfPastYear?.map(v => v.views) as number[])
const showMonthlyPublicCreatedPagesAsBars = ref(true)

//PrivatePages Creation
const monthlyPrivateCreatedPagesOfPastYearLabels = computed(() => overviewData.value?.monthlyPrivateCreatedPagesOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const monthlyPrivateCreatedPagesOfPastYearCounts = computed(() => overviewData.value?.monthlyPrivateCreatedPagesOfPastYear?.map(v => v.views) as number[])
const showMonthlyPrivateCreatedPagesAsBars = ref(true)

const dailyPrivateCreatedPagesOfPastYearLabels = computed(() => overviewData.value?.dailyPrivateCreatedPagesOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const dailyPrivateCreatedPagesOfPastYearCounts = computed(() => overviewData.value?.dailyPrivateCreatedPagesOfPastYear?.map(v => v.views) as number[])
const showDailyPrivateCreatedPagesAsBars = ref(true)


//PageViews
const pageViewsOfPastYearLabels = computed(() => overviewData.value?.pageViewsOfPastYear?.map(v => v.dateTime.split("T")[0]) as string[])
const pageViewsOfPastYearCounts = computed(() => overviewData.value?.pageViewsOfPastYear?.map(v => v.views) as number[])
const showPageViewsAsBars = ref(true)

//QuestionViews
const questionViewsOfPastYearLabels = computed(() => overviewData.value?.questionViewsOfPastYear?.map(v => v.dateTime.split("T")[0]) as string[])
const questionViewsOfPastYearCounts = computed(() => overviewData.value?.questionViewsOfPastYear?.map(v => v.views) as number[])
const showQuestionViewsAsBars = ref(true)

const emit = defineEmits(['setPage', 'setBreadcrumb'])
emit('setPage', PageEnum.Metrics)
emit('setBreadcrumb', [{ name: 'Metriken', url: '/Metriken' }])

</script>

<template>
    <div class="container">
        <div class="row main-page">
            <div class="col-xs-12 container">

                <div class="metrics-header">
                    <h1>Gesamtdaten Memucho</h1>
                </div>

                <div class="row content">
                    <div class="col-xs-12">

                        <div class="chart-section">
                            <h3>Registrierungen </h3>
                            <div class="chart-header">
                                Heutige Registrierungen: {{ overviewData?.todaysRegistrationCount }}
                            </div>

                            <div class="chart-container">
                                <div class="chart-toggle-section">
                                    <div class="chart-toggle-container" @click="showMonthlyRegistrationsAsBars = !showMonthlyRegistrationsAsBars">
                                        <div class="chart-toggle" :class="{ 'is-active': showMonthlyRegistrationsAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-column']" />
                                        </div>
                                        <div class="chart-toggle" :class="{ 'is-active': !showMonthlyRegistrationsAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-line']" />
                                        </div>
                                    </div>
                                </div>

                                <LazySharedChartsBar v-if="showMonthlyRegistrationsAsBars"
                                    :labels="monthlyRegistrationsOfPastYearLabels"
                                    :datasets="monthlyRegistrationsOfPastYearCounts"
                                    :title="'Jahresübersicht Registrierungen'"
                                    :color="color.middleBlue" />
                                <LazySharedChartsLine v-else
                                    :labels="monthlyRegistrationsOfPastYearLabels"
                                    :datasets="monthlyRegistrationsOfPastYearCounts"
                                    :title="'Jahresübersicht Registrierungen'"
                                    :color="color.middleBlue" />
                            </div>

                            <div class="chart-container">
                                <div class="chart-toggle-section">
                                    <div class="chart-toggle-container" @click="showDailyRegistrationsAsBars = !showDailyRegistrationsAsBars">
                                        <div class="chart-toggle" :class="{ 'is-active': showDailyRegistrationsAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-column']" />
                                        </div>
                                        <div class="chart-toggle" :class="{ 'is-active': !showDailyRegistrationsAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-line']" />
                                        </div>
                                    </div>
                                </div>

                                <LazySharedChartsBar v-if="showDailyRegistrationsAsBars"
                                    :labels="dailyRegistrationsOfPastYearLabels"
                                    :datasets="dailyRegistrationsOfPastYearCounts"
                                    :title="'Jahresübersicht Registrierungen'"
                                    :color="color.middleBlue" />
                                <LazySharedChartsLine v-else
                                    :labels="dailyRegistrationsOfPastYearLabels"
                                    :datasets="dailyRegistrationsOfPastYearCounts"
                                    :title="'Jahresübersicht Registrierungen'"
                                    :color="color.middleBlue" />
                            </div>

                        </div>

                        <div class="chart-section">
                            <h3>Aktive Nutzer</h3>
                            <div class="chart-header">
                                Heutige Aktive Nutzer: {{ overviewData?.todaysActiveUserCount }}
                            </div>

                            <div class="chart-container">
                                <div class="chart-toggle-section">
                                    <div class="chart-toggle-container" @click="showMonthlyActiveUsersAsBars = !showMonthlyActiveUsersAsBars">
                                        <div class="chart-toggle" :class="{ 'is-active': showMonthlyActiveUsersAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-column']" />
                                        </div>
                                        <div class="chart-toggle" :class="{ 'is-active': !showMonthlyActiveUsersAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-line']" />
                                        </div>
                                    </div>
                                </div>

                                <LazySharedChartsBar v-if="showMonthlyActiveUsersAsBars"
                                    :labels="monthlyActiveUsersOfPastYearLabels"
                                    :datasets="monthlyActiveUsersOfPastYearCounts"
                                    :title="'Jahresübersicht aktive Nutzer'"
                                    :color="color.darkBlue" />
                                <LazySharedChartsLine v-else
                                    :labels="monthlyActiveUsersOfPastYearLabels"
                                    :datasets="monthlyActiveUsersOfPastYearCounts"
                                    :title="'Jahresübersicht aktive Nutzer'"
                                    :color="color.darkBlue" />
                            </div>

                            <div class="chart-container">
                                <div class="chart-toggle-section">
                                    <div class="chart-toggle-container" @click="showDailyActiveUsersAsBars = !showDailyActiveUsersAsBars">
                                        <div class="chart-toggle" :class="{ 'is-active': showDailyActiveUsersAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-column']" />
                                        </div>
                                        <div class="chart-toggle" :class="{ 'is-active': !showDailyActiveUsersAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-line']" />
                                        </div>
                                    </div>
                                </div>

                                <LazySharedChartsBar v-if="showDailyActiveUsersAsBars"
                                    :labels="dailyActiveUsersOfPastYearLabels"
                                    :datasets="dailyActiveUsersOfPastYearCounts"
                                    :title="'Jahresübersicht aktive Nutzer'"
                                    :color="color.darkBlue" />
                                <LazySharedChartsLine v-else
                                    :labels="dailyActiveUsersOfPastYearLabels"
                                    :datasets="dailyActiveUsersOfPastYearCounts"
                                    :title="'Jahresübersicht aktive Nutzer'"
                                    :color="color.darkBlue" />
                            </div>

                        </div>

                        <div class="chart-section">
                            <h3>Private Themen</h3>
                            <div class="chart-header">
                                Heute erstellt: {{ overviewData?.createdPrivatePageCount }}
                            </div>

                            <div class="chart-container">
                                <div class="chart-toggle-section">
                                    <div class="chart-toggle-container" @click="showMonthlyPrivateCreatedPagesAsBars = !showMonthlyPrivateCreatedPagesAsBars">
                                        <div class="chart-toggle" :class="{ 'is-active': showMonthlyPrivateCreatedPagesAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-column']" />
                                        </div>
                                        <div class="chart-toggle" :class="{ 'is-active': !showMonthlyPrivateCreatedPagesAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-line']" />
                                        </div>
                                    </div>
                                </div>
                                <LazySharedChartsBar v-if="showMonthlyPrivateCreatedPagesAsBars"
                                    :labels="monthlyPrivateCreatedPagesOfPastYearLabels"
                                    :datasets="monthlyPrivateCreatedPagesOfPastYearCounts"
                                    :title="'Jahresübersicht erstellte Private Pages'"
                                    :color="color.lightRed" />
                                <LazySharedChartsLine v-else
                                    :labels="monthlyPrivateCreatedPagesOfPastYearLabels"
                                    :datasets="monthlyPrivateCreatedPagesOfPastYearCounts"
                                    :title="'Jahresübersicht erstellte Private Pages'"
                                    :color="color.lightRed" />
                            </div>

                            <div class="chart-container">
                                <div class="chart-toggle-section">
                                    <div class="chart-toggle-container" @click="showDailyPrivateCreatedPagesAsBars = !showDailyPrivateCreatedPagesAsBars">
                                        <div class="chart-toggle" :class="{ 'is-active': showDailyPrivateCreatedPagesAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-column']" />
                                        </div>
                                        <div class="chart-toggle" :class="{ 'is-active': !showDailyPrivateCreatedPagesAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-line']" />
                                        </div>
                                    </div>
                                </div>
                                <LazySharedChartsBar v-if="showDailyPrivateCreatedPagesAsBars"
                                    :labels="dailyPrivateCreatedPagesOfPastYearLabels"
                                    :datasets="dailyPrivateCreatedPagesOfPastYearCounts"
                                    :title="'Jahresübersicht erstellte Private Pages'"
                                    :color="color.lightRed" />
                                <LazySharedChartsLine v-else
                                    :labels="dailyPrivateCreatedPagesOfPastYearLabels"
                                    :datasets="dailyPrivateCreatedPagesOfPastYearCounts"
                                    :title="'Jahresübersicht erstellte Private Pages'"
                                    :color="color.lightRed" />
                            </div>
                        </div>

                        <div class="chart-section">
                            <h3>Öffentliche Themen</h3>
                            <div class="chart-header">
                                Heute erstellt: {{ overviewData?.todaysPublicPageCreatedCount }}
                            </div>

                            <div class="chart-container">
                                <div class="chart-toggle-section">
                                    <div class="chart-toggle-container" @click="showMonthlyPublicCreatedPagesAsBars = !showMonthlyPublicCreatedPagesAsBars">
                                        <div class="chart-toggle" :class="{ 'is-active': showMonthlyPublicCreatedPagesAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-column']" />
                                        </div>
                                        <div class="chart-toggle" :class="{ 'is-active': !showMonthlyPublicCreatedPagesAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-line']" />
                                        </div>
                                    </div>
                                </div>
                                <LazySharedChartsBar v-if="showMonthlyPublicCreatedPagesAsBars"
                                    :labels="monthlyPublicCreatedPagesOfPastYearLabels"
                                    :datasets="monthlyPublicCreatedPagesOfPastYearCounts"
                                    :title="'Jahresübersicht erstellte Public Pages'"
                                    :color="color.darkRed" />
                                <LazySharedChartsLine v-else
                                    :labels="monthlyPublicCreatedPagesOfPastYearLabels"
                                    :datasets="monthlyPublicCreatedPagesOfPastYearCounts"
                                    :title="'Jahresübersicht erstellte Public Pages'"
                                    :color="color.darkRed" />
                            </div>
                        </div>

                        <div class="chart-section">
                            <h3>Pageviews</h3>
                            <div class="chart-header">
                                Heutige Pageviews: {{ overviewData?.todaysPageViewCount }}
                            </div>

                            <div class="chart-container">
                                <div class="chart-toggle-section">
                                    <div class="chart-toggle-container" @click="showPageViewsAsBars = !showPageViewsAsBars">
                                        <div class="chart-toggle" :class="{ 'is-active': showPageViewsAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-column']" />
                                        </div>
                                        <div class="chart-toggle" :class="{ 'is-active': !showPageViewsAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-line']" />
                                        </div>
                                    </div>
                                </div>

                                <LazySharedChartsBar v-if="showPageViewsAsBars"
                                    :labels="pageViewsOfPastYearLabels"
                                    :datasets="pageViewsOfPastYearCounts"
                                    :title="'Jahresübersicht Page Views'"
                                    :color="color.memoGreen" />
                                <LazySharedChartsLine v-else
                                    :labels="pageViewsOfPastYearLabels"
                                    :datasets="pageViewsOfPastYearCounts"
                                    :title="'Jahresübersicht Page Views'"
                                    :color="color.memoGreen" />
                            </div>
                        </div>

                        <div class="chart-section">
                            <h3>Questionviews</h3>
                            <div class="chart-header">
                                Heutige Questionviews: {{ overviewData?.todaysQuestionViewCount }}
                            </div>

                            <div class="chart-container">
                                <div class="chart-toggle-section">
                                    <div class="chart-toggle-container" @click="showQuestionViewsAsBars = !showQuestionViewsAsBars">
                                        <div class="chart-toggle" :class="{ 'is-active': showQuestionViewsAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-column']" />
                                        </div>
                                        <div class="chart-toggle" :class="{ 'is-active': !showQuestionViewsAsBars }">
                                            <font-awesome-icon :icon="['fas', 'chart-line']" />
                                        </div>
                                    </div>
                                </div>

                                <LazySharedChartsBar v-if="showQuestionViewsAsBars"
                                    :labels="questionViewsOfPastYearLabels"
                                    :datasets="questionViewsOfPastYearCounts"
                                    :title="'Jahresübersicht Question Views'"
                                    :color="color.darkGreen" />

                                <LazySharedChartsLine v-else
                                    :labels="questionViewsOfPastYearLabels"
                                    :datasets="questionViewsOfPastYearCounts"
                                    :title="'Jahresübersicht Question Views'"
                                    :color="color.darkGreen" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.chart-header {
    display: flex;
    margin-bottom: 20px;
    justify-content: space-between;
    font-size: 18px;
}

.chart-container {
    margin-bottom: 40px;
    min-height: 300px;
}

.chart-toggle-section {
    display: flex;
    justify-content: flex-end;
    position: absolute;
    right: 0px;

    .chart-toggle-container {
        display: flex;
        cursor: pointer;
        flex-wrap: nowrap;
        color: @memo-grey-dark;
        border-radius: 4px;
        border: solid 1px @memo-grey-lighter;

        .chart-toggle {
            justify-content: center;
            align-items: center;
            padding: 4px 12px;
            background: white;

            &.is-active {
                color: @memo-blue-link;
                background: @memo-grey-lighter;
            }

            &:hover {
                filter: brightness(0.95);
            }

            &:active {
                filter: brightness(0.9);
            }
        }

    }
}

.metrics-header {
    height: 54px;
    margin-top: 20px;
    margin-bottom: 10px;
}

.divider {
    height: 1px;
    background: @memo-grey-lighter;
    width: 100%;
    margin-top: 10px;
    margin-bottom: 60px;
}

.chart-section {
    margin-bottom: 45px;
}
</style>
