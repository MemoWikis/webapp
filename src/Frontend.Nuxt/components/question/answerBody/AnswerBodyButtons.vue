<script lang="ts" setup>
import { SolutionType } from '~/components/question/solutionTypeEnum'
import { AnswerState } from '~/components/page/learning/learningSessionStore'

interface Props {
    answerBodyModel: any
    amountOfTries: number
    showAnswer: boolean
    showAnswerButtons: boolean
    flashCardAnswered: boolean
    answerIsWrong: boolean
    allMultipleChoiceCombinationTried: boolean
    learningSessionStore: any
}

interface Emits {
    (e: 'answer'): void
    (e: 'flip'): void
    (e: 'answerFlashcard', isCorrect: boolean): void
    (e: 'loadSolution', answered?: boolean): void
    (e: 'markAsCorrect'): void
    (e: 'loadResult'): void
}

const props = defineProps<Props>()
const emit = defineEmits<Emits>()
const { t } = useI18n()
</script>

<template>
    <div id="Buttons">
        <template v-if="answerBodyModel.solutionType === SolutionType.Flashcard && !flashCardAnswered">
            <button
                class="btn btn-warning memo-button"
                @click="emit('flip')"
                v-if="amountOfTries === 0">
                {{ t('answerbody.flashcardFlip') }}
            </button>
            <div id="buttons-answer" class="ButtonGroup flashCardAnswerButtons" v-else>
                <button
                    id="btnRightAnswer"
                    class="btn btn-correct memo-button"
                    @click="emit('answerFlashcard', true)">
                    {{ t('answerbody.flashcardKnew') }}
                </button>
                <button
                    id="btnWrongAnswer"
                    class="btn btn-wrong memo-button"
                    @click="emit('answerFlashcard', false)">
                    {{ t('answerbody.flashcardDidNotKnow') }}
                </button>
                <button
                    v-if="!learningSessionStore.isInTestMode && learningSessionStore.answerHelp"
                    id="flashCard-dontCountAnswer"
                    class="selectorShowSolution SecAction btn btn-link memo-button"
                    @click="learningSessionStore.skipStep()">
                    {{ t('answerbody.flashcardDontCount') }}
                </button>
            </div>
        </template>

        <template v-else-if="showAnswerButtons && answerBodyModel.solutionType != SolutionType.Flashcard">
            <div id="buttons-first-try" class="ButtonGroup">
                <button class="btn btn-primary memo-button" @click="emit('answer')">
                    {{ t('answerbody.answer') }}
                </button>
                <button
                    v-if="!learningSessionStore.isInTestMode && learningSessionStore.answerHelp && !showAnswer"
                    class="selectorShowSolution SecAction btn btn-link memo-button"
                    @click="emit('loadSolution', false)">
                    <font-awesome-icon icon="fa-solid fa-lightbulb" />
                    {{ t('answerbody.showSolution') }}
                </button>
            </div>
        </template>

        <div v-if="learningSessionStore.isLearningSession
            && !learningSessionStore.isInTestMode
            && (amountOfTries === 0 && !showAnswer && learningSessionStore.currentStep?.state != AnswerState.Skipped)">
            <button class="SecAction btn btn-link memo-button" @click="learningSessionStore.skipStep()">
                <font-awesome-icon icon="fa-solid fa-forward" />
                {{ t('answerbody.skipQuestion') }}
            </button>
        </div>

        <div id="buttons-next-question" class="ButtonGroup" v-if="(amountOfTries > 0 || showAnswer)
            && !learningSessionStore.currentStep?.isLastStep
            && !showAnswerButtons">
            <button v-if="!learningSessionStore.currentStep?.isLastStep" @click="learningSessionStore.loadNextQuestionInSession()" id="btnNext" class="btn btn-primary memo-button" rel="nofollow">
                {{ t('answerbody.nextQuestion') }}
            </button>

            <button v-if="answerBodyModel.solutionType === SolutionType.Text
                && !learningSessionStore.isInTestMode
                && learningSessionStore.answerHelp
                && answerIsWrong" href="#" id="aCountAsCorrect" class="SecAction btn btn-link show-tooltip memo-button" :title="t('answerBody.tooltip.markAsCorrect')" rel="nofollow" @click="emit('markAsCorrect')">
                {{ t('answerbody.markAsKnown') }}
            </button>
        </div>

        <Transition name="fade">
            <div v-if="learningSessionStore.currentStep?.isLastStep
                && (amountOfTries > 0
                    || learningSessionStore.currentStep?.state === AnswerState.Skipped
                    || learningSessionStore.currentStep?.state === AnswerState.ShowedSolutionOnly)">
                <button @click="emit('loadResult')" class="btn btn-primary memo-button" rel="nofollow">
                    {{ t('answerbody.toResult') }}
                </button>
            </div>
        </Transition>

        <div v-if="answerBodyModel.solutionType != SolutionType.Flashcard" id="buttons-answer-again" class="ButtonGroup">
            <button v-if="!allMultipleChoiceCombinationTried
                && !learningSessionStore.isInTestMode
                && answerIsWrong
                && !showAnswer
                && !showAnswerButtons" id="btnCheckAgain" class="btn btn-warning memo-button" rel="nofollow" @click="emit('answer')">
                {{ t('answerbody.answerAgain') }}
            </button>
            <button v-if="!learningSessionStore.isInTestMode
                && learningSessionStore.answerHelp
                && !showAnswer
                && !showAnswerButtons" class="selectorShowSolution SecAction btn btn-link memo-button" @click="emit('loadSolution', true)">
                <font-awesome-icon icon="fa-solid fa-lightbulb" />
                {{ t('answerbody.showSolution') }}
            </button>
        </div>
        <div style="clear: both"></div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.ButtonGroup {
    display: flex;
    justify-content: flex-start;
    flex-wrap: wrap;
}

.btn-primary {
    margin-right: 22px;
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

.btn-correct {
    background-color: @solid-knowledge-color;
}

.btn-wrong {
    background-color: @needs-learning-color;
}
</style>
