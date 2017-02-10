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
        new SetVideo();
    }

    new Pin(PinRowType.SetDetail);
    new Pin(PinRowType.Question);
});