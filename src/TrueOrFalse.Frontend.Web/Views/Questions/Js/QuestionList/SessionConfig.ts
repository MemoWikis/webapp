declare var VueSlider: any;

let sc= Vue.component('session-config-component', {
    components: {
        VueSlider: window['vue-slider-component']
    },
    props: ['questionsCount', 'allQuestionsCountFromCategory','isMyWorld'],
    data() {
        return {
            answerBody: new AnswerBody(),
            probabilityRange: [0, 100],
            questionFilter: {
                minProbability: 0,
                maxProbability: 100,
                maxQuestionCount: 0,
                inWishknowledge: true,
                createdByCurrentUser: true,
                allQuestions: true,
                isNotQuestionInWishKnowledge: true,
                isInTestMode: false,
                safeLearningSessionOptions: false,
                categoryId: $('#hhdCategoryId').val(),
                answerHelp: true,
                repititions: true,
                randomQuestions: false
            },
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
            repititions: true,
            categoryName: $("#hhdCategoryName").val(),
            displayMinus: false,
            isFirstLoad: true
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

        this.$nextTick(function () {
            window.addEventListener('resize', this.matchSize);
            this.matchSize();
        });

        $('#SessionConfigModal').on('shown.bs.modal',
            function () {
                self.matchSize();
            });
        $('#SessionConfigModal').on('hidden.bs.modal',
            function () {
                if (self.openLogin)
                    Login.OpenModal();
            });
        eventBus.$on('init-new-session', () => {
            this.$nextTick(() => this.selectedQuestionCount = this.maxSelectableQuestionCount);
        });
    },
    watch: {
        probabilityRange: function () {
            this.questionFilter.minProbability = this.probabilityRange[0];
            this.questionFilter.maxProbability = this.probabilityRange[1];
            this.loadQuestionCount();
        },
        isTestMode: function (val) {
            this.questionFilter.isInTestMode = val;
            this.isTestModeOrNotLoginIn = val;
            if (val == true) {
                this.randomQuestions = true;
                this.answerHelp = false;
                this.repititions = false;
            } else {
                this.randomQuestions = false;
                this.answerHelp = true;
                this.repititions = true;
            }

        },
        selectedQuestionCount: function (val) {
            this.questionFilter.maxQuestionCount = parseInt(val);
        },
        inWishknowledge: function (val) {
            if (val == true && this.isNotQuestionInWishKnowledge && this.createdByCurrentUser)
                this.allQuestions = true;
            else
                this.allQuestions = false;

            if (this.allQuestions == false) {
                this.loadQuestionCount();
            }
            if (this.inWishknowledge == false &&
                this.createdByCurrentUser == false &&
                this.isNotQuestionInWishKnowledge == false
                || this.inWishknowledge == true &&
                this.createdByCurrentUser == true &&
                this.isNotQuestionInWishKnowledge == true) {
                this.displayMinus = false;
            } else
                this.displayMinus = true;
        },
        createdByCurrentUser: function (val) {
            if (val == true && this.isNotQuestionInWishKnowledge && this.inWishknowledge)
                this.allQuestions = true;
            else
                this.allQuestions = false;

            if (this.allQuestions == false) {
                this.loadQuestionCount();
            }

            if (this.inWishknowledge == false &&
                this.createdByCurrentUser == false &&
                this.isNotQuestionInWishKnowledge == false
                || this.inWishknowledge == true &&
                this.createdByCurrentUser == true &&
                this.isNotQuestionInWishKnowledge == true) {
                this.displayMinus = false;
            } else
                this.displayMinus = true;
        },
        allQuestions: function (val) {

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
        isNotQuestionInWishKnowledge: function (val) {
            if (val == true && this.inWishknowledge && this.createdByCurrentUser)
                this.allQuestions = true;
            else
                this.allQuestions = false;

            if (this.allQuestions == false) {
                this.loadQuestionCount();
            }

            if (this.inWishknowledge == false &&
                this.createdByCurrentUser == false &&
                this.isNotQuestionInWishKnowledge == false
                || this.inWishknowledge == true &&
                this.createdByCurrentUser == true &&
                this.isNotQuestionInWishKnowledge == true) {
                this.displayMinus = false;
            } else
                this.displayMinus = true;
        },
        safeLearningSessionOptions: function (val) {
            this.questionFilter.safeLearningSessionOptions = val;
        },
        randomQuestions: function () {
            this.questionFilter.randomQuestions = this.randomQuestions;
        },
        answerHelp: function (val) {
            this.questionFilter.answerHelp = this.answerHelp;
            if (this.answerHelp == true) {
                this.isTestMode = false;
                this.questionFilter.isInTestMode = false;
            }

        },
        repititions: function () {
            this.questionFilter.repititions = this.repititions;
            if (this.repititions == true) {
                this.isTestMode = false;
                this.questionFilter.isInTestMode = false;
            }
        },
        'questionFilter.maxQuestionCount': function (val) {
            this.maxQuestionCountIsZero = val === 0;
        }
    },
    methods: {
        loadQuestionCount() {
            this.safeQuestionFilter();

            $.ajax({
                url: "/AnswerQuestion/GetQuestionCount/",
                async: false,
                data: {
                    config: this.questionFilter
                },
                type: "POST",
                success: result => {
                    this.maxSelectableQuestionCount = parseInt(result);

                    if (this.isFirstLoad && this.selectedQuestionCount == result || typeof (this.isFirstLoad) == "undefined"
                        || this.selectedQuestionCount > this.maxSelectableQuestionCount
                        || this.selectedQuestionCount == 0)
                        this.selectedQuestionCount = parseInt(result);
                }
            });
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

            this.safeQuestionFilter(); 

            if (this.isMyWorld == 'True')
                this.questionFilter.isNotQuestionInWishKnowledge = false;

            this.answerBody.Loader.loadNewSession(this.questionFilter, true);
            $('#SessionConfigModal').modal('hide');
            this.questionFilter.safeLearningSessionOptions = this.safeLearningSessionOptions = false;
            eventBus.$emit("send-selected-questions", this.selectedQuestionCount);
            this.isFirstLoad = firstLoad;
        },
        matchSize() {
            this.radioHeight = this.$refs.radioSection.clientHeight;
        },
        openModal() {
            this.loadQuestionCount();
            $('#SessionConfigModal').modal('show');
            this.openLogin = false;
            $(".data-btn-login").click();
        },
        goToLogin() {
            this.openLogin = true;
            $('#SessionConfigModal').modal('hide');
        },
        safeQuestionFilter() {
            this.questionFilter.allQuestions = this.allQuestions;
            this.questionFilter.isNotQuestionInWishKnowledge = this.isNotQuestionInWishKnowledge;
            this.questionFilter.inWishknowledge = this.inWishknowledge;
            this.questionFilter.createdByCurrentUser = this.createdByCurrentUser;
            this.questionFilter.safeLearningSessionOptions = this.safeLearningSessionOptions;
            this.questionFilter.isInTestMode = this.isTestMode;
            this.questionFilter.maxQuestionCount = this.selectedQuestionCount;
            this.questionFilter.maxProbability = this.probabilityRange[1] === 100 ? 100 : this.probabilityRange[1];
            this.questionFilter.minProbability = this.probabilityRange[0] === 0 ? 0 : this.probabilityRange[0];
        }
    }
});