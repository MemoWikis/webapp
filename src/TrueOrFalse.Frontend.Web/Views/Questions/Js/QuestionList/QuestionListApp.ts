declare var Vue: any;

declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

var v = new Vue({
    el: '#QuestionListApp',
    data: {
        isQuestionListToShow: false
    },
    methods: {
        toggleQuestionsList: function() {
            this.isQuestionListToShow = !this.isQuestionListToShow;
        }
    }
});