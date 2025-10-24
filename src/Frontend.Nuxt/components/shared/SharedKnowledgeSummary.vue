<script lang="ts" setup>
import { KnowledgeSummaryType } from '~/composables/knowledgeSummary'

interface Props {
    knowledgeSummary: KnowledgeSummary
    showActions?: boolean
    actionIcon?: string
    useTotal?: boolean
}

const props = defineProps<Props>()
const { t } = useI18n()

const knowledgeStatusItems = computed(() => {
    const items = []

    if (props.knowledgeSummary.total) {
        console.log('Adding total knowledge status items', props.knowledgeSummary.total)
        items.push(
            {
                label: t('knowledgeStatus.solid'),
                value: props.knowledgeSummary.total.solid,
                percentage: props.knowledgeSummary.total.solidPercentage,
                class: 'solid',
                type: KnowledgeSummaryType.SolidWishKnowledge
            },
            {
                label: t('knowledgeStatus.needsConsolidation'),
                value: props.knowledgeSummary.total.needsConsolidation,
                percentage: props.knowledgeSummary.total.needsConsolidationPercentage,
                class: 'needsConsolidation',
                type: KnowledgeSummaryType.NeedsConsolidationWishKnowledge
            },
            {
                label: t('knowledgeStatus.needsLearning'),
                value: props.knowledgeSummary.total.needsLearning,
                percentage: props.knowledgeSummary.total.needsLearningPercentage,
                class: 'needsLearning',
                type: KnowledgeSummaryType.NeedsLearningWishKnowledge
            },
            {
                label: t('knowledgeStatus.notLearned'),
                value: props.knowledgeSummary.total.notLearned,
                percentage: props.knowledgeSummary.total.notLearnedPercentage,
                class: 'notLearned',
                type: KnowledgeSummaryType.NotLearnedWishKnowledge
            },
            {
                label: t('knowledgeStatus.notInWishKnowledge'),
                value: props.knowledgeSummary.total.notInWishKnowledgeCount,
                percentage: props.knowledgeSummary.total.notInWishKnowledgePercentage,
                class: 'notInWishKnowledge',
                type: KnowledgeSummaryType.NotInWishKnowledge
            }
        )
    }

    return items
})

const knowledgeStatusItemsInWishKnowledge = computed(() => {
    const items = []

    // InWishKnowledge items (wishKnowledge)
    if (props.knowledgeSummary.inWishKnowledge) {
        items.push(
            {
                label: t('knowledgeStatus.solid'),
                value: props.knowledgeSummary.inWishKnowledge.solid,
                percentage: props.knowledgeSummary.inWishKnowledge.solidPercentage,
                class: 'solid',
                type: KnowledgeSummaryType.SolidWishKnowledge
            },
            {
                label: t('knowledgeStatus.needsConsolidation'),
                value: props.knowledgeSummary.inWishKnowledge.needsConsolidation,
                percentage: props.knowledgeSummary.inWishKnowledge.needsConsolidationPercentage,
                class: 'needsConsolidation',
                type: KnowledgeSummaryType.NeedsConsolidationWishKnowledge
            },
            {
                label: t('knowledgeStatus.needsLearning'),
                value: props.knowledgeSummary.inWishKnowledge.needsLearning,
                percentage: props.knowledgeSummary.inWishKnowledge.needsLearningPercentage,
                class: 'needsLearning',
                type: KnowledgeSummaryType.NeedsLearningWishKnowledge
            },
            {
                label: t('knowledgeStatus.notLearned'),
                value: props.knowledgeSummary.inWishKnowledge.notLearned,
                percentage: props.knowledgeSummary.inWishKnowledge.notLearnedPercentage,
                class: 'notLearned',
                type: KnowledgeSummaryType.NotLearnedWishKnowledge
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
        <div class="summary-details" v-if="props.useTotal">
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
                <!-- <h4>{{ t('label.inWishKnowledge') }}</h4> -->
                <div
                    v-for="(item, index) in knowledgeStatusItemsInWishKnowledge"
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
                    background-color: @solid-knowledge-color;
                }

                &.dot-needsConsolidation {
                    background-color: @needs-consolidation-color;
                }

                &.dot-needsLearning {
                    background-color: @needs-learning-color;
                }

                &.dot-notLearned {
                    background-color: @not-learned-color;
                }

                &.dot-notInWishKnowledge {
                    background-color: @not-in-wish-knowledge-color;
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
