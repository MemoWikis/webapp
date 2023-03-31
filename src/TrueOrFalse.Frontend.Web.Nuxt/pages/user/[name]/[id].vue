<script lang="ts" setup>
import { BreadcrumbItem } from '~~/components/header/breadcrumbItems';
import { ImageStyle } from '~~/components/image/imageStyleEnum'
import { Tab } from '~~/components/user/tabs/tabsEnum'
import { useUserStore } from '~~/components/user/userStore'

const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit
const userStore = useUserStore()

interface Overview {
    activityPoints: {
        total: number
        questionsInOtherWishknowledges: number
        questionsCreated: number
        publicWishknowledges: number
    }
    publicQuestionsCount: number
    publicTopicsCount: number
    privateQuestionsCount: number
    privateTopicsCount: number
    wuwiCount: number
}
interface Question {
    title: string
    encodedPrimaryTopicName: string
    primaryTopicId: number
    id: number
}
interface Topic {
    name: string
    encodedName: string
    id: number
    questionCount: number
}
interface Wuwi {
    questions: Question[]
    topics: Topic[]
}
interface User {
    id: number
    name: string
    wikiUrl?: string
    imageUrl: string
    reputationPoints: number
    rank: number
    showWuwi: boolean
    encodedName: string
}
interface ProfileData {
    user: User
    overview: Overview
    isCurrentUser: boolean
}

const { data: profile, refresh: refreshProfile } = await useFetch<ProfileData>(`/apiVue/VueUser/Get?id=${route.params.id ? route.params.id : userStore.id}`, {
    credentials: 'include',
    mode: 'no-cors',
    onRequest({ options }) {
        if (process.server) {
            options.headers = headers
            options.baseURL = config.public.serverBase
        }
    }
})

const { data: wuwi, refresh: refreshWuwi } = await useLazyFetch<Wuwi>(`/apiVue/VueUser/GetWuwi?id=${route.params.id ? route.params.id : userStore.id}`, {
    credentials: 'include',
    mode: 'no-cors',
    onRequest({ options }) {
        if (process.server) {
            options.headers = headers
            options.baseURL = config.public.serverBase
        }
    }
})

const tab = ref<Tab>()
const isCurrentUser = computed(() => {
    if (profile.value?.isCurrentUser && userStore.id == profile.value?.user.id)
        return true
    else return false
})

const badgeCount = ref(0)
const maxBadgeCount = ref(0)

interface Props {
    isSettingsPage?: boolean
}
const props = defineProps<Props>()

const emit = defineEmits(['setBreadcrumb'])
function handleBreadcrumb(t: Tab) {
    if (t == Tab.Settings) {
        history.pushState(null, 'Alle Nutzer', `/Nutzer`)
        const breadcrumbItem: BreadcrumbItem = {
            name: 'Einstellungen',
            url: `/Nutzer/Einstellungen`
        }
        emit('setBreadcrumb', [breadcrumbItem])
    }
    else if (profile.value?.user.id && profile.value.user.id > 0) {
        const breadcrumbItems: BreadcrumbItem[] = [
            {
                name: 'Nutzer',
                url: '/Nutzer'
            },
            {
                name: `${profile.value?.user.name}`,
                url: `/Nutzer/${profile.value?.user.name}/${profile.value?.user.id}/Einstellungen`
            }]
        emit('setBreadcrumb', breadcrumbItems)
    } else {
        emit('setBreadcrumb', [{ name: 'Fehler', url: '' }])
    }
}
onMounted(() => {
    tab.value = props.isSettingsPage && profile.value?.isCurrentUser ? Tab.Settings : Tab.Overview
    handleBreadcrumb(tab.value)
    watch(tab, (t) => {
        if (t)
            handleBreadcrumb(t)
    })
})

watch(() => userStore.isLoggedIn, () => {
    refreshWuwi()
    refreshProfile()
})

useHead(() => ({
    link: [
        {
            rel: 'canonical',
            href: `${config.public.serverBase}/${profile.value?.user.encodedName}/${profile.value?.user.id}`,
        },
    ],
    meta: [
        {
            property: 'og:title',
            content: profile.value?.user.encodedName
        },
    ]
}))

</script>

<template>
    <div class="container">
        <div class="row profile-container  main-page">
            <div class="col-xs-12 container" v-if="profile && profile.user.id > 0">
                <div class="row">
                    <div class="col-xs-12 profile-header ">
                        <Image :style="ImageStyle.Author" :url="profile.user.imageUrl" class="profile-picture hidden-xs" />
                        <Image :style="ImageStyle.Author" :url="profile.user.imageUrl"
                            class="profile-picture-small hidden-sm hidden-md hidden-lg" />

                        <div class="profile-header-info">
                            <h1>{{ profile.user.name }}</h1>
                            <div class="sub-info">
                                <b>{{ profile.user.reputationPoints }}</b> Reputationspunkte
                                <font-awesome-icon icon="fa-solid fa-circle-info" class="info-icon" />
                                (Rang {{ profile.user.rank }})
                                <NuxtLink class="link-to-all-users">
                                    Zur Übersicht aller Nutzer
                                </NuxtLink>
                            </div>
                            <div class="profile-btn-container">
                                <button class="memo-button btn btn-primary" v-if="profile.user.wikiUrl"
                                    :to="profile.user.wikiUrl">
                                    <NuxtLink>

                                        <font-awesome-icon icon="fa-solid fa-house-user" v-if="isCurrentUser" />
                                        <font-awesome-icon icon="fa-solid fa-house" v-else />
                                        Zu {{ isCurrentUser ? 'meinem' : `${profile.user.name}s` }} Wiki
                                    </NuxtLink>
                                </button>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <UserTabs :tab="tab" :badge-count="badgeCount" :max-badge-count="maxBadgeCount" @set-tab="tab = $event"
                        :is-current-user="isCurrentUser" />
                </div>

                <Transition>
                    <div v-show="tab == Tab.Overview" class="row content">
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

                            <NuxtLink to="/Globales-Wiki/1">Erfahre mehr über Reputationspunkte</NuxtLink>
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
                            <div class="main-counter-container">
                                <div class="count">
                                    <h1>{{ profile.overview.privateQuestionsCount }}</h1>
                                </div>
                                <div class="count-label">
                                    <div>
                                        Private Fragen <font-awesome-icon icon="fa-solid fa-lock" />
                                    </div>
                                </div>
                            </div>
                            <div class="divider"></div>

                            <div class="main-counter-container">
                                <div class="count">
                                    <h1>{{ profile.overview.privateTopicsCount }}</h1>
                                </div>
                                <div class="count-label">
                                    <div>
                                        Private Themen <font-awesome-icon icon="fa-solid fa-lock" />
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
                </Transition>
                <Transition>
                    <div v-show="tab == Tab.Wishknowledge">
                        <div v-if="wuwi && (profile.user.showWuwi || profile.isCurrentUser)">
                            <div v-if="!profile.user.showWuwi && profile.isCurrentUser">

                            </div>
                            <UserTabsWishknowledge :questions="wuwi.questions" :topics="wuwi.topics" keep-alive />
                        </div>
                        <div v-else></div>
                    </div>
                </Transition>
                <Transition>
                    <div v-show="tab == Tab.Badges">

                    </div>
                </Transition>
                <Transition v-if="userStore.isLoggedIn && profile.isCurrentUser">
                    <UserSettings v-show="tab == Tab.Settings" :image-url="profile.user.imageUrl"
                        @update-profile="refreshProfile" />
                </Transition>

            </div>
            <Error v-else />
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
        margin-right: 30px;
        min-width: 166px;
    }

    .profile-picture-small {
        width: 96px;
        height: 96px;
        margin-right: 30px;
        min-width: 96px;
    }

    .sub-info {
        font-size: 18px;
        margin-bottom: 10px;

        .info-icon {
            color: @memo-grey-light;
            margin-right: 4px;
        }

        .link-to-all-users {
            font-size: 14px;
        }
    }
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
