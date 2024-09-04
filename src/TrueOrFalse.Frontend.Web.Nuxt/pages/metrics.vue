<script lang="ts" setup>
import { Page } from '~/components/shared/pageEnum'
import { color } from '~/components/shared/colors'

const { $logger } = useNuxtApp()

interface ViewsResult {
    dateTime: string
    views: number
}

interface GetAllDataResponse {

    todaysLoginCount: number
    monthlyLoginsOfPastYear: ViewsResult[]
    dailyLoginsOfPastYear: ViewsResult[]

    todaysRegistrationCount: number
    monthlyRegistrationsOfPastYear: ViewsResult[]
    dailyRegistrationsOfPastYear: ViewsResult[]

    todaysPublicTopicCreatedCount: number
    monthlyPublicCreatedTopicsOfPastYear: ViewsResult[]

    createdPrivateTopicCount: number
    annualPrivateCreatedTopics: ViewsResult[]

    todaysTopicViewCount: number
    topicViewsOfPastYear: ViewsResult[]

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

const viewTopicLabels = computed(() => overviewData.value?.topicViewsOfPastYear?.map(v => v.dateTime.split("T")[0]) as string[])
const viewTopicViews = computed(() => overviewData.value?.topicViewsOfPastYear?.map(v => v.views) as number[])

const viewQuestionLabels = computed(() => overviewData.value?.questionViewsOfPastYear?.map(v => v.dateTime.split("T")[0]) as string[])
const viewQuestionViews = computed(() => overviewData.value?.questionViewsOfPastYear?.map(v => v.views) as number[])

const monthlyLoginsOfPastYearLabels = computed(() => overviewData.value?.monthlyLoginsOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const monthlyLoginsOfPastYearCounts = computed(() => overviewData.value?.monthlyLoginsOfPastYear?.map(v => v.views) as number[])

const dailyLoginsOfPastYearLabels = computed(() => overviewData.value?.dailyLoginsOfPastYear?.map(v => {
    const [year, month, day] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}-${day}`
}) as string[])
const dailyLoginsOfPastYearCounts = computed(() => overviewData.value?.dailyLoginsOfPastYear?.map(v => v.views) as number[])

const monthlyRegistrationsOfPastYearLabels = computed(() => overviewData.value?.monthlyRegistrationsOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const monthlyRegistrationsOfPastYearCounts = computed(() => overviewData.value?.monthlyRegistrationsOfPastYear?.map(v => v.views) as number[])

const annualPublicCreatedTopicLabels = computed(() => overviewData.value?.monthlyPublicCreatedTopicsOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const annualPublicCreatedTopicCounts = computed(() => overviewData.value?.monthlyPublicCreatedTopicsOfPastYear?.map(v => v.views) as number[])

const annualPrivateCreatedTopicLabels = computed(() => overviewData.value?.annualPrivateCreatedTopics?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const annualPrivateCreatedTopicCounts = computed(() => overviewData.value?.annualPrivateCreatedTopics?.map(v => v.views) as number[])

const emit = defineEmits(['setPage', 'setBreadcrumb'])
emit('setPage', Page.Metrics)
emit('setBreadcrumb', [{ name: 'Metriken', url: '/Metriken' }])


const showAnnualRegistrationBar = ref(false)

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

                                <div class="chart-toggle-container" @click="showAnnualRegistrationBar = !showAnnualRegistrationBar">
                                    <div class="chart-toggle" :class="{ 'is-active': showAnnualRegistrationBar }">
                                        <font-awesome-icon :icon="['fas', 'chart-column']" />
                                    </div>
                                    <div class="chart-toggle" :class="{ 'is-active': !showAnnualRegistrationBar }">
                                        <font-awesome-icon :icon="['fas', 'chart-line']" />
                                    </div>
                                </div>

                            </div>

                            <div class="chart-container">
                                <LazySharedChartsBar v-if="showAnnualRegistrationBar"
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
                        </div>

                        <div class="chart-section">
                            <h3>Logins</h3>
                            <div class="chart-header">
                                Heutige Logins: {{ overviewData?.todaysLoginCount }}
                            </div>

                            <div class="chart-conta iner">
                                <LazySharedChartsBar
                                    :labels="monthlyLoginsOfPastYearLabels"
                                    :datasets="monthlyLoginsOfPastYearCounts"
                                    :title="'Jahresübersicht Logins'"
                                    :color="color.darkBlue" />
                            </div>

                            <div class="chart-container">
                                <LazySharedChartsBar
                                    :labels="dailyLoginsOfPastYearLabels"
                                    :datasets="dailyLoginsOfPastYearCounts"
                                    :title="'Jahresübersicht Logins'"
                                    :color="color.darkBlue" />
                            </div>
                        </div>

                        <div class="chart-section">
                            <h3>Private Themen</h3>
                            <div class="bar-header">
                                Heute erstellt: {{ overviewData?.createdPrivateTopicCount }}
                            </div>

                            <div class="chart-container">
                                <LazySharedChartsBar
                                    :labels="annualPrivateCreatedTopicLabels"
                                    :datasets="annualPrivateCreatedTopicCounts"
                                    :title="'Jahresübersicht erstellte Private Topics'"
                                    :color="color.lightRed" />
                            </div>
                        </div>

                        <div class="chart-section">
                            <h3>Öffentliche Themen</h3>
                            <div class="bar-header">
                                Heute erstellt: {{ overviewData?.todaysPublicTopicCreatedCount }}
                            </div>

                            <div class="chart-container">
                                <LazySharedChartsBar
                                    :labels="annualPublicCreatedTopicLabels"
                                    :datasets="annualPublicCreatedTopicCounts"
                                    :title="'Jahresübersicht erstellte Public Topics'"
                                    :color="color.darkRed" />

                            </div>
                        </div>

                        <div class="chart-section">
                            <h3>Topicviews</h3>
                            <div class="bar-header">
                                Heutige Topicviews: {{ overviewData?.todaysTopicViewCount }}
                            </div>

                            <div class="chart-container">
                                <LazySharedChartsBar
                                    :labels="viewTopicLabels"
                                    :datasets="viewTopicViews"
                                    :title="'Jahresübersicht Topic Views'"
                                    :color="color.memoGreen" />
                            </div>
                        </div>

                        <div class="chart-section">
                            <h3>Questionviews</h3>
                            <div class="bar-header">
                                Heutige Questionviews: {{ overviewData?.todaysQuestionViewCount }}
                            </div>

                            <div class="chart-container">
                                <LazySharedChartsBar
                                    :labels="viewQuestionLabels"
                                    :datasets="viewQuestionViews"
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
    font-size:18px;

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
