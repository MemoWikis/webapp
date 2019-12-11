declare var Vue: any;

new Vue({
    el: '#QuestionListApp',
});

Vue.Component('question-list-component', {
    props: {
        categoryId: Number,
        allQuestionCount: Number,
    },
    data() {
        return {
            pages: {
                type: Number,
                required: false,
            },
            currentPage: {
                type: Number,
                required: false,
            },
            itemCountPerPage: 25,
            allQuestionCount: 0,
            questions: {
                type: Array,
                required: false,
            },
        };
    },

    created() {
        this.categoryId = $("#hhdCategoryId").val();
    },

    mounted() {
        this.initQuestionList();
    },

    methods: {
        initQuestionList() {
            this.getPageCount();
            this.loadQuestions(1);
        },

        getPageCount() {
            $.ajax({
                url: "",
                data: {
                    categoryId: this.categoryId,
                    itemCountPerPage: this.itemCountPerPage,
                },
                type: "POST",
                success: count => {
                    this.pages = count;
                }
            });
        },

        loadQuestions(selectedPage) {
            $.ajax({
                url: "",
                data: {
                    categoryId: this.categoryId,
                    selectedPage: selectedPage,
                },
                type: "POST",
                success: questions => {
                    this.questions = questions;
                }
            });
        },

        expandQuestion(index) {
            var questionToExpand = this.questions[index];
            var questionId = questionToExpand.Id;
            this.loadQuestionBody(questionToExpand);
            this.loadQuestionDetails(questionId);
        },

        loadQuestionBody(questionToExpand) {
            $.ajax({
                url: "",
                data: { questionId: questionToExpand.Id },
                type: "POST",
                success: data => {
                    questionToExpand.Answer = data.Answer;
                    questionToExpand.ExtendedAnswer = data.ExtendedAnswer;
                    questionToExpand.Categories = data.CategoryList;
                    questionToExpand.Author = data.AuthorList;
                    questionToExpand.Sources = data.SourceList;
                }
            });
        },

        loadQuestionDetails(questionId) {
            $.ajax({
                url: '/AnswerQuestion/RenderUpdatedQuestionDetails',
                data: { questionId: questionId },
                type: "POST",
                success: data => {
                    $(".questionDetails[data-question-id='" + questionId + "']").html(data);
                    FillSparklineTotals();
                    $('.show-tooltip').tooltip();
                }
            });
        },

        loadQuestionComments() {

        },
    },
});