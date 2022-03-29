class SessionConfig {

    questionFilterOptions = {
        inWuwi: {
            count: 0,
            label: 'Im Wunschwissen',
            icon: 'fas fa-heart',
            isSelected: true,
            questionIds: [],
        },
        notInWuwi: {
            count: 0,
            label: 'Nicht im Wunschwissen',
            icon: 'fa fa-heart-o',
            isSelected: true,
            questionIds: [],
        },
        createdByCurrentUser: {
            count: 0,
            label: 'Von mir erstellt',
            icon: 'fas fa-user-check',
            isSelected: true,
            questionIds: [],
        },
        notCreatedByCurrentUser: {
            count: 0,
            label: 'Nicht von mir erstellt',
            icon: 'fas fa-user-slash',
            isSelected: true,
            questionIds: [],
        },
        privateQuestions: {
            count: 0,
            label: 'Private Fragen',
            icon: 'fas fa-lock',
            isSelected: true,
            questionIds: [],
        },
        publicQuestions: {
            count: 0,
            label: 'Öffentliche Fragen',
            icon: 'fas fa-globe',
            isSelected: true,
            questionIds: [],
        }
    };

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
    };
}

enum QuestionOrder {
    Easiest,
    Hardest,
    PersonalHardest,
    Random
}

enum RepetitionType {
    Normal,
    Leitner,
    None
}

Vue.component('session-config-component',
    {
        template: '#session-config-template',
        props: ['questionsCount', 'allQuestionCount', 'isLoggedIn'],
        data() {
            return {
                answerBody: new AnswerBody(),
                probabilityRange: [0, 100],
                maxSelectableQuestionCount: 0,
                selectedQuestionCount: 0,

                categoryName: $("#hhdCategoryName").val(),
                displayMinus: false,
                isFirstLoad: true,
                showDropdown: false,

                knowledgeSummary: new SessionConfig().knowledgeSummary,
                questionFilterOptions: new SessionConfig().questionFilterOptions,
                knowledgeSummaryCount: 0,
                selectedQuestionFilterOptionsDisplay: [],
                selectedQuestionFilterOptionsExtraCount: 0,
                allQuestionFilterOptionsAreSelected: true,
                allKnowledgeSummaryOptionsAreSelected: true,

                userHasChangedMaxCount: false,

                showFilterDropdown: false,
                showQuestionFilterOptionsDropdown: false,
                showKnowledgeSummaryDropdown: false,
                showModeSelectionDropdown: false,
                isTestMode: this.isLoggedIn ? false : true,
                testOptions: {
                    questionOrder: 3,
                    timeLimit: 0
                },
                isPracticeMode: this.isLoggedIn ? true : false,
                practiceOptions: {
                    questionOrder: 0,
                    repetition: 1,
                    answerHelp: true,
                },

                inputTimeout: null,
                inputFocused: false,
                timeLimit: 0,
            };
        },

        created() {
            eventBus.$on('openLearnOptions', () => { this.openModal() });
            eventBus.$on("start-learning-session", () => { this.loadCustomSession() });
        },

        mounted() {
            eventBus.$on('sync-session-config',
                (sessionConfig) => {
                    this.knowledgeSummary = sessionConfig.knowledgeSummary;
                    this.questionFilterOptions = sessionConfig.questionFilterOptions;
                    this.userHasChangedMaxCount = sessionConfig.userHasChangedMaxCount;
                    this.selectedQuestionCount = sessionConfig.selectedQuestionCount;
                    this.isTestMode = sessionConfig.isTestMode;
                    this.isPracticeMode = sessionConfig.isPracticeMode;
                    this.testOptions = sessionConfig.testOptions;
                    this.practiceOptions = sessionConfig.practiceOptions;

                });
            eventBus.$on('session-config-question-counter',
                (e) => {
                    if (e != null) {
                        this.questionFilterOptions.inWuwi.count = e.InWuwi;
                        this.questionFilterOptions.notInWuwi.count = e.NotInWuwi;
                        this.questionFilterOptions.createdByCurrentUser.count = e.CreatedByCurrentUser;
                        this.questionFilterOptions.notCreatedByCurrentUser.count = e.NotCreatedByCurrentUser;
                        this.questionFilterOptions.privateQuestions.count = e.Private;
                        this.questionFilterOptions.publicQuestions.count = e.Public;

                        this.knowledgeSummary.notLearned.count = e.NotLearned;
                        this.knowledgeSummary.needsLearning.count = e.NeedsLearning;
                        this.knowledgeSummary.needsConsolidation.count = e.NeedsConsolidation;
                        this.knowledgeSummary.solid.count = e.Solid;

                        this.maxSelectableQuestionCount = e.Max;

                        if (!this.userHasChangedMaxCount)
                            this.selectedQuestionCount = e.Max;
                    }

                });

            this.setQuestionFilterDisplay();
        },

        watch: {
            isTestMode(val) {
                this.isPracticeMode = !val;
            },
            isPracticeMode(val) {
                this.isTestMode = !val;
            },
            selectedQuestionCount(val) {
                var count = parseInt(val);

                if (count > this.maxSelectableQuestionCount)
                    this.selectedQuestionCount = this.maxSelectableQuestionCount;
                else if (count == 0 && this.maxSelectableQuestionCount > 0)
                    this.selectedQuestionCount = 1;
                else
                    this.selectedQuestionCount = count;
            },
            'questionFilter.maxQuestionCount'(val) {
                this.maxQuestionCountIsZero = val === 0;
            },
            showQuestionFilterOptionsDropdown(val) {
                if (val) {
                    this.showKnowledgeSummaryDropdown = false;
                    this.showModeSelectionDropdown = false;
                }
            },
            showKnowledgeSummaryDropdown(val) {
                if (val) {
                    this.showQuestionFilterOptionsDropdown = false;
                    this.showModeSelectionDropdown = false;
                }
            },
            showModeSelectionDropdown(val) {
                if (val) {
                    this.showQuestionFilterOptionsDropdown = false;
                    this.showKnowledgeSummaryDropdown = false;
                }
            }
        },

        methods: {
            loadCustomSession(firstLoad = true) {
                if (this.maxQuestionCountIsZero)
                    return;

                eventBus.$emit('update-selected-page', 1);
                AnswerQuestion.LogTimeForQuestionView();
                var json = this.buildSessionConfigJson();
                this.answerBody.Loader.loadNewSession(json, true, false, false);
                $('#SessionConfigModal').modal('hide');
                this.sendSessionConfig();
                this.isFirstLoad = firstLoad;
            },

            sendSessionConfig() {
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
                    eventBus.$emit('sync-session-config', sessionConfig);
                });
            },

            goToLogin() {
                this.openLogin = true;
                eventBus.$emit('show-login-modal');
            },

            selectAllKnowledgeSummary() {
                if (!this.isLoggedIn) {
                    NotLoggedIn.ShowErrorMsg('set-session-filter-options');
                    return;
                }

                var force = {
                    true: !this.allKnowledgeSummaryOptionsAreSelected,
                    false: this.allKnowledgeSummaryOptionsAreSelected,
                }

                for (var key in this.knowledgeSummary) {
                    this.selectKnowledgeSummary(this.knowledgeSummary[key], false, force);
                }
                this.loadCustomSession(false);
            },

            selectKnowledgeSummary(summary, loadCustomSession = true, force = null) {
                if (!this.isLoggedIn) {
                    NotLoggedIn.ShowErrorMsg('set-session-filter-options');
                    return;
                }

                if (force == null)
                    summary.isSelected = !summary.isSelected;
                else if (force.true)
                    summary.isSelected = true;
                else if (force.false)
                    summary.isSelected = false;

                this.checkKnowledgeSummarySelection();

                if (loadCustomSession)
                    this.loadCustomSession(false);
            },

            checkKnowledgeSummarySelection() {
                var count = 0;

                for (var key in this.knowledgeSummary)
                    if (this.knowledgeSummary[key].isSelected)
                        count++;

                this.knowledgeSummaryCount = count;
                var allKnowledgeSummaryOptionsAreSelected = true;

                for (var key in this.knowledgeSummary)
                    if (!this.knowledgeSummary[key].isSelected)
                        allKnowledgeSummaryOptionsAreSelected = false;

                this.allKnowledgeSummaryOptionsAreSelected = allKnowledgeSummaryOptionsAreSelected;
            },

            selectAllQuestionFilter() {
                if (!this.isLoggedIn) {
                    NotLoggedIn.ShowErrorMsg('set-session-filter-options');
                    return;
                }
                var force = {
                    true: !this.allQuestionFilterOptionsAreSelected,
                    false: this.allQuestionFilterOptionsAreSelected,
                }
                for (var key in this.questionFilterOptions) {
                    this.selectQuestionFilter(this.questionFilterOptions[key], false, force);
                }

                this.checkQuestionFilterSelection();
                this.loadCustomSession(false);

            },

            selectQuestionFilter(option, loadCustomSession = true, force = null) {
                if (!this.isLoggedIn) {
                    NotLoggedIn.ShowErrorMsg('set-session-filter-options');
                    return;
                }

                if (force == null)
                    option.isSelected = !option.isSelected;
                else if (force.true)
                    option.isSelected = true;
                else if (force.false)
                    option.isSelected = false;

                this.checkQuestionFilterSelection();

                if (loadCustomSession)
                    this.loadCustomSession(false);
            },

            checkQuestionFilterSelection() {
                var allQuestionFilterOptionsAreSelected = true;
                for (var key in this.questionFilterOptions)
                    if (!this.questionFilterOptions[key].isSelected)
                        allQuestionFilterOptionsAreSelected = false;

                this.allQuestionFilterOptionsAreSelected = allQuestionFilterOptionsAreSelected;
                this.setQuestionFilterDisplay();
            },

            setQuestionFilterDisplay() {
                var count = 0;
                var selectedOptions = [];

                for (var key in this.questionFilterOptions)
                    if (this.questionFilterOptions[key].isSelected) {
                        selectedOptions.push(this.questionFilterOptions[key]);
                        count++;
                    }

                this.selectedQuestionFilterOptionsExtraCount = count - 3;

                if (count > 4)
                    this.selectedQuestionFilterOptionsDisplay = selectedOptions.slice(0, 3);
                else
                    this.selectedQuestionFilterOptionsDisplay = selectedOptions;
            },

            buildSessionConfigJson() {
                var json = {}
                var base = {
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
                    Solid: this.knowledgeSummary.solid.isSelected
                }

                Object.keys(base)
                    .forEach(key => json[key] = base[key]);

                if (this.isPracticeMode) {

                    var practiceJson = {
                        QuestionOrder: this.practiceOptions.questionOrder,
                        Repetition: this.practiceOptions.repetition,
                        AnswerHelp: this.practiceOptions.answerHelp
                    }

                    Object.keys(practiceJson)
                        .forEach(key => json[key] = practiceJson[key]);

                } else if (this.isTestMode) {

                    var testJson = {
                        QuestionOrder: this.testOptions.questionOrder,
                        Repetition: 0,
                        AnswerHelp: false,
                        TimeLimit: this.testOptions.timeLimit
                    }

                    Object.keys(testJson)
                        .forEach(key => json[key] = testJson[key]);
                }

                return json;
            },

            selectQuestionCount(val: Number) {
                if (this.selectedQuestionCount + val > this.maxSelectableQuestionCount ||
                    this.selectedQuestionCount + val == 0)
                    return;

                this.userHasChangedMaxCount = true;
                var count = this.selectedQuestionCount + val;
                this.selectedQuestionCount = count;
                this.lazyLoadCustomSession();
            },
            setSelectedQuestionCount(e) {
                this.userHasChangedMaxCount = true;
                this.lazyLoadCustomSession();
            },
            lazyLoadCustomSession() {
                clearTimeout(this.inputTimeout);
                this.inputTimeout = setTimeout(() => {
                        this.loadCustomSession(false);
                    },
                    1000);
            },
            closeDropdowns() {
                this.showQuestionFilterOptionsDropdown = false;
                this.showModeSelectionDropdown = false;
                this.showKnowledgeSummaryDropdown = false;
            },
            reset() {
                this.knowledgeSummary = new SessionConfig().knowledgeSummary;
                this.questionFilterOptions = new SessionConfig().questionFilterOptions;
                this.checkQuestionFilterSelection();
                this.checkKnowledgeSummarySelection();
                this.userHasChangedMaxCount = false;
                this.isTestMode = false;
                    this.testOptions = {
                        questionOrder: 3,
                        timeLimit: 0
                    };
                this.isPracticeMode = true;
                    this.practiceOptions = {
                        questionOrder: 0,
                        repetition: 1,
                        answerHelp: true,
                    };
                this.showQuestionFilterOptionsDropdown = false;
                this.showKnowledgeSummaryDropdown = false;
                this.showModeSelectionDropdown = false;

                this.loadCustomSession();
            },
            selectPracticeMode() {
                this.isPracticeMode = true;
                this.loadCustomSession(false);
            },
            selectTestMode() {
                this.isTestMode = true;
                this.loadCustomSession(false);
            },
            selectPracticeOption(key, val) {
                if (!this.isLoggedIn && val == 2 && key == 'questionOrder') {
                    NotLoggedIn.ShowErrorMsg('set-session-filter-options');
                    return;
                }
                this.practiceOptions[key] = val;
                this.loadCustomSession(false);
            },
            selectTestOption(key, val) {
                console.log(key)
                this.testOptions[key] = val;
                this.loadCustomSession(false);
            }
        }
    });

var SessionHeader = new Vue({
    el: '#SessionHeader',
    data() {
        return {
            categoryHasNoQuestions: false,
            filterError: false,
            showFilter: true,
        }
    },
    mounted() {
        this.categoryHasNoQuestions = $('#SessionConfigQuestionChecker').data('category-has-no-questions') != 'True';
        if (this.categoryHasNoQuestions)
            this.showFilter = false;

        eventBus.$on('set-session-progress',
            (e) => {
                if (e == null) 
                    this.filterError = true;
                else if (e.isResult)
                    this.showFilter = false;
                else
                    this.filterError = false;
            });

        eventBus.$on('init-new-session',
            () => {
                this.showFilter = true;
            });

        eventBus.$on('category-has-question',
            () => {
                this.categoryHasNoQuestions = false;
                this.showFilter = true;
            });
    }
});