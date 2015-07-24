$(function () {
    $(".pieTotals").each(function () {
        var me = $(this);
        var values = $(this).attr("data-percentage").split('-');
        me.sparkline([values[0], values[1]], {
            type: 'pie',
            sliceColors: ['#3e7700', '#B13A48']
        });
    });

    $("[data-btn=startLearningSession]").click(function (e) {
        if (NotLoggedIn.Yes()) {
            e.preventDefault();
            NotLoggedIn.ShowErrorMsg();
        }
    });

    new Pin(2 /* SetDetail */);
});
//# sourceMappingURL=Set.js.map
