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
                mode: "Learning",
                userIsAuthor: false,
                allQuestions: false,
                isNotQuestionInWishKnowledge: false
            },
            isLoggedIn: true,
            maxSelectableQuestionCount: 50,
            selectedQuestionCount: 10,
            questionsInWishknowledge: "False",
            percentages: '{value}%',
            maxQuestionCountIsZero: false,
            isTestMode: false,
            isHoveringOptions: false,
            radioHeight: 0,
            radioWidth: 0,
            openLogin: false,
            userIsAuthor: false,
            allQuestions: false,
            isNotQuestionInWishKnowledge: false
        };
    },

    mounted() {
        var self = this;

        if (NotLoggedIn.Yes()) {
            this.title = 'Test';
            this.isLoggedIn = false;
            this.questionFilter.mode = 'Test';
        };

        this.loadQuestionCount();

        this.$nextTick(function() {
            window.addEventListener('resize', this.matchSize);
            self.matchSize();
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
            this.questionFilter.questionsInWishknowledge = val;
            if (val) {
                this.isNotQuestionInWishKnowledge = false;
                this.allQuestions = false;
                this.loadQuestionCount();
                

            }
        },
        userIsAuthor: function (val) {
            this.questionFilter.userIsAuthor = val;
            if (val) {
                this.allQuestions = false;
                this.loadQuestionCount();
            }
        },
        allQuestions: function (val) {
            this.questionFilter.allQuestions = val;
            if (val) {
                this.questionsInWishknowledge = false;
                this.userIsAuthor = false;
                this.isNotQuestionInWishKnowledge = false;
                this.loadQuestionCount();
            }
        },

        isNotQuestionInWishKnowledge: function (val) {
            this.questionFilter.isNotQuestionInWishKnowledge = val;
            if (val) {
                this.questionsInWishknowledge = false;
                this.allQuestions = false;
                this.loadQuestionCount();
            }
           
        },

     

        'questionFilter.maxQuestionCount': function(val) {
            this.maxQuestionCountIsZero = val === 0;
        },
    },

    methods: {
        loadQuestionCount() {
            $.ajax({
                url: "/AnswerQuestion/GetQuestionCount/",
                data: {
                    categoryId: $('#hhdCategoryId').val(),
                    questionFilter: this.questionFilter
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
                questionsInWishknowledge: false,
                questionOrder: 0,
                mode: "Learning",
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

            this.answerBody.Loader.loadNewSession(this.questionFilter, true);
            $('#SessionConfigModal').modal('hide');

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
        }
    }
});