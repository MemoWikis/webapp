<script lang="ts" setup>
import { useLearningSessionConfigurationStore } from '../page/learning/learningSessionConfigurationStore'
const { locale } = useI18n()

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

    const routeNames = {
        de: 'learningPageDE',
        en: 'learningPageEN',
        fr: 'learningPageFR',
        es: 'learningPageES'
    }
    const routeName = routeNames[locale.value as keyof typeof routeNames] || 'learningPageEN'

    await navigateTo({ name: routeName })
}

const onActionClick = async (type: KnowledgeSummaryType) => {
    learningSessionConfigurationStore.selectKnowledgeSummaryByType(type)
    await navigateToLearning()
}

const learnAllWishknowledge = async () => {
    learningSessionConfigurationStore.selectAllKnowledgeSummary()
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
            <div class="summary-details">
                <div class="status-item">
                    <div class="status-info">
                        <span class="status-dot dot-all"></span>
                        <span class="status-label">{{ $t('missionControl.learnAll') }}</span>
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

    .summary-details {
        border-top: 1px solid @memo-grey-light;

        .status-dot.dot-all {
            background: linear-gradient(45deg, @solid-knowledge-color 25%, @needs-consolidation-color 25%, @needs-consolidation-color 50%, @needs-learning-color 50%, @needs-learning-color 75%, @not-learned-color 75%);
        }

        .status-label {
            font-weight: 600;
        }
    }
}
</style>