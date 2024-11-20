<script lang="ts" setup>
import { PageEnum } from '~/components/shared/pageEnum'
import { color } from '~/components/shared/colors'
import { SectionChart } from '~/components/metrics/sectionChart'

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

    todaysPublishedQuestionCount: number
    monthlyPublishedQuestionsOfPastYear: ViewsResult[]
    dailyPublishedQuestionsOfPastYear: ViewsResult[]

    todaysPublicQuestionCreatedCount: number
    monthlyPublicCreatedQuestionsOfPastYear: ViewsResult[]

    todaysPrivateQuestionCreatedCount: number
    monthlyPrivateCreatedQuestionsOfPastYear: ViewsResult[]
}

const { data: overviewData } = await useFetch<GetAllDataResponse>('/apiVue/Metrics/GetAllData', {
    mode: 'cors',
    credentials: 'include',
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        throw createError({ statusMessage: context.error?.message })
    },
})

const barToggles = reactive({
    showMonthlyRegistrationsAsBars: true,
    showDailyRegistrationsAsBars: true,
    showMonthlyActiveUsersAsBars: true,
    showDailyActiveUsersAsBars: true,
    showMonthlyPublicCreatedPagesAsBars: true,
    showMonthlyPrivateCreatedPagesAsBars: true,
    showDailyPrivateCreatedPagesAsBars: true,
    showPageViewsAsBars: true,
    showQuestionViewsAsBars: true,
    showMonthlyPublishedQuestionsAsBars: true,
    showDailyPublishedQuestionsAsBars: true,
    showMonthlyPublicCreatedQuestionsAsBars: true,
    showMonthlyPrivateCreatedQuestionsAsBars: true,
})

const toggleBar = (key: keyof typeof barToggles) => {
    barToggles[key] = !barToggles[key]
}

// Registrations
const monthlyRegistrationsOfPastYearLabels = computed(() => overviewData.value?.monthlyRegistrationsOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const monthlyRegistrationsOfPastYearCounts = computed(() => overviewData.value?.monthlyRegistrationsOfPastYear?.map(v => v.views) as number[])
// const showMonthlyRegistrationsAsBars = ref(true)

const dailyRegistrationsOfPastYearLabels = computed(() => overviewData.value?.dailyRegistrationsOfPastYear?.map(v => {
    const [year, month, day] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}-${day}`
}) as string[])
const dailyRegistrationsOfPastYearCounts = computed(() => overviewData.value?.dailyRegistrationsOfPastYear?.map(v => v.views) as number[])

const registrationCharts = computed(() => {
    return [
        {
            showBar: barToggles.showMonthlyRegistrationsAsBars,
            barToggleKey: 'showMonthlyRegistrationsAsBars',
            labels: monthlyRegistrationsOfPastYearLabels.value,
            datasets: monthlyRegistrationsOfPastYearCounts.value,
            title: 'Jahresübersicht Registrierungen',
            color: color.middleBlue,
        },
        {
            showBar: barToggles.showDailyRegistrationsAsBars,
            barToggleKey: 'showDailyRegistrationsAsBars',
            labels: dailyRegistrationsOfPastYearLabels.value,
            datasets: dailyRegistrationsOfPastYearCounts.value,
            title: 'Jahresübersicht Registrierungen',
            color: color.middleBlue,
        },
    ] as SectionChart[]
})

//ActiveUsers
const monthlyActiveUsersOfPastYearLabels = computed(() => overviewData.value?.monthlyActiveUsersOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const monthlyActiveUsersOfPastYearCounts = computed(() => overviewData.value?.monthlyActiveUsersOfPastYear?.map(v => v.views) as number[])

const dailyActiveUsersOfPastYearLabels = computed(() => overviewData.value?.dailyActiveUsersOfPastYear?.map(v => {
    const [year, month, day] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}-${day}`
}) as string[])
const dailyActiveUsersOfPastYearCounts = computed(() => overviewData.value?.dailyActiveUsersOfPastYear?.map(v => v.views) as number[])

const activeUsersChart = computed(() => {
    return [
        {
            showBar: barToggles.showMonthlyActiveUsersAsBars,
            barToggleKey: 'showMonthlyActiveUsersAsBars',
            labels: monthlyActiveUsersOfPastYearLabels.value,
            datasets: monthlyActiveUsersOfPastYearCounts.value,
            title: 'Jahresübersicht aktive Nutzer',
            color: color.darkBlue,
        },
        {
            showBar: barToggles.showDailyActiveUsersAsBars,
            barToggleKey: 'showDailyActiveUsersAsBars',
            labels: dailyActiveUsersOfPastYearLabels.value,
            datasets: dailyActiveUsersOfPastYearCounts.value,
            title: 'Jahresübersicht aktive Nutzer',
            color: color.darkBlue,
        }

    ] as SectionChart[]
})

//PublicPages Creation
const monthlyPublicCreatedPagesOfPastYearLabels = computed(() => overviewData.value?.monthlyPublicCreatedPagesOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const monthlyPublicCreatedPagesOfPastYearCounts = computed(() => overviewData.value?.monthlyPublicCreatedPagesOfPastYear?.map(v => v.views) as number[])

const publicCreatedPagesChart = computed(() => {
    return [
        {
            showBar: barToggles.showMonthlyPublicCreatedPagesAsBars,
            barToggleKey: 'showMonthlyPublicCreatedPagesAsBars',
            labels: monthlyPublicCreatedPagesOfPastYearLabels.value,
            datasets: monthlyPublicCreatedPagesOfPastYearCounts.value,
            title: 'Jahresübersicht erstellte Public Pages',
            color: color.darkRed,
        },
    ] as SectionChart[]
})

//PrivatePages Creation
const monthlyPrivateCreatedPagesOfPastYearLabels = computed(() => overviewData.value?.monthlyPrivateCreatedPagesOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const monthlyPrivateCreatedPagesOfPastYearCounts = computed(() => overviewData.value?.monthlyPrivateCreatedPagesOfPastYear?.map(v => v.views) as number[])

const dailyPrivateCreatedPagesOfPastYearLabels = computed(() => overviewData.value?.dailyPrivateCreatedPagesOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const dailyPrivateCreatedPagesOfPastYearCounts = computed(() => overviewData.value?.dailyPrivateCreatedPagesOfPastYear?.map(v => v.views) as number[])

const privateCreatedPagesCharts = computed(() => {
    return [
        {
            showBar: barToggles.showMonthlyPrivateCreatedPagesAsBars,
            barToggleKey: 'showMonthlyPrivateCreatedPagesAsBars',
            labels: monthlyPrivateCreatedPagesOfPastYearLabels.value,
            datasets: monthlyPrivateCreatedPagesOfPastYearCounts.value,
            title: 'Jahresübersicht erstellte Private Pages',
            color: color.lightRed,
        },
        {
            showBar: barToggles.showDailyPrivateCreatedPagesAsBars,
            barToggleKey: 'showDailyPrivateCreatedPagesAsBars',
            labels: dailyPrivateCreatedPagesOfPastYearLabels.value,
            datasets: dailyPrivateCreatedPagesOfPastYearCounts.value,
            title: 'Jahresübersicht erstellte Private Pages',
            color: color.lightRed,
        },
    ] as SectionChart[]
})

//PageViews
const pageViewsOfPastYearLabels = computed(() => overviewData.value?.pageViewsOfPastYear?.map(v => v.dateTime.split("T")[0]) as string[])
const pageViewsOfPastYearCounts = computed(() => overviewData.value?.pageViewsOfPastYear?.map(v => v.views) as number[])

const pageViewsCharts = computed(() => {
    return [
        {
            showBar: barToggles.showPageViewsAsBars,
            barToggleKey: 'showPageViewsAsBars',
            labels: pageViewsOfPastYearLabels.value,
            datasets: pageViewsOfPastYearCounts.value,
            title: 'Jahresübersicht Page Views',
            color: color.memoGreen,
        },
    ] as SectionChart[]
})

//QuestionViews
const questionViewsOfPastYearLabels = computed(() => overviewData.value?.questionViewsOfPastYear?.map(v => v.dateTime.split("T")[0]) as string[])
const questionViewsOfPastYearCounts = computed(() => overviewData.value?.questionViewsOfPastYear?.map(v => v.views) as number[])

const questionViewsCharts = computed(() => {
    return [
        {
            showBar: barToggles.showQuestionViewsAsBars,
            barToggleKey: 'showQuestionViewsAsBars',
            labels: questionViewsOfPastYearLabels.value,
            datasets: questionViewsOfPastYearCounts.value,
            title: 'Jahresübersicht Question Views',
            color: color.darkGreen,
        },
    ] as SectionChart[]
})

//Published Questions
const monthlyPublishedQuestionsOfPastYearLabels = computed(() => overviewData.value?.monthlyPublishedQuestionsOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const monthlyPublishedQuestionsOfPastYearCounts = computed(() => overviewData.value?.monthlyPublishedQuestionsOfPastYear?.map(v => v.views) as number[])

const dailyPublishedQuestionsOfPastYearLabels = computed(() => overviewData.value?.dailyPublishedQuestionsOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const dailyPublishedQuestionsOfPastYearCounts = computed(() => overviewData.value?.dailyPublishedQuestionsOfPastYear?.map(v => v.views) as number[])

const publishedQuestionsCharts = computed(() => {
    return [
        {
            showBar: barToggles.showMonthlyPublishedQuestionsAsBars,
            barToggleKey: 'showMonthlyPublishedQuestionsAsBars',
            labels: monthlyPublishedQuestionsOfPastYearLabels.value,
            datasets: monthlyPublishedQuestionsOfPastYearCounts.value,
            title: 'Jahresübersicht veröffentlichte Fragen',
            color: color.lightPurple,
        },
        {
            showBar: barToggles.showDailyPublishedQuestionsAsBars,
            barToggleKey: 'showDailyPublishedQuestionsAsBars',
            labels: dailyPublishedQuestionsOfPastYearLabels.value,
            datasets: dailyPublishedQuestionsOfPastYearCounts.value,
            title: 'Jahresübersicht veröffentlichte Fragen',
            color: color.lightPurple,
        },
    ] as SectionChart[]
})

const monthlyPublicCreatedQuestionsOfPastYearLabels = computed(() => overviewData.value?.monthlyPublicCreatedQuestionsOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const monthlyPublicCreatedQuestionsOfPastYearCounts = computed(() => overviewData.value?.monthlyPublicCreatedQuestionsOfPastYear?.map(v => v.views) as number[])

const publicCreatedPagesCharts = computed(() => {
    return [
        {
            showBar: barToggles.showMonthlyPublicCreatedQuestionsAsBars,
            barToggleKey: 'showMonthlyPublicCreatedQuestionsAsBars',
            labels: monthlyPublicCreatedQuestionsOfPastYearLabels.value,
            datasets: monthlyPublicCreatedQuestionsOfPastYearCounts.value,
            title: 'Jahresübersicht erstellte Public Fragen',
            color: color.middlePurple,
        },
    ] as SectionChart[]
})

const monthlyPrivateCreatedQuestionsOfPastYearLabels = computed(() => overviewData.value?.monthlyPrivateCreatedQuestionsOfPastYear?.map(v => {
    const [year, month] = v.dateTime.split("T")[0].split("-")
    return `${year}-${month}`
}) as string[])
const monthlyPrivateCreatedQuestionsOfPastYearCounts = computed(() => overviewData.value?.monthlyPrivateCreatedQuestionsOfPastYear?.map(v => v.views) as number[])

const privateCreatedQuestionsCharts = computed(() => {
    return [
        {
            showBar: barToggles.showMonthlyPrivateCreatedQuestionsAsBars,
            barToggleKey: 'showMonthlyPrivateCreatedQuestionsAsBars',
            labels: monthlyPrivateCreatedQuestionsOfPastYearLabels.value,
            datasets: monthlyPrivateCreatedQuestionsOfPastYearCounts.value,
            title: 'Jahresübersicht erstellte Private Fragen',
            color: color.darkPurple,
        },
    ] as SectionChart[]
})

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

                <div class="row content" v-if="overviewData">
                    <div class="col-xs-12">

                        <MetricsSection title="Registrierungen" :sub-title="`Heutige Registrierungen: ${overviewData?.todaysRegistrationCount}`" :charts="registrationCharts" @toggle-bar="toggleBar" />
                        <MetricsSection title="Aktive Nutzer" :sub-title="`Heutige Aktive Nutzer: ${overviewData?.todaysActiveUserCount}`" :charts="activeUsersChart" @toggle-bar="toggleBar" />
                        <MetricsSection title="Private Seiten" :sub-title="`Heute erstellt: ${overviewData?.createdPrivatePageCount}`" :charts="privateCreatedPagesCharts" @toggle-bar="toggleBar" />
                        <MetricsSection title="Öffentliche Seiten" :sub-title="`Heute erstellt: ${overviewData?.todaysPublicPageCreatedCount}`" :charts="publicCreatedPagesChart" @toggle-bar="toggleBar" />
                        <MetricsSection title="Wiki-Pageviews" :sub-title="`Heutige Wiki-Pageviews: ${overviewData?.todaysPageViewCount}`" :charts="pageViewsCharts" @toggle-bar="toggleBar" />
                        <MetricsSection title="Questionviews" :sub-title="`Heutige Questionviews: ${overviewData?.todaysQuestionViewCount}`" :charts="questionViewsCharts" @toggle-bar="toggleBar" />
                        <MetricsSection title="Veröffentlichte Fragen" :sub-title="`Heute veröffentlicht: ${overviewData?.todaysPublishedQuestionCount}`" :charts="publishedQuestionsCharts" @toggle-bar="toggleBar" />
                        <MetricsSection title="Öffentliche Fragen" :sub-title="`Heute erstellt: ${overviewData?.todaysPublicQuestionCreatedCount}`" :charts="publicCreatedPagesCharts" @toggle-bar="toggleBar" />
                        <MetricsSection title="Private Fragen" :sub-title="`Heute erstellt: ${overviewData?.todaysPrivateQuestionCreatedCount}`" :charts="privateCreatedQuestionsCharts" @toggle-bar="toggleBar" />

                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
.metrics-header {
    height: 54px;
    margin-top: 20px;
    margin-bottom: 10px;
}
</style>
