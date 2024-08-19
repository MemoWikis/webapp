<script lang="ts" setup>
const { $logger } = useNuxtApp();

interface ViewsResult {
    dateTime: string;
    views: number;
}

interface OverviewRunJson {
    registrationsCount: number;
    loginCount: number;
    createdPrivatizedTopicCount: number;
    createdPublicTopicCount: number;
    todayTopicViews: number;
    todayQuestionViews: number;
    viewsQuestions: ViewsResult[];
    viewsTopics: ViewsResult[];
    yearlyLogins: ViewsResult[];
    yearlyRegistrations: ViewsResult[];
}

const { data: overviewData } = await useFetch<OverviewRunJson>('/apiVue/Overview/GetAllData', {
    mode: 'cors',
    credentials: 'include',
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }]);
        throw createError({ statusMessage: context.error?.message });
    },
});

const showLazyBarChart = ref(false);
const viewTopicLabels = computed(() => overviewData.value?.viewsTopics?.map(v => v.dateTime) as string[]);
const viewTopicViews = computed(() => overviewData.value?.viewsTopics?.map(v => v.views) as number[]);

const viewQuestionLabels = computed(() => overviewData.value?.viewsQuestions?.map(v => v.dateTime) as string[]);
const viewQuestionViews = computed(() => overviewData.value?.viewsQuestions?.map(v => v.views) as number[]);

const yearlyLoginsLabels = computed(() => overviewData.value?.yearlyLogins?.map(v => v.dateTime) as string[]);
const yearlyLoginsCount = computed(() => overviewData.value?.yearlyLogins?.map(v => v.views) as number[]);

const yearlyRegistrationLabels = computed(() => overviewData.value?.yearlyRegistrations?.map(v => v.dateTime) as string[]);
const yearlyRegistrationCounts = computed(() => overviewData.value?.yearlyRegistrations?.map(v => v.views) as number[]);

function toggleLazyBarChart() {
    showLazyBarChart.value = !showLazyBarChart.value;
}
</script>
<template>
    <div class="container">
        <div class="main-page">
            <div class="row content">
                <div class="col-xs-12 col-sm-12 header">
                    <div>
                        <h1>Gesamtdaten Memucho</h1>
                    </div>
                </div>
            </div>
            <div class="divider"></div>

            <div class="row content">
                <div class="col-xs-12 col-sm-12 flex">
                    <div>
                        <h3>Heutige Registrierungen: </h3>
                    </div>
                    <div>
                        <h3>{{ overviewData?.registrationsCount }}</h3>
                    </div>
                </div>
            </div>

            <LazyOverviewBarChart :labels="yearlyRegistrationLabels" :datasets="yearlyRegistrationCounts"
                :title="'jährliche Übersicht Logins'" />

            <div class="row content">
                <div class="col-xs-12 col-sm-12 flex">
                    <h3>Heutige Logins: </h3>
                    <div>
                        <h3>{{ overviewData?.loginCount }}</h3>
                    </div>
                </div>
            </div>
            <LazyOverviewBarChart :labels="yearlyLoginsLabels" :datasets="yearlyLoginsCount"
                :title="'jährliche Übersicht Logins'" />

            <div class="row content">
                <div class="col-xs-12 col-sm-12 flex">
                    <div>
                        <h3>Erstellte private Themen: </h3>
                    </div>
                    <div>
                        <h3>{{ overviewData?.createdPrivatizedTopicCount }}</h3>
                    </div>
                </div>
            </div>
            <div class="row content">
                <div class="col-xs-12 col-sm-12 flex">
                    <h3>Erstellte öffentliche Themen: </h3>
                    <div>
                        <h3>{{ overviewData?.createdPublicTopicCount }}</h3>
                    </div>
                </div>
            </div>

            <div class="row content">
                <div class="col-xs-12 col-sm-12 flex">
                    <h3>Views Topics: </h3>
                    <div>
                        <h3>{{ overviewData?.todayTopicViews }}</h3>
                    </div>
                </div>
            </div>

            <LazyOverviewBarChart :labels="viewTopicLabels" :datasets="viewTopicViews"
                :title="'jährliche Übersicht Topic Views'" />

            <div class="row content">
                <div class="col-xs-12 col-sm-12 flex">
                    <h3>Views Fragen: </h3>
                    <div>
                        <h3>{{ overviewData?.todayQuestionViews }}</h3>
                    </div>
                </div>
            </div>
            <LazyOverviewBarChart :labels="viewQuestionLabels" :datasets="viewQuestionViews"
                :title="'jährliche Übersicht Question Views'" />

            <div class="row content">
                <button @click="toggleLazyBarChart">
                    {{ showLazyBarChart ? 'Verstecken' : 'Diagramm anzeigen' }}
                </button>
            </div>
        </div>
    </div>
</template>
<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.flex {
    display: flex;
    margin-bottom: 20px;
    justify-content: space-between;
}

.header {
    margin-bottom: 10px;
    display: flex;
    justify-content: center;
}

.divider {
    height: 1px;
    background: @memo-grey-lighter;
    width: 100%;
    margin-top: 10px;
    margin-bottom: 60px;
}
</style>