<script setup lang="ts">
import { usePageStore } from '~/components/page/pageStore'
import { useLearningSessionConfigurationStore } from '../learning/learningSessionConfigurationStore'
import { KnowledgeSummaryType } from '~/composables/knowledgeSummary'
import { useLearningSessionStore } from '../learning/learningSessionStore'
import { useTabsStore, Tab } from '../tabs/tabsStore'

const pageStore = usePageStore()

const { t } = useI18n()

const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
const learningSessionStore = useLearningSessionStore()
const tabsStore = useTabsStore()


const onActionClick = async (type: KnowledgeSummaryType) => {
    learningSessionConfigurationStore.selectKnowledgeSummaryByType(type)
    await nextTick()
    learningSessionStore.startNewSession()
    tabsStore.activeTab = Tab.Learning
}

</script>

<template>
    <div class="knowledgesummary-section">

        <div class="knowledgesummary-container">
            <div v-if="pageStore.knowledgeSummary.totalCount > 0">
                <div class="knowledgesummary-content">
                    <SharedKnowledgeSummaryPie :knowledge-status-counts="pageStore.knowledgeSummary.total" :total-count="pageStore.knowledgeSummary.totalCount" />
                    <SharedKnowledgeSummary
                        :knowledge-summary="pageStore.knowledgeSummary"
                        :show-actions="true"
                        :action-icon="'fa-solid fa-play'"
                        @action-click="onActionClick"
                        :use-total="true" />
                </div>
            </div>

            <div v-else class="knowledgesummary-no-questions-answered">
                {{ t('page.analytics.noQuestionsAnswered') }}
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.knowledgesummary-section {

    .knowledgesummary-section-header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 16px 20px;

        .knowledgesummary-section-title {
            font-size: 18px;
            font-weight: 600;
            color: @memo-grey-dark;
            margin: 0;
        }
    }

    .knowledgesummary-container {

        .toggle-container {
            padding: 16px 20px 0 20px;

            .toggle-label {
                display: flex;
                align-items: center;
                cursor: pointer;
                font-size: 14px;
                color: @memo-grey-dark;

                .toggle-checkbox {
                    margin-right: 8px;
                }

                .toggle-text {
                    user-select: none;
                }
            }
        }

        .knowledgesummary-sub-label {
            font-size: 1.6rem;
            color: @memo-grey-darker;
            font-weight: 600;
        }

        .knowledgesummary-info {
            display: flex;
            flex-wrap: nowrap;
            align-items: center;
            padding: 4px 0;

            .color-container {
                height: 24px;
                width: 24px;
                margin-right: 4px;
                border-radius: 50%;

                &.color-notLearned {
                    background: @memo-grey-light;
                }

                &.color-needsLearning {
                    background: @memo-salmon;
                }

                &.color-needsConsolidation {
                    background: @memo-yellow;
                }

                &.color-solid {
                    background: @memo-green;
                }

                &.color-notInWishKnowledge {
                    background: @memo-grey-dark;
                }
            }

            .knowledgesummary-label {
                padding-left: 20px;
            }
        }

        .knowledgesummary-no-questions-answered {
            font-size: 1.4rem;
            color: @memo-grey-dark;
            padding: 20px 0;
        }

        .knowledgesummary-content {
            display: flex;
            flex-direction: row;
            border-radius: 8px;
            padding: 16px 20px;
            gap: 2rem 6rem;

            @media (max-width:576px) {
                flex-direction: column;
            }
        }
    }

    @media screen and (max-width: 991px) {
        width: 100%;
    }
}

h3 {
    margin-top: 36px;
}
</style>
