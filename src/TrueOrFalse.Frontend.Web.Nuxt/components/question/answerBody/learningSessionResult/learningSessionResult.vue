<script lang="ts" setup>
import { useLearningSessionConfigurationStore } from '~~/components/topic/learning/learningSessionConfigurationStore';
import { AnswerState } from '~~/components/topic/learning/learningSessionStore'
import { useUserStore } from '~~/components/user/userStore'

const userStore = useUserStore()
const learningSessionResult = ref<LearningSessionResult | null>(null)
const emit = defineEmits(['startNewSession'])

interface Step {
    answerState: AnswerState
    answerAsHtml?: string
}

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
    questions: {
        correctAnswerHtml: string
        title: string
        steps: Step[]
        imgUrl: string
        id: number
    }[]
    encodedTopicName: string
    topicId: number
    inWuwi: boolean
}

onBeforeMount(async () => {
    learningSessionResult.value = await $fetch<LearningSessionResult>('/apiVue/VueLearningSessionResult/Get', {
        credentials: 'include',
        mode: 'cors'
    })
})
</script>

<template>

    <div v-if="learningSessionResult">
        <h2 style="margin-bottom: 15px; margin-top: 0px;">
            Ergebnis
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

                <div class="buttonRow">
                    <template v-if="!userStore.isLoggedIn || !learningSessionResult.inWuwi">
                        <NuxtLink :to="`/${learningSessionResult.encodedTopicName}/${learningSessionResult.topicId}`"
                            class="btn btn-link " style="padding-right: 10px">Zum Thema</NuxtLink>
                        <button @click="emit('startNewSession')" class="btn btn-primary nextLearningSession memo-button"
                            style="padding-right: 10px">
                            Weiterlernen
                        </button>
                    </template>
                    <button v-else class="btn btn-primary nextLearningSession memo-button" style="padding-right: 10px"
                        @click="emit('startNewSession')">
                        Neue Lernsitzung
                    </button>
                </div>

                <QuestionDetail v-if="learningSessionResult.questions" :questions="learningSessionResult.questions" />

                <div v-if="learningSessionResult.questions.length > 300">Es werden nicht mehr als 300 Fragen in der
                    Auswertung
                    angezeigt</div>
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

.claimsMemucho {
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

    .nextLearningTestSession {
        background: @memo-blue-link;
    }
}
</style>