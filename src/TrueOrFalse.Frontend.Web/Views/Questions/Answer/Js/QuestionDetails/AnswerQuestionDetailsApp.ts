﻿declare var Vue: any;

declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

var qApp = new Vue({
    el: '#QuestionDetailsApp',
    mounted() {
        eventBus.$on('destroy-answer-question-details', () => this.$destroy());
    },
});