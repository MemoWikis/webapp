<script lang="ts" setup>
const { $logger } = useNuxtApp();

interface ViewsResult {
    dateTime: string;
    views: number;
}

interface GetAllDataResponse {
    registrationsCount: number;
    loginCount: number;
    createdPrivatizedTopicCount: number;
    createdPublicTopicCount: number;
    todayTopicViews: number;
    todayQuestionViews: number;
    viewsQuestions: ViewsResult[];
    viewsTopics: ViewsResult[];
    annualLogins: ViewsResult[];
    annualRegistrations: ViewsResult[];
    annualPublicCreatedTopics: ViewsResult[];
    annualPrivateCreatedTopics: ViewsResult[];
}

const { data: overviewData } = await useFetch<GetAllDataResponse>('/apiVue/Overview/GetAllData', {
    mode: 'cors',
    credentials: 'include',
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }]);
        throw createError({ statusMessage: context.error?.message });
    },
});

const showAnnualQuestionViewBarchart = ref(false);
const showAnnualTopicViewBarChart = ref(false);
const showAnnualCreatedPublicTopicBarchart = ref(false);
const showAnnualCreatedPrivateTopicBarchart = ref(false);
const showAnnualLoginBarchart = ref(false);
const showAnnualRegistrationBarchart = ref(false);

const viewTopicLabels = computed(() => overviewData.value?.viewsTopics?.map(v => v.dateTime) as string[]);
const viewTopicViews = computed(() => overviewData.value?.viewsTopics?.map(v => v.views) as number[]);

const viewQuestionLabels = computed(() => overviewData.value?.viewsQuestions?.map(v => v.dateTime) as string[]);
const viewQuestionViews = computed(() => overviewData.value?.viewsQuestions?.map(v => v.views) as number[]);

const annualLoginsLabels = computed(() => overviewData.value?.annualLogins?.map(v => v.dateTime) as string[]);
const annualLoginsCount = computed(() => overviewData.value?.annualLogins?.map(v => v.views) as number[]);

const annualRegistrationLabels = computed(() => overviewData.value?.annualRegistrations?.map(v => v.dateTime) as string[]);
const annualRegistrationCounts = computed(() => overviewData.value?.annualRegistrations?.map(v => v.views) as number[]);

const annualPublicCreatedTopicLabels = computed(() => overviewData.value?.annualPublicCreatedTopics?.map(v => v.dateTime) as string[]);
const annualPublicCreatedTopicCounts = computed(() => overviewData.value?.annualPublicCreatedTopics?.map(v => v.views) as number[]);

const annualPrivateCreatedTopicLabels = computed(() => overviewData.value?.annualPrivateCreatedTopics?.map(v => v.dateTime) as string[]);
const annualPrivateCreatedTopicCounts = computed(() => overviewData.value?.annualPrivateCreatedTopics?.map(v => v.views) as number[]);

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
                <button @click="showAnnualRegistrationBarchart = !showAnnualRegistrationBarchart;">
                    {{ showAnnualRegistrationBarchart ? 'Verstecken' : 'Jahresübersicht Registrierungen' }}
                </button>
            </div>
            <div v-if="showAnnualRegistrationBarchart">
                <LazyOverviewBarChart :labels="annualRegistrationLabels" :datasets="annualRegistrationCounts"
                    :title="'Jahresübersicht Registrierungen'" />
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
                <button @click="showAnnualLoginBarchart = !showAnnualLoginBarchart;">
                    {{ showAnnualLoginBarchart ? 'Verstecken' : 'Jahresübersicht Logins anzeigen' }}
                </button>
            </div>
            <div v-if="showAnnualLoginBarchart">
                <div class="row content">
                    <button @click="showAnnualLoginBarchart = !showAnnualLoginBarchart;">
                        {{ showAnnualQuestionViewBarchart ? 'Verstecken' : 'Jahresübersicht Logins' }}
                    </button>
                </div>
                <div v-if="showAnnualLoginBarchart">
                    <LazyOverviewBarChart :labels="annualLoginsLabels" :datasets="annualLoginsCount"
                        :title="'Jahresübersicht Logins'" />
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
                <button @click="showAnnualCreatedPrivateTopicBarchart = !showAnnualCreatedPrivateTopicBarchart;">
                    {{ showAnnualCreatedPrivateTopicBarchart ? 'Verstecken' : 'Jahresübersicht Created Private Topics'
                    }}
                </button>
            </div>
            <div v-if="showAnnualCreatedPrivateTopicBarchart">
                <LazyOverviewBarChart :labels="annualPrivateCreatedTopicLabels"
                    :datasets="annualPrivateCreatedTopicCounts" :title="'Jahresübersicht erstellte Private Topics'" />
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
                <button @click="showAnnualCreatedPublicTopicBarchart = !showAnnualCreatedPublicTopicBarchart;">
                    {{ showAnnualCreatedPublicTopicBarchart ? 'Verstecken' : 'Jahresübersicht Public Topics'
                    }}
                </button>
            </div>
            <div v-if="showAnnualCreatedPublicTopicBarchart">
                <LazyOverviewBarChart :labels="annualPublicCreatedTopicLabels"
                    :datasets="annualPublicCreatedTopicCounts" :title="'Jahresübersicht erstellte Public Topics'" />
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
                <button @click="showAnnualTopicViewBarChart = !showAnnualTopicViewBarChart;">
                    {{ showAnnualTopicViewBarChart ? 'Verstecken' : 'JahresübersichtDaten Topic Views anzeigen' }}
                </button>
            </div>
            <div v-if="showAnnualTopicViewBarChart">
                <LazyOverviewBarChart :labels="viewTopicLabels" :datasets="viewTopicViews"
                    :title="'Jahresübersicht Topic Views'" />
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
                <button @click="showAnnualQuestionViewBarchart = !showAnnualQuestionViewBarchart;">
                    {{ showAnnualQuestionViewBarchart ? 'Verstecken' : 'Jahresübersicht Question Views anzeigen' }}
                </button>
            </div>
            <div v-if="showAnnualQuestionViewBarchart">
                <LazyOverviewBarChart :labels="viewQuestionLabels" :datasets="viewQuestionViews"
                    :title="'Jahresübersicht Question Views'" />
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