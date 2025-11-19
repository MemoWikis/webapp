<script lang="ts" setup>
import { useLearningSessionConfigurationStore } from '../page/learning/learningSessionConfigurationStore'

const learningSessionConfigurationStore = useLearningSessionConfigurationStore()

interface Props {
    knowledgeStatus: KnowledgeSummary
}
const sumWishKnowledge = computed(() => {
    const inWishKnowledge = props.knowledgeStatus.inWishKnowledge
    return inWishKnowledge.solid + inWishKnowledge.needsConsolidation + inWishKnowledge.needsLearning + inWishKnowledge.notLearned
})

const props = defineProps<Props>()

const navigateToLearning = async () => {
    learningSessionConfigurationStore.selectedQuestionCount = sumWishKnowledge.value

    learningSessionConfigurationStore.saveSessionConfig()
    await nextTick()
    navigateTo({ name: 'learningPageEN' })
}

const onActionClick = async (type: KnowledgeSummaryType) => {
    learningSessionConfigurationStore.selectKnowledgeSummaryByType(type)
    await navigateToLearning()
}

const learnAllWishknowledge = async () => {
    // Select all knowledge summary types
    learningSessionConfigurationStore.selectKnowledgeSummaryByType(KnowledgeSummaryType.SolidWishKnowledge)
    learningSessionConfigurationStore.selectKnowledgeSummaryByType(KnowledgeSummaryType.NeedsConsolidationWishKnowledge)
    learningSessionConfigurationStore.selectKnowledgeSummaryByType(KnowledgeSummaryType.NeedsLearningWishKnowledge)
    learningSessionConfigurationStore.selectKnowledgeSummaryByType(KnowledgeSummaryType.NotLearnedWishKnowledge)
    await navigateToLearning()
}

</script>

<template>
    <div class="knowledge-summary">
        <SharedKnowledgeSummaryPie :knowledge-status-counts="props.knowledgeStatus.inWishKnowledge" :total-count="sumWishKnowledge" />
        <SharedKnowledgeSummary
            :knowledge-summary="props.knowledgeStatus"
            :use-total="false"
            :show-actions="true"
            :action-icon="'fa-solid fa-play'"
            @action-click="onActionClick">

            <!-- Additional action for selecting all knowledge types -->
            <div class="all-action-container">
                <div class="status-item">
                    <div class="status-info">
                        <span class="status-dot dot-all"></span>
                        <span class="status-label">{{ $t('knowledgeStatus.all') }}</span>
                    </div>
                    <div class="status-details">
                        <div class="status-value">
                            <span class="value">{{ sumWishKnowledge }}</span>
                        </div>
                        <div class="status-actions">
                            <button class="play-button" @click="learnAllWishknowledge">
                                <font-awesome-icon icon="fa-solid fa-play" />
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </SharedKnowledgeSummary>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.knowledge-summary {
    display: flex;
    flex-direction: column;
    border-radius: 8px;
    padding: 16px 20px;
    gap: 2rem 4rem;

    @media (max-width:576px) {
        min-width: unset;
    }

    @media (min-width: @screen-sm) {
        flex-direction: row;
        align-items: center;
        flex-wrap: wrap;
        justify-content: center;
    }
}
</style>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.sidesheet-open {
    .knowledge-summary {
        @media (max-width:976px) {
            flex-direction: row;
            align-items: center;
            min-width: unset;
            flex-wrap: wrap;
            justify-content: center;
        }
    }

    .all-action-container {
        border-top: 1px solid @memo-grey-light;
        padding-top: 12px;
        margin-top: 12px;

        .status-item {
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 4rem;

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

                    &.dot-all {
                        background: linear-gradient(45deg, @solid-knowledge-color 25%, @needs-consolidation-color 25%, @needs-consolidation-color 50%, @needs-learning-color 50%, @needs-learning-color 75%, @not-learned-color 75%);
                    }
                }

                .status-label {
                    font-size: 14px;
                    color: @memo-grey-dark;
                    font-weight: 600;
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
}
</style>