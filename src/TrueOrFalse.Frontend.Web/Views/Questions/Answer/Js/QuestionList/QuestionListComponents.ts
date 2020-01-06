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
    props:
    {
        categoryId: Number,
        allQuestionCount: Number,
    },
    data() {
        return {
            pages: 0,
            currentPage: 1,
            itemCountPerPage: 25,
            questions: [],
            allQuestionCountIsBiggerThanItemCount: false,
            hasQuestions: false,
            showFirstPage: true,
        };
    },

    created() {

    },

    mounted() {
        this.categoryId = $("#hhdCategoryId").val();
        this.initQuestionList();
        this.pages = Math.ceil(this.allQuestionCount / this.itemCountPerPage);
        console.log(this.questionsOnFirstPage);
    },

    watch: {
        itemCountPerPage: function (val) {
            this.pages = Math.ceil(this.allQuestionCount / val);
        },
        questions: function() {
            if (this.questions.length > 0)
                this.hasQuestions = true;
        }
    },

    methods: {
        initQuestionList() {
            this.loadQuestions(1);
        },

        loadQuestions(selectedPage) {
            $.ajax({
                url: "/QuestionList/LoadQuestions/",
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
