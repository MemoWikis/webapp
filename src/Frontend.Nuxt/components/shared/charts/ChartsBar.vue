<script lang="ts" setup>
import { Bar } from 'vue-chartjs'
import { memoBlue } from '~/constants/colors'

interface Props {
    title?: string
    labels: string[]
    color?: string
    stepSize?: number
    maxTicksLimit?: number
    datasets: {
        label: string
        data: number[]
        backgroundColor: string
    }[] | number[],
    stacked?: boolean
}

const props = withDefaults(defineProps<Props>(), {
    color: memoBlue
})

const chartData = ref<{
    labels: string[],
    datasets: {
        label: string
        data: number[]
        backgroundColor: string
    }[]
}>({
    labels: props.labels,
    datasets: []
})

const chartOptions = ref<any>({
    responsive: true,
    maintainAspectRatio: false,
})

onBeforeMount(() => {
    const isNumberArray = Array.isArray(props.datasets) &&
        props.datasets.length > 0 &&
        typeof props.datasets[0] === 'number'

    if (isNumberArray) {
        chartData.value = {
            labels: props.labels,
            datasets: [{
                label: props.title || 'Data',
                data: props.datasets as number[],
                backgroundColor: props.color
            }]
        }
    } else {
        chartData.value = {
            labels: props.labels,
            datasets: props.datasets as {
                label: string
                data: number[]
                backgroundColor: string
            }[]
        }
    }

    chartOptions.value.scales = {
        y: {
            beginAtZero: true
        },
        x: {}
    }

    if (props.stepSize) {
        chartOptions.value.scales.y.stepSize = props.stepSize
    }

    if (props.maxTicksLimit) {
        if (!chartOptions.value.scales.y.ticks) {
            chartOptions.value.scales.y.ticks = {}
        }
        if (!chartOptions.value.scales.x.ticks) {
            chartOptions.value.scales.x.ticks = {}
        }
        chartOptions.value.scales.y.ticks.maxTicksLimit = props.maxTicksLimit
        chartOptions.value.scales.x.ticks.maxTicksLimit = props.maxTicksLimit
    }

    if (props.stacked) {
        chartOptions.value.scales.y.stacked = true
        chartOptions.value.scales.x.stacked = true
    }
})

</script>

<template>
    <Bar :options="chartOptions" :data="chartData" />
</template>