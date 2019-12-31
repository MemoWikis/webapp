declare var Vue: any;


Vue.component('question-component',
    {
        props: ['questionId', 'questionTitle', 'questionImage', 'knowledgeStatus','isInWishknowledge'],
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

Vue.component('question-list-component', {
    props: ['categoryId', 'allQuestionCount'],
    data() {
        return {
            pages: {
                type: Number,
                required: false,
            },
            currentPage: 1,
            itemCountPerPage: 25,
            questions: [],
            allQuestionCountIsBiggerThanItemCount: false,
        };
    },

    created() {
        this.categoryId = $("#hhdCategoryId").val();
        this.initQuestionList();
    },

    mounted() {
    },

    watch: {
        itemCountPerPage: function (val) {
            if (this.allQuestionCount > val)
                this.allQuestionCountIsBiggerThanItemCount = true;
            else
                this.allQuestionCountIsBiggerThanItemCount = false;
        }
    },

    methods: {
        initQuestionList() {
            this.getPageCount();
            this.loadQuestions(1);
        },

        getPageCount() {
            $.ajax({
                url: "/Questions/QuestionList/GetPageCount/",
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
                url: "/Questions/QuestionList/LoadQuestions/",
                data: {
                    categoryId: this.categoryId,
                    pageNumber: this.itemCountPerPage,
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
