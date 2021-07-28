declare var Vue: any;
declare var VueAdsPagination: any;
declare var eventBus: any;

if (eventBus == null)
    var eventBus = new Vue();

var questionListApp = new Vue({
    el: '#QuestionListApp',
    data: {
        isQuestionListToShow: false,
        answerBody: new AnswerBody(),
        questionsCount: 10,       
        activeQuestion: 0,      // which question is active
        learningSessionData: "",
        selectedPageFromActiveQuestion: 1,
        allQuestionsCountFromCategory: 0,
        selectedQuestionCount: "alle",
        activeQuestionId: 0 as Number,
        hasNoQuestions: true,
    },
    methods: {
        toggleQuestionsList() {
            this.isQuestionListToShow = !this.isQuestionListToShow;
        },
        startNewLearningSession: () => {
            eventBus.$emit("start-learning-session"); 
        },
        updateQuestionsCount: function(val) {
            this.questionsCount = val;
        },
        changeActiveQuestion: function (index) {
            this.activeQuestion = index;
        }, 
        getAllQuestionsCountFromCategory() {
            $.ajax({
                url: "/AnswerQuestion/GetQuestionCount/",
                data: {
                    config: null,
                    categoryId: $("#hhdCategoryId").val()
                },
                type: "POST",
                success: result => {
                    result = parseInt(result);
                    this.questionsCount = result;
                    this.allQuestionsCountFromCategory = result;

                }
            });
        },
        setActiveQuestionId: function () {
            this.activeQuestionId = parseInt($('input#hddQuestionId').attr('value'));
        }
    },
    created: function() {
        eventBus.$on("change-active-question", () => this.setActiveQuestionId());
        eventBus.$on("change-active-page", (index) => { this.selectedPageFromActiveQuestion = index });
        this.questionsCount = this.getAllQuestionsCountFromCategory();
        eventBus.$on("send-selected-questions", (numberOfQuestions) => {
            numberOfQuestions !== this.questionsCount
                ? this.selectedQuestionCount = numberOfQuestions
                : this.selectedQuestionCount = "alle";
        });
        eventBus.$on('update-selected-page', (selectedPage) => {
            this.selectedPageFromActiveQuestion = selectedPage;
        });
        eventBus.$on('add-question-to-list',
            () => {
                this.getAllQuestionsCountFromCategory();
            });
        eventBus.$on('init-new-session', () => {
            this.$nextTick(() => this.selectedQuestionCount = 'alle');
        });
    },
    mounted() {
        $('#CustomSessionConfigBtn').tooltip();

        $("#LearningSessionReminderQuestionList>.fa-times-circle").on('click',
            () => {
                $.post("/Category/SetSettingsCookie?name=SessionConfigurationMessageList");
                $("#LearningSessionReminderQuestionList").hide(200);
            });
        this.setActiveQuestionId();
    },
    watch: {
        activeQuestion: function (indexQuestion) {
            let questionsPerPage = 25 - 1; // question 25 is page 2 question 0  then 0 -24 = 25 questions
            let selectedPage = Math.floor(indexQuestion / (questionsPerPage)); 
            if (indexQuestion > questionsPerPage) {
                this.activeQuestion = 0 + (indexQuestion % (questionsPerPage + 1 ));
                this.selectedPageFromActiveQuestion = selectedPage + 1;      //question 25 is page 2 
            }
        },
        questionsCount(val) {
            if (val < 1)
                this.hasNoQuestions = true;
            else
                this.hasNoQuestions = false;
        }
    }
});

