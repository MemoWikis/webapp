class WidgetQuestion {
    constructor() {
        var answerEntry = new AnswerEntry();
        answerEntry.Init();
        var pinQuestion = new PinQuestion();        
        pinQuestion.Init();
    }
}

$(() => {
    new WidgetQuestion();
}); 
