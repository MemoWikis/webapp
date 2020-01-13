declare var Vue: any;
declare var VueAdsPagination: any;


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

Vue.component('question-component',
    {
        props: ['questionId', 'questionTitle', 'questionImage', 'knowledgeState', 'isInWishknowledge', 'url', 'hasPersonalAnswer'],
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
                    totalWrongAnswers: 0,
                    personalAnswers: 0,
                    personalCorrectAnswers: 0,
                    inWishknowledgeCount: 0,
                },
                isLoggedIn: IsLoggedIn.Yes,
                pinId: "QuestionListPin-" + this.questionId,
                innerPinId: "InnerQuestionListPin-" + this.questionId,
                linkToFirstCategory: "",
                questionTitleId: "#QuestionTitle-" + this.questionId,
            }
        },

        mounted() {
            this.correctnessProbability = this.knowledgeState + "%";
            this.setKnowledgebarColor(this.knowledgeState);
            this.getWishknowledgePinButton();
        },

        watch: {
            knowledgeState(val) {
                this.setKnowledgebarColor(val);
            },
        },

        methods: {
            setKnowledgebarColor(val) {
                if (this.isInWishknowledge) {
                    if (this.hasPersonalAnswer) {
                        if (val >= 80)
                            this.backgroundColor = { backgroundColor: "#AFD534" };
                        else if (val < 80 && val >= 50)
                            this.backgroundColor = { backgroundColor: "#AFD534" };
                        else if (val < 50 && val >= 0)
                            this.backgroundColor = { backgroundColor: "#FFA07A" };
                    }
                    else {
                        this.backgroundColor = { backgroundColor: "#949494" };
                    }
                } else
                    this.backgroundColor = { backgroundColor: "#D6D6D6" };
            },

            expandQuestion() {
                this.showFullQuestion = !this.showFullQuestion;
                if (this.allDataLoaded == false) {
                    this.loadQuestionBody();
                };
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
                        };
                        if (data.categories) {
                            this.categories = data.categories;
                            this.linkToFirstCategory = data.categories[0].linkToCategory;
                        };
                        this.references = data.references;
                        this.author = data.author;
                        this.authorImage = data.authorImage;
                        this.allDataLoaded = true;
                        this.questionDetails = {
                            extendedQuestion: data.questionDetails.extendedQuestion,
                            views: data.questionDetails.views,
                            totalAnswers: data.questionDetails.totalAnswers,
                            totalCorrectAnswers: data.questionDetails.totalCorrectAnswers,
                            totalWrongAnswers: data.questionDetails.totalAnswers - data.questionDetails.totalCorrectAnswers,
                            personalAnswers: data.questionDetails.personalAnswers,
                            personalCorrectAnswers: data.questionDetails.personalCorrectAnswer,
                            personalWrongAnswers: data.questionDetails.personalAnswers - data.questionDetails.personalCorrectAnswer,
                            inWishknowledgeCount: data.questionDetails.inWishknowledgeCount,
                        };
                        this.$nextTick(function() {
                            FillSparklineTotals();
                        });
                    },
                });
            },

            setKnowledgeState() {

            },

            loadQuestionComments() {

            },

            getWishknowledgePinButton() {
                var pinId = "#" + this.pinId;
                var innerPinId = "#" + this.innerPinId;
                $.ajax({
                    url: "/QuestionList/RenderWishknowledgePinButton/",
                    data: {
                        isInWishknowledge: this.isInWishknowledge,
                    },
                    type: "POST",
                    success: partialView => {
                        $(pinId).html(partialView);
                        $(innerPinId).html(partialView);
                    }
                });
            },
        }

    });

Vue.component('vue-ads-pagination', VueAdsPagination);

