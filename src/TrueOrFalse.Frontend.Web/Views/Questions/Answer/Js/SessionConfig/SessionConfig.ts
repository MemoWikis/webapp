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
                maxQuestionCount: 50,
                questionInWishknowledge: false,
                questionOrder: 0,
            },
            isLoggedIn: true,
            maxSelectableQuestionCount: 50,
            questionInWishknowledge: false,
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

    watch: {
        probabilityRange: function() {
            this.questionFilter.minProbability = this.probabilityRange[0];
            this.questionFilter.maxProbability = this.probabilityRange[1];
            this.loadQuestionCount();
        },

        questionInWishknowledge: function(val) {
            this.questionFilter.questionInWishknowledge = val;
            this.loadQuestionCount();
        }
    },

    methods: {
        loadQuestionCount() {
            $.ajax({
                url: "/AnswerQuestion/GetQuestionCount/",
                data: { categoryId: $('#hhdCategoryId').val(), minProbability: this.questionFilter.minProbability, maxProbability: this.questionFilter.maxProbability },
                type: "POST",
                success: result => {
                    if (result > 50) {
                        this.maxSelectableQuestionCount = 50;
                        this.questionFilter.maxQuestionCount = 50;
                    } else {
                        this.questionFilter.maxQuestionCount = result;
                        this.maxSelectableQuestionCount = result;
                    }
                }
            });
        },

        loadNewSession() {

            this.answerBody.Loader.loadNewSession(this.mode, this.questionFilter);
        }
    }
});