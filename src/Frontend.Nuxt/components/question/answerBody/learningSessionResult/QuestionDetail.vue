<script lang="ts" setup>
import { handleNewLine } from '~/utils/utils'
import { AnswerState } from '~/components/page/learning/learningSessionStore'
import { SolutionType } from '../../solutionTypeEnum'

export class SessionAnswer {
    answerState: AnswerState
    answerAsHtml?: string

    constructor(answerState: AnswerState, answerAsHtml?: string) {
        this.answerState = answerState
        this.answerAsHtml = answerAsHtml
    }

    get isCorrect(): boolean {
        return this.answerState === AnswerState.Correct
    }

    get isUnanswered(): boolean {
        return this.answerState === AnswerState.Unanswered
    }

    get isSkipped(): boolean {
        return this.answerState === AnswerState.Skipped
    }

    get isFalse(): boolean {
        return this.answerState === AnswerState.False
    }
}

export class Question {
    correctAnswerHtml: string = ""
    title: string = ""
    sessionAnswers: SessionAnswer[] = []
    imgUrl: string = ""
    id: number = 0
    solutionType: SolutionType = SolutionType.Flashcard

    get isUnanswered(): boolean {
        return this.sessionAnswers.every(s => s.isUnanswered)
    }

    get hasWrongAnswer(): boolean {
        return this.sessionAnswers.some(s => s.isFalse) && this.sessionAnswers.every(s => !s.isCorrect)
    }
}

interface Props {
    questions: Question[]
}
const props = defineProps<Props>()
const collapseTrackingArray = ref<boolean[]>([])
const { t } = useI18n()

onBeforeMount(() => {
    collapseTrackingArray.value = []
    props.questions.forEach(() =>
        collapseTrackingArray.value.push(false)
    )
})

</script>

<template>
    <LayoutPanel :title="t('questionDetail.title')" :collapsable="false">
        <LayoutCollapse :size="LayoutCardSize.Large" v-for="question in props.questions" :key="question.id">
            <template #header>
                <div>
                    <font-awesome-icon icon="fa-solid fa-circle-check" v-if="question.sessionAnswers[0].isCorrect && question.sessionAnswers.length === 1" v-tooltip="t('questionDetail.tooltips.correctFirstTry')" />
                    <font-awesome-icon icon="fa-solid fa-circle-check"
                        v-else-if="!question.sessionAnswers[0].isUnanswered && question.sessionAnswers.length > 1 && question.sessionAnswers[question.sessionAnswers.length - 1].isCorrect"
                        v-tooltip="t('questionDetail.tooltips.correctLaterTry')" />
                    <font-awesome-icon icon="fa-solid fa-circle" v-else-if="question.isUnanswered" v-tooltip="t('questionDetail.tooltips.unanswered')" />
                    <font-awesome-icon icon="fa-solid fa-circle-minus" v-else-if="question.hasWrongAnswer"
                        v-tooltip="t('questionDetail.tooltips.wrong')" />

                    {{ question.title }}
                </div>
            </template>

            <template #body>
                <div class="answer-details">
                    <div class="answer-details-image-container">
                        <Image :src="question.imgUrl" />
                    </div>
                    <div class="answer-details-body">
                        <p class="correct-answer">{{ t('questionDetail.labels.correctAnswer') }}
                            <span v-html="handleNewLine(question.correctAnswerHtml)"></span>
                        </p>
                        <p class="answer-try" v-for="(sessionAnswer, sessionAnswerIndex) in question.sessionAnswers"
                            :class="[sessionAnswer.isSkipped ? 'skipped' : '', sessionAnswer.isUnanswered ? 'unanswered' : '', sessionAnswer.isFalse ? 'needs-learning' : '']">

                            {{ t('questionDetail.labels.yourTry', { number: sessionAnswerIndex + 1 }) }}

                            <template v-if="sessionAnswer.isSkipped">
                                {{ t('questionDetail.labels.skipped') }}
                            </template>
                            <template v-else-if="sessionAnswer.isUnanswered">
                                {{ t('questionDetail.labels.notSeen') }}
                            </template>

                            <template v-else>
                                <span v-if="question.solutionType === SolutionType.Flashcard">
                                    {{ t(`questionDetail.answerState.${sessionAnswer.answerState}`) }}
                                </span>
                                <span v-else v-html="sessionAnswer.answerAsHtml"></span>
                            </template>
                        </p>
                    </div>
                </div>
            </template>
        </LayoutCollapse>
    </LayoutPanel>
</template>

<style lang="less" scoped>
@import (reference) '~/assets/includes/imports.less';

@color-correctAnswer: @solid-knowledge-color;
@color-correctAfterRepetitionAnswer: @needs-consolidation-color;
@color-wrongAnswer: @needs-learning-color;
@color-notAnswered: @memo-grey-dark;


.fa-circle-check {
    color: @memo-green;
}

.fa-circle-minus {
    color: @memo-red-wrong;
}

.answer-details {
    display: flex;
    flex-direction: row;
    justify-content: flex-start;
    gap: 0 2rem;

    .answer-details-image-container {
        height: 76px;
        width: 76px;
        max-width: 76px;
        max-height: 76px;
    }

    .answer-details-body {
        flex: 1;
        display: flex;
        flex-direction: column;

        .correct-answer {
            font-weight: bold;
            margin-bottom: 0.5rem;
        }

        .answer-try {
            margin-bottom: 0.5rem;

            &.skipped {
                color: @color-notAnswered;
            }

            &.unanswered {
                color: @color-notAnswered;
            }

            &.needs-learning {
                color: @color-wrongAnswer;
            }
        }

    }
}
</style>