<script lang="ts" setup>
import { BreadcrumbItem } from '~~/components/header/breadcrumbItems'
import { ImageFormat } from '~~/components/image/imageFormatEnum.js'
import { Tab } from '~~/components/user/tabs/tabsEnum'
import { useUserStore } from '~~/components/user/userStore'
import { SiteType } from '~/components/shared/siteEnum'
import { ErrorCode } from '~/components/error/errorCodeEnum'
import { LayoutCardSize } from '~/composables/layoutCardSize'
import { LayoutGridSize } from '~/composables/layoutGridSize'
import { PageData } from '~/composables/missionControl/pageData'
import { color } from '~/constants/colors'

const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit
const userStore = useUserStore()
const { $logger, $urlHelper } = useNuxtApp()
const { t } = useI18n()

interface Props {
    tab?: Tab
}
const props = withDefaults(defineProps<Props>(), {
    tab: Tab.Overview,
})

interface Overview {
    activityPoints: {
        total: number
        questionsInOtherWishknowledges: number
        questionsCreated: number
        publicWishknowledges: number
    }
    publicQuestionsCount: number
    publicPagesCount: number
    privateQuestionsCount: number
    privatePagesCount: number
    wuwiCount: number
}
interface Question {
    title: string
    primaryPageName: string
    primaryPageId: number
    id: number
}
interface Page {
    name: string
    id: number
    questionCount: number
}
interface Wuwi {
    questions: Question[]
    pages: Page[]
}
interface User {
    id: number
    name: string
    wikiName: string
    wikiId?: number
    imageUrl: string
    reputationPoints: number
    rank: number
    showWuwi: boolean
}
interface ProfileData {
    user: User
    overview: Overview
    isCurrentUser: boolean
    messageKey?: string
    errorCode?: ErrorCode
    wikis?: PageData[]
}

const { data: profile, refresh: refreshProfile } = await useFetch<ProfileData>(`/apiVue/User/Get/${route.params.id ? route.params.id : userStore.id}`, {
    credentials: 'include',
    mode: 'no-cors',
    onRequest({ options }) {
        if (import.meta.server) {
            options.headers = new Headers(headers)
            options.baseURL = config.public.serverBase
        }
    },
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
    },
})

if (profile.value && profile.value.messageKey && profile.value?.messageKey != "") {

    $logger.warn(`User: ${profile.value.messageKey} route ${route.fullPath}`)
    throw createError({ statusCode: profile.value.errorCode, statusMessage: t(profile.value.messageKey) })
}

const { data: wuwi, refresh: refreshWuwi } = await useLazyFetch<Wuwi>(`/apiVue/User/GetWuwi/${route.params.id ? route.params.id : userStore.id}`, {
    credentials: 'include',
    mode: 'no-cors',
    onRequest({ options }) {
        if (import.meta.server) {
            options.headers = new Headers(headers)
            options.baseURL = config.public.serverBase
        }
    },
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
    },
})

const tab = ref<Tab>(props.tab)
const isCurrentUser = computed(() => {
    if (profile.value?.isCurrentUser && userStore.id === profile.value?.user.id)
        return true
    else return false
})

const badgeCount = ref(0)
const maxBadgeCount = ref(0)

const router = useRouter()

const emit = defineEmits(['setBreadcrumb', 'setPage'])

function handleBreadcrumb(tab: Tab) {
    if (profile.value && profile.value.user.id > 0 && tab === Tab.Wishknowledge) {
        const newPath = `${$urlHelper.getUserUrl(profile.value.user.name, profile.value.user.id)}/Wunschwissen`
        router.push({ path: newPath })

        const breadcrumbItems: BreadcrumbItem[] = [
            {
                name: t('url.user'),
                url: `/${t('url.user')}`
            },
            {
                name: `${profile.value.user.name}'s Wunschwissen`,
                url: `${$urlHelper.getUserUrl(profile.value.user.name, profile.value.user.id)}/Wunschwissen`
            }]
        emit('setBreadcrumb', breadcrumbItems)
    }
    else if (profile.value?.user.id && profile.value.user.id > 0 && tab === Tab.Overview) {
        const newPath = $urlHelper.getUserUrl(profile.value.user.name, profile.value.user.id)
        router.push({ path: newPath })

        const breadcrumbItems: BreadcrumbItem[] = [
            {
                name: t('url.user'),
                url: `/${t('url.user')}`
            },
            {
                name: `${profile.value.user.name}`,
                url: $urlHelper.getUserUrl(profile.value.user.name, profile.value.user.id)
            }]
        emit('setBreadcrumb', breadcrumbItems)
    } else {
        emit('setBreadcrumb', [{ name: 'Fehler', url: '' }])
    }
}

onMounted(() => {
    emit('setPage', SiteType.User)
    handleBreadcrumb(tab.value)
})

watch(tab, (t) => {
    if (t != undefined)
        handleBreadcrumb(t)
})

watch(() => userStore.isLoggedIn, () => {
    refreshWuwi()
    refreshProfile()
})
useHead(() => ({
    link: [
        {
            rel: 'canonical',
            href: profile.value ? `${config.public.officialBase}${$urlHelper.getUserUrl(profile.value.user.name, profile.value?.user.id)}` : '',
        },
    ],
    meta: [
        {
            property: 'og:title',
            content: profile.value ? $urlHelper.sanitizeUri(profile.value.user.name) : ''
        },
    ]
}))

userStore.$onAction(({ name, after }) => {
    if (name === 'logout') {
        after(async (loggedOut) => {
            // Handle logout actions if needed
        })
    }
})

const { isMobile } = useDevice()

</script>

<template>

    <div class="main-content" v-if="profile && profile.user.id > 0">
        <LayoutPanel :no-background="true">
            <div class="profile-header">
                <Image :format="ImageFormat.Author" :src="profile.user.imageUrl"
                    class="profile-picture hidden-xs" />
                <Image :format="ImageFormat.Author" :src="profile.user.imageUrl"
                    class="profile-picture-small hidden-sm hidden-md hidden-lg" />

                <div class="profile-header-info">
                    <h1>{{ profile.user.name }}</h1>
                    <div class="sub-info">
                        <font-awesome-icon :icon="['fas', 'star']" class="star" /> <b>{{ profile.user.reputationPoints }}</b> {{ t('user.profile.reputationPoints') }}
                        <font-awesome-icon icon="fa-solid fa-circle-info" class="info-icon" />
                        <p>
                            ({{ t('user.profile.rank') }} {{ profile.user.rank }})
                        </p>
                        <p>
                            <NuxtLink class="link-to-all-users" to="/Nutzer">
                                {{ t('user.profile.viewAllUsers') }}
                            </NuxtLink>
                        </p>
                    </div>
                </div>
            </div>
        </LayoutPanel>

        <LayoutPanel>
            <LayoutGrid :size="LayoutGridSize.Small" direction="row" title="Title 1">
                <LayoutCard :size="LayoutCardSize.Medium">
                    <LayoutCounter :value="1000" label="reputation" :icon="['fas', 'star']" :icon-color="color.memoYellow" />
                </LayoutCard>

                <!-- <NuxtLink class="link-to-all-users" to="/Nutzer">
                    {{ t('user.profile.viewAllUsers') }}
                </NuxtLink> -->
                <LayoutCard :size="LayoutCardSize.Medium">
                    <LayoutCounter :value="2" label="Rank" :icon="['fas', 'crown']" :icon-color="color.memoYellow" url-value="/Nutzer" />
                </LayoutCard>
                <LayoutCard :size="LayoutCardSize.Medium">
                    <LayoutCounter :value="31" label="Pages" :icon="['fas', 'file-lines']" :icon-color="color.memoBlue" />
                </LayoutCard>
                <LayoutCard :size="LayoutCardSize.Medium">
                    <LayoutCounter :value="15523" label="Questions" :icon="['fas', 'circle-question']" :icon-color="color.memoGreyDark" />
                </LayoutCard>
            </LayoutGrid>

            <LayoutGrid :size="LayoutGridSize.Small" direction="column" title="Title 2">

                <LayoutCard :size="LayoutCardSize.Flex">
                    <div class="sub-counter-container">
                        <div class="count">
                            {{ profile.overview.activityPoints.questionsInOtherWishknowledges }} P
                        </div>
                        <div class="count-label">{{ t('user.overview.reputation.questionsInOtherWishknowledges') }}</div>
                    </div>
                    <div class="sub-counter-container">
                        <div class="count">
                            {{ profile.overview.activityPoints.questionsCreated }} P
                        </div>
                        <div class="count-label">{{ t('user.overview.reputation.questionsCreated') }}</div>
                    </div>
                    <div class="sub-counter-container">
                        <div class="count">
                            {{ profile.overview.activityPoints.publicWishknowledges }} P
                        </div>
                        <div class="count-label">{{ t('user.overview.reputation.publicWishknowledges') }}</div>
                    </div>
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Flex">
                    <LayoutCounter
                        :value="profile.overview.activityPoints.total"
                        :label="t('user.overview.reputation.total')" />

                </LayoutCard>

                <!-- <NuxtLink to="/Globales-Wiki/1">{{ t('user.overview.reputation.learnMore') }}</NuxtLink> -->

            </LayoutGrid>


            <LayoutGrid :size="LayoutGridSize.Flex" title="Content">
                <LayoutCard :size="LayoutCardSize.Micro">
                    <LayoutCounter
                        :value="profile.overview.publicQuestionsCount"
                        :label="t('user.overview.content.publicQuestions')" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Micro">
                    <LayoutCounter
                        :value="profile.overview.publicPagesCount"
                        :label="t('user.overview.content.publicPages')" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Micro">
                    <LayoutCounter
                        :value="profile.overview.privateQuestionsCount"
                        :label="t('user.overview.content.privateQuestions')"
                        icon="fa-solid fa-lock" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Micro">
                    <LayoutCounter
                        :value="profile.overview.privatePagesCount"
                        :label="t('user.overview.content.privatePages')"
                        icon="fa-solid fa-lock" />
                </LayoutCard>

                <LayoutCard :size="LayoutCardSize.Micro">
                    <LayoutCounter :value="profile.overview.wuwiCount" :label="t('user.overview.wishknowledge.questions')" />
                </LayoutCard>
            </LayoutGrid>
        </LayoutPanel>
        <!-- <LayoutPanel :title="t('user.overview.wishknowledge.title')" v-if="profile.user.showWuwi || profile.isCurrentUser">
            <div v-if="!profile.user.showWuwi" class="wuwi-is-hidden">
                <template v-if="profile.isCurrentUser">
                    {{ t('user.wishknowledge.private.own') }}
                    <NuxtLink :to="`/${t('url.user')}/${t('url.settings')}`" class="btn-link">
                        {{ t('user.wishknowledge.private.change') }}
                    </NuxtLink>
                </template>
<template v-else>
                    <b>{{ t('user.wishknowledge.private.notPublic') }}</b> {{ t('user.wishknowledge.private.userNotPublished', { name: profile.user.name }) }}
                </template>
</div>
<div v-if="wuwi && (profile.user.showWuwi || profile.isCurrentUser)">
    <UserTabsWishknowledge :questions="wuwi.questions" :pages="wuwi.pages" />
</div>
</LayoutPanel> -->


        <LayoutPanel v-if="isMobile && profile.wikis" :title="t('missionControl.sections.wikis')">
            <MissionControlGrid :pages="profile.wikis" :no-pages-text="t('missionControl.pageTable.noWikis')" />
        </LayoutPanel>

        <LayoutPanel v-else-if="profile.wikis" :title="t('missionControl.sections.wikis')">
            <LayoutCard :no-padding="true">
                <MissionControlTable :pages="profile.wikis" :no-pages-text="t('missionControl.pageTable.noWikis')" />
            </LayoutCard>
        </LayoutPanel>
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.profile-header-info {
    display: flex;
    flex-direction: column;
    justify-content: center;

    h1 {
        margin-top: 0px;
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

        .star {
            color: @memo-yellow;
        }

        .info-icon {
            color: @memo-grey-light;
            margin-right: 4px;
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
        color: @memo-grey-darker;
    }

    .count-label {
        color: @memo-grey-dark;
    }
}

.wuwi-is-hidden {
    border: solid 1px @memo-grey-lighter;
    padding: 20px;
    width: 100%;
    margin-top: 20px;
}
</style>
