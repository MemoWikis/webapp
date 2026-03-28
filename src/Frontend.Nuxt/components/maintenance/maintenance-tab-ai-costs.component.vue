<script lang="ts" setup>
import type { AiDailyCostByModelItem, AiCostsByDayAndModelResult } from './maintenance.types'
import { $api } from '~~/composables/fetchWithError'

// Props
const props = defineProps<{
    antiForgeryToken?: string
}>()

const { t } = useI18n()

// State
const loading = ref(false)
const days = ref(30)
const items = ref<AiDailyCostByModelItem[]>([])
const totalCostUsd = ref(0)

// Group items by date
const groupedByDate = computed(() => {
    const groups: Record<string, AiDailyCostByModelItem[]> = {}
    for (const item of items.value) {
        if (!groups[item.date]) {
            groups[item.date] = []
        }
        groups[item.date].push(item)
    }
    return groups
})

// Get daily totals
const dailyTotals = computed(() => {
    const totals: Record<string, { cost: number; requests: number; tokens: number }> = {}
    for (const [date, dayItems] of Object.entries(groupedByDate.value)) {
        totals[date] = {
            cost: dayItems.reduce((sum, i) => sum + i.totalCostUsd, 0),
            requests: dayItems.reduce((sum, i) => sum + i.requestCount, 0),
            tokens: dayItems.reduce((sum, i) => sum + i.totalInputTokens + i.totalOutputTokens, 0)
        }
    }
    return totals
})

// Format currency
const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('de-DE', {
        style: 'currency',
        currency: 'USD',
        minimumFractionDigits: 6,
        maximumFractionDigits: 6
    }).format(value)
}

// Format number with thousand separators
const formatNumber = (value: number) => {
    return new Intl.NumberFormat('de-DE').format(value)
}

// Format date for display
const formatDate = (dateStr: string) => {
    const date = new Date(dateStr)
    return date.toLocaleDateString('de-DE', {
        weekday: 'short',
        year: 'numeric',
        month: '2-digit',
        day: '2-digit'
    })
}

// Fetch data
async function fetchData() {
    if (!props.antiForgeryToken) {
        console.error('Missing antiForgeryToken')
        return
    }

    loading.value = true
    try {
        const formData = new FormData()
        formData.append('__RequestVerificationToken', props.antiForgeryToken)
        formData.append('days', days.value.toString())

        const result = await $api<AiCostsByDayAndModelResult>('/apiVue/VueMaintenance/GetAiCostsByDayAndModel', {
            method: 'POST',
            body: formData,
            mode: 'cors',
            credentials: 'include'
        })
        items.value = result.items
        totalCostUsd.value = result.totalCostUsd
    } catch (error) {
        console.error('Failed to fetch AI costs:', error)
    } finally {
        loading.value = false
    }
}

// Fetch when token becomes available
watch(() => props.antiForgeryToken, (token) => {
    if (token && items.value.length === 0) {
        fetchData()
    }
}, { immediate: true })
</script>

<template>
    <div class="ai-costs-tab">
        <!-- Header with filters -->
        <div class="ai-costs-header">
            <h3>{{ t('maintenance.aiCosts.title') }}</h3>
            <div class="filter-controls">
                <label>
                    {{ t('maintenance.aiCosts.days') }}:
                    <select v-model="days" @change="fetchData">
                        <option :value="7">7</option>
                        <option :value="14">14</option>
                        <option :value="30">30</option>
                        <option :value="60">60</option>
                        <option :value="90">90</option>
                    </select>
                </label>
                <button class="btn btn-link" :disabled="loading" @click="fetchData">
                    <font-awesome-icon v-if="loading" :icon="['fas', 'spinner']" spin />
                    <font-awesome-icon v-else :icon="['fas', 'sync']" />
                </button>
            </div>
        </div>

        <!-- Total summary -->
        <div class="total-summary">
            <div class="summary-item">
                <span class="label">{{ t('maintenance.aiCosts.totalCost') }}:</span>
                <span class="value">{{ formatCurrency(totalCostUsd) }}</span>
            </div>
            <div class="summary-item">
                <span class="label">{{ t('maintenance.aiCosts.totalRequests') }}:</span>
                <span class="value">{{formatNumber(items.reduce((sum, i) => sum + i.requestCount, 0))}}</span>
            </div>
        </div>

        <!-- Loading state -->
        <div v-if="loading" class="loading-state">
            <font-awesome-icon :icon="['fas', 'spinner']" spin size="2x" />
        </div>

        <!-- Data by day -->
        <div v-else class="costs-by-day">
            <div v-for="(dayItems, date) in groupedByDate" :key="date" class="day-group">
                <div class="day-header">
                    <span class="date">{{ formatDate(date) }}</span>
                    <span class="day-total">
                        {{ formatCurrency(dailyTotals[date].cost) }}
                        ({{ formatNumber(dailyTotals[date].requests) }} {{ t('maintenance.aiCosts.requests') }})
                    </span>
                </div>
                <table class="models-table">
                    <thead>
                        <tr>
                            <th>{{ t('maintenance.aiCosts.model') }}</th>
                            <th class="text-right">{{ t('maintenance.aiCosts.requests') }}</th>
                            <th class="text-right">{{ t('maintenance.aiCosts.inputTokens') }}</th>
                            <th class="text-right">{{ t('maintenance.aiCosts.outputTokens') }}</th>
                            <th class="text-right">{{ t('maintenance.aiCosts.inputCost') }}</th>
                            <th class="text-right">{{ t('maintenance.aiCosts.outputCost') }}</th>
                            <th class="text-right">{{ t('maintenance.aiCosts.totalCost') }}</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in dayItems" :key="`${date}-${item.modelId}`">
                            <td>{{ item.displayName }}</td>
                            <td class="text-right">{{ formatNumber(item.requestCount) }}</td>
                            <td class="text-right">{{ formatNumber(item.totalInputTokens) }}</td>
                            <td class="text-right">{{ formatNumber(item.totalOutputTokens) }}</td>
                            <td class="text-right">{{ formatCurrency(item.totalInputCostUsd) }}</td>
                            <td class="text-right">{{ formatCurrency(item.totalOutputCostUsd) }}</td>
                            <td class="text-right total-cell">{{ formatCurrency(item.totalCostUsd) }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <!-- Empty state -->
            <div v-if="Object.keys(groupedByDate).length === 0" class="empty-state">
                {{ t('maintenance.aiCosts.noData') }}
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~/assets/includes/imports.less';

.ai-costs-tab {
    padding: 20px;
}

.ai-costs-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;

    h3 {
        margin: 0;
    }

    .filter-controls {
        display: flex;
        align-items: center;
        gap: 15px;

        label {
            display: flex;
            align-items: center;
            gap: 8px;
        }

        select {
            padding: 4px 8px;
            border-radius: 4px;
            border: 1px solid @memo-grey-light;
        }
    }
}

.total-summary {
    display: flex;
    gap: 30px;
    padding: 15px;
    background: @memo-grey-lighter;
    border-radius: 8px;
    margin-bottom: 20px;

    .summary-item {
        display: flex;
        gap: 8px;

        .label {
            font-weight: 500;
        }

        .value {
            font-weight: 700;
            color: @memo-blue;
        }
    }
}

.loading-state {
    display: flex;
    justify-content: center;
    padding: 40px;
    color: @memo-grey-dark;
}

.costs-by-day {
    display: flex;
    flex-direction: column;
    gap: 20px;
}

.day-group {
    border: 1px solid @memo-grey-light;
    border-radius: 8px;
    overflow: hidden;
}

.day-header {
    display: flex;
    justify-content: space-between;
    padding: 12px 15px;
    background: @memo-grey-lighter;
    font-weight: 600;

    .date {
        color: @memo-grey-darker;
    }

    .day-total {
        color: @memo-blue;
    }
}

.models-table {
    width: 100%;
    border-collapse: collapse;

    th,
    td {
        padding: 10px 15px;
        border-top: 1px solid @memo-grey-light;
    }

    th {
        background: @memo-grey-lighter;
        font-weight: 500;
        font-size: 0.9em;
        color: @memo-grey-dark;
    }

    .text-right {
        text-align: right;
    }

    .total-cell {
        font-weight: 600;
        color: @memo-blue;
    }

    tbody tr:hover {
        background: fade(@memo-blue, 5%);
    }
}

.empty-state {
    text-align: center;
    padding: 40px;
    color: @memo-grey-dark;
    font-style: italic;
}
</style>
