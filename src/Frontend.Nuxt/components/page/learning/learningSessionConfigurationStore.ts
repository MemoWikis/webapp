import { defineStore } from 'pinia'
import { useUserStore } from '../../user/userStore'
import { usePageStore } from '../pageStore'
import { useLearningSessionStore } from './learningSessionStore'
import { KnowledgeSummaryType } from '~/composables/knowledgeSummary'
import _ from 'underscore'

export interface QustionCounter {
    createdByCurrentUser: number
    inWuwi: number
    max: number
    needsConsolidation: number
    needsLearning: number
    notCreatedByCurrentUser: number
    notInWuwi: number
    notLearned: number
    private: number
    public: number
    solid: number
}

export class SessionConfig {
    questionFilterOptions: { [key: string]: any } = {
        inWuwi: {
            count: 0,
            label: 'questionFilterOptions.inWuwi',
            icon: 'fa-solid fa-heart',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[] },
        notInWuwi: {
            count: 0,
            label: 'questionFilterOptions.notInWuwi',
            icon: 'fa-regular fa-heart',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[] },
        createdByCurrentUser: {
            count: 0,
            label: 'questionFilterOptions.createdByCurrentUser',
            icon: 'fa-solid fa-user-check',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[] },
        notCreatedByCurrentUser: {
            count: 0,
            label: 'questionFilterOptions.notCreatedByCurrentUser',
            icon: 'fa-solid fa-user-slash',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[] },
        privateQuestions: {
            count: 0,
            label: 'questionFilterOptions.private',
            icon: 'fa-solid fa-lock',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[] },
        publicQuestions: {
            count: 0,
            label: 'questionFilterOptions.public',
            icon: 'fa-solid fa-globe',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[] },
    }
    knowledgeSummary: { [key: string]: any } = {
        notLearned: {
            count: 0,
            label: 'knowledgeStatus.notLearned',
            colorClass: 'not-learned',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[] },
        needsLearning: {
            count: 0,
            label: 'knowledgeStatus.needsLearning',
            colorClass: 'needs-learning',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[] },
        needsConsolidation: {
            count: 0,
            label: 'knowledgeStatus.needsConsolidation',
            colorClass: 'needs-consolidation',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[] },
        solid: {
            count: 0,
            label: 'knowledgeStatus.solid',
            colorClass: 'solid',
            isSelected: true,
            questionIds: [],
        } as { [key: string]: number | string | boolean | number[] },
    }
}

export enum QuestionOrder {
    Easiest,
    Hardest,
    PersonalHardest,
    Random,
}

export enum RepetitionType {
    Normal,
    Leitner,
    None,
}
export interface StoredSessionConfig {
    userId: number
    state: any
}

export const useLearningSessionConfigurationStore = defineStore(
    'learningSessionConfigurationStore',
    {
        state: () => {
            const sessionConfig = new SessionConfig()

            return {
                pageId: 0,
                order: QuestionOrder.Random,
                repetition: RepetitionType.Normal,
                questionsCount: 0,
                allQuestionCount: 0,
                currentQuestionCount: 0,
                knowledgeSummary: sessionConfig.knowledgeSummary,
                knowledgeSummaryCount: 0,
                questionFilterOptions: sessionConfig.questionFilterOptions,
                isTestMode: false,
                isPracticeMode: true,
                pageHasNoQuestions: true,
                showError: false,
                activeCustomSettings: false,

                probabilityRange: [0, 100],
                maxSelectableQuestionCount: 0,
                selectedQuestionCount: 0,

                pageName: '',
                selectedQuestionFilterOptionsDisplay: new SessionConfig()
                    .questionFilterOptions,
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
                    timeLimit: 0,
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
                showFilter: false,
            }
        },
        getters: {
            maxQuestionCountIsZero(): boolean {
                return this.maxSelectableQuestionCount === 0
            },
        },
        actions: {
            setCounter(e: QustionCounter) {
                if (e != null) {
                    this.questionFilterOptions.inWuwi.count = e.inWuwi
                    this.questionFilterOptions.notInWuwi.count = e.notInWuwi
                    this.questionFilterOptions.createdByCurrentUser.count =
                        e.createdByCurrentUser
                    this.questionFilterOptions.notCreatedByCurrentUser.count =
                        e.notCreatedByCurrentUser
                    this.questionFilterOptions.privateQuestions.count =
                        e.private
                    this.questionFilterOptions.publicQuestions.count = e.public

                    this.knowledgeSummary.notLearned.count = e.notLearned
                    this.knowledgeSummary.needsLearning.count = e.needsLearning
                    this.knowledgeSummary.needsConsolidation.count =
                        e.needsConsolidation
                    this.knowledgeSummary.solid.count = e.solid

                    this.maxSelectableQuestionCount = e.max as number

                    if (!this.userHasChangedMaxCount)
                        this.selectedQuestionCount = e.max as number

                    if (this.maxQuestionCountIsZero)
                        this.showSelectionError = true
                    else this.showSelectionError = false
                }
            },
            async loadSessionFromLocalStorage() {
                const preLoadJson = this.buildSessionConfigJson()
                const userStore = useUserStore()

                if (userStore.isLoggedIn)
                    this.sessionConfigKey = `sessionConfig-u${userStore.id}`

                const storedSession = localStorage.getItem(
                    this.sessionConfigKey
                )

                if (storedSession != null) {
                    const sessionConfig = JSON.parse(storedSession)

                    if (userStore.isLoggedIn) {
                        this.knowledgeSummary = sessionConfig.knowledgeSummary
                        this.questionFilterOptions =
                            sessionConfig.questionFilterOptions
                    }
                    this.userHasChangedMaxCount =
                        sessionConfig.userHasChangedMaxCount
                    this.selectedQuestionCount =
                        sessionConfig.selectedQuestionCount as number
                    this.isTestMode = sessionConfig.isTestMode
                    this.isPracticeMode = sessionConfig.isPracticeMode
                    this.testOptions = sessionConfig.testOptions
                    this.practiceOptions = sessionConfig.practiceOptions
                }

                const postLoadJson = this.buildSessionConfigJson()

                if (preLoadJson != postLoadJson)
                    this.activeCustomSettings = true
            },
            async getQuestionCount() {
                const pageStore = usePageStore()
                const sessionJson = this.buildSessionConfigJson(pageStore.id)
                const count = await $api<QustionCounter>(
                    `/apiVue/LearningSessionConfigurationStore/GetCount/`,
                    {
                        body: sessionJson,
                        method: 'POST',
                        mode: 'cors',
                        credentials: 'include',
                    }
                )
                if (count) this.setCounter(count)
            },

            async loadCustomSession() {
                await this.getQuestionCount()
                if (this.maxQuestionCountIsZero) return
                const learningSessionStore = useLearningSessionStore()
                await learningSessionStore.startNewSession()

                this.saveSessionConfig()
            },

            async saveSessionConfig() {
                const sessionConfig = {
                    knowledgeSummary: this.knowledgeSummary,
                    questionFilterOptions: this.questionFilterOptions,
                    userHasChangedMaxCount: this.userHasChangedMaxCount,
                    selectedQuestionCount: this.selectedQuestionCount,
                    isTestMode: this.isTestMode,
                    isPracticeMode: this.isPracticeMode,
                    testOptions: this.testOptions,
                    practiceOptions: this.practiceOptions,
                }

                localStorage.setItem(
                    this.sessionConfigKey,
                    JSON.stringify(sessionConfig)
                )
            },

            selectAllKnowledgeSummary() {
                const userStore = useUserStore()
                if (!userStore.isLoggedIn) {
                    userStore.openLoginModal()
                    return
                }

                const force = this.allKnowledgeSummaryOptionsAreSelected
                    ? false
                    : true

                for (const key in this.knowledgeSummary) {
                    this.selectKnowledgeSummary(
                        this.knowledgeSummary[key],
                        false,
                        force
                    )
                }
                this.activeCustomSettings = true

                this.lazyLoadCustomSession()
            },

            selectKnowledgeSummary(
                summary: any,
                loadCustomSession = true,
                force: boolean | null = null
            ) {
                const userStore = useUserStore()
                if (!userStore.isLoggedIn) {
                    userStore.openLoginModal()
                    return
                }

                if (force === true) summary.isSelected = true
                else if (force === false) summary.isSelected = false
                else summary.isSelected = !summary.isSelected

                this.checkKnowledgeSummarySelection()
                this.activeCustomSettings = true

                if (loadCustomSession) this.lazyLoadCustomSession()
            },

            checkKnowledgeSummarySelection() {
                let count = 0

                for (const key in this.knowledgeSummary)
                    if (this.knowledgeSummary[key].isSelected) count++

                this.knowledgeSummaryCount = count
                let allKnowledgeSummaryOptionsAreSelected = true

                for (const key in this.knowledgeSummary)
                    if (!this.knowledgeSummary[key].isSelected)
                        allKnowledgeSummaryOptionsAreSelected = false

                this.allKnowledgeSummaryOptionsAreSelected =
                    allKnowledgeSummaryOptionsAreSelected
            },

            selectAllQuestionFilter() {
                const userStore = useUserStore()
                if (!userStore.isLoggedIn) {
                    userStore.openLoginModal()
                    return
                }
                const force = this.allQuestionFilterOptionsAreSelected
                    ? false
                    : true

                for (const key in this.questionFilterOptions) {
                    this.selectQuestionFilter(
                        this.questionFilterOptions[key],
                        false,
                        force
                    )
                }
                this.activeCustomSettings = true

                this.checkQuestionFilterSelection()
                this.lazyLoadCustomSession()
            },

            selectQuestionFilter(
                option: any,
                loadCustomSession = true,
                force: boolean | null = null
            ) {
                const userStore = useUserStore()
                if (!userStore.isLoggedIn) {
                    userStore.openLoginModal()
                    return
                }

                if (force === true) option.isSelected = true
                else if (force === false) option.isSelected = false
                else option.isSelected = !option.isSelected

                this.checkQuestionFilterSelection()
                this.activeCustomSettings = true

                if (loadCustomSession) this.lazyLoadCustomSession()
            },

            checkQuestionFilterSelection() {
                let allQuestionFilterOptionsAreSelected = true
                for (const key in this.questionFilterOptions)
                    if (!this.questionFilterOptions[key].isSelected)
                        allQuestionFilterOptionsAreSelected = false

                this.allQuestionFilterOptionsAreSelected =
                    allQuestionFilterOptionsAreSelected
                this.setQuestionFilterDisplay()
            },

            setQuestionFilterDisplay() {
                let count = 0
                const selectedOptions: any = []

                for (const key in this.questionFilterOptions)
                    if (this.questionFilterOptions[key].isSelected) {
                        selectedOptions.push(this.questionFilterOptions[key])
                        count++
                    }

                this.selectedQuestionFilterOptionsExtraCount = count - 3

                if (count > 4)
                    this.selectedQuestionFilterOptionsDisplay =
                        selectedOptions.slice(0, 3)
                else this.selectedQuestionFilterOptionsDisplay = selectedOptions
            },

            buildSessionConfigJson(id: number = 0, isInLearningTab = true) {
                const pageStore = usePageStore()
                const userStore = useUserStore()

                const json: { [key: string]: any } = {}
                const base: { [key: string]: any } = {
                    pageId: pageStore.id,
                    maxQuestionCount: this.selectedQuestionCount,

                    inWuwi: this.questionFilterOptions.inWuwi.isSelected,
                    notInWuwi: this.questionFilterOptions.notInWuwi.isSelected,
                    createdByCurrentUser:
                        this.questionFilterOptions.createdByCurrentUser
                            .isSelected,
                    notCreatedByCurrentUser:
                        this.questionFilterOptions.notCreatedByCurrentUser
                            .isSelected,
                    privateQuestions:
                        this.questionFilterOptions.privateQuestions.isSelected,
                    publicQuestions:
                        this.questionFilterOptions.publicQuestions.isSelected,
                    currentUserId: userStore.id,
                    notLearned: this.knowledgeSummary.notLearned.isSelected,
                    needsLearning:
                        this.knowledgeSummary.needsLearning.isSelected,
                    needsConsolidation:
                        this.knowledgeSummary.needsConsolidation.isSelected,
                    solid: this.knowledgeSummary.solid.isSelected,
                    isInLearningTab: isInLearningTab,
                }

                Object.keys(base).forEach((key) => (json[key] = base[key]))

                if (this.isPracticeMode) {
                    const practiceJson: { [key: string]: any } = {
                        questionOrder: this.practiceOptions.questionOrder,
                        repetition: this.practiceOptions.repetition,
                        answerHelp: this.practiceOptions.answerHelp,
                    }

                    Object.keys(practiceJson).forEach(
                        (key) => (json[key] = practiceJson[key])
                    )
                } else if (this.isTestMode) {
                    const testJson: { [key: string]: any } = {
                        questionOrder: this.testOptions.questionOrder,
                        repetition: 0,
                        answerHelp: false,
                        timeLimit: this.testOptions.timeLimit,
                    }

                    Object.keys(testJson).forEach(
                        (key) => (json[key] = testJson[key])
                    )
                }

                return json
            },

            selectQuestionCount(val: number) {
                if (this.selectedQuestionCount + val <= 0) return

                this.userHasChangedMaxCount = true
                const count = this.selectedQuestionCount + val
                this.selectedQuestionCount = count
                this.activeCustomSettings = true
                this.lazyLoadCustomSession()
            },
            setSelectedQuestionCount(val: number) {
                this.questionCountIsInvalid =
                    val <= 0 || isNaN(val) || val == null
                this.userHasChangedMaxCount = true
                if (this.questionCountIsInvalid) return

                this.lazyLoadCustomSession()
            },
            lazyLoadCustomSession() {
                if (this.inputTimeout != null) clearTimeout(this.inputTimeout)

                this.inputTimeout = setTimeout(() => {
                    this.loadCustomSession()
                }, 400)
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
                if (!this.activeCustomSettings) return
                const userStore = useUserStore()
                const pageStore = usePageStore()
                this.knowledgeSummary = new SessionConfig().knowledgeSummary
                this.questionFilterOptions =
                    new SessionConfig().questionFilterOptions
                this.checkQuestionFilterSelection()
                this.checkKnowledgeSummarySelection()
                this.selectedQuestionCount = pageStore.questionCount
                this.userHasChangedMaxCount = false
                this.isTestMode = !userStore.isLoggedIn
                this.testOptions = {
                    questionOrder: 3,
                    timeLimit: 0,
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
                if (this.isPracticeMode) return
                this.isTestMode = false
                this.isPracticeMode = true
                this.activeCustomSettings = true
                this.lazyLoadCustomSession()
            },
            selectTestMode() {
                if (this.isTestMode) return
                this.isPracticeMode = false
                this.isTestMode = true
                this.activeCustomSettings = true
                this.lazyLoadCustomSession()
            },
            selectPracticeOption(key: string, val: number) {
                const userStore = useUserStore()
                if (
                    !userStore.isLoggedIn &&
                    val === 2 &&
                    key === 'questionOrder'
                ) {
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
            },
            selectKnowledgeSummaryByType(type: KnowledgeSummaryType) {
                // First, unselect all knowledge summaries
                for (const key in this.knowledgeSummary) {
                    this.knowledgeSummary[key].isSelected = false
                }
                
                // Map the KnowledgeSummaryType to the corresponding key in knowledgeSummary
                const typeToKeyMap: { [key in KnowledgeSummaryType]: string } = {
                    [KnowledgeSummaryType.Solid]: 'solid',
                    [KnowledgeSummaryType.NeedsConsolidation]: 'needsConsolidation',
                    [KnowledgeSummaryType.NeedsLearning]: 'needsLearning',
                    [KnowledgeSummaryType.NotLearned]: 'notLearned',
                    [KnowledgeSummaryType.NotInWishknowledge]: 'notInWishknowledge',
                }
                
                const targetKey = typeToKeyMap[type]
                
                if (targetKey === KnowledgeSummaryType.NotInWishknowledge) {
                    for (const key in this.knowledgeSummary) {
                        this.knowledgeSummary[key].isSelected = true
                    }
                    this.questionFilterOptions.inWuwi.isSelected = false
                    this.questionFilterOptions.notInWuwi.isSelected = true

                } else {
                    this.questionFilterOptions.inWuwi.isSelected = true
                    this.questionFilterOptions.notInWuwi.isSelected = false

                    if (this.knowledgeSummary[targetKey]) {
                        this.knowledgeSummary[targetKey].isSelected = true
                    }
                }
                
                // Update the selection state
                this.checkQuestionFilterSelection()
                this.checkKnowledgeSummarySelection()
                this.activeCustomSettings = true
            },
        },
    }
)
