declare var Vue: any;


Vue.component('question-component',
    {
        props: ['questionId', 'questionTitle', 'questionImage', 'knowledgeState','isInWishknowledge','url'],
        data() {
            return {
                answer: "",
                extendedAnswer: "",
                categories: [],
                references: [],
                author: "",
                authorId: "",
                authorImage: "",
                allDataLoaded: false,
                state: "",
                correctnessProbability: "",
                showFullQuestion: false,
                commentCount: 0,
                questionDetails: {
                    extendedQuestion: "",
                    views: 0,
                    totalAnswers: 0,
                    totalCorrectAnswers: 0,
                    personalAnswers: 0,
                    personalCorrectAnswer: 0,
                },
                isLoggedIn: IsLoggedIn.Yes,
            }
        },

        mounted() {
            this.correctnessProbability = this.knowledgeState + "%";
        },

        methods: {
            expandQuestion() {
                this.showFullQuestion = !this.showFullQuestion;
                if (this.allDataLoaded == false) {
                    this.loadQuestionBody();
                }
            },

            loadQuestionBody() {
                $.ajax({
                    url: "/QuestionList/LoadQuestionBody/",
                    data: { questionId: this.questionId },
                    type: "POST",
                    success: data => {
                        this.answer = data.answer;
                        this.extendedAnswer = data.extendedAnswer;
                        this.categories = data.categories;
                        this.references = data.references;
                        this.author = data.author;
                        this.authorImage = data.authorImage;
                        this.allDataLoaded = true;
                        this.questionDetails = {
                            extendedQuestion: data.questionDetails.extendedQuestion,
                            views: data.questionDetails.views,
                            totalAnswers: data.questionDetails.totalAnswers,
                            totalCorrectAnswers: data.questionDetails.totalCorrectAnswers,
                            personalAnswers: data.questionDetails.personalAnswers,
                            personalCorrectAnswer: data.questionDetails.personalCorrectAnswer,
                        }
                    },
                });
            },

            setKnowledgeState() {

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
            selectedPage: 1,
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
        },
        selectedPage: function(val) {
            this.loadQuestions(val);
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
