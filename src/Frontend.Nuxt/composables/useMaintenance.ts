import type {
    JobStatusResponse,
    JobSystemStatusResponse,
    DatabaseJobResponse,
    QuartzJob,
    VueMaintenanceResult,
    JobStatus,
} from '~~/components/maintenance/maintenance.types'
import { JobStatus as JobStatusEnum } from '~~/components/maintenance/maintenance.types'
import { $api } from '~~/composables/fetchWithError'

interface UseMaintenanceJobsOptions {
    isAdmin: Ref<boolean>
    antiForgeryToken: Ref<string | undefined>
    userStore: { isAdmin: boolean }
}

/**
 * Composable for managing maintenance job polling and status
 */
export function useMaintenanceJobs(options: UseMaintenanceJobsOptions) {
    const { isAdmin, antiForgeryToken, userStore } = options

    // Reactive state
    const resultMsg = ref('')
    const runningJobs = ref<Map<string, string>>(new Map())
    const jobProgress = ref<Map<string, JobStatusResponse>>(new Map())
    const jobSystemStatus = ref<JobSystemStatusResponse | null>(null)
    const databaseJobs = ref<DatabaseJobResponse[]>([])
    const jobStatusLoaded = ref(false)
    const quartzJobs = ref<QuartzJob[]>([])
    const quartzJobsLoaded = ref(false)

    // Adaptive polling configuration
    const FAST_POLL_INTERVAL = 2000 // 2 seconds when jobs are active
    const SLOW_POLL_INTERVAL = 15000 // 15 seconds when no jobs are running
    const userStartedJob = ref(false)
    let currentPollInterval = SLOW_POLL_INTERVAL
    let jobPollingInterval: NodeJS.Timeout | null = null

    /**
     * Check if the current user has valid admin credentials
     */
    const hasValidCredentials = (): boolean => {
        return (
            isAdmin.value &&
            userStore.isAdmin &&
            antiForgeryToken.value !== undefined &&
            antiForgeryToken.value.length > 0
        )
    }

    /**
     * Create a FormData object with the anti-forgery token
     */
    const createFormData = (): FormData => {
        const data = new FormData()
        if (antiForgeryToken.value) {
            data.append('__RequestVerificationToken', antiForgeryToken.value)
        }
        return data
    }

    /**
     * Check for all running jobs
     */
    const checkForRunningJobs = async () => {
        if (!hasValidCredentials()) {
            return
        }

        const result = await $api<JobStatusResponse[]>(
            '/apiVue/VueMaintenance/GetAllRunningJobs',
            {
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
                body: createFormData(),
            },
        )

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
                if (job.status === JobStatusEnum.Running) {
                    resultMsg.value = job.message
                }
            }

            // Check for jobs that were in previous list but not in current (they completed/failed)
            for (const [_jobTrackingId, operationName] of previousJobs) {
                if (!runningJobs.value.has(_jobTrackingId)) {
                    resultMsg.value = `Job ${operationName} completed`
                }
            }

            // If this is the first call and we found jobs, switch to fast polling
            if (previousJobCount === 0 && result.length > 0) {
                userStartedJob.value = true
            }

            updatePollingInterval()
        }
    }

    /**
     * Update the polling interval based on job activity
     */
    const updatePollingInterval = () => {
        const hasRunningJobs = runningJobs.value.size > 0
        const shouldUseFastPolling = userStartedJob.value || hasRunningJobs
        const newInterval = shouldUseFastPolling
            ? FAST_POLL_INTERVAL
            : SLOW_POLL_INTERVAL

        if (newInterval !== currentPollInterval) {
            currentPollInterval = newInterval

            if (jobPollingInterval) {
                clearInterval(jobPollingInterval)
                jobPollingInterval = setInterval(() => {
                    checkForRunningJobs()
                }, currentPollInterval)
            }
        }

        if (!hasRunningJobs) {
            userStartedJob.value = false
        }
    }

    /**
     * Execute a maintenance operation
     */
    const executeMaintenanceOperation = async (operationUrl: string) => {
        if (!hasValidCredentials()) {
            throw createError({ statusCode: 404, statusMessage: 'Not Found' })
        }

        const data = createFormData()

        if (operationUrl === 'ClearStuckJobs') {
            data.append('maxHours', '2')
        }

        const result = await $api<FetchResult<string>>(
            `/apiVue/VueMaintenance/${operationUrl}`,
            {
                body: data,
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
            },
        )

        if (result?.success) {
            const jobTrackingId = result.data
            runningJobs.value.set(jobTrackingId, operationUrl)
            resultMsg.value = `Job ${operationUrl} started. Checking status...`
            userStartedJob.value = true
            updatePollingInterval()
            await checkForRunningJobs()
        }
    }

    /**
     * Clear a specific job by tracking ID
     */
    const clearJob = async (jobTrackingId: string) => {
        if (!hasValidCredentials()) {
            throw createError({ statusCode: 404, statusMessage: 'Not Found' })
        }

        const data = createFormData()
        data.append('jobTrackingId', jobTrackingId)

        const result = await $api<FetchResult<string>>(
            `/apiVue/VueMaintenance/ClearJob`,
            {
                body: data,
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
            },
        )

        if (result.success) {
            resultMsg.value = result.data
            runningJobs.value.delete(jobTrackingId)
            jobProgress.value.delete(jobTrackingId)
            await checkForRunningJobs()
        }
    }

    /**
     * Load Quartz scheduler jobs
     */
    const loadQuartzJobs = async () => {
        if (!hasValidCredentials()) {
            throw createError({ statusCode: 404, statusMessage: 'Not Found' })
        }

        const data = createFormData()

        const quartzResult = await $api<VueMaintenanceResult>(
            `/apiVue/VueMaintenance/GetQuartzJobs`,
            {
                body: data,
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
            },
        )

        const jobSystemResult = await $api<JobSystemStatusResponse>(
            '/apiVue/VueMaintenance/GetJobSystemStatus',
            {
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
                body: data,
            },
        )

        if (quartzResult?.success) {
            quartzJobs.value = JSON.parse(quartzResult.data)
            quartzJobsLoaded.value = true

            if (jobSystemResult) {
                jobSystemStatus.value = jobSystemResult
                jobStatusLoaded.value = true

                runningJobs.value.clear()
                jobProgress.value.clear()

                for (const job of jobSystemResult.inMemoryJobs) {
                    runningJobs.value.set(job.jobTrackingId, job.operationName)
                    jobProgress.value.set(job.jobTrackingId, {
                        jobTrackingId: job.jobTrackingId,
                        status: parseInt(job.status) as JobStatus,
                        message: job.message,
                        operationName: job.operationName,
                    })
                }

                databaseJobs.value = jobSystemResult.databaseJobs

                const totalJobs =
                    jobSystemResult.inMemoryJobs.length +
                    jobSystemResult.databaseJobs.length
                resultMsg.value = `Loaded ${quartzJobs.value.length} Quartz jobs, ${jobSystemResult.inMemoryJobs.length} in-memory jobs, and ${jobSystemResult.databaseJobs.length} database jobs (${totalJobs} total active jobs).`
            } else {
                resultMsg.value = `Loaded ${quartzJobs.value.length} Quartz jobs.`
            }
        } else {
            resultMsg.value = 'Failed to load Quartz jobs.'
        }
    }

    /**
     * Interrupt a Quartz job
     */
    const interruptQuartzJob = async (jobName: string, jobGroup?: string) => {
        if (!hasValidCredentials()) {
            throw createError({ statusCode: 404, statusMessage: 'Not Found' })
        }

        const data = createFormData()
        data.append('jobName', jobName)
        if (jobGroup) {
            data.append('jobGroup', jobGroup)
        }

        const result = await $api<VueMaintenanceResult>(
            `/apiVue/VueMaintenance/InterruptQuartzJob`,
            {
                body: data,
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
            },
        )

        if (result?.success) {
            resultMsg.value = result.data
            await loadQuartzJobs()
        } else {
            resultMsg.value = 'Failed to interrupt Quartz job.'
        }
    }

    /**
     * Start the job polling
     */
    const startPolling = () => {
        checkForRunningJobs()
        jobPollingInterval = setInterval(() => {
            checkForRunningJobs()
        }, currentPollInterval)
    }

    /**
     * Stop the job polling
     */
    const stopPolling = () => {
        if (jobPollingInterval) {
            clearInterval(jobPollingInterval)
            jobPollingInterval = null
        }
    }

    return {
        // State
        resultMsg,
        runningJobs,
        jobProgress,
        jobSystemStatus,
        databaseJobs,
        jobStatusLoaded,
        quartzJobs,
        quartzJobsLoaded,
        userStartedJob,

        // Methods
        checkForRunningJobs,
        executeMaintenanceOperation,
        clearJob,
        loadQuartzJobs,
        interruptQuartzJob,
        startPolling,
        stopPolling,
        hasValidCredentials,
        createFormData,
    }
}
