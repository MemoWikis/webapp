<script lang="ts" setup>
import { KnowledgeSummaryType } from '~/composables/knowledgeSummary'

interface Props {
    knowledgeSummary: KnowledgeSummary
    showActions?: boolean
    actionIcon?: string
    showNotInWishknowledge?: boolean
}

const props = defineProps<Props>()
const { t } = useI18n()

const knowledgeStatusItems = computed(() => {
    const items = [
        {
            label: t('knowledgeStatus.solid'),
            value: props.knowledgeSummary.solid,
            percentage: props.knowledgeSummary.solidPercentage,
            class: 'solid',
            type: KnowledgeSummaryType.Solid
        },
        {
            label: t('knowledgeStatus.needsConsolidation'),
            value: props.knowledgeSummary.needsConsolidation,
            percentage: props.knowledgeSummary.needsConsolidationPercentage,
            class: 'needsConsolidation',
            type: KnowledgeSummaryType.NeedsConsolidation
        },
        {
            label: t('knowledgeStatus.needsLearning'),
            value: props.knowledgeSummary.needsLearning,
            percentage: props.knowledgeSummary.needsLearningPercentage,
            class: 'needsLearning',
            type: KnowledgeSummaryType.NeedsLearning
        },
        {
            label: t('knowledgeStatus.notLearned'),
            value: props.knowledgeSummary.notLearned,
            percentage: props.knowledgeSummary.notLearnedPercentage,
            class: 'notLearned',
            type: KnowledgeSummaryType.NotLearned
        }
    ]

    if (props.showNotInWishknowledge) {
        items.push({
            label: t('knowledgeStatus.notInWishknowledge'),
            value: props.knowledgeSummary.notInWishknowledge,
            percentage: props.knowledgeSummary.notInWishknowledgePercentage,
            class: 'notInWishknowledge',
            type: KnowledgeSummaryType.NotInWishknowledge
        })
    }

    return items
})

const emit = defineEmits<{
    (e: 'actionClick', type: KnowledgeSummaryType): void
}>()
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
            <div class="status-details">
                <div class="status-value">
                    <span class="value">{{ item.value }}</span>
                    <span v-if="item.percentage !== null" class="percentage">({{ item.percentage }}%)</span>
                </div>
                <div class="status-actions" v-if="props.showActions">
                    <button class="play-button" @click="$emit('actionClick', item.type)">
                        <font-awesome-icon :icon="props.actionIcon" />
                    </button>
                </div>
            </div>

        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.summary-details {
    padding: 20px 0;

    .status-item {
        display: flex;
        justify-content: space-between;
        margin-bottom: 12px;
        gap: 4rem;
        align-items: center;

        &:last-child {
            margin-bottom: 0;
        }

        .status-info {
            display: flex;
            align-items: center;
            min-width: 140px;

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
                    background-color: @memo-grey-dark;
                }

                &.dot-notInWishknowledge {
                    background-color: @memo-grey-light;
                }
            }

            .status-label {
                font-size: 14px;
                color: @memo-grey-dark;
            }
        }

        .status-details {
            display: flex;
            align-items: center;
            gap: 1rem;
            justify-content: flex-end;

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

            .status-actions {
                margin-left: 16px;

                .play-button {
                    background: white;
                    border: none;
                    cursor: pointer;
                    border-radius: 24px;
                    color: @memo-grey-dark;
                    font-size: 16px;
                    padding: 4px;
                    height: 24px;
                    width: 24px;
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    padding-left: 6px;

                    &:hover {
                        color: @memo-grey-darker;
                        background: darken(white, 5%);
                    }

                    &:focus {
                        outline: 2px solid @memo-blue;
                        outline-offset: 2px;
                    }
                }
            }
        }
    }
}
</style>
