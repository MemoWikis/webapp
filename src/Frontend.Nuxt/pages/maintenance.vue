<script lang="ts" setup>
import { useUserStore } from '~~/components/user/userStore'

const headers = useRequestHeaders(['cookie']) as HeadersInit
const config = useRuntimeConfig()
const userStore = useUserStore()
const { $logger } = useNuxtApp()

const isAdmin = ref(false)
const antiForgeryToken = ref<string>()
const { data: maintenanceDataResult } = await useFetch<FetchResult<string>>('/apiVue/VueMaintenance/Get',
    {
        credentials: 'include',
        mode: 'cors',
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

if (maintenanceDataResult.value?.success) {
    isAdmin.value = true
    antiForgeryToken.value = maintenanceDataResult.value.data
}

const { data: activeSessionsResult } = await useFetch<ActiveSessionsResponse>('/apiVue/VueMaintenance/GetActiveSessions', {
    credentials: 'include',
    mode: 'cors',
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

const loggedInUserCount = ref(0)
const anonymousUserCount = ref(0)
watchEffect(() => {
    if (activeSessionsResult.value) {
        loggedInUserCount.value = activeSessionsResult.value.loggedInUserCount
        anonymousUserCount.value = activeSessionsResult.value.anonymousUserCount
    }

})

interface MethodData {
    url: string
    translationKey: string
}

interface ActiveSessionsResponse {
    loggedInUserCount: number,
    anonymousUserCount: number
}

const questionMethods = ref<MethodData[]>([
    { url: 'RecalculateAllKnowledgeItems', translationKey: 'maintenance.questions.recalculateAllKnowledgeItems' },
    { url: 'CalcAggregatedValuesQuestions', translationKey: 'maintenance.questions.calcAggregatedValues' }
])
const cacheMethods = ref<MethodData[]>([
    { url: 'ClearCache', translationKey: 'maintenance.cache.clearCache' },
])
const pageMethods = ref<MethodData[]>([
    { url: 'UpdateCategoryAuthors', translationKey: 'maintenance.pages.updateCategoryAuthors' }
])
const meiliSearchMethods = ref<MethodData[]>([
    { url: 'MeiliReIndexAllQuestions', translationKey: 'maintenance.meiliSearch.questions' },
    { url: 'MeiliReIndexAllQuestionsCache', translationKey: 'maintenance.meiliSearch.questionsCache' },
    { url: 'MeiliReIndexAllPages', translationKey: 'maintenance.meiliSearch.pages' },
    { url: 'MeiliReIndexAllPagesCache', translationKey: 'maintenance.meiliSearch.pagesCache' },
    { url: 'MeiliReIndexAllUsers', translationKey: 'maintenance.meiliSearch.users' },
    { url: 'MeiliReIndexAllUsersCache', translationKey: 'maintenance.meiliSearch.usersCache' }
])
const userMethods = ref<MethodData[]>([
    { url: 'UpdateUserReputationAndRankings', translationKey: 'maintenance.users.updateReputationAndRankings' },
    { url: 'UpdateUserWishCount', translationKey: 'maintenance.users.updateWishCount' }
])
const miscMethods = ref<MethodData[]>([
    { url: 'CheckForDuplicateInteractionNumbers', translationKey: 'maintenance.misc.checkDuplicateInteractions' }
])
const toolsMethods = ref<MethodData[]>([
    { url: 'Throw500', translationKey: 'maintenance.tools.throwException' },
    { url: 'ReloadListFromIgnoreCrawlers', translationKey: 'maintenance.tools.reloadIgnoreCrawlers' },
    { url: 'CleanUpWorkInProgressQuestions', translationKey: 'maintenance.tools.cleanupWorkInProgress' },
    { url: 'Start100TestJobs', translationKey: 'maintenance.tools.start100TestJobs' },

])
const resultMsg = ref('')

async function handleClick(url: string) {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)

    const result = await $api<FetchResult<string>>(`/apiVue/VueMaintenance/${url}`, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result?.success)
        resultMsg.value = result.data
}

const emit = defineEmits(['setBreadcrumb'])

onBeforeMount(() => {
    if (!isAdmin.value && !userStore.isAdmin)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    emit('setBreadcrumb', [{ name: 'Maintenance', url: '/Maintenance' }])
})

const userIdToDelete = ref(0)
async function deleteUser() {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)
    data.append('userId', userIdToDelete.value.toString())

    const result = await $api<FetchResult<string>>(`/apiVue/VueMaintenance/DeleteUser`, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result.success)
        resultMsg.value = result.data
}

async function removeAdminRights() {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)

    const result = await $api<FetchResult<string>>(`/apiVue/VueMaintenance/RemoveAdminRights`, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result.success) {
        userStore.isAdmin = false
        antiForgeryToken.value = undefined
        await navigateTo('/')
    }
}
</script>

<template>
    <div class="main-content"
        v-if="isAdmin && userStore.isAdmin && antiForgeryToken != null && antiForgeryToken?.length > 0">
        <h1>{{ $t('maintenance.title') }}</h1>
        <div class="">
            <div class="alert alert-warning alert-dismissible" role="alert" v-if="resultMsg.length > 0">
                <button type="button" class="close" data-dismiss="alert" aria-label="Close"
                    @click.prevent="resultMsg = ''"><span aria-hidden="true">&times;</span></button>
                {{ resultMsg }}
            </div>
            <LayoutPanel :title="$t('maintenance.metrics.title')">
                <NuxtLink to="/Metriken" class="memo-button btn btn-primary">
                    {{ $t('maintenance.metrics.viewOverview') }}
                </NuxtLink>
            </LayoutPanel>
            <MaintenanceSection :title="$t('maintenance.questions.title')" :methods="questionMethods" @method-clicked="handleClick"
                :icon="['fas', 'retweet']" />
            <MaintenanceSection :title="$t('maintenance.cache.title')" :methods="cacheMethods" @method-clicked="handleClick"
                :icon="['fas', 'retweet']" />
            <MaintenanceSection :title="$t('maintenance.pages.title')" :methods="pageMethods" @method-clicked="handleClick"
                :icon="['fas', 'retweet']" />
            <MaintenanceSection :title="$t('maintenance.meiliSearch.title')" :methods="meiliSearchMethods"
                :description="$t('maintenance.meiliSearch.description')" @method-clicked="handleClick"
                :icon="['fas', 'retweet']" />
            <MaintenanceSection :title="$t('maintenance.users.title')" :methods="userMethods" @method-clicked="handleClick"
                :icon="['fas', 'retweet']">
                <LayoutCard :size="LayoutCardSize.Tiny">
                    <div class="active-users-info">
                        <h4>{{ $t('maintenance.users.activeSessions') }}</h4>
                        <ul>
                            <li>{{ $t('maintenance.users.loggedIn') }}: {{ loggedInUserCount }} ({{ $t('maintenance.users.last5Minutes') }})</li>
                            <li>{{ $t('maintenance.users.anonymous') }}: {{ anonymousUserCount }} ({{ $t('maintenance.users.lastMinute') }})</li>
                        </ul>
                    </div>
                </LayoutCard>
                <LayoutCard :size="LayoutCardSize.Small">
                    <div class="delete-user-container">
                        <h4>{{ $t('maintenance.users.deleteUser') }}</h4>
                        <div class="delete-user-input">
                            <input v-model="userIdToDelete" />
                            <button @click="deleteUser" class="memo-button btn btn-primary">
                                {{ $t('maintenance.users.deleteUserButton') }}
                            </button>
                        </div>
                    </div>
                </LayoutCard>

            </MaintenanceSection>
            <MaintenanceSection :title="$t('maintenance.misc.title')" :methods="miscMethods" @method-clicked="handleClick"
                :icon="['fas', 'retweet']" />
            <MaintenanceSection :title="$t('maintenance.tools.title')" :methods="toolsMethods" @method-clicked="handleClick"
                :icon="['fas', 'hammer']" />
            <LayoutPanel :title="$t('maintenance.removeAdminRights.title')">
                <button @click="removeAdminRights" class="memo-button btn btn-primary">
                    {{ $t('maintenance.removeAdminRights.button') }}
                </button>
            </LayoutPanel>
        </div>
    </div>
</template>
<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.alert {
    position: relative;

    .close-button {
        position: absolute;
        right: 15px;
        top: 15px;
        cursor: pointer;
    }
}

.custom-container {
    padding: 15px;
}

.delete-user-container {
    padding: 15px;

    .delete-user-input {

        display: flex;
        align-items: center;
        flex-direction: row;

        input {
            border: solid 1px @memo-grey-light;
            padding: 7px;
            margin-right: 8px;
        }
    }
}

.active-users-info {
    padding: 15px;
}

.remove-admin-rights-section {
    border: solid 1px @memo-grey-lighter;
    padding: 8px;
    margin: 8px;

    h3,
    .description {
        padding: 6px 12px;
        margin-top: 0;
    }
}
</style>
