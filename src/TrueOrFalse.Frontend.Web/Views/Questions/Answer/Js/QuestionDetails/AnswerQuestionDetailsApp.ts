declare var Vue: any;

declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

new Vue({
    name: 'QuestionDetails',
    el: '#QuestionDetailsApp',
    mounted() {
        eventBus.$on('destroy-answer-question-details', () => this.$destroy());
    },
});