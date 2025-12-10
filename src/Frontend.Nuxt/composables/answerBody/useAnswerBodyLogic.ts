import { ref, computed, nextTick } from 'vue'
import { SolutionType } from '~/components/question/solutionTypeEnum'
import { AnswerBodyModel } from '~/components/question/answerBody/answerBodyInterfaces'
import { AnswerState } from '~/components/page/learning/learningSessionStore'
import { Activity } from '~/components/activityPoints/activityPointsStore'
import { useAnswerFeedback } from './useAnswerFeedback'
import { useAnswerAttempts } from './useAnswerAttempts'
import { useAnswerSubmission } from './useAnswerSubmission'
import { useSolutionDisplay } from './useSolutionDisplay'
import { useFlashcardLogic } from './useFlashcardLogic'
import { useLearningSessionStore } from '~/components/page/learning/learningSessionStore'
import { useActivityPointsStore } from '~/components/activityPoints/activityPointsStore'
import { usePageStore } from '~/components/page/pageStore'
import { hydrateFigcaptions } from '~/components/shared/figureExtension'

export const useAnswerBodyLogic = (allWishknowledgeMode: boolean = false) => {
    const { $logger } = useNuxtApp()
    
    // Store instances
    const learningSessionStore = useLearningSessionStore()
    const activityPointsStore = useActivityPointsStore()
    const pageStore = usePageStore()
    
    // Composables
    const feedback = useAnswerFeedback()
    const attempts = useAnswerAttempts()
    const submission = useAnswerSubmission()
    const solution = useSolutionDisplay()
    const flashcard = useFlashcardLogic()

    // Core state
    const answerBodyModel = ref<AnswerBodyModel>()
    const currentRequest = ref<AbortController | null>(null)

    // Component refs
    const multipleChoice = ref()
    const text = ref()
    const matchList = ref()
    const flashcardRef = ref()

    const allMultipleChoiceCombinationTried = computed(() => {
        if (answerBodyModel.value?.solutionType === SolutionType.MultipleChoice) {
            return attempts.checkMultipleChoiceCombinationTried(
                answerBodyModel.value.solutionType,
                answerBodyModel.value.solution
            )
        }
        return false
    })

    const loadAnswerBodyModel = async () => {
        if (!learningSessionStore.currentStep) {
            return
        }

        if (currentRequest.value) {
            currentRequest.value.abort()
        }

        currentRequest.value = new AbortController()

        const result = await $api(`/apiVue/AnswerBody/Get/${learningSessionStore.currentIndex}`, {
            mode: 'cors',
            credentials: 'include',
            signal: currentRequest.value.signal,
            onResponseError(context: any) {
                $logger.error(`fetch Error: ${context.response?.statusText}`, [{ 
                    response: context.response, 
                    host: context.request 
                }])
            }
        }).catch(() => {
            return null
        })

        currentRequest.value = null

        if (result != null) {
            resetState()
            answerBodyModel.value = result
            await nextTick()
            solution.highlightCode()
            hydrateFigcaptions()
        }
    }

    const resetState = () => {
        flashcard.resetFlashcard()
        feedback.resetFeedback()
        attempts.resetAttempts()
        submission.resetSubmissionState()
        solution.resetSolution()
    }

    const answer = async () => {
        attempts.showWrongAnswers.value = false

        if (answerBodyModel.value?.solutionType === SolutionType.Text && text.value.getAnswerText().trim().length === 0) {
            feedback.setEmptyAnswerError()
            attempts.showWrongAnswers.value = true
            return
        }

        const solutionComponent = submission.getSolutionComponent(
            answerBodyModel.value!.solutionType, 
            { multipleChoice, text, matchList, flashcard: flashcardRef }
        )
        
        if (solutionComponent == null) {
            return
        }

        const answerText = solutionComponent.getAnswerText()
        const repeatedAnswer = attempts.isRepeatedAnswer(answerText)
        attempts.addAnswer(answerText)
        attempts.incrementTries()

        const result = await submission.submitAnswer(
            answerBodyModel.value!,
            solutionComponent
        )

        if (result) {
            learningSessionStore.knowledgeStatusChanged(answerBodyModel.value!.id)
            await pageStore.reloadKnowledgeSummary()
            
            if (result.correct) {
                activityPointsStore.addPoints(Activity.CorrectAnswer)
                learningSessionStore.markCurrentStepAsCorrect()
                feedback.setCorrectAnswer()
            } else {
                activityPointsStore.addPoints(Activity.WrongAnswer)
                learningSessionStore.markCurrentStepAsWrong()
                feedback.setWrongAnswer(repeatedAnswer)
            }

            submission.showAnswerButtons.value = false

            if (shouldLoadSolution()) {
                await solution.loadSolution(answerBodyModel.value, attempts.amountOfTries.value)
                submission.showAnswer.value = true
                if (answerBodyModel.value?.solutionType === SolutionType.Text) {
                    attempts.showWrongAnswers.value = true
                }
            }

            await nextTick()
            if (result.newStepAdded) {
                await learningSessionStore.loadSteps()
            }

            learningSessionStore.currentStep!.isLastStep = result.isLastStep
        }
    }

    const shouldLoadSolution = (): boolean => {
        return learningSessionStore.isInTestMode
            || feedback.answerIsCorrect.value
            || (answerBodyModel.value?.solutionType != SolutionType.MultipleChoice && attempts.amountOfTries.value > 1)
            || (answerBodyModel.value?.solutionType === SolutionType.MultipleChoice && allMultipleChoiceCombinationTried.value)
    }

    const answerFlashcard = (isCorrect: boolean) => {
        flashcard.answerFlashcard(isCorrect, () => answer())
    }

    const flip = () => {
        flashcard.flip(flashcardRef)
    }

    const markAsCorrect = async () => {
        const result = await submission.markAsCorrect(answerBodyModel.value!, attempts.amountOfTries.value)

        if (result != false) {
            activityPointsStore.addPoints(Activity.CountAsCorrect)
            learningSessionStore.markCurrentStepAsCorrect()
            feedback.setCorrectAnswer()
        }
    }

    const loadSolution = async (answered: boolean = true) => {
        submission.showAnswerButtons.value = false
        if (answerBodyModel.value?.solutionType === SolutionType.Text) {
            attempts.showWrongAnswers.value = true
        }
        submission.showAnswer.value = true
        
        const solutionResult = await solution.loadSolution(
            answerBodyModel.value,
            attempts.amountOfTries.value,
            answered
        )

        if (!answered && solutionResult) {
            learningSessionStore.markCurrentStep(AnswerState.ShowedSolutionOnly)
        }
    }

    const loadResult = () => {
        answerBodyModel.value = undefined
        learningSessionStore.showResult = true
    }

    const startNewSession = () => {
        learningSessionStore.showResult = false
        learningSessionStore.startNewSession(allWishknowledgeMode)
    }

    return {
        // State
        answerBodyModel,
        currentRequest,
        
        // Component refs  
        multipleChoice,
        text,
        matchList,
        flashcard: flashcardRef,
        
        // Computed
        allMultipleChoiceCombinationTried,
        
        // Composable exports
        ...feedback,
        ...attempts,
        ...submission,
        ...solution,
        ...flashcard,
        
        // Methods
        loadAnswerBodyModel,
        resetState,
        answer,
        answerFlashcard,
        flip,
        markAsCorrect,
        loadSolution,
        loadResult,
        startNewSession
    }
}
