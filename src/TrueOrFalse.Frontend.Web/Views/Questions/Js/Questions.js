$(function () {

    $('#btnSelectionToSet').tooltip();
    $('#btnSelectionDelete').tooltip();
    $('#btnExport').tooltip();

    $('.userPopover').popover({ content: getPopupOverContent });
    function getPopupOverContent() {
        return "123, <b>456</b>";  
    }

    function SubmitSearch() {
        window.location.href = $('#txtSearch').attr("formUrl") + 
            $('#txtSearch').val()
                .replace("Kat:\"", "Kat__")
                .replace("kat:\"", "Kat__")
                .replace("\"", "__")
                .replace("'", "__")
                .replace(":", "___")
                .replace("&", "_and_");
    }

    $('#btnSearch').click(function () { SubmitSearch(); });
    
    $(function () {
        $("#txtSearch").keypress(function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code === 13) {
                SubmitSearch();
                e.preventDefault();
            }
        });
    });
});

/************************/
/******* Charts *********/

$(function () {
    $(".pieTotals").each(function () {
        var me = $(this);
        var values = $(this).attr("data-percentage").split('-');
        me.sparkline([values[0], values[1]], {
            type: 'pie',
            sliceColors: ['#3e7700', '#B13A48']
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

    $(".tristateHistory").sparkline([1, 1, 0, 1, -1, -1, 1, -1, 0], {
        type: 'tristate',
        barWidth: 2,
        posBarColor: '#05c105',
        negBarColor: '#ce1212',
        tooltipValueLookups: { map: { '-1': 'Falsch', '0': 'Versuch und Antwort gezeigt', '1': 'Richtig'} }
    });
    
    $('.viewsHistory').sparkline('html', { fillColor: false });
    $('.viewsHistory').sparkline([4, 1, 5, 7, 9, 9, 8, 7, 6, 6, 4, 7, 8, 4, 3, 2, 2, 5, 6, 7],
        { composite: true, fillColor: false, lineColor: 'red' });
});

/************************/
$(function () {
    $('#tabInfoMyKnowledge').click(function () {
        $("#modalTabInfoMyKnowledge").modal('show');
    });
});