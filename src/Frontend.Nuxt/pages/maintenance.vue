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
    jobTrackingId: string
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

interface VueMaintenanceResult {
    success: boolean
    data: string
}

interface JobSystemStatusResponse {
    inMemoryJobs: InMemoryJobResponse[]
    databaseJobs: DatabaseJobResponse[]
    summary: JobSummaryResponse
}

interface InMemoryJobResponse {
    jobTrackingId: string
    status: string
    message: string
    operationName: string
}

interface DatabaseJobResponse {
    id: number
    name: string
    startedAt: string
    duration: string
    isStuck: boolean
    durationHours: number
}

interface JobSummaryResponse {
    totalInMemory: number
    totalInDatabase: number
    runningInMemory: number
    completedInMemory: number
    failedInMemory: number
    stuckInDatabase: number
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
const resultMsg = ref('')
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
            runningJobs.value.set(job.jobTrackingId, job.operationName)
            jobProgress.value.set(job.jobTrackingId, job)

            // Update result message with the latest job status
            if (job.status === JobStatus.Running) {
                resultMsg.value = job.message
            }
        }

        // Check for jobs that were in previous list but not in current (they completed/failed)
        for (const [jobTrackingId, operationName] of previousJobs) {
            if (!runningJobs.value.has(jobTrackingId)) {
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

// Token Management
const tokenUserId = ref(0)
const tokenAmount = ref(0)
const tokenType = ref<'subscription' | 'paid'>('subscription')

async function addTokensToUser() {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    if (tokenUserId.value <= 0) {
        resultMsg.value = 'Please enter a valid user ID'
        return
    }

    if (tokenAmount.value <= 0) {
        resultMsg.value = 'Please enter a valid amount greater than 0'
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
        resultMsg.value = result.data
        tokenAmount.value = 0
    } else {
        resultMsg.value = `Error: ${result.data}`
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

        relationAnalysisjobTrackingId.value = startResult.data
        resultMsg.value = 'Analysis in progress...'

        // Step 2: Set up watcher for job completion
        if (stopRelationJobWatcher) {
            stopRelationJobWatcher() // Stop any existing watcher
        }

        stopRelationJobWatcher = watch(jobProgress, (jobs) => {
            if (relationAnalysisjobTrackingId.value && jobs.has(relationAnalysisjobTrackingId.value)) {
                const job = jobs.get(relationAnalysisjobTrackingId.value)
                if (job) {
                    if (job.status === JobStatus.Completed) {
                        resultMsg.value = 'Analysis completed. Fetching results...'
                        fetchCachedRelationErrors()
                    } else if (job.status === JobStatus.Failed) {
                        resultMsg.value = `Analysis failed: ${job.message}`
                        relationAnalysisjobTrackingId.value = null
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
        resultMsg.value = `Found ${cachedResult.data.length} pages with relation errors.`
        relationErrorsLoaded.value = true
    } else {
        resultMsg.value = 'Error loading cached results.'
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
        resultMsg.value = result.data
        // Remove the job from local state immediately for responsive UI
        runningJobs.value.delete(jobTrackingId)
        jobProgress.value.delete(jobTrackingId)
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

const clearStuckJobs = async () => {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    const stuckJobs = databaseJobs.value.filter(job => job.isStuck)
    resultMsg.value = `Clearing ${stuckJobs.length} stuck jobs...`

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
            resultMsg.value = `✅ Cleared stuck jobs successfully (Quartz + Database): ${result.data}`
            // Refresh all job information
            await loadQuartzJobs()
        } else {
            resultMsg.value = `⚠️ Failed to clear stuck jobs from database: ${result?.data || 'Unknown error'}`
        }
    } catch (error) {
        console.error('Error clearing stuck jobs:', error)
        resultMsg.value = `❌ Error clearing stuck jobs: ${error}`
    }
}

const clearJobById = async (jobId: number) => {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    // Find the job name from the database jobs
    const job = databaseJobs.value.find(j => j.id === jobId)
    if (!job) {
        resultMsg.value = `Job with ID ${jobId} not found.`
        return
    }

    resultMsg.value = `Clearing job "${job.name}" (ID: ${jobId})...`

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
            resultMsg.value = `✅ Job "${job.name}" cleared successfully (Quartz + Database)`
            // Refresh all job information
            await loadQuartzJobs()
        } else {
            resultMsg.value = `⚠️ Database clear failed for job "${job.name}": ${result?.data || 'Unknown error'}`
        }
    } catch (error) {
        console.error('Error clearing job:', error)
        resultMsg.value = `❌ Error clearing job "${job.name}": ${error}`
    }
}

const clearJobsByIds = async (jobIds: number[]) => {
    if (!isAdmin.value || !userStore.isAdmin || antiForgeryToken.value == undefined || antiForgeryToken.value.length < 0)
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })

    const jobsToClear = databaseJobs.value.filter(job => jobIds.includes(job.id))
    resultMsg.value = `Clearing ${jobsToClear.length} selected jobs...`

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
            resultMsg.value = `✅ Cleared selected jobs successfully (Quartz + Database): ${result.data}`
            // Refresh all job information
            await loadQuartzJobs()
        } else {
            resultMsg.value = `⚠️ Failed to clear selected jobs from database: ${result?.data || 'Unknown error'}`
        }
    } catch (error) {
        console.error('Error clearing selected jobs:', error)
        resultMsg.value = `❌ Error clearing selected jobs: ${error}`
    }
}

const quartzJobs = ref<any[]>([])
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
            resultMsg.value = `Loaded ${quartzJobs.value.length} Quartz jobs, ${jobSystemResult.inMemoryJobs.length} in-memory jobs, and ${jobSystemResult.databaseJobs.length} database jobs (${totalJobs} total active jobs).`
        } else {
            resultMsg.value = `Loaded ${quartzJobs.value.length} Quartz jobs.`
        }
    } else {
        resultMsg.value = 'Failed to load Quartz jobs.'
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
        resultMsg.value = result.data
        // Refresh jobs list
        await loadQuartzJobs()
    } else {
        resultMsg.value = 'Failed to interrupt Quartz job.'
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

const formatDuration = (duration: string): string => {
    // Simple duration formatter - you can enhance this
    return duration || 'N/A'
}

// AI Models Management
interface WhitelistedModel {
    id: number
    provider: string
    modelId: string
    displayName: string
    tokenCostMultiplier: number
}

interface AvailableModel {
    modelId: string
    displayName: string
    isWhitelisted: boolean
}

interface ProviderModels {
    providerName: string
    models: AvailableModel[]
}

interface GetWhitelistedModelsResponse {
    success: boolean
    models: WhitelistedModel[]
}

interface GetAllProviderModelsResponse {
    success: boolean
    providers: ProviderModels[]
    error: string
}

const whitelistedModels = ref<WhitelistedModel[]>([])
const providerModels = ref<ProviderModels[]>([])
const fetchingModels = ref(false)
const showDeleteConfirmModal = ref(false)
const modelToDelete = ref<WhitelistedModel | null>(null)
const editingCostRate = ref<{ id: number, value: number } | null>(null)
const editingDisplayName = ref<{ id: number, value: string } | null>(null)

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
        resultMsg.value = `Fetched ${totalModels} models from ${result.providers.length} providers`
    } else {
        resultMsg.value = `Error fetching models: ${result?.error || 'Unknown error'}`
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

    const result = await $api<VueMaintenanceResult>('/apiVue/VueMaintenance/RemoveFromWhitelist', {
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
        resultMsg.value = 'Model removed from whitelist'
    } else {
        resultMsg.value = `Error: ${result?.data || 'Unknown error'}`
    }

    showDeleteConfirmModal.value = false
    modelToDelete.value = null
}

const startEditCostRate = (model: WhitelistedModel) => {
    editingCostRate.value = { id: model.id, value: model.tokenCostMultiplier }
}

const saveCostRate = async () => {
    if (!antiForgeryToken.value || !editingCostRate.value) return

    const data = new FormData()
    data.append('__RequestVerificationToken', antiForgeryToken.value)
    data.append('id', editingCostRate.value.id.toString())
    data.append('tokenCostMultiplier', editingCostRate.value.value.toString())

    const result = await $api<VueMaintenanceResult>('/apiVue/VueMaintenance/UpdateWhitelistCostRate', {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include'
    })

    if (result?.success) {
        // Find the model in the flat list
        const model = whitelistedModels.value.find(model => model.id === editingCostRate.value!.id)
        if (model) {
            model.tokenCostMultiplier = editingCostRate.value.value
        }
        resultMsg.value = 'Cost rate updated'
    } else {
        resultMsg.value = `Error: ${result?.data || 'Unknown error'}`
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
        resultMsg.value = 'Display name updated'
    } else {
        resultMsg.value = `Error: ${result?.data || 'Unknown error'}`
    }

    editingDisplayName.value = null
}

const cancelEditDisplayName = () => {
    editingDisplayName.value = null
}

const toggleWhitelist = async (providerName: string, model: AvailableModel) => {
    if (!antiForgeryToken.value) return

    if (model.isWhitelisted) {
        // Remove from whitelist
        const data = new FormData()
        data.append('__RequestVerificationToken', antiForgeryToken.value)
        data.append('modelId', model.modelId)
        data.append('modelProvider', providerName)

        const result = await $api<VueMaintenanceResult>('/apiVue/VueMaintenance/RemoveFromWhitelist', {
            body: data,
            method: 'POST',
            mode: 'cors',
            credentials: 'include'
        })

        if (result?.success) {
            model.isWhitelisted = false
            // Remove from whitelisted models list
            whitelistedModels.value = whitelistedModels.value.filter(whitelistedModel => whitelistedModel.modelId !== model.modelId)
            resultMsg.value = `${model.displayName} removed from whitelist`
        } else {
            resultMsg.value = `Error: ${result?.data || 'Unknown error'}`
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
            resultMsg.value = `${model.displayName} added to whitelist`
        } else {
            resultMsg.value = `Error: ${result?.data || 'Unknown error'}`
        }
    }
}

// Load whitelisted models on mount
onMounted(() => {
    loadWhitelistedModels()
})
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
                <LayoutCard v-for="[jobTrackingId, operationName] in runningJobs.entries()" :key="jobTrackingId" :size="LayoutCardSize.Small">
                    <div class="running-job">
                        <div class="job-header">
                            <h4>{{ operationName }}</h4>
                            <button @click="clearJob(jobTrackingId)" class="clear-job-btn" title="Clear Job">
                                <font-awesome-icon icon="fa-solid fa-xmark" />
                            </button>
                        </div>
                        <div v-if="jobProgress.has(jobTrackingId)" class="job-status">
                            <span>{{ jobProgress.get(jobTrackingId)?.message }}</span>
                        </div>
                        <div v-else class="job-status">
                            <span>Starting...</span>
                        </div>
                    </div>
                </LayoutCard>
            </LayoutPanel>

            <!-- Job System Status Panel -->
            <LayoutPanel title="Job System Status">

                <template v-if="jobStatusLoaded && jobSystemStatus && jobSystemStatus.summary">
                    <LayoutCard :size="LayoutCardSize.Medium" class="job-summary-card">
                        <h4>Summary</h4>
                        <div class="job-stats">
                            <div class="stat-item">
                                <span class="stat-label">In-Memory Jobs:</span>
                                <span class="stat-value">{{ jobSystemStatus.summary.totalInMemory }}</span>
                            </div>
                            <div class="stat-item">
                                <span class="stat-label">Running:</span>
                                <span class="stat-value running">{{ jobSystemStatus.summary.runningInMemory }}</span>
                            </div>
                            <div class="stat-item">
                                <span class="stat-label">Completed:</span>
                                <span class="stat-value completed">{{ jobSystemStatus.summary.completedInMemory }}</span>
                            </div>
                            <div class="stat-item">
                                <span class="stat-label">Failed:</span>
                                <span class="stat-value failed">{{ jobSystemStatus.summary.failedInMemory }}</span>
                            </div>
                            <div class="stat-item">
                                <span class="stat-label">Database Jobs:</span>
                                <span class="stat-value">{{ jobSystemStatus.summary.totalInDatabase }}</span>
                            </div>
                            <div class="stat-item">
                                <span class="stat-label">Stuck:</span>
                                <span class="stat-value stuck">{{ jobSystemStatus.summary.stuckInDatabase }}</span>
                            </div>
                        </div>
                    </LayoutCard>
                </template>

                <template v-if="databaseJobs.length > 0">
                    <LayoutCard :size="LayoutCardSize.Large">
                        <div class="database-jobs-header">
                            <h4>Database Running Jobs ({{ databaseJobs.length }})</h4>
                            <div class="bulk-actions">
                                <button @click="clearStuckJobs" class="memo-button btn btn-warning btn-sm">
                                    Clear Stuck Only (>2h)
                                </button>
                                <button @click="clearJobsByIds(databaseJobs.filter(job => job.isStuck).map(job => job.id))"
                                    class="memo-button btn btn-danger btn-sm ms-2"
                                    :disabled="!databaseJobs.some(job => job.isStuck)">
                                    Clear All Stuck (Quartz + DB)
                                </button>
                            </div>
                        </div>

                        <div class="database-jobs-list">
                            <div v-for="job in databaseJobs" :key="job.id" class="database-job-item" :class="{ 'stuck-job': job.isStuck }">
                                <div class="job-info">
                                    <div class="job-name">
                                        <strong>{{ job.name }}</strong>
                                        <span v-if="job.isStuck" class="stuck-badge">⚠️ STUCK</span>
                                    </div>
                                    <div class="job-details">
                                        <span>ID: {{ job.id }}</span>
                                        <span>Started: {{ job.startedAt }}</span>
                                        <span>Duration: {{ job.duration }} ({{ job.durationHours }}h)</span>
                                    </div>
                                </div>
                                <button @click="clearJobById(job.id)"
                                    class="memo-button btn btn-warning btn-sm">
                                    Clear Job (Quartz + DB)
                                </button>
                            </div>
                        </div>

                        <details class="raw-json-details">
                            <summary>Show Raw JSON</summary>
                            <pre class="job-json">{{ JSON.stringify(databaseJobs, null, 2) }}</pre>
                        </details>
                    </LayoutCard>
                </template>
            </LayoutPanel>

            <!-- Quartz Jobs Panel -->
            <LayoutPanel title="Quartz Scheduler Jobs">
                <LayoutCard :size="LayoutCardSize.Large">
                    <div class="quartz-jobs-header">
                        <h4>Quartz Job Management</h4>
                        <button @click="loadQuartzJobs" class="memo-button btn btn-primary">
                            Load Quartz Jobs
                        </button>
                    </div>

                    <template v-if="quartzJobsLoaded && quartzJobs.length > 0">
                        <div class="quartz-jobs-list">
                            <div v-for="job in quartzJobs" :key="job.JobKey" class="quartz-job-item" :class="{ 'executing-job': job.IsExecuting }">
                                <div class="job-info">
                                    <div class="job-name">
                                        <strong>{{ job.JobName }}</strong>
                                        <span v-if="job.IsExecuting" class="executing-badge">⚡ RUNNING</span>
                                        <span class="job-type">{{ job.JobType }}</span>
                                    </div>
                                    <div class="job-details">
                                        <span>Key: {{ job.JobKey }}</span>
                                        <span>Group: {{ job.JobGroup }}</span>
                                        <span v-if="job.IsExecuting && job.RunTime">Runtime: {{ formatDuration(job.RunTime) }}</span>
                                        <span v-if="job.FireTime">Fire Time: {{ new Date(job.FireTime).toLocaleString() }}</span>
                                    </div>
                                </div>
                                <div class="job-actions">
                                    <button v-if="job.IsExecuting"
                                        @click="interruptQuartzJob(job.JobName, job.JobGroup)"
                                        class="memo-button btn btn-warning btn-sm">
                                        Interrupt
                                    </button>
                                </div>
                            </div>
                        </div>
                    </template>

                    <template v-else-if="quartzJobsLoaded && quartzJobs.length === 0">
                        <p class="no-jobs-message">No Quartz jobs found.</p>
                    </template>

                    <template v-if="quartzJobsLoaded">
                        <details class="raw-json-details">
                            <summary>Show Raw Quartz Jobs JSON</summary>
                            <pre class="job-json">{{ JSON.stringify(quartzJobs, null, 2) }}</pre>
                        </details>
                    </template>
                </LayoutCard>
            </LayoutPanel>

            <LayoutPanel :title="$t('maintenance.metrics.title')">
                <NuxtLink to="/Metriken" class="memo-button btn btn-primary">
                    {{ $t('maintenance.metrics.viewOverview') }}
                </NuxtLink>
            </LayoutPanel>

            <!-- AI Models Management Panel -->
            <LayoutPanel title="AI Models Management">
                <!-- Section 1: Whitelisted Models -->
                <LayoutCard :size="LayoutCardSize.Large">
                    <div class="ai-models-header">
                        <h4>Whitelisted Models</h4>
                        <button @click="loadWhitelistedModels" class="memo-button btn btn-secondary">
                            <font-awesome-icon icon="fa-solid fa-sync" /> Refresh
                        </button>
                    </div>

                    <template v-if="whitelistedModels.length > 0">
                        <table class="whitelist-table">
                            <thead>
                                <tr>
                                    <th>Provider</th>
                                    <th>Display Name</th>
                                    <th>Model ID</th>
                                    <th>Cost Rate</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="model in whitelistedModels" :key="model.id">
                                    <td>
                                        <span class="provider-badge" :class="model.provider.toLowerCase()">{{ model.provider }}</span>
                                    </td>
                                    <td>
                                        <template v-if="editingDisplayName?.id === model.id">
                                            <div class="display-name-edit">
                                                <input
                                                    v-model="editingDisplayName.value"
                                                    type="text"
                                                    class="display-name-input" />
                                                <button @click="saveDisplayName" class="btn-icon btn-save" title="Save">
                                                    <font-awesome-icon icon="fa-solid fa-check" />
                                                </button>
                                                <button @click="cancelEditDisplayName" class="btn-icon btn-cancel" title="Cancel">
                                                    <font-awesome-icon icon="fa-solid fa-times" />
                                                </button>
                                            </div>
                                        </template>
                                        <template v-else>
                                            <span class="display-name" @click="startEditDisplayName(model)" title="Click to edit">
                                                {{ model.displayName || '(no name)' }}
                                            </span>
                                        </template>
                                    </td>
                                    <td class="model-id-cell">{{ model.modelId }}</td>
                                    <td>
                                        <template v-if="editingCostRate?.id === model.id">
                                            <div class="cost-rate-edit">
                                                <input
                                                    v-model.number="editingCostRate.value"
                                                    type="number"
                                                    step="0.1"
                                                    min="0"
                                                    class="cost-rate-input" />
                                                <button @click="saveCostRate" class="btn-icon btn-save" title="Save">
                                                    <font-awesome-icon icon="fa-solid fa-check" />
                                                </button>
                                                <button @click="cancelEditCostRate" class="btn-icon btn-cancel" title="Cancel">
                                                    <font-awesome-icon icon="fa-solid fa-times" />
                                                </button>
                                            </div>
                                        </template>
                                        <template v-else>
                                            <span class="cost-rate" @click="startEditCostRate(model)" title="Click to edit">
                                                {{ model.tokenCostMultiplier }}x
                                            </span>
                                        </template>
                                    </td>
                                    <td>
                                        <button @click="confirmDeleteModel(model)" class="btn-icon btn-delete" title="Remove from whitelist">
                                            <font-awesome-icon icon="fa-solid fa-trash" />
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </template>
                    <template v-else>
                        <p class="no-models-message">No models whitelisted yet. Use the section below to fetch and add models.</p>
                    </template>
                </LayoutCard>

                <!-- Section 2: Available Models from Providers -->
                <LayoutCard :size="LayoutCardSize.Large">
                    <div class="ai-models-header">
                        <h4>Available Models</h4>
                        <button @click="fetchAllProviderModels" class="memo-button btn btn-primary" :disabled="fetchingModels">
                            <font-awesome-icon v-if="fetchingModels" icon="fa-solid fa-spinner" spin />
                            <font-awesome-icon v-else icon="fa-solid fa-download" />
                            Load All Models
                        </button>
                    </div>

                    <template v-if="providerModels.length > 0">
                        <div v-for="provider in providerModels" :key="provider.providerName" class="provider-section">
                            <h5 class="provider-header">
                                <span class="provider-badge" :class="provider.providerName.toLowerCase()">{{ provider.providerName }}</span>
                                <span class="model-count">({{ provider.models.length }} models)</span>
                            </h5>
                            <table class="provider-models-table">
                                <thead>
                                    <tr>
                                        <th>Display Name</th>
                                        <th>Model ID</th>
                                        <th>Whitelisted</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="model in provider.models" :key="model.modelId" :class="{ 'whitelisted-row': model.isWhitelisted }">
                                        <td>{{ model.displayName }}</td>
                                        <td class="model-id-cell">{{ model.modelId }}</td>
                                        <td>
                                            <label class="toggle-switch">
                                                <input
                                                    type="checkbox"
                                                    :checked="model.isWhitelisted"
                                                    @change="toggleWhitelist(provider.providerName, model)" />
                                                <span class="toggle-slider"></span>
                                            </label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </template>
                    <template v-else>
                        <p class="no-models-message">Click "Load All Models" to fetch available models from providers.</p>
                    </template>
                </LayoutCard>

                <!-- Delete Confirmation Modal -->
                <div v-if="showDeleteConfirmModal" class="modal-overlay" @click.self="cancelDelete">
                    <div class="confirm-modal">
                        <h4>Confirm Delete</h4>
                        <p>Are you sure you want to remove <strong>{{ modelToDelete?.displayName }}</strong> ({{ modelToDelete?.modelId }}) from the whitelist?</p>
                        <div class="modal-actions">
                            <button @click="executeDelete" class="memo-button btn btn-danger">Delete</button>
                            <button @click="cancelDelete" class="memo-button btn btn-secondary">Cancel</button>
                        </div>
                    </div>
                </div>
            </LayoutPanel>
            <MaintenanceSection :title="$t('maintenance.questions.title')" :methods="questionMethods" @method-clicked="executeMaintenanceOperation" :icon="['fas', 'retweet']" />
            <MaintenanceSection :title="$t('maintenance.cache.title')" :methods="cacheMethods" @method-clicked="executeMaintenanceOperation" :icon="['fas', 'retweet']" />
            <MaintenanceSection :title="$t('maintenance.pages.title')" v-if="pageMethods.length > 0" :methods="pageMethods" @method-clicked="executeMaintenanceOperation" :icon="['fas', 'retweet']" />

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
                <LayoutCard :size="LayoutCardSize.Small">
                    <div class="token-management-container">
                        <h4>Add Tokens to User</h4>
                        <div class="token-management-form">
                            <div class="form-group">
                                <label>User ID:</label>
                                <input v-model.number="tokenUserId" type="number" placeholder="User ID" />
                            </div>
                            <div class="form-group">
                                <label>Amount:</label>
                                <input v-model.number="tokenAmount" type="number" placeholder="Token amount" />
                            </div>
                            <div class="form-group">
                                <label>Token Type:</label>
                                <select v-model="tokenType">
                                    <option value="subscription">Subscription Tokens</option>
                                    <option value="paid">Paid Tokens</option>
                                </select>
                            </div>
                            <button @click="addTokensToUser" class="memo-button btn btn-primary">
                                Add Tokens
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
