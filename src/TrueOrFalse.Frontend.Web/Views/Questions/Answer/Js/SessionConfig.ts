declare var Vue: any;

new Vue({
    el: '#CategoryTabsApp',
    data() {
        return {
            mode: 'Testing',
            questionList: null,
            minProbability: 0,
            maxProbability: 100,
            maxQuestionCount: 50,
            questionInWishknowledge: false,
            questionOrder: 0,
            answerBodyLoader: AnswerBodyLoader,
            isLoggedIn: IsLoggedIn,
            title: 'Testsitzung',
            questionCountInCategory: 0,
        };
    },

    mounted() {
        if (IsLoggedIn) {
            this.mode = 'Learning';
            this.title = 'Lernsitzung';
        }

        this.loadQuestionList();
    },

    methods: {
        loadQuestionList() {
            $.ajax({
                url: "/AnswerQuestion/GetQuestionList/",
                dataType: "json",
                contentType: "int",
                data: $('#hhdCategoryId').val(),
                type: "POST",
                success: result => {
                    this.questionList = JSON.parse(result);
                    this.questionCountInCategory = this.questionList.length;
                }
            });
        },

        loadNewSession() {
            var questionFilter = {
                minProbability: this.minProbability,
                maxProbability: this.maxProbability,
                maxQuestionCount: this.maxQuestionCount,
                questionInWishknowledge: this.questionInWishknowledge,
                questionOrder: this.questionOrder,
            }

            this._answerBodyLoader.loadNewSession(this.mode, questionFilter);
        }
    }
});