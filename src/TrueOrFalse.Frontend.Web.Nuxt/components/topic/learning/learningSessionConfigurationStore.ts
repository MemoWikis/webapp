import { defineStore } from 'pinia'
import { useUserStore } from '../../user/userStore'
import { useTopicStore } from '../topicStore'
import { useLearningSessionStore } from './learningSessionStore'
import _ from 'underscore'

export class SessionConfig {
    questionFilterOptions: { [key: string]: any; } = {
        inWuwi: {
            count: 0,
            label: 'Im Wunschwissen',
            icon: 'fa-solid fa-heart',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[]; },
        notInWuwi: {
            count: 0,
            label: 'Nicht im Wunschwissen',
            icon: 'fa-regular fa-heart',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[]; },
        createdByCurrentUser: {
            count: 0,
            label: 'Von mir erstellt',
            icon: 'fa-solid fa-user-check',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[]; },
        notCreatedByCurrentUser: {
            count: 0,
            label: 'Nicht von mir erstellt',
            icon: 'fa-solid fa-user-slash',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[]; },
        privateQuestions: {
            count: 0,
            label: 'Private Fragen',
            icon: 'fa-solid fa-lock',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[]; },
        publicQuestions: {
            count: 0,
            label: 'Öffentliche Fragen',
            icon: 'fa-solid fa-globe',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[]; }
    }
    knowledgeSummary: { [key: string]: any; } = {
        notLearned: {
            count: 0,
            label: 'Noch nicht Gelernt',
            colorClass: 'not-learned',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[]; },
        needsLearning: {
            count: 0,
            label: 'Zu Lernen',
            colorClass: 'needs-learning',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[]; },
        needsConsolidation: {
            count: 0,
            label: 'Zu Festigen',
            colorClass: 'needs-consolidation',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[]; },
        solid: {
            count: 0,
            label: 'Sicher gewußt',
            colorClass: 'solid',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[]; }
    }
}

export enum QuestionOrder {
    Easiest,
    Hardest,
    PersonalHardest,
    Random
}

export enum RepetitionType {
    Normal,
    Leitner,
    None
}
export interface StoredSessionConfig {
    userId: number
    state: any
}

export const useLearningSessionConfigurationStore = defineStore('learningSessionConfigurationStore', {
    state: () => {
        const userStore = useUserStore()
        let sessionConfig = new SessionConfig()

        return {
            topicId: 0,
            order: QuestionOrder.Random,
            repetition: RepetitionType.Normal,
            questionsCount: 0,
            allQuestionCount: 0,
            currentQuestionCount: 0,
            knowledgeSummary: sessionConfig.knowledgeSummary,
            knowledgeSummaryCount: 0,
            questionFilterOptions: sessionConfig.questionFilterOptions,
            isTestMode: userStore.isLoggedIn ? false : true,
            isPracticeMode: userStore.isLoggedIn ? true : false,
            categoryHasNoQuestions: true,
            showError: false,
            activeCustomSettings: false,

            probabilityRange: [0, 100],
            maxSelectableQuestionCount: 0,
            selectedQuestionCount: 0,

            categoryName: '',
            selectedQuestionFilterOptionsDisplay: new SessionConfig().questionFilterOptions,
            selectedQuestionFilterOptionsExtraCount: 0,
            allQuestionFilterOptionsAreSelected: true,
            allKnowledgeSummaryOptionsAreSelected: true,

            userHasChangedMaxCount: false,

            showFilterDropdown: true,
            showQuestionFilterOptionsDropdown: false,
            showKnowledgeSummaryDropdown: false,
            showModeSelectionDropdown: false,
            testOptions: {
                questionOrder: 3,
                timeLimit: 0
            } as { [key: string]: number },
            practiceOptions: {
                questionOrder: 0,
                repetition: 1,
                answerHelp: true,
            } as { [key: string]: boolean | number },

            inputTimeout: null as ReturnType<typeof setTimeout> | null,
            questionCountInputFocused: false,
            timeLimit: 0,
            questionCountIsInvalid: false,
            userIdString: '',
            defaultMode: null,
            showSelectionError: false,
            sessionConfigKey: 'sessionConfig',
            showFilter: false
        }
    },
    getters: {
        maxQuestionCountIsZero(): boolean {
            return this.maxSelectableQuestionCount == 0
        }
    },
    actions: {
        setCounter(e: any) {
            if (e != null) {
                this.questionFilterOptions.inWuwi.count = e.InWuwi
                this.questionFilterOptions.notInWuwi.count = e.NotInWuwi
                this.questionFilterOptions.createdByCurrentUser.count = e.CreatedByCurrentUser
                this.questionFilterOptions.notCreatedByCurrentUser.count = e.NotCreatedByCurrentUser
                this.questionFilterOptions.privateQuestions.count = e.Private
                this.questionFilterOptions.publicQuestions.count = e.Public

                this.knowledgeSummary.notLearned.count = e.NotLearned
                this.knowledgeSummary.needsLearning.count = e.NeedsLearning
                this.knowledgeSummary.needsConsolidation.count = e.NeedsConsolidation
                this.knowledgeSummary.solid.count = e.Solid

                this.maxSelectableQuestionCount = e.Max as number

                if (!this.userHasChangedMaxCount)
                    this.selectedQuestionCount = e.Max as number
            }
        },
        startNewSession() {

        },
        async loadSessionFromLocalStorage() {
            const preLoadJson = this.buildSessionConfigJson()
            const userStore = useUserStore()

            if (userStore.isLoggedIn)
                this.sessionConfigKey = `sessionConfig-u${userStore.id}`

            const storedSession = localStorage.getItem(this.sessionConfigKey)

            if (storedSession != null) {
                const sessionConfig = JSON.parse(storedSession)

                if (userStore.isLoggedIn) {
                    this.knowledgeSummary = sessionConfig.knowledgeSummary
                    this.questionFilterOptions = sessionConfig.questionFilterOptions
                }
                this.userHasChangedMaxCount = sessionConfig.userHasChangedMaxCount
                this.selectedQuestionCount = sessionConfig.selectedQuestionCount as number
                this.isTestMode = sessionConfig.isTestMode
                this.isPracticeMode = sessionConfig.isPracticeMode
                this.testOptions = sessionConfig.testOptions
                this.practiceOptions = sessionConfig.practiceOptions
            }

            const postLoadJson = this.buildSessionConfigJson()

            if (preLoadJson != postLoadJson)
                this.activeCustomSettings = true
        },
        loadCustomSession() {
            if (this.maxQuestionCountIsZero)
                return
            const learningSessionStore = useLearningSessionStore()
            learningSessionStore.startNewSession()

            this.saveSessionConfig()
        },

        async saveSessionConfig() {
            var sessionConfig = {
                knowledgeSummary: this.knowledgeSummary,
                questionFilterOptions: this.questionFilterOptions,
                userHasChangedMaxCount: this.userHasChangedMaxCount,
                selectedQuestionCount: this.selectedQuestionCount,
                isTestMode: this.isTestMode,
                isPracticeMode: this.isPracticeMode,
                testOptions: this.testOptions,
                practiceOptions: this.practiceOptions,
            }

            localStorage.setItem(this.sessionConfigKey, JSON.stringify(sessionConfig))
        },

        selectAllKnowledgeSummary() {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }

            const force = this.allKnowledgeSummaryOptionsAreSelected ? false : true

            for (var key in this.knowledgeSummary) {
                this.selectKnowledgeSummary(this.knowledgeSummary[key], false, force)
            }
            this.activeCustomSettings = true

            this.lazyLoadCustomSession()
        },

        selectKnowledgeSummary(summary: any, loadCustomSession = true, force: boolean | null = null) {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }

            if (force)
                summary.isSelected = true
            else if (!force)
                summary.isSelected = false
            else
                summary.isSelected = !summary.isSelected

            this.checkKnowledgeSummarySelection()
            this.activeCustomSettings = true

            if (loadCustomSession)
                this.lazyLoadCustomSession()
        },

        checkKnowledgeSummarySelection() {
            var count = 0

            for (var key in this.knowledgeSummary)
                if (this.knowledgeSummary[key].isSelected)
                    count++

            this.knowledgeSummaryCount = count
            var allKnowledgeSummaryOptionsAreSelected = true

            for (var key in this.knowledgeSummary)
                if (!this.knowledgeSummary[key].isSelected)
                    allKnowledgeSummaryOptionsAreSelected = false

            this.allKnowledgeSummaryOptionsAreSelected = allKnowledgeSummaryOptionsAreSelected
        },

        selectAllQuestionFilter() {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }
            var force = this.allQuestionFilterOptionsAreSelected ? false : true

            for (var key in this.questionFilterOptions) {
                this.selectQuestionFilter(this.questionFilterOptions[key], false, force)
            }
            this.activeCustomSettings = true

            this.checkQuestionFilterSelection()
            this.lazyLoadCustomSession()

        },

        selectQuestionFilter(option: any, loadCustomSession = true, force: boolean | null = null) {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }

            if (force == null)
                option.isSelected = !option.isSelected
            else if (force)
                option.isSelected = true
            else if (!force)
                option.isSelected = false

            this.checkQuestionFilterSelection()
            this.activeCustomSettings = true

            if (loadCustomSession)
                this.lazyLoadCustomSession()
        },

        checkQuestionFilterSelection() {
            var allQuestionFilterOptionsAreSelected = true
            for (var key in this.questionFilterOptions)
                if (!this.questionFilterOptions[key].isSelected)
                    allQuestionFilterOptionsAreSelected = false

            this.allQuestionFilterOptionsAreSelected = allQuestionFilterOptionsAreSelected
            this.setQuestionFilterDisplay()
        },

        setQuestionFilterDisplay() {
            var count = 0
            var selectedOptions: any = []

            for (var key in this.questionFilterOptions)
                if (this.questionFilterOptions[key].isSelected) {
                    selectedOptions.push(this.questionFilterOptions[key])
                    count++
                }

            this.selectedQuestionFilterOptionsExtraCount = count - 3

            if (count > 4)
                this.selectedQuestionFilterOptionsDisplay = selectedOptions.slice(0, 3)
            else
                this.selectedQuestionFilterOptionsDisplay = selectedOptions
        },

        buildSessionConfigJson(id: number = 0, isInLearningTab = true) {
            const topicStore = useTopicStore()

            var json: { [key: string]: any; } = {}
            var base: { [key: string]: any; } = {
                CategoryId: topicStore.id,
                maxQuestionCount: this.selectedQuestionCount,

                InWuwi: this.questionFilterOptions.inWuwi.isSelected,
                NotInWuwi: this.questionFilterOptions.notInWuwi.isSelected,
                CreatedByCurrentUser: this.questionFilterOptions.createdByCurrentUser.isSelected,
                NotCreatedByCurrentUser: this.questionFilterOptions.notCreatedByCurrentUser.isSelected,
                PrivateQuestions: this.questionFilterOptions.privateQuestions.isSelected,
                PublicQuestions: this.questionFilterOptions.publicQuestions.isSelected,

                NotLearned: this.knowledgeSummary.notLearned.isSelected,
                NeedsLearning: this.knowledgeSummary.needsLearning.isSelected,
                NeedsConsolidation: this.knowledgeSummary.needsConsolidation.isSelected,
                Solid: this.knowledgeSummary.solid.isSelected,
                isInLearningTab: isInLearningTab
            }

            Object.keys(base)
                .forEach(key => json[key] = base[key])

            if (this.isPracticeMode) {

                var practiceJson: { [key: string]: any; } = {
                    QuestionOrder: this.practiceOptions.questionOrder,
                    Repetition: this.practiceOptions.repetition,
                    AnswerHelp: this.practiceOptions.answerHelp
                }

                Object.keys(practiceJson)
                    .forEach(key => json[key] = practiceJson[key])

            } else if (this.isTestMode) {

                var testJson: { [key: string]: any; } = {
                    QuestionOrder: this.testOptions.questionOrder,
                    Repetition: 0,
                    AnswerHelp: false,
                    TimeLimit: this.testOptions.timeLimit
                }

                Object.keys(testJson)
                    .forEach(key => json[key] = testJson[key])
            }

            return json
        },

        selectQuestionCount(val: number) {
            if (this.selectedQuestionCount + val <= 0)
                return

            this.userHasChangedMaxCount = true
            var count = this.selectedQuestionCount + val
            this.selectedQuestionCount = count
            this.activeCustomSettings = true
            this.lazyLoadCustomSession()
        },
        setSelectedQuestionCount(val: number) {
            this.questionCountIsInvalid = val <= 0 || isNaN(val) || val == null
            this.userHasChangedMaxCount = true
            if (this.questionCountIsInvalid)
                return

            this.lazyLoadCustomSession()
        },
        lazyLoadCustomSession() {
            if (this.inputTimeout != null)
                clearTimeout(this.inputTimeout)

            this.inputTimeout = setTimeout(() => {
                this.loadCustomSession()
            },
                400)
        },
        closeQuestionFilterDropdown() {
            this.showQuestionFilterOptionsDropdown = false
        },
        closeKnowledgeSummaryDropdown() {
            this.showKnowledgeSummaryDropdown = false
        },
        closeModeSelectionDropdown() {
            this.showModeSelectionDropdown = false
        },
        resetData() {
            if (!this.activeCustomSettings)
                return
            const userStore = useUserStore()

            this.knowledgeSummary = new SessionConfig().knowledgeSummary
            this.questionFilterOptions = new SessionConfig().questionFilterOptions
            this.checkQuestionFilterSelection()
            this.checkKnowledgeSummarySelection()
            this.userHasChangedMaxCount = false
            this.isTestMode = !userStore.isLoggedIn
            this.testOptions = {
                questionOrder: 3,
                timeLimit: 0
            }
            this.isPracticeMode = userStore.isLoggedIn
            this.practiceOptions = {
                questionOrder: 0,
                repetition: 1,
                answerHelp: true,
            }
            this.showQuestionFilterOptionsDropdown = false
            this.showKnowledgeSummaryDropdown = false
            this.showModeSelectionDropdown = false

            this.activeCustomSettings = false
        },
        reset() {
            this.resetData()
            this.loadCustomSession()
        },
        selectPracticeMode() {
            if (this.isPracticeMode)
                return
            this.isTestMode = false
            this.isPracticeMode = true
            this.activeCustomSettings = true
            this.lazyLoadCustomSession()
        },
        selectTestMode() {
            if (this.isTestMode)
                return
            this.isPracticeMode = false
            this.isTestMode = true
            this.activeCustomSettings = true
            this.lazyLoadCustomSession()
        },
        selectPracticeOption(key: string, val: number) {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn && val == 2 && key == 'questionOrder') {
                userStore.openLoginModal()
                return
            }
            this.practiceOptions[key] = val
            this.activeCustomSettings = true
            this.lazyLoadCustomSession()
        },
        selectTestOption(key: string | number, val: any) {
            this.testOptions[key] = val
            this.activeCustomSettings = true
            this.lazyLoadCustomSession()
        }
    },
})

