<script lang="ts" setup>

interface Props {
    knowledgeSummary: KnowledgeSummary
}

const props = defineProps<Props>()
const { t } = useI18n()

const knowledgeStatusItems = computed(() => [
    {
        label: t('knowledgeStatus.solid'),
        value: props.knowledgeSummary.solid,
        percentage: props.knowledgeSummary.solidPercentage,
        class: 'solid'
    },
    {
        label: t('knowledgeStatus.needsConsolidation'),
        value: props.knowledgeSummary.needsConsolidation,
        percentage: props.knowledgeSummary.needsConsolidationPercentage,
        class: 'needsConsolidation'
    },
    {
        label: t('knowledgeStatus.needsLearning'),
        value: props.knowledgeSummary.needsLearning,
        percentage: props.knowledgeSummary.needsLearningPercentage,
        class: 'needsLearning'
    },
    {
        label: t('knowledgeStatus.notLearned'),
        value: props.knowledgeSummary.notLearned,
        percentage: props.knowledgeSummary.notLearnedPercentage,
        class: 'notLearned'
    }
])
</script>

<template>
    <div class="summary-details">
        <div
            v-for="(item, index) in knowledgeStatusItems"
            :key="index"
            class="status-item">
            <div class="status-info">
                <span class="status-dot" :class="`dot-${item.class}`"></span>
                <span class="status-label">{{ item.label }}</span>
            </div>
            <div class="status-value">
                <span class="value">{{ item.value }}</span>
                <span v-if="item.percentage !== null" class="percentage">({{ item.percentage }}%)</span>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.summary-details {
    flex-grow: 1;
    padding: 20px 0;

    .status-item {
        display: flex;
        justify-content: space-between;
        margin-bottom: 12px;

        &:last-child {
            margin-bottom: 0;
        }

        .status-info {
            display: flex;
            align-items: center;

            .status-dot {
                width: 12px;
                height: 12px;
                border-radius: 50%;
                margin-right: 8px;
                min-width: 12px;

                &.dot-solid {
                    background-color: @memo-green;
                }

                &.dot-needsConsolidation {
                    background-color: @memo-yellow;
                }

                &.dot-needsLearning {
                    background-color: @memo-salmon;
                }

                &.dot-notLearned {
                    background-color: @memo-grey-light;
                }
            }

            .status-label {
                font-size: 14px;
                color: @memo-grey-dark;
            }
        }

        .status-value {
            font-size: 14px;

            .value {
                font-weight: 600;
                color: @memo-grey-darker;
            }

            .percentage {
                margin-left: 4px;
                color: @memo-grey-dark;
            }
        }
    }
}
</style>
