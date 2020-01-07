declare var Vue: any;


Vue.component('question-component',
    {
        props: ['questionId', 'questionTitle', 'questionImage', 'knowledgeStatus','isInWishknowledge','url'],
        data() {
            return {
                answer: "",
                extendedAnswer: "",
                categories: [],
                references: [],
                author: [],
                authorImage: "",
                allDataLoaded: false,
                status: "",
                showFullQuestion: false,
                commentCount: 0,
            }
        },

        methods: {
            expandQuestion() {
                this.showFullQuestion = !this.showFullQuestion;
                if (this.allDataLoaded == false) {
                    this.loadQuestionBody();
                    this.loadQuestionDetails(this.questionId);
                }
            },

            loadQuestionBody() {
                $.ajax({
                    url: "/QuestionList/LoadQuestionBody/",
                    data: { questionId: this.questionId },
                    type: "POST",
                    success: data => {
                        console.log(data);
                        this.answer = data.answer;
                        this.extendedAnswer = data.extendedAnswer;
                        this.categories = data.categories;
                        this.references = data.references;
                        this.author = data.author;
                        this.authorImage = data.authorImage;
                        this.allDataLoaded = true;
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
        }

    });

Vue.component('question-list-component', {
    props: ['categoryId','allQuestionCount'],

    data() {
        return {
            pages: 0,
            currentPage: 1,
            itemCountPerPage: 25,
            questions: [],
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
                    itemCount: this.itemCountPerPage,
                    pageNumber: selectedPage,
                },
                type: "POST",
                success: questions => {
                    this.questions = questions;
                }
            });
        },
    },
});
