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
    url: string
}
interface Topic {
    name: string
    url: string
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
    return profile.value?.isCurrentUser && userStore.id == profile.value?.user.id
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
                <div class="profile-header row">
                    <div class="col-xs-12">
                        <Image :style="ImageStyle.Author" :url="''" class="profile-picture" />
                        <div>
                            <h1>{{ profile.user.name }}</h1>
                            <div>
                                {{ profile.user.reputationPoints }} Reputationspunkte
                                <NuxtLink>
                                    Zur Übersicht aller Nutzer
                                </NuxtLink>
                            </div>
                            <div>
                                <NuxtLink v-if="profile.user.wikiUrl" :to="profile.user.wikiUrl">
                                    <div class="memo-btn btn-default">
                                        <font-awesome-icon icon="fa-solid fa-house-user" v-if="isCurrentUser" />
                                        <font-awesome-icon icon="fa-solid fa-house" v-else />
                                        Zu {{ isCurrentUser? 'meinem': `${profile.user.name}s'` }} Wiki
                                    </div>
                                </NuxtLink>
                                <button @click="tab = Tab.Settings" v-if="isCurrentUser">
                                    <div class="memo-btn btn-link">
                                        Profil bearbeiten
                                    </div>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                <UserTabs :tab="tab" :badge-count="badgeCount" :max-badge-count="maxBadgeCount" @set-tab="setTab" />
                <Transition>
                    <div v-if="tab == Tab.Overview" class="row">
                        <div class="col-lg-3 col-md-6 col-xs-12">a</div>
                        <div class="col-lg-3 col-md-6 col-xs-12">b</div>
                        <div class="col-lg-3 col-md-6 col-xs-12">c</div>
                    </div>
                </Transition>
            </div>
        </div>
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.profile-header {
    display: flex;

    .profile-picture {
        width: 166px;
        height: 166px;
    }
}
</style>
