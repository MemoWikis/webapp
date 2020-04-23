declare var Vue: any;

declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

new Vue({
    el: '#QuestionDetailsApp',
});