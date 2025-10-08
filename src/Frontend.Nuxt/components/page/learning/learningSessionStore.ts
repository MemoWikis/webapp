import { defineStore } from "pinia"
import { useLearningSessionConfigurationStore } from "./learningSessionConfigurationStore"
import { AlertType, useAlertStore } from "~~/components/alert/alertStore"
import { QuestionListItem } from "./questionListItem"

export enum AnswerState {
    Unanswered = 0,
    False = 1,
    Correct = 2,
    Skipped = 3,
    ShowedSolutionOnly = 4,
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

export const useLearningSessionStore = defineStore("learningSessionStore", {
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
            const result = await $api<{
                success: boolean
                steps: Step[]
                activeQuestionCount: number
                lastQuestionInList: Step
            }>(
                `/apiVue/LearningSessionStore/GetLastStepInQuestionList/${this.lastIndexInQuestionList}`,
                {
                    mode: "cors",
                    credentials: "include",
                }
            )
            if (result != null && result.success) {
                this.steps = result.steps
                this.activeQuestionCount = result.activeQuestionCount
                this.setCurrentStep(result.lastQuestionInList)
                return true
            } else return false
        },
        async loadLearningSession(data: any, url: string) {
            const result = await $api<LearningSessionResult>(
                `/apiVue/LearningSessionStore/${url}`,
                {
                    method: "POST",
                    body: data,
                    mode: "cors",
                    credentials: "include",
                }
            )

            if (result.success && result.steps.length > 0) {
                this.steps = result.steps
                this.activeQuestionCount = result.activeQuestionCount
                this.setCurrentStep(result.currentStep)
                this.answerHelp = result.answerHelp
                this.isInTestMode = result.isInTestMode
            } else if (result.success && result.steps.length == 0) {
                this.steps = []
                this.currentStep = null
                this.currentIndex = 0
            }
            const nuxtApp = useNuxtApp()
            const { $i18n } = nuxtApp

            const errorMsg = result.messageKey
                ? $i18n.t(result.messageKey)
                : null
            return errorMsg
        },
        async startNewSession() {
            const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
            const config = learningSessionConfigurationStore.buildSessionConfigJson()
            learningSessionConfigurationStore.getQuestionCount()

            return await this.loadLearningSession(config, "NewSession")
        },
        async startNewSessionWithJumpToQuestion(id: number) {
            const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
            const config = learningSessionConfigurationStore.buildSessionConfigJson()
            learningSessionConfigurationStore.getQuestionCount()

            return await this.loadLearningSession(
                { config: config, id: id },
                "NewSessionWithJumpToQuestion"
            )
        },
        handleQuestionNotInSessionAlert(id: number, msg: string) {
            const alertStore = useAlertStore()
            alertStore.openAlert(
                AlertType.Default,
                { text: msg, customBtnKey: "reset-learning-session" },
                "Filter zurücksetzen",
                true
            )
            const learningSessionConfigurationStore =
                useLearningSessionConfigurationStore()

            alertStore.$onAction(({ name, after }) => {
                if (name == "closeAlert")
                    after((result) => {
                        if (
                            !result.cancelled &&
                            result.customKey == "reset-learning-session"
                        ) {
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
            const result = await $api<Step[]>(
                "/apiVue/LearningSessionStore/LoadSteps/",
                {
                    mode: "cors",
                    credentials: "include",
                }
            )
            if (result != null) this.steps = result
        },
        async changeActiveQuestion(index: number) {
            const result = await $api<LearningSessionResult>(
                `/apiVue/LearningSessionStore/LoadSpecificQuestion/${index}`,
                {
                    method: "POST",
                    mode: "cors",
                    credentials: "include",
                }
            )

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
            const result = await $api<Step>(
                `/apiVue/LearningSessionStore/SkipStep/${this.currentIndex}`,
                {
                    method: "POST",
                    credentials: "include",
                    mode: "cors",
                }
            )
            if (result) {
                this.steps[this.currentIndex].state = AnswerState.Skipped
                this.setCurrentStep(result)
            }
        },
        markCurrentStep(state: AnswerState) {
            if (this.currentStep) this.currentStep.state = state
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
        addNewQuestionsToList(startIndex: number, endIndex: number) {
            return { startIndex: startIndex, endIndex: endIndex }
        },
        updateQuestionList(question: QuestionListItem) {
            return question
        },
        knowledgeStatusChanged(id: number) {
            return id
        },
        reloadAnswerBody(id: number, index: number) {
            return { id: id, index: index }
        },
    },
})
