<script lang="ts" setup>
import { useLearningSessionStore } from '~/components/page/learning/learningSessionStore'
import { useUserStore } from '~/components/user/userStore'
import { useTabsStore, Tab } from '~/components/page/tabs/tabsStore'
import { SolutionType } from '../solutionTypeEnum'
import { usePageStore } from '~/components/page/pageStore'
import { useCommentsStore } from '~/components/comment/commentsStore'
import { usePublishQuestionStore } from '../edit/publish/publishQuestionStore'
import { useAnswerBodyLogic } from '~/composables/answerBody/useAnswerBodyLogic'

const learningSessionStore = useLearningSessionStore()
const pageStore = usePageStore()
const userStore = useUserStore()
const tabsStore = useTabsStore()
const commentsStore = useCommentsStore()
const publishQuestionStore = usePublishQuestionStore()
const { $urlHelper } = useNuxtApp()
const router = useRouter()
const route = useRoute()

// Check if we're in wishknowledge mode (mission-control/learning route)
const isWishknowledgeMode = computed(() => route.path.startsWith('/mission-control/learning'))

commentsStore.loadComments()

// Use the main composable that orchestrates all functionality
const answerBodyLogic = useAnswerBodyLogic()

const attachQuestionIdToUrl = async () => {
    if (!tabsStore.isLearning || !answerBodyLogic.answerBodyModel.value?.id || answerBodyLogic.answerBodyModel.value.id <= 0)
        return

    if (isWishknowledgeMode.value) {
        // Wishknowledge mode: simple path structure
        const newPath = `/mission-control/learning/${answerBodyLogic.answerBodyModel.value.id}`
        if (newPath !== route.path) {
            router.push(newPath)
        }
    } else {
        // Page-based mode: original logic
        const pathSegments = window.location.pathname
            .split('/')
            .filter(segment => segment.length > 0)

        const currentPageId = pathSegments.length >= 2 && !isNaN(parseInt(pathSegments[1]))
            ? parseInt(pathSegments[1])
            : null

        if (currentPageId === pageStore.id) {
            const newPath = $urlHelper.getPageUrlWithQuestionId(
                pageStore.name,
                pageStore.id,
                answerBodyLogic.answerBodyModel.value.id
            )

            if (newPath !== window.location.pathname) {
                router.push(newPath)
            }
        }
    }
}
watch(() => answerBodyLogic.answerBodyModel.value?.id, (newId, oldId) => {
    if (newId !== oldId && newId)
        attachQuestionIdToUrl()
})
watch(() => pageStore.id, (newId, oldId) => {
    // Only apply page change logic when not in wishknowledge mode
    if (!isWishknowledgeMode.value && newId !== oldId && answerBodyLogic.currentRequest.value) {
        answerBodyLogic.currentRequest.value.abort()
        answerBodyLogic.currentRequest.value = null
    }
})

watch(() => tabsStore.activeTab, () => {
    if (tabsStore.isLearning && isNaN(parseInt(route.params.questionId?.toString())))
        attachQuestionIdToUrl()
})

onMounted(() => {
    watch([() => learningSessionStore.currentStep?.index, () => learningSessionStore.currentStep?.id], () => {
        answerBodyLogic.loadAnswerBodyModel()
    })

    learningSessionStore.$onAction(({ name, after }) => {
        if (name === 'startNewSession') {
            after((newSession) => {
                if (newSession) {
                    answerBodyLogic.loadAnswerBodyModel()
                }
            })
        }

        if (name === 'reloadAnswerBody') {
            after((result) => {
                if (result.id === answerBodyLogic.answerBodyModel.value?.id && learningSessionStore.currentIndex === result.index) {
                    answerBodyLogic.loadAnswerBodyModel()
                }
            })
        }
    })

    watch(() => userStore.isLoggedIn, () => learningSessionStore.startNewSession())
})

watch(() => pageStore.id, () => {
    // Only reset results on page change when not in wishknowledge mode
    if (!isWishknowledgeMode.value) {
        learningSessionStore.showResult = false
    }
})

publishQuestionStore.$onAction(({ name, after }) => {
    if (name === 'confirmPublish') {
        after((id) => {
            if (id && id === answerBodyLogic.answerBodyModel.value?.id) {
                answerBodyLogic.answerBodyModel.value!.isPrivate = false
            }
        })
    }
})

// Event handlers that delegate to the composable
const handleAnswer = () => answerBodyLogic.answer()
const handleAnswerFlashcard = (isCorrect: boolean) => answerBodyLogic.answerFlashcard(isCorrect)
const handleFlip = () => answerBodyLogic.flip()
const handleLoadSolution = (answered?: boolean) => answerBodyLogic.loadSolution(answered)
const handleMarkAsCorrect = () => answerBodyLogic.markAsCorrect()
const handleLoadResult = () => answerBodyLogic.loadResult()
const handleStartNewSession = () => answerBodyLogic.startNewSession()
</script>

<template>
    <div id="AnswerBody" v-if="answerBodyLogic.answerBodyModel.value && !learningSessionStore.showResult">
        <QuestionAnswerBodyHeader :answer-body-model="answerBodyLogic.answerBodyModel.value" />

        <div class="row">
            <QuestionAnswerBodyExtendedQuestionContent :answer-body-model="answerBodyLogic.answerBodyModel.value" />

            <div id="AnswerAndSolutionCol">
                <div id="AnswerAndSolution">
                    <div
                        class="row"
                        :class="{ 'hasFlashcard': answerBodyLogic.answerBodyModel.value.solutionType === SolutionType.Flashcard }">
                        <div id="AnswerInputSection">
                            <QuestionAnswerBodyFlashcard
                                :key="answerBodyLogic.answerBodyModel.value.id + 'flashcard'"
                                v-if="answerBodyLogic.answerBodyModel.value.solutionType === SolutionType.Flashcard"
                                :ref="answerBodyLogic.flashcard"
                                :solution="answerBodyLogic.answerBodyModel.value.solution"
                                :front-content="answerBodyLogic.answerBodyModel.value.textHtml"
                                :marked-as-correct="answerBodyLogic.markFlashcardAsCorrect.value"
                                @flipped="answerBodyLogic.incrementTries()" />
                            <QuestionAnswerBodyMatchlist
                                :key="answerBodyLogic.answerBodyModel.value.id + 'matchlist'"
                                v-else-if="answerBodyLogic.answerBodyModel.value.solutionType === SolutionType.MatchList"
                                :ref="answerBodyLogic.matchList"
                                :solution="answerBodyLogic.answerBodyModel.value.solution"
                                :show-answer="answerBodyLogic.showAnswer.value"
                                @flipped="answerBodyLogic.incrementTries()" />
                            <QuestionAnswerBodyMultipleChoice
                                :key="answerBodyLogic.answerBodyModel.value.id + 'multiplechoice'"
                                v-else-if="answerBodyLogic.answerBodyModel.value.solutionType === SolutionType.MultipleChoice"
                                :solution="answerBodyLogic.answerBodyModel.value.solution"
                                :show-answer="answerBodyLogic.showAnswer.value"
                                :ref="answerBodyLogic.multipleChoice" />
                            <QuestionAnswerBodyText
                                :key="answerBodyLogic.answerBodyModel.value.id + 'text'"
                                v-else-if="answerBodyLogic.answerBodyModel.value.solutionType === SolutionType.Text"
                                :ref="answerBodyLogic.text"
                                :show-answer="answerBodyLogic.showAnswer.value" />
                        </div>

                        <div id="ButtonsAndSolutionCol">
                            <div id="ButtonsAndSolution" class="Clearfix">
                                <QuestionAnswerBodyButtons
                                    :answer-body-model="answerBodyLogic.answerBodyModel.value"
                                    :amount-of-tries="answerBodyLogic.amountOfTries.value"
                                    :show-answer="answerBodyLogic.showAnswer.value"
                                    :show-answer-buttons="answerBodyLogic.showAnswerButtons.value"
                                    :flash-card-answered="answerBodyLogic.flashCardAnswered.value"
                                    :answer-is-wrong="answerBodyLogic.answerIsWrong.value"
                                    :all-multiple-choice-combination-tried="answerBodyLogic.allMultipleChoiceCombinationTried.value"
                                    :learning-session-store="learningSessionStore"
                                    @answer="handleAnswer"
                                    @flip="handleFlip"
                                    @answer-flashcard="handleAnswerFlashcard"
                                    @load-solution="handleLoadSolution"
                                    @mark-as-correct="handleMarkAsCorrect"
                                    @load-result="handleLoadResult" />

                                <QuestionAnswerBodyFeedback
                                    :answer-body-model="answerBodyLogic.answerBodyModel.value"
                                    :answer-is-correct="answerBodyLogic.answerIsCorrect.value"
                                    :answer-is-wrong="answerBodyLogic.answerIsWrong.value"
                                    :answer-is-correct-pop-up="answerBodyLogic.answerIsCorrectPopUp.value"
                                    :answer-is-wrong-pop-up="answerBodyLogic.answerIsWrongPopUp.value"
                                    :well-done-msg="answerBodyLogic.wellDoneMsg.value"
                                    :wrong-answer-msg="answerBodyLogic.wrongAnswerMsg.value"
                                    :show-answer="answerBodyLogic.showAnswer.value"
                                    :show-wrong-answers="answerBodyLogic.showWrongAnswers.value"
                                    :answers-so-far="answerBodyLogic.answersSoFar.value"
                                    :solution-data="answerBodyLogic.solutionData.value" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <QuestionAnswerBodyAnswerQuestionDetails :id="answerBodyLogic.answerBodyModel.value.id" />
    </div>
    <div v-else-if="learningSessionStore.showResult === true">
        <QuestionAnswerBodyLearningSessionResult @start-new-session="handleStartNewSession" />
    </div>
</template>



<style lang="less">
@import '~~/assets/views/answerQuestion.less';

#AnswerBody {
    transition: all 1s ease-in-out;
    overflow: hidden;
}

.activity-points-icon {
    font-size: 14px;
}

#ActivityPointsDisplay {
    min-width: 100px;

    .activitypoints-display-detail {
        display: flex;
        justify-content: flex-end;
        align-items: center;
        flex-wrap: nowrap;

        #ActivityPoints {
            margin-right: 8px;
        }
    }
}
</style>

<style lang="less" scoped>
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
