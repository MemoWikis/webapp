<script lang="ts" setup>
import { ImageStyle } from '~~/components/image/imageStyleEnum'
import { Tab } from '~~/components/user/tabs/tabsEnum';
import { useUserStore } from '~~/components/user/userStore'

const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit

interface Overview {
    activityPoints: {
        total: number
        questionsInOtherWishknowledges: number
        questionsCreated: number
        publicWishknowledges: number
    }
    publicQuestionsCount: number
    publicTopicsCount: number
    wuwiCount: number
}
interface Question {
    title: string
    encodedName: string
    id: number
}
interface Topic {
    name: string
    encodedName: string
    id: number
}
interface Wuwi {
    questions: Question[]
    topics: Topic[]
}
interface User {
    id: number
    name: string
    wikiUrl?: string
    imageUrl?: string
    reputationPoints: number
    rank: number
}
interface ProfileData {
    user: User
    overview: Overview
    wuwi: Wuwi
    isCurrentUser: boolean
}
const { data: profile } = await useFetch<ProfileData>(`/apiVue/VueUser/Get?id=${route.params.id}`, {
    credentials: 'include',
    mode: 'no-cors',
    onRequest({ options }) {
        if (process.server) {
            options.headers = headers
            options.baseURL = config.public.serverBase
        }
    }
})

const tab = ref<Tab>(Tab.Overview)
const userStore = useUserStore()
const isCurrentUser = computed(() => {
    if (profile.value?.isCurrentUser && userStore.id == profile.value?.user.id)
        return true
    else return false
})

const badgeCount = ref(0)
const maxBadgeCount = ref(0)

function setTab(t: Tab) {
    tab.value = t
}
</script>

<template>
    <div class="container main-page">
        <div class="row profile-container mt-45">
            <div class="col-xs-12 container" v-if="profile">
                <div class="row">
                    <div class="col-xs-12 profile-header ">
                        <Image :style="ImageStyle.Author" :url="''" class="profile-picture" />
                        <div class="profile-header-info">
                            <h1>{{ profile.user.name }}</h1>
                            <div>
                                {{ profile.user.reputationPoints }} Reputationspunkte (Rang {{ profile.user.rank }})
                                <NuxtLink>
                                    Zur Übersicht aller Nutzer
                                </NuxtLink>
                            </div>
                            <div>
                                <NuxtLink v-if="profile.user.wikiUrl" :to="profile.user.wikiUrl">
                                    <div class="memo-btn btn btn-primary">
                                        <font-awesome-icon icon="fa-solid fa-house-user" v-if="isCurrentUser" />
                                        <font-awesome-icon icon="fa-solid fa-house" v-else />
                                        Zu {{ isCurrentUser? 'meinem': `${profile.user.name}s` }} Wiki
                                    </div>
                                </NuxtLink>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <UserTabs :tab="tab" :badge-count="badgeCount" :max-badge-count="maxBadgeCount" @set-tab="setTab"
                        :is-current-user="isCurrentUser" />
                </div>

                <Transition>
                    <div v-if="tab == Tab.Overview" class="row content">
                        <div class="col-lg-4 col-sm-6 col-xs-12 overview-partial">

                            <div class="overline-s">
                                Reputationspunkte
                            </div>

                            <div class="main-counter-container">
                                <div class="count">
                                    <h1>{{ profile.overview.activityPoints.total }}</h1>
                                </div>
                                <div class="count-label">
                                    <div>gesamt</div>
                                </div>
                            </div>

                            <div class="divider"></div>

                            <div class="sub-counter-container">
                                <div class="count">
                                    {{ profile.overview.activityPoints.questionsInOtherWishknowledges }} P
                                </div>
                                <div class="count-label">Eigene Fragen im Wunschwissen anderer</div>
                            </div>
                            <div class="sub-counter-container">
                                <div class="count">
                                    {{ profile.overview.activityPoints.questionsCreated }} P
                                </div>
                                <div class="count-label">Erstellte Fragen</div>
                            </div>
                            <div class="sub-counter-container">
                                <div class="count">
                                    {{ profile.overview.activityPoints.publicWishknowledges }} P
                                </div>
                                <div class="count-label">Veröffentlichung des eigenes Wunschwissen</div>
                            </div>

                            <div class="divider"></div>

                        </div>
                        <div class="col-lg-4 col-sm-6 col-xs-12 overview-partial">

                            <div class="overline-s">
                                Erstellte Inhalte
                            </div>

                            <div class="main-counter-container">
                                <div class="count">
                                    <h1>{{ profile.overview.publicQuestionsCount }}</h1>
                                </div>
                                <div class="count-label">
                                    <div>
                                        Öffentliche Fragen
                                    </div>
                                </div>
                            </div>
                            <div class="divider"></div>

                            <div class="main-counter-container">
                                <div class="count">
                                    <h1>{{ profile.overview.publicTopicsCount }}</h1>
                                </div>
                                <div class="count-label">
                                    <div>
                                        Öffentliche Themen
                                    </div>
                                </div>
                            </div>
                            <div class="divider"></div>

                        </div>
                        <div class="col-lg-4 col-sm-6 col-xs-12 overview-partial">

                            <div class="overline-s">
                                Im Wunschwissen
                            </div>

                            <div class="main-counter-container">
                                <div class="count">
                                    <h1>{{ profile.overview.wuwiCount }}</h1>
                                </div>
                                <div class="count-label">
                                    <div>Fragen</div>
                                </div>

                            </div>
                            <div class="divider"></div>

                        </div>
                    </div>
                    <UserSettings v-else-if="tab == Tab.Settings" />
                </Transition>
            </div>
        </div>
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.content {

    .overview-partial {
        padding-top: 50px;
    }
}

.profile-header {
    display: flex;
    flex-direction: row;

    .profile-picture {
        width: 166px;
        height: 166px;
        margin-right: 10px;
    }

    .profile-header-info {}
}

.divider {
    height: 1px;
    background: @memo-grey-lighter;
    width: 100%;
    margin-bottom: 10px;
}

.overline-s {
    margin-bottom: 10px;
}

.main-counter-container {
    display: flex;
    flex-wrap: nowrap;

    margin: 0 20px;

    .count {
        margin-right: 10px;
        // width: 150px;
        // text-align: right;

        h1 {
            margin-top: 0px;
            color: @memo-grey-darker;
        }
    }

    .count-label {
        line-height: 54px;
        display: flex;
        flex-direction: column-reverse;
        color: @memo-grey-darker;
    }
}

.sub-counter-container {
    display: flex;
    flex-wrap: nowrap;
    margin: 0 20px;
    margin-bottom: 10px;


    .count {
        font-weight: 700;
        min-width: 70px;
        text-align: right;
        margin-right: 10px;
        color: @memo-grey-dark;
    }

    .count-label {
        color: @memo-grey-dark;
    }
}
</style>
