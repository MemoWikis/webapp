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

enum JobStatus {
    Running = 0,
    Completed = 1,
    Failed = 2,
    NotFound = 3
}

interface JobStatusResponse {
    jobId: string
    status: JobStatus
    message: string
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

const isAnalyzing = ref(false)

const questionMethods = ref<MethodData[]>([
    { url: 'RecalculateAllKnowledgeItems', translationKey: 'maintenance.questions.recalculateAllKnowledgeItems' },
    { url: 'CalcAggregatedValuesQuestions', translationKey: 'maintenance.questions.calcAggregatedValues' }
])
const cacheMethods = ref<MethodData[]>([
    { url: 'ClearCache', translationKey: 'maintenance.cache.clearCache' },
    { url: 'RefreshMmapCaches', translationKey: 'maintenance.cache.refreshMmapCaches' }
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
    { url: 'ClearAllJobs', translationKey: 'maintenance.tools.clearAllJobs' },
    { url: 'PollingTest5s', translationKey: 'maintenance.tools.pollingTest5s' },
    { url: 'PollingTest30s', translationKey: 'maintenance.tools.pollingTest30s' },
    { url: 'PollingTest120s', translationKey: 'maintenance.tools.pollingTest120s' },
])
const resultMsg = ref('')
const relationErrors = ref<RelationErrorItem[]>([])
const runningJobs = ref<Map<string, string>>(new Map())
const jobProgress = ref<Map<string, JobStatusResponse>>(new Map())

// Adaptive polling configuration
const FAST_POLL_INTERVAL = 2000 // 2 seconds when jobs are active
const SLOW_POLL_INTERVAL = 15000 // 15 seconds when no jobs are running
const userStartedJob = ref(false) // Track if user manually started a job
let currentPollInterval = SLOW_POLL_INTERVAL

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

        // Mark that user manually started a job
        userStartedJob.value = true

        // Switch to fast polling and immediately check for updates
        updatePollingInterval()

        // Immediately fetch all running jobs to get the latest status
        await checkForRunningJobs()
    }
}

const checkForRunningJobs = async () => {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        return

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)

    const result = await $api<JobStatusResponse[]>('/apiVue/VueMaintenance/GetAllRunningJobs', {
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
        body: data
    })

    if (result && Array.isArray(result)) {
        const previousJobCount = runningJobs.value.size
        const previousJobs = new Map(runningJobs.value)

        // Clear current running jobs and repopulate from backend
        runningJobs.value.clear()
        jobProgress.value.clear()

        for (const job of result) {
            runningJobs.value.set(job.jobId, job.operationName)
            jobProgress.value.set(job.jobId, job)

            // Update result message with the latest job status
            if (job.status === JobStatus.Running) {
                resultMsg.value = job.message
            }
        }

        // Check for jobs that were in previous list but not in current (they completed/failed)
        for (const [jobId, operationName] of previousJobs) {
            if (!runningJobs.value.has(jobId)) {
                // Job is no longer in the running list - it completed or failed
                resultMsg.value = `Job ${operationName} completed`
            }
        }

        // If this is the first call and we found jobs, switch to fast polling
        if (previousJobCount === 0 && result.length > 0) {
            userStartedJob.value = true
        }

        // Update polling interval based on job status
        updatePollingInterval()
    }
}

const updatePollingInterval = () => {
    const hasRunningJobs = runningJobs.value.size > 0
    const shouldUseFastPolling = userStartedJob.value || hasRunningJobs

    const newInterval = shouldUseFastPolling ? FAST_POLL_INTERVAL : SLOW_POLL_INTERVAL

    if (newInterval !== currentPollInterval) {
        currentPollInterval = newInterval

        // Restart the polling interval with new timing
        if (jobPollingInterval) {
            clearInterval(jobPollingInterval)
            jobPollingInterval = setInterval(() => {
                checkForRunningJobs()
            }, currentPollInterval)
        }
    }

    // Reset userStartedJob flag when no jobs are running
    if (!hasRunningJobs) {
        userStartedJob.value = false
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

    // Set up periodic polling with adaptive interval
    jobPollingInterval = setInterval(() => {
        checkForRunningJobs()
    }, currentPollInterval)
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
const relationAnalysisJobId = ref<string | null>(null)
let stopRelationJobWatcher: (() => void) | null = null

async function loadRelationErrors() {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    if (isAnalyzing.value) {
        return // Already analyzing
    }

    try {
        isAnalyzing.value = true
        // Step 1: Start the background analysis
        resultMsg.value = 'Starting relation analysis...'
        relationErrorsLoaded.value = false
        relationErrors.value = []

        const data = new FormData()
        data.append('__RequestVerificationToken', antiForgeryToken.value)

        const startResult = await $api<FetchResult<string>>(`/apiVue/VueMaintenance/StartRelationAnalysis`, {
            body: data,
            method: 'POST',
            mode: 'cors',
            credentials: 'include'
        })

        if (!startResult.success) {
            resultMsg.value = 'Error starting analysis.'
            return
        }

        relationAnalysisJobId.value = startResult.data
        resultMsg.value = 'Analysis in progress...'

        // Step 2: Set up watcher for job completion
        if (stopRelationJobWatcher) {
            stopRelationJobWatcher() // Stop any existing watcher
        }

        stopRelationJobWatcher = watch(jobProgress, (jobs) => {
            if (relationAnalysisJobId.value && jobs.has(relationAnalysisJobId.value)) {
                const job = jobs.get(relationAnalysisJobId.value)
                if (job) {
                    if (job.status === JobStatus.Completed) {
                        resultMsg.value = 'Analysis completed. Fetching results...'
                        fetchCachedRelationErrors()
                    } else if (job.status === JobStatus.Failed) {
                        resultMsg.value = `Analysis failed: ${job.message}`
                        relationAnalysisJobId.value = null
                        isAnalyzing.value = false
                        if (stopRelationJobWatcher) {
                            stopRelationJobWatcher()
                            stopRelationJobWatcher = null
                        }
                    } else if (job.status === JobStatus.Running) {
                        resultMsg.value = `Analysis in progress... ${job.message}`
                    }
                }
            }
        }, { deep: true })

    } catch (error) {
        console.error('Error in relation analysis flow:', error)
        resultMsg.value = 'Error during analysis flow.'
        relationAnalysisJobId.value = null
        if (stopRelationJobWatcher) {
            stopRelationJobWatcher()
            stopRelationJobWatcher = null
        }
    } finally {
        isAnalyzing.value = false
    }
    checkForRunningJobs()
}

const fetchCachedRelationErrors = async () => {

    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        return

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)
    const cachedResult = await $api<RelationErrorsResponse>('/apiVue/VueMaintenance/GetRelationErrors', {
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
        body: data
    })

    if (cachedResult.success) {
        relationErrors.value = cachedResult.data
        resultMsg.value = `Found ${cachedResult.data.length} pages with relation errors.`
        relationErrorsLoaded.value = true
    } else {
        resultMsg.value = 'Error loading cached results.'
    }

    relationAnalysisJobId.value = null
    if (stopRelationJobWatcher) {
        stopRelationJobWatcher()
        stopRelationJobWatcher = null
    }
}

async function clearRelationErrorsCache() {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    try {
        const data = new FormData()
        data.append('__RequestVerificationToken', antiForgeryToken.value)

        const result = await $api<FetchResult<string>>(`/apiVue/VueMaintenance/ClearRelationErrorsCache`, {
            body: data,
            method: 'POST',
            mode: 'cors',
            credentials: 'include'
        })

        if (result.success) {
            resultMsg.value = 'Cache cleared successfully.'
            relationErrors.value = []
            relationErrorsLoaded.value = false
        } else {
            resultMsg.value = 'Error clearing cache.'
        }
    } catch (error) {
        console.error('Error clearing cache:', error)
        resultMsg.value = 'Error clearing cache.'
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
        }
    }
}

const clearJob = async (jobId: string) => {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)
    data.append('jobId', jobId)

    const result = await $api<FetchResult<string>>(`/apiVue/VueMaintenance/ClearJob`, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result.success) {
        resultMsg.value = result.data
        // Remove the job from local state immediately for responsive UI
        runningJobs.value.delete(jobId)
        jobProgress.value.delete(jobId)
        // Also refresh the job list to sync with backend
        await checkForRunningJobs()
    }
}
interface MmapCacheStatus {
    exists: boolean
    lastModified: string
    sizeBytes: number
}

interface MmapCacheStatusData {
    pageViewsCache: MmapCacheStatus
    questionViewsCache: MmapCacheStatus
}

const mmapCacheStatus = ref<MmapCacheStatusData | null>(null)
const mmapCacheStatusLoaded = ref(false)

interface GetMmapCacheStatusResult {
    success: boolean
    data: string
}

const loadMmapCacheStatus = async () => {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)

    const result = await $api<GetMmapCacheStatusResult>(`/apiVue/VueMaintenance/GetMmapCacheStatus`, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result?.success) {
        mmapCacheStatus.value = JSON.parse(result.data) as MmapCacheStatusData
    }
    mmapCacheStatusLoaded.value = true
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
            <LayoutPanel v-if="runningJobs.size > 0" title="Active Jobs" class="active-jobs-panel">
                <LayoutCard v-for="[jobId, operationName] in runningJobs.entries()" :key="jobId" :size="LayoutCardSize.Small">
                    <div class="running-job">
                        <div class="job-header">
                            <h4>{{ operationName }}</h4>
                            <button @click="clearJob(jobId)" class="clear-job-btn" title="Clear Job">
                                <font-awesome-icon icon="fa-solid fa-xmark" />
                            </button>
                        </div>
                        <div v-if="jobProgress.has(jobId)" class="job-status">
                            <span>{{ jobProgress.get(jobId)?.message }}</span>
                        </div>
                        <div v-else class="job-status">
                            <span>Starting...</span>
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

            <LayoutPanel :title="$t('maintenance.mmapCache.title')">

                <LayoutCard :size="LayoutCardSize.Large" :background-color="'transparent'">
                    <button @click="loadMmapCacheStatus" class="memo-button btn btn-primary">
                        {{ $t('maintenance.mmapCache.loadStatus') }}
                    </button>
                </LayoutCard>
                <template v-if="mmapCacheStatus">
                    <LayoutCard :size="LayoutCardSize.Tiny" v-if="mmapCacheStatus.pageViewsCache" :title="$t('maintenance.mmapCache.pageViews')">
                        <ul v-if="mmapCacheStatus.pageViewsCache.exists">
                            <li>lastModified: <br /><b>{{ mmapCacheStatus.pageViewsCache.lastModified ? new Date(mmapCacheStatus.pageViewsCache.lastModified).toLocaleString() : 'N/A' }}</b></li>
                            <li>sizeBytes: <br /><b>{{ mmapCacheStatus.pageViewsCache.sizeBytes }}</b></li>
                        </ul>
                        <span v-else>{{ $t('maintenance.mmapCache.noPageViewsCacheFile') }}</span>
                    </LayoutCard>
                    <LayoutCard :size="LayoutCardSize.Tiny" v-if="mmapCacheStatus.questionViewsCache" :title="$t('maintenance.mmapCache.questionViews')">
                        <ul v-if="mmapCacheStatus.questionViewsCache.exists">
                            <li>lastModified: <br /><b>{{ mmapCacheStatus.questionViewsCache.lastModified ? new Date(mmapCacheStatus.questionViewsCache.lastModified).toLocaleString() : 'N/A' }}</b></li>
                            <li>sizeBytes: <br /><b>{{ mmapCacheStatus.questionViewsCache.sizeBytes }}</b></li>
                        </ul>
                        <span v-else>{{ $t('maintenance.mmapCache.noQuestionViewsCacheFile') }}</span>
                    </LayoutCard>
                </template>

                <div v-else-if="mmapCacheStatusLoaded && mmapCacheStatus == null" class="no-errors-message">
                    {{ $t('maintenance.relations.noErrorsFound') }}
                </div>
            </LayoutPanel>

            <LayoutPanel :title="$t('maintenance.relations.title')">
                <LayoutCard :size="LayoutCardSize.Large" :background-color="'transparent'">
                    <div class="relation-errors-controls">
                        <button @click="loadRelationErrors" class="memo-button btn btn-primary" :disabled="isAnalyzing">
                            <i v-if="isAnalyzing" class="fas fa-spinner fa-spin"></i>
                            {{ isAnalyzing ? 'Analyzing...' : 'Analyze and Show' }}
                        </button>
                        <button @click="clearRelationErrorsCache" class="memo-button btn btn-secondary ms-2" :disabled="isAnalyzing">
                            Clear Cache
                        </button>
                    </div>
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

    h4 {
        margin: 0 0 12px 0;
        color: @memo-grey-darker;
    }
}

.mmap-cache-controls {
    padding: 15px;
    display: flex;
    gap: 10px;

    .memo-button {
        margin-right: 0;
    }
}

.job-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 12px;

    h4 {
        margin: 0;
        color: @memo-grey-darker;
    }
}

.clear-job-btn {
    background: white;
    border: none;
    cursor: pointer;
    color: @memo-grey;
    font-size: 16px;
    padding: 4px 8px;
    border-radius: 22px;
    transition: all 0.2s ease;

    &:hover {
        color: @memo-wuwi-red;
        background: brightness(0.95);
    }

    &:active {
        background: brightness(0.9);
    }
}

.job-status {
    font-size: 14px;
    color: @memo-grey-darker;
    font-style: italic;
}

.relation-errors-controls {
    display: flex;
    flex-direction: row;
    gap: 1em;

    .btn-secondary {
        background-color: @memo-grey;
    }
}
</style>
