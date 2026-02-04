<script lang="ts" setup>
import { useUserStore } from '~~/components/user/userStore'
import { useSnackbarStore } from '~~/components/snackBar/snackBarStore'
import {
    type MethodData,
    type ActiveSessionsResponse,
    type JobStatusResponse,
    type JobSystemStatusResponse,
    type DatabaseJobResponse,
    type RelationErrorItem,
    type RelationErrorsResponse,
    type VueMaintenanceResult,
    type MmapCacheStatusData,
    type GetMmapCacheStatusResult,
    type QuartzJob,
    type WhitelistedModel,
    type AvailableModel,
    type ProviderModels,
    type GetWhitelistedModelsResponse,
    type GetAllProviderModelsResponse,
    type MaintenanceTabType,
    JobStatus
} from '~~/components/maintenance/maintenance.types'

const headers = useRequestHeaders(['cookie']) as HeadersInit
const config = useRuntimeConfig()
const userStore = useUserStore()
const snackbarStore = useSnackbarStore()
const { $logger } = useNuxtApp()
const route = useRoute()

// Helper function to show toast messages
type MessageType = 'success' | 'error' | 'warning' | 'info'
const showMessage = (message: string, type: MessageType = 'info') => {
    snackbarStore.showSnackbar({
        type,
        text: { message },
        duration: type === 'error' ? 8000 : 5000
    })
}

const activeTab = ref<MaintenanceTabType>(
    (route.query.tab as MaintenanceTabType) || 'general'
)

watch(activeTab, (newTab) => {
    navigateTo({ query: { tab: newTab } }, { replace: true })
})

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
    // { url: 'UpdateCategoryAuthors', translationKey: 'maintenance.pages.updateCategoryAuthors' } might not be used anymore????
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
    { url: 'Start100TestJobs', translationKey: 'maintenance.tools.start100TestJobs' },
    { url: 'PollingTest5s', translationKey: 'maintenance.tools.pollingTest5s' },
    { url: 'PollingTest30s', translationKey: 'maintenance.tools.pollingTest30s' },
    { url: 'PollingTest120s', translationKey: 'maintenance.tools.pollingTest120s' },
])
const relationErrors = ref<RelationErrorItem[]>([])
const runningJobs = ref<Map<string, string>>(new Map())
const jobProgress = ref<Map<string, JobStatusResponse>>(new Map())
const jobSystemStatus = ref<JobSystemStatusResponse | null>(null)
const databaseJobs = ref<DatabaseJobResponse[]>([])
const jobStatusLoaded = ref(false)

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

    // Add default parameter for ClearStuckJobs (2 hours)
    if (operationUrl === 'ClearStuckJobs') {
        data.append('maxHours', '2')
    }

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
        const jobTrackingId = result.data
        runningJobs.value.set(jobTrackingId, operationUrl)
        showMessage(`Job ${operationUrl} started. Checking status...`, 'info')

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
            runningJobs.value.set(job.jobTrackingId, job.operationName)
            jobProgress.value.set(job.jobTrackingId, job)

            // Update result message with the latest job status
            if (job.status === JobStatus.Running) {
                showMessage(job.message, 'info')
            }
        }

        // Check for jobs that were in previous list but not in current (they completed/failed)
        for (const [jobTrackingId, operationName] of previousJobs) {
            if (!runningJobs.value.has(jobTrackingId)) {
                // Job is no longer in the running list - it completed or failed
                showMessage(`Job ${operationName} completed`, 'success')
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
        showMessage(result.data, 'success')
}

// Token Management
const tokenUserId = ref(0)
const tokenAmount = ref(0)
const tokenType = ref<'subscription' | 'paid'>('subscription')

async function addTokensToUser() {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    if (tokenUserId.value <= 0) {
        showMessage('Please enter a valid user ID', 'error')
        return
    }

    if (tokenAmount.value <= 0) {
        showMessage('Please enter a valid amount greater than 0', 'error')
        return
    }

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)
    data.append('userId', tokenUserId.value.toString())
    data.append('amount', tokenAmount.value.toString())
    data.append('tokenType', tokenType.value)

    const result = await $api<VueMaintenanceResult>(`/apiVue/VueMaintenance/AddTokensToUser`, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result.success) {
        showMessage(result.data, 'success')
        tokenAmount.value = 0
    } else {
        showMessage(`Error: ${result.data}`, 'error')
    }
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
const relationAnalysisjobTrackingId = ref<string | null>(null)
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
        showMessage('Starting relation analysis...', 'info')
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
            showMessage('Error starting analysis.', 'error')
            return
        }

        relationAnalysisjobTrackingId.value = startResult.data
        showMessage('Analysis in progress...', 'info')

        // Step 2: Set up watcher for job completion
        if (stopRelationJobWatcher) {
            stopRelationJobWatcher() // Stop any existing watcher
        }

        stopRelationJobWatcher = watch(jobProgress, (jobs) => {
            if (relationAnalysisjobTrackingId.value && jobs.has(relationAnalysisjobTrackingId.value)) {
                const job = jobs.get(relationAnalysisjobTrackingId.value)
                if (job) {
                    if (job.status === JobStatus.Completed) {
                        showMessage('Analysis completed. Fetching results...', 'success')
                        fetchCachedRelationErrors()
                    } else if (job.status === JobStatus.Failed) {
                        showMessage(`Analysis failed: ${job.message}`, 'error')
                        relationAnalysisjobTrackingId.value = null
                        isAnalyzing.value = false
                        if (stopRelationJobWatcher) {
                            stopRelationJobWatcher()
                            stopRelationJobWatcher = null
                        }
                    } else if (job.status === JobStatus.Running) {
                        showMessage(`Analysis in progress... ${job.message}`, 'info')
                    }
                }
            }
        }, { deep: true })

    } catch (error) {
        console.error('Error in relation analysis flow:', error)
        showMessage('Error during analysis flow.', 'error')
        relationAnalysisjobTrackingId.value = null
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
        showMessage(`Found ${cachedResult.data.length} pages with relation errors.`, 'info')
        relationErrorsLoaded.value = true
    } else {
        showMessage('Error loading cached results.', 'error')
    }

    relationAnalysisjobTrackingId.value = null
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
            showMessage('Cache cleared successfully.', 'success')
            relationErrors.value = []
            relationErrorsLoaded.value = false
        } else {
            showMessage('Error clearing cache.', 'error')
        }
    } catch (error) {
        console.error('Error clearing cache:', error)
        showMessage('Error clearing cache.', 'error')
    }
}

async function healRelations(pageId: number) {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    if (pageId <= 0) {
        showMessage('Please enter a valid page ID.', 'error')
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
        showMessage(result.data, 'success')
        // Refresh relation errors if they are currently displayed
        if (relationErrors.value.length > 0) { /* empty */ }
    }
}

const clearJob = async (jobTrackingId: string) => {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)
    data.append('jobTrackingId', jobTrackingId)

    const result = await $api<FetchResult<string>>(`/apiVue/VueMaintenance/ClearJob`, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result.success) {
        showMessage(result.data, 'success')
        // Remove the job from local state immediately for responsive UI
        runningJobs.value.delete(jobTrackingId)
        jobProgress.value.delete(jobTrackingId)
        // Also refresh the job list to sync with backend
        await checkForRunningJobs()
    }
}

const mmapCacheStatus = ref<MmapCacheStatusData | null>(null)
const mmapCacheStatusLoaded = ref(false)

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

const clearStuckJobs = async () => {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    const stuckJobs = databaseJobs.value.filter(job => job.isStuck)
    showMessage(`Clearing ${stuckJobs.length} stuck jobs...`, 'info')

    try {
        // Step 1: Try to interrupt all stuck jobs in Quartz first
        const interruptPromises = stuckJobs.map(job => interruptQuartzJobByName(job.name))
        await Promise.allSettled(interruptPromises)

        // Step 2: Clear stuck jobs from database (jobs older than 2 hours)
        const data = new FormData()
        data.append('__RequestVerificationToken', antiForgeryToken.value)
        data.append('maxHours', '2')

        const result = await $api<VueMaintenanceResult>(`/apiVue/VueMaintenance/ClearStuckJobs`, {
            body: data,
            method: 'POST',
            mode: 'cors',
            credentials: 'include'
        })

        if (result?.success) {
            showMessage(`Cleared stuck jobs successfully (Quartz + Database): ${result.data}`, 'success')
            // Refresh all job information
            await loadQuartzJobs()
        } else {
            showMessage(`Failed to clear stuck jobs from database: ${result?.data || 'Unknown error'}`, 'warning')
        }
    } catch (error) {
        console.error('Error clearing stuck jobs:', error)
        showMessage(`Error clearing stuck jobs: ${error}`, 'error')
    }
}

const clearJobById = async (jobId: number) => {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    // Find the job name from the database jobs
    const job = databaseJobs.value.find(j => j.id === jobId)
    if (!job) {
        showMessage(`Job with ID ${jobId} not found.`, 'error')
        return
    }

    showMessage(`Clearing job "${job.name}" (ID: ${jobId})...`, 'info')

    try {
        // Step 1: Try to interrupt the Quartz job first (graceful cancellation)
        await interruptQuartzJobByName(job.name)

        // Step 2: Clear the database entry
        const data = new FormData()
        data.append('__RequestVerificationToken', antiForgeryToken.value)
        data.append('jobId', jobId.toString())

        const result = await $api<VueMaintenanceResult>(`/apiVue/VueMaintenance/ClearJobById`, {
            body: data,
            method: 'POST',
            mode: 'cors',
            credentials: 'include'
        })

        if (result?.success) {
            showMessage(`Job "${job.name}" cleared successfully (Quartz + Database)`, 'success')
            // Refresh all job information
            await loadQuartzJobs()
        } else {
            showMessage(`Database clear failed for job "${job.name}": ${result?.data || 'Unknown error'}`, 'warning')
        }
    } catch (error) {
        console.error('Error clearing job:', error)
        showMessage(`Error clearing job "${job.name}": ${error}`, 'error')
    }
}

const _clearJobsByIds = async (jobIds: number[]) => {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    const jobsToClear = databaseJobs.value.filter(job => jobIds.includes(job.id))
    showMessage(`Clearing ${jobsToClear.length} selected jobs...`, 'info')

    try {
        // Step 1: Try to interrupt all selected jobs in Quartz first
        const interruptPromises = jobsToClear.map(job => interruptQuartzJobByName(job.name))
        await Promise.allSettled(interruptPromises)

        // Step 2: Clear selected jobs from database
        const data = new FormData()
        data.append('__RequestVerificationToken', antiForgeryToken.value)
        data.append('jobIds', jobIds.join(','))

        const result = await $api<VueMaintenanceResult>(`/apiVue/VueMaintenance/ClearJobsByIds`, {
            body: data,
            method: 'POST',
            mode: 'cors',
            credentials: 'include'
        })

        if (result?.success) {
            showMessage(`Cleared selected jobs successfully (Quartz + Database): ${result.data}`, 'success')
            // Refresh all job information
            await loadQuartzJobs()
        } else {
            showMessage(`Failed to clear selected jobs from database: ${result?.data || 'Unknown error'}`, 'warning')
        }
    } catch (error) {
        console.error('Error clearing selected jobs:', error)
        showMessage(`Error clearing selected jobs: ${error}`, 'error')
    }
}

const quartzJobs = ref<QuartzJob[]>([])
const quartzJobsLoaded = ref(false)

const loadQuartzJobs = async () => {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)

    // Load Quartz jobs
    const quartzResult = await $api<VueMaintenanceResult>(`/apiVue/VueMaintenance/GetQuartzJobs`, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    // Load complete job system status (in-memory + database jobs)
    const jobSystemResult = await $api<JobSystemStatusResponse>('/apiVue/VueMaintenance/GetJobSystemStatus', {
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
        body: data
    })

    if (quartzResult?.success) {
        quartzJobs.value = JSON.parse(quartzResult.data)
        quartzJobsLoaded.value = true

        // Update job system status (includes both in-memory and database jobs)
        if (jobSystemResult) {
            jobSystemStatus.value = jobSystemResult
            jobStatusLoaded.value = true

            // Update running jobs from in-memory jobs
            runningJobs.value.clear()
            jobProgress.value.clear()

            for (const job of jobSystemResult.inMemoryJobs) {
                runningJobs.value.set(job.jobTrackingId, job.operationName)
                jobProgress.value.set(job.jobTrackingId, {
                    jobTrackingId: job.jobTrackingId,
                    status: parseInt(job.status) as JobStatus,
                    message: job.message,
                    operationName: job.operationName
                })
            }

            // Update database jobs reference
            databaseJobs.value = jobSystemResult.databaseJobs

            const totalJobs = jobSystemResult.inMemoryJobs.length + jobSystemResult.databaseJobs.length
            showMessage(`Loaded ${quartzJobs.value.length} Quartz jobs, ${jobSystemResult.inMemoryJobs.length} in-memory jobs, and ${jobSystemResult.databaseJobs.length} database jobs (${totalJobs} total active jobs).`, 'info')
        } else {
            showMessage(`Loaded ${quartzJobs.value.length} Quartz jobs.`, 'info')
        }
    } else {
        showMessage('Failed to load Quartz jobs.', 'error')
    }
}

onMounted(() => {
    loadQuartzJobs()
})

const interruptQuartzJob = async (jobName: string, jobGroup?: string) => {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)
    data.append('jobName', jobName)
    if (jobGroup) data.append('jobGroup', jobGroup)

    const result = await $api<VueMaintenanceResult>(`/apiVue/VueMaintenance/InterruptQuartzJob`, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result?.success) {
        showMessage(result.data, 'success')
        // Refresh jobs list
        await loadQuartzJobs()
    } else {
        showMessage('Failed to interrupt Quartz job.', 'error')
    }
}

const interruptQuartzJobByName = async (jobName: string) => {
    try {
        if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
            return false

        const data = new FormData()
        data.append('__RequestVerificationToken', antiForgeryToken.value)
        data.append('jobName', jobName)

        const result = await $api<VueMaintenanceResult>(`/apiVue/VueMaintenance/InterruptQuartzJob`, {
            body: data,
            method: 'POST',
            mode: 'cors',
            credentials: 'include'
        })

        return result?.success || false
    } catch (error) {
        console.warn(`Failed to interrupt Quartz job "${jobName}":`, error)
        return false
    }
}

// AI Models Management
const whitelistedModels = ref<WhitelistedModel[]>([])
const providerModels = ref<ProviderModels[]>([])
const fetchingModels = ref(false)
const showDeleteConfirmModal = ref(false)
const modelToDelete = ref<WhitelistedModel | null>(null)
const editingCostRate = ref<{ id: number, value: string } | null>(null)
const editingDisplayName = ref<{ id: number, value: string } | null>(null)
const editingPrices = ref<{ id: number, inputPrice: string, outputPrice: string } | null>(null)

// Helper function to parse price strings (handles both dot and comma as decimal separator)
const parsePrice = (value: string): number => {
    const normalized = value.replace(',', '.')
    const parsed = parseFloat(normalized)
    return isNaN(parsed) ? 0 : parsed
}

const formatDecimalForServer = (value: string): string => {
    // Server uses German culture - use comma as decimal separator
    const parsed = parsePrice(value)
    return parsed.toString().replace('.', ',')
}

const loadWhitelistedModels = async () => {
    const result = await $api<GetWhitelistedModelsResponse>('/apiVue/VueMaintenance/GetWhitelistedAiModels', {
        method: 'GET',
        mode: 'cors',
        credentials: 'include'
    })

    if (result?.success) {
        whitelistedModels.value = result.models
    }
}

const fetchAllProviderModels = async () => {
    if (!antiForgeryToken.value) return

    fetchingModels.value = true
    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)

    const result = await $api<GetAllProviderModelsResponse>('/apiVue/VueMaintenance/FetchAllProviderModels', {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    fetchingModels.value = false

    if (result?.success) {
        providerModels.value = result.providers
        const totalModels = result.providers.reduce((sum, provider) => sum + provider.models.length, 0)
        showMessage(`Fetched ${totalModels} models from ${result.providers.length} providers`, 'success')
    } else {
        showMessage(`Error fetching models: ${result?.error || 'Unknown error'}`, 'error')
    }
}

const confirmDeleteModel = (model: WhitelistedModel) => {
    modelToDelete.value = model
    showDeleteConfirmModal.value = true
}

const cancelDelete = () => {
    showDeleteConfirmModal.value = false
    modelToDelete.value = null
}

const executeDelete = async () => {
    if (!antiForgeryToken.value || !modelToDelete.value) return

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)
    data.append('id', modelToDelete.value.id.toString())

    const result = await $api<VueMaintenanceResult>('/apiVue/VueMaintenance/RemoveFromWhitelistById', {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result?.success) {
        // Remove from whitelisted models
        whitelistedModels.value = whitelistedModels.value.filter(model => model.id !== modelToDelete.value!.id)

        // Also update the provider models list if loaded
        providerModels.value.forEach(provider => {
            const model = provider.models.find(model => model.modelId === modelToDelete.value!.modelId)
            if (model) {
                model.isWhitelisted = false
            }
        })
        showMessage('Model removed from whitelist', 'success')
    } else {
        showMessage(`Error: ${result?.data || 'Unknown error'}`, 'error')
    }

    showDeleteConfirmModal.value = false
    modelToDelete.value = null
}

const startEditCostRate = (model: WhitelistedModel) => {
    editingCostRate.value = { id: model.id, value: model.tokenCostMultiplier.toString() }
}

const saveCostRate = async (costRate?: { id: number, value: string }) => {
    const targetCostRate = costRate ?? editingCostRate.value
    if (!antiForgeryToken.value || !targetCostRate) return

    const parsedCostRate = parsePrice(targetCostRate.value)
    const serverCostRate = formatDecimalForServer(targetCostRate.value)

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)
    data.append('id', targetCostRate.id.toString())
    data.append('tokenCostMultiplier', serverCostRate)

    try {
        const response = await fetch('/apiVue/VueMaintenance/UpdateWhitelistCostRate', {
            method: 'POST',
            body: data,
            credentials: 'include'
        })

        if (!response.ok) {
            showMessage(`Error: Failed to save cost rate (${response.status})`, 'error')
            return
        }

        const result = await response.json() as VueMaintenanceResult

        if (result?.success) {
            // Find the model in the flat list
            const model = whitelistedModels.value.find(model => model.id === targetCostRate.id)
            if (model) {
                model.tokenCostMultiplier = parsedCostRate
            }
            showMessage('Cost rate updated', 'success')
        } else {
            showMessage(`Error: ${result?.data || 'Unknown error'}`, 'error')
        }
    } catch {
        showMessage('Error: Failed to save cost rate', 'error')
    }

    editingCostRate.value = null
}

const cancelEditCostRate = () => {
    editingCostRate.value = null
}

const startEditDisplayName = (model: WhitelistedModel) => {
    editingDisplayName.value = { id: model.id, value: model.displayName }
}

const saveDisplayName = async () => {
    if (!antiForgeryToken.value || !editingDisplayName.value) return

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)
    data.append('id', editingDisplayName.value.id.toString())
    data.append('displayName', editingDisplayName.value.value)

    const result = await $api<VueMaintenanceResult>('/apiVue/VueMaintenance/UpdateWhitelistDisplayName', {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result?.success) {
        const model = whitelistedModels.value.find(model => model.id === editingDisplayName.value!.id)
        if (model) {
            model.displayName = editingDisplayName.value.value
        }
        showMessage('Display name updated', 'success')
    } else {
        showMessage(`Error: ${result?.data || 'Unknown error'}`, 'error')
    }

    editingDisplayName.value = null
}

const cancelEditDisplayName = () => {
    editingDisplayName.value = null
}

const startEditPrices = (model: WhitelistedModel) => {
    editingPrices.value = {
        id: model.id,
        inputPrice: model.inputPricePerMillion.toString(),
        outputPrice: model.outputPricePerMillion.toString()
    }
}

const savePrices = async (prices?: { id: number, inputPrice: string, outputPrice: string }) => {
    const targetPrices = prices ?? editingPrices.value
    if (!antiForgeryToken.value || !targetPrices) {
        return
    }

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)
    data.append('id', targetPrices.id.toString())
    data.append('inputPricePerMillion', formatDecimalForServer(targetPrices.inputPrice))
    data.append('outputPricePerMillion', formatDecimalForServer(targetPrices.outputPrice))

    try {
        const response = await fetch('/apiVue/VueMaintenance/UpdateWhitelistPrices', {
            method: 'POST',
            body: data,
            credentials: 'include'
        })

        if (!response.ok) {
            showMessage(`Error: Failed to save prices (${response.status})`, 'error')
            return
        }

        const result = await response.json() as VueMaintenanceResult

        if (result?.success) {
            await loadWhitelistedModels()
            showMessage('Prices updated', 'success')
        } else {
            showMessage(`Error: ${result?.data || 'Unknown error'}`, 'error')
        }
    } catch {
        showMessage('Error: Failed to save prices', 'error')
    }

    editingPrices.value = null
}

const cancelEditPrices = () => {
    editingPrices.value = null
}

const toggleWhitelist = async (providerName: string, model: AvailableModel) => {
    if (!antiForgeryToken.value) return

    if (model.isWhitelisted) {
        // Find the whitelisted model to get its database ID
        const whitelistedModel = whitelistedModels.value.find(wm => wm.modelId === model.modelId)
        if (!whitelistedModel) {
            showMessage('Error: Model not found in whitelist', 'error')
            return
        }

        // Remove from whitelist using the database ID
        const data = new FormData()
        data.append('__RequestVerificationToken', antiForgeryToken.value)
        data.append('id', whitelistedModel.id.toString())

        const result = await $api<VueMaintenanceResult>('/apiVue/VueMaintenance/RemoveFromWhitelistById', {
            body: data,
            method: 'POST',
            mode: 'cors',
            credentials: 'include'
        })

        if (result?.success) {
            model.isWhitelisted = false
            // Remove from whitelisted models list
            whitelistedModels.value = whitelistedModels.value.filter(whitelistedModel => whitelistedModel.modelId !== model.modelId)
            showMessage(`${model.displayName} removed from whitelist`, 'success')
        } else {
            showMessage(`Error: ${result?.data || 'Unknown error'}`, 'error')
        }
    } else {
        // Add to whitelist
        const data = new FormData()
        data.append('__RequestVerificationToken', antiForgeryToken.value)
        data.append('modelId', model.modelId)
        data.append('modelProvider', providerName)
        data.append('displayName', model.displayName)

        const result = await $api<VueMaintenanceResult>('/apiVue/VueMaintenance/AddToWhitelist', {
            body: data,
            method: 'POST',
            mode: 'cors',
            credentials: 'include'
        })

        if (result?.success) {
            model.isWhitelisted = true
            await loadWhitelistedModels() // Reload to get the new ID
            showMessage(`${model.displayName} added to whitelist`, 'success')
        } else {
            showMessage(`Error: ${result?.data || 'Unknown error'}`, 'error')
        }
    }
}

// Load whitelisted models on mount
onMounted(() => {
    loadWhitelistedModels()
})
</script>

<template>
    <div v-if="isAdmin && userStore.isAdmin && antiForgeryToken != null && antiForgeryToken?.length > 0"
        class="main-content">
        <h1>{{ $t('maintenance.title') }}</h1>
        <div class="">

            <!-- Tab Navigation -->
            <div class="maintenance-tabs">
                <button class="tab-button" :class="{ active: activeTab === 'general' }" @click="activeTab = 'general'">
                    <font-awesome-icon :icon="['fas', 'cogs']" />
                    General
                </button>
                <button class="tab-button" :class="{ active: activeTab === 'quartz' }" @click="activeTab = 'quartz'">
                    <font-awesome-icon :icon="['fas', 'clock']" />
                    Quartz
                    <span v-if="runningJobs.size > 0" class="tab-badge">{{ runningJobs.size }}</span>
                </button>
                <button class="tab-button" :class="{ active: activeTab === 'ai' }" @click="activeTab = 'ai'">
                    <font-awesome-icon :icon="['fas', 'robot']" />
                    AI
                </button>
                <button class="tab-button" :class="{ active: activeTab === 'ai-costs' }"
                    @click="activeTab = 'ai-costs'">
                    <font-awesome-icon :icon="['fas', 'chart-line']" />
                    {{ $t('maintenance.aiCosts.tabTitle') }}
                </button>
            </div>

            <!-- ==================== QUARTZ TAB ==================== -->
            <MaintenanceTabQuartzComponent v-show="activeTab === 'quartz'" :running-jobs="runningJobs"
                :job-progress="jobProgress" :job-system-status="jobSystemStatus" :job-status-loaded="jobStatusLoaded"
                :database-jobs="databaseJobs" :quartz-jobs="quartzJobs" :quartz-jobs-loaded="quartzJobsLoaded"
                @clear-job="clearJob" @clear-stuck-jobs="clearStuckJobs" @clear-job-by-id="clearJobById"
                @load-quartz-jobs="loadQuartzJobs" @interrupt-quartz-job="interruptQuartzJob" />

            <!-- ==================== AI TAB ==================== -->
            <MaintenanceTabAiComponent v-show="activeTab === 'ai'" :whitelisted-models="whitelistedModels"
                :provider-models="providerModels" :fetching-models="fetchingModels" :editing-cost-rate="editingCostRate"
                :editing-display-name="editingDisplayName" :editing-prices="editingPrices"
                :show-delete-confirm-modal="showDeleteConfirmModal" :model-to-delete="modelToDelete"
                @load-whitelisted-models="loadWhitelistedModels" @fetch-all-provider-models="fetchAllProviderModels"
                @start-edit-cost-rate="startEditCostRate" @save-cost-rate="saveCostRate"
                @cancel-edit-cost-rate="cancelEditCostRate" @start-edit-display-name="startEditDisplayName"
                @save-display-name="saveDisplayName" @cancel-edit-display-name="cancelEditDisplayName"
                @start-edit-prices="startEditPrices" @save-prices="savePrices" @cancel-edit-prices="cancelEditPrices"
                @confirm-delete-model="confirmDeleteModel" @execute-delete="executeDelete" @cancel-delete="cancelDelete"
                @toggle-whitelist="toggleWhitelist" @update:editing-cost-rate="editingCostRate = $event"
                @update:editing-display-name="editingDisplayName = $event"
                @update:editing-prices="editingPrices = $event" />

            <!-- ==================== AI COSTS TAB ==================== -->
            <MaintenanceTabAiCostsComponent v-show="activeTab === 'ai-costs'" :anti-forgery-token="antiForgeryToken" />

            <!-- ==================== GENERAL TAB ==================== -->
            <MaintenanceTabGeneralComponent v-show="activeTab === 'general'" :question-methods="questionMethods"
                :cache-methods="cacheMethods" :page-methods="pageMethods" :meili-search-methods="meiliSearchMethods"
                :user-methods="userMethods" :misc-methods="miscMethods" :tools-methods="toolsMethods"
                :logged-in-user-count="loggedInUserCount" :anonymous-user-count="anonymousUserCount"
                :user-id-to-delete="userIdToDelete" :token-user-id="tokenUserId" :token-amount="tokenAmount"
                :token-type="tokenType" :mmap-cache-status="mmapCacheStatus"
                :mmap-cache-status-loaded="mmapCacheStatusLoaded" :relation-errors="relationErrors"
                :relation-errors-loaded="relationErrorsLoaded" :is-analyzing="isAnalyzing"
                @execute-maintenance-operation="executeMaintenanceOperation"
                @load-mmap-cache-status="loadMmapCacheStatus" @load-relation-errors="loadRelationErrors"
                @clear-relation-errors-cache="clearRelationErrorsCache" @heal-relations="healRelations"
                @delete-user="deleteUser" @add-tokens-to-user="addTokensToUser" @remove-admin-rights="removeAdminRights"
                @update:user-id-to-delete="userIdToDelete = $event" @update:token-user-id="tokenUserId = $event"
                @update:token-amount="tokenAmount = $event" @update:token-type="tokenType = $event" />
        </div>
    </div>
</template>
<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.maintenance-tabs {
    display: flex;
    gap: 4px;
    margin-bottom: 24px;
    border-bottom: 2px solid @memo-grey-lighter;
    padding-bottom: 0;

    .tab-button {
        display: flex;
        align-items: center;
        gap: 8px;
        padding: 12px 20px;
        border: none;
        background: transparent;
        color: @memo-grey-dark;
        font-size: 14px;
        font-weight: 500;
        cursor: pointer;
        border-bottom: 2px solid transparent;
        margin-bottom: -2px;
        transition: all 0.2s ease;

        &:hover {
            color: @memo-blue;
            background-color: rgba(0, 0, 0, 0.03);
        }

        &.active {
            color: @memo-blue;
            border-bottom-color: @memo-blue;
        }

        .tab-badge {
            background-color: @memo-blue;
            color: white;
            font-size: 11px;
            padding: 2px 6px;
            border-radius: 10px;
            min-width: 18px;
            text-align: center;
        }
    }
}

.tab-content {
    animation: fadeIn 0.2s ease-in-out;
}

@keyframes fadeIn {
    from {
        opacity: 0;
        transform: translateY(4px);
    }

    to {
        opacity: 1;
        transform: translateY(0);
    }
}

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

.token-management-container {
    padding: 15px;

    .token-management-form {
        display: flex;
        flex-direction: column;
        gap: 12px;

        .form-group {
            display: flex;
            align-items: center;
            gap: 8px;

            label {
                min-width: 100px;
            }

            input,
            select {
                border: solid 1px @memo-grey-light;
                padding: 7px;
                min-width: 200px;
            }
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
        color: @memo-wish-knowledge-red;
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

.job-management-controls {
    display: flex;
    gap: 10px;
    padding: 15px;

    .memo-button {
        margin-right: 0;
    }
}

.job-summary-card {
    .job-stats {
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 15px;
        padding: 15px;

        .stat-item {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 10px;
            border-radius: 8px;
            background-color: @memo-grey-light;

            .stat-label {
                font-size: 12px;
                color: @memo-grey-darker;
                margin-bottom: 5px;
            }

            .stat-value {
                font-size: 18px;
                font-weight: bold;

                &.running {
                    color: #3498db;
                }

                &.completed {
                    color: #27ae60;
                }

                &.failed {
                    color: #e74c3c;
                }

                &.stuck {
                    color: #f39c12;
                }
            }
        }
    }
}

.database-job {
    padding: 12px;

    &.stuck-job {
        border-left: 4px solid #f39c12;
        background-color: #fef9e7;
    }

    .job-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 8px;

        h4 {
            margin: 0;
            font-size: 16px;
        }

        .job-meta {
            display: flex;
            gap: 10px;
            align-items: center;

            .duration {
                font-family: monospace;
                background: @memo-grey-light;
                padding: 2px 6px;
                border-radius: 4px;
                font-size: 12px;
            }

            .stuck-indicator {
                color: #f39c12;
                font-weight: bold;
                font-size: 12px;
            }
        }
    }

    .job-details {
        display: flex;
        flex-direction: column;
        gap: 4px;
        font-size: 12px;
        color: @memo-grey-darker;
    }
}

.job-json {
    border-radius: 4px;
    padding: 12px;
    font-family: 'Courier New', monospace;
    font-size: 12px;
    line-height: 1.4;
    max-height: 300px;
    overflow-y: auto;
    white-space: pre-wrap;
    word-wrap: break-word;
}

.database-jobs-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 16px;

    h4 {
        margin: 0;
    }

    .bulk-actions {
        display: flex;
        gap: 8px;
    }
}

.database-jobs-list {
    display: flex;
    flex-direction: column;
    gap: 12px;
    margin-bottom: 16px;
}

.database-job-item {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 12px;
    border: 1px solid #e9ecef;
    border-radius: 6px;
    background-color: #fff;

    &.stuck-job {
        border-left: 4px solid #f39c12;
        background-color: #fef9e7;
    }

    .job-info {
        flex: 1;

        .job-name {
            display: flex;
            align-items: center;
            gap: 8px;
            margin-bottom: 4px;

            .stuck-badge {
                font-size: 12px;
                color: #f39c12;
                font-weight: bold;
            }
        }

        .job-details {
            display: flex;
            flex-wrap: wrap;
            gap: 12px;
            font-size: 12px;
            color: @memo-grey-darker;

            span {
                padding: 2px 6px;
                border-radius: 3px;
            }
        }
    }
}

.raw-json-details {
    margin-top: 16px;

    summary {
        cursor: pointer;
        font-size: 14px;
        color: @memo-grey-darker;
        margin-bottom: 8px;

        &:hover {
            color: @memo-blue;
        }
    }
}

.quartz-jobs-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 16px;

    h4 {
        margin: 0;
    }
}

.quartz-jobs-list {
    display: flex;
    flex-direction: column;
    gap: 12px;
    margin-bottom: 16px;
}

.quartz-job-item {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 12px;
    border: 1px solid #e9ecef;
    border-radius: 6px;
    background-color: #fff;

    &.executing-job {
        border-left: 4px solid #17a2b8;
        background-color: #e6f3ff;
    }

    .job-info {
        flex: 1;

        .job-name {
            display: flex;
            align-items: center;
            gap: 8px;
            margin-bottom: 4px;

            .executing-badge {
                font-size: 12px;
                color: #17a2b8;
                font-weight: bold;
            }

            .job-type {
                font-size: 11px;
                color: @memo-grey;
                padding: 2px 6px;
                border-radius: 3px;
            }
        }

        .job-details {
            display: flex;
            flex-wrap: wrap;
            gap: 12px;
            font-size: 12px;
            color: @memo-grey-darker;

            span {
                padding: 2px 6px;
                border-radius: 3px;
            }
        }
    }

    .job-actions {
        display: flex;
        gap: 4px;
        flex-shrink: 0;
    }
}

.no-jobs-message {
    text-align: center;
    color: @memo-grey;
    font-style: italic;
    margin: 20px 0;
}

// AI Models Management Styles
.ai-models-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;

    h4 {
        margin: 0;
    }
}

.whitelist-table,
.provider-models-table {
    width: 100%;
    border-collapse: collapse;
    margin-bottom: 16px;

    th,
    td {
        padding: 10px 12px;
        text-align: left;
        border-bottom: 1px solid #e9ecef;
    }

    th {
        background-color: #f8f9fa;
        font-weight: 600;
        font-size: 13px;
        color: @memo-grey-darker;
    }

    .model-id-cell {
        font-family: monospace;
        font-size: 13px;
    }
}

.provider-badge {
    padding: 3px 10px;
    border-radius: 12px;
    font-size: 11px;
    font-weight: 500;
    color: white;
    display: inline-block;

    &.anthropic {
        background-color: #d97706;
    }

    &.openai {
        background-color: #10a37f;
    }
}

.cost-rate {
    cursor: pointer;
    padding: 4px 8px;
    border-radius: 4px;
    background-color: #e9ecef;
    font-weight: 500;

    &:hover {
        background-color: #dee2e6;
    }
}

.cost-rate-edit {
    display: flex;
    align-items: center;
    gap: 6px;

    .cost-rate-input {
        width: 70px;
        padding: 4px 8px;
        border: 1px solid #ced4da;
        border-radius: 4px;
        font-size: 13px;
    }
}

.display-name {
    cursor: pointer;
    padding: 4px 8px;
    border-radius: 4px;
    background-color: #e9ecef;

    &:hover {
        background-color: #dee2e6;
    }
}

.display-name-edit {
    display: flex;
    align-items: center;
    gap: 6px;

    .display-name-input {
        width: 180px;
        padding: 4px 8px;
        border: 1px solid #ced4da;
        border-radius: 4px;
        font-size: 13px;
    }
}

.btn-icon {
    background: none;
    border: none;
    cursor: pointer;
    padding: 6px 8px;
    border-radius: 4px;
    transition: background-color 0.2s;

    &.btn-delete {
        color: #dc3545;

        &:hover {
            background-color: #f8d7da;
        }
    }

    &.btn-save {
        color: #28a745;

        &:hover {
            background-color: #d4edda;
        }
    }

    &.btn-cancel {
        color: #6c757d;

        &:hover {
            background-color: #e9ecef;
        }
    }
}

.provider-section {
    margin-bottom: 24px;

    .provider-header {
        display: flex;
        align-items: center;
        gap: 10px;
        margin-bottom: 12px;
        padding-bottom: 8px;
        border-bottom: 2px solid #e9ecef;

        .model-count {
            font-size: 12px;
            color: @memo-grey;
            font-weight: normal;
        }
    }
}

.display-name-group {
    margin-bottom: 16px;
    margin-left: 16px;

    .display-name-header {
        font-size: 14px;
        font-weight: 600;
        color: @memo-grey-dark;
        margin-bottom: 8px;
        padding: 6px 12px;
        background-color: #f8f9fa;
        border-radius: 4px;
        border-left: 3px solid @memo-blue;
    }
}

.whitelisted-row {
    background-color: #d4edda;
}

.toggle-switch {
    position: relative;
    display: inline-block;
    width: 44px;
    height: 24px;

    input {
        opacity: 0;
        width: 0;
        height: 0;
    }

    .toggle-slider {
        position: absolute;
        cursor: pointer;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background-color: #ccc;
        transition: 0.3s;
        border-radius: 24px;

        &:before {
            position: absolute;
            content: "";
            height: 18px;
            width: 18px;
            left: 3px;
            bottom: 3px;
            background-color: white;
            transition: 0.3s;
            border-radius: 50%;
        }
    }

    input:checked+.toggle-slider {
        background-color: #28a745;
    }

    input:checked+.toggle-slider:before {
        transform: translateX(20px);
    }
}

.no-models-message {
    text-align: center;
    color: @memo-grey;
    font-style: italic;
    padding: 20px;
    background-color: #f8f9fa;
    border-radius: 6px;
}

.modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1000;
}

.confirm-modal {
    background-color: white;
    padding: 24px;
    border-radius: 8px;
    width: 400px;
    max-width: 90vw;

    h4 {
        margin-top: 0;
        margin-bottom: 16px;
    }

    p {
        margin-bottom: 20px;
    }

    .modal-actions {
        display: flex;
        gap: 10px;
        justify-content: flex-end;
    }
}
</style>
