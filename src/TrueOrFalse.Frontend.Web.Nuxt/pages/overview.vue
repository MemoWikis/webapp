<script lang="ts" setup>
const { $logger } = useNuxtApp()
interface OverviewData {
    registrationsCount: number
    loginCount: number
    createdPrivatizedTopicCount: number
    createdPublicTopicCount: number
    viewsTopics: number
    viewsQuestions: number
}


const { data: overviewData } = await useFetch<OverviewData>('/apiVue/Overview/GetAllData', {
    mode: 'cors',
    credentials: 'include',
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        throw createError({ statusMessage: context.error?.message })
    },
})


</script>
<template>
    <div class="container">
        <div class="main-page">
            <div class="row content ">
                <div class="col-xs-12 col-sm-12 header">
                    <div>
                        <h1>Gesamtdaten Memucho </h1>
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
            <div class="row content">
                <div class="col-xs-12 col-sm-12 flex">
                    <h3>Heutige Logins: </h3>
                    <div>
                        <h3>{{ overviewData?.loginCount }}</h3>
                    </div>
                </div>
            </div>
            <div class="row content">
                <div class="col-xs-12 col-sm-12 flex">
                    <div>
                        <h3>erstellte private Themen: </h3>
                    </div>
                    <div>
                        <h3>{{ overviewData?.createdPrivatizedTopicCount }}</h3>
                    </div>
                </div>
            </div>
            <div class="row content">
                <div class="col-xs-12 col-sm-12 flex">
                    <h3>erstellte öffentliche Themen: </h3>
                    <div>
                        <h3>{{ overviewData?.createdPublicTopicCount }}</h3>
                    </div>
                </div>
            </div>
            <div class="row content">
                <div class="col-xs-12 col-sm-12 flex">
                    <h3>Views Topics: </h3>
                    <div>
                        <h3>{{ overviewData?.viewsTopics }}</h3>
                    </div>
                </div>
            </div>
            <div class="row content">
                <div class="col-xs-12 col-sm-12 flex">
                    <h3>Views Fragen: </h3>
                    <div>
                        <h3>{{ overviewData?.viewsQuestions }}</h3>
                    </div>
                </div>
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