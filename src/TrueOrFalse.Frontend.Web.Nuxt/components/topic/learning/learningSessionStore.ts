import { defineStore } from 'pinia'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'

export enum AnswerState {
    Unanswered = 0,
    False = 1,
    Correct = 2,
    Skipped = 3,
    ShowedSolutionOnly = 4
}

interface Step {
    state: AnswerState,
    id: number,
    index: number
}

interface NewSessionResult {
    success: boolean,
    steps: Step[],
    activeQuestionCount: number,
    firstStep: Step
}

export const useLearningSessionStore = defineStore('learningSessionStore', {
    state: () => {
        return {
            isLearningSession: true,
            isTestMode: false,
            lastIndexInQuestionList: 0,
            currentIndex: 0,
            steps: [] as Step[],
            currentStep: null as Step | null,
            activeQuestionCount: 0
        }
    },
    actions: {
        async getLastStepInQuestionList() {
            const result = await $fetch<{
                success: boolean,
                steps: Step[],
                activeQuestionCount: number,
            }>(`/apiVue/LearningSessionStore/GetLastStepInQuestionList/&index=${this.lastIndexInQuestionList}`, {
                mode: 'cors',
                credentials: 'include'
            })
            if (result != null && result.success) {
                this.steps = result.steps
                this.activeQuestionCount = result.activeQuestionCount
                return true
            } else return false
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
                this.activeQuestionCount = result.activeQuestionCount
                this.currentStep = result.firstStep
                return true
            } else return false
        },
        answerQuestion() {

        },
        changeActiveQuestion(index: number) {
            console.log(index)
        }
    },
})