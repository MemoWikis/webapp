<script lang="ts" setup>
import { Bar } from 'vue-chartjs'

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
            data: props.datasets, // Use the datasets prop
            backgroundColor: props.color
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
    <Bar :options="chartOptions" :data="chartData" />
</template>