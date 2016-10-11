/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />

$(() => {
    var answerEntry = new AnswerEntry();
    answerEntry.Init();

    var pinQuestion = new PinQuestion();
    pinQuestion.Init();

    function loadFacebook(d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/de_DE/all.js#xfbml=1&appId=128827270569993";
        fjs.parentNode.insertBefore(js, fjs);
    } loadFacebook(window.document, 'script', 'facebook-jssdk');

    $('#hddTimeRecords').attr('data-time-on-load', $.now());
    
    $(window).unload( function () {
        AnswerQuestion.LogTimeForQuestionView();
    });

    new QuestionRowDelete(QuestionRowDeleteSourcePage.QuestionDetail);
    new ShareQuestion();

    $('[data-toggle=popover]').popover({ html: true });
});