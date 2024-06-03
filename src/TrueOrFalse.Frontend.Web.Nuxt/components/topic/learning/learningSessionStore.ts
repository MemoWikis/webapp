import { defineStore } from 'pinia'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'
import { AlertType, useAlertStore } from '~~/components/alert/alertStore'
import { messages } from '~~/components/alert/messages'
import { QuestionListItem } from './questionListItem'

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

interface LearningSessionResult {
    success: boolean
    steps: Step[]
    activeQuestionCount: number
    currentStep: Step
    answerHelp: boolean
    isInTestMode: boolean
    messageKey?: string
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
            }>(`/apiVue/LearningSessionStore/GetLastStepInQuestionList/${this.lastIndexInQuestionList}`, {
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
        async loadLearningSession(data: any, url: string) {

            const result = await $fetch<LearningSessionResult>(`/apiVue/LearningSessionStore/${url}`, {
                method: 'POST',
                body: data,
                mode: 'cors',
                credentials: 'include'
            })

            if (result.success && result.steps.length > 0) {
                this.steps = result.steps
                this.activeQuestionCount = result.activeQuestionCount
                this.setCurrentStep(result.currentStep)
                this.answerHelp = result.answerHelp
                this.isInTestMode = result.isInTestMode
            }

            const errorMsg = result.messageKey ? messages.getByCompositeKey(result.messageKey) : null
            return errorMsg
        },
        async startNewSession() {
            const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
            const config = learningSessionConfigurationStore.buildSessionConfigJson()

            return await this.loadLearningSession(config, 'NewSession')
        },
        async startNewSessionWithJumpToQuestion(id: number) {
            const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
            const config = learningSessionConfigurationStore.buildSessionConfigJson()
            learningSessionConfigurationStore.getQuestionCount()

            return await this.loadLearningSession({ config: config, id: id }, 'NewSessionWithJumpToQuestion')
        },
        handleQuestionNotInSessionAlert(id: number, msg: string) {
            const alertStore = useAlertStore()
            alertStore.openAlert(AlertType.Default, { text: msg, customBtnKey: 'reset-learning-session' }, 'Filter zurücksetzen', true)
            const learningSessionConfigurationStore = useLearningSessionConfigurationStore()

            alertStore.$onAction(({ name, after }) => {
                if (name == 'closeAlert')
                    after((result) => {
                        if (!result.cancelled && result.customKey == 'reset-learning-session') {
                            learningSessionConfigurationStore.resetData()
                            learningSessionConfigurationStore.saveSessionConfig()
                            learningSessionConfigurationStore.getQuestionCount()
                            this.startNewSessionWithJumpToQuestion(id)
                        } else {
                            // this.startNewSession()
                        }
                    })
            })
        },
        async loadSteps() {
            const result = await $fetch<Step[]>('/apiVue/LearningSessionStore/LoadSteps/', {
                mode: 'cors',
                credentials: 'include'
            })
            if (result != null)
                this.steps = result
        },
        async changeActiveQuestion(index: number) {
            const result = await $fetch<LearningSessionResult>('/apiVue/LearningSessionStore/LoadSpecificQuestion/', {
                method: 'POST',
                body: { index: index },
                mode: 'cors',
                credentials: 'include'
            })

            if (result) {
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
        },
        updateQuestionList(question: QuestionListItem) {
            return question
        },
        knowledgeStatusChanged(id: number) {
            return id
        },
        reloadAnswerBody(id:number, index: number) {
            return {id: id, index: index}
        }
    },
})