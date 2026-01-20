<script lang="ts" setup>
import type {
    JobStatusResponse,
    DatabaseJobResponse,
    JobSystemStatusResponse,
    QuartzJob
} from './maintenance.types'

// Props
const props = defineProps<{
    runningJobs: Map<string, string>
    jobProgress: Map<string, JobStatusResponse>
    jobSystemStatus: JobSystemStatusResponse | null
    jobStatusLoaded: boolean
    databaseJobs: DatabaseJobResponse[]
    quartzJobs: QuartzJob[]
    quartzJobsLoaded: boolean
}>()

// Emits
const emit = defineEmits<{
    (event: 'clearStuckJobs' | 'loadQuartzJobs'): void
    (event: 'clearJob', jobTrackingId: string): void
    (event: 'clearJobById', jobId: number): void
    (event: 'interruptQuartzJob', jobName: string, jobGroup: string): void
}>()

const formatDuration = (duration: string): string => {
    return duration || 'N/A'
}
</script>

<template>
    <div class="tab-content">
        <!-- Active Jobs Status Panel -->
        <LayoutPanel v-if="props.runningJobs.size > 0" title="Active Jobs" class="active-jobs-panel">
            <LayoutCard v-for="[jobTrackingId, operationName] in props.runningJobs.entries()" :key="jobTrackingId"
                :size="LayoutCardSize.Small">
                <div class="running-job">
                    <div class="job-header">
                        <h4>{{ operationName }}</h4>
                        <button class="clear-job-btn" title="Clear Job" @click="emit('clearJob', jobTrackingId)">
                            <font-awesome-icon icon="fa-solid fa-xmark" />
                        </button>
                    </div>
                    <div v-if="props.jobProgress.has(jobTrackingId)" class="job-status">
                        <span>{{ props.jobProgress.get(jobTrackingId)?.message }}</span>
                    </div>
                    <div v-else class="job-status">
                        <span>Starting...</span>
                    </div>
                </div>
            </LayoutCard>
        </LayoutPanel>

        <!-- Job System Status Panel -->
        <LayoutPanel title="Job System Status">
            <template v-if="props.jobStatusLoaded && props.jobSystemStatus && props.jobSystemStatus.summary">
                <LayoutCard :size="LayoutCardSize.Medium" class="job-summary-card">
                    <h4>Summary</h4>
                    <div class="job-stats">
                        <div class="stat-item">
                            <span class="stat-label">In-Memory Jobs:</span>
                            <span class="stat-value">{{ props.jobSystemStatus.summary.totalInMemory }}</span>
                        </div>
                        <div class="stat-item">
                            <span class="stat-label">Running:</span>
                            <span class="stat-value running">{{ props.jobSystemStatus.summary.runningInMemory }}</span>
                        </div>
                        <div class="stat-item">
                            <span class="stat-label">Completed:</span>
                            <span class="stat-value completed">{{ props.jobSystemStatus.summary.completedInMemory
                                }}</span>
                        </div>
                        <div class="stat-item">
                            <span class="stat-label">Failed:</span>
                            <span class="stat-value failed">{{ props.jobSystemStatus.summary.failedInMemory }}</span>
                        </div>
                        <div class="stat-item">
                            <span class="stat-label">Database Jobs:</span>
                            <span class="stat-value">{{ props.jobSystemStatus.summary.totalInDatabase }}</span>
                        </div>
                        <div class="stat-item">
                            <span class="stat-label">Stuck:</span>
                            <span class="stat-value stuck">{{ props.jobSystemStatus.summary.stuckInDatabase }}</span>
                        </div>
                    </div>

                    <div v-if="props.jobSystemStatus.summary.stuckInDatabase > 0" class="stuck-jobs-warning">
                        <font-awesome-icon icon="fa-solid fa-exclamation-triangle" />
                        <span>{{ props.jobSystemStatus.summary.stuckInDatabase }} stuck job(s) detected</span>
                        <button class="memo-button btn btn-warning btn-sm" @click="emit('clearStuckJobs')">
                            Clear All Stuck Jobs
                        </button>
                    </div>
                </LayoutCard>
            </template>

            <template
                v-if="props.jobStatusLoaded && props.jobSystemStatus && props.jobSystemStatus.databaseJobs.length > 0">
                <LayoutCard :size="LayoutCardSize.Large" class="database-jobs-card">
                    <h4>Database Running Jobs</h4>
                    <div class="database-jobs-list">
                        <div v-for="job in props.databaseJobs" :key="job.id" class="database-job-item"
                            :class="{ 'stuck-job': job.isStuck }">
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
                            <button class="memo-button btn btn-warning btn-sm" @click="emit('clearJobById', job.id)">
                                Clear Job (Quartz + DB)
                            </button>
                        </div>
                    </div>

                    <details class="raw-json-details">
                        <summary>Show Raw JSON</summary>
                        <pre class="job-json">{{ JSON.stringify(props.databaseJobs, null, 2) }}</pre>
                    </details>
                </LayoutCard>
            </template>
        </LayoutPanel>

        <!-- Quartz Jobs Panel -->
        <LayoutPanel title="Quartz Scheduler Jobs">
            <LayoutCard :size="LayoutCardSize.Large">
                <div class="quartz-jobs-header">
                    <h4>Quartz Job Management</h4>
                    <button class="memo-button btn btn-primary" @click="emit('loadQuartzJobs')">
                        Load Quartz Jobs
                    </button>
                </div>

                <template v-if="props.quartzJobsLoaded && props.quartzJobs.length > 0">
                    <div class="quartz-jobs-list">
                        <div v-for="job in props.quartzJobs" :key="job.JobKey" class="quartz-job-item"
                            :class="{ 'executing-job': job.IsExecuting }">
                            <div class="job-info">
                                <div class="job-name">
                                    <strong>{{ job.JobName }}</strong>
                                    <span v-if="job.IsExecuting" class="executing-badge">⚡ RUNNING</span>
                                    <span class="job-type">{{ job.JobType }}</span>
                                </div>
                                <div class="job-details">
                                    <span>Key: {{ job.JobKey }}</span>
                                    <span>Group: {{ job.JobGroup }}</span>
                                    <span v-if="job.IsExecuting && job.RunTime">Runtime: {{
                                        formatDuration(job.RunTime) }}</span>
                                    <span v-if="job.FireTime">Fire Time: {{ new
                                        Date(job.FireTime).toLocaleString()
                                        }}</span>
                                </div>
                            </div>
                            <div class="job-actions">
                                <button v-if="job.IsExecuting" class="memo-button btn btn-warning btn-sm"
                                    @click="emit('interruptQuartzJob', job.JobName, job.JobGroup)">
                                    Interrupt
                                </button>
                            </div>
                        </div>
                    </div>
                </template>

                <template v-else-if="props.quartzJobsLoaded && props.quartzJobs.length === 0">
                    <p class="no-jobs-message">No Quartz jobs found.</p>
                </template>

                <template v-if="props.quartzJobsLoaded">
                    <details class="raw-json-details">
                        <summary>Show Raw Quartz Jobs JSON</summary>
                        <pre class="job-json">{{ JSON.stringify(props.quartzJobs, null, 2) }}</pre>
                    </details>
                </template>
            </LayoutCard>
        </LayoutPanel>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.active-jobs-panel {
    background-color: lightyellow;
    border-left: 4px solid @memo-yellow;
}

.running-job {
    .job-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 8px;

        h4 {
            margin: 0;
            font-size: 14px;
        }

        .clear-job-btn {
            background: transparent;
            border: none;
            cursor: pointer;
            color: #B13A48;
            padding: 4px 8px;
            border-radius: 4px;

            &:hover {
                background-color: rgba(0, 0, 0, 0.1);
                color: darken(#B13A48, 15%);
            }
        }
    }

    .job-status {
        font-size: 12px;
        color: @memo-grey-dark;
    }
}

.job-summary-card {
    h4 {
        margin-top: 0;
        margin-bottom: 16px;
        font-size: 16px;
    }

    .job-stats {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
        gap: 12px;

        .stat-item {
            display: flex;
            justify-content: space-between;
            padding: 8px 12px;
            background: @memo-grey-lighter;
            border-radius: 4px;

            .stat-label {
                font-weight: 500;
            }

            .stat-value {
                font-weight: bold;

                &.running {
                    color: @memo-blue;
                }

                &.completed {
                    color: @memo-green;
                }

                &.failed {
                    color: @memo-red-wrong;
                }

                &.stuck {
                    color: darken(@memo-yellow, 20%);
                }
            }
        }
    }

    .stuck-jobs-warning {
        margin-top: 16px;
        padding: 12px;
        background: lightyellow;
        border: 1px solid @memo-yellow;
        border-radius: 4px;
        display: flex;
        align-items: center;
        gap: 12px;

        svg {
            color: darken(@memo-yellow, 20%);
        }
    }
}

.database-jobs-card {
    h4 {
        margin-top: 0;
    }

    .database-jobs-list {
        display: flex;
        flex-direction: column;
        gap: 8px;

        .database-job-item {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 12px;
            background: @memo-grey-lighter;
            border-radius: 4px;

            &.stuck-job {
                background: lightyellow;
                border-left: 3px solid @memo-yellow;
            }

            .job-info {
                .job-name {
                    font-size: 14px;
                    margin-bottom: 4px;

                    .stuck-badge {
                        margin-left: 8px;
                        font-weight: normal;
                    }
                }

                .job-details {
                    font-size: 12px;
                    color: @memo-grey-dark;

                    span {
                        margin-right: 16px;
                    }
                }
            }
        }
    }
}

.raw-json-details {
    margin-top: 16px;

    summary {
        cursor: pointer;
        color: @memo-blue;
        font-size: 12px;

        &:hover {
            text-decoration: underline;
        }
    }

    .job-json {
        font-size: 11px;
        background: @memo-grey-lighter;
        padding: 12px;
        border-radius: 4px;
        overflow-x: auto;
        max-height: 300px;
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
    gap: 8px;

    .quartz-job-item {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 12px;
        background: @memo-grey-lighter;
        border-radius: 4px;

        &.executing-job {
            background: #e8f5e9;
            border-left: 3px solid @memo-green;
        }

        .job-info {
            .job-name {
                font-size: 14px;
                margin-bottom: 4px;

                .executing-badge {
                    margin-left: 8px;
                    color: @memo-green;
                    font-weight: normal;
                }

                .job-type {
                    margin-left: 8px;
                    font-size: 11px;
                    color: @memo-grey-dark;
                    background: rgba(0, 0, 0, 0.05);
                    padding: 2px 6px;
                    border-radius: 4px;
                }
            }

            .job-details {
                font-size: 12px;
                color: @memo-grey-dark;

                span {
                    margin-right: 16px;
                }
            }
        }
    }
}

.no-jobs-message {
    color: @memo-grey-dark;
    font-style: italic;
}
</style>
