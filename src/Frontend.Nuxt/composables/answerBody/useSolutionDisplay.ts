import { ref } from 'vue'
import { SolutionType } from '~/components/question/solutionTypeEnum'
import { SolutionData } from '~/components/question/answerBody/answerBodyInterfaces'
import { getHighlightedCode } from '~/utils/utils'

export const useSolutionDisplay = () => {
    const { $logger } = useNuxtApp()
    const { t } = useI18n()
    
    const solutionData = ref<SolutionData | null>(null)

    const loadSolution = async (
        answerBodyModel: any,
        amountOfTries: number,
        answered: boolean = true
    ) => {
        const data = {
            id: answerBodyModel?.id,
            questionViewGuid: answerBodyModel?.questionViewGuid,
            interactionNumber: amountOfTries,
            unanswered: !answered
        }

        const solutionResult = await $api('/apiVue/AnswerBody/GetSolution', {
            method: 'POST',
            body: data,
            mode: 'cors',
            credentials: 'include',
            onResponseError(context: any) {
                $logger.error(`fetch Error: ${context.response?.statusText} `, [{ 
                    response: context.response, 
                    host: context.request 
                }])
            }
        })

        if (solutionResult != null) {
            if (answerBodyModel!.solutionType === SolutionType.MultipleChoice && solutionResult.answer == null) {
                solutionResult.answer = t('answerbody.noCorrectAnswers')
                solutionResult.answerAsHTML = solutionResult.answer
            }
            solutionData.value = solutionResult
            highlightCode()
        }

        return solutionResult
    }

    const highlightCode = () => {
        const el = document.getElementById('AnswerBody')
        if (el != null) {
            el.querySelectorAll('code').forEach(block => {
                if (block.textContent != null) {
                    block.innerHTML = getHighlightedCode(block.textContent)
                }
            })
        }
    }

    const resetSolution = () => {
        solutionData.value = null
    }

    return {
        solutionData,
        loadSolution,
        highlightCode,
        resetSolution
    }
}
