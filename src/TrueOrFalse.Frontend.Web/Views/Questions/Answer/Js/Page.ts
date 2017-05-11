/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />

$(() => {

    var questionId = $("#hddQuestionId").val();

    var answerEntry = new AnswerEntry();
    answerEntry.Init();

    new Pin(PinType.Question);
    new Pin(PinType.Set); //only needed if Set-Cards are presented as content

    new AsyncLoading();

    $('#hddTimeRecords').attr('data-time-on-load', $.now());
    
    $(window).unload( function () {
        AnswerQuestion.LogTimeForQuestionView();
    });

    new QuestionRowDelete(QuestionRowDeleteSourcePage.QuestionDetail);
    new ShareQuestion(questionId);

    $('[data-toggle=popover]').popover({ html: true }).click(e => { e.preventDefault(); });

    //set focus to first possible answer element
    if (!Utils.IsInWidget()) {

        if (document.getElementsByName("answer").length > 0)
            $("[name=answer]")[0].focus();

        $("#txtAnswer:visible").focus();

        $("#row-1:visible").focus();
        window.scrollTo(0, 0);
    }
});