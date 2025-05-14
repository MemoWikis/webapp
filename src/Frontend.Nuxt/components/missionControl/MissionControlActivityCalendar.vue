<script setup lang="ts">
import { ActivityCalendarData } from '~/composables/missionControl/activityCalendar'
import { formatDate, getDaysBetween } from '../shared/utils'

const props = defineProps<{ calendarData?: ActivityCalendarData }>()

const today = new Date()
today.setHours(0, 0, 0, 0)
const rawStart = new Date(today)
rawStart.setDate(rawStart.getDate() - 364)

const startDate = new Date(rawStart)
startDate.setDate(rawStart.getDate() - rawStart.getDay())
const endDate = today

const activities = computed(() => props.calendarData?.activity ?? [])
const activityMap = computed<Record<string, number>>(() => {
    const map: Record<string, number> = {}
    activities.value.forEach(({ day, count }) => {
        map[formatDate(new Date(day))] = count
    })
    return map
})

const fullDays = computed(() => getDaysBetween(startDate, endDate))
const weeks = computed(() => {
    const weeksArray: Array<Array<{ date: string; count: number } | null>> = []
    fullDays.value.forEach(d => {
        const dateStr = formatDate(d)
        const weekIndex = Math.floor((d.getTime() - startDate.getTime()) / (7 * 24 * 60 * 60 * 1000))
        if (!weeksArray[weekIndex]) weeksArray[weekIndex] = Array(7).fill(null)
        weeksArray[weekIndex][d.getDay()] = { date: dateStr, count: activityMap.value[dateStr] || 0 }
    })
    return weeksArray
})

const pastMap = computed(() => fullDays.value.map(d => ({ date: formatDate(d), count: activityMap.value[formatDate(d)] || 0 })))
const longestStreak = computed(() => {
    let max = 0, curr = 0
    pastMap.value.forEach(d => { if (d.count > 0) { curr++; max = Math.max(max, curr) } else curr = 0 })
    return max
})
const currentStreak = computed(() => {
    let streak = 0
    for (let i = pastMap.value.length - 1; i >= 0; i--) {
        if (pastMap.value[i].count > 0) streak++; else break
    }
    return streak
})

// Labels
const dayLabels = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat']
const months = computed(() => {
    const monthLabels: string[] = []
    let lastMonth = ''
    weeks.value.forEach((week, i) => {
        const dayObj = week.find(d => d !== null)
        if (dayObj) {
            const mName = new Date(dayObj.date).toLocaleString('default', { month: 'short' })
            monthLabels[i] = mName !== lastMonth ? (lastMonth = mName, mName) : ''
        } else {
            monthLabels[i] = ''
        }
    })
    return monthLabels
})

function getColorClass(count: number) {
    if (count === 0) return 'level-0'
    if (count < 3) return 'level-1'
    if (count < 5) return 'level-2'
    if (count < 8) return 'level-3'
    return 'level-4'
}
</script>

<template>
    <div class="activity-calendar">
        <div class="stats">
            <div>Current Streak: {{ currentStreak }} days</div>
            <div>Longest Streak: {{ longestStreak }} days</div>
        </div>
        <div class="grid">
            <table>
                <thead>
                    <tr>
                        <th></th>
                        <th v-for="(month, idx) in months" :key="idx" class="month-label">{{ month }}</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(label, d) in dayLabels" :key="d">
                        <th class="day-label">
                            <span v-if="[1, 3, 5].includes(d)">{{ label }}</span>
                        </th>
                        <td
                            v-for="(week, wIdx) in weeks"
                            :key="wIdx"
                            class="day"
                            :class="getColorClass(week[d]?.count || 0)"
                            v-tooltip="`${week[d]?.date}: ${week[d]?.count} contribution(s)`"></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.activity-calendar {
    .stats {
        display: flex;
        gap: 1rem;
        margin-bottom: .5rem;
    }

    .grid {
        table {
            border-collapse: separate;
            border-spacing: 2px;
            table-layout: fixed;

            thead {
                tr {
                    th.month-label {
                        width: 14px;
                        max-width: 14px;
                        overflow: visible;
                        white-space: nowrap;
                        padding: 0 2px;
                        font-size: 1rem;
                        text-align: center;
                        color: @memo-grey-dark;
                    }
                }
            }

            tbody {
                tr {
                    th.day-label {
                        width: 30px;
                        padding-right: 4px;
                        font-size: 1rem;
                        text-align: right;
                        color: @memo-grey-dark;
                    }

                    td.day {
                        width: 14px;
                        height: 14px;
                        border-radius: 2px;
                        transition: transform .1s;

                        &:hover {
                            transform: scale(1.2);
                        }

                        &.level-0 {
                            background: #ebedf0;
                        }

                        &.level-1 {
                            background: #c6e48b;
                        }

                        &.level-2 {
                            background: #7bc96f;
                        }

                        &.level-3 {
                            background: #239a3b;
                        }

                        &.level-4 {
                            background: #196127;
                        }
                    }
                }
            }
        }
    }
}
</style>
