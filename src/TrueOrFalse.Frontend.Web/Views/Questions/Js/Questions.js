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
                .replace("Ersteller:\"", "Ersteller__")
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
});

/************************/
$(function () {
    $('#tabInfoMyKnowledge').click(function () {
        $("#modalTabInfoMyKnowledge").modal('show');
    });
});