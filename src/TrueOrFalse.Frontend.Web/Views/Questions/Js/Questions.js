$(function () {
    $('#btnExport').tooltip();
});

/************************/
/******* Charts *********/

$(function () {
    $(".pieTotals").each(function () {
        var me = $(this);
        var values = $(this).attr("data-percentage").split('-');
        me.sparkline([values[0], values[1]], {
            type: 'pie',
            sliceColors: ['#90EE90', '#FFA07A']
        });
    });

    $(".piePersonalRelevanceTotal").each(function () {
        
        var value = parseFloat($(this).attr("data-avg"));
        var height = (value * parseFloat(0.9)) + 5;

        $(this).sparkline([Math.round(value)], {
            type: 'pie',
            sliceColors: ['#ABC0FF'],
            height: Math.round(height),
            disableTooltips: 'true'
        });
    });    
});

/************************/
$(function () {
    $('#tabInfoMyKnowledge').click(function () {
        $("#modalTabInfoMyKnowledge").modal('show');
    });
});