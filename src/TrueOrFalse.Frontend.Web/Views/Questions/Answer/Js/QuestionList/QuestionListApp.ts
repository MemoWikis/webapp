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
            expandedQuestions: {
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
    },
});

Vue.Component('question-component',
    {
        props: {
            questionId: Number,
            questionTitle: String,
            questionImage: String,
            knowledgeStatus: Number,
            isInWishknowledge: Boolean,
        },
        data() {
            return {
                answer: "",
                extendedAnswer: "",
                categories: [],
                references: [],
                author: [],
                authorImage: "",
                allDataLoaded: false,
            }
        },

        async expandQuestion() {
            if (this.allDataLoaded == false) {
                await this.loadQuestionBody(this.questionId);
                this.loadQuestionDetails(this.questionId);
            }
        },

        loadQuestionBody(questionId) {
            $.ajax({
                url: "",
                data: { questionId: questionId },
                type: "POST",
                success: data => {
                    this.answer = data.Answer;
                    this.extendedAnswer = data.extendedAnswer;
                    this.categories = data.categories;
                    this.references = data.references;
                    this.author = data.author;
                    this.authorImage = data.authorImage;
                },
            });
        },

        loadQuestionDetails() {
            $.ajax({
                url: '/AnswerQuestion/RenderUpdatedQuestionDetails',
                data: { questionId: this.questionId },
                type: "POST",
                success: data => {
                    $(".questionDetails[data-question-id='" + this.questionId + "']").html(data);
                    FillSparklineTotals();
                    $('.show-tooltip').tooltip();
                }
            });
        },

        loadQuestionComments() {

        },
    });