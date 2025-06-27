<script lang="ts" setup>
import { SolutionType } from '~/components/question/solutionTypeEnum'

interface Props {
    answerBodyModel: any
    answerIsCorrect: boolean
    answerIsWrong: boolean
    answerIsCorrectPopUp: boolean
    answerIsWrongPopUp: boolean
    wellDoneMsg: string
    wrongAnswerMsg: string
    showAnswer: boolean
    showWrongAnswers: boolean
    answersSoFar: string[]
    solutionData: any
}

const props = defineProps<Props>()
const { t } = useI18n()
</script>

<template>
    <div id="AnswerFeedbackAndSolutionDetails">
        <!-- Flashcard feedback popups -->
        <template v-if="answerBodyModel.solutionType === SolutionType.Flashcard">
            <Transition name="fade">
                <div class="answerFeedback answerFeedbackCorrect" v-show="answerIsCorrectPopUp">
                    <font-awesome-icon icon="fa-solid fa-circle-check" />
                    &nbsp;{{ t('answerbody.feedbackCorrectPopUp') }}
                </div>
            </Transition>
            <Transition name="fade">
                <div class="answerFeedback answerFeedbackWrong" v-show="answerIsWrongPopUp">
                    <font-awesome-icon icon="fa-solid fa-circle-minus" />
                    &nbsp;{{ t('answerbody.feedbackWrongPopUp') }}
                </div>
            </Transition>
        </template>

        <!-- Non-flashcard feedback -->
        <div v-if="answerBodyModel.solutionType != SolutionType.Flashcard" id="AnswerFeedback">
            <div id="divAnsweredCorrect" v-if="answerIsCorrect">
                <b class="correct-answer-label">
                    {{ t('answerbody.correctAnswerLabel') }}
                </b>
                <SharedRawHtml v-if="wellDoneMsg.length > 0" :html="wellDoneMsg" />
            </div>

            <div id="Solution" v-if="showAnswer">
                <div class="solution-label">
                    {{ t('answerbody.rightAnswerHeading') }}
                </div>
                <SharedRawHtml class="Content body-m" v-if="solutionData" :html="solutionData.answerAsHTML" />
            </div>

            <div id="divWrongAnswer" v-if="answerIsWrong">
                <div id="spnWrongAnswer">
                    <b class="wrong-answer-label">
                        {{ t('answerbody.wrongAnswerLabel') }}
                    </b>
                </div>
                <SharedRawHtml v-if="wrongAnswerMsg.length > 0" :html="wrongAnswerMsg" />
                <br />

                <div id="divWrongAnswers" v-if="showWrongAnswers">
                    <span class="WrongAnswersHeading">
                        {{ answersSoFar.length === 1
                            ? t('answerbody.yourAnswersHeadingSingular')
                            : t('answerbody.yourAnswersHeadingPlural')
                        }}
                    </span>
                    <ul id="ulAnswerHistory">
                        <li v-for="answer in answersSoFar">
                            <SharedRawHtml :html="answer" />
                        </li>
                    </ul>
                </div>
            </div>
        </div>

        <div id="SolutionDetails" v-if="solutionData
            && (solutionData.answerDescription?.trim().length > 0
                || solutionData.answerDescriptionHtml?.trim().length > 0)
            && showAnswer">
            <div id="Description">
                <div class="solution-label">
                    {{ t('answerbody.answerAdditionsHeading') }}
                </div>

                <SharedRawHtml v-if="solutionData.answerDescriptionHtml.trim().length > 0" class="Content body-m" :html="solutionData.answerDescriptionHtml" />
                <SharedRawHtml v-else class="Content body-m" :html="solutionData.answerDescription" />
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#ulAnswerHistory {
    padding-top: 5px;
}

.solution-label {
    font-weight: bold;
    padding-right: 5px;
}

.correct-answer-label {
    color: @memo-green-correct;
}

.wrong-answer-label {
    color: @memo-red-wrong;
}

.fade-enter-active {
    transition: opacity 0.5s ease;
}

.fade-leave-active {
    transition: opacity 0;
}

.fade-enter-from,
.fade-leave-to {
    opacity: 0;
}
</style>
