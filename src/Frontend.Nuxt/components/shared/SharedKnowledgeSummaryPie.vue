<script lang="ts" setup>
import { KnowledgeSummary } from '~/composables/knowledgeSummary'

interface Props {
    knowledgeSummary: KnowledgeSummary
}

const props = defineProps<Props>()
const { t } = useI18n()

const pieData = computed(() => {
    return convertKnowledgeSummaryToChartData(props.knowledgeSummary)
})
</script>

<template>
    <div class="summary-visualization">
        <div class="pie-chart-wrapper">
            <ChartPie :data="pieData" :width="150" :height="150" :single-color="true" />
        </div>
        <div class="total-questions">
            <span class="count">{{ props.knowledgeSummary.totalCount }}</span>
            <span class="total-questions-label">{{ t('label.questionCountAsText', props.knowledgeSummary.totalCount) }}</span>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.summary-visualization {
    position: relative;
    margin: 0 auto 20px;
    display: flex;
    justify-content: center;
    align-items: center;

    @media (min-width: @screen-sm) {
        margin: 0 10px 0 10px;
        flex: 0 0 150px;
    }

    .pie-chart-wrapper {
        width: 150px;
        height: 150px;
    }

    .total-questions {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        text-align: center;
        max-width: 100px;
        height: 100px;
        display: flex;
        justify-content: center;
        align-items: center;
        flex-direction: column;
        background: white;
        border-radius: 50px;
        width: 100px;

        .count {
            display: block;
            font-size: 24px;
            font-weight: 600;
            color: @memo-grey-darker;
            line-height: 1.2;
        }

        .total-questions-label {
            display: block;
            font-size: 12px;
            color: @memo-grey-darker;
        }
    }
}
</style>
