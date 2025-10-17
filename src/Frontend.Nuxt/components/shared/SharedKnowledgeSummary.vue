<script lang="ts" setup>
import { KnowledgeSummaryType } from '~/composables/knowledgeSummary'

interface Props {
    knowledgeSummary: KnowledgeSummary
    showActions?: boolean
    actionIcon?: string
    showNotInWishknowledge?: boolean
    showSum?: boolean
}

const props = defineProps<Props>()
const { t } = useI18n()

const knowledgeStatusItems = computed(() => {
    const items = []

    // Use backend's calculated totals
    if (props.knowledgeSummary.totalDetailed) {
        items.push(
            {
                label: t('knowledgeStatus.solid'),
                value: props.knowledgeSummary.totalDetailed.solid,
                percentage: props.knowledgeSummary.totalDetailed.solidPercentage,
                class: 'solid',
                type: KnowledgeSummaryType.SolidWuwi // Using Wuwi type as default for totals
            },
            {
                label: t('knowledgeStatus.needsConsolidation'),
                value: props.knowledgeSummary.totalDetailed.needsConsolidation,
                percentage: props.knowledgeSummary.totalDetailed.needsConsolidationPercentage,
                class: 'needsConsolidation',
                type: KnowledgeSummaryType.NeedsConsolidationWuwi
            },
            {
                label: t('knowledgeStatus.needsLearning'),
                value: props.knowledgeSummary.totalDetailed.needsLearning,
                percentage: props.knowledgeSummary.totalDetailed.needsLearningPercentage,
                class: 'needsLearning',
                type: KnowledgeSummaryType.NeedsLearningWuwi
            },
            {
                label: t('knowledgeStatus.notLearned'),
                value: props.knowledgeSummary.totalDetailed.notLearned,
                percentage: props.knowledgeSummary.totalDetailed.notLearnedPercentage,
                class: 'notLearned',
                type: KnowledgeSummaryType.NotLearnedWuwi
            }
        )
    }

    return items
})

const knowledgeStatusItemsInWuwi = computed(() => {
    const items = []

    // InWishknowledge items (wuwi)
    if (props.knowledgeSummary.inWishknowledge) {
        items.push(
            {
                label: t('knowledgeStatus.solid'),
                value: props.knowledgeSummary.inWishknowledge.solid,
                percentage: props.showNotInWishknowledge ? props.knowledgeSummary.inWishknowledge.solidPercentageOfTotal : props.knowledgeSummary.inWishknowledge.solidPercentage,
                class: 'solid',
                type: KnowledgeSummaryType.SolidWuwi
            },
            {
                label: t('knowledgeStatus.needsConsolidation'),
                value: props.knowledgeSummary.inWishknowledge.needsConsolidation,
                percentage: props.showNotInWishknowledge ? props.knowledgeSummary.inWishknowledge.needsConsolidationPercentageOfTotal : props.knowledgeSummary.inWishknowledge.needsConsolidationPercentage,
                class: 'needsConsolidation',
                type: KnowledgeSummaryType.NeedsConsolidationWuwi
            },
            {
                label: t('knowledgeStatus.needsLearning'),
                value: props.knowledgeSummary.inWishknowledge.needsLearning,
                percentage: props.showNotInWishknowledge ? props.knowledgeSummary.inWishknowledge.needsLearningPercentageOfTotal : props.knowledgeSummary.inWishknowledge.needsLearningPercentage,
                class: 'needsLearning',
                type: KnowledgeSummaryType.NeedsLearningWuwi
            },
            {
                label: t('knowledgeStatus.notLearned'),
                value: props.knowledgeSummary.inWishknowledge.notLearned,
                percentage: props.showNotInWishknowledge ? props.knowledgeSummary.inWishknowledge.notLearnedPercentageOfTotal : props.knowledgeSummary.inWishknowledge.notLearnedPercentage,
                class: 'notLearned',
                type: KnowledgeSummaryType.NotLearnedWuwi
            }
        )
    }
    return items
})

const knowledgeStatusItemsNotInWuwi = computed(() => {
    const items = []
    if (props.showNotInWishknowledge && props.knowledgeSummary.notInWishknowledge) {
        items.push(
            {
                label: t('knowledgeStatus.solid'),
                value: props.knowledgeSummary.notInWishknowledge.solid,
                percentage: props.knowledgeSummary.notInWishknowledge.solidPercentageOfTotal,
                class: 'solid notInWuwi',
                type: KnowledgeSummaryType.SolidNotInWuwi
            },
            {
                label: t('knowledgeStatus.needsConsolidation'),
                value: props.knowledgeSummary.notInWishknowledge.needsConsolidation,
                percentage: props.knowledgeSummary.notInWishknowledge.needsConsolidationPercentageOfTotal,
                class: 'needsConsolidation notInWuwi',
                type: KnowledgeSummaryType.NeedsConsolidationNotInWuwi
            },
            {
                label: t('knowledgeStatus.needsLearning'),
                value: props.knowledgeSummary.notInWishknowledge.needsLearning,
                percentage: props.knowledgeSummary.notInWishknowledge.needsLearningPercentageOfTotal,
                class: 'needsLearning notInWuwi',
                type: KnowledgeSummaryType.NeedsLearningNotInWuwi
            },
            {
                label: t('knowledgeStatus.notLearned'),
                value: props.knowledgeSummary.notInWishknowledge.notLearned,
                percentage: props.knowledgeSummary.notInWishknowledge.notLearnedPercentageOfTotal,
                class: 'notLearned notInWuwi',
                type: KnowledgeSummaryType.NotLearnedNotInWuwi
            }
        )
    }

    return items
})

const emit = defineEmits<{
    (e: 'actionClick', type: KnowledgeSummaryType): void
}>()
</script>

<template>

    <div class="summary-details-container">
        <div class="summary-details" v-if="props.showSum">
            <h4>{{ t('label.Sum') }}</h4>
            <div
                v-for="(item, index) in knowledgeStatusItems"
                :key="index"
                class="status-item">
                <div class="status-info">
                    <span class="status-dot" :class="'dot-' + item.class.replace(' ', ' dot-')"></span>
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
        <template v-else>
            <div class="summary-details">
                <h4>{{ t('label.inWishknowledge') }}</h4>
                <div
                    v-for="(item, index) in knowledgeStatusItemsInWuwi"
                    :key="index"
                    class="status-item">
                    <div class="status-info">
                        <span class="status-dot" :class="'dot-' + item.class.replace(' ', ' dot-')"></span>
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
            <div class="summary-details" v-if="props.showNotInWishknowledge">
                <h4>{{ t('label.notInWishknowledge') }}</h4>
                <div
                    v-for="(item, index) in knowledgeStatusItemsNotInWuwi"
                    :key="index"
                    class="status-item">
                    <div class="status-info">
                        <span class="status-dot" :class="'dot-' + item.class.replace(' ', ' dot-')"></span>
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
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.summary-details-container {
    display: flex;
    justify-content: space-between;
    gap: 1rem 4rem;
    flex-wrap: wrap;

    h4 {
        margin-top: 0;
    }
}

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

                // Styling for not-in-wuwi items (faded/striped)
                &.dot-solid.dot-notInWuwi {
                    background-color: fade(@memo-green, 50%);
                }

                &.dot-needsConsolidation.dot-notInWuwi {
                    background-color: fade(@memo-yellow, 50%);
                }

                &.dot-needsLearning.dot-notInWuwi {
                    background-color: fade(@memo-salmon, 50%);
                }

                &.dot-notLearned.dot-notInWuwi {
                    background-color: fade(@memo-grey-dark, 50%);
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
