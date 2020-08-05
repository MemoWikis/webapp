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
        activeQuestion: 0,
        learningSessionData: "",
        selectedPageFromParent: 1
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
        }
    },
    watch: {
        activeQuestion: function (val) {
            let questionsPerPage = 25;
            let selectedPage = Math.floor(val / (questionsPerPage - 1)); 
            if (val > questionsPerPage) {
                this.activeQuestion = 0 + (val % (questionsPerPage - 1)); // question 25 is page 2 question 0  then 0 -24 = 25 questions
                this.selectedPageFromParent = selectedPage + 1;      //question 25 is page 2 
            }
        },
    }
});