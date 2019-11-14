declare var Vue: any;
declare var VueSlider: any;

new Vue({
    el: '#SessionConfigApp',
    components: {
        VueSlider: window['vue-slider-component']
    },

    data() {
        return {
            mode: 'Learning',
            title: 'Lernsitzung',
            answerBody: new AnswerBody(),
            probabilityRange: [0, 100],
            questionFilter: {
                minProbability: 0,
                maxProbability: 100,
                maxQuestionCount: 10,
                questionsInWishknowledge: false,
                questionOrder: 0,
            },
            isLoggedIn: true,
            maxSelectableQuestionCount: 50,
            selectedQuestionCount: 10,
            questionsInWishknowledge: "False",
            percentages: '{value}%',
        };
    },

    mounted() {
        if (NotLoggedIn.Yes()) {
            this.mode = 'Test';
            this.title = 'Testsitzung';
            this.isLoggedIn = false;
        }

        this.loadQuestionCount();
    },

    computed: {
        if ()
    },

    watch: {
        probabilityRange: function() {
            this.questionFilter.minProbability = this.probabilityRange[0];
            this.questionFilter.maxProbability = this.probabilityRange[1];
            this.loadQuestionCount();
        },

        questionsInWishknowledge: function (val) {
            if (val == "True")
                this.questionFilter.questionsInWishknowledge = true;
            else
                this.questionFilter.questionsInWishknowledge = false;
            this.loadQuestionCount();
        },

        selectedQuestionCount: function(val) {
            this.questionFilter.maxQuestionCount = parseInt(val); 
        }
    },

    methods: {
        loadQuestionCount() {
            $.ajax({
                url: "/AnswerQuestion/GetQuestionCount/",
                data: { categoryId: $('#hhdCategoryId').val(), questionFilter: this.questionFilter },
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
                maxQuestionCount: 10,
                questionsInWishknowledge: false,
                questionOrder: 0,
            };
        },

        loadNewSession(val) {
            this.resetQuestionFilter();

            if (val == "random")
                this.questionFilter.questionOrder = 0;
            if (val == "highProbability")
                this.questionFilter.questionOrder = 1;
            if (val == "lowProbability")
                this.questionFilter.questionOrder = 2;


            this.loadCustomSession();
        },

        loadCustomSession() {
            if (this.questionFilter.maxQuestionCount == 0)
                return;

            this.answerBody.Loader.loadNewSession(this.mode, this.questionFilter);
            this.questionFilter.questionOrder = 0;
            $('#SessionConfigModal').modal('hide');
        }
    }
});