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

        // Switch to fast polling since we have an active job
        updatePollingInterval()

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

            if (result.status === JobStatus.Completed) {
                resultMsg.value = result.message
                runningJobs.value.delete(jobId)
                jobProgress.value.delete(jobId)
                // Job completed - stop individual polling, let background polling handle cleanup
                return
            } else if (result.status === JobStatus.Failed) {
                resultMsg.value = `Job failed: ${result.message}`
                runningJobs.value.delete(jobId)
                jobProgress.value.delete(jobId)
                // Job failed - stop individual polling, let background polling handle cleanup
                return
            } else if (result.status === JobStatus.Running) {
                jobProgress.value.set(jobId, result)
                resultMsg.value = result.message
                pollCount++

                if (pollCount < maxPolls) {
                    setTimeout(poll, pollInterval)
                } else {
                    resultMsg.value = 'Job timeout - please check server logs'
                    runningJobs.value.delete(jobId)
                    jobProgress.value.delete(jobId)
                    // Job timed out - stop individual polling
                    return
                }
            } else if (result.status === JobStatus.NotFound) {
                resultMsg.value = 'Job not found - it may have completed or failed'
                runningJobs.value.delete(jobId)
                jobProgress.value.delete(jobId)
                // Job not found - stop individual polling
                return
            }
        } catch (error) {
            console.error('Error polling job status:', error)
            runningJobs.value.delete(jobId)
            jobProgress.value.delete(jobId)
            // Error occurred - stop individual polling
            return
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
            const previousJobCount = runningJobs.value.size

            // Clear current running jobs and repopulate from backend
            runningJobs.value.clear()
            jobProgress.value.clear()

            for (const job of result) {
                runningJobs.value.set(job.jobId, job.operationName)
                jobProgress.value.set(job.jobId, job)
            }

            // If this is the first call and we found jobs, switch to fast polling
            if (previousJobCount === 0 && result.length > 0) {
                userStartedJob.value = true
            }

            // Update polling interval based on job status
            updatePollingInterval()
        }
    } catch (error) {
        console.error('Error checking for running jobs:', error)
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

        console.log(`Polling interval changed to ${currentPollInterval}ms (${shouldUseFastPolling ? 'fast' : 'slow'} mode)`)
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

        const jobId = startResult.data
        resultMsg.value = 'Analysis in progress...'

        // Step 2: Poll for completion
        let isComplete = false
        let attempts = 0
        const maxAttempts = 180 // 3 minutes max polling (2s intervals)

        while (!isComplete && attempts < maxAttempts) {
            await new Promise(resolve => setTimeout(resolve, 2000)) // Wait 2 seconds
            attempts++

            try {
                const statusResult = await $api<JobStatusResponse>(`/apiVue/VueMaintenance/GetJobStatus?jobId=${jobId}`, {
                    method: 'GET',
                    mode: 'cors',
                    credentials: 'include'
                })

                if (statusResult.status === JobStatus.Completed) {
                    isComplete = true
                    resultMsg.value = 'Analysis completed. Loading results...'
                } else if (statusResult.status === JobStatus.Failed) {
                    resultMsg.value = `Analysis failed: ${statusResult.message}`
                    return
                } else if (statusResult.status === JobStatus.Running) {
                    resultMsg.value = `Analysis in progress... ${statusResult.message}`
                }
            } catch (pollError) {
                console.warn('Polling error:', pollError)
                // Continue polling even if one request fails
            }
        }

        if (!isComplete) {
            resultMsg.value = 'Analysis timed out. Please try again.'
            return
        }

        // Step 3: Fetch the cached results
        const cachedResult = await $api<RelationErrorsResponse>('/apiVue/VueMaintenance/ShowRelationErrors', {
            method: 'GET',
            mode: 'cors',
            credentials: 'include'
        })

        if (cachedResult.success) {
            relationErrors.value = cachedResult.data
            resultMsg.value = `Found ${cachedResult.data.length} pages with relation errors.`
            relationErrorsLoaded.value = true
        } else {
            resultMsg.value = 'Error loading cached results.'
        }

    } catch (error) {
        console.error('Error in relation analysis flow:', error)
        resultMsg.value = 'Error during analysis flow.'
    } finally {
        isAnalyzing.value = false
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
            <LayoutPanel v-if="runningJobs.size > 0" title="Active Jobs" class="active-jobs-panel">
                <LayoutCard v-for="[jobId, operationName] in runningJobs.entries()" :key="jobId" :size="LayoutCardSize.Small">
                    <div class="running-job">
                        <h4>{{ operationName }}</h4>
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
            <LayoutPanel :title="$t('maintenance.relations.title')">
                <LayoutCard :size="LayoutCardSize.Large" :background-color="'transparent'">
                    <button @click="loadRelationErrors" class="memo-button btn btn-primary" :disabled="isAnalyzing">
                        <i v-if="isAnalyzing" class="fas fa-spinner fa-spin"></i>
                        {{ isAnalyzing ? 'Analyzing...' : 'Analyze and Show' }}
                    </button>
                    <button @click="clearRelationErrorsCache" class="memo-button btn btn-secondary ms-2" :disabled="isAnalyzing">
                        Clear Cache
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

    h4 {
        margin: 0 0 12px 0;
        color: @memo-grey-darker;
    }
}

.job-status {
    font-size: 14px;
    color: @memo-grey-darker;
    font-style: italic;
}
</style>
