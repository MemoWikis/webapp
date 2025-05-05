<script lang="ts" setup>
import { Line } from 'vue-chartjs'
import { memoBlue } from '../colors'

interface Props {
    title: string
    labels: string[]
    datasets: number[]
    color?: string
    stepSize?: number
}

const props = withDefaults(defineProps<Props>(), {
    color: memoBlue
})

const chartData = ref({
    labels: props.labels,
    datasets: [
        {
            label: props.title,
            data: props.datasets,
            borderColor: props.color,
            borderWidth: 1,
            backgroundColor: props.color,
            pointRadius: 1.5,
        }
    ]
})

const chartOptions = ref<any>({
    responsive: true,
    maintainAspectRatio: false,
})

onBeforeMount(() => {
    if (props.stepSize) {
        chartOptions.value = {
            ...chartOptions.value,
            scales: {
                y: {
                    beginAtZero: true,
                    stepSize: props.stepSize
                }
            }
        }
    }
})

</script>

<template>
    <Line :options="chartOptions" :data="chartData" />
</template>