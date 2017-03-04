class WidgetQuestion {
    constructor() {
        var answerEntry = new AnswerEntry();
        answerEntry.Init();

        new Pin(PinType.Question);
    }
}

$(() => {
    new WidgetQuestion();
}); 
