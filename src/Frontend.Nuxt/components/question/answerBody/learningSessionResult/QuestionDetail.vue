<script lang="ts" setup>
import { handleNewLine } from '~/utils/utils'
import { AnswerState } from '~/components/page/learning/learningSessionStore'
import { SolutionType } from '../../solutionTypeEnum'

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
        solutionType: SolutionType
    }[]
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
        <LayoutCollapse :size="LayoutContentSize.Large" v-for="question in props.questions" :key="question.id">
            <template #header>
                <div>
                    <font-awesome-icon icon="fa-solid fa-circle-check" v-if="question.steps[0].answerState === AnswerState.Correct && question.steps.length === 1" v-tooltip="t('questionDetail.tooltips.correctFirstTry')" />
                    <font-awesome-icon icon="fa-solid fa-circle-check"
                        v-else-if="question.steps[0].answerState != AnswerState.Unanswered && question.steps.length > 1 && question.steps[question.steps.length - 1].answerState === AnswerState.Correct"
                        v-tooltip="t('questionDetail.tooltips.correctLaterTry')" />
                    <font-awesome-icon icon="fa-solid fa-circle" v-else-if="question.steps.every(s => s.answerState === AnswerState.Unanswered)" v-tooltip="t('questionDetail.tooltips.unanswered')" />
                    <font-awesome-icon icon="fa-solid fa-circle-minus" v-else-if="question.steps.some(s => s.answerState === AnswerState.False) && question.steps.every(s => s.answerState != AnswerState.Correct)"
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
                        <p class="answer-try" v-for="(step, stepIndex) in question.steps"
                            :class="[step.answerState === AnswerState.Skipped ? 'skipped' : '', step.answerState === AnswerState.Unanswered ? 'unanswered' : '', step.answerState === AnswerState.False ? 'needs-learning' : '']">

                            {{ t('questionDetail.labels.yourTry', { number: stepIndex + 1 }) }}

                            <template v-if="step.answerState === AnswerState.Skipped">
                                {{ t('questionDetail.labels.skipped') }}
                            </template>
                            <template v-else-if="step.answerState === AnswerState.Unanswered">
                                {{ t('questionDetail.labels.notSeen') }}
                            </template>

                            <template v-else>
                                <span v-if="question.solutionType === SolutionType.Flashcard">
                                    {{ t(`questionDetail.answerState.${step.answerState}`) }}
                                </span>
                                <span v-else v-html="step.answerAsHtml"></span>
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