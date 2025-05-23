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
        loggedInUserCount.value = activeSessionsResult.value.loggedInUserCount;
        anonymousUserCount.value = activeSessionsResult.value.anonymousUserCount;
    }

})

interface MethodData {
    url: string
    label: string
}

interface ActiveSessionsResponse {
    loggedInUserCount: number,
    anonymousUserCount: number
}

const questionMethods = ref<MethodData[]>([
    { url: 'RecalculateAllKnowledgeItems', label: 'Recalculate all answer probabilities' },
    { url: 'CalcAggregatedValuesQuestions', label: 'Update aggregated numbers' }
])
const cacheMethods = ref<MethodData[]>([
    { url: 'ClearCache', label: 'Clear cache' },
])
const pageMethods = ref<MethodData[]>([
    { url: 'UpdateCategoryAuthors', label: 'Update page authors' }
])
const meiliSearchMethods = ref<MethodData[]>([
    { url: 'MeiliReIndexAllQuestions', label: 'Questions' },
    { url: 'MeiliReIndexAllQuestionsCache', label: 'Questions (Cache)' },
    { url: 'MeiliReIndexAllPages', label: 'Pages' },
    { url: 'MeiliReIndexAllPagesCache', label: 'Pages (Cache)' },
    { url: 'MeiliReIndexAllUsers', label: 'Users' },
    { url: 'MeiliReIndexAllUsersCache', label: 'Users (Cache)' }
])
const userMethods = ref<MethodData[]>([
    { url: 'UpdateUserReputationAndRankings', label: 'Rankings and Reputation + Aggregates' },
    { url: 'UpdateUserWishCount', label: 'Update desired knowledge counter' }
])
const miscMethods = ref<MethodData[]>([
    { url: 'CheckForDuplicateInteractionNumbers', label: 'Check for answers with the same Guid and InteractionNr' }
])
const toolsMethods = ref<MethodData[]>([
    { url: 'Throw500', label: 'Throw exception' },
    { url: 'ReloadListFromIgnoreCrawlers', label: 'Reload list of ignored crawlers' },
    { url: 'CleanUpWorkInProgressQuestions', label: 'Clean up work in progress questions' },
    { url: 'Start100TestJobs', label: 'Start 100 test jobs' },

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
    <div class="container">
        <div class="row main-page">
            <div class="main-content">
                <div class="col-xs-12"
                    v-if="isAdmin && userStore.isAdmin && antiForgeryToken != null && antiForgeryToken?.length > 0">
                    <h1>Admin Page</h1>
                    <div class="row">
                        <div class="alert alert-warning alert-dismissible" role="alert" v-if="resultMsg.length > 0">
                            <button type="button" class="close" data-dismiss="alert" aria-label="Close"
                                @click.prevent="resultMsg = ''"><span aria-hidden="true">&times;</span></button>
                            {{ resultMsg }}
                        </div>
                        <MaintenanceSection title="Metrics" :methods="[]">
                            <div class="custom-container">
                                <NuxtLink to="/Metriken" class="memo-button btn btn-primary">
                                    View Overview
                                </NuxtLink>
                            </div>
                        </MaintenanceSection>
                        <MaintenanceSection title="Questions" :methods="questionMethods" @method-clicked="handleClick"
                            :icon="['fas', 'retweet']" />
                        <MaintenanceSection title="Cache" :methods="cacheMethods" @method-clicked="handleClick"
                            :icon="['fas', 'retweet']" />
                        <MaintenanceSection title="Pages" :methods="pageMethods" @method-clicked="handleClick"
                            :icon="['fas', 'retweet']" />
                        <MaintenanceSection title="Search MeiliSearch" :methods="meiliSearchMethods"
                            description="Re-index all for search:" @method-clicked="handleClick"
                            :icon="['fas', 'retweet']" />
                        <MaintenanceSection title="Users" :methods="userMethods" @method-clicked="handleClick"
                            :icon="['fas', 'retweet']">
                            <div class="active-users-info">
                                <h4>Active Sessions</h4>
                                <ul>
                                    <li>Logged in: {{ loggedInUserCount }} (last 5 minutes)</li>
                                    <li>Anonymous: {{ anonymousUserCount }} (last minute)</li>
                                </ul>
                            </div>
                            <div class="delete-user-container">
                                <h4>Delete User (ID)</h4>
                                <div class="delete-user-input">
                                    <input v-model="userIdToDelete" />
                                    <button @click="deleteUser" class="memo-button btn btn-primary">
                                        Delete User
                                    </button>
                                </div>
                            </div>
                        </MaintenanceSection>
                        <MaintenanceSection title="Miscellaneous" :methods="miscMethods" @method-clicked="handleClick"
                            :icon="['fas', 'retweet']" />
                        <MaintenanceSection title="Tools" :methods="toolsMethods" @method-clicked="handleClick"
                            :icon="['fas', 'hammer']" />
                        <div class="remove-admin-rights-section col-xs-12 col-lg-6">
                            <h3>Remove Admin Rights for current user</h3>
                            <div>
                                <button @click="removeAdminRights" class="memo-button btn btn-primary">
                                    Remove Admin Rights for current user
                                </button>
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
