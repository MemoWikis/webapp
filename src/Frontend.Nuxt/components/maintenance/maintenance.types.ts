// Maintenance Types
// Centralized type definitions for the maintenance system

// ==================== Enums ====================

export enum JobStatus {
    Running = 0,
    Completed = 1,
    Failed = 2,
    NotFound = 3,
}

export type MaintenanceTabType = 'general' | 'quartz' | 'ai' | 'ai-costs'

// ==================== Method Configuration ====================

export interface MethodData {
    url: string
    translationKey: string
}

// ==================== Job System ====================

export interface JobStatusResponse {
    jobTrackingId: string
    status: JobStatus
    message: string
    operationName: string
}

export interface InMemoryJobResponse {
    jobTrackingId: string
    status: string
    message: string
    operationName: string
}

export interface DatabaseJobResponse {
    id: number
    name: string
    startedAt: string
    duration: string
    isStuck: boolean
    durationHours: number
}

export interface JobSummaryResponse {
    totalInMemory: number
    totalInDatabase: number
    runningInMemory: number
    completedInMemory: number
    failedInMemory: number
    stuckInDatabase: number
}

export interface JobSystemStatusResponse {
    inMemoryJobs: InMemoryJobResponse[]
    databaseJobs: DatabaseJobResponse[]
    summary: JobSummaryResponse
}

export interface QuartzJob {
    JobKey: string
    JobName: string
    JobType: string
    JobGroup: string
    IsExecuting: boolean
    RunTime?: string
    FireTime?: string
}

// ==================== AI Models ====================

export interface WhitelistedModel {
    id: number
    provider: string
    modelId: string
    displayName: string
    tokenCostMultiplier: number
    inputPricePerMillion: number
    outputPricePerMillion: number
}

export interface AvailableModel {
    modelId: string
    displayName: string
    isWhitelisted: boolean
}

export interface ProviderModels {
    providerName: string
    models: AvailableModel[]
}

export interface GetWhitelistedModelsResponse {
    success: boolean
    models: WhitelistedModel[]
}

export interface GetAllProviderModelsResponse {
    success: boolean
    providers: ProviderModels[]
    error: string
}

// ==================== AI Costs ====================

export interface AiDailyCostByModelItem {
    date: string
    modelId: string
    displayName: string
    requestCount: number
    totalInputTokens: number
    totalOutputTokens: number
    totalInputCostUsd: number
    totalOutputCostUsd: number
    totalCostUsd: number
}

export interface AiCostsByDayAndModelResult {
    items: AiDailyCostByModelItem[]
    totalCostUsd: number
}

// ==================== MMap Cache ====================

export interface MmapCacheStatus {
    exists: boolean
    lastModified: string
    sizeBytes: number
}

export interface MmapCacheStatusData {
    pageViewsCache: MmapCacheStatus
    questionViewsCache: MmapCacheStatus
}

export interface GetMmapCacheStatusResult {
    success: boolean
    data: string
}

// ==================== Relations ====================

export interface RelationErrorItem {
    parentId: number
    errors: string[]
    relations: string[]
}

export interface RelationErrorsResponse {
    success: boolean
    data: RelationErrorItem[]
}

// ==================== User Sessions ====================

export interface ActiveSessionsResponse {
    loggedInUserCount: number
    anonymousUserCount: number
}

// ==================== API Responses ====================

export interface VueMaintenanceResult {
    success: boolean
    data: string
}
