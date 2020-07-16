declare var Vue: any;
declare var VueSlider: any;

var vue = new Vue({
    el: '#SessionConfigApp',
    components: {
        VueSlider: window['vue-slider-component']
    },

    data() {
        return {
            title: 'Lernen',
            answerBody: new AnswerBody(),
            probabilityRange: [0, 100],
            questionFilter: {
                minProbability: 0,
                maxProbability: 100,
                maxQuestionCount: 10,
                questionsInWishknowledge: false,
                questionOrder: 0,
                userIsAuthor: false,
                allQuestions: false,
                isNotQuestionInWishKnowledge: false,
                isInTestMode: false,
                safeLearningSessionOptions: false,
                categoryId: $('#hhdCategoryId').val()
            },
            isLoggedIn: true,
            maxSelectableQuestionCount: 50,
            selectedQuestionCount: 10,
            questionsInWishknowledge: false,
            percentages: '{value}%',
            maxQuestionCountIsZero: false,
            isTestMode: false,
            isHoveringOptions: false,
            radioHeight: 0,
            radioWidth: 0,
            openLogin: false,
            userIsAuthor: false,
            allQuestions: false,
            isNotQuestionInWishKnowledge: false,
            safeLearningSessionOptions: false,
            displayNone: true
        };
    },

    mounted() {
        var self = this;

        if (NotLoggedIn.Yes()) {
            this.title = 'Test';
            this.isLoggedIn = false;
        };

        this.$nextTick(function() {
            window.addEventListener('resize', this.matchSize);
            this.matchSize();
        });

        $('#SessionConfigModal').on('shown.bs.modal', function () {
            self.matchSize();
        });
        $('#SessionConfigModal').on('hidden.bs.modal', function () {
            if (self.openLogin)
                Login.OpenModal();
        });
    },

    watch: {
        probabilityRange: function() {
            this.questionFilter.minProbability = this.probabilityRange[0];
            this.questionFilter.maxProbability = this.probabilityRange[1];
            this.loadQuestionCount();
        },

        selectedQuestionCount: function (val) {
            this.questionFilter.maxQuestionCount = parseInt(val);
        },
        questionsInWishknowledge: function (val) {
            
            if (val) {
                this.isNotQuestionInWishKnowledge = false;
                this.allQuestions = false;
            }
            this.loadQuestionCount();
        },
        userIsAuthor: function (val) {
           
            if (val) {
                this.allQuestions = false;
            }
            this.loadQuestionCount();
        },
        allQuestions: function (val) {
            
            if (val) {
                this.questionsInWishknowledge = false;
                this.userIsAuthor = false;
                this.isNotQuestionInWishKnowledge = false;
            }
            this.loadQuestionCount();
        },
        isNotQuestionInWishKnowledge: function (val) {
            
            if (val) {
                this.questionsInWishknowledge = false;
                this.allQuestions = false;
            }
            this.loadQuestionCount();
        },
        safeLearningSessionOptions: function (val) {
            this.questionFilter.safeLearningSessionOptions = val;
        },
        'questionFilter.maxQuestionCount': function(val) {
            this.maxQuestionCountIsZero = val === 0;
        }
    },

    methods: {
        loadQuestionCount() {
            this.safeQuestionFilter();

            $.ajax({
                url: "/AnswerQuestion/GetQuestionCount/",
                data: {
                    config: this.questionFilter
                },
                type: "POST",
                success: result => {
                    result = parseInt(result);
                    if (result > 50) {
                        this.maxSelectableQuestionCount = 50;
                        if (this.selectedQuestionCount > 50)
                            this.selectedQuestionCount = 50;
                    } else {
                        this.maxSelectableQuestionCount = result;
                        if (this.selectedQuestionCount > result)
                            this.selectedQuestionCount = result;
                    }
                }
            });
        },

        resetQuestionFilter() {
            this.questionFilter = {
                minProbability: 0,
                maxProbability: 100,
                maxQuestionCount: this.isLoggedIn ? 10 : 5,
                questionOrder: 0,
                questionsInWishknowledge: false,
                isNotQuestionInWishKnowledge: false,
                userIsAuthor: false,
                allQuestions: false
            };
        },

        async loadNewSession(val) {
            await this.resetQuestionFilter();

            if (val == "random")
                this.questionFilter.questionOrder = 0;
            if (val == "highProbability")
                this.questionFilter.questionOrder = 1;
            if (val == "lowProbability")
                this.questionFilter.questionOrder = 2;

            this.loadCustomSession();
        },

        loadCustomSession() {
            if (this.maxQuestionCountIsZero)
                return;

            this.safeQuestionFilter();
            this.answerBody.Loader.loadNewSession(this.questionFilter, true);
            $('#SessionConfigModal').modal('hide');
            this.questionFilter.safeLearningSessionOptions= this.safeLearningSessionOptions = false;
        },

        matchSize() {
            this.radioHeight = this.$refs.radioSection.clientHeight;
            this.radioWidth = this.$refs.radioSection.clientWidth;
        },

        openModal() {
            this.loadQuestionCount();
            $('#SessionConfigModal').modal('show');
            this.openLogin = false;
        },

        goToLogin() {
            this.openLogin = true;
            $('#SessionConfigModal').modal('hide');
        }, 
        safeQuestionFilter() {
            this.questionFilter.allQuestions = this.allQuestions;
            this.questionFilter.isNotQuestionInWishKnowledge = this.isNotQuestionInWishKnowledge;
            this.questionFilter.questionsInWishknowledge = this.questionsInWishknowledge;
            this.questionFilter.userIsAuthor = this.userIsAuthor;
            this.questionFilter.safeLearningSessionOptions = this.safeLearningSessionOptions; 
            this.questionFilter.isInTestMode = this.isInTestMode;
            this.questionFilter.maxQuestionCount = this.selectedQuestionCount;
            this.questionFilter.maxProbability = this.probabilityRange[1] === 100  ? 100 : this.probabilityRange[1];
            this.questionFilter.minProbability = this.probabilityRange[0] === 0 ? 0 : this.probabilityRange[0];
        }
    }
});