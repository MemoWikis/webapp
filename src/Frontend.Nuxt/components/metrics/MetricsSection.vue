<script lang="ts" setup>
import { SectionChart } from './sectionChart'
interface Props {
    title: string
    subTitle: string
    charts: SectionChart[]
}

const props = defineProps<Props>()

const emit = defineEmits(['toggleBar'])
</script>

<template>
    <div class="chart-section">
        <h3>{{ title }}</h3>
        <div class="chart-header">
            <!-- Heutige Registrierungen: {{ overviewData?.todaysRegistrationCount }} -->
            {{ subTitle }}
        </div>

        <div class="chart-container" v-for="chart in charts">
            <div class="chart-toggle-section">
                <div class="chart-toggle-container" @click="emit('toggleBar', chart.barToggleKey)">
                    <div class="chart-toggle" :class="{ 'is-active': chart.showBar }">
                        <font-awesome-icon :icon="['fas', 'chart-column']" />
                    </div>
                    <div class="chart-toggle" :class="{ 'is-active': !chart.showBar }">
                        <font-awesome-icon :icon="['fas', 'chart-line']" />
                    </div>
                </div>
            </div>

            <LazySharedChartsBar v-if="chart.showBar"
                :labels="chart.labels"
                :datasets="chart.datasets"
                :title="chart.title"
                :color="chart.color" />
            <LazySharedChartsLine v-else
                :labels="chart.labels"
                :datasets="chart.datasets"
                :title="chart.title"
                :color="chart.color" />
        </div>

    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.chart-header {
    display: flex;
    margin-bottom: 20px;
    justify-content: space-between;
    font-size: 18px;
}

.chart-container {
    margin-bottom: 40px;
    min-height: 300px;
}

.chart-toggle-section {
    display: flex;
    justify-content: flex-end;
    position: absolute;
    right: 0px;

    .chart-toggle-container {
        display: flex;
        cursor: pointer;
        flex-wrap: nowrap;
        color: @memo-grey-dark;
        border-radius: 4px;
        border: solid 1px @memo-grey-lighter;

        .chart-toggle {
            justify-content: center;
            align-items: center;
            padding: 4px 12px;
            background: white;

            &.is-active {
                color: @memo-blue-link;
                background: @memo-grey-lighter;
            }

            &:hover {
                filter: brightness(0.95);
            }

            &:active {
                filter: brightness(0.9);
            }
        }

    }
}

.metrics-header {
    height: 54px;
    margin-top: 20px;
    margin-bottom: 10px;
}

.divider {
    height: 1px;
    background: @memo-grey-lighter;
    width: 100%;
    margin-top: 10px;
    margin-bottom: 60px;
}

.chart-section {
    margin-bottom: 45px;
}
</style>