import { ref } from 'vue'
import { SolutionType } from '~/components/question/solutionTypeEnum'
import { AnswerBodyModel } from '~/components/question/answerBody/answerBodyInterfaces'
import { useLearningSessionStore } from '~/components/page/learning/learningSessionStore'

export const useAnswerSubmission = () => {
    const { $logger } = useNuxtApp()
    const learningSessionStore = useLearningSessionStore()
    
    const showAnswer = ref(false)
    const showAnswerButtons = ref(true)

    const submitAnswer = async (
        answerBodyModel: AnswerBodyModel,
        solutionComponent: any
    ) => {
        const data = {
            answer: await solutionComponent.getAnswerDataString(),
            id: answerBodyModel.id,
            questionViewGuid: answerBodyModel.questionViewGuid,
            inTestMode: learningSessionStore.isInTestMode,
            isLearningSession: learningSessionStore.isLearningSession
        }

        const result = await $api(`/apiVue/AnswerBody/SendAnswerToLearningSession/`, {
            method: 'POST',
            body: data,
            credentials: 'include',
            mode: 'cors',
            onResponseError(context: any) {
                $logger.error(`fetch Error: ${context.response?.statusText}`, [{ 
                    response: context.response, 
                    host: context.request 
                }])
            }
        })

        return result
    }

    const markAsCorrect = async (answerBodyModel: AnswerBodyModel, amountOfTries: number) => {
        const data = {
            id: answerBodyModel.id,
            questionViewGuid: answerBodyModel.questionViewGuid,
            amountOfTries: amountOfTries,
        }

        const result = await $api('/apiVue/AnswerBody/MarkAsCorrect', {
            method: 'POST',
            mode: 'cors',
            body: data,
            onResponseError(context: any) {
                $logger.error(`fetch Error: ${context.response?.statusText} `, [{ 
                    response: context.response, 
                    host: context.request 
                }])
            }
        })

        return result
    }

    const getSolutionComponent = (solutionType: SolutionType, refs: any) => {
        switch (solutionType) {
            case SolutionType.MultipleChoice:
                return refs.multipleChoice.value
            case SolutionType.Text:
                return refs.text.value
            case SolutionType.MatchList:
                return refs.matchList.value
            case SolutionType.Flashcard:
                return refs.flashcard.value
            default:
                return null
        }
    }

    const resetSubmissionState = () => {
        showAnswer.value = false
        showAnswerButtons.value = true
    }

    return {
        showAnswer,
        showAnswerButtons,
        submitAnswer,
        markAsCorrect,
        getSolutionComponent,
        resetSubmissionState
    }
}
