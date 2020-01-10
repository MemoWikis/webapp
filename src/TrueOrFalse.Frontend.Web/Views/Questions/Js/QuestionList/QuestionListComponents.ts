declare var Vue: any;


Vue.component('question-component',
    {
        props: ['questionId', 'questionTitle', 'questionImage', 'knowledgeState', 'isInWishknowledge', 'url'],
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
                backgroundColor: { backgroundColor: '' },
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

        watch: {
            correctnessProbability(val) {
                if (this.isInWishknowledge) {
                    if (val >= 80)
                        this.backgroundColor = { backgroundColor: "#AFD534" };
                    else if (val < 80 && val >= 50)
                        this.backgroundColor = { backgroundColor: "#AFD534" };
                    else if (val < 50 && val >= 0)
                        this.backgroundColor = { backgroundColor: "#FFA07A" };
                } else
                    this.backgroundColor = { backgroundColor: "#949494" };
            }
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
                        if (data.answer == null || data.answer.length <= 0) {
                            if (data.extendedAnswer && data.extendedAnswer > 0)
                                this.answer = data.extendedAnswer;
                            else
                                this.answer = "Fehler: Keine Antwort!";
                        } else {
                            this.answer = data.answer;
                            this.extendedAnswer = data.extendedAnswer;
                        }
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
            pageArray: [],
            questionText: "Fragen",
        };
    },

    created() {

    },

    mounted() {
        this.categoryId = $("#hhdCategoryId").val();
        this.initQuestionList();
        this.pages = Math.ceil(this.allQuestionCount / this.itemCountPerPage);
    },

    watch: {
        itemCountPerPage: function (val) {
            this.pages = Math.ceil(this.allQuestionCount / val);
        },
        questions: function() {
            if (this.questions.length > 0)
                this.hasQuestions = true;
            if (this.questions.length == 1)
                this.questionText = "Frage";
        },
        selectedPage: function(val) {
            this.loadQuestions(val);
        },
        pages: function (val) {
            let newArray = [];
            let currentNumber = 1;
            for (let i = 0; currentNumber < val + 1; i++) {
                newArray.push(currentNumber);
                currentNumber = currentNumber + 1;
            }
            this.pageArray = newArray;
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
