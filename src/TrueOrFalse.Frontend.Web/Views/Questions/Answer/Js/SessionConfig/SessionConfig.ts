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
            maxQuestionCount: 50,
            title: 'Lernsitzung',
            questionCountInCategory: 0,
            answerBody: new AnswerBody(),
            probabilityRange: [0, 100],
            questionFilter: {
                minProbability: 0,
                maxProbability: 100,
                maxQuestionCount: 50,
                questionInWishknowledge: false,
                questionOrder: 0,
            },
            isLoggedIn: true,
        };
    },

    mounted() {
        if (NotLoggedIn) {
            this.mode = 'Test';
            this.title = 'Testsitzung';
            this.isLoggedIn = false;
        }

        this.loadQuestionCount();
    },

    watch: {
        probabilityRange: function() {
            this.questionFilter.minProbability = Math.min(this.probabilityRange);
            this.questionFilter.maxProbability = Math.max(this.probabilityRange);
        },
    },

    methods: {
        loadQuestionCount() {
            $.ajax({
                url: "/AnswerQuestion/GetQuestionCount/",
                data: { categoryId: $('#hhdCategoryId').val(), minProbability: this.questionFilter.minProbability, maxProbability: this.questionFilter.maxProbability },
                type: "POST",
                success: result => {
                    if (result > 50)
                        this.questionCountInCategory = 50;
                    else
                        this.questionCountInCategory = result;
                }
            });
        },

        loadNewSession() {

            this.answerBody.Loader.loadNewSession(this.mode, this.questionFilter);
        }
    }
});