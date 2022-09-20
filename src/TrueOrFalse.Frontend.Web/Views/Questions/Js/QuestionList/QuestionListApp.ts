declare var Vue: any;
declare var VueAdsPagination: any;
declare var eventBus: any;

if (eventBus == null)
    var eventBus = new Vue();

let questionListApp = new Vue({
    name: 'QuestionList',
    el: '#QuestionListApp',
    data: {
        expandQuestions: false,
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
        categoryHasNoQuestions: false,
        showFilter: true,
        filterError: false,
        isTestMode: false,
        showError: false,
        tiptapIsReady: false,
    },

    created() {
        this.categoryHasNoQuestions = $('#SessionConfigQuestionChecker').data('category-has-no-questions') == 'False';
        if (this.categoryHasNoQuestions)
            this.showFilter = false;

        eventBus.$on("change-active-question", () => this.setActiveQuestionId());
        eventBus.$on("change-active-page", (index) => { this.selectedPageFromActiveQuestion = index });
        eventBus.$on("sync-session-config", (isTestMode) => {
            this.isTestMode = isTestMode;
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
        eventBus.$on('tiptap-is-ready', (val) => {
            this.tiptapIsReady = val;
        });
        this.setActiveQuestionId();

        this.$nextTick(() => {
            $('[data-toggle="tooltip"]').tooltip();
        });
        eventBus.$on('unload-comment', () => { this.commentIsLoaded = false });
        eventBus.$on('update-question-count', () => { this.getCurrentLearningSessionData() });

        eventBus.$on('set-session-progress',
            (e) => {
                if (e == null)
                    this.filterError = true;
                else if (e.isResult)
                    this.showFilter = false;
                else
                    this.filterError = false;
            });

        eventBus.$on('init-new-session',
            () => {
                this.showFilter = true;
            });

        eventBus.$on('category-has-question',
            () => {
                this.categoryHasNoQuestions = false;
                this.showFilter = true;
            });
    },

    watch: {
        activeQuestion(indexQuestion) {
            let questionsPerPage = 25 - 1; // question 25 is page 2 question 0  then 0 -24 = 25 questions
            let selectedPage = Math.floor(indexQuestion / (questionsPerPage));
            if (indexQuestion > questionsPerPage) {
                this.activeQuestion = 0 + (indexQuestion % (questionsPerPage + 1));
                this.selectedPageFromActiveQuestion = selectedPage + 1;      //question 25 is page 2 
            }
        },
        stepCount(val) {
            this.hasNoQuestions = val < 1;
            if (!this.hasNoQuestions && !isNaN(val))
                eventBus.$emit('category-has-question');
        }
    },

    methods: {
        expandAllQuestions() {
                this.expandQuestions = !this.expandQuestions;
        },
        startNewLearningSession() {
            eventBus.$emit("start-learning-session");
        },
        changeActiveQuestion(index) {
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
        setActiveQuestionId() {
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

