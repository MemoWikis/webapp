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
    state: AnswerState
    id: number
    index: number
}

interface NewSessionResult {
    success: boolean
    steps: Step[]
    activeQuestionCount: number
    firstStep: Step
    answerHelp: boolean
    isInTestMode: boolean
}

export const useLearningSessionStore = defineStore('learningSessionStore', {
    state: () => {
        return {
            isLearningSession: true,
            isInTestMode: false,
            lastIndexInQuestionList: 0,
            currentIndex: 0,
            steps: [] as Step[],
            currentStep: null as Step | null,
            activeQuestionCount: 0,
            answerHelp: true
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
                this.answerHelp = result.answerHelp
                this.isInTestMode = result.isInTestMode
                return true
            } else return false
        },
        answerQuestion() {

        },
        async changeActiveQuestion(index: number) {
            const result = await $fetch<Step>('/apiVue/LearningSessionStore/LoadSpecificQuestion/', {
                method: 'POST',
                body: { index: index },
                mode: 'cors',
                credentials: 'include'
            })
            if (result != null) {
                this.currentStep = result
            }
        }
    },
})