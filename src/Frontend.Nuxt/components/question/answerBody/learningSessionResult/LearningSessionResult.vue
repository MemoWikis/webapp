<script lang="ts" setup>
import { useTabsStore, Tab } from '~/components/page/tabs/tabsStore'
import { useUserStore } from '~~/components/user/userStore'
import { Question } from '~/types/learningSession'

const userStore = useUserStore()
const emit = defineEmits(['startNewSession'])
const route = useRoute()

interface Props {
    allWishknowledgeMode?: boolean
}

const props = defineProps<Props>()

interface LearningSessionResult {
    correct: {
        percentage: number
        count: number
    }
    wrong: {
        percentage: number
        count: number
    }
    correctAfterRepetition: {
        percentage: number
        count: number
    }
    notAnswered: {
        percentage: number
        count: number
    }
    uniqueQuestionCount: number
    questions: Question[]
    pageName: string
    pageId: number
    inWishKnowledge: boolean
}
const { $logger } = useNuxtApp()

const learningSessionResult = ref<LearningSessionResult>()

onMounted(async () => {
    // Use appropriate API endpoint based on mode
    const apiEndpoint = props.allWishknowledgeMode
        ? '/apiVue/VueLearningSessionResult/GetForWishknowledge'
        : '/apiVue/VueLearningSessionResult/Get'

    learningSessionResult.value = await $api<LearningSessionResult>(apiEndpoint, {
        credentials: 'include',
        mode: 'cors',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        }
    })
})

const tabsStore = useTabsStore()

const { $urlHelper } = useNuxtApp()
const { t } = useI18n()

</script>

<template>
    <div v-if="learningSessionResult">
        <h2>
            {{ t('answerbody.learningSessionResult.heading') }}
        </h2>

        <div class="row">
            <div class="col-xs-12">
                <div class="stackedBarChartContainer">
                    <div class="stackedBarChart chartCorrectAnswer"
                        :style="`width: ${learningSessionResult.correct.percentage}%;`"
                        v-if="learningSessionResult.correct.percentage > 0">
                        {{ learningSessionResult.correct.percentage }}%
                    </div>
                    <div class="stackedBarChart chartCorrectAfterRepetitionAnswer"
                        :style="`width: ${learningSessionResult.correctAfterRepetition.percentage}%;`"
                        v-if="learningSessionResult.correctAfterRepetition.percentage > 0">
                        {{ learningSessionResult.correctAfterRepetition.percentage }}%
                    </div>
                    <div class="stackedBarChart chartWrongAnswer"
                        :style="`width: ${learningSessionResult.wrong.percentage}%;`"
                        v-if="learningSessionResult.wrong.percentage > 0">
                        {{ learningSessionResult.wrong.percentage }}%
                    </div>
                    <div class="stackedBarChart chartNotAnswered"
                        :style="`width: ${learningSessionResult.notAnswered.percentage}%;`"
                        v-if="learningSessionResult.notAnswered.percentage > 0">
                        {{ learningSessionResult.notAnswered.percentage }}%
                    </div>
                </div>

                <div class="result-legend">
                    <div v-if="learningSessionResult.correct.count > 0" class="result-info">
                        <div class="color-container" :class="`color-correct`"></div>
                        <div class="result-label">{{ t('answerbody.learningSessionResult.correctAnswers') }}
                            <b>{{ learningSessionResult.correct.count }}</b>
                        </div>
                    </div>
                    <div v-if="learningSessionResult.correctAfterRepetition.count > 0" class="result-info">
                        <div class="color-container" :class="`color-correctAfterRepetition`"></div>
                        <div class="result-label">{{ t('answerbody.learningSessionResult.correctAfterRepetition') }}
                            <b>{{ learningSessionResult.correctAfterRepetition.count }}</b>
                        </div>
                    </div>
                    <div v-if="learningSessionResult.wrong.count > 0" class="result-info">
                        <div class="color-container" :class="`color-wrong`"></div>
                        <div class="result-label">{{ t('answerbody.learningSessionResult.wrongAnswers') }}
                            <b>{{ learningSessionResult.wrong.count }}</b>
                        </div>
                    </div>
                    <div v-if="learningSessionResult.notAnswered.count > 0" class="result-info">
                        <div class="color-container" :class="`color-notAnswered`"></div>
                        <div class="result-label">{{ t('answerbody.learningSessionResult.skippedQuestions') }}
                            <b>{{ learningSessionResult.notAnswered.count }}</b>
                        </div>
                    </div>
                </div>

                <div class="buttonRow">
                    <template v-if="!userStore.isLoggedIn || !learningSessionResult.inWishKnowledge">

                        <button @click="emit('startNewSession')" class="btn btn-primary nextLearningSession memo-button"
                            style="padding-right: 10px">
                            {{ t('answerbody.learningSessionResult.continueStudying') }}
                        </button>
                        <button v-if="tabsStore.activeTab === Tab.Learning" @click="tabsStore.activeTab = Tab.Text"
                            class="memo-button btn btn-link">
                            {{ t('answerbody.learningSessionResult.toPage') }}
                        </button>
                        <NuxtLink v-else
                            :to="$urlHelper.getPageUrl(learningSessionResult.pageName, learningSessionResult.pageId)"
                            class="memo-button btn btn-link" style="padding-right: 10px">
                            <button class="memo-button btn btn-link">
                                {{ t('answerbody.learningSessionResult.toPage') }}
                            </button>
                        </NuxtLink>
                    </template>
                    <button v-else class="btn btn-primary nextLearningSession memo-button" style="padding-right: 10px" @click="emit('startNewSession')">
                        {{ t('answerbody.learningSessionResult.newSession') }}
                    </button>
                </div>

                <QuestionAnswerBodyLearningSessionResultQuestionDetail v-if="learningSessionResult.questions" :questions="learningSessionResult.questions" />

                <div v-if="learningSessionResult.questions.length > 300">
                    {{ t('answerbody.learningSessionResult.maxQuestionsShown') }}
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~/assets/includes/imports.less';

@color-correctAnswer: @solid-knowledge-color;
@color-correctAfterRepetitionAnswer: @needs-consolidation-color;
@color-wrongAnswer: @needs-learning-color;
@color-notAnswered: @not-learned-color;

#ResultMainColumn {

    #LearningTabContent & {
        width: 100%;
    }
}

#ResultSideColumn {
    #LearningTabContent & {
        display: none;
    }
}

.claimsMemoWikis {
    i {
        color: @memo-green;
        margin-bottom: 10px;
    }

    font-weight: bold;
    color: @global-text-color;
    margin-top: 30px;
    margin-bottom: 30px;
}

.tableLayout {
    display: table;
}

.tableCellLayout {
    display: table-cell;
}

.h2-additional-info {
    position: absolute;
    bottom: 0px;
    right: 0px;
    padding-left: 100px;
    font-size: 12px;
    opacity: 0.8;
}

.progress-bar {
    background-color: @memo-green;
    color: @memo-blue-lighter;
    font-size: 14px;
    font-weight: bold;
    line-height: 30px;
}

.progress {
    height: 30px;
}

#divIndicatorAverage {
    margin-top: -15px;
    color: silver;
}

#divIndicatorAverageText {
    text-align: center;
    width: 230px;
    margin-top: -10px;
    margin-bottom: 50px;
}

.stackedBarChartContainer {
    margin-top: 30px;
    margin-bottom: 50px;
    display: table;
    width: 100%;
}

.stackedBarChart {
    display: table-cell;
    /*inline-block would prevent to show div when 0-width, but vertical-centered only for table-cell. */
    height: 50px;
    text-align: center;
    vertical-align: middle;
    overflow: hidden;
    font-weight: bold;
    color: @white;
}

.chartCorrectAnswer {
    background-color: @color-correctAnswer;
    color: @white;
}

.chartCorrectAfterRepetitionAnswer {
    background-color: @color-correctAfterRepetitionAnswer;
}

.chartWrongAnswer {
    background-color: @color-wrongAnswer;
    color: @white;
}

.chartNotAnswered {
    background-color: @color-notAnswered;
}

.SummaryText {
    font-size: @font-size-bigger;
    margin: 40px 0;

    p {
        margin-bottom: 5px;
        font-weight: normal;
    }
}

.ResultDescription {
    font-size: @font-size-bigger;

    .label {
        max-width: 100%;
    }
}

.sumPctCol {
    width: auto !important;
    padding-right: 0;
}

.sumPct {
    text-align: center;
    padding: 8px;
    border-right-width: 0px;
    border-right-style: solid;
    color: white;
    width: 55px;
}

.sumPctSpan {
    width: 35px;
}

.sumPctRight {
    background-color: @color-correctAnswer;
}

.sumPctRightAfterRep {
    background-color: @color-correctAfterRepetitionAnswer;
}

.sumPctWrong {
    background-color: @color-wrongAnswer;
}

.sumPctNotAnswered {
    background-color: @color-notAnswered;
}

.sumExpl {
    text-align: left;
    padding-left: 6px;
    padding-top: 5px;
    padding-bottom: 5px;
}

.buttonRow {
    text-align: right;
    margin-top: 40px;
    margin-bottom: 40px;
    display: flex;
    flex-direction: row-reverse;

    .nextLearningTestSession {
        background: @memo-blue-link;
    }

    .nextLearningSession {
        margin-left: 0px;
    }
}

.result-legend {
    .result-info {
        display: flex;
        align-items: center;
        padding: 4px 0;

        .color-container {
            width: 24px;
            height: 24px;
            border-radius: 50%;

            &.color-correct {
                background: @memo-green;
            }

            &.color-correctAfterRepetition {
                background: @memo-yellow;
            }

            &.color-wrong {
                background: @memo-salmon;
            }

            &.color-notAnswered {
                background: @memo-grey-lighter;
            }
        }

        .result-label {
            padding-left: 8px;
        }
    }
}
</style>