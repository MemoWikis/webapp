import { ref } from 'vue'

export const useFlashcardLogic = () => {
    const flashCardAnswered = ref(false)
    const markFlashcardAsCorrect = ref(false)

    const answerFlashcard = (isCorrect: boolean, answerCallback: () => void) => {
        markFlashcardAsCorrect.value = isCorrect
        flashCardAnswered.value = true
        answerCallback()
    }

    const flip = (flashcardRef: any) => {
        flashcardRef.value?.flip()
    }

    const resetFlashcard = () => {
        flashCardAnswered.value = false
        markFlashcardAsCorrect.value = false
    }

    return {
        flashCardAnswered,
        markFlashcardAsCorrect,
        answerFlashcard,
        flip,
        resetFlashcard
    }
}
