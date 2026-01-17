<script lang="ts" setup>
const { t } = useI18n()

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
        const result = await $api<AiUsageResponse>('/apiVue/AiUsageStore/GetAiUsage', {
            method: 'GET',
            query: { days: 30 },
            credentials: 'include'
        })
        usageData.value = result
    } catch (error) {
        console.error('Failed to load AI usage data:', error)
    } finally {
        isLoading.value = false
    }
}

onMounted(() => {
    loadUsageData()
})

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
            <!-- Token Balance Section -->
            <div class="settings-section">
                <div class="overline-s no-line">{{ t('settings.aiUsage.tokenBalance') }}</div>
                <div class="balance-cards">
                    <div class="balance-card total">
                        <div class="balance-value">{{ formatNumber(usageData.tokenBalance) }}</div>
                        <div class="balance-label">{{ t('settings.aiUsage.totalBalance') }}</div>
                    </div>
                    <div class="balance-card subscription">
                        <div class="balance-value">{{ formatNumber(usageData.subscriptionTokensBalance) }}</div>
                        <div class="balance-label">{{ t('settings.aiUsage.subscriptionTokens') }}</div>
                    </div>
                    <div class="balance-card paid">
                        <div class="balance-value">{{ formatNumber(usageData.paidTokensBalance) }}</div>
                        <div class="balance-label">{{ t('settings.aiUsage.paidTokens') }}</div>
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
                    <div class="stat">
                        <span class="stat-value">{{ formatNumber(totalTokensIn) }}</span>
                        <span class="stat-label">{{ t('settings.aiUsage.tokensIn') }}</span>
                    </div>
                    <div class="stat">
                        <span class="stat-value">{{ formatNumber(totalTokensOut) }}</span>
                        <span class="stat-label">{{ t('settings.aiUsage.tokensOut') }}</span>
                    </div>
                </div>
            </div>

            <!-- Daily Usage Table -->
            <div class="settings-section">
                <div class="overline-s no-line">{{ t('settings.aiUsage.dailyBreakdown') }}</div>

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
                                <span class="stat-pill tokens-in">
                                    <font-awesome-icon icon="fa-solid fa-arrow-down" />
                                    {{ formatNumber(day.totalTokensIn) }}
                                </span>
                                <span class="stat-pill tokens-out">
                                    <font-awesome-icon icon="fa-solid fa-arrow-up" />
                                    {{ formatNumber(day.totalTokensOut) }}
                                </span>
                            </div>
                        </div>

                        <Transition name="expand">
                            <div v-if="expandedDates.has(day.date)" class="day-details">
                                <div v-for="model in day.models" :key="`${day.date}-${model.modelId}`"
                                    class="model-row">
                                    <div class="model-name">
                                        {{ model.displayName || model.modelId }}
                                    </div>
                                    <div class="model-stats">
                                        <span class="stat-mini">{{ model.requestCount }}x</span>
                                        <span class="stat-mini tokens-in">
                                            <font-awesome-icon icon="fa-solid fa-arrow-down" />
                                            {{ formatNumber(model.tokensIn) }}
                                        </span>
                                        <span class="stat-mini tokens-out">
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
                font-size: 20px;
                font-weight: 600;
                color: @memo-blue;
            }

            .stat-label {
                font-size: 12px;
                color: @memo-grey-dark;
            }
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

                .model-name {
                    font-size: 13px;
                    color: @memo-grey-dark;
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
