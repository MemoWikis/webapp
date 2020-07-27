declare var Vue: any;
declare var VueAdsPagination: any;
declare var VueSlider: any;

Vue.component('session-config-component', {
    components: {
        VueSlider: window['vue-slider-component']
    },

    data() {
        return {
            title: 'Lernen',
            answerBody: new AnswerBody(),
            probabilityRange: [0, 100],
            questionFilter: {
                minProbability: 0,
                maxProbability: 100,
                maxQuestionCount: 10,
                inWishknowledge: false,
                createdByCurrentUser: false,
                allQuestions: false,
                isNotQuestionInWishKnowledge: false,
                isInTestMode: false,
                safeLearningSessionOptions: false,
                categoryId: $('#hhdCategoryId').val(),
                answerHelp: true,
                repititions: true,
                randomQuestions: false
            },
            isLoggedIn: true,
            maxSelectableQuestionCount: 50,
            selectedQuestionCount: 10,
            inWishknowledge: false,
            percentages: '{value}%',
            maxQuestionCountIsZero: false,
            isTestMode: false,
            isHoveringOptions: false,
            radioHeight: 0,
            radioWidth: 0,
            openLogin: false,
            createdByCurrentUser: false,
            allQuestions: false,
            isNotQuestionInWishKnowledge: false,
            safeLearningSessionOptions: false,
            displayNone: true,
            randomQuestions: false,
            answerHelp: true,
            repititions: true
        };
    },

    mounted() {
        var self = this;

        if (NotLoggedIn.Yes()) {
            this.title = 'Test';
            this.isLoggedIn = false;
        };

        this.$nextTick(function () {
            window.addEventListener('resize', this.matchSize);
            this.matchSize();
        });

        $('#SessionConfigModal').on('shown.bs.modal', function () {
            self.matchSize();
        });
        $('#SessionConfigModal').on('hidden.bs.modal', function () {
            if (self.openLogin)
                Login.OpenModal();
        });
    },

    watch: {
        probabilityRange: function () {
            this.questionFilter.minProbability = this.probabilityRange[0];
            this.questionFilter.maxProbability = this.probabilityRange[1];
            this.loadQuestionCount();
        },
        selectedQuestionCount: function (val) {
            this.questionFilter.maxQuestionCount = parseInt(val);
        },
        questionsInWishknowledge: function (val) {

            if (val) {
                this.isNotQuestionInWishKnowledge = false;
                this.allQuestions = false;
            }
            this.loadQuestionCount();
        },
        createdByCurrentUser: function (val) {
            if (val) {
                this.allQuestions = false;
            }
            this.loadQuestionCount();
        },
        allQuestions: function (val) {

            if (val) {
                this.inWishknowledge = false;
                this.createdByCurrentUser = false;
                this.isNotQuestionInWishKnowledge = false;
            }
            this.loadQuestionCount();
        },
        isNotQuestionInWishKnowledge: function (val) {

            if (val) {
                this.inWishknowledge = false;
                this.allQuestions = false;
            }
            this.loadQuestionCount();
        },
        safeLearningSessionOptions: function (val) {
            this.questionFilter.safeLearningSessionOptions = val;
        },
        randomQuestions: function () {
            this.questionFilter.randomQuestions = this.randomQuestions;
        },
        answerHelp: function () {
            this.questionFilter.answerHelp = this.answerHelp;
        },
        repititions: function () {
            this.questionFilter.repititions = this.repititions;
        },
        'questionFilter.maxQuestionCount': function (val) {
            this.maxQuestionCountIsZero = val === 0;
        }
    },

    methods: {
        loadQuestionCount() {
            this.safeQuestionFilter();

            $.ajax({
                url: "/AnswerQuestion/GetQuestionCount/",
                data: {
                    config: this.questionFilter
                },
                type: "POST",
                success: result => {
                    result = parseInt(result);
                    if (result > 50) {
                        this.maxSelectableQuestionCount = 50;
                        if (this.selectedQuestionCount > 50)
                            this.selectedQuestionCount = 50;
                    } else {
                        this.maxSelectableQuestionCount = result;
                        if (this.selectedQuestionCount > result)
                            this.selectedQuestionCount = result;
                    }
                }
            });
        },

        resetQuestionFilter() {
            this.questionFilter = {
                minProbability: 0,
                maxProbability: 100,
                maxQuestionCount: this.isLoggedIn ? 10 : 5,
                questionOrder: 0,
                questionsInWishknowledge: false,
                isNotQuestionInWishKnowledge: false,
                userIsAuthor: false,
                allQuestions: false
            };
        },
        loadCustomSession() {
            if (this.maxQuestionCountIsZero)
                return;

            this.safeQuestionFilter();
            this.answerBody.Loader.loadNewSession(this.questionFilter, true);
            $('#SessionConfigModal').modal('hide');
            this.questionFilter.safeLearningSessionOptions = this.safeLearningSessionOptions = false;
        },

        matchSize() {
            this.radioHeight = this.$refs.radioSection.clientHeight;
            this.radioWidth = this.$refs.radioSection.clientWidth;
        },

        openModal() {
            this.loadQuestionCount();
            $('#SessionConfigModal').modal('show');
            this.openLogin = false;
        },

        goToLogin() {
            this.openLogin = true;
            $('#SessionConfigModal').modal('hide');
        },
        safeQuestionFilter() {
            this.questionFilter.allQuestions = this.allQuestions;
            this.questionFilter.isNotQuestionInWishKnowledge = this.isNotQuestionInWishKnowledge;
            this.questionFilter.inWishknowledge = this.inWishknowledge;
            this.questionFilter.createdByCurrentUser = this.createdByCurrentUser;
            this.questionFilter.safeLearningSessionOptions = this.safeLearningSessionOptions;
            this.questionFilter.isInTestMode = this.isInTestMode;
            this.questionFilter.maxQuestionCount = this.selectedQuestionCount;
            this.questionFilter.maxProbability = this.probabilityRange[1] === 100 ? 100 : this.probabilityRange[1];
            this.questionFilter.minProbability = this.probabilityRange[0] === 0 ? 0 : this.probabilityRange[0];
        }
    }
});

Vue.component('sort-list',{
    data() {
        return {
            activeSortOrder: 'Alphabetisch',
            questionOrder: "",
            sortOrdersForQuestionsList: [
                'Antwortwahrscheinlichkeit',
                'Wunschwissen',
                'Zufällig',
                'Alphabetisch'
            ],
        }
    },
    methods: {
        changeSortOrder(sortOrder) {
            this.activeSortOrder = sortOrder;
            this.$emit("send-question-order", sortOrder );
        },
    }
});

Vue.component('question-list-component', {
    components: {
        VueSlider: window['vue-slider-component']
    },
    props: [
        'categoryId',
        'allQuestionCount',
        'isAdmin',
        'isQuestionListToShow',
        'questionOrder'],
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
            pageIsLoading: false,
        };
    },

    created() {
        eventBus.$on('reload-knowledge-state', () => this.loadQuestions(this.selectedPage));
        eventBus.$on('reload-wishknowledge-state-per-question', (data) => this.changeQuestionWishknowledgeState(data.questionId, data.isInWishknowledge));
        eventBus.$on('reload-correctnessprobability-for-question', (id) => this.getUpdatedCorrectnessProbability(id));
    },

    mounted() {
        this.categoryId = $("#hhdCategoryId").val();
        this.initQuestionList(-1);
    },

    watch: {
        itemCountPerPage: function (val) {
            this.pages = Math.ceil(this.allQuestionCount / val);
        },
        questionOrder: function(val) {
            switch (val) {
                case "Antwortwahrscheinlichkeit":
                    this.initQuestionList(0);
                    break;
                case "Wunschwissen":
                    this.initQuestionList(1);
                    break;
                case "Zufällig":
                    this.initQuestionList(2);
                    break;
                case "Alphabetisch":
                    this.initQuestionList(3);
                    break;
                default:
            }
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
            let startNumber = 1;
            for (let i = 0; startNumber < val + 1; i++) {
                newArray.push(startNumber);
                startNumber = startNumber + 1;
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
        initQuestionList(questionsSort: number) {
            this.pages = Math.ceil(this.allQuestionCount / this.itemCountPerPage);
            this.loadQuestions(this.selectedPage ,questionsSort);
        },

        loadQuestions(selectedPage) {
            this.pageIsLoading = true;
            $.ajax({
                url: "/QuestionList/LoadQuestions/",
                data: {
                    itemCount: this.itemCountPerPage,
                    pageNumber: selectedPage,
                },
                type: "POST",
                success: questions => {
                    this.questions = questions;
                    this.selectedPage = selectedPage;
                    this.showLeftSelectionDropUp = false;
                    this.showRightSelectionDropUp = false;

                    this.$nextTick(function () {
                        this.setPaginationRanges(selectedPage);
                        new Pin(PinType.Question);
                    });
                    this.pageIsLoading = false;
                },
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

        changeQuestionWishknowledgeState(questionId, isInWishknowledge) {
            for (var q in this.questions) {
                if (this.questions[q].Id == questionId) {
                    this.questions[q].IsInWishknowledge = isInWishknowledge;
                    break;
                }
            }
        },

        getUpdatedCorrectnessProbability(id) {
            $.ajax({
                url: "/QuestionList/GetUpdatedCorrectnessProbability/",
                data: { questionId: id },
                type: "Post",
                success: correctnessProbability => {
                    for (var q in this.questions) {
                        if (this.questions[q].Id == id) {
                            this.questions[q].CorrectnessProbability = correctnessProbability;
                            break;
                        }
                    }
                }
            });
        },
    },
});

Vue.component('question-component', {
    props: [
        'questionId',
        'questionTitle',
        'questionImage',
        'knowledgeState',
        'isInWishknowledge',
        'url',
        'hasPersonalAnswer',
        'isAdmin',
        'selectedPage',
        'isQuestionListToShow'],
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
            correctnessProbabilityLabel: "",
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
            pageHasChanged: false,
            answerCount: "0",
            correctAnswers: "0",
            wrongAnswers: "0",
          
    }   
        },
    mounted() {
        this.correctnessProbability = this.knowledgeState + "%";
        this.setKnowledgebarData(this.knowledgeState);
        this.getWishknowledgePinButton();
    
        eventBus.$on('reload-question-details', () => {
            if (this.showFullQuestion)
                this.setQuestionDetails();
        });
    
    },
    watch: {
        isQuestionListToShow() {
            this.expandQuestion();
        },
        knowledgeState(val) {
            this.setKnowledgebarData(val);
            this.correctnessProbability = this.knowledgeState + "%";
            this.loadQuestionBody();
        },
        selectedPage() {
            this.showFullQuestion = false;
                if (this.isInWishknowledge) {
                    $("#" + this.pinId + " .iAddedNot").addClass("hide2");
                    $("#" + this.pinId + " .iAdded").removeClass("hide2");
                } else {
                    $("#" + this.pinId + " .iAdded").addClass("hide2");
                    $("#" + this.pinId + " .iAddedNot").removeClass("hide2");
                }
        },
        isInWishknowledge() {
            this.setKnowledgebarData(this.knowledgeState);
        },
        categories() {
            if (this.categories.length >= 2)
                this.topicTitle = "Themen";
            else
                this.topicTitle = "Thema";
        },
        questionId() {
            this.allDataLoaded = false;
            this.pinId = "QuestionListPin-" + this.questionId;
            this.questionTitleId = "#QuestionTitle-" + this.questionId;
            this.questionDetailsId = "QuestionDetails-" + this.questionId;
        }
    },
    methods: {
        abbreviateNumber(val) {
            var newVal;
            if (val < 1000000) {
                return val.toLocaleString("de-DE");
            }
            else if (val >= 1000000 && val < 1000000000) {
                newVal = val / 1000000;
                return newVal.toFixed(2).toLocaleString("de-DE") + " Mio.";
            }
        },
        setKnowledgebarData(val) {
            if (this.isInWishknowledge) {
                if (this.hasPersonalAnswer) {
                    if (val >= 80) {
                        this.backgroundColor = "solid";
                        this.correctnessProbabilityLabel = "Sicheres Wissen";
                    } else if (val < 80 && val >= 50) {
                        this.backgroundColor = "shouldConsolidate";
                        this.correctnessProbabilityLabel = "Zu festigen";
                    } else if (val < 50 && val >= 0) {
                        this.backgroundColor = "shouldLearn";
                        this.correctnessProbabilityLabel = "Zu lernen";
                    }
                } else {
                    this.backgroundColor = "inWishknowledge";
                    this.correctnessProbabilityLabel = "Nicht gelernt";
                }
            } else {
                this.backgroundColor = "";
                this.correctnessProbabilityLabel = "Nicht im Wunschwissen";
            }
    
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
                    this.extendedQuestion = data.extendedQuestion;
                    this.commentCount = data.commentCount;
                    this.isCreator = data.isCreator && this.isLoggedIn;
                    this.editUrl = data.editUrl;
                    this.historyUrl = data.historyUrl;
                    this.authorUrl = data.authorUrl;
                    this.$nextTick(function () {
                        Images.Init();
                    });
                    this.allDataLoaded = true;
                    this.answerCount = this.abbreviateNumber(data.answerCount);
                    this.correctAnswers = this.abbreviateNumber(data.correctAnswerCount);
                    this.wrongAnswers = this.abbreviateNumber(data.wrongAnswerCount);
                },
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
    },
});
