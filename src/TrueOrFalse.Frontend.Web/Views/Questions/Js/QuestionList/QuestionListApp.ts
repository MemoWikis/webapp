declare var Vue: any;
declare var VueAdsPagination: any;
declare var eventBus: any;

if (eventBus == null)
    var eventBus = new Vue();

let questionListApp = new Vue({
    el: '#QuestionListApp',
    data: {
        isQuestionListToShow: false,
        answerBody: new AnswerBody(),
        activeQuestion: 0, // which question is active
        learningSessionData: "",
        selectedPageFromActiveQuestion: 1,
        activeQuestionId: 0 as Number,
        hasNoQuestions: true,
        commentIsLoaded: false,
        commentQuestionId: 0,
        stepCount: 0,
        currentQuestionCount: 0,
        repetitionCount: 0,
        allQuestionCount: 0,
    },

    created() {
        eventBus.$on("change-active-question", () => this.setActiveQuestionId());
        eventBus.$on("change-active-page", (index) => { this.selectedPageFromActiveQuestion = index });
        eventBus.$on("sync-session-config", () => {
            this.getCurrentLearningSessionData();
        });
        eventBus.$on('update-selected-page', (selectedPage) => {
            this.$nextTick(() => this.selectedPageFromActiveQuestion = selectedPage);
        });
        eventBus.$on('init-new-session', () => {
            this.$nextTick(() => this.getCurrentLearningSessionData());
            this.$nextTick(() => this.selectedPageFromActiveQuestion = 1);
        });

        eventBus.$on('show-comment-section-modal', (questionId) => {
            this.showCommentModal(questionId);
        });
        eventBus.$on('closeModal', () => {
            this.hideCommentModal();
        });

    },

    mounted() {
        this.setActiveQuestionId();

        this.$nextTick(() => {
            $('[data-toggle="tooltip"]').tooltip();
        });
        eventBus.$on('unload-comment', () => { this.commentIsLoaded = false });
        eventBus.$on('update-question-count', () => { this.getCurrentLearningSessionData() });
    },

    watch: {
        activeQuestion: function (indexQuestion) {
            let questionsPerPage = 25 - 1; // question 25 is page 2 question 0  then 0 -24 = 25 questions
            let selectedPage = Math.floor(indexQuestion / (questionsPerPage));
            if (indexQuestion > questionsPerPage) {
                this.activeQuestion = 0 + (indexQuestion % (questionsPerPage + 1));
                this.selectedPageFromActiveQuestion = selectedPage + 1;      //question 25 is page 2 
            }
        },
        stepCount(val) {
            this.hasNoQuestions = val < 1;
            if (!this.hasNoQuestions)
                eventBus.$emit('category-has-question');
        }
    },

    methods: {
        toggleQuestionsList() {
            this.isQuestionListToShow = !this.isQuestionListToShow;
        },
        startNewLearningSession: () => {
            eventBus.$emit("start-learning-session");
        },
        changeActiveQuestion: function (index) {
            this.activeQuestion = index;
        },
        getCurrentLearningSessionData() {
            $.ajax({
                url: "/QuestionList/GetCurrentLearningSessionData/",
                data: {
                    categoryId: $("#hhdCategoryId").val()
                },
                type: "POST",
                success: result => {
                    this.allQuestionCount = result.allQuestionCount;
                    this.stepCount = result.stepCount;
                    this.currentQuestionCount = result.currentQuestionCount;
                    this.repetitionCount = this.stepCount - this.currentQuestionCount;
                }
            });
        },
        setActiveQuestionId: function () {
            this.activeQuestionId = parseInt($('input#hddQuestionId').attr('value'));
        },
        showCommentModal(questionId) {
            this.commentQuestionId = questionId;
            this.commentIsLoaded = true;
            eventBus.$emit("load-comment-section-modal", questionId);
        },
        hideCommentModal() {
            this.commentIsLoaded = false;
        }
    },
});

