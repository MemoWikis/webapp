import { ref, watch, computed } from 'vue'

export const useAnswerAttempts = () => {
    const { t } = useI18n()
    
    const amountOfTries = ref(0)
    const amountOfTriesText = ref('')
    const answersSoFar = ref<string[]>([])
    const showWrongAnswers = ref(false)

    watch(amountOfTries, (val) => {
        switch (val) {
            case 0:
            case 1:
                amountOfTriesText.value = t(`answerbody.tries.${val}`)
                break
            case 2:
            case 3:
            case 4:
            case 5:
                amountOfTriesText.value = t(`answerbody.tries.${val}`)
                break
            case 6:
            case 7:
                amountOfTriesText.value = t(`answerbody.tries.${val}`)
                break
            default:
                amountOfTriesText.value = t('answerbody.tries.many')
        }
    })

    const incrementTries = () => {
        amountOfTries.value++
    }

    const addAnswer = (answer: string) => {
        answersSoFar.value.push(answer)
    }

    const isRepeatedAnswer = (answer: string): boolean => {
        return answersSoFar.value.indexOf(answer) >= 0
    }

    const resetAttempts = () => {
        amountOfTries.value = 0
        answersSoFar.value = []
        showWrongAnswers.value = false
    }

    const checkMultipleChoiceCombinationTried = (solutionType: any, solution: string): boolean => {
        if (solutionType === 'MultipleChoice') {
            interface Choice {
                Text: string,
                IsCorrect: boolean
            }
            const json: { Choices: Choice[], isSolutionOrdered: boolean } = JSON.parse(solution)
            const maxCombinationCount = 2 ** json.Choices.length
            const uniqueAnswerCount = [...new Set(answersSoFar.value)].length
            return uniqueAnswerCount >= maxCombinationCount
        }
        return false
    }

    return {
        amountOfTries,
        amountOfTriesText,
        answersSoFar,
        showWrongAnswers,
        incrementTries,
        addAnswer,
        isRepeatedAnswer,
        resetAttempts,
        checkMultipleChoiceCombinationTried
    }
}
