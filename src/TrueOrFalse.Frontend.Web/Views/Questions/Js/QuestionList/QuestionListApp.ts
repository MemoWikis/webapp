declare var Vue: any;
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
        allQuestionsCountFromCategory: 0
    },
    methods: {
        toggleQuestionsList: function() {
            this.isQuestionListToShow = !this.isQuestionListToShow;
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
        }
    },
    created: function() {
        eventBus.$on("change-active-question", (index) => { this.changeActiveQuestion(index) });
        eventBus.$on("change-active-page", (index) => { this.selectedPageFromActiveQuestion = index });
        this.questionsCount = this.getAllQuestionsCountFromCategory() ; 
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
    }
});

