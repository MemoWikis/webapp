$(function () {
    $(".pieTotals").each(function () {
        var me = $(this);
        var values = $(this).attr("data-percentage").split('-');
        me.sparkline([values[0], values[1]], {
            type: 'pie',
            sliceColors: ['#3e7700', '#B13A48']
        });
    });
});
//# sourceMappingURL=Set.js.map
