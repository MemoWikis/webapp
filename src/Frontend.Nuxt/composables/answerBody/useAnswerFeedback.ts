import { ref, watch } from 'vue'
import { random } from '~/utils/utils'

export function useAnswerFeedback() {
    const { t } = useI18n()
    
    const answerIsCorrect = ref(false)
    const answerIsCorrectPopUp = ref(false)
    const answerIsWrong = ref(false)
    const answerIsWrongPopUp = ref(false)
    const wellDoneMsg = ref('')
    const wrongAnswerMsg = ref('')

    const errorMessages = [
        'answerbody.errorMessages.churchill',
        'answerbody.errorMessages.keepGoing',
        'answerbody.errorMessages.practice'
    ]

    const repeatedErrorMessages = [
        'answerbody.repeatedErrorMessages.confucius',
        'answerbody.repeatedErrorMessages.balanced'
    ]

    const successMessages = [
        'answerbody.successMessages.yeah',
        'answerbody.successMessages.goodWay',
        'answerbody.successMessages.clean',
        'answerbody.successMessages.wellDone',
        'answerbody.successMessages.great',
        'answerbody.successMessages.keepItUp',
        'answerbody.successMessages.exactly',
        'answerbody.successMessages.absolutely',
        'answerbody.successMessages.cantBeMoreCorrect',
        'answerbody.successMessages.flawless',
        'answerbody.successMessages.correct',
        'answerbody.successMessages.perfect',
        'answerbody.successMessages.more',
        'answerbody.successMessages.awesome',
        'answerbody.successMessages.schubidu',
        'answerbody.successMessages.thereYouGo',
        'answerbody.successMessages.exact',
        'answerbody.successMessages.thatsIt',
        'answerbody.successMessages.nothingToComplain',
        'answerbody.successMessages.looksGood',
        'answerbody.successMessages.oha',
        'answerbody.successMessages.rrrright'
    ]

    watch(answerIsCorrect, (val) => {
        if (val) {
            answerIsCorrectPopUp.value = true
        }
        setTimeout(() => {
            answerIsCorrectPopUp.value = false
        }, 1200)
    })

    watch(answerIsWrong, (val) => {
        if (val) {
            answerIsWrongPopUp.value = true
        }
        setTimeout(() => {
            answerIsWrongPopUp.value = false
        }, 1200)
    })

    const setCorrectAnswer = () => {
        answerIsWrong.value = false
        answerIsCorrect.value = true
        wellDoneMsg.value = t(successMessages[random(0, successMessages.length - 1)])
    }

    const setWrongAnswer = (isRepeatedAnswer: boolean = false) => {
        answerIsCorrect.value = false
        answerIsWrong.value = true
        const messages = isRepeatedAnswer ? repeatedErrorMessages : errorMessages
        wrongAnswerMsg.value = t(messages[random(0, messages.length - 1)])
    }

    const setEmptyAnswerError = () => {
        wrongAnswerMsg.value = t('answerbody.emptyAnswerMessage')
    }

    const resetFeedback = () => {
        answerIsCorrect.value = false
        answerIsWrong.value = false
        wellDoneMsg.value = ''
        wrongAnswerMsg.value = ''
    }

    return {
        answerIsCorrect,
        answerIsCorrectPopUp,
        answerIsWrong,
        answerIsWrongPopUp,
        wellDoneMsg,
        wrongAnswerMsg,
        setCorrectAnswer,
        setWrongAnswer,
        setEmptyAnswerError,
        resetFeedback
    }
}
