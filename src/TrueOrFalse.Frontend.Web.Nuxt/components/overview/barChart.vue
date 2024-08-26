<script lang="ts" setup>
import { Bar } from 'vue-chartjs'

const props = defineProps({
    title: {
        type: String,
        required: true
    },
    labels: {
        type: Array as () => string[],
        required: true
    },
    datasets: {
        type: Array as () => number[],
        required: true
    },
    color: {
        type: String,
        required: true
    }
})

const chartData = {
    labels: props.labels,
    datasets: [
        {
            label: props.title,
            data: props.datasets, // Use the datasets prop
            backgroundColor: props.color
        }
    ], chartOptions: {
        responsive: true,

    }
}

const chartOptions = {
    responsive: true
}
</script>

<template>
    <Bar :options="chartOptions" :data="chartData" />
</template>