declare var Vue: any;
declare var eventBus: any;

if (eventBus == null)
    var eventBus = new Vue();

var v = new Vue({
    el: '#QuestionListApp',
    data: {
        isQuestionListToShow: false,
        answerBody: new AnswerBody(),
        questionsCount: 30,
        activeQuestion: 3,
        learningSessionData: "",
        currentStep: 0
    },
    mounted() {
        this.doPoll();
    },
    methods: {
        
        toggleQuestionsList: function() {
            this.isQuestionListToShow = !this.isQuestionListToShow;
        },
        updateQuestionsCount: function(val) {
            this.questionsCount = val;
        },
        doPoll: function () {
            var self = this; 
            $.post('/QuestionList/GetLearningSession', function (data) {
                if (data === "") {
                    console.log(data);
                    setTimeout( self.doPoll(), 500 );
                } else {
                    console.log(data);
                }
                

            });
        }
    }
});