<script lang="ts" setup>
import { Page } from '~/components/shared/pageEnum'

const { $logger } = useNuxtApp()

interface ViewsResult {
    dateTime: string;
    views: number;
}

interface GetAllDataResponse {
    todaysRegistrationCount: number;
    todaysLoginCount: number;
    createdPrivateTopicCount: number;
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

const { data: overviewData } = await useFetch<GetAllDataResponse>('/apiVue/Metrics/GetAllData', {
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

const viewTopicLabels = computed(() => overviewData.value?.viewsTopics?.map(v => v.dateTime.split("T")[0]) as string[]);
const viewTopicViews = computed(() => overviewData.value?.viewsTopics?.map(v => v.views) as number[]);

const viewQuestionLabels = computed(() => overviewData.value?.viewsQuestions?.map(v => v.dateTime.split("T")[0]) as string[]);
const viewQuestionViews = computed(() => overviewData.value?.viewsQuestions?.map(v => v.views) as number[]);

const annualLoginsLabels = computed(() => overviewData.value?.annualLogins?.map(v => v.dateTime.split("T")[0]) as string[]);
const annualLoginsCount = computed(() => overviewData.value?.annualLogins?.map(v => v.views) as number[]);

const annualRegistrationLabels = computed(() => overviewData.value?.annualRegistrations?.map(v => v.dateTime.split("T")[0]) as string[]);
const annualRegistrationCounts = computed(() => overviewData.value?.annualRegistrations?.map(v => v.views) as number[]);

const annualPublicCreatedTopicLabels = computed(() => overviewData.value?.annualPublicCreatedTopics?.map(v => v.dateTime.split("T")[0]) as string[]);
const annualPublicCreatedTopicCounts = computed(() => overviewData.value?.annualPublicCreatedTopics?.map(v => v.views) as number[]);

const annualPrivateCreatedTopicLabels = computed(() => overviewData.value?.annualPrivateCreatedTopics?.map(v => v.dateTime.split("T")[0]) as string[]);
const annualPrivateCreatedTopicCounts = computed(() => overviewData.value?.annualPrivateCreatedTopics?.map(v => v.views) as number[]);

const emit = defineEmits(['setPage', 'setBreadcrumb'])
emit('setPage', Page.Metrics)
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

                        <div class="bar-section">
                            <h3>Registrierungen </h3>
                            <div class="bar-header">
                                <h4>Heutige Registrierungen:</h4>
                                <h4>{{ overviewData?.todaysRegistrationCount }}</h4>
                            </div>

                            <div class="bar-container">
                                <button class="memo-button btn-default"
                                    @click="showAnnualRegistrationBarchart = !showAnnualRegistrationBarchart">
                                    {{ showAnnualRegistrationBarchart ? 'Verstecken' : 'Jahresübersicht Registrierungen' }}
                                </button>
                                <LazyOverviewBarChart v-if="showAnnualRegistrationBarchart"
                                    :labels="annualRegistrationLabels"
                                    :datasets="annualRegistrationCounts"
                                    :title="'Jahresübersicht Registrierungen'" />
                            </div>
                        </div>

                        <div class="bar-section">
                            <h3>Logins</h3>
                            <div class="bar-header">
                                <h4>Heutige Logins:</h4>
                                <h4>{{ overviewData?.todaysLoginCount }}</h4>
                            </div>

                            <div class="bar-container">
                                <button class="memo-button btn-default" @click="showAnnualLoginBarchart = !showAnnualLoginBarchart;">
                                    {{ showAnnualLoginBarchart ? 'Verstecken' : 'Jahresübersicht Logins anzeigen' }}
                                </button>
                                <LazyOverviewBarChart v-if="showAnnualLoginBarchart"
                                    :labels="annualLoginsLabels"
                                    :datasets="annualLoginsCount"
                                    :title="'Jahresübersicht Logins'" />
                            </div>
                        </div>

                        <div class="bar-section">
                            <h3>Private Themen</h3>
                            <div class="bar-header">
                                <h4>Heute erstellt:</h4>
                                <h4>{{ overviewData?.createdPrivateTopicCount }}</h4>
                            </div>

                            <div class="bar-container">
                                <button class="memo-button btn-default" @click="showAnnualCreatedPrivateTopicBarchart = !showAnnualCreatedPrivateTopicBarchart;">
                                    {{ showAnnualCreatedPrivateTopicBarchart ? 'Verstecken' : 'Jahresübersicht Created Private Topics' }}
                                </button>
                                <LazyOverviewBarChart v-if="showAnnualCreatedPrivateTopicBarchart"
                                    :labels="annualPrivateCreatedTopicLabels"
                                    :datasets="annualPrivateCreatedTopicCounts"
                                    :title="'Jahresübersicht erstellte Private Topics'" />
                            </div>
                        </div>

                        <div class="bar-section">
                            <h3>Öffentliche Themen</h3>
                            <div class="bar-header">
                                <h4>Heute erstellt:</h4>
                                <h4>{{ overviewData?.createdPublicTopicCount }}</h4>
                            </div>

                            <div class="bar-container">
                                <button class="memo-button btn-default" @click="showAnnualCreatedPublicTopicBarchart = !showAnnualCreatedPublicTopicBarchart;">
                                    {{ showAnnualCreatedPublicTopicBarchart ? 'Verstecken' : 'Jahresübersicht Public Topics' }}
                                </button>
                                <LazyOverviewBarChart v-if="showAnnualCreatedPublicTopicBarchart"
                                    :labels="annualPublicCreatedTopicLabels"
                                    :datasets="annualPublicCreatedTopicCounts"
                                    :title="'Jahresübersicht erstellte Public Topics'" />

                            </div>
                        </div>

                        <div class="bar-section">
                            <h3>Topicviews</h3>
                            <div class="bar-header">
                                <h4>Heutige Topicviews:</h4>
                                <h4>{{ overviewData?.todayTopicViews }}</h4>
                            </div>

                            <div class="bar-container">
                                <button class="memo-button btn-default" @click="showAnnualTopicViewBarChart = !showAnnualTopicViewBarChart;">
                                    {{ showAnnualTopicViewBarChart ? 'Verstecken' : 'JahresübersichtDaten Topic Views anzeigen' }}
                                </button>
                                <LazyOverviewBarChart v-if="showAnnualTopicViewBarChart" :labels="viewTopicLabels"
                                    :datasets="viewTopicViews"
                                    :title="'Jahresübersicht Topic Views'" />
                            </div>
                        </div>

                        <div class="bar-section">
                            <h3>Questionviews</h3>
                            <div class="bar-header">
                                <h4>Heutige Questionviews:</h4>
                                <h4>{{ overviewData?.todayQuestionViews }}</h4>
                            </div>

                            <div class="bar-container">
                                <button class="memo-button btn-default" @click="showAnnualQuestionViewBarchart = !showAnnualQuestionViewBarchart;">
                                    {{ showAnnualQuestionViewBarchart ? 'Verstecken' : 'Jahresübersicht Question Views anzeigen' }}
                                </button>
                                <LazyOverviewBarChart v-if="showAnnualQuestionViewBarchart"
                                    :labels="viewQuestionLabels"
                                    :datasets="viewQuestionViews"
                                    :title="'Jahresübersicht Question Views'" />
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

.bar-header {
    display: flex;
    margin-bottom: 20px;
    justify-content: space-between;
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

.bar-section {
    margin-bottom: 45px;
}
</style>
