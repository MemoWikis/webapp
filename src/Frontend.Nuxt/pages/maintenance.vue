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

interface JobStatusResponse {
    jobId: string
    status: string
    message: string
    progress: number
    operationName: string
}

interface RelationErrorItem {
    parentId: number
    errors: any[]
    relations: any[]
}

interface RelationErrorsResponse {
    success: boolean
    data: RelationErrorItem[]
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
const relationMethods = ref<MethodData[]>([])

// Simple button for showing relation errors
const showRelationErrorsButton = async () => {
    await loadRelationErrors()
}
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
const relationErrors = ref<RelationErrorItem[]>([])
const runningJobs = ref<Map<string, string>>(new Map())
const jobProgress = ref<Map<string, JobStatusResponse>>(new Map())

const executeMaintenanceOperation = async (operationUrl: string) => {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)

    // RemoveAdminRights is handled synchronously
    if (operationUrl === 'RemoveAdminRights') {
        const result = await $api<FetchResult<string>>(`/apiVue/VueMaintenance/${operationUrl}`, {
            body: data,
            method: 'POST',
            mode: 'cors',
            credentials: 'include'
        })

        if (result?.success) {
            userStore.isAdmin = false
            antiForgeryToken.value = undefined
            await navigateTo('/')
        }
        return
    }

    // All other operations are handled as background jobs
    const result = await $api<FetchResult<string>>(`/apiVue/VueMaintenance/${operationUrl}`, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result?.success) {
        const jobId = result.data
        runningJobs.value.set(jobId, operationUrl)
        resultMsg.value = `Job ${operationUrl} started. Checking status...`

        // Start polling for status
        await pollJobStatus(jobId)
    }
}

const pollJobStatus = async (jobId: string) => {
    const pollInterval = 2000 // 2 seconds
    const maxPolls = 300 // Maximum 10 minutes
    let pollCount = 0

    const poll = async () => {
        try {
            const result = await $api<JobStatusResponse>(`/apiVue/VueMaintenance/GetJobStatus?jobId=${jobId}`, {
                method: 'GET',
                mode: 'cors',
                credentials: 'include'
            })

            if (result.status === 'completed') {
                resultMsg.value = result.message
                runningJobs.value.delete(jobId)
                jobProgress.value.delete(jobId)
            } else if (result.status === 'failed') {
                resultMsg.value = `Job failed: ${result.message}`
                runningJobs.value.delete(jobId)
                jobProgress.value.delete(jobId)
            } else if (result.status === 'running') {
                jobProgress.value.set(jobId, result)
                resultMsg.value = `${result.message} (${result.progress}%)`
                pollCount++

                if (pollCount < maxPolls) {
                    setTimeout(poll, pollInterval)
                } else {
                    resultMsg.value = 'Job timeout - please check server logs'
                    runningJobs.value.delete(jobId)
                    jobProgress.value.delete(jobId)
                }
            } else if (result.status === 'not_found') {
                resultMsg.value = 'Job not found - it may have completed or failed'
                runningJobs.value.delete(jobId)
                jobProgress.value.delete(jobId)
            }
        } catch (error) {
            console.error('Error polling job status:', error)
            runningJobs.value.delete(jobId)
            jobProgress.value.delete(jobId)
        }
    }

    await poll()
}

const checkForRunningJobs = async () => {
    if (!isAdmin.value || !userStore.isAdmin) return

    try {
        const result = await $api<JobStatusResponse[]>('/apiVue/VueMaintenance/GetAllRunningJobs', {
            method: 'GET',
            mode: 'cors',
            credentials: 'include'
        })

        if (result && Array.isArray(result)) {
            // Clear current running jobs and repopulate from backend
            runningJobs.value.clear()
            jobProgress.value.clear()

            for (const job of result) {
                runningJobs.value.set(job.jobId, job.operationName)
                jobProgress.value.set(job.jobId, job)
            }
        }
    } catch (error) {
        console.error('Error checking for running jobs:', error)
    }
}

const emit = defineEmits(['setBreadcrumb'])

let jobPollingInterval: NodeJS.Timeout | null = null

onBeforeMount(() => {
    if (!isAdmin.value && !userStore.isAdmin)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    emit('setBreadcrumb', [{ name: 'Maintenance', url: '/Maintenance' }])
})

onMounted(() => {
    // Check for running jobs immediately
    checkForRunningJobs()

    // Set up periodic polling for running jobs every 5 seconds
    jobPollingInterval = setInterval(() => {
        checkForRunningJobs()
    }, 5000)
})

onBeforeUnmount(() => {
    // Clean up the polling interval
    if (jobPollingInterval) {
        clearInterval(jobPollingInterval)
        jobPollingInterval = null
    }
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

const relationErrorsLoaded = ref(false)

async function loadRelationErrors() {
    try {
        const result = await $api<RelationErrorsResponse>('/apiVue/VueMaintenance/ShowRelationErrors', {
            method: 'GET',
            mode: 'cors',
            credentials: 'include'
        })

        if (result.success) {
            relationErrors.value = result.data
            resultMsg.value = `Found ${result.data.length} pages with relation errors.`
            relationErrorsLoaded.value = true
        } else {
            resultMsg.value = 'Error loading relation errors.'
        }
    } catch (error) {
        console.error('Error loading relation errors:', error)
        resultMsg.value = 'Error loading relation errors.'
    }
}

async function healRelations(pageId: number) {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    if (pageId <= 0) {
        resultMsg.value = 'Please enter a valid page ID.'
        return
    }

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)
    data.append('pageId', pageId.toString())

    const result = await $api<FetchResult<string>>(`/apiVue/VueMaintenance/HealRelations`, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result.success) {
        resultMsg.value = result.data
        // Refresh relation errors if they are currently displayed
        if (relationErrors.value.length > 0) {
            await loadRelationErrors()
        }
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

            <!-- Active Jobs Status Panel -->
            <LayoutPanel v-if="runningJobs.size > 0" :title="$t('maintenance.activeJobs.title')" class="active-jobs-panel">
                <LayoutCard v-for="[jobId, operationName] in runningJobs.entries()" :key="jobId" :size="LayoutCardSize.Medium">
                    <div class="running-job">
                        <h4>{{ operationName }}</h4>
                        <div v-if="jobProgress.has(jobId)" class="job-progress">
                            <div class="progress-info">
                                <span>{{ jobProgress.get(jobId)?.message }}</span>
                                <span class="progress-percentage">{{ jobProgress.get(jobId)?.progress }}%</span>
                            </div>
                            <div class="progress-bar">
                                <div class="progress-fill" :style="{ width: `${jobProgress.get(jobId)?.progress || 0}%` }"></div>
                            </div>
                        </div>
                        <div v-else class="job-status">
                            <span>{{ $t('maintenance.activeJobs.starting') }}</span>
                        </div>
                    </div>
                </LayoutCard>
            </LayoutPanel>
            <LayoutPanel :title="$t('maintenance.metrics.title')">
                <NuxtLink to="/Metriken" class="memo-button btn btn-primary">
                    {{ $t('maintenance.metrics.viewOverview') }}
                </NuxtLink>
            </LayoutPanel>
            <MaintenanceSection :title="$t('maintenance.questions.title')" :methods="questionMethods" @method-clicked="executeMaintenanceOperation"
                :icon="['fas', 'retweet']" />
            <MaintenanceSection :title="$t('maintenance.cache.title')" :methods="cacheMethods" @method-clicked="executeMaintenanceOperation"
                :icon="['fas', 'retweet']" />
            <MaintenanceSection :title="$t('maintenance.pages.title')" :methods="pageMethods" @method-clicked="executeMaintenanceOperation"
                :icon="['fas', 'retweet']" />
            <LayoutPanel :title="$t('maintenance.relations.title')">

                <LayoutCard :size="LayoutCardSize.Large" :background-color="'transparent'">
                    <button @click="showRelationErrorsButton" class="memo-button btn btn-primary">
                        {{ $t('maintenance.relations.showErrors') }}
                    </button>
                </LayoutCard>

                <MaintenanceRelationErrorCard v-for="errorItem in relationErrors" :key="errorItem.parentId" :error-item="errorItem" @heal-relations="healRelations" />
                <div v-if="relationErrorsLoaded && relationErrors.length === 0" class="no-errors-message">
                    {{ $t('maintenance.relations.noErrorsFound') }}
                </div>
            </LayoutPanel>
            <MaintenanceSection :title="$t('maintenance.meiliSearch.title')" :methods="meiliSearchMethods" :description="$t('maintenance.meiliSearch.description')" @method-clicked="executeMaintenanceOperation" :icon="['fas', 'retweet']" />
            <MaintenanceSection :title="$t('maintenance.users.title')" :methods="userMethods" @method-clicked="executeMaintenanceOperation" :icon="['fas', 'retweet']">
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
            <MaintenanceSection :title="$t('maintenance.misc.title')" :methods="miscMethods" @method-clicked="executeMaintenanceOperation" :icon="['fas', 'retweet']" />
            <MaintenanceSection :title="$t('maintenance.tools.title')" :methods="toolsMethods" @method-clicked="executeMaintenanceOperation" :icon="['fas', 'hammer']" />
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

.found-errors-heading {
    margin-top: 48px;
}

.running-job {
    padding: 16px;
    border-left: 4px solid #007bff;
    background-color: #f8f9fa;

    h4 {
        margin: 0 0 12px 0;
        color: #495057;
    }
}

.job-progress {
    .progress-info {
        display: flex;
        justify-content: space-between;
        margin-bottom: 8px;
        font-size: 14px;
        color: #6c757d;
    }

    .progress-percentage {
        font-weight: bold;
        color: #007bff;
    }

    .progress-bar {
        width: 100%;
        height: 8px;
        background-color: #e9ecef;
        border-radius: 4px;
        overflow: hidden;

        .progress-fill {
            height: 100%;
            background-color: #007bff;
            transition: width 0.3s ease;
        }
    }
}

.job-status {
    font-size: 14px;
    color: #6c757d;
    font-style: italic;
}
</style>
