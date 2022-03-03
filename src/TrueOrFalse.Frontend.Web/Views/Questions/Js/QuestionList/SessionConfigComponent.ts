declare var VueSlider: any;
declare var piniaBuild: any;

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
    Leitner
}

interface SessionConfigJson {
    categoryId: Number;
    maxQuestionCount: Number;

    inWuwi: Boolean;
    notInWuwi: Boolean;
    createdByCurrentUser: Boolean;
    notCreatedByCurrentUser: Boolean;
    privateQuestions: Boolean;
    publicQuestions: Boolean;
    
    notLearned: Boolean;
    needsLearning: Boolean;
    needsConsolidation: Boolean;
    solid: Boolean;
}

let sc = Vue.component('session-config-component',
    {
        components: {
            VueSlider: window['vue-slider-component']
        },

        props: ['questionsCount', 'allQuestionCount', 'isMyWorld'],

        data() {
            return {
                answerBody: new AnswerBody(),
                probabilityRange: [0, 100],
                isLoggedIn: true,
                maxSelectableQuestionCount: 0,
                selectedQuestionCount: 0,
                inWishknowledge: true,
                percentages: '{value}%',
                maxQuestionCountIsZero: false,
                isTestMode: false,
                isTestModeOrNotLoginIn: false,
                isHoveringOptions: false,
                radioHeight: 0,
                openLogin: false,
                createdByCurrentUser: true,
                allQuestions: true,
                isNotQuestionInWishKnowledge: true,
                safeLearningSessionOptions: false,
                displayNone: true,
                randomQuestions: false,
                answerHelp: true,
                repetitions: true,
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
                showFilterDropdown: false,
                questionFilter: {
                    selectedQuestionFilterOptions: this.selectedQuestionFilterOptions,
                    selectedKnowledgeSummary: this.selectedKnowledgeSummary,
                    maxQuestionCount: 0,
                    inWishknowledge: true,
                    createdByCurrentUser: true,
                    allQuestions: true,
                    isNotQuestionInWishKnowledge: true,
                    isInTestMode: false,
                    safeLearningSessionOptions: false,
                    categoryId: $('#hhdCategoryId').val(),
                    answerHelp: true,
                    repetitions: true,
                    randomQuestions: false
                },
                orderByEasy: true,
                orderByHard: false,
                orderByPersonalHardest: false,
                repetitionByLeitner: false,
                timeLimit: null,
            };
        },

        created() {
            eventBus.$on('openLearnOptions', () => { this.openModal() });
            eventBus.$on("start-learning-session", () => { this.loadCustomSession() });
        },

        mounted() {
            var self = this;
            this.loadQuestionCount();
            if (NotLoggedIn.Yes()) {
                this.title = 'Test';
                this.isLoggedIn = false;
                this.isTestModeOrNotLoginIn = IsLoggedIn;
                this.randomQuestions = !this.isLoggedIn;
            };

            this.$nextTick(() => {
                window.addEventListener('resize', this.matchSize);
                this.matchSize();
            });

            $('#SessionConfigModal').on('shown.bs.modal',
                () => {
                    self.matchSize();
                });
            $('#SessionConfigModal').on('hidden.bs.modal',
                () => {
                    if (self.openLogin)
                        Login.OpenModal();
                });
            eventBus.$on('init-new-session',
                () => {
                    this.$nextTick(() => this.selectedQuestionCount = this.maxSelectableQuestionCount);
                });

            eventBus.$on('filter-details',
                (filterDetails) => {
                    filterDetails.forEach(f => {
                        if (f.Group == 'questionFilterOptions') {
                            this.questionFilterOptions[f.Key].questionIds = f.Ids;
                            this.questionFilterOptions[f.Key].count = f.Count;
                        } else if (f.Group == 'knowledgeSummary') {
                            this.knowledgeSummary[f.Key].questionIds = f.Ids;
                            this.knowledgeSummary[f.Key].count = f.Count;
                        }
                    });
                });
        },

        watch: {
            probabilityRange() {
                this.questionFilter.minProbability = this.probabilityRange[0];
                this.questionFilter.maxProbability = this.probabilityRange[1];
                this.loadQuestionCount();
            },
            isTestMode(val) {
                this.questionFilter.isInTestMode = val;
                this.isTestModeOrNotLoginIn = val;
                if (val == true) {
                    this.randomQuestions = true;
                    this.answerHelp = false;
                    this.repetitions = false;
                } else {
                    this.randomQuestions = false;
                    this.answerHelp = true;
                    this.repetitions = true;
                }

            },
            selectedQuestionCount(val) {
                this.questionFilter.maxQuestionCount = parseInt(val);
            },
            inWishknowledge(val) {
                if (val == true && this.isNotQuestionInWishKnowledge && this.createdByCurrentUser)
                    this.allQuestions = true;
                else
                    this.allQuestions = false;

                if (this.allQuestions == false) {
                    this.loadQuestionCount();
                }
                if (this.inWishknowledge == false &&
                    this.createdByCurrentUser == false &&
                    this.isNotQuestionInWishKnowledge == false ||
                    this.inWishknowledge == true &&
                    this.createdByCurrentUser == true &&
                    this.isNotQuestionInWishKnowledge == true) {
                    this.displayMinus = false;
                } else
                    this.displayMinus = true;
            },
            createdByCurrentUser(val) {
                if (val == true && this.isNotQuestionInWishKnowledge && this.inWishknowledge)
                    this.allQuestions = true;
                else
                    this.allQuestions = false;

                if (this.allQuestions == false) {
                    this.loadQuestionCount();
                }

                if (this.inWishknowledge == false &&
                    this.createdByCurrentUser == false &&
                    this.isNotQuestionInWishKnowledge == false ||
                    this.inWishknowledge == true &&
                    this.createdByCurrentUser == true &&
                    this.isNotQuestionInWishKnowledge == true) {
                    this.displayMinus = false;
                } else
                    this.displayMinus = true;
            },
            allQuestions(val) {

                if (val == true) {
                    this.inWishknowledge = true;
                    this.createdByCurrentUser = true;
                    this.isNotQuestionInWishKnowledge = true;
                }
                if (val == false &&
                    this.inWishknowledge == true &&
                    this.createdByCurrentUser == true &&
                    this.isNotQuestionInWishKnowledge == true) {
                    this.inWishknowledge = false;
                    this.createdByCurrentUser = false;
                    this.isNotQuestionInWishKnowledge = false;
                }

                this.loadQuestionCount();
            },
            isNotQuestionInWishKnowledge(val) {
                if (val == true && this.inWishknowledge && this.createdByCurrentUser)
                    this.allQuestions = true;
                else
                    this.allQuestions = false;

                if (this.allQuestions == false) {
                    this.loadQuestionCount();
                }

                if (this.inWishknowledge == false &&
                    this.createdByCurrentUser == false &&
                    this.isNotQuestionInWishKnowledge == false ||
                    this.inWishknowledge == true &&
                    this.createdByCurrentUser == true &&
                    this.isNotQuestionInWishKnowledge == true) {
                    this.displayMinus = false;
                } else
                    this.displayMinus = true;
            },
            safeLearningSessionOptions(val) {
                this.questionFilter.safeLearningSessionOptions = val;
            },
            randomQuestions: () => {
                this.questionFilter.randomQuestions = this.randomQuestions;
            },
            answerHelp(val) {
                this.questionFilter.answerHelp = this.answerHelp;
                if (this.answerHelp == true) {
                    this.isTestMode = false;
                    this.questionFilter.isInTestMode = false;
                }

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
                this.allQuestionFilterOptionsAreSelected = true;
                for (var key in val)
                    if (!val[key].isSelected)
                        this.allQuestionFilterOptionsAreSelected = false;
            }
        },

        methods: {
            loadQuestionCount() {
                //this.saveQuestionFilter();

                //$.ajax({
                //    url: "/AnswerQuestion/GetQuestionCount/",
                //    async: false,
                //    data: {
                //        config: this.questionFilter
                //    },
                //    type: "POST",
                //    success: result => {
                //        this.maxSelectableQuestionCount = parseInt(result);

                //        if (this.isFirstLoad && this.selectedQuestionCount == result ||
                //            typeof (this.isFirstLoad) == "undefined" ||
                //            this.selectedQuestionCount > this.maxSelectableQuestionCount ||
                //            this.selectedQuestionCount == 0)
                //            this.selectedQuestionCount = parseInt(result);
                //    }
                //});
            },
            resetQuestionFilter() {
                this.questionFilter = {
                    minProbability: 0,
                    maxProbability: 100,
                    maxQuestionCount: this.isLoggedIn ? 10 : 5,
                    questionsInWishknowledge: false,
                    isNotQuestionInWishKnowledge: false,
                    userIsAuthor: false,
                    allQuestions: false
                };
            },
            loadCustomSession(firstLoad = true) {
                if (this.maxQuestionCountIsZero)
                    return;
                eventBus.$emit('update-selected-page', 1);
                AnswerQuestion.LogTimeForQuestionView();

                this.saveQuestionFilter();

                if (this.isMyWorld == 'True')
                    this.questionFilter.isNotQuestionInWishKnowledge = false;

                this.answerBody.Loader.loadNewSession(this.questionFilter, true);
                $('#SessionConfigModal').modal('hide');
                this.questionFilter.safeLearningSessionOptions = this.safeLearningSessionOptions = false;
                this.$nextTick(() => {
                    eventBus.$emit("send-selected-questions", this.selectedQuestionCount);
                });
                this.isFirstLoad = firstLoad;
            },
            matchSize() {
                //    this.radioHeight = this.$refs.radioSection.clientHeight;
            },
            openModal() {
                this.loadQuestionCount();
                $('#SessionConfigModal').modal('show');
                this.openLogin = false;
            },
            goToLogin() {
                this.openLogin = true;
                $('#SessionConfigModal').modal('hide');
                document.body.classList.remove('modal-open');
                eventBus.$emit('show-login-modal');
            },
            saveQuestionFilter() {
                this.questionFilter.allQuestions = this.allQuestions;
                this.questionFilter.isNotQuestionInWishKnowledge = this.isNotQuestionInWishKnowledge;
                this.questionFilter.inWishknowledge = this.inWishknowledge;
                this.questionFilter.createdByCurrentUser = this.createdByCurrentUser;
                this.questionFilter.safeLearningSessionOptions = this.safeLearningSessionOptions;
                this.questionFilter.isInTestMode = this.isTestMode;
                this.questionFilter.maxQuestionCount = this.selectedQuestionCount;
                this.questionFilter.maxProbability = this.probabilityRange[1] === 100 ? 100 : this.probabilityRange[1];
                this.questionFilter.minProbability = this.probabilityRange[0] === 0 ? 0 : this.probabilityRange[0];
            },
            selectAllKnowledgeSummary() {
                for (var key in this.knowledgeSummary) {
                    this.selectKnowledgeSummary(this.knowledgeSummary[key]);
                }
            },
            selectKnowledgeSummary(summary) {
                summary.isSelected = !summary.isSelected;
                var count = 0;

                for (var key in this.knowledgeSummary)
                    if (this.knowledgeSummary[key].isSelected)
                        count++;

                this.knowledgeSummaryCount = count;
                this.loadCustomSession(false);
            },
            selectAllQuestionFilter() {
                for (var key in this.questionFilterOptions) {
                    this.selectQuestionFilter(this.questionFilterOptions[key]);
                }
            },
            selectQuestionFilter(option) {
                option.isSelected = !option.isSelected;

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

                this.loadCustomSession(false);
            },

        }
    });