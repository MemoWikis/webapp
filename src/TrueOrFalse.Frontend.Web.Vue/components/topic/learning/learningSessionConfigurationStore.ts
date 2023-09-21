import { defineStore } from 'pinia'
import { useUserStore } from '../../user/userStore'
import { useTopicStore } from '../topicStore'
import _ from 'underscore'

export class SessionConfig {
    questionFilterOptions = {
        inWuwi: {
            count: 0,
            label: 'Im Wunschwissen',
            icon: 'fa-solid fa-heart',
            isSelected: true,
            questionIds: [],
        },
        notInWuwi: {
            count: 0,
            label: 'Nicht im Wunschwissen',
            icon: 'fa-regular fa-heart',
            isSelected: true,
            questionIds: [],
        },
        createdByCurrentUser: {
            count: 0,
            label: 'Von mir erstellt',
            icon: 'fa-solid fa-user-check',
            isSelected: true,
            questionIds: [],
        },
        notCreatedByCurrentUser: {
            count: 0,
            label: 'Nicht von mir erstellt',
            icon: 'fa-solid fa-user-slash',
            isSelected: true,
            questionIds: [],
        },
        privateQuestions: {
            count: 0,
            label: 'Private Fragen',
            icon: 'fa-solid fa-lock',
            isSelected: true,
            questionIds: [],
        },
        publicQuestions: {
            count: 0,
            label: 'Öffentliche Fragen',
            icon: 'fa-solid fa-globe',
            isSelected: true,
            questionIds: [],
        }
    }
    knowledgeSummary = {
        notLearned: {
            count: 0,
            label: 'Noch nicht Gelernt',
            colorClass: 'not-learned',
            isSelected: true,
            questionIds: [],
        },
        needsLearning: {
            count: 0,
            label: 'Zu Lernen',
            colorClass: 'needs-learning',
            isSelected: true,
            questionIds: [],
        },
        needsConsolidation: {
            count: 0,
            label: 'Zu Festigen',
            colorClass: 'needs-consolidation',
            isSelected: true,
            questionIds: [],
        },
        solid: {
            count: 0,
            label: 'Sicher gewußt',
            colorClass: 'solid',
            isSelected: true,
            questionIds: [],
        }
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

export const useLearningSessionConfigurationStore = defineStore('learningSessionConfigurationStore', {
    state: () => {
    const userStore = useUserStore()
      return {
        topicId: 0,
        order: QuestionOrder.Random,
        repetition: RepetitionType.Normal,
        questionsCount: 0,
        allQuestionCount: 0,
        currentQuestionCount: 0,
        knowledgeSummary: new SessionConfig().knowledgeSummary,
        knowledgeSummaryCount: 0,
        questionFilterOptions: new SessionConfig().questionFilterOptions,
        isTestMode: userStore.isLoggedIn ? false : true,
        isPracticeMode: userStore.isLoggedIn ? true : false,
        categoryHasNoQuestions: true,
        showError: false,
        activeCustomSettings: false,

        probabilityRange: [0, 100],
        maxSelectableQuestionCount: 0,
        selectedQuestionCount: 0,

        categoryName:'',
        selectedQuestionFilterOptionsDisplay: [],
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
        },
        practiceOptions: {
            questionOrder: 0,
            repetition: 1,
            answerHelp: true,
        },

        inputTimeout: null,
        questionCountInputFocused: false,
        timeLimit: 0,
        questionCountIsInvalid: false,
        sessionConfigKey: 'sessionConfig',
        userIdString: '',
        defaultMode: null,
        showSelectionError: false,
      }
    },
    getters: {
        maxQuestionCountIsZero() {
            return this.questionFilter.maxQuestionCount == 0
        }
    },
    actions: {
        startNewSession(){
            
        },
        loadSessionFromLocalStorage(firstLoad = false) {
            var storedSession = localStorage.getItem(this.sessionConfigKey)

            if (storedSession != null) {
                var sessionConfig = JSON.parse(storedSession)
                this.knowledgeSummary = sessionConfig.knowledgeSummary
                this.questionFilterOptions = sessionConfig.questionFilterOptions
                this.userHasChangedMaxCount = sessionConfig.userHasChangedMaxCount
                this.selectedQuestionCount = sessionConfig.selectedQuestionCount
                this.isTestMode = sessionConfig.isTestMode
                this.isPracticeMode = sessionConfig.isPracticeMode
                this.testOptions = sessionConfig.testOptions
                this.practiceOptions = sessionConfig.practiceOptions
            }

            if (firstLoad && this.isInQuestionList)
                this.$nextTick(() => this.loadCustomSession())

            var json = this.buildSessionConfigJson()
            localStorage.setItem('sessionConfigJson', JSON.stringify(json))
        },
        loadCustomSession() {
            if (this.maxQuestionCountIsZero)
                return

                // needs questionliststore
            // eventBus.$emit('update-selected-page', 1)
            var json = this.buildSessionConfigJson()
            // this.answerBody.Loader.loadNewSession(json, true, false, false)
            this.saveSessionConfig()
        },

        saveSessionConfig() {
            this.$nextTick(() => {
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

                // needs questionliststore
                // eventBus.$emit('sync-session-config', (this.isTestMode))
                this.checkSettingChanges()
            })
        },

        checkSettingChanges() {
            var hasCustomSettings = false
            var currentMode = JSON.parse(JSON.stringify({
                testMode: this.isTestMode,
                testOptions: this.testOptions,
                isPracticeMode: this.isPracticeMode,
                practiceOptions: this.practiceOptions
            }))

            var changedMode = !_.isEqual(JSON.stringify(this.defaultMode), JSON.stringify(currentMode))

            if (!this.allQuestionFilterOptionsAreSelected ||
                !this.allKnowledgeSummaryOptionsAreSelected ||
                this.userHasChangedMaxCount ||
                changedMode)
                hasCustomSettings = true
        },

        goToLogin() {
            this.openLogin = true
            const userStore = useUserStore()
            userStore.openLoginModal()
        },

        selectAllKnowledgeSummary() {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                // NotLoggedIn.ShowErrorMsg('set-session-filter-options')
                return
            }

            var force = {
                true: !this.allKnowledgeSummaryOptionsAreSelected,
                false: this.allKnowledgeSummaryOptionsAreSelected,
            }

            for (var key in this.knowledgeSummary) {
                this.selectKnowledgeSummary(this.knowledgeSummary[key], false, force)
            }
            this.lazyLoadCustomSession()
        },

        selectKnowledgeSummary(summary, loadCustomSession = true, force: any = null) {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                // NotLoggedIn.ShowErrorMsg('set-session-filter-options')
                return
            }

            if (force == null)
                summary.isSelected = !summary.isSelected
            else if (force.true)
                summary.isSelected = true
            else if (force.false)
                summary.isSelected = false

            this.checkKnowledgeSummarySelection()

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
                // NotLoggedIn.ShowErrorMsg('set-session-filter-options')
                return
            }
            var force = {
                true: !this.allQuestionFilterOptionsAreSelected,
                false: this.allQuestionFilterOptionsAreSelected,
            }
            for (var key in this.questionFilterOptions) {
                this.selectQuestionFilter(this.questionFilterOptions[key], false, force)
            }

            this.checkQuestionFilterSelection()
            this.lazyLoadCustomSession()

        },

        selectQuestionFilter(option, loadCustomSession = true, force: any = null) {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                // NotLoggedIn.ShowErrorMsg('set-session-filter-options')
                return
            }

            if (force == null)
                option.isSelected = !option.isSelected
            else if (force.true)
                option.isSelected = true
            else if (force.false)
                option.isSelected = false

            this.checkQuestionFilterSelection()

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

        buildSessionConfigJson(id, isInLearningTab = true) {
            if (id != 0)
                this.id = id

            var json = {}
            var base = {
                CategoryId: this.id,
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

                var practiceJson = {
                    QuestionOrder: this.practiceOptions.questionOrder,
                    Repetition: this.practiceOptions.repetition,
                    AnswerHelp: this.practiceOptions.answerHelp
                }

                Object.keys(practiceJson)
                    .forEach(key => json[key] = practiceJson[key])

            } else if (this.isTestMode) {

                var testJson = {
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

        selectQuestionCount(val: Number) {
            if (this.selectedQuestionCount + val <= 0)
                return

            this.userHasChangedMaxCount = true
            var count = this.selectedQuestionCount + val
            this.selectedQuestionCount = count
            this.lazyLoadCustomSession()
        },
        setSelectedQuestionCount(e) {
            var val = parseInt(e.target.value)
            this.questionCountIsInvalid = val <= 0 || isNaN(val) || val == null
            this.userHasChangedMaxCount = true
                
            if (this.questionCountIsInvalid)
                return

            this.lazyLoadCustomSession()
        },
        lazyLoadCustomSession() {
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
        reset() {
            if (!this.activeCustomSettings)
                return

            this.knowledgeSummary = new SessionConfig().knowledgeSummary
            this.questionFilterOptions = new SessionConfig().questionFilterOptions
            this.checkQuestionFilterSelection()
            this.checkKnowledgeSummarySelection()
            this.userHasChangedMaxCount = false
            this.isTestMode = this.isLoggedIn ? false : true
            this.testOptions = {
                    questionOrder: 3,
                    timeLimit: 0
                }
            this.isPracticeMode = this.isLoggedIn ? true : false
            this.practiceOptions = {
                    questionOrder: 0,
                    repetition: 1,
                    answerHelp: true,
                }
            this.showQuestionFilterOptionsDropdown = false
            this.showKnowledgeSummaryDropdown = false
            this.showModeSelectionDropdown = false

            this.loadCustomSession()
        },
        selectPracticeMode() {
            if (this.isPracticeMode)
                return
            this.isTestMode = false
            this.isPracticeMode = true
            this.lazyLoadCustomSession()
        },
        selectTestMode() {
            if (this.isTestMode)
                return
            this.practiceMode = false
            this.isTestMode = true
            this.lazyLoadCustomSession()
        },
        selectPracticeOption(key, val) {
            const userStore = useUserStore()
            if (!this.isLoggedIn && val == 2 && key == 'questionOrder') {
                // NotLoggedIn.ShowErrorMsg('set-session-filter-options')
                return
            }
            this.practiceOptions[key] = val
            this.lazyLoadCustomSession()
        },
        selectTestOption(key, val) {
            this.testOptions[key] = val
            this.lazyLoadCustomSession()
        }
    },
  })

  