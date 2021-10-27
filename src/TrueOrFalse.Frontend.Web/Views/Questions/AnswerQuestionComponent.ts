interface AnswerQuestion {
    CategoryId: Number,
    Title: String,
    ChildCategoryIds: Array<Number>,
};

var AnswerQuestionComponent = Vue.component('AnswerQuestion', {
    props: {
    },

    data() {
        return {
        };
    },

    created() {
    },

    mounted() { },
    methods: {
        showLoginModal() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("AnswerQuestion-Comment");
                return;
            }
        }
    },
});