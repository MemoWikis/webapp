declare var Vue: any;
declare var eventBus: any;

if (eventBus == null)
    var eventBus = new Vue();

var v = new Vue({
    el: '#QuestionListApp',
    data: {
        isQuestionListToShow: false,
        answerBody: new AnswerBody(),
        questionOrder: ""
        
    },
    methods: {
        toggleQuestionsList: function() {
            this.isQuestionListToShow = !this.isQuestionListToShow;
        },
        loadCustomSession() {
            this.answerBody.Loader.loadNewSession(null, true);
        },
        sendQuestionOrder(val) {
            this.questionOrder = val;
        },


    }
});