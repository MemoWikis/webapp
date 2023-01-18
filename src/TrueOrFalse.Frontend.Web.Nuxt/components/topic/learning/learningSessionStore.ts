import { defineStore } from 'pinia'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'

export enum AnswerState {
    Unanswered = 0,
    False = 1,
    Correct = 2,
    Skipped = 3,
    ShowedSolutionOnly = 4
}

interface NewSessionResult {
    success: boolean,
    steps: AnswerState[]
}

export const useLearningSessionStore = defineStore('learningSessionStore', {
    state: () => {
        return {
            isLearningSession: true,
            isTestMode: false,
            lastIndex: 0,
            currentIndex: 0,
            steps: [] as AnswerState[],
            currentStep: 1,
        }
    },
    actions: {
        loadQuestion(skipIndex: number, sessionIndex: number) {
        },
        async startNewSession() {
            const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
            const json = learningSessionConfigurationStore.buildSessionConfigJson()
            const result = await $fetch<NewSessionResult>('/apiVue/LearningSessionStore/NewSession/', {
                method: 'POST',
                body: json,
                mode: 'cors',
                credentials: 'include'
            })
            if (result != null && result.success) {
                this.steps = result.steps
                return true
            } else return false
        },
        answerQuestion() {

        }
    },
})