class SessionConfig {

    static questionFilterOptions = {
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
            icon: 'fas fa-unlock',
            isSelected: true,
            questionIds: [],
        }
    };

    static knowledgeSummary = {
        notLearned: {
            count: 0,
            label: 'Nicht Gelernt',
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
            label: 'Sicheres Wissen',
            colorClass: 'solid',
            isSelected: true,
            questionIds: [],
        }
    };

    static mode = {
        practice: {
            orderBy: 0 as QuestionOrder,
            repetition: 0 as RepetitionType,
            isSelected: true
        },
        test: {
            orderBy: 3 as QuestionOrder,
            timeLimit: 0,
            isSelected: false
        }
    }
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

                knowledgeSummary: SessionConfig.knowledgeSummary,
                questionFilterOptions: SessionConfig.questionFilterOptions,
                knowledgeSummaryCount: 0,
                selectedQuestionFilterOptionsDisplay: [],
                selectedQuestionFilterOptionsExtraCount: 0,
                allQuestionFilterOptionsAreSelected: true,
                
                userHasChangedMaxCount: false,

                showFilterDropdown: false,
                showQuestionFilterOptionsDropdown: false,
                showKnowledgeSummaryDropdown: false,
                showModeSelectionDropdown: false,
                isTestMode: false,
                testOptions: {
                    questionOrder: 3,
                    timeLimit: null
                },
                isPracticeMode: true,
                practiceOptions: {
                    questionOrder: 0,
                    repetition: 1,
                    answerHelp: true,
                },

                inputTimeout: null
            };
        },

        created() {
            eventBus.$on('openLearnOptions', () => { this.openModal() });
            eventBus.$on("start-learning-session", () => { this.loadCustomSession() });
        },

        mounted() {
            if (NotLoggedIn.Yes()) {
                this.title = 'Test';
                this.isLoggedIn = false;
                this.isTestModeOrNotLoginIn = IsLoggedIn;
                this.randomQuestions = !this.isLoggedIn;
            };

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
                else
                    this.selectedQuestionCount = parseInt(val);
            },
            repetitions: () => {
                this.questionFilter.repetitions = this.repetitions;
                if (this.repetitions == true) {
                    this.isTestMode = false;
                    this.questionFilter.isInTestMode = false;
                }
            },
            'questionFilter.maxQuestionCount'(val) {
                this.maxQuestionCountIsZero = val === 0;
            },
            knowledgeSummary(val) {
                this.knowledgeSummary = true;
                for (var key in val)
                    if (!val[key].isSelected)
                        this.knowledgeSummary = false;
            },
            questionFilterOptions(val) {
                this.allQuestionFilterOptionsAreSelected = true;
                for (var key in val)
                    if (!val[key].isSelected)
                        this.allQuestionFilterOptionsAreSelected = false;
            },
        },

        methods: {
            loadCustomSession(firstLoad = true) {
                if (this.maxQuestionCountIsZero)
                    return;

                eventBus.$emit('update-selected-page', 1);
                AnswerQuestion.LogTimeForQuestionView();

                var json = this.buildSessionConfigJson();

                this.answerBody.Loader.loadNewSession(json, true);

                $('#SessionConfigModal').modal('hide');

                this.$nextTick(() => {
                    eventBus.$emit("send-selected-questions", this.selectedQuestionCount);
                });

                this.isFirstLoad = firstLoad;
            },
            goToLogin() {
                this.openLogin = true;
                eventBus.$emit('show-login-modal');
            },
            selectAllKnowledgeSummary() {
                if (!this.isLoggedIn)
                    return;

                for (var key in this.knowledgeSummary) {
                    this.selectKnowledgeSummary(this.knowledgeSummary[key]);
                }
            },
            selectKnowledgeSummary(summary) {
                if (!this.isLoggedIn)
                    return;

                summary.isSelected = !summary.isSelected;
                var count = 0;

                for (var key in this.knowledgeSummary)
                    if (this.knowledgeSummary[key].isSelected)
                        count++;

                this.knowledgeSummaryCount = count;
                this.loadCustomSession(false);
            },
            selectAllQuestionFilter() {
                if (!this.isLoggedIn)
                    return;

                for (var key in this.questionFilterOptions) {
                    this.selectQuestionFilter(this.questionFilterOptions[key]);
                }
            },
            selectQuestionFilter(option) {
                if (!this.isLoggedIn)
                    return;

                option.isSelected = !option.isSelected;

                this.setQuestionFilterDisplay();
                this.loadCustomSession(false);
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
                        Repetitons: this.practiceOptions.repetitions,
                        AnswerHelp: this.practiceOptions.answerHelp
                    }

                    Object.keys(practiceJson)
                        .forEach(key => json[key] = practiceJson[key]);

                } else if (this.isTestMode) {

                    var testJson = {
                        QuestionOrder: this.timeLimit.questionOrder,
                        Repetitons: false,
                        AnswerHelp: false,
                        TimeLimit: this.testOptions.timeLimit
                    }

                    Object.keys(testJson)
                        .forEach(key => json[key] = testJson[key]);
                }

                return json;
            },

            selectQuestionCount(val: Number) {
                if (this.selectedQuestionCount + val > this.maxSelectableQuestionCount || this.selectedQuestionCount + val == 0)
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
                }, 1000);
            }

        }
    });

var SessionHeader = new Vue({
    el: '#SessionHeader',
}); 