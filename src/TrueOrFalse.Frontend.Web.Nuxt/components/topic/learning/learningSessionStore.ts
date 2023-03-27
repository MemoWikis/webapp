import { defineStore } from 'pinia'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'
import { AlertType, useAlertStore } from '~~/components/alert/alertStore'
import { messages } from '~~/components/alert/messages'

export enum AnswerState {
    Unanswered = 0,
    False = 1,
    Correct = 2,
    Skipped = 3,
    ShowedSolutionOnly = 4
}

export interface Step {
    state: AnswerState
    id: number
    index: number
    isLastStep: boolean
}

interface NewSessionResult {
    success: boolean
    steps: Step[]
    activeQuestionCount: number
    firstStep: Step
    answerHelp: boolean
    isInTestMode: boolean
}

interface NewSessionWithJumpToQuestionResult {
    success: boolean
    message?: string
    steps?: Step[]
    activeQuestionCount?: number
    currentStep?: Step
    answerHelp?: boolean
    isInTestMode?: boolean
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
            answerHelp: true,
            showResult: false,
        }
    },
    actions: {
        setCurrentStep(step: Step) {
            this.currentStep = step
            this.currentIndex = step.index
        },
        async getLastStepInQuestionList() {
            const result = await $fetch<{
                success: boolean,
                steps: Step[],
                activeQuestionCount: number,
                lastQuestionInList: Step
            }>(`/apiVue/LearningSessionStore/GetLastStepInQuestionList/?index=${this.lastIndexInQuestionList}`, {
                mode: 'cors',
                credentials: 'include'
            })
            if (result != null && result.success) {
                this.steps = result.steps
                this.activeQuestionCount = result.activeQuestionCount
                this.setCurrentStep(result.lastQuestionInList)
                return true
            } else return false
        },
        async startNewSession() {
            const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
            const config = learningSessionConfigurationStore.buildSessionConfigJson()
            learningSessionConfigurationStore.getQuestionCount()

            const result = await $fetch<NewSessionResult>('/apiVue/LearningSessionStore/NewSession/', {
                method: 'POST',
                body: config,
                mode: 'cors',
                credentials: 'include'
            })
            if (result != null && result.success) {
                this.steps = result.steps
                this.activeQuestionCount = result.activeQuestionCount
                this.setCurrentStep(result.firstStep)
                this.answerHelp = result.answerHelp
                this.isInTestMode = result.isInTestMode
                return true
            } else return false
        },
        async startNewSessionWithJumpToQuestion(id: number) {

            const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
            const config = learningSessionConfigurationStore.buildSessionConfigJson()
            learningSessionConfigurationStore.getQuestionCount()

            const result = await $fetch<NewSessionWithJumpToQuestionResult>('/apiVue/LearningSessionStore/NewSessionWithJumpToQuestion/', {
                method: 'POST',
                body: { config: config, id: id },
                mode: 'cors',
                credentials: 'include'
            })

            if (result != null && result.success) {
                this.steps = result.steps!
                this.activeQuestionCount = result.activeQuestionCount!
                this.setCurrentStep(result.currentStep!)
                this.answerHelp = result.answerHelp!
                this.isInTestMode = result.isInTestMode!
                return true
            } else if (!result.success && result.message == 'questionNotInFilter') {
                const alertStore = useAlertStore()
                alertStore.openAlert(AlertType.Default, { text: messages.info.questionNotInFilter }, 'Filter zurücksetzen', true)

                alertStore.$onAction(({ name, after }) => {
                    if (name == 'closeAlert')
                        after((result) => {
                            if (!result.cancelled) {
                                learningSessionConfigurationStore.resetData()
                                learningSessionConfigurationStore.saveSessionConfig()
                                learningSessionConfigurationStore.getQuestionCount()
                                this.startNewSessionWithJumpToQuestion(id)
                            } else {
                                this.startNewSession()
                            }
                        })
                })
            } else return false
        },
        async loadSteps() {
            const result = await $fetch<Step[]>('/apiVue/LearningSessionStore/LoadSteps/', {
                mode: 'cors',
                credentials: 'include'
            })
            if (result != null) {
                this.steps = result
                return true
            } else return false
        },
        async changeActiveQuestion(index: number) {
            const result = await $fetch<{ steps: Step[], currentStep: Step }>('/apiVue/LearningSessionStore/LoadSpecificQuestion/', {
                method: 'POST',
                body: { index: index },
                mode: 'cors',
                credentials: 'include'
            })
            if (result != null) {
                this.steps = result.steps
                this.setCurrentStep(result.currentStep)
            }
        },
        loadNextQuestionInSession() {
            if (this.currentIndex < this.steps[this.steps.length - 1].index)
                this.changeActiveQuestion(this.currentIndex + 1)
        },
        async skipStep() {
            const data = {
                index: this.currentIndex
            }
            const result = await $fetch<Step>(`/apiVue/LearningSessionStore/SkipStep/`,
                {
                    method: 'POST',
                    body: data,
                    credentials: 'include',
                    mode: 'cors',
                })
            if (result) {
                this.steps[this.currentIndex].state = AnswerState.Skipped
                this.setCurrentStep(result)
            }
        },
        markCurrentStep(state: AnswerState) {
            if (this.currentStep)
                this.currentStep.state = state
            this.steps[this.currentIndex].state = state
        },
        markCurrentStepAsCorrect() {
            this.markCurrentStep(AnswerState.Correct)
        },
        markCurrentStepAsWrong() {
            this.markCurrentStep(AnswerState.False)
        },
        addNewQuestionToList(index: number) {
            return index
        }
    },
})