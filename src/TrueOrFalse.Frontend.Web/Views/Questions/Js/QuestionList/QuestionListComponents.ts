declare var Vue: any;
declare var VueAdsPagination: any;


Vue.component('question-list-component', {
    props: [
        'categoryId',
        'allQuestionCount',
        'isAdmin'],
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
            showLeftPageSelector: false,
            showRightPageSelector: false,
            leftSelectorArray: [],
            rightSelectorArray: [],
            centerArray: [],
            showLeftSelectionDropUp: false,
            showRightSelectionDropUp: false,
        };
    },

    created() {
        eventBus.$on('reload-knowledge-state', () => this.loadQuestions(this.selectedPage));
        eventBus.$on('reload-knowledge-state-per-question', (data) => this.changeQuestionKnowledgeState(data.questionId, data.isInWishknowledge));
    },

    mounted() {
        this.categoryId = $("#hhdCategoryId").val();
        this.initQuestionList();
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
        },
        leftSelectorArray: function() {
            if (this.leftSelectorArray.length >= 2) {
                this.showLeftPageSelector = true;
            }
            else
                this.showLeftPageSelector = false;
        },
        rightSelectorArray: function () {
            if (this.rightSelectorArray.length >= 2) {
                this.showRightPageSelector = true;
            }
            else
                this.showRightPageSelector = false;
        }
    },

    methods: {
        initQuestionList() {
            this.pages = Math.ceil(this.allQuestionCount / this.itemCountPerPage);
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
                    this.selectedPage = selectedPage;
                },
            });
            this.showLeftSelectionDropUp = false;
            this.showRightSelectionDropUp = false;

            this.$nextTick(function () {
                this.setPaginationRanges(selectedPage);
                new Pin(PinType.Question);
            });
        },


        setPaginationRanges(selectedPage) {
            if ((selectedPage - 2) <= 2) {
                this.hideLeftPageSelector = true;
            };
            if ((selectedPage + 2) >= this.pageArray.length) {
                this.hideRightPageSelector = true;
            };

            let leftArray = [];
            let centerArray = [];
            let rightArray = [];

            if (this.pageArray.length >= 8) {

                centerArray = _.range(selectedPage - 2, selectedPage + 3);
                centerArray = centerArray.filter(e => e >= 2 && e <= this.pageArray.length - 1);

                leftArray = _.range(2, centerArray[0]);
                rightArray = _.range(centerArray[centerArray.length - 1] + 1, this.pageArray.length);

                this.centerArray = centerArray;
                this.leftSelectorArray = leftArray;
                this.rightSelectorArray = rightArray;    

            } else {
                this.centerArray = this.pageArray;
            };
        },

        loadPreviousQuestions() {
            if (this.selectedPage != 1)
                this.loadQuestions(this.selectedPage - 1);
        },

        loadNextQuestions() {
            if (this.selectedPage != this.pageArray.length)
                this.loadQuestions(this.selectedPage + 1);
        },

        changeQuestionKnowledgeState(questionId, isInWishknowledge) {
            for (var q in this.questions) {
                if (this.questions[q].Id == questionId) {
                    this.questions[q].IsInWishknowledge = isInWishknowledge;
                    break;
                }
            }
        },
    },
});

Vue.component('question-component',
    {
        props: [
            'questionId',
            'questionTitle',
            'questionImage',
            'knowledgeState',
            'isInWishknowledge',
            'url',
            'hasPersonalAnswer',
            'isAdmin',
            'selectedPage'],
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
                backgroundColor: "",
                correctnessProbability: "",
                showFullQuestion: false,
                commentCount: 0,
                extendedQuestion: "",
                isLoggedIn: IsLoggedIn.Yes,
                pinId: "QuestionListPin-" + this.questionId,
                questionTitleId: "#QuestionTitle-" + this.questionId,
                questionDetailsId: "QuestionDetails-" + this.questionId,
                showQuestionMenu: false,
                isCreator: false,
                editUrl: "",
                historyUrl: "",
                linkToComments: this.url + "#QuestionComments",
                topicTitle: "Thema",
                authorUrl: "",
                questionDetails: "",
            }   
        },

        mounted() {
            this.correctnessProbability = this.knowledgeState + "%";
            this.setKnowledgebarColor(this.knowledgeState);
            this.getWishknowledgePinButton();

            eventBus.$on('reload-question-details', () => {
                if (this.showFullQuestion)
                    this.setQuestionDetails();
            });

        },

        watch: {
            knowledgeState(val) {
                this.setKnowledgebarColor(val);
            },
            selectedPage() {
                this.showFullQuestion = false;
            },
            isInWishknowledge() {
                this.setKnowledgebarColor(this.knowledgeState);
            },
            categories() {
                if (this.categories.length >= 2)
                    this.topicTitle = "Themen";
                else
                    this.topicTitle = "Thema";
            }
        },

        methods: {
            setKnowledgebarColor(val) {
                if (this.isInWishknowledge) {
                    if (this.hasPersonalAnswer) {
                        if (val >= 80)
                            this.backgroundColor = "solid";
                        else if (val < 80 && val >= 50)
                            this.backgroundColor = "shouldConsolidate";
                        else if (val < 50 && val >= 0)
                            this.backgroundColor = "shouldLearn";
                    }
                    else {
                        this.backgroundColor = "inWishknowledge";
                    }
                } else
                    this.backgroundColor = "";

            },

            expandQuestion() {
                this.showFullQuestion = !this.showFullQuestion;
                if (this.allDataLoaded == false) {
                    this.loadQuestionBody();
                    this.loadQuestionDetails();
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
                                this.answer = "<div>" + data.extendedAnswer + "</div>";
                            else
                                this.answer = "<div> Fehler: Keine Antwort! </div>";
                        } else {
                            this.answer = "<div>" + data.answer + "</div>";;
                            if (data.extendedAnswer != null)
                                this.extendedAnswer = "<div>" + data.extendedAnswer + "</div>";;
                        };
                        if (data.categories) {
                            this.categories = data.categories;
                            this.linkToFirstCategory = data.categories[0].linkToCategory;
                        };
                        this.references = data.references;
                        this.author = data.author;
                        this.authorImage = data.authorImage;
                        this.allDataLoaded = true;
                        this.extendedQuestion = data.extendedQuestion;
                        this.commentCount = data.commentCount;
                        this.isCreator = data.isCreator && this.isLoggedIn;
                        this.editUrl = data.editUrl;
                        this.historyUrl = data.historyUrl;
                        this.authorUrl = data.authorUrl;
                    },
                });
            },

            setKnowledgeState() {

            },

            loadQuestionComments() {

            },

            loadQuestionDetails() {
                $.ajax({
                    url: "/AnswerQuestion/RenderUpdatedQuestionDetails",
                    data: {
                        questionId: this.questionId,
                        showCategoryList: false,
                    },
                    type: "POST",
                    success: partialView => {
                        this.questionDetails = partialView;
                        this.setQuestionDetails();
                    }
                });
            },

            setQuestionDetails() {
                var questionDetailsId = "#" + this.questionDetailsId;
                $(questionDetailsId).html(this.questionDetails);
                this.$nextTick(function () {
                    FillSparklineTotals();
                    $('.show-tooltip').tooltip();
                    new Pin(PinType.Question);
                });
            },

            getWishknowledgePinButton() {
                var pinId = "#" + this.pinId;
                $.ajax({
                    url: "/QuestionList/RenderWishknowledgePinButton/",
                    data: {
                        isInWishknowledge: this.isInWishknowledge,
                    },
                    type: "POST",
                    success: partialView => {
                        $(pinId).html(partialView);
                    }
                });
            },

            getEditUrl() {
                $.ajax({
                    url: "/QuestionList/GetEditUrl/",
                    data: {
                        isInWishknowledge: this.isInWishknowledge,
                    },
                    type: "POST",
                    success: url => {
                        this.editUrl = url;
                    }
                });
            },
        }
    });
