declare var Vue: any;
declare var eventBus: any;

if (eventBus == null)
    var eventBus = new Vue();

var questionListApp = new Vue({
    el: '#QuestionListApp',
    data: {
        isQuestionListToShow: false,
        answerBody: new AnswerBody(),
        questionsCount: 30,
        activeQuestion: 0,
        learningSessionData: ""
    },
    methods: {
        toggleQuestionsList: function() {
            this.isQuestionListToShow = !this.isQuestionListToShow;
        },
        updateQuestionsCount: function(val) {
            this.questionsCount = val;
        },
    }
});