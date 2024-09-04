<script lang="ts" setup>
import { Line } from 'vue-chartjs'

interface Props {
    title: string
    labels: string[]
    datasets: number[]
    color: string
}

const props = defineProps<Props>()

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
    ],
    chartOptions: {
        responsive: true,
    }
})

const chartOptions = ref({
    responsive: true
})

</script>

<template>
    <Line :options="chartOptions" :data="chartData" />
</template>