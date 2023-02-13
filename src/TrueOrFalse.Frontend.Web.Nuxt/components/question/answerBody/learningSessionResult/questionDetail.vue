<script lang="ts" setup>
import { handleNewLine } from '~~/components/shared/utils'
import { AnswerState } from '~~/components/topic/learning/learningSessionStore'

interface Step {
    answerState: AnswerState
    answerAsHtml?: string
}

interface Props {
    questions: {
        correctAnswerHtml: string
        title: string
        steps: Step[]
        imgUrl: string
        id: number
    }[]
}
const props = defineProps<Props>()
const collapseTrackingArray = ref<boolean[]>([])

onBeforeMount(() => {
    collapseTrackingArray.value = []
    props.questions.forEach(() =>
        collapseTrackingArray.value.push(false)
    )
})

</script>

<template>
    <div class="row" v-for="(question, index) in props.questions" :key="question.id">
        <div class="col-xs-12">
            <div class="QuestionLearned AnsweredRight">
                <div @click="collapseTrackingArray[index] = !collapseTrackingArray[index]" class="detail-title">

                    <font-awesome-icon icon="fa-solid fa-circle-check"
                        v-if="question.steps[0].answerState == AnswerState.Correct && question.steps.length == 1"
                        v-tooltip="'Beim 1. Versuch richtig beantwortet'" />

                    <font-awesome-icon icon="fa-solid fa-circle-check"
                        v-else-if="question.steps[0].answerState != AnswerState.Unanswered && question.steps.length > 1 && question.steps[question.steps.length - 1].answerState == AnswerState.Correct"
                        v-tooltip="'Beim 2. oder 3. Versuch richtig beantwortet'" />

                    <font-awesome-icon icon="fa-solid fa-circle"
                        v-else-if="question.steps.every(s => s.answerState == AnswerState.Unanswered)"
                        v-tooltip="'Nicht beantwortet'" />

                    <font-awesome-icon icon="fa-solid fa-circle-minus"
                        v-else-if="question.steps.some(s => s.answerState == AnswerState.False) && question.steps.every(s => s.answerState != AnswerState.Correct)"
                        v-tooltip="'Falsch beantwortet'" />

                    {{ question.title }}

                </div>

                <br />

                <Transition name="fade">
                    <div class="answerDetails" v-show="collapseTrackingArray[index]">
                        <div class="row">
                            <div class="col-xs-3 col-sm-2 answerDetailImage">
                                <div class="ImageContainer ShortLicenseLinkText">
                                    <img :src="question.imgUrl" />
                                </div>
                            </div>
                            <div class="col-xs-9 col-sm-10">
                                <p class="rightAnswer">Richtige Antwort:
                                <div v-html="handleNewLine(question.correctAnswerHtml)"></div>

                                <br /></p>
                                <p class="answerTry" v-for="(step, index) in question.steps">
                                    Dein {{ index + 1 }}. Versuch:
                                    <template v-if="step.answerState == AnswerState.Skipped">
                                        (übersprungen)
                                    </template>
                                    <template v-else-if="step.answerState == AnswerState.Unanswered">
                                        (noch nicht gesehen)
                                    </template>
                                    <template v-else>
                                        <span v-html="step.answerAsHtml"></span>
                                    </template>
                                </p>

                            </div>

                        </div>
                    </div>
                </Transition>
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


.fa-circle-check {
    color: @memo-green;
}

.fa-circle-minus {
    color: @memo-red-wrong;
}

.detail-title {
    cursor: pointer;
    user-select: none;
}

.QuestionLearned {
    margin-bottom: 7px;
    border: 1px solid lightgray;
    padding: 5px 10px;
    background-color: @white;
    transition: all 0.2s ease-in;

    /*i.fa-check {
        color: @color-correctAnswer;
     }

     i.fa-dot-circle-o {
         color: @color-correctAfterRepetitionAnswer;
     }
     i.fa-minus-circle {
        color: @color-wrongAnswer;
     }

     i.fa-circle-o {
        color: @color-notAnswered;
     }*/
}

.AnswerResultIcon {
    font-size: 14px;
}

.QuestionLearned.AnsweredRight {
    .AnswerResultIcon {
        color: @color-correctAnswer;
    }
}

.QuestionLearned.AnsweredRightAfterRepetition {
    .AnswerResultIcon {
        color: @color-correctAfterRepetitionAnswer;
    }
}

.QuestionLearned.AnsweredWrong {
    .AnswerResultIcon {
        color: @color-wrongAnswer;
    }
}

.QuestionLearned.Unanswered {
    .AnswerResultIcon {
        color: @color-notAnswered;
    }
}

.answerDetails {
    padding-left: 30px;
    margin-bottom: 2px;

    p {
        margin: 2px 0;
    }

    .rightAnswer {
        font-weight: bold;
    }

    .answerTry {}

    .averageCorrectness {
        padding-top: 3px;
    }

    .answerLinkToQ {
        font-size: 11px;
        margin-top: +9px;
        color: @global-text-color-grey;
    }
}

.answerDetailImage {
    padding: 4px 4px 4px 0;
    max-width: 80px;
}

.boxInfo {
    margin-top: 30px;
    border-width: 1px;
    border-style: solid;
    border-radius: 4px;
    border-color: #eeeeee;
}

.boxInfoHeader {
    background-color: #eeeeee;
    padding: 8px;
    font-size: 14px;
    font-weight: bold;
}

.boxInfoContent {
    padding: 8px;
    font-size: 14px;

    h3 {
        font-family: 'Open Sans';
        margin: 2px 0 10px;
        font-size: 14px;
        font-weight: bold;
    }

    p {
        margin: 3px 0;
    }
}
</style>