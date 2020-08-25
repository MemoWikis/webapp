﻿declare var Vue: any;
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
        selectedPageFromParent: 1,
        allQuestionsCount: 0
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
                    this.allQuestionsCount = result;

                }
            });
        }
    },
    created: function() {
        eventBus.$on("change-active-question", (index) => { this.changeActiveQuestion(index) });
        this.questionsCount = this.getAllQuestionsCountFromCategory() ; 
    },
    watch: {
        activeQuestion: function (val) {
            let questionsPerPage = 25 - 1; // question 25 is page 2 question 0  then 0 -24 = 25 questions
            let selectedPage = Math.floor(val / (questionsPerPage)); 
            if (val > questionsPerPage) {
                this.activeQuestion = 0 + (val % (questionsPerPage + 1 ));
                this.selectedPageFromParent = selectedPage + 1;      //question 25 is page 2 
            }
        },
    }
});