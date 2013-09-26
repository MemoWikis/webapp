/************************/
/******* FILTER *********/
$(function () {
    $("#addFilterUserId").val("");
    $("#delFilterUserId").val("");
});

$(function () {

    $('#btnSelectionToSet').tooltip();
    $('#btnSelectionDelete').tooltip();

    $('.btn-filterByMe').click(function () {
        $(this).toggleClass('active');
        $('#FilterByMe').val($(this).hasClass('active'));
    });
    if ($('#FilterByMe').val().toLowerCase() == "true") {
        $('.btn-filterByMe').addClass('active');
    }

    $('.btn-filterByAll').click(function () {
        $(this).toggleClass('active');
        $('#FilterByAll').val($(this).hasClass('active'));
    });
    if ($('#FilterByAll').val().toLowerCase() == "true") {
        $('.btn-filterByAll').addClass('active');
    }

    $("#toggleExtendSearch").click(function() {
        $("#extendedSearch").toggle();
    });

    $('.userPopover').popover({ content: getPopupOverContent });
    function getPopupOverContent() {
        return "123, <b>456</b>";  
    }

    function SubmitSearch() {
        window.location = "/Fragen/Suche/" + $('#txtSearch').val();
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
            sliceColors: ['#1BE022', 'red']
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