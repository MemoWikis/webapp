<script lang="ts" setup>
import * as d3 from "d3"
import { ChartData } from "./chartData"

interface Props {
    data: ChartData[]
    width?: number
    height?: number
    singleColor?: boolean
}

const props = withDefaults(defineProps<Props>(), {
    width: 40,
    height: 40,
})

const chartData = ref<ChartData[]>([
    {
        value: 70,
        class: 'placeholder-lighter'
    },
    {
        value: 30,
        class: 'placeholder-light'
    }
])

const svg = ref<SVGSVGElement | null>(null)

function drawPie() {
    if (!svg.value)
        return

    const radius = Math.min(props.width, props.height) / 2

    const arc = d3
        .arc<d3.PieArcDatum<ChartData>>()
        .innerRadius(0)
        .outerRadius(radius)

    const pie = d3
        .pie<ChartData>()
        .sort(null)
        .value((d) => d.value)

    const pieData = pie(chartData.value)

    const g = d3
        .select(svg.value)
        .append("g")
        .attr("transform", `translate(${props.width / 2}, ${props.height / 2})`)

    g.selectAll(".arc")
        .data(pieData)
        .enter()
        .append("path")
        .attr("class", (d) => `arc svg-color-${d.data.class}`)
        .attr("d", arc)

    g.selectAll(".arc")
        .data(pieData)
        .enter()
        .append("text")
        .attr("transform", (d) => `translate(${arc.centroid(d)})`)
        .attr("dy", ".35em")
        .text((d) => d.data.label ? d.data.label : '')
        .style("text-anchor", "middle")
        .style("font-size", "12px")
}

onMounted(() => {
    drawPie()
})

watch(() => props.data, () => drawPie(), { deep: true })

onBeforeMount(() => {
    if (props.singleColor)
        chartData.value = [
            {
                value: 100,
                class: 'placeholder-lighter'
            }]

    if (props.data.some(d => d.value > 0))
        chartData.value = props.data
})
</script>

<template>
    <svg ref="svg" :width="props.width" :height="props.height"></svg>
</template>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.svg-color-placeholder-lighter {
    fill: @memo-grey-lighter;
}

.svg-color-placeholder-light {
    fill: @memo-grey-light;
}

.svg-color-notLearned {
    fill: @not-learned-color;
}

.svg-color-notInWishKnowledge {
    fill: @not-in-wish-knowledge-color;
}

.svg-color-needsLearning {
    fill: @needs-learning-color;
}

.svg-color-needsConsolidation {
    fill: @needs-consolidation-color;
}

.svg-color-solid {
    fill: @solid-knowledge-color;
}
</style>
