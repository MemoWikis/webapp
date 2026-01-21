<script lang="ts" setup>
import { useUserStore } from '~/components/user/userStore'

const { t, locale } = useI18n()
const userStore = useUserStore()

interface DailyUsageSummary {
    date: string
    requestCount: number
    totalTokensIn: number
    totalTokensOut: number
}

interface DailyModelUsage {
    date: string
    modelId: string
    displayName: string | null
    requestCount: number
    tokensIn: number
    tokensOut: number
    tokenCostMultiplier: number
}

interface AiUsageResponse {
    success: boolean
    tokenBalance: number
    subscriptionTokensBalance: number
    paidTokensBalance: number
    dailySummary: DailyUsageSummary[]
    dailyModelUsage: DailyModelUsage[]
}

const isLoading = ref(true)
const usageData = ref<AiUsageResponse | null>(null)

const loadUsageData = async () => {
    isLoading.value = true
    try {
        const [usageResult] = await Promise.all([
            $api<AiUsageResponse>('/apiVue/AiUsageStore/GetAiUsage', {
                method: 'GET',
                query: { days: 30 },
                credentials: 'include'
            }),
            userStore.fetchQuotaInfo()
        ])
        usageData.value = usageResult
    } catch (error) {
        console.error('Failed to load AI usage data:', error)
    } finally {
        isLoading.value = false
    }
}

onMounted(() => {
    loadUsageData()
})

function formatResetDate(date: Date): string {
    return date.toLocaleDateString(locale.value, {
        weekday: 'long',
        day: 'numeric',
        month: 'long',
        year: 'numeric'
    })
}
const formatNumber = (num: number): string => {
    return new Intl.NumberFormat().format(num)
}

const formatDate = (dateString: string): string => {
    const date = new Date(dateString)
    return date.toLocaleDateString(undefined, { weekday: 'short', year: 'numeric', month: 'short', day: 'numeric' })
}

const totalRequests = computed(() => {
    if (!usageData.value?.dailySummary) return 0
    return usageData.value.dailySummary.reduce((sum, day) => sum + day.requestCount, 0)
})

const totalTokensIn = computed(() => {
    if (!usageData.value?.dailySummary) return 0
    return usageData.value.dailySummary.reduce((sum, day) => sum + day.totalTokensIn, 0)
})

const totalTokensOut = computed(() => {
    if (!usageData.value?.dailySummary) return 0
    return usageData.value.dailySummary.reduce((sum, day) => sum + day.totalTokensOut, 0)
})

interface GroupedDailyData {
    date: string
    requestCount: number
    totalTokensIn: number
    totalTokensOut: number
    models: DailyModelUsage[]
}

const groupedByDate = computed((): GroupedDailyData[] => {
    if (!usageData.value) return []

    const dateMap = new Map<string, GroupedDailyData>()

    for (const summary of usageData.value.dailySummary) {
        dateMap.set(summary.date, {
            date: summary.date,
            requestCount: summary.requestCount,
            totalTokensIn: summary.totalTokensIn,
            totalTokensOut: summary.totalTokensOut,
            models: []
        })
    }

    for (const modelUsage of usageData.value.dailyModelUsage) {
        const dayData = dateMap.get(modelUsage.date)
        if (dayData) {
            dayData.models.push(modelUsage)
        }
    }

    return Array.from(dateMap.values())
})

const expandedDates = ref<Set<string>>(new Set())
const showHelp = ref(true)

const toggleDate = (date: string) => {
    if (expandedDates.value.has(date)) {
        expandedDates.value.delete(date)
    } else {
        expandedDates.value.add(date)
    }
}
</script>

<template>
    <div class="ai-usage-container">
        <div v-if="isLoading" class="loading-state">
            <font-awesome-icon icon="fa-solid fa-spinner" spin />
            {{ t('settings.aiUsage.loading') }}
        </div>

        <template v-else-if="usageData?.success">
            <!-- Weekly Quota Progress Section -->
            <div v-if="userStore.quotaInfo" class="settings-section quota-section">
                <div class="overline-s no-line">{{ t('settings.aiUsage.weeklyQuotaProgress') }}</div>
                <div class="quota-card">
                    <div class="quota-header">
                        <div class="quota-title">
                            <font-awesome-icon :icon="['fas', 'chart-pie']" />
                            {{ t('settings.aiUsage.currentQuota') }}
                        </div>
                        <div v-if="userStore.quotaInfo.hasActiveSubscription && userStore.quotaInfo.nextResetDate"
                            class="quota-reset">
                            <font-awesome-icon :icon="['fas', 'calendar-alt']" />
                            {{ t('settings.aiUsage.resetDate') }}: {{ formatResetDate(userStore.quotaInfo.nextResetDate)
                            }}
                        </div>
                    </div>

                    <div class="quota-progress-container">
                        <div class="quota-progress-bar">
                            <div class="quota-progress-fill" :class="{
                                'low': userStore.quotaInfo.percentageUsed > 80,
                                'depleted': userStore.quotaInfo.isQuotaDepleted
                            }" :style="{ width: `${userStore.quotaInfo.percentageUsed}%` }" />
                        </div>
                        <div class="quota-values">
                            <span class="quota-remaining">
                                {{ userStore.quotaInfo.tokensUsedThisWeek.toLocaleString() }}
                            </span>
                            <span class="quota-separator">{{ t('settings.aiUsage.of') }}</span>
                            <span class="quota-total">
                                {{ userStore.quotaInfo.weeklyLimit.toLocaleString() }} {{ t('settings.aiUsage.points')
                                }}
                            </span>
                            <span class="quota-percentage">({{ userStore.quotaInfo.percentageUsed.toFixed(0)
                            }}% {{ t('settings.aiUsage.used') }})</span>
                        </div>
                    </div>

                    <div v-if="userStore.quotaInfo.isQuotaDepleted" class="quota-depleted-alert">
                        <font-awesome-icon :icon="['fas', 'exclamation-triangle']" />
                        <div class="alert-content">
                            <div class="alert-title">{{ t('settings.aiUsage.quotaDepleted') }}</div>
                            <div class="alert-message">{{ t('settings.aiUsage.quotaDepletedMessage') }}</div>
                        </div>
                    </div>
                </div>
            </div>



            <!-- Usage Summary Section -->
            <div class="settings-section">
                <div class="overline-s no-line">{{ t('settings.aiUsage.last30Days') }}</div>
                <div class="summary-stats">
                    <div class="stat">
                        <span class="stat-value">{{ formatNumber(totalRequests) }}</span>
                        <span class="stat-label">{{ t('settings.aiUsage.requests') }}</span>
                    </div>
                    <div class="stat tokens-in">
                        <span class="stat-value">
                            <font-awesome-icon icon="fa-solid fa-arrow-down" class="stat-icon" />
                            {{ formatNumber(totalTokensIn) }}
                        </span>
                        <span class="stat-label">{{ t('settings.aiUsage.pointsIn') }}</span>
                    </div>
                    <div class="stat tokens-out">
                        <span class="stat-value">
                            <font-awesome-icon icon="fa-solid fa-arrow-up" class="stat-icon" />
                            {{ formatNumber(totalTokensOut) }}
                        </span>
                        <span class="stat-label">{{ t('settings.aiUsage.pointsOut') }}</span>
                    </div>
                </div>
            </div>

            <!-- Daily Usage Table -->
            <div class="settings-section">
                <div class="overline-s no-line">{{ t('settings.aiUsage.dailyBreakdown') }}</div>

                <!-- Help Text -->
                <div v-if="showHelp" class="help-box">
                    <div class="help-header">
                        <font-awesome-icon :icon="['fas', 'info-circle']" />
                        <span class="help-title">{{ t('settings.aiUsage.helpTitle') }}</span>
                        <button class="help-close" @click="showHelp = false" :title="'Schließen'">
                            <font-awesome-icon :icon="['fas', 'times']" />
                        </button>
                    </div>
                    <p class="help-text">{{ t('settings.aiUsage.helpText') }}</p>
                </div>

                <div v-if="groupedByDate.length === 0" class="no-data">
                    {{ t('settings.aiUsage.noUsageData') }}
                </div>

                <div v-else class="usage-table">
                    <div v-for="day in groupedByDate" :key="day.date" class="day-row">
                        <div class="day-header" @click="toggleDate(day.date)">
                            <div class="day-date">
                                <font-awesome-icon
                                    :icon="expandedDates.has(day.date) ? 'fa-solid fa-chevron-down' : 'fa-solid fa-chevron-right'"
                                    class="expand-icon" />
                                {{ formatDate(day.date) }}
                            </div>
                            <div class="day-stats">
                                <span class="stat-pill requests">
                                    {{ day.requestCount }} {{ t('settings.aiUsage.requests') }}
                                </span>
                                <span class="stat-pill tokens-in" v-tooltip="t('settings.aiUsage.tooltipTokensIn')">
                                    <font-awesome-icon icon="fa-solid fa-arrow-down" />
                                    {{ formatNumber(day.totalTokensIn) }}
                                </span>
                                <span class="stat-pill tokens-out" v-tooltip="t('settings.aiUsage.tooltipTokensOut')">
                                    <font-awesome-icon icon="fa-solid fa-arrow-up" />
                                    {{ formatNumber(day.totalTokensOut) }}
                                </span>
                            </div>
                        </div>

                        <Transition name="expand">
                            <div v-if="expandedDates.has(day.date)" class="day-details">
                                <div v-for="model in day.models" :key="`${day.date}-${model.modelId}`"
                                    class="model-row">
                                    <div class="model-info">
                                        <span class="model-name">
                                            {{ model.displayName || model.modelId }}
                                        </span>
                                        <span v-if="model.tokenCostMultiplier && model.tokenCostMultiplier !== 1"
                                            class="multiplier-badge"
                                            v-tooltip="t('settings.aiUsage.tooltipMultiplier', { value: model.tokenCostMultiplier })">
                                            {{ model.tokenCostMultiplier }}×
                                        </span>
                                    </div>
                                    <div class="model-stats">
                                        <span class="stat-mini">{{ model.requestCount }}x</span>
                                        <span class="stat-mini tokens-in"
                                            v-tooltip="t('settings.aiUsage.tooltipTokensIn')">
                                            <font-awesome-icon icon="fa-solid fa-arrow-down" />
                                            {{ formatNumber(model.tokensIn) }}
                                        </span>
                                        <span class="stat-mini tokens-out"
                                            v-tooltip="t('settings.aiUsage.tooltipTokensOut')">
                                            <font-awesome-icon icon="fa-solid fa-arrow-up" />
                                            {{ formatNumber(model.tokensOut) }}
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </Transition>
                    </div>
                </div>
            </div>
        </template>

        <div v-else class="error-state">
            <div class="alert alert-danger">{{ t('settings.aiUsage.loadError') }}</div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.ai-usage-container {
    .loading-state {
        text-align: center;
        padding: 40px;
        color: @memo-grey-dark;

        svg {
            margin-right: 8px;
        }
    }

    .settings-section {
        margin-bottom: 32px;
    }

    // Quota Progress Section
    .quota-section {
        .quota-card {
            background: white;
            border: 1px solid @memo-grey-light;
            border-radius: 12px;
            padding: 20px;

            .quota-header {
                display: flex;
                justify-content: space-between;
                align-items: center;
                margin-bottom: 16px;
                flex-wrap: wrap;
                gap: 8px;

                .quota-title {
                    font-weight: 600;
                    color: @memo-blue;
                    display: flex;
                    align-items: center;
                    gap: 8px;
                }

                .quota-reset {
                    font-size: 13px;
                    color: @memo-grey-dark;
                    display: flex;
                    align-items: center;
                    gap: 6px;

                    svg {
                        color: @memo-blue;
                    }
                }
            }

            .quota-progress-container {
                .quota-progress-bar {
                    height: 12px;
                    background: @memo-grey-lighter;
                    border-radius: 6px;
                    overflow: hidden;
                    margin-bottom: 8px;

                    .quota-progress-fill {
                        height: 100%;
                        background: linear-gradient(90deg, @memo-green, darken(@memo-green, 10%));
                        border-radius: 6px;
                        transition: width 0.5s ease;

                        &.low {
                            background: linear-gradient(90deg, @memo-yellow, darken(@memo-yellow, 20%));
                        }

                        &.depleted {
                            background: #B13A48;
                            width: 0 !important;
                        }
                    }
                }

                .quota-values {
                    display: flex;
                    align-items: baseline;
                    gap: 6px;
                    font-size: 14px;

                    .quota-remaining {
                        font-weight: 700;
                        font-size: 18px;
                        color: @memo-blue;
                    }

                    .quota-separator,
                    .quota-total {
                        color: @memo-grey-dark;
                    }

                    .quota-percentage {
                        color: @memo-green;
                        font-weight: 500;
                    }
                }
            }

            .quota-depleted-alert {
                display: flex;
                align-items: flex-start;
                gap: 12px;
                margin-top: 16px;
                padding: 12px 16px;
                background: fade(#B13A48, 8%);
                border: 1px solid fade(#B13A48, 20%);
                border-radius: 8px;

                >svg {
                    color: #B13A48;
                    font-size: 18px;
                    flex-shrink: 0;
                    margin-top: 2px;
                }

                .alert-content {
                    .alert-title {
                        font-weight: 600;
                        color: #B13A48;
                        margin-bottom: 4px;
                    }

                    .alert-message {
                        font-size: 13px;
                        color: @memo-grey-darker;
                        line-height: 1.5;
                    }
                }
            }
        }
    }

    .balance-cards {
        display: flex;
        gap: 16px;
        flex-wrap: wrap;

        .balance-card {
            flex: 1;
            min-width: 140px;
            padding: 16px;
            border-radius: 8px;
            text-align: center;

            &.total {
                background: linear-gradient(135deg, @memo-blue 0%, @memo-blue-link 100%);
                color: white;
            }

            &.subscription {
                background: @memo-grey-lighter;
                border: 1px solid @memo-grey-light;
            }

            &.paid {
                background: @memo-grey-lighter;
                border: 1px solid @memo-grey-light;
            }

            .balance-value {
                font-size: 24px;
                font-weight: 700;
                margin-bottom: 4px;
            }

            .balance-label {
                font-size: 12px;
                opacity: 0.9;
            }
        }
    }

    .summary-stats {
        display: flex;
        gap: 24px;
        flex-wrap: wrap;

        .stat {
            display: flex;
            flex-direction: column;

            .stat-value {
                display: flex;
                align-items: center;
                gap: 6px;
                font-size: 20px;
                font-weight: 600;
                color: @memo-blue;

                .stat-icon {
                    font-size: 14px;
                }
            }

            .stat-label {
                font-size: 12px;
                color: @memo-grey-dark;
            }

            &.tokens-in .stat-value {
                color: @memo-green;
            }

            &.tokens-out .stat-value {
                color: @memo-blue-link;
            }
        }
    }

    .help-box {
        background: fade(@memo-blue, 8%);
        border: 1px solid fade(@memo-blue, 20%);
        border-radius: 8px;
        padding: 12px 16px;
        margin-bottom: 16px;

        .help-header {
            display: flex;
            align-items: center;
            gap: 8px;
            margin-bottom: 6px;

            >svg {
                color: @memo-blue;
                font-size: 14px;
            }

            .help-title {
                font-weight: 600;
                color: @memo-blue;
                font-size: 13px;
                flex: 1;
            }

            .help-close {
                background: none;
                border: none;
                color: @memo-grey-dark;
                cursor: pointer;
                padding: 4px;
                line-height: 1;

                &:hover {
                    color: @memo-blue;
                }
            }
        }

        .help-text {
            margin: 0;
            font-size: 13px;
            color: @memo-grey-darker;
            line-height: 1.5;
        }
    }

    .no-data {
        padding: 24px;
        text-align: center;
        color: @memo-grey-dark;
        background: @memo-grey-lighter;
        border-radius: 8px;
    }

    .usage-table {
        border: 1px solid @memo-grey-light;
        border-radius: 8px;
        overflow: hidden;

        .day-row {
            border-bottom: 1px solid @memo-grey-light;

            &:last-child {
                border-bottom: none;
            }
        }

        .day-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 12px 16px;
            cursor: pointer;
            background: white;
            transition: background-color 0.2s;

            &:hover {
                background: @memo-grey-lighter;
            }

            .day-date {
                font-weight: 500;
                display: flex;
                align-items: center;
                gap: 8px;

                .expand-icon {
                    width: 12px;
                    color: @memo-grey-dark;
                }
            }

            .day-stats {
                display: flex;
                gap: 8px;
                flex-wrap: wrap;

                .stat-pill {
                    padding: 4px 8px;
                    border-radius: 12px;
                    font-size: 12px;
                    background: @memo-grey-lighter;

                    svg {
                        margin-right: 4px;
                        font-size: 10px;
                    }

                    &.requests {
                        background: @memo-blue;
                        color: white;
                    }

                    &.tokens-in {
                        color: @memo-green;
                    }

                    &.tokens-out {
                        color: @memo-blue-link;
                    }
                }
            }
        }

        .day-details {
            background: @memo-grey-lighter;
            padding: 8px 16px 8px 36px;

            .model-row {
                display: flex;
                justify-content: space-between;
                align-items: center;
                padding: 8px 0;
                border-bottom: 1px solid @memo-grey-light;

                &:last-child {
                    border-bottom: none;
                }

                .model-info {
                    display: flex;
                    align-items: center;
                    gap: 8px;

                    .model-name {
                        font-size: 13px;
                        color: @memo-grey-dark;
                    }

                    .multiplier-badge {
                        display: inline-flex;
                        align-items: center;
                        padding: 2px 6px;
                        background: fade(@memo-blue, 12%);
                        color: @memo-blue;
                        border-radius: 4px;
                        font-size: 11px;
                        font-weight: 600;
                        cursor: help;
                    }
                }

                .model-stats {
                    display: flex;
                    gap: 12px;

                    .stat-mini {
                        font-size: 12px;
                        color: @memo-grey-dark;

                        svg {
                            margin-right: 2px;
                            font-size: 10px;
                        }

                        &.tokens-in {
                            color: @memo-green;
                        }

                        &.tokens-out {
                            color: @memo-blue-link;
                        }
                    }
                }
            }
        }
    }

    .error-state {
        padding: 20px 0;
    }
}

.expand-enter-active,
.expand-leave-active {
    transition: all 0.2s ease;
}

.expand-enter-from,
.expand-leave-to {
    opacity: 0;
    transform: translateY(-8px);
}

@media (max-width: 600px) {
    .ai-usage-container {
        .balance-cards {
            flex-direction: column;

            .balance-card {
                min-width: 100%;
            }
        }

        .day-header {
            flex-direction: column;
            align-items: flex-start;
            gap: 8px;

            .day-stats {
                margin-left: 20px;
            }
        }

        .model-row {
            flex-direction: column;
            align-items: flex-start !important;
            gap: 4px;
        }
    }
}
</style>
