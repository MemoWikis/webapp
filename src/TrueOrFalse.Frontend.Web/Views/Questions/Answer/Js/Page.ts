/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />

$(() => {
    var answerEntry = new AnswerEntry();
    answerEntry.Init();

    var pinQuestion = new PinQuestion();
    pinQuestion.Init();

    $('#hddTimeRecords').attr('data-time-on-load', $.now());
    
    $(window).unload( function () {
        AnswerQuestion.LogTimeForQuestionView();
    });

    new QuestionRowDelete(QuestionRowDeleteSourcePage.QuestionDetail);
    new ShareQuestion();

    $('[data-toggle=popover]').popover({ html: true }).click(e => { e.preventDefault(); });
});