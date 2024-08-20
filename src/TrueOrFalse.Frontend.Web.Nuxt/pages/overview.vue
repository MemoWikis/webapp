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
    yearlyPublicCreatedTopics: ViewsResult[];
    yearlyPrivateCreatedTopics: ViewsResult[];
}

const { data: overviewData } = await useFetch<OverviewRunJson>('/apiVue/Overview/GetAllData', {
    mode: 'cors',
    credentials: 'include',
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }]);
        throw createError({ statusMessage: context.error?.message });
    },
});

const showYearlyQuestionViewBarchart = ref(false);
const showYearlyTopicViewBarChart = ref(false);
const showYearlyCreatedPublicTopicBarchart = ref(false);
const showYearlyCreatedPrivateTopicBarchart = ref(false);
const showYearlyLoginBarchart = ref(false);
const showYearlyRegistrationBarchart = ref(false);

const viewTopicLabels = computed(() => overviewData.value?.viewsTopics?.map(v => v.dateTime) as string[]);
const viewTopicViews = computed(() => overviewData.value?.viewsTopics?.map(v => v.views) as number[]);

const viewQuestionLabels = computed(() => overviewData.value?.viewsQuestions?.map(v => v.dateTime) as string[]);
const viewQuestionViews = computed(() => overviewData.value?.viewsQuestions?.map(v => v.views) as number[]);

const yearlyLoginsLabels = computed(() => overviewData.value?.yearlyLogins?.map(v => v.dateTime) as string[]);
const yearlyLoginsCount = computed(() => overviewData.value?.yearlyLogins?.map(v => v.views) as number[]);

const yearlyRegistrationLabels = computed(() => overviewData.value?.yearlyRegistrations?.map(v => v.dateTime) as string[]);
const yearlyRegistrationCounts = computed(() => overviewData.value?.yearlyRegistrations?.map(v => v.views) as number[]);

const yearlyPublicCreatedTopicLabels = computed(() => overviewData.value?.yearlyPublicCreatedTopics?.map(v => v.dateTime) as string[]);
const yearlyPublicCreatedTopicCounts = computed(() => overviewData.value?.yearlyPublicCreatedTopics?.map(v => v.views) as number[]);

const yearlyPrivateCreatedTopicLabels = computed(() => overviewData.value?.yearlyPrivateCreatedTopics?.map(v => v.dateTime) as string[]);
const yearlyPrivateCreatedTopicCounts = computed(() => overviewData.value?.yearlyPrivateCreatedTopics?.map(v => v.views) as number[]);

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
            <div>
                <button @click="showYearlyRegistrationBarchart = !showYearlyRegistrationBarchart;">
                    {{ showYearlyRegistrationBarchart ? 'Verstecken' : 'Jährliche Daten Registrierungen' }}
                </button>
            </div>
            <div v-if="showYearlyRegistrationBarchart">
                <LazyOverviewBarChart :labels="yearlyRegistrationLabels" :datasets="yearlyRegistrationCounts"
                    :title="'jährliche Übersicht Registrierungen'" />
            </div>
            <div class="row content">
                <div class="col-xs-12 col-sm-12 flex">
                    <h3>Heutige Logins: </h3>
                    <div>
                        <h3>{{ overviewData?.loginCount }}</h3>
                    </div>
                </div>
            </div>
            <div>
                <button @click="showYearlyLoginBarchart = !showYearlyLoginBarchart;">
                    {{ showYearlyLoginBarchart ? 'Verstecken' : 'Jährliche Daten Logins anzeigen' }}
                </button>
            </div>
            <div v-if="showYearlyLoginBarchart">
                <div class="row content">
                    <button @click="showYearlyLoginBarchart = !showYearlyLoginBarchart;">
                        {{ showYearlyQuestionViewBarchart ? 'Verstecken' : 'Jährliche Daten Logins' }}
                    </button>
                </div>
                <div v-if="showYearlyLoginBarchart">
                    <LazyOverviewBarChart :labels="yearlyLoginsLabels" :datasets="yearlyLoginsCount"
                        :title="'jährliche Übersicht Logins'" />
                </div>
            </div>
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
            <div>
                <button @click="showYearlyCreatedPrivateTopicBarchart = !showYearlyCreatedPrivateTopicBarchart;">
                    {{ showYearlyCreatedPrivateTopicBarchart ? 'Verstecken' : 'Jährliche Daten Created Private Topics'
                    }}
                </button>
            </div>
            <div v-if="showYearlyCreatedPrivateTopicBarchart">
                <LazyOverviewBarChart :labels="yearlyPrivateCreatedTopicLabels"
                    :datasets="yearlyPrivateCreatedTopicCounts"
                    :title="'jährliche Übersicht erstellte Private Topics'" />
            </div>
            <div class="row content">
                <div class="col-xs-12 col-sm-12 flex">
                    <h3>Erstellte öffentliche Themen: </h3>
                    <div>
                        <h3>{{ overviewData?.createdPublicTopicCount }}</h3>
                    </div>
                </div>
            </div>
            <div>
                <button @click="showYearlyCreatedPublicTopicBarchart = !showYearlyCreatedPublicTopicBarchart;">
                    {{ showYearlyCreatedPublicTopicBarchart ? 'Verstecken' : 'Jährliche Daten Public Topics'
                    }}
                </button>
            </div>
            <div v-if="showYearlyCreatedPublicTopicBarchart">
                <LazyOverviewBarChart :labels="yearlyPublicCreatedTopicLabels"
                    :datasets="yearlyPublicCreatedTopicCounts" :title="'jährliche Übersicht erstellte Public Topics'" />
            </div>
            <div class="row content">
                <div class="col-xs-12 col-sm-12 flex">
                    <h3>Views Topics: </h3>
                    <div>
                        <h3>{{ overviewData?.todayTopicViews }}</h3>
                    </div>
                </div>
            </div>
            <div>
                <button @click="showYearlyTopicViewBarChart = !showYearlyTopicViewBarChart;">
                    {{ showYearlyTopicViewBarChart ? 'Verstecken' : 'Jährliche Daten Topic Views anzeigen' }}
                </button>
            </div>
            <div v-if="showYearlyTopicViewBarChart">
                <LazyOverviewBarChart :labels="viewTopicLabels" :datasets="viewTopicViews"
                    :title="'jährliche Übersicht Topic Views'" />
            </div>
            <div class="row content">
                <div class="col-xs-12 col-sm-12 flex">
                    <h3>Views Fragen: </h3>
                    <div>
                        <h3>{{ overviewData?.todayQuestionViews }}</h3>
                    </div>
                </div>
            </div>
            <div>
                <button @click="showYearlyQuestionViewBarchart = !showYearlyQuestionViewBarchart;">
                    {{ showYearlyQuestionViewBarchart ? 'Verstecken' : 'Jährliche Daten Question Views anzeigen' }}
                </button>
            </div>
            <div v-if="showYearlyQuestionViewBarchart">
                <LazyOverviewBarChart :labels="viewQuestionLabels" :datasets="viewQuestionViews"
                    :title="'jährliche Übersicht Question Views'" />
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