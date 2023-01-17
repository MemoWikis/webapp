import { defineStore } from 'pinia'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'

export const useLearningSessionStore = defineStore('learningSessionStore', {
    state: () => {
        return {
            isLearningSession: true,
            isTestMode: false,
            lastIndex: 0,
            currentIndex: 0,
        }
    },
    actions: {
        loadQuestion(skipIndex: number, sessionIndex: number) {
        },
        startNewSession() {
            const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
        }
    },
})