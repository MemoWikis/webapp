$(() => {
    $(".pieTotals").each(function() {
        var me = $(this);
        var values = $(this).attr("data-percentage").split('-');
        me.sparkline([values[0], values[1]], {
            type: 'pie',
            sliceColors: ['#90EE90', '#FFA07A']
        });
    });

    if ($("#hhdHasVideo").val() == "True") {
        var answerEntry = new AnswerEntry();
        answerEntry.Init();

        $('#hddTimeRecords').attr('data-time-on-load', $.now());        

        var pinQuestion = new PinQuestion();
        pinQuestion.Init();
    }

    new Pin(PinRowType.SetDetail);
    new Pin(PinRowType.Question);
});